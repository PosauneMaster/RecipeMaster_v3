using BR.AN.PviServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;

[assembly: SecurityPermission(SecurityAction.RequestMinimum)]

namespace ControlWorks.RecipeMaster
{
    public partial class frmMachines : Form
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
        public frmMachines()
        {
            InitializeComponent();
            this.Disposed += new EventHandler(frmMachines_Disposed);
        }
        #endregion

        #region Load Methods
        private void frmMachines_Load(object sender, EventArgs e)
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
            m_MachineCollection = MachineCollection.LoadSettings(SettingsPath());
        }

        private void SaveSettings()
        {
            m_MachineCollection.Save(SettingsPath());
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
            string message = String.Format("service_ServiceError; Action={0}, Address={1}, Error Code={2}, Error Text={3}, Name={4} ",
                e.Action, e.Address, e.ErrorCode, e.ErrorText, e.Name);
            Log.LogError(message);
            string text = "PVI network error -- connection closed";
            string caption = "PVI Error";
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool m_ShouldClose;
        private void Exit()
        {
            m_ShouldClose = true;

            PviService service = PviService.PviServiceInstance;
            service.DisconnectPVIService();
            this.Close();
            Application.Exit();
        }

        #endregion

        #region Event Handlers
        private void frmMachines_Disposed(object sender, EventArgs e)
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

        private void frmMachines_Resize(object sender, EventArgs e)
        {
            this.ShowInTaskbar = !(this.WindowState == FormWindowState.Minimized);
        }

        #endregion

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
                Log.LogError("", ex);
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

        private void frmMachines_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Exit();
        }
    }
}