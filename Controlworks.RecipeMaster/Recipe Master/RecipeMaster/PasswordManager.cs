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
    public partial class PasswordManager : Form
    {
        public PasswordManager()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidPassword())
            {
                MessageBox.Show("Password Entries must match!", "Match Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword1.Clear();
                txtPassword2.Clear();
            }
            else
            {
                SecurityManager.SaveSecuritySetting(this.chkRequirePassword.Checked, this.txtPassword1.Text);
                Close();
            }
        }

        private void PasswordManager_Load(object sender, EventArgs e)
        {
            chkRequirePassword.Checked = SecurityManager.RequirePassword();
        }

        private bool ValidPassword()
        {
            return txtPassword1.Text.Equals(this.txtPassword2.Text);
        }
    }
}