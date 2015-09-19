namespace BendSheets
{
    partial class PVINetworkForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.rtbConnectionStatus = new System.Windows.Forms.RichTextBox();
            this.rtbAction = new System.Windows.Forms.RichTextBox();
            this.rtbAddress = new System.Windows.Forms.RichTextBox();
            this.rtbErrorCode = new System.Windows.Forms.RichTextBox();
            this.rtbErrorText = new System.Windows.Forms.RichTextBox();
            this.rtbName = new System.Windows.Forms.RichTextBox();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connection Status:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Action:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Address:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Error Code:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Error Text:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Name:";
            // 
            // rtbConnectionStatus
            // 
            this.rtbConnectionStatus.Location = new System.Drawing.Point(111, 6);
            this.rtbConnectionStatus.Name = "rtbConnectionStatus";
            this.rtbConnectionStatus.ReadOnly = true;
            this.rtbConnectionStatus.Size = new System.Drawing.Size(141, 20);
            this.rtbConnectionStatus.TabIndex = 6;
            this.rtbConnectionStatus.Text = "";
            // 
            // rtbAction
            // 
            this.rtbAction.Location = new System.Drawing.Point(111, 31);
            this.rtbAction.Name = "rtbAction";
            this.rtbAction.ReadOnly = true;
            this.rtbAction.Size = new System.Drawing.Size(141, 20);
            this.rtbAction.TabIndex = 7;
            this.rtbAction.Text = "";
            // 
            // rtbAddress
            // 
            this.rtbAddress.Location = new System.Drawing.Point(111, 56);
            this.rtbAddress.Name = "rtbAddress";
            this.rtbAddress.ReadOnly = true;
            this.rtbAddress.Size = new System.Drawing.Size(141, 20);
            this.rtbAddress.TabIndex = 8;
            this.rtbAddress.Text = "";
            // 
            // rtbErrorCode
            // 
            this.rtbErrorCode.Location = new System.Drawing.Point(111, 81);
            this.rtbErrorCode.Name = "rtbErrorCode";
            this.rtbErrorCode.ReadOnly = true;
            this.rtbErrorCode.Size = new System.Drawing.Size(141, 20);
            this.rtbErrorCode.TabIndex = 9;
            this.rtbErrorCode.Text = "";
            // 
            // rtbErrorText
            // 
            this.rtbErrorText.Location = new System.Drawing.Point(111, 106);
            this.rtbErrorText.Name = "rtbErrorText";
            this.rtbErrorText.ReadOnly = true;
            this.rtbErrorText.Size = new System.Drawing.Size(141, 20);
            this.rtbErrorText.TabIndex = 10;
            this.rtbErrorText.Text = "";
            // 
            // rtbName
            // 
            this.rtbName.Location = new System.Drawing.Point(111, 131);
            this.rtbName.Name = "rtbName";
            this.rtbName.ReadOnly = true;
            this.rtbName.Size = new System.Drawing.Size(141, 20);
            this.rtbName.TabIndex = 11;
            this.rtbName.Text = "";
            // 
            // btnOkay
            // 
            this.btnOkay.Location = new System.Drawing.Point(258, 128);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 12;
            this.btnOkay.Text = "OK";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(258, 70);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 13;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(258, 99);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 23);
            this.btnCopy.TabIndex = 14;
            this.btnCopy.Text = "Copy Info";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // PVINetworkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 159);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.rtbName);
            this.Controls.Add(this.rtbErrorText);
            this.Controls.Add(this.rtbErrorCode);
            this.Controls.Add(this.rtbAddress);
            this.Controls.Add(this.rtbAction);
            this.Controls.Add(this.rtbConnectionStatus);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PVINetworkForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PVI Network";
            this.Load += new System.EventHandler(this.PVINetworkForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RichTextBox rtbConnectionStatus;
        private System.Windows.Forms.RichTextBox rtbAction;
        private System.Windows.Forms.RichTextBox rtbAddress;
        private System.Windows.Forms.RichTextBox rtbErrorCode;
        private System.Windows.Forms.RichTextBox rtbErrorText;
        private System.Windows.Forms.RichTextBox rtbName;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnCopy;
    }
}