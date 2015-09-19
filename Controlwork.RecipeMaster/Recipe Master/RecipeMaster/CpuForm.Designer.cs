namespace BendSheets
{
    partial class CpuForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CpuForm));
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnSendData = new System.Windows.Forms.Button();
            this.btnViewFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnViewExcel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.rtbMachineName = new System.Windows.Forms.RichTextBox();
            this.rtbProductionFile = new System.Windows.Forms.RichTextBox();
            this.rtbStatus = new System.Windows.Forms.RichTextBox();
            this.rtbMasterFile = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rtbDestination = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnOkay = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbMaster = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbProduction = new System.Windows.Forms.ToolStripButton();
            this.label6 = new System.Windows.Forms.Label();
            this.rtbTemplate = new System.Windows.Forms.RichTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.rtbFileName = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(10, 19);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnSendData
            // 
            this.btnSendData.Location = new System.Drawing.Point(91, 48);
            this.btnSendData.Name = "btnSendData";
            this.btnSendData.Size = new System.Drawing.Size(75, 23);
            this.btnSendData.TabIndex = 1;
            this.btnSendData.Text = "Send Data";
            this.btnSendData.UseVisualStyleBackColor = true;
            this.btnSendData.Click += new System.EventHandler(this.btnSendData_Click);
            // 
            // btnViewFile
            // 
            this.btnViewFile.Location = new System.Drawing.Point(91, 19);
            this.btnViewFile.Name = "btnViewFile";
            this.btnViewFile.Size = new System.Drawing.Size(75, 23);
            this.btnViewFile.TabIndex = 4;
            this.btnViewFile.Text = "View Variables";
            this.btnViewFile.UseVisualStyleBackColor = true;
            this.btnViewFile.Click += new System.EventHandler(this.btnViewFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 171);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Status:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Master File:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnViewExcel);
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Controls.Add(this.btnViewFile);
            this.groupBox1.Controls.Add(this.btnSendData);
            this.groupBox1.Location = new System.Drawing.Point(104, 230);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(177, 87);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Files";
            // 
            // btnViewExcel
            // 
            this.btnViewExcel.Location = new System.Drawing.Point(10, 48);
            this.btnViewExcel.Name = "btnViewExcel";
            this.btnViewExcel.Size = new System.Drawing.Size(75, 23);
            this.btnViewExcel.TabIndex = 5;
            this.btnViewExcel.Text = "View Excel";
            this.btnViewExcel.UseVisualStyleBackColor = true;
            this.btnViewExcel.Click += new System.EventHandler(this.btnViewExcel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Machine Name:";
            // 
            // rtbMachineName
            // 
            this.rtbMachineName.Location = new System.Drawing.Point(103, 43);
            this.rtbMachineName.Multiline = false;
            this.rtbMachineName.Name = "rtbMachineName";
            this.rtbMachineName.ReadOnly = true;
            this.rtbMachineName.Size = new System.Drawing.Size(178, 20);
            this.rtbMachineName.TabIndex = 12;
            this.rtbMachineName.Text = "";
            // 
            // rtbProductionFile
            // 
            this.rtbProductionFile.Location = new System.Drawing.Point(103, 143);
            this.rtbProductionFile.Multiline = false;
            this.rtbProductionFile.Name = "rtbProductionFile";
            this.rtbProductionFile.ReadOnly = true;
            this.rtbProductionFile.Size = new System.Drawing.Size(178, 20);
            this.rtbProductionFile.TabIndex = 13;
            this.rtbProductionFile.Text = "";
            // 
            // rtbStatus
            // 
            this.rtbStatus.Location = new System.Drawing.Point(103, 168);
            this.rtbStatus.Multiline = false;
            this.rtbStatus.Name = "rtbStatus";
            this.rtbStatus.ReadOnly = true;
            this.rtbStatus.Size = new System.Drawing.Size(178, 20);
            this.rtbStatus.TabIndex = 14;
            this.rtbStatus.Text = "";
            // 
            // rtbMasterFile
            // 
            this.rtbMasterFile.Location = new System.Drawing.Point(103, 118);
            this.rtbMasterFile.Multiline = false;
            this.rtbMasterFile.Name = "rtbMasterFile";
            this.rtbMasterFile.ReadOnly = true;
            this.rtbMasterFile.Size = new System.Drawing.Size(178, 20);
            this.rtbMasterFile.TabIndex = 15;
            this.rtbMasterFile.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 146);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Production File:";
            // 
            // rtbDestination
            // 
            this.rtbDestination.Location = new System.Drawing.Point(103, 93);
            this.rtbDestination.Multiline = false;
            this.rtbDestination.Name = "rtbDestination";
            this.rtbDestination.ReadOnly = true;
            this.rtbDestination.Size = new System.Drawing.Size(178, 20);
            this.rtbDestination.TabIndex = 17;
            this.rtbDestination.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Destination:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx|All Files|*.*";
            // 
            // btnOkay
            // 
            this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOkay.Location = new System.Drawing.Point(206, 335);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 6;
            this.btnOkay.Text = "OK";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbMaster,
            this.toolStripSeparator1,
            this.tsbProduction});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(301, 25);
            this.toolStrip1.TabIndex = 19;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbMaster
            // 
            this.tsbMaster.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbMaster.Image = ((System.Drawing.Image)(resources.GetObject("tsbMaster.Image")));
            this.tsbMaster.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMaster.Name = "tsbMaster";
            this.tsbMaster.Size = new System.Drawing.Size(100, 22);
            this.tsbMaster.Text = "Open Master File";
            this.tsbMaster.Click += new System.EventHandler(this.tsbMaster_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbProduction
            // 
            this.tsbProduction.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbProduction.Image = ((System.Drawing.Image)(resources.GetObject("tsbProduction.Image")));
            this.tsbProduction.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbProduction.Name = "tsbProduction";
            this.tsbProduction.Size = new System.Drawing.Size(123, 22);
            this.tsbProduction.Text = "Open Production File";
            this.tsbProduction.Click += new System.EventHandler(this.tsbProduction_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Template:";
            // 
            // rtbTemplate
            // 
            this.rtbTemplate.Location = new System.Drawing.Point(103, 68);
            this.rtbTemplate.Multiline = false;
            this.rtbTemplate.Name = "rtbTemplate";
            this.rtbTemplate.ReadOnly = true;
            this.rtbTemplate.Size = new System.Drawing.Size(178, 20);
            this.rtbTemplate.TabIndex = 21;
            this.rtbTemplate.Text = "";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 197);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "File Name:";
            // 
            // rtbFileName
            // 
            this.rtbFileName.Location = new System.Drawing.Point(103, 194);
            this.rtbFileName.Multiline = false;
            this.rtbFileName.Name = "rtbFileName";
            this.rtbFileName.ReadOnly = true;
            this.rtbFileName.Size = new System.Drawing.Size(178, 20);
            this.rtbFileName.TabIndex = 23;
            this.rtbFileName.Text = "";
            // 
            // CpuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOkay;
            this.ClientSize = new System.Drawing.Size(301, 370);
            this.Controls.Add(this.rtbFileName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.rtbTemplate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.rtbDestination);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rtbMasterFile);
            this.Controls.Add(this.rtbStatus);
            this.Controls.Add(this.rtbProductionFile);
            this.Controls.Add(this.rtbMachineName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CpuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect Machine";
            this.Load += new System.EventHandler(this.CpuForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnSendData;
        private System.Windows.Forms.Button btnViewFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox rtbMachineName;
        private System.Windows.Forms.RichTextBox rtbProductionFile;
        private System.Windows.Forms.RichTextBox rtbStatus;
        private System.Windows.Forms.RichTextBox rtbMasterFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox rtbDestination;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbMaster;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbProduction;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RichTextBox rtbTemplate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox rtbFileName;
        private System.Windows.Forms.Button btnViewExcel;
    }
}