using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace BendSheets
{
    public partial class AddMachine : Form
    {
        private Machine m_Machine;

        public AddMachine(Machine machine)
        {
            InitializeComponent();
            m_Machine = machine;
        }

        private void btnMaster_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtMasterFile.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnProduction_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtProductionFile.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void AddMachine_Load(object sender, EventArgs e)
        {
            this.txtTemplate.Text = m_Machine.TemplatePath;
            this.txtMasterFile.Text = m_Machine.MasterFile;
            this.txtProductionFile.Text = m_Machine.ProductionFile;
            this.nudDestination.Value = m_Machine.Destination;
            this.txtMachineName.Text = m_Machine.MachineName;
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            m_Machine.MasterFile = this.txtMasterFile.Text;
            m_Machine.ProductionFile = this.txtProductionFile.Text;
            m_Machine.Destination = (int)this.nudDestination.Value;
            m_Machine.MachineName = this.txtMachineName.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory() + @"\Templates";

                if (DialogResult.OK == openFileDialog1.ShowDialog(this))
                {
                    txtTemplate.Text = openFileDialog1.FileName;
                    m_Machine.OpenTemplate(openFileDialog1.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error assigning template file", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
