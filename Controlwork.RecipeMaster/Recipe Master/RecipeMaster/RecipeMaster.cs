using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BendSheets.ConfigurationManagement;
using RecipeMaster.DataMapping;
using BendSheets.Properties;
using BendSheets.PVICommunication;
using RecipeMaster.Services;
using WH.ComUtils.ExcelManager;
using WH.Utils.Logging;
using BR.AN.PviServices;
using System.Reflection;

[assembly: SecurityPermission(SecurityAction.RequestMinimum)]

namespace BendSheets
{
    public partial class BendSheet : Form
    {
        #region Private Variables
        private readonly string settingsFile = @"Templates\MachineSettings.xml";
        private readonly double OPACITY_INCREMENT = .10;
        private AboutForm aboutForm;
        private BindingList<Machine> m_ActiveMachineList = new BindingList<Machine>();
        private Dictionary<Int32, Machine> m_MachineDictionary = new Dictionary<int, Machine>();
        private CpuManager m_CpuManager;

        private MachineCollection m_MachineCollection;

        #endregion

        #region Constructor
        public BendSheet()
        {
            InitializeComponent();
            this.Disposed += new EventHandler(BendSheet_Disposed);
        }
        #endregion

        #region Load Methods
        private void BendSheet_Load(object sender, EventArgs e)
        {
            m_CpuManager = new CpuManager();

            this.Opacity = 0;
            LoadForm();
            timer1.Tick += new EventHandler(timer1_Tick);
            this.timer1.Enabled = true;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Opacity < 1)
            {
                this.Opacity += OPACITY_INCREMENT;
            }
            else
            {
                this.timer1.Enabled = false;
            }
        }

        private void LoadForm()
        {
            if (!ExcelEngine.ExcelInstalled())
            {
                MessageBox.Show(
                    "Microsoft Excel must be installed before this application will function!",
                    "Excel Not Found", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                SetStatusText();

                return;
            }

            CreateActiveMachineList();
            SetGrid();

            SetConnectionIcon(false);
            LoadPVIEngine();
            SetStatusText();

            List<int> station = new List<int>();
            station.Add(1);

            this.notifyIcon1.Text = "Recipe Master Processor";
        }

        private void CreateActiveMachineList()
        {
            LoadSettings();
            //m_MachineCollection = MachineCollection.LoadSettings(Path.Combine(Directory.GetCurrentDirectory(), settingsFile));
            m_MachineCollection.MachinePropertyChanged += new EventHandler<PropertyChangedEventArgs>(m_MachineCollection_MachinePropertyChanged);
            m_ActiveMachineList = new BindingList<Machine>();
            foreach (Machine machine in m_MachineCollection.MachineList)
            {
                if (machine.Active)
                {
                    this.m_ActiveMachineList.Add(machine);
                }
            }
        }

        private void LoadSettings()
        {
            try
            {
                Log.Write(LogLevel.DEBUG, "Loading user preferences from " + SettingsPath());
                m_MachineCollection = MachineCollection.LoadSettings(SettingsPath());
            }
            catch (System.Exception ex)
            {
                Log.Write(ex);
            }

        }

        private void SaveSettings()
        {
            try
            {
                Log.Write(LogLevel.DEBUG, "Saving user preferences to " + SettingsPath());
                m_MachineCollection.Save(SettingsPath());
            }
            catch (System.Exception ex)
            {
                Log.Write(ex);
            }
        }

        private string SettingsPath()
        {
            return Path.Combine(Application.StartupPath, settingsFile);
        }

        private void m_MachineCollection_MachinePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Machine machine = sender as Machine;
            if (machine != null)
            {
                if (e.PropertyName == "Active")
                {
                    if (machine.Active)
                    {
                        m_ActiveMachineList.Add(machine);
                    }
                    else
                    {
                        m_ActiveMachineList.Remove(machine);
                    }
                }
            }
        }

        private void SetGrid()
        {
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.Columns["colMachineName"].DataPropertyName = "MachineName";
            this.dataGridView1.Columns["colDestination"].DataPropertyName = "Destination";
            this.dataGridView1.Columns["colStatus"].DataPropertyName = "Status";
            this.dataGridView1.Columns["colFileName"].DataPropertyName = "FileName";

            this.dataGridView1.DataSource = m_ActiveMachineList;
        }

        private void LoadConnectedIcon()
        {
            this.Icon = Properties.Resources.Connected;
            this.notifyIcon1.Icon = Properties.Resources.Connected;
            this.notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
        }

        private void LoadDisconnectedIcon()
        {
            this.Icon = Properties.Resources.Disconnected;
            this.notifyIcon1.Icon = Properties.Resources.Disconnected;
            this.notifyIcon1.BalloonTipIcon = ToolTipIcon.Error;
        }

        private void LoadPVIEngine()
        {
            PviService service = PviService.PviServiceInstance;
            service.ServiceError += new EventHandler<PviEventArgs>(service_ServiceError);
        }

        private void service_ServiceError(object sender, PviEventArgs e)
        {
            string text = "PVI network error -- connection closed";
            string caption = "PVI Error";
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Log.Write(LogLevel.FATAL, e.ToString());
        }

        private bool m_ShouldClose;
        private void Exit()
        {
            m_ShouldClose = true;

            //m_MachineCollection = MachineCollection.LoadSettings(Path.Combine(Directory.GetCurrentDirectory(), settingsFile));


            PviService service = PviService.PviServiceInstance;
            service.DisconnectPVIService();
            this.Close();
            Application.Exit();
        }

        #endregion

        #region Event Handlers
        private void BendSheet_Disposed(object sender, EventArgs e)
        {
            this.notifyIcon1.Dispose();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.Focus();
            this.TopMost = false;
        }

        private void BendSheet_Resize(object sender, EventArgs e)
        {
            this.ShowInTaskbar = !(this.WindowState == FormWindowState.Minimized);
        }

        #endregion

        private void HandleException(System.Exception ex)
        {
            string message = String.Concat("An error opening the selected file has occurred.\n\n", ex.Message);
            MessageBox.Show(message, "Error Processing File", MessageBoxButtons.OK, MessageBoxIcon.Error);

            ex.Data.Add("fileName", "");

            LogException(ex);
        }

        private static void LogException(System.Exception ex)
        {
            Thread t = new Thread(new ParameterizedThreadStart(Log.Write));
            t.Start(ex);
            t.Join();

        }
        private void SetStatusText()
        {
            if (!ExcelEngine.ExcelInstalled())
            {
                this.statusStrip1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.toolStripStatusLabel1.Text = "Excel Not Installed!";
            }
            else
            {
                PviService service = PviService.PviServiceInstance;
                string status = service.IsConnected ? "Connected" : "Disconnected";
                this.toolStripStatusLabel1.Text = String.Concat("PVI Manager Status: ", status);
                SetConnectionIcon(service.IsConnected);
            }
        }

        private void SetConnectionIcon(bool connected)
        {
            if (connected)
            {
                LoadConnectedIcon();
            }
            else
            {
                LoadDisconnectedIcon();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = !m_ShouldClose;
            this.WindowState = FormWindowState.Minimized;
            base.OnClosing(e);
        }

        #region ToolStrip Event Handlers

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (aboutForm == null)
            {
                aboutForm = new AboutForm();
            }
            aboutForm.ShowDialog();

        }
        private void pVINetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PVINetworkForm form = new PVINetworkForm();
            form.ShowDialog();
        }

        #endregion

        private void machineSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SettingsForm form = new SettingsForm(m_MachineCollection);
                form.ShowDialog();
            }
            catch (System.Exception ex)
            {
                Log.Write(ex);
            }
        }

        CpuForm cpuForm;

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dg = sender as DataGridView;
            Machine machine = dg.Rows[e.RowIndex].DataBoundItem as Machine;
            using (cpuForm = new CpuForm(machine, this.m_CpuManager))
            {
                cpuForm.ShowDialog();
            }
        }

        private void templatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmCreateTemplate templateForm = new frmCreateTemplate();
                templateForm.ShowDialog(this);
            }
            catch (System.Exception ex)
            {
            }
        }

        private void BendSheet_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Exit();
        }
    }
}