namespace Modules.Reports
{
    partial class frmReprintOR
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReprintOR));
            this.txtOR = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbFormType = new System.Windows.Forms.ComboBox();
            this.btnRetrieveInfo = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.grpOR = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.grpPayer = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAddr = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtAmt = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtBankAddr = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtBank = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtChkNo = new System.Windows.Forms.TextBox();
            this.dgvFees = new System.Windows.Forms.DataGridView();
            this.Particulars = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Due = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Surcharge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtGrandTotal = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtTeller = new System.Windows.Forms.TextBox();
            this.grpOR.SuspendLayout();
            this.grpPayer.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFees)).BeginInit();
            this.SuspendLayout();
            // 
            // txtOR
            // 
            this.txtOR.Location = new System.Drawing.Point(6, 43);
            this.txtOR.Name = "txtOR";
            this.txtOR.Size = new System.Drawing.Size(123, 26);
            this.txtOR.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "O.R. No.";
            // 
            // cmbFormType
            // 
            this.cmbFormType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFormType.FormattingEnabled = true;
            this.cmbFormType.Location = new System.Drawing.Point(135, 43);
            this.cmbFormType.Name = "cmbFormType";
            this.cmbFormType.Size = new System.Drawing.Size(85, 26);
            this.cmbFormType.TabIndex = 2;
            // 
            // btnRetrieveInfo
            // 
            this.btnRetrieveInfo.Location = new System.Drawing.Point(226, 42);
            this.btnRetrieveInfo.Name = "btnRetrieveInfo";
            this.btnRetrieveInfo.Size = new System.Drawing.Size(76, 27);
            this.btnRetrieveInfo.TabIndex = 3;
            this.btnRetrieveInfo.Text = "Retrieve Info";
            this.btnRetrieveInfo.UseVisualStyleBackColor = true;
            this.btnRetrieveInfo.Click += new System.EventHandler(this.btnRetrieveInfo_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(132, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "Form Type";
            // 
            // grpOR
            // 
            this.grpOR.Controls.Add(this.label3);
            this.grpOR.Controls.Add(this.dateTimePicker1);
            this.grpOR.Controls.Add(this.label1);
            this.grpOR.Controls.Add(this.label2);
            this.grpOR.Controls.Add(this.txtOR);
            this.grpOR.Controls.Add(this.btnRetrieveInfo);
            this.grpOR.Controls.Add(this.cmbFormType);
            this.grpOR.Location = new System.Drawing.Point(12, -2);
            this.grpOR.Name = "grpOR";
            this.grpOR.Size = new System.Drawing.Size(594, 95);
            this.grpOR.TabIndex = 5;
            this.grpOR.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(473, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 18);
            this.label3.TabIndex = 6;
            this.label3.Text = "OR Date";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Enabled = false;
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(476, 40);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(109, 26);
            this.dateTimePicker1.TabIndex = 5;
            // 
            // grpPayer
            // 
            this.grpPayer.Controls.Add(this.label5);
            this.grpPayer.Controls.Add(this.txtAddr);
            this.grpPayer.Controls.Add(this.label4);
            this.grpPayer.Controls.Add(this.txtName);
            this.grpPayer.Enabled = false;
            this.grpPayer.Location = new System.Drawing.Point(12, 93);
            this.grpPayer.Name = "grpPayer";
            this.grpPayer.Size = new System.Drawing.Size(594, 105);
            this.grpPayer.TabIndex = 6;
            this.grpPayer.TabStop = false;
            this.grpPayer.Text = "Payer\'s Information";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 18);
            this.label5.TabIndex = 8;
            this.label5.Text = "Address";
            // 
            // txtAddr
            // 
            this.txtAddr.Location = new System.Drawing.Point(75, 57);
            this.txtAddr.Name = "txtAddr";
            this.txtAddr.Size = new System.Drawing.Size(510, 26);
            this.txtAddr.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(75, 25);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(510, 26);
            this.txtName.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtAmt);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtBankAddr);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtBank);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtChkNo);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(10, 197);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(594, 105);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Check Info";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(288, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 18);
            this.label9.TabIndex = 12;
            this.label9.Text = "Amount";
            // 
            // txtAmt
            // 
            this.txtAmt.Location = new System.Drawing.Point(381, 25);
            this.txtAmt.Name = "txtAmt";
            this.txtAmt.Size = new System.Drawing.Size(207, 26);
            this.txtAmt.TabIndex = 13;
            this.txtAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(288, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 18);
            this.label8.TabIndex = 10;
            this.label8.Text = "Bank Address";
            // 
            // txtBankAddr
            // 
            this.txtBankAddr.Location = new System.Drawing.Point(380, 57);
            this.txtBankAddr.Name = "txtBankAddr";
            this.txtBankAddr.Size = new System.Drawing.Size(208, 26);
            this.txtBankAddr.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 18);
            this.label6.TabIndex = 8;
            this.label6.Text = "Bank";
            // 
            // txtBank
            // 
            this.txtBank.Location = new System.Drawing.Point(75, 57);
            this.txtBank.Name = "txtBank";
            this.txtBank.Size = new System.Drawing.Size(207, 26);
            this.txtBank.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 18);
            this.label7.TabIndex = 7;
            this.label7.Text = "Check No";
            // 
            // txtChkNo
            // 
            this.txtChkNo.Location = new System.Drawing.Point(75, 25);
            this.txtChkNo.Name = "txtChkNo";
            this.txtChkNo.Size = new System.Drawing.Size(207, 26);
            this.txtChkNo.TabIndex = 7;
            // 
            // dgvFees
            // 
            this.dgvFees.AllowUserToAddRows = false;
            this.dgvFees.AllowUserToDeleteRows = false;
            this.dgvFees.AllowUserToOrderColumns = true;
            this.dgvFees.AllowUserToResizeRows = false;
            this.dgvFees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFees.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Particulars,
            this.Due,
            this.Surcharge,
            this.Total});
            this.dgvFees.Location = new System.Drawing.Point(12, 307);
            this.dgvFees.MultiSelect = false;
            this.dgvFees.Name = "dgvFees";
            this.dgvFees.ReadOnly = true;
            this.dgvFees.RowHeadersVisible = false;
            this.dgvFees.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFees.Size = new System.Drawing.Size(585, 150);
            this.dgvFees.TabIndex = 11;
            // 
            // Particulars
            // 
            this.Particulars.HeaderText = "Particulars";
            this.Particulars.Name = "Particulars";
            this.Particulars.ReadOnly = true;
            this.Particulars.Width = 250;
            // 
            // Due
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.Due.DefaultCellStyle = dataGridViewCellStyle1;
            this.Due.HeaderText = "Due";
            this.Due.Name = "Due";
            this.Due.ReadOnly = true;
            this.Due.Width = 110;
            // 
            // Surcharge
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.Surcharge.DefaultCellStyle = dataGridViewCellStyle2;
            this.Surcharge.HeaderText = "Surcharge";
            this.Surcharge.Name = "Surcharge";
            this.Surcharge.ReadOnly = true;
            this.Surcharge.Width = 110;
            // 
            // Total
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.Total.DefaultCellStyle = dataGridViewCellStyle3;
            this.Total.HeaderText = "Total";
            this.Total.Name = "Total";
            this.Total.ReadOnly = true;
            this.Total.Width = 110;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(439, 505);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(76, 27);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "Print OR";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(521, 505);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(76, 27);
            this.btnExit.TabIndex = 12;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(388, 466);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 18);
            this.label10.TabIndex = 14;
            this.label10.Text = "Total";
            // 
            // txtGrandTotal
            // 
            this.txtGrandTotal.Enabled = false;
            this.txtGrandTotal.Location = new System.Drawing.Point(430, 463);
            this.txtGrandTotal.Name = "txtGrandTotal";
            this.txtGrandTotal.Size = new System.Drawing.Size(167, 26);
            this.txtGrandTotal.TabIndex = 15;
            this.txtGrandTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 466);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 18);
            this.label11.TabIndex = 14;
            this.label11.Text = "Teller";
            // 
            // txtTeller
            // 
            this.txtTeller.Enabled = false;
            this.txtTeller.Location = new System.Drawing.Point(53, 463);
            this.txtTeller.Name = "txtTeller";
            this.txtTeller.Size = new System.Drawing.Size(163, 26);
            this.txtTeller.TabIndex = 15;
            // 
            // frmReprintOR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleTurquoise;
            this.ClientSize = new System.Drawing.Size(616, 542);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtTeller);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.txtGrandTotal);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.dgvFees);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpPayer);
            this.Controls.Add(this.grpOR);
            this.Font = new System.Drawing.Font("Calibri", 11.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmReprintOR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reprint OR";
            this.Load += new System.EventHandler(this.frmReprintOR_Load);
            this.grpOR.ResumeLayout(false);
            this.grpOR.PerformLayout();
            this.grpPayer.ResumeLayout(false);
            this.grpPayer.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFees)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOR;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbFormType;
        private System.Windows.Forms.Button btnRetrieveInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox grpOR;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.GroupBox grpPayer;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAddr;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtBank;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtChkNo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtAmt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBankAddr;
        private System.Windows.Forms.DataGridView dgvFees;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtGrandTotal;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtTeller;
        private System.Windows.Forms.DataGridViewTextBoxColumn Particulars;
        private System.Windows.Forms.DataGridViewTextBoxColumn Due;
        private System.Windows.Forms.DataGridViewTextBoxColumn Surcharge;
        private System.Windows.Forms.DataGridViewTextBoxColumn Total;
    }
}