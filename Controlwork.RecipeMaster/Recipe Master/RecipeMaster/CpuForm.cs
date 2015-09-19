using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BendSheets.PVICommunication;
using BR.AN.PviServices;
using RecipeMaster.Services;
using System.IO;
using WH.Utils.Logging;
using WH.ComUtils.ExcelManager;

namespace BendSheets
{
    public partial class CpuForm : Form
    {
        private object m_EventLocker = new object();
        private CpuManager m_CpuManager;

        public Machine CPUMachine { get; set; }

        public CpuForm()
        {
            InitializeComponent();
        }

        public CpuForm(Machine machine, CpuManager cpuManager)
        {
            InitializeComponent();
            CPUMachine = machine;
            m_CpuManager = cpuManager;
        }

        public void Connected(BR.AN.PviServices.PviEventArgs e)
        {
            CPUMachine.Status = "Connected";
            CPUMachine.IsConnected = true;
            SetConnectedState();
        }

        public void Disconnected()
        {
            CPUMachine.Status = "Disconnected";
            CPUMachine.IsConnected = false;

            SetDisconnectedState();
        }

        private void SetConnectedState()
        {
            this.rtbStatus.Text = "Connected";
            this.btnConnect.Text = "Disconnect";
            this.btnConnect.Enabled = true;
            this.btnViewFile.Enabled = false;
            this.btnSendData.Enabled = false;
            this.tsbProduction.Enabled = true;
            this.tsbMaster.Enabled = true;
        }

        private void SetDisconnectedState()
        {
            this.rtbStatus.Text = "Disconnected";
            this.btnConnect.Text = "Connect";

            this.btnConnect.Enabled = true;
            this.btnViewFile.Enabled = false;
            this.btnSendData.Enabled = false;
            this.tsbProduction.Enabled = false;
            this.tsbMaster.Enabled = false;
            this.btnViewExcel.Enabled = false;
        }

        private void CpuForm_Load(object sender, EventArgs e)
        {
            m_CpuManager.CpuConnected += new EventHandler<BR.AN.PviServices.PviEventArgs>(m_CpuManager_CpuConnected);
            m_CpuManager.CpuDisconnected += new EventHandler<BR.AN.PviServices.PviEventArgs>(m_CpuManager_CpuDisconnected);
            m_CpuManager.CpuError += new EventHandler<BR.AN.PviServices.PviEventArgs>(m_CpuManager_CpuError);
            if (CPUMachine != null)
            {
                if (CPUMachine.IsConnected)
                {
                    SetConnectedState();
                }
                else
                {
                    SetDisconnectedState();
                }

                this.rtbMasterFile.Text = CPUMachine.MasterFile;
                this.rtbProductionFile.Text = CPUMachine.ProductionFile;
                this.rtbMachineName.Text = CPUMachine.MachineName;
                this.rtbStatus.Text = CPUMachine.Status;
                this.rtbDestination.Text = CPUMachine.Destination.ToString("N0");
                this.rtbTemplate.Text = CPUMachine.Template;
            }
        }

        void m_CpuManager_CpuError(object sender, BR.AN.PviServices.PviEventArgs e)
        {
            CPUMachine.SendInactive();
        }

        void m_CpuManager_CpuDisconnected(object sender, BR.AN.PviServices.PviEventArgs e)
        {
            this.btnConnect.Text = "Connect";
            this.tsbMaster.Enabled = false;
            this.tsbProduction.Enabled = false;
            this.rtbStatus.Text = "Disconnected";
            CPUMachine.SendInactive();
        }

        void m_CpuManager_CpuConnected(object sender, BR.AN.PviServices.PviEventArgs e)
        {
            Cpu cpu = PviService.PviServiceInstance.Service.Cpus[CpuName()];
            CPUMachine.SetCpu(cpu);
            this.Connected(e);
        }

        private void btnViewFile_Click(object sender, EventArgs e)
        {
            using (frmVariables variables = new frmVariables(CPUMachine))
            {
                variables.ShowDialog(this);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                Button b = sender as Button;
                if (b.Text == "Connect")
                {
                    m_CpuManager.ConnectCpu(CpuName(), CPUMachine.Destination);
                }
                else
                {
                    CPUMachine.SendInactive();
                    Cpu cpu = PviService.PviServiceInstance.Service.Cpus[CpuName()];
                    cpu.Disconnect();
                }
            }
            catch (System.Exception ex)
            {
                Log.Write(LogLevel.ERROR, ex);
            }
        }

        private string CpuName()
        {
            string name = this.rtbMachineName.Text.Replace(" ", "_");
            return name + this.rtbDestination.Text;
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbProduction_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = CPUMachine.ProductionFile;
            OpenFile();
        }

        private void tsbMaster_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = CPUMachine.MasterFile;
            OpenFile();
        }

        private void OpenFile()
        {
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                string productionFile = Path.Combine(CPUMachine.ProductionFile, openFileDialog1.SafeFileName);

                if (!File.Exists(CPUMachine.ProductionFile))
                {
                    try
                    {
                        File.Copy(openFileDialog1.FileName, productionFile, true);
                    }
                    catch (System.Exception ex)
                    {
                        Log.Write(ex);
                    }
                }
                this.rtbFileName.Text = openFileDialog1.SafeFileName;
                CPUMachine.GetExcelData(openFileDialog1.FileName);
                this.btnViewFile.Enabled = true;
                this.btnSendData.Enabled = true;
                this.btnViewExcel.Enabled = true;
            }
        }

        private void btnSendData_Click(object sender, EventArgs e)
        {
            CPUMachine.Send();
        }

        private void btnViewExcel_Click(object sender, EventArgs e)
        {
            IExcelEngine excelEngine = new ExcelEngine(openFileDialog1.FileName);
            excelEngine.SetVisibleTo(true);
        }
    }
}
