namespace Modules.Utilities.Forms
{
    partial class frmORDeclaration
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
            this.dgvTellerList = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMI = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFN = new System.Windows.Forms.TextBox();
            this.txtLN = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtTellerCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbFormType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCurrOR = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtORTo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvORRanges = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtORFrom = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnDeclare = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.Teller = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Form = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.To = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateAssigned = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AssignedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastOR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTellerList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvORRanges)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTellerList
            // 
            this.dgvTellerList.AllowUserToAddRows = false;
            this.dgvTellerList.AllowUserToDeleteRows = false;
            this.dgvTellerList.AllowUserToResizeRows = false;
            this.dgvTellerList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTellerList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Teller,
            this.Form,
            this.From,
            this.To,
            this.DateAssigned,
            this.AssignedBy,
            this.LastName,
            this.FirstName,
            this.MI,
            this.LastOR});
            this.dgvTellerList.Location = new System.Drawing.Point(12, 12);
            this.dgvTellerList.MultiSelect = false;
            this.dgvTellerList.Name = "dgvTellerList";
            this.dgvTellerList.RowHeadersVisible = false;
            this.dgvTellerList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTellerList.Size = new System.Drawing.Size(466, 134);
            this.dgvTellerList.TabIndex = 0;
            this.dgvTellerList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTellerList_CellClck);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtMI);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtFN);
            this.groupBox1.Controls.Add(this.txtLN);
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtTellerCode);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbFormType);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(12, 152);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(466, 169);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Teller\'s Information";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(282, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 18);
            this.label4.TabIndex = 114;
            this.label4.Text = "MI";
            // 
            // txtMI
            // 
            this.txtMI.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMI.Location = new System.Drawing.Point(312, 123);
            this.txtMI.MaxLength = 50;
            this.txtMI.Name = "txtMI";
            this.txtMI.Size = new System.Drawing.Size(37, 26);
            this.txtMI.TabIndex = 113;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 18);
            this.label3.TabIndex = 112;
            this.label3.Text = "First Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 18);
            this.label2.TabIndex = 111;
            this.label2.Text = "Last Name";
            // 
            // txtFN
            // 
            this.txtFN.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFN.Location = new System.Drawing.Point(90, 123);
            this.txtFN.MaxLength = 50;
            this.txtFN.Name = "txtFN";
            this.txtFN.Size = new System.Drawing.Size(186, 26);
            this.txtFN.TabIndex = 110;
            // 
            // txtLN
            // 
            this.txtLN.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLN.Location = new System.Drawing.Point(90, 91);
            this.txtLN.MaxLength = 50;
            this.txtLN.Name = "txtLN";
            this.txtLN.Size = new System.Drawing.Size(186, 26);
            this.txtLN.TabIndex = 109;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(282, 59);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(67, 26);
            this.btnClear.TabIndex = 108;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(209, 59);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(67, 26);
            this.btnSearch.TabIndex = 107;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtTellerCode
            // 
            this.txtTellerCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTellerCode.Location = new System.Drawing.Point(90, 59);
            this.txtTellerCode.MaxLength = 50;
            this.txtTellerCode.Name = "txtTellerCode";
            this.txtTellerCode.Size = new System.Drawing.Size(113, 26);
            this.txtTellerCode.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 18);
            this.label1.TabIndex = 11;
            this.label1.Text = "Teller Code";
            // 
            // cmbFormType
            // 
            this.cmbFormType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFormType.FormattingEnabled = true;
            this.cmbFormType.Location = new System.Drawing.Point(90, 27);
            this.cmbFormType.Name = "cmbFormType";
            this.cmbFormType.Size = new System.Drawing.Size(113, 26);
            this.cmbFormType.TabIndex = 10;
            this.cmbFormType.SelectedIndexChanged += new System.EventHandler(this.cmbFormType_SelectedIndexChanged);
            this.cmbFormType.SelectedValueChanged += new System.EventHandler(this.cmbFormType_SelectedValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 18);
            this.label7.TabIndex = 9;
            this.label7.Text = "Form Type";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtCurrOR);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtORTo);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.dgvORRanges);
            this.groupBox2.Controls.Add(this.txtORFrom);
            this.groupBox2.Location = new System.Drawing.Point(11, 327);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(467, 208);
            this.groupBox2.TabIndex = 115;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Available OR Ranges";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(298, 173);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 18);
            this.label8.TabIndex = 120;
            this.label8.Text = "Curr. OR";
            // 
            // txtCurrOR
            // 
            this.txtCurrOR.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCurrOR.Location = new System.Drawing.Point(357, 170);
            this.txtCurrOR.MaxLength = 50;
            this.txtCurrOR.Name = "txtCurrOR";
            this.txtCurrOR.Size = new System.Drawing.Size(104, 26);
            this.txtCurrOR.TabIndex = 119;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(163, 173);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 18);
            this.label6.TabIndex = 118;
            this.label6.Text = "To";
            // 
            // txtORTo
            // 
            this.txtORTo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtORTo.Location = new System.Drawing.Point(191, 170);
            this.txtORTo.MaxLength = 11;
            this.txtORTo.Name = "txtORTo";
            this.txtORTo.Size = new System.Drawing.Size(104, 26);
            this.txtORTo.TabIndex = 117;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 173);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 18);
            this.label5.TabIndex = 116;
            this.label5.Text = "From";
            // 
            // dgvORRanges
            // 
            this.dgvORRanges.AllowUserToAddRows = false;
            this.dgvORRanges.AllowUserToDeleteRows = false;
            this.dgvORRanges.AllowUserToOrderColumns = true;
            this.dgvORRanges.AllowUserToResizeRows = false;
            this.dgvORRanges.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvORRanges.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dgvORRanges.Location = new System.Drawing.Point(6, 25);
            this.dgvORRanges.MultiSelect = false;
            this.dgvORRanges.Name = "dgvORRanges";
            this.dgvORRanges.RowHeadersVisible = false;
            this.dgvORRanges.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvORRanges.Size = new System.Drawing.Size(455, 134);
            this.dgvORRanges.TabIndex = 116;
            this.dgvORRanges.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvORRanges_CellClicked);
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Form";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "From";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 120;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "To";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 120;
            // 
            // txtORFrom
            // 
            this.txtORFrom.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtORFrom.Location = new System.Drawing.Point(53, 170);
            this.txtORFrom.MaxLength = 11;
            this.txtORFrom.Name = "txtORFrom";
            this.txtORFrom.Size = new System.Drawing.Size(104, 26);
            this.txtORFrom.TabIndex = 115;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(381, 540);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(97, 33);
            this.btnExit.TabIndex = 119;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnDeclare
            // 
            this.btnDeclare.Location = new System.Drawing.Point(177, 541);
            this.btnDeclare.Name = "btnDeclare";
            this.btnDeclare.Size = new System.Drawing.Size(97, 33);
            this.btnDeclare.TabIndex = 117;
            this.btnDeclare.Text = "Declare";
            this.btnDeclare.UseVisualStyleBackColor = true;
            this.btnDeclare.Click += new System.EventHandler(this.btnDeclare_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.Location = new System.Drawing.Point(278, 541);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(97, 33);
            this.btnReturn.TabIndex = 118;
            this.btnReturn.Text = "Return";
            this.btnReturn.UseVisualStyleBackColor = true;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // Teller
            // 
            this.Teller.HeaderText = "Teller";
            this.Teller.Name = "Teller";
            // 
            // Form
            // 
            this.Form.HeaderText = "Form";
            this.Form.Name = "Form";
            // 
            // From
            // 
            this.From.HeaderText = "From";
            this.From.Name = "From";
            this.From.Width = 120;
            // 
            // To
            // 
            this.To.HeaderText = "To";
            this.To.Name = "To";
            this.To.Width = 120;
            // 
            // DateAssigned
            // 
            this.DateAssigned.HeaderText = "Date Assigned";
            this.DateAssigned.Name = "DateAssigned";
            // 
            // AssignedBy
            // 
            this.AssignedBy.HeaderText = "Assigned By";
            this.AssignedBy.Name = "AssignedBy";
            // 
            // LastName
            // 
            this.LastName.HeaderText = "TellerLN";
            this.LastName.Name = "LastName";
            this.LastName.Visible = false;
            // 
            // FirstName
            // 
            this.FirstName.HeaderText = "TellerFN";
            this.FirstName.Name = "FirstName";
            this.FirstName.Visible = false;
            // 
            // MI
            // 
            this.MI.HeaderText = "TellerMI";
            this.MI.Name = "MI";
            this.MI.Visible = false;
            // 
            // LastOR
            // 
            this.LastOR.HeaderText = "LastOR";
            this.LastOR.Name = "LastOR";
            this.LastOR.Visible = false;
            // 
            // frmORDeclaration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleTurquoise;
            this.ClientSize = new System.Drawing.Size(490, 585);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnDeclare);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvTellerList);
            this.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmORDeclaration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OR Declaration";
            this.Load += new System.EventHandler(this.frmORDeclaration_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTellerList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvORRanges)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTellerList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbFormType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTellerCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMI;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFN;
        private System.Windows.Forms.TextBox txtLN;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtORTo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgvORRanges;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.TextBox txtORFrom;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnDeclare;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCurrOR;
        private System.Windows.Forms.DataGridViewTextBoxColumn Teller;
        private System.Windows.Forms.DataGridViewTextBoxColumn Form;
        private System.Windows.Forms.DataGridViewTextBoxColumn From;
        private System.Windows.Forms.DataGridViewTextBoxColumn To;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateAssigned;
        private System.Windows.Forms.DataGridViewTextBoxColumn AssignedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn MI;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastOR;
    }
}