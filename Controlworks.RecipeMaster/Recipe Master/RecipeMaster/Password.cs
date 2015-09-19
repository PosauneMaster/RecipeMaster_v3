using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using BendSheets.ConfigurationManagement;

namespace BendSheets
{
    public partial class Password : Form
    {
        public Password()
        {
            InitializeComponent();
        }

        public Password(Form launchForm)
        {
            InitializeComponent();
            _launchForm = launchForm;
        }

        private Form _launchForm;

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (SecurityManager.ValidatePassword(this.txtPassword.Text))
            {
                this.Visible = false;
                _launchForm.ShowDialog();
            }
            else 
            {
                MessageBox.Show("Invalid Password!", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                _launchForm.Close();
            }
            Close();               
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}