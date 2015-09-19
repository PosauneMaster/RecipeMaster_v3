using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BendSheets.PVICommunication;
using BR.AN.PviServices;

namespace BendSheets
{
    public partial class PVINetworkForm : Form
    {
        private string m_Details = String.Empty;

        public PVINetworkForm()
        {
            InitializeComponent();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PVINetworkForm_Load(object sender, EventArgs e)
        {
            PviService pviService = PviService.PviServiceInstance;
            SetStatus(pviService.IsConnected);

            if (pviService.PVIEventArgs != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Error Code:  " + pviService.PVIEventArgs.ErrorCode.ToString());
                sb.AppendLine("ErrorText:   " + pviService.PVIEventArgs.ErrorText);
                sb.AppendLine("Name:        " + pviService.PVIEventArgs.Name);
                sb.AppendLine("Address:     " + pviService.PVIEventArgs.Address);
                sb.AppendLine("Action:      " + pviService.PVIEventArgs.Action.ToString());
                this.m_Details = sb.ToString();

                this.rtbErrorCode.Text = pviService.PVIEventArgs.ErrorCode.ToString();
                this.rtbErrorText.Text = pviService.PVIEventArgs.ErrorText;
                this.rtbName.Text = pviService.PVIEventArgs.Name;
                this.rtbAddress.Text = pviService.PVIEventArgs.Address;
                this.rtbAction.Text = pviService.PVIEventArgs.Action.ToString();
            }
        }

        private void SetStatus(bool connected)
        {
            if (connected)
            {
                this.rtbConnectionStatus.BackColor = System.Drawing.Color.Green;
                this.rtbConnectionStatus.ForeColor = System.Drawing.Color.White;
                this.rtbConnectionStatus.Text = "Connected";
            }
            else
            {
                this.rtbConnectionStatus.BackColor = System.Drawing.Color.Red;
                this.rtbConnectionStatus.ForeColor = System.Drawing.Color.White;
                this.rtbConnectionStatus.Text = "Not Connected";
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.m_Details);
        }

    }
}
