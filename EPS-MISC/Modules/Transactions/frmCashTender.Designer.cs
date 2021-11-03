namespace Modules.Transactions
{
    partial class frmCashTender
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCashTender));
            this.txtAmtDue = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.grpAmtDue = new System.Windows.Forms.GroupBox();
            this.frbBrkdown = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCashAmt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtChkAmt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPrevCred = new System.Windows.Forms.TextBox();
            this.grpBal = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtBal = new System.Windows.Forms.TextBox();
            this.grpCashRen = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCashRendered = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblChange = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.grpAmtDue.SuspendLayout();
            this.frbBrkdown.SuspendLayout();
            this.grpBal.SuspendLayout();
            this.grpCashRen.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtAmtDue
            // 
            this.txtAmtDue.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAmtDue.Enabled = false;
            this.txtAmtDue.Font = new System.Drawing.Font("Calibri", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAmtDue.Location = new System.Drawing.Point(6, 48);
            this.txtAmtDue.MaxLength = 50;
            this.txtAmtDue.MinimumSize = new System.Drawing.Size(200, 40);
            this.txtAmtDue.Name = "txtAmtDue";
            this.txtAmtDue.Size = new System.Drawing.Size(338, 43);
            this.txtAmtDue.TabIndex = 111;
            this.txtAmtDue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(117, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 23);
            this.label7.TabIndex = 112;
            this.label7.Text = "Amount Due";
            // 
            // grpAmtDue
            // 
            this.grpAmtDue.Controls.Add(this.label7);
            this.grpAmtDue.Controls.Add(this.txtAmtDue);
            this.grpAmtDue.Location = new System.Drawing.Point(12, 12);
            this.grpAmtDue.Name = "grpAmtDue";
            this.grpAmtDue.Size = new System.Drawing.Size(350, 110);
            this.grpAmtDue.TabIndex = 113;
            this.grpAmtDue.TabStop = false;
            // 
            // frbBrkdown
            // 
            this.frbBrkdown.Controls.Add(this.label3);
            this.frbBrkdown.Controls.Add(this.txtCashAmt);
            this.frbBrkdown.Controls.Add(this.label2);
            this.frbBrkdown.Controls.Add(this.txtChkAmt);
            this.frbBrkdown.Controls.Add(this.label1);
            this.frbBrkdown.Controls.Add(this.txtPrevCred);
            this.frbBrkdown.Location = new System.Drawing.Point(12, 128);
            this.frbBrkdown.Name = "frbBrkdown";
            this.frbBrkdown.Size = new System.Drawing.Size(350, 183);
            this.frbBrkdown.TabIndex = 114;
            this.frbBrkdown.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 23);
            this.label3.TabIndex = 116;
            this.label3.Text = "Cash Amount";
            // 
            // txtCashAmt
            // 
            this.txtCashAmt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCashAmt.Enabled = false;
            this.txtCashAmt.Font = new System.Drawing.Font("Calibri", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCashAmt.Location = new System.Drawing.Point(144, 120);
            this.txtCashAmt.MaxLength = 50;
            this.txtCashAmt.MinimumSize = new System.Drawing.Size(200, 40);
            this.txtCashAmt.Name = "txtCashAmt";
            this.txtCashAmt.Size = new System.Drawing.Size(200, 43);
            this.txtCashAmt.TabIndex = 115;
            this.txtCashAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 23);
            this.label2.TabIndex = 114;
            this.label2.Text = "Check Amount";
            // 
            // txtChkAmt
            // 
            this.txtChkAmt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtChkAmt.Enabled = false;
            this.txtChkAmt.Font = new System.Drawing.Font("Calibri", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChkAmt.Location = new System.Drawing.Point(144, 71);
            this.txtChkAmt.MaxLength = 50;
            this.txtChkAmt.MinimumSize = new System.Drawing.Size(200, 40);
            this.txtChkAmt.Name = "txtChkAmt";
            this.txtChkAmt.Size = new System.Drawing.Size(200, 43);
            this.txtChkAmt.TabIndex = 113;
            this.txtChkAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 23);
            this.label1.TabIndex = 112;
            this.label1.Text = "Previous Credit";
            // 
            // txtPrevCred
            // 
            this.txtPrevCred.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPrevCred.Enabled = false;
            this.txtPrevCred.Font = new System.Drawing.Font("Calibri", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrevCred.Location = new System.Drawing.Point(144, 22);
            this.txtPrevCred.MaxLength = 50;
            this.txtPrevCred.MinimumSize = new System.Drawing.Size(200, 40);
            this.txtPrevCred.Name = "txtPrevCred";
            this.txtPrevCred.Size = new System.Drawing.Size(200, 43);
            this.txtPrevCred.TabIndex = 111;
            this.txtPrevCred.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // grpBal
            // 
            this.grpBal.Controls.Add(this.label6);
            this.grpBal.Controls.Add(this.txtBal);
            this.grpBal.Location = new System.Drawing.Point(12, 310);
            this.grpBal.Name = "grpBal";
            this.grpBal.Size = new System.Drawing.Size(350, 83);
            this.grpBal.TabIndex = 117;
            this.grpBal.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 23);
            this.label6.TabIndex = 112;
            this.label6.Text = "Balance";
            // 
            // txtBal
            // 
            this.txtBal.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBal.Enabled = false;
            this.txtBal.Font = new System.Drawing.Font("Calibri", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBal.Location = new System.Drawing.Point(144, 22);
            this.txtBal.MaxLength = 50;
            this.txtBal.MinimumSize = new System.Drawing.Size(200, 40);
            this.txtBal.Name = "txtBal";
            this.txtBal.Size = new System.Drawing.Size(200, 43);
            this.txtBal.TabIndex = 111;
            this.txtBal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // grpCashRen
            // 
            this.grpCashRen.Controls.Add(this.label4);
            this.grpCashRen.Controls.Add(this.txtCashRendered);
            this.grpCashRen.Location = new System.Drawing.Point(12, 392);
            this.grpCashRen.Name = "grpCashRen";
            this.grpCashRen.Size = new System.Drawing.Size(350, 83);
            this.grpCashRen.TabIndex = 118;
            this.grpCashRen.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 23);
            this.label4.TabIndex = 112;
            this.label4.Text = "Cash Rendered";
            // 
            // txtCashRendered
            // 
            this.txtCashRendered.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCashRendered.Font = new System.Drawing.Font("Calibri", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCashRendered.Location = new System.Drawing.Point(144, 22);
            this.txtCashRendered.MaxLength = 50;
            this.txtCashRendered.MinimumSize = new System.Drawing.Size(200, 40);
            this.txtCashRendered.Name = "txtCashRendered";
            this.txtCashRendered.Size = new System.Drawing.Size(200, 43);
            this.txtCashRendered.TabIndex = 111;
            this.txtCashRendered.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCashRendered.TextChanged += new System.EventHandler(this.txtCashRendered_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(18, 487);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 23);
            this.label5.TabIndex = 113;
            this.label5.Text = "Change";
            // 
            // lblChange
            // 
            this.lblChange.AutoSize = true;
            this.lblChange.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChange.Location = new System.Drawing.Point(162, 487);
            this.lblChange.MinimumSize = new System.Drawing.Size(200, 23);
            this.lblChange.Name = "lblChange";
            this.lblChange.Size = new System.Drawing.Size(200, 23);
            this.lblChange.TabIndex = 119;
            this.lblChange.Text = "0";
            this.lblChange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(261, 526);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 33);
            this.btnCancel.TabIndex = 120;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(154, 526);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(101, 33);
            this.btnOk.TabIndex = 121;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmCashTender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleTurquoise;
            this.ClientSize = new System.Drawing.Size(374, 567);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblChange);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.grpCashRen);
            this.Controls.Add(this.grpBal);
            this.Controls.Add(this.frbBrkdown);
            this.Controls.Add(this.grpAmtDue);
            this.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmCashTender";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cash Tender";
            this.Load += new System.EventHandler(this.frmCashTender_Load);
            this.grpAmtDue.ResumeLayout(false);
            this.grpAmtDue.PerformLayout();
            this.frbBrkdown.ResumeLayout(false);
            this.frbBrkdown.PerformLayout();
            this.grpBal.ResumeLayout(false);
            this.grpBal.PerformLayout();
            this.grpCashRen.ResumeLayout(false);
            this.grpCashRen.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtAmtDue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox grpAmtDue;
        private System.Windows.Forms.GroupBox frbBrkdown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCashAmt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtChkAmt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPrevCred;
        private System.Windows.Forms.GroupBox grpBal;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtBal;
        private System.Windows.Forms.GroupBox grpCashRen;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCashRendered;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblChange;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
    }
}