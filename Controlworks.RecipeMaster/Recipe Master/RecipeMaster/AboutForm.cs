using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Configuration;
using WH.Utils.Logging;

namespace BendSheets
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyCopyrightAttribute assemblyCopyright = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0] as AssemblyCopyrightAttribute;
            AssemblyProductAttribute assemblyProduct = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0] as AssemblyProductAttribute;

            this.lblProduct.Text = assemblyProduct.Product;
            this.lblCopyright.Text = assemblyCopyright.Copyright;
            this.lblVersion.Text = "Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string webSite = ConfigurationManager.AppSettings["ControlWorksWebsite"].ToString();
                System.Diagnostics.Process.Start(webSite);
            }
            catch (Exception ex)
            {
                Log.Write(LogLevel.ERROR, ex);
            }
        }
    }
}