using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WH.Utils.Logging;

namespace BendSheets
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
                Log.Write(ex);
            }
        }
    }
}
