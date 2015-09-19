using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BendSheets.ConfigurationManagement;
using System.Xml.Serialization;
using System.IO;
using WH.Utils.Logging;
using BendSheets.PVICommunication;

namespace BendSheets
{
    public partial class SettingsForm : Form
    {
        private bool m_SaveOnClose;
        private MachineCollection m_MachineCollection;

        public SettingsForm()
        {
            InitializeComponent();
        }

        public SettingsForm(MachineCollection machineCollection)
        {
            InitializeComponent();
            m_MachineCollection = machineCollection;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            m_SaveOnClose = true;

            this.txtMasterFile.TextChanged += new EventHandler(txtMasterFile_TextChanged);
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.Columns["colMachineName"].DataPropertyName = "MachineName";
            this.dataGridView1.Columns["colDestination"].DataPropertyName = "Destination";
            this.dataGridView1.Columns["colActive"].DataPropertyName = "Active";
            this.dataGridView1.Columns["colActiveText"].DataPropertyName = "ActiveText";
            this.dataGridView1.Columns["colProductionFile"].DataPropertyName = "ProductionFile";
            this.dataGridView1.Columns["colMasterFile"].DataPropertyName = "MasterFile";
            this.dataGridView1.Columns["colTemplates"].DataPropertyName = "TemplatePath";

            this.txtMasterFile.Text = m_MachineCollection.MasterFilePath;
            this.txtProductionDirectory.Text = m_MachineCollection.ProductionFilePath;

            this.dataGridView1.DataSource = m_MachineCollection.MachineList;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                SetToolTip(row);
            }
        }

        private void txtMasterFile_TextChanged(object sender, EventArgs e)
        {
            SetToolTipText(sender as TextBox);
        }

        private void SetToolTip(DataGridViewRow row)
        {
            row.Cells["colProductionFile"].ToolTipText = row.Cells["colProductionFile"].Value.ToString();
        }

        private void btnBrowseMaster_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == folderBrowserDialog1.ShowDialog())
            {
                txtMasterFile.Text = folderBrowserDialog1.SelectedPath;
                m_MachineCollection.MasterFilePath = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnBrowseProduction_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == folderBrowserDialog1.ShowDialog())
            {
                this.txtProductionDirectory.Text = folderBrowserDialog1.SelectedPath;
                m_MachineCollection.ProductionFilePath = folderBrowserDialog1.SelectedPath;
            }
        }

        private void SetToolTipText(TextBox textBox)
        {
            this.toolTip1.Active = false;
            toolTip1.SetToolTip(textBox, textBox.Text);
            this.toolTip1.Active = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.m_SaveOnClose = false;
            this.Close();
        }

        private void AddMachine()
        {
            Machine machine = new Machine();
            machine.MasterFile = this.txtMasterFile.Text;
            machine.ProductionFile = this.txtProductionDirectory.Text;
            AddMachine am = new AddMachine(machine);
            if (am.ShowDialog() == DialogResult.OK)
            {
                this.m_MachineCollection.AddMachine(machine);
            }
        }

        private void EditMachine(DataGridViewRow row)
        {
            try
            {
                Machine machine = row.DataBoundItem as Machine;
                using (AddMachine am = new AddMachine(machine))
                {
                    am.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading settings", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Write(ex);
            }
        }

        private void DeleteMachine(DataGridViewRow row)
        {
            try
            {
                Machine machine = row.DataBoundItem as Machine;
                if (MessageBox.Show(
                    "Are you sure you wish to delete machine " + machine.MachineName + "?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    m_MachineCollection.MachineList.Remove(machine);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing settings", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Write(ex);
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid != null)
            {
                this.EditMachine(grid.Rows[e.RowIndex]);
            }
        }

        private void loadSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                m_MachineCollection = MachineCollection.LoadSettings(openFileDialog1.FileName);
                InitializeGrid();
            }

        }

        private void saveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                m_MachineCollection.Save(saveFileDialog1.FileName);
            }
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    if (m_SaveOnClose)
        //    {
        //        string path = Directory.GetCurrentDirectory() + @"\" + settingsFile;
        //        this.Save(path);
        //    }
        //}

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == mnuAdd)
            {
                this.AddMachine();
            }
            if (e.ClickedItem == mnuDelete)
            {
                if (dataGridView1.SelectedRows[0] != null)
                {
                    ContextMenuStrip context = sender as ContextMenuStrip;
                    context.Close();
                    this.DeleteMachine(dataGridView1.SelectedRows[0]);
                }
            }
            if (e.ClickedItem == mnuEdit)
            {
                if (dataGridView1.SelectedRows[0] != null)
                {
                    this.EditMachine(dataGridView1.SelectedRows[0]);
                }
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                this.Validate();
            }
            if (e.ColumnIndex == 5)
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    DataGridView dg = sender as DataGridView;
                    dg.Rows[e.RowIndex].Cells["colProductionFile"].Value = folderBrowserDialog1.SelectedPath;
                    SetToolTip(dg.Rows[e.RowIndex]);
                    this.Validate();
                }
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
    }
}
