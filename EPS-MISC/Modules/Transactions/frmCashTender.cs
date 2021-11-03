using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modules.Transactions
{
    public partial class frmCashTender : Form
    {
        public frmCashTender()
        {
            InitializeComponent();
        }

        public string AmtDue { get; set; }
        public string ChkAmt { get; set; }
        public string CashAmt { get; set; }
        public string PrevCred { get; set; }
        public string CashTender { get; set; }
        private string m_sChange = string.Empty;
        public bool isOK = false;

        private void frmCashTender_Load(object sender, EventArgs e)
        {
            double dAmtDue = 0;
            double dChkAmt = 0;
            double dCashAmt = 0;
            double dPrevCred = 0;

            double.TryParse(AmtDue, out dAmtDue);
            double.TryParse(ChkAmt, out dChkAmt);
            double.TryParse(CashAmt, out dCashAmt);
            double.TryParse(PrevCred, out dPrevCred);

            txtAmtDue.Text = string.Format("{0:#,##0.00}", dAmtDue);
            txtChkAmt.Text = string.Format("{0:#,##0.00}", dChkAmt);
            txtCashAmt.Text = string.Format("{0:#,##0.00}", dCashAmt);
            txtPrevCred.Text = string.Format("{0:#,##0.00}", dPrevCred);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            CashTender = txtCashRendered.Text;
            isOK = true;
            this.Close();
        }

        private void txtCashRendered_TextChanged(object sender, EventArgs e)
        {
            double dTrueChange = 0;
            string sTrueChange = string.Empty;

            if (txtCashRendered.Text.Trim() == "" || txtCashRendered.Text.Trim() == ".")
                txtCashRendered.Text = "0.00";

            if (txtCashAmt.Text == "0.00")
            {
                if (txtBal.Text == "0.00")
                    txtCashRendered.ReadOnly = true;
                else
                    dTrueChange = double.Parse(txtCashRendered.Text.Trim()) - double.Parse(txtBal.Text.Trim());
            }
            else
                dTrueChange = double.Parse(txtCashRendered.Text.Trim()) - double.Parse(txtCashAmt.Text.Trim());

            sTrueChange = dTrueChange.ToString();
            lblChange.Text = string.Format("{0: #,##0.00}", dTrueChange);
            m_sChange = sTrueChange;

            if (dTrueChange >= 0)
                btnOk.Enabled = true;
            else
                btnOk.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isOK = false;
            this.Close();
        }
    }
}
