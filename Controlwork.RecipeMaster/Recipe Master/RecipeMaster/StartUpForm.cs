using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BendSheets.PVICommunication;
using BR.AN.PviServices;
using BendSheets.ConfigurationManagement;
using System.Configuration;

namespace BendSheets
{
    public partial class StartUpForm : Form
    {
        private readonly string CONNECTING = "Connecting to PVI Network..";
        private readonly string CONNECTED = "PVI Network Connected.";
        private readonly string ERROR_CONNECTING = "Error Connecting to PVI Network.";

        public StartUpForm()
        {
            InitializeComponent();
        }

        private void StartUpForm_Load(object sender, EventArgs e)
        {
            timerFade.Tick += new EventHandler(timerFadeIn_Tick);
            timerFade.Enabled = true;
            this.lblProgress.Text = CONNECTING;
        }

        private void timerFadeIn_Tick(object sender, EventArgs e)
        {
            if (this.progressBar1.Value < this.progressBar1.Maximum)
            {
                this.progressBar1.PerformStep();
                this.lblProgress.Text += ".";
            }
            if (this.Opacity < 1)
            {
                this.Opacity += .10;
                this.progressBar1.PerformStep();
            }
            if (this.progressBar1.Value >= this.progressBar1.Maximum && this.Opacity >= 1)
            {
                ConnectPVI();
            }
        }
        private void timerFadeOut_Tick(object sender, EventArgs e)
        {
            if(this.Opacity > 0)
            {
                this.Opacity -= .10;
            }
            else
            {
                this.Close();
            }
        }

        private void ConnectPVI()
        {
            timerFade.Enabled = false;

            PviService pviService = PviService.PviServiceInstance;
            pviService.ServiceConnected += new EventHandler<PviEventArgs>(pviEngine_ServiceConnected);
            pviService.ServiceError += new EventHandler<PviEventArgs>(pviEngine_ServiceError);
            pviService.ServiceDisconnected += new EventHandler<PviEventArgs>(pviService_ServiceDisconnected);

            bool mockConnection;

            if (!Boolean.TryParse(ConfigurationManager.AppSettings["mockConnection"], out mockConnection))
            {
                mockConnection = false;
            }

            pviService.ConnectPVIService(mockConnection);
        }

        private void pviService_ServiceDisconnected(object sender, PviEventArgs e)
        {
            PviService pviService = PviService.PviServiceInstance;
            pviService.PVIEventArgs = e;

            FadeOut(ERROR_CONNECTING);
        }

        private void pviEngine_ServiceError(object sender, PviEventArgs e)
        {
            PviService pviService = PviService.PviServiceInstance;
            pviService.PVIEventArgs = new PviEventArgs("Test", "123456", 98765, "language", BR.AN.PviServices.Action.ErrorEvent);
            FadeOut(ERROR_CONNECTING);
        }

        private void pviEngine_ServiceConnected(object sender, PviEventArgs e)
        {
            PviService pviService = PviService.PviServiceInstance;
            pviService.PVIEventArgs = e;
            FadeOut(CONNECTED);
        }

        private void FadeOut(string s)
        {
            this.lblProgress.Text = s;
            Application.DoEvents();
            System.Threading.Thread.Sleep(500);

            timerFade.Tick -= timerFadeIn_Tick;
            timerFade.Tick += new EventHandler(timerFadeOut_Tick);
            timerFade.Enabled = true;
        }
    }
}
