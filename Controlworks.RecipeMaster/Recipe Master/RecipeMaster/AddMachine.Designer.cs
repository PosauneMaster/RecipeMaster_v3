namespace BendSheets
{
    partial class AddMachine
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnMaster = new System.Windows.Forms.Button();
            this.btnProduction = new System.Windows.Forms.Button();
            this.txtMasterFile = new System.Windows.Forms.TextBox();
            this.txtProductionFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMachineName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudDestination = new System.Windows.Forms.NumericUpDown();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnTemplate = new System.Windows.Forms.Button();
            this.txtTemplate = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.nudDestination)).BeginInit();
            this.SuspendLayout();
            // 
            // btnMaster
            // 
            this.btnMaster.Location = new System.Drawing.Point(12, 56);
            this.btnMaster.Name = "btnMaster";
            this.btnMaster.Size = new System.Drawing.Size(86, 23);
            this.btnMaster.TabIndex = 0;
            this.btnMaster.Text = "Master File";
            this.btnMaster.UseVisualStyleBackColor = true;
            this.btnMaster.Click += new System.EventHandler(this.btnMaster_Click);
            // 
            // btnProduction
            // 
            this.btnProduction.Location = new System.Drawing.Point(12, 85);
            this.btnProduction.Name = "btnProduction";
            this.btnProduction.Size = new System.Drawing.Size(86, 23);
            this.btnProduction.TabIndex = 1;
            this.btnProduction.Text = "Production File";
            this.btnProduction.UseVisualStyleBackColor = true;
            this.btnProduction.Click += new System.EventHandler(this.btnProduction_Click);
            // 
            // txtMasterFile
            // 
            this.txtMasterFile.Location = new System.Drawing.Point(104, 59);
            this.txtMasterFile.Name = "txtMasterFile";
            this.txtMasterFile.Size = new System.Drawing.Size(264, 20);
            this.txtMasterFile.TabIndex = 2;
            // 
            // txtProductionFile
            // 
            this.txtProductionFile.Location = new System.Drawing.Point(104, 88);
            this.txtProductionFile.Name = "txtProductionFile";
            this.txtProductionFile.Size = new System.Drawing.Size(264, 20);
            this.txtProductionFile.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Machine Name";
            // 
            // txtMachineName
            // 
            this.txtMachineName.Location = new System.Drawing.Point(104, 115);
            this.txtMachineName.Name = "txtMachineName";
            this.txtMachineName.Size = new System.Drawing.Size(100, 20);
            this.txtMachineName.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Destination";
            // 
            // nudDestination
            // 
            this.nudDestination.Location = new System.Drawing.Point(104, 141);
            this.nudDestination.Maximum = global::BendSheets.Properties.Settings.Default.MaxValue;
            this.nudDestination.Name = "nudDestination";
            this.nudDestination.Size = new System.Drawing.Size(100, 20);
            this.nudDestination.TabIndex = 7;
            this.nudDestination.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnOkay
            // 
            this.btnOkay.Location = new System.Drawing.Point(226, 179);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 8;
            this.btnOkay.Text = "OK";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(316, 179);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnTemplate
            // 
            this.btnTemplate.Location = new System.Drawing.Point(12, 27);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(86, 23);
            this.btnTemplate.TabIndex = 10;
            this.btnTemplate.Text = "Template";
            this.btnTemplate.UseVisualStyleBackColor = true;
            this.btnTemplate.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // txtTemplate
            // 
            this.txtTemplate.Location = new System.Drawing.Point(104, 30);
            this.txtTemplate.Name = "txtTemplate";
            this.txtTemplate.Size = new System.Drawing.Size(264, 20);
            this.txtTemplate.TabIndex = 11;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "\"XML files|*.xml|All files|*.*\"";
            // 
            // AddMachine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 219);
            this.Controls.Add(this.txtTemplate);
            this.Controls.Add(this.btnTemplate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.nudDestination);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMachineName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtProductionFile);
            this.Controls.Add(this.txtMasterFile);
            this.Controls.Add(this.btnProduction);
            this.Controls.Add(this.btnMaster);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddMachine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Machine Settings";
            this.Load += new System.EventHandler(this.AddMachine_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudDestination)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnMaster;
        private System.Windows.Forms.Button btnProduction;
        private System.Windows.Forms.TextBox txtMasterFile;
        private System.Windows.Forms.TextBox txtProductionFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMachineName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudDestination;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnTemplate;
        private System.Windows.Forms.TextBox txtTemplate;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}