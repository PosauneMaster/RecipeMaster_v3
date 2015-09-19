using System;
using System.Windows.Forms;
using System.IO;
using System.Configuration;

namespace BendSheets
{
    public partial class frmCreateTemplate : Form
    {
        private RecipeTemplates m_RecipeTemplates;
        private BindingSource m_BindingSource;

        public frmCreateTemplate()
        {
            InitializeComponent();
        }

        private void frmCreateTemplate_Load(object sender, EventArgs e)
        {

            this.txtSendVariable.Text = ConfigurationManager.AppSettings["SendVariablePrefix"].ToString();
            this.txtReceiveVariable.Text =  ConfigurationManager.AppSettings["ReceiveVariablePrefix"].ToString();

            try
            {
                this.dataGridView1.AutoGenerateColumns = false;
                this.dataGridView1.Columns["colSendVariable"].DataPropertyName = "SendName";
                this.dataGridView1.Columns["colReceiveVariable"].DataPropertyName = "ReceiveName";
                this.dataGridView1.Columns["colVariableType"].DataPropertyName = "ItemType";
                this.dataGridView1.Columns["colCellStart"].DataPropertyName = "CellStart";
                this.dataGridView1.Columns["colCellEnd"].DataPropertyName = "CellEnd";

                this.dataGridView1.Columns["colSendVariable"].Width = 230;
                this.dataGridView1.Columns["colReceiveVariable"].Width = 230;
                this.dataGridView1.Columns["colVariableType"].Width = 100;
                this.dataGridView1.Columns["colCellStart"].Width = 74;
            }
            catch (Exception ex)
            {
            }

            Initialize();
        }

        private void Initialize()
        {
            m_RecipeTemplates = new RecipeTemplates();
            m_BindingSource = new BindingSource();
            m_BindingSource.DataSource = m_RecipeTemplates.TemplateList;
            this.dataGridView1.DataSource = m_BindingSource;

            this.dataGridView1.RowsAdded += new DataGridViewRowsAddedEventHandler(dataGridView1_RowsAdded);
        }

        private void openTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory() + @"\Templates";
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                try
                {
                    m_RecipeTemplates = new RecipeTemplates();
                    m_RecipeTemplates.Load(openFileDialog1.FileName);

                    m_BindingSource = new BindingSource();
                    m_BindingSource.DataSource = m_RecipeTemplates.TemplateList;
                    this.dataGridView1.DataSource = m_BindingSource;

                    this.dataGridView1.RowsAdded += new DataGridViewRowsAddedEventHandler(dataGridView1_RowsAdded);

                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void saveTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_RecipeTemplates == null)
            {
                return;
            }

            if (m_RecipeTemplates.Count == 0)
            {
                return;
            }

            saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory() + @"\Templates";

            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                m_RecipeTemplates.Save(saveFileDialog1.FileName);
            }
        }


        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            this.dataGridView1.Rows[e.RowIndex].Selected = true;
        }

        private void newItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNew();
        }

        private void AddNew()
        {
            if (m_RecipeTemplates == null)
            {
                return;
            }
            m_RecipeTemplates.AddTemplate(new RecipeTemplateItem());
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count > 0)
            {               
                RecipeTemplateItem item = dataGridView1.CurrentRow.DataBoundItem as RecipeTemplateItem;
                if (item == null)
                {
                    return;
                }
                if (DialogResult.OK == MessageBox.Show("Really Delete Variable " + item.SendName + "?", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    m_RecipeTemplates.DeleteTemplate(item);
                }
            }
        }

        private void newTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Initialize();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (frmAddVariable addVariable = new frmAddVariable(txtSendVariable.Text, txtReceiveVariable.Text))
                {
                    if (DialogResult.OK == addVariable.ShowDialog(this))
                    {
                        m_RecipeTemplates.AddTemplate(addVariable.RecipeItem);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.Rows.Count > 0)
                {
                    RecipeTemplateItem item = dataGridView1.CurrentRow.DataBoundItem as RecipeTemplateItem;
                    if (item != null)
                    {
                        using (frmAddVariable addVariable = new frmAddVariable(item))
                        {
                            if (DialogResult.OK == addVariable.ShowDialog(this))
                            {
                                m_RecipeTemplates.DeleteTemplate(item);
                                m_RecipeTemplates.AddTemplate(addVariable.RecipeItem);
                                m_BindingSource.MoveFirst();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
