using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

using BendSheets.ConfigurationManagement;


namespace BendSheets
{
    public partial class RecipeMasterfigurationManager : Form
    {
        public event EventHandler<EventArgs> SettingsChanged;

        public RecipeMasterfigurationManager()
        {
            InitializeComponent();
        }

        private void btnBrowseMaster_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtPathMaster.Text = folderBrowserDialog1.SelectedPath;
                SetToolTipText(txtPathMaster);
            }
        }

        private void btnBrowseProduction_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            TextBox t = b.Tag as TextBox;

            folderBrowserDialog1.SelectedPath = t.Text;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                t.Text = folderBrowserDialog1.SelectedPath;
                SetToolTipText(t);
            }
        }

        private void SetToolTipText(TextBox textBox)
        {
            this.toolTip1.Active = false;
            toolTip1.SetToolTip(textBox, textBox.Text);
            this.toolTip1.Active = true;
        }

        private void ConfigurationManager_Load(object sender, EventArgs e)
        {
            txtPathMaster.Text = BendSheetSettings.MasterFile1;
            txtPathProduction1.Text = BendSheetSettings.ProductionFile1;
            txtPathProduction2.Text = BendSheetSettings.ProductionFile2;
            txtPathProduction3.Text = BendSheetSettings.ProductionFile3;
            txtPathProduction4.Text = BendSheetSettings.ProductionFile4;
            txtPathProduction5.Text = BendSheetSettings.ProductionFile5;

            txtMachine1.Text = BendSheetSettings.MachineName1;
            txtMachine2.Text = BendSheetSettings.MachineName2;
            txtMachine3.Text = BendSheetSettings.MachineName3;
            txtMachine4.Text = BendSheetSettings.MachineName4;
            txtMachine5.Text = BendSheetSettings.MachineName5;

            numDest1.Value = BendSheetSettings.Destination1;
            numDest2.Value = BendSheetSettings.Destination2;
            numDest3.Value = BendSheetSettings.Destination3;
            numDest4.Value = BendSheetSettings.Destination4;
            numDest5.Value = BendSheetSettings.Destination5;

            SetToolTipText(txtPathMaster);
            SetToolTipText(txtPathProduction1);
            SetToolTipText(txtPathProduction2);
            SetToolTipText(txtPathProduction3);
            SetToolTipText(txtPathProduction4);
            SetToolTipText(txtPathProduction5);

            btnBrowseProduction1.Tag = txtPathProduction1;
            btnBrowseProduction2.Tag = txtPathProduction2;
            btnBrowseProduction3.Tag = txtPathProduction3;
            btnBrowseProduction4.Tag = txtPathProduction4;
            btnBrowseProduction5.Tag = txtPathProduction5;

            checkBox1.Checked = BendSheetSettings.Archive;
            txtCell.Text = BendSheetSettings.SavePathCell;

            SetLabelText();

            txtPathMaster.SelectionStart = 0;
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            if (ApplySettings())
            {
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ApplySettings();
        }

        private bool ApplySettings()
        {
            if (!Valid())
            {
                MessageBox.Show("Destinations must be unique or zero!", "Duplicate Destination", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            Save();
            SettingsChanged(this, new EventArgs());
            return true;
        }

        private bool Valid()
        {
            Stack<int> s = new Stack<int>(4);
            s.Push((int)numDest1.Value);
            s.Push((int)numDest2.Value);
            s.Push((int)numDest3.Value);
            s.Push((int)numDest4.Value);
            s.Push((int)numDest5.Value);

            while (s.Count > 1)
            {
                int pop = s.Pop();
                if (s.Contains(pop) && pop > 0)
                {
                    return false;
                }
            }

            return true;
        }

        private void Save()
        {
            BendSheetSettings.MasterFile1 = txtPathMaster.Text;

            BendSheetSettings.ProductionFile1 = txtPathProduction1.Text;
            BendSheetSettings.ProductionFile2 = txtPathProduction2.Text;
            BendSheetSettings.ProductionFile3 = txtPathProduction3.Text;
            BendSheetSettings.ProductionFile4 = txtPathProduction4.Text;
            BendSheetSettings.ProductionFile5 = txtPathProduction5.Text;

            BendSheetSettings.MachineName1 = txtMachine1.Text;
            BendSheetSettings.MachineName2 = txtMachine2.Text;
            BendSheetSettings.MachineName3 = txtMachine3.Text;
            BendSheetSettings.MachineName4 = txtMachine4.Text;
            BendSheetSettings.MachineName5 = txtMachine5.Text;

            BendSheetSettings.Destination1 = (int)numDest1.Value;
            BendSheetSettings.Destination2 = (int)numDest2.Value;
            BendSheetSettings.Destination3 = (int)numDest3.Value;
            BendSheetSettings.Destination4 = (int)numDest4.Value;
            BendSheetSettings.Destination5 = (int)numDest5.Value;

            BendSheetSettings.SavePathCell = txtCell.Text;

            SetLabelText();
        }

        private void SetLabelText()
        {
            lblProduction1.Text = BendSheetSettings.MachineName1;
            lblProduction2.Text = BendSheetSettings.MachineName2;
            lblProduction3.Text = BendSheetSettings.MachineName3;
            lblProduction4.Text = BendSheetSettings.MachineName4;
            lblProduction5.Text = BendSheetSettings.MachineName5;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            BendSheetSettings.Archive = cb.Checked;
        }

        private void numDest_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nud = sender as NumericUpDown;
            if (nud.Value < 0 || nud.Value >= 50)
            {
                MessageBox.Show("Value must be between 1 and 49", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                nud.Value = 0;
            }
        }
    }
}