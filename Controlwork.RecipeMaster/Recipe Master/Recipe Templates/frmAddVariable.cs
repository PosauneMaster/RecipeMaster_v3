using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BendSheets
{
    public partial class frmAddVariable : Form
    {
        private string m_SendVariable;
        private string m_ReceiveVariable;

        public RecipeTemplateItem RecipeItem
        {
            get
            {
                return new RecipeTemplateItem(m_SendVariable, m_ReceiveVariable, txtVariableName.Text, (RecipeTemplateItemType)comboBox1.SelectedItem, txtCellStart.Text, txtCellEnd.Text);             
            }
        }

        public frmAddVariable(string sendPrefix, string receivePrefix)
        {
            InitializeComponent();
            m_SendVariable = sendPrefix;
            m_ReceiveVariable = receivePrefix;
            this.comboBox1.Items.Add(RecipeTemplateItemType.Cell);
            this.comboBox1.Items.Add(RecipeTemplateItemType.Range);
            this.comboBox1.SelectedItem = RecipeTemplateItemType.Cell;
        }

        public frmAddVariable(string sendPrefix, string receivePrefix, string name)
        {
            InitializeComponent();
            m_SendVariable = sendPrefix;
            m_ReceiveVariable = receivePrefix;
            this.txtVariableName.Text = name; 
            this.comboBox1.Items.Add(RecipeTemplateItemType.Cell);
            this.comboBox1.Items.Add(RecipeTemplateItemType.Range);
            this.comboBox1.SelectedItem = RecipeTemplateItemType.Cell;
        }


        public frmAddVariable(RecipeTemplateItem recipeItem)
        {
            InitializeComponent();
            this.txtVariableName.Text = recipeItem.SendName;
            this.txtCellStart.Text = recipeItem.CellStart;
            this.txtCellEnd.Text = recipeItem.CellEnd;
            this.comboBox1.Items.Add(RecipeTemplateItemType.Cell);
            this.comboBox1.Items.Add(RecipeTemplateItemType.Range);
            this.comboBox1.SelectedItem = recipeItem.ItemType;
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
