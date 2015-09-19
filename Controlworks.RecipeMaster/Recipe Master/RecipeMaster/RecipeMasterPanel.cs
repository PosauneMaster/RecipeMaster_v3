using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using RecipeMaster.DataMapping;
using BendSheets.ConfigurationManagement;
using BendSheets.PVICommunication;

namespace BendSheets
{
    public partial class RecipeMasterPanel : UserControl
    {
        public event EventHandler<ButtonActionEventArgs> PanelButtonAction;

        private BendSheetDataType _fileType;

        private string _productionTemplate;
        private int _tabPanelIndex;
        private bool m_Connected;
        private TabPage _recipeTabPage;
        private string m_MachineName;

        private int m_Index;

        public RecipeMasterPanel(int index)
        {
            InitializeComponent();
            SetButtonTags();
            m_Index = index;

            this.ProductionTemplate = BendSheetSettings.GetProductionTemplate(index);
            this.TabPanelIndex = 0;
            this.MachineName = BendSheetSettings.getMachineName(index);

            DisableButtons();
        }

        protected void OnPanelButtonAction(object sender)
        {
            if (PanelButtonAction != null)
            {
                Button button = sender as Button;
                ButtonAction action = (ButtonAction)button.Tag;
                //this.PanelButtonAction(this, new ButtonActionEventArgs(button, action));
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            OnSearchFile(sender);
        }

        protected void OnSearchFile(object sender)
        {
            if (PanelButtonAction != null)
            {
                Button button = sender as Button;
                ButtonAction action = (ButtonAction)button.Tag;

                string directory;
                if (_fileType == BendSheetDataType.MASTER)
                {
                    directory = BendSheetSettings.MasterFile1;
                }
                else
                {
                    directory = ProductionDirectory;
                }
                SearchEventArgs searchEventArgs = new SearchEventArgs(txtPartNumber.Text, directory, _fileType);
                //ButtonActionEventArgs buttonActionEventArgs = new ButtonActionEventArgs(button, action, searchEventArgs);

                //PanelButtonAction(this, buttonActionEventArgs);
            }
        }

        public void ExcelLoaded()
        {
            SetStatusLabel();
            EnableAll();
        }

        public void ExcelClosed()
        {
            EnableButtonsOnConnect();
        }

        private void EnableAll()
        {
            this.btnConnect.Enabled = true;
            this.btnExit.Enabled = true;
            this.btnOpenMasterFile.Enabled = true;
            this.btnOpenProductionFile.Enabled = true;
            this.btnSave.Enabled = true;
            this.btnSearch.Enabled = true;
            this.btnSendData.Enabled = true;
            this.btnViewFile.Enabled = true;
        }

        public void EnableButtonsOnConnect()
        {
            this.btnConnect.Enabled = true;
            this.btnExit.Enabled = true;
            this.btnOpenMasterFile.Enabled = true;
            this.btnOpenProductionFile.Enabled = true;
            this.btnSave.Enabled = false;
            this.btnSearch.Enabled = true;
            this.btnSendData.Enabled = false;
            this.btnViewFile.Enabled = false;
        }


        private void DisableButtons()
        {
            this.btnConnect.Enabled = true;
            this.btnExit.Enabled = true;
            this.btnOpenMasterFile.Enabled = false;
            this.btnOpenProductionFile.Enabled = false;
            this.btnSave.Enabled = false;
            this.btnSearch.Enabled = false;
            this.btnSendData.Enabled = false;
            this.btnViewFile.Enabled = false;
        }

        private void DisableAll()
        {
            this.btnConnect.Enabled = false;
            this.btnExit.Enabled = false;
            this.btnOpenMasterFile.Enabled = false;
            this.btnOpenProductionFile.Enabled = false;
            this.btnSave.Enabled = false;
            this.btnSearch.Enabled = false;
            this.btnSendData.Enabled = false;
            this.btnViewFile.Enabled = false;
        }


        private void SetStatusLabel()
        {
            this.lblMachineName.Text = this.MachineName + " Status:";
            this.lblStatus.BackColor = this.IsConnected ? Color.Green : Color.Red;
            this.lblStatus.Text = this.IsConnected ? "Connected" : "Disconnected";
        }

        public void Connected()
        {
            this.IsConnected = true;
            this.SetStatusLabel();
            EnableButtonsOnConnect();
            this.btnConnect.Text = "Disconnect";
            this.btnConnect.Tag = ButtonAction.Disconnect;
        }

        public void Disconnected()
        {
            this.IsConnected = false;
            this.SetStatusLabel();
            DisableButtons();
            this.btnConnect.Text = "Connect";
            this.btnConnect.Tag = ButtonAction.Connect;
        }

        public void ExcelVisible(bool visible)
        {
            if (visible)
            {
                this.btnViewFile.Tag = ButtonAction.HideFile;
                this.btnViewFile.Text = "Hide File";
            }
            else
            {
                this.btnViewFile.Tag = ButtonAction.ViewFile;
                this.btnViewFile.Text = "View File";
            }
        }

        public string ProductionTemplate
        {
            get { return _productionTemplate; }
            set { _productionTemplate = value; }
        }

        public BendSheetDataType FileType
        {
            get { return _fileType; }
            set
            {
                _fileType = value;
                if (_fileType == BendSheetDataType.MASTER)
                {
                    this.cboFileType.SelectedIndex = 0;
                }
                else if (_fileType == BendSheetDataType.PRODUCTION)
                {
                    this.cboFileType.SelectedIndex = 1;
                }
            }
        }

        public string FileName
        {
            get { return txtFileName.Text; }
            set { txtFileName.Text = value; }
        }

        public string CustomerName
        {
            get { return txtCustomerName.Text; }
            set { txtCustomerName.Text = value; }
        }

        public string PartNumber
        {
            get { return txtPartNumber.Text; }
            set { txtPartNumber.Text = value; }
        }

        public int Destination
        {
            get { return BendSheetSettings.getDestination(m_Index); }
        }

        public string ProductionDirectory
        {
            get { return BendSheetSettings.getProductionDirectory(this.m_Index); }
        }

        public bool IsConnected
        {
            get { return m_Connected; }
            set
            {
                m_Connected = value;
                txtPartNumber.Enabled = m_Connected;
            }
        }

        public int TabPanelIndex
        {
            get { return _tabPanelIndex; }
            set { _tabPanelIndex = value; }
        }

        public string MachineName
        {
            get { return m_MachineName; }
            set { m_MachineName = value; }
        }

        public TabPage RecipeTabPage
        {
            get { return _recipeTabPage; }
            set { _recipeTabPage = value; }
        }

        public RecipeMasterPanel()
        {
            InitializeComponent();
        }

        private void SetButtonTags()
        {
            this.btnConnect.Tag = ButtonAction.Connect;
            this.btnExit.Tag = ButtonAction.Exit;
            this.btnOpenMasterFile.Tag = ButtonAction.OpenMasterFile;
            this.btnOpenProductionFile.Tag = ButtonAction.OpenProductionFile;
            this.btnSave.Tag = ButtonAction.Save; this.btnSearch.Tag = ButtonAction.Search;
            this.btnSendData.Tag = ButtonAction.SendData;
            this.btnViewFile.Tag = ButtonAction.ViewFile;
        }

        private void BendSheetPanel_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            this.FileType = BendSheetDataType.MASTER;
            this.TabIndex = 0;
        }

        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            this.toolTip1.Active = false;
            toolTip1.SetToolTip(tb, tb.Text);
            this.toolTip1.Active = true;
        }

        public void Reset()
        {
            FileType = BendSheetDataType.MASTER;
            txtFileName.Text = String.Empty;
            txtCustomerName.Text = String.Empty;
            txtPartNumber.Text = String.Empty;
        }

        private void cboFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboFileType.SelectedIndex == 0)
            {
                _fileType = BendSheetDataType.MASTER;
            }
            else if (this.cboFileType.SelectedIndex == 1)
            {
                _fileType = BendSheetDataType.PRODUCTION;
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            ButtonAction action = (ButtonAction)b.Tag;
            if (action == ButtonAction.OpenMasterFile || action == ButtonAction.OpenProductionFile)
            {
                DisableAll();
            }
            OnPanelButtonAction(sender);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            ButtonAction action = (ButtonAction)b.Tag;
            DisableAll();
            if (action == ButtonAction.Connect)
            {
                this.lblStatus.Text = "Connecting";
                this.lblStatus.BackColor = Color.Blue;
            }
            OnPanelButtonAction(sender);
        }

        private void open_FileHandler(object sender, EventArgs e)
        {
            DisableAll();
            this.lblStatus.Text = "Opening File";
            this.lblStatus.BackColor = Color.Blue;
            OnPanelButtonAction(sender);
        }
    }
}
