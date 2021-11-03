using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Common.DataConnector;
using Common.StringUtilities;
using Modules.Utilities;
using Common.AppSettings;

namespace Modules.Transactions
{
    public partial class frmCancelPayment : Form
    {
        public frmCancelPayment()
        {
            InitializeComponent();
        }

        OracleResultSet result = new OracleResultSet();
        private string m_sAn = string.Empty;

        private void frmCancelPayment_Load(object sender, EventArgs e)
        {
            arn1.ArnCode.Enabled = true;
            this.LoadList();
        }

        private void LoadList()
        {
            result.Query = @"select distinct a.refno as arn, b.acct_code, b.acct_ln, b.acct_fn, b.acct_mi, a.bill_no, a.or_no, a.date_posted  
                            from payments_info a, account b where a.payer_code = b.acct_code";
            dgView.Rows.Clear();
            if (result.Execute())
            {
                while (result.Read())
                {
                    dgView.Rows.Add(
                        result.GetString(0),
                        result.GetString(1),
                        result.GetString(2),
                        result.GetString(3),
                        result.GetString(4),
                        result.GetString(5),
                        result.GetString(6),
                        result.GetDateTime(7).ToShortDateString());
                }
            }
            result.Close();

            lblTotalRec.Text = dgView.RowCount + " Record(s) found";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                m_sAn = arn1.GetAn();
                result.Query = @"select distinct a.refno as arn, b.acct_code, b.acct_ln, b.acct_fn, b.acct_mi, a.bill_no, a.or_no, a.date_posted  
                            from payments_info a, account b where a.payer_code = b.acct_code ";

                if (txtLastName.Text.Trim() != string.Empty ||
                    txtFirstName.Text.Trim() != string.Empty ||
                    txtMI.Text.Trim() != string.Empty)
                {
                    string strData = string.Empty;
                    result.Query += "AND b.Acct_Code IN (SELECT Acct_Code FROM Account WHERE 1=1 ";

                    if (txtLastName.Text != string.Empty)
                        strData = string.Format("AND acct_ln like '%{0}%' ", StringUtilities.HandleApostrophe(txtLastName.Text));

                    if (txtFirstName.Text != string.Empty)
                        strData = string.Format("AND acct_fn like '%{0}%' ", StringUtilities.HandleApostrophe(txtFirstName.Text));

                    if (txtMI.Text != string.Empty)
                        strData = string.Format("AND acct_mi like '%{0}%' ", StringUtilities.HandleApostrophe(txtMI.Text));

                    result.Query += ") ";
                }

                if (m_sAn.Trim() != string.Empty)
                {
                    m_sAn = arn1.GetAn();
                    result.Query += string.Format("AND a.refno = '{0}' ", StringUtilities.HandleApostrophe(m_sAn));
                }

                if (txtBillNo.Text.Trim() != string.Empty)
                    result.Query += string.Format("AND a.bill_no = '{0}' ", StringUtilities.HandleApostrophe(txtBillNo.Text));

                if (txtORNo.Text.Trim() != string.Empty)
                    result.Query += string.Format("AND a.or_no = '{0}' ", StringUtilities.HandleApostrophe(txtORNo.Text));

                if (chkInclude.CheckState == CheckState.Checked)
                {
                    result.Query += string.Format("AND a.date_posted between to_date('{0}', 'MM/dd/yyyy') AND to_date('{1}', 'MM/dd/yyyy') ", dtpFrom.Value.ToShortDateString(), dtpTo.Value.ToShortDateString());
                }

                dgView.Rows.Clear();
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        dgView.Rows.Add(
                        result.GetString(0),
                        result.GetString(1),
                        result.GetString(2),
                        result.GetString(3),
                        result.GetString(4),
                        result.GetString(5),
                        result.GetString(6),
                        result.GetDateTime(7).ToShortDateString());
                    }
                }
                result.Close();
                lblTotalRec.Text = dgView.RowCount + " Record(s) found";
            }
            catch { }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in this.grpFields.Controls) 
            {
                if (ctrl is TextBox)
                    ctrl.Text = string.Empty;
            }

            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;

            this.LoadList();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Cancel?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strARN = dgView.CurrentRow.Cells[0].Value.ToString();
                string strBillNo = dgView.CurrentRow.Cells[5].Value.ToString();
                OracleResultSet rs = new OracleResultSet();
                rs.Transaction = true;

                rs.Query = string.Format("INSERT INTO canc_payments_info SELECT * FROM payments_info WHERE refno = '{0}' and bill_no = '{1}' ", strARN, strBillNo);
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("DELETE FROM payments_info WHERE refno = '{0}' and bill_no = '{1}' ", strARN, txtBillNo.Text.Trim());
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format(@"INSERT INTO cancelled_payments
                                        SELECT arn, 
                                                or_no,
                                                or_date,
                                                bill_no,
                                                fees_code,
                                                fees_due,
                                                payment_type,
                                                mode_fld,
                                                time_posted,
                                                date_posted,
                                                '{0}' as time_cancelled,
                                                to_date('{1}', 'MM/dd/yyyy') as date_cancelled,
                                                teller_code,
                                                '{2}' as cancelled_by,
                                                memo,
                                                fees_category,
                                                permit_code
                                                FROM payments a WHERE arn = '{3}' and bill_no = '{4}' ", AppSettingsManager.GetSystemDate().ToString("HH:mm"), AppSettingsManager.GetSystemDate().ToShortDateString(), AppSettingsManager.SystemUser.UserCode.ToString() ,strARN, strBillNo);
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("DELETE FROM payments WHERE arn = '{0}' and bill_no = '{1}' ", strARN, strBillNo);
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("INSERT INTO canc_pay_tendered SELECT * FROM payments_tendered WHERE or_no = '{0}' ", txtORNo.Text.Trim());
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("DELETE FROM payments_tendered WHERE or_no = '{0}' ", txtORNo.Text.Trim());
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("INSERT INTO application_que SELECT * FROM application WHERE arn = '{0}' ", strARN);
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("DELETE FROM application WHERE arn = '{0}' ", strARN);
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("INSERT INTO taxdues SELECT * FROM taxdues_hist WHERE arn = '{0}' and bill_no = '{1}' ", strARN, strBillNo);
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("DELETE FROM taxdues_hist WHERE arn = '{0}' and bill_no = '{1}' ", strARN, strBillNo);
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("INSERT INTO billing SELECT * FROM billing_hist WHERE arn = '{0}' and bill_no = '{1}' ", strARN, strBillNo);
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("DELETE FROM billing_hist WHERE arn = '{0}' and bill_no = '{1}' ", strARN, strBillNo);
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("INSERT INTO tax_details SELECT * FROM tax_details_hist WHERE arn = '{0}' and bill_no = '{1}' ", strARN, strBillNo);
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("DELETE FROM tax_details_hist WHERE arn = '{0}' and bill_no = '{1}' ", strARN, strBillNo);
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("DELETE FROM application WHERE arn = '{0}' ", strARN);
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("DELETE FROM billing_paid WHERE arn = '{0}' ", strARN);
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("DELETE FROM taxdues_paid WHERE arn = '{0}'", strARN);
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("DELETE FROM tax_details_paid WHERE arn = '{0}'", strARN);
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("DELETE FROM payment_denom WHERE arn = '{0}'", strARN);
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("DELETE FROM debit_credit WHERE or_no = '{0}'", strARN);
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("INSERT INTO canc_chk_tbl SELECT * FROM chk_tbl WHERE or_no = '{0}' ", txtORNo.Text.Trim());
                if (rs.ExecuteNonQuery() == 0) { }

                rs.Query = string.Format("DELETE FROM chk_tbl WHERE or_no = '{0}' ", txtORNo.Text.Trim());
                if (rs.ExecuteNonQuery() == 0) { }

                if (!rs.Commit())
                {
                    rs.Rollback();
                    MessageBox.Show("Transaction cannot be cancelled", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                rs.Commit();
                MessageBox.Show("Payment cancelled.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (AuditTrail.InsertTrail("COL-P-C", "PAYMENTS", "ARN: " + strARN) == 0)
                {
                    MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkInclude_CheckedChanged(object sender, EventArgs e)
        {
            if (chkInclude.CheckState == CheckState.Checked)
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
            }
            else if (chkInclude.CheckState == CheckState.Unchecked)
            {
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;
            }
        }

        private void dgView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgView.RowCount > 0)
            {
                txtARN.Text = dgView.CurrentRow.Cells[0].Value.ToString();
                txtLastName.Text = dgView.CurrentRow.Cells[2].Value.ToString();
                txtFirstName.Text = dgView.CurrentRow.Cells[3].Value.ToString();
                txtMI.Text = dgView.CurrentRow.Cells[4].Value.ToString();
                txtBillNo.Text = dgView.CurrentRow.Cells[5].Value.ToString();
                txtORNo.Text = dgView.CurrentRow.Cells[6].Value.ToString();
            }
        }

        private void dgView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
