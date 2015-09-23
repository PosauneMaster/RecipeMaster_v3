using System;
using System.Windows.Forms;

namespace ControlWorks.RecipeMaster
{
    public partial class frmVariables : Form
    {
        private Machine m_Machine;

        public frmVariables()
        {
            InitializeComponent();
        }

        public frmVariables(Machine machine)
        {
            InitializeComponent();
            m_Machine = machine;
        }

        private void frmVariables_Load(object sender, EventArgs e)
        {
            try
            {
                if (m_Machine != null)
                {
                    foreach (RecipeVariable variable in m_Machine.GetVariableCollection())
                    {
                        dataGridView1.Rows.Add(variable.Name, variable.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogError("frmVariables.frmVariables_Load", ex);
            }
        }
    }
}
