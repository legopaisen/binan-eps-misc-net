using Common.AppSettings;
using Common.DataConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modules.Reports
{
    public partial class frmReprintOR : Form
    {
        public frmReprintOR()
        {
            InitializeComponent();
        }

        private string sBillNo = string.Empty;
        private string m_sAn = string.Empty;
        private DateTime dtOR;

        private void frmReprintOR_Load(object sender, EventArgs e)
        {
            LoadFormType();
            cmbFormType.SelectedIndex = 0;
            ClearControls();
        }
        
        private void LoadFormType()
        {
            cmbFormType.Items.Clear();
            OracleResultSet res = new OracleResultSet();
            res.Query = "select form_fld from form_tbl order by form_fld";
            if(res.Execute())
                while(res.Read())
                {
                    cmbFormType.Items.Add(res.GetString("form_fld"));
                }
            res.Close();
        }

        private void ClearControls()
        {
            txtName.Text = string.Empty;
            txtAddr.Text = string.Empty;
            txtChkNo.Text = string.Empty;
            txtBank.Text = string.Empty;
            txtAmt.Text = string.Empty;
            txtBankAddr.Text = string.Empty;
            btnPrint.Enabled = false;
            dgvFees.Rows.Clear();
        }

        private bool ValidateOR()
        {
            OracleResultSet res = new OracleResultSet();
            int iCnt = 0;
            res.Query = $"select count(*) from payments_info where or_no = '{txtOR.Text.Trim()}'";
            int.TryParse(res.ExecuteScalar(), out iCnt);
            if (iCnt == 0)
                return false;
            else
                return true;
        }

        private void btnRetrieveInfo_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtOR.Text.Trim()) && string.IsNullOrEmpty(cmbFormType.Text))
            {
                MessageBox.Show("Input OR and form type!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if(!ValidateOR())
            {
                MessageBox.Show("No O.R. found!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                ClearControls();
                return;
            }

            ClearControls();
            string sPermitCode = string.Empty;
            string sPermitDesc = string.Empty;
            string sFees = string.Empty;
            string sFeesDesc = string.Empty;
            string sCat = string.Empty;
            string sTeller = string.Empty;
            string sPayer = string.Empty;
            int iRow = 0;
            int iRow2 = 0;
            double dFeesAmt = 0;
            double dFeesAmt2 = 0;
            double dSurch = 0;
            double dFeesTotal = 0;
            double dGrandTotalAmt = 0;
            bool blnDisplay = false;
            OracleResultSet res = new OracleResultSet();
            OracleResultSet res2 = new OracleResultSet();
            res.Query = $"select distinct fees_category, permit_code, fees_surch, fees_amt_due, sum(fees_due) as fees_due from payments_info where or_no = '{txtOR.Text.Trim()}' and fees_category = 'MAIN' group by fees_category, permit_code, fees_surch, fees_amt_due order by permit_code";
            if(res.Execute())
            {
                if (res.Read())
                {
                    while (res.Read())
                    {
                        dgvFees.Rows.Add();
                        iRow2 = iRow;
                        sFees = string.Empty;
                        sFeesDesc = string.Empty;
                        sCat = string.Empty;
                        dFeesAmt = 0;
                        dFeesAmt2 = 0;
                        sPermitCode = res.GetString("permit_code");
                        sPermitDesc = AppSettingsManager.GetPermitDesc(sPermitCode);

                        sCat = res.GetString("fees_category");
                        dFeesAmt = res.GetDouble("fees_due");
                        dSurch = res.GetDouble("fees_surch");

                        dgvFees[0, iRow].Value = sPermitDesc;
                        //dgvFees[1, iRow].Value = dFeesAmt;

                        dgvFees[2, iRow].Value = dSurch;


                        //other fees - not displayed
                        dFeesAmt2 = dFeesAmt;
                        res2.Query = $"select distinct fees_category, permit_code, fees_surch, fees_amt_due, fees_code, sum(fees_due) as fees_due from payments_info where or_no = '{txtOR.Text.Trim()}' and fees_category <> 'MAIN' and permit_code = '{sPermitCode}' group by fees_category, permit_code, fees_surch, fees_amt_due, fees_code order by permit_code";
                        if (res2.Execute())
                            while (res2.Read())
                            {
                                sFees = res2.GetString("fees_code");
                                sCat = res2.GetString("fees_category");
                                sFeesDesc = AppSettingsManager.GetFeesDesc(sCat, sFees);
                                dFeesAmt = res2.GetDouble("fees_due");

                                blnDisplay = AppSettingsManager.FeesDisplayOnly(sCat, sFees);
                                if (blnDisplay == false) //skip display of fees if false
                                {
                                    dFeesAmt2 += dFeesAmt;
                                    dgvFees[1, iRow].Value = dFeesAmt2;
                                }

                            }
                        res2.Close();

                        //other fees - displayed
                        res2.Query = $"select distinct fees_category, permit_code, fees_surch, fees_amt_due, fees_code, sum(fees_due) as fees_due from payments_info where or_no = '{txtOR.Text.Trim()}' and fees_category <> 'MAIN' and permit_code = '{sPermitCode}' group by fees_category, permit_code, fees_surch, fees_amt_due, fees_code order by permit_code";
                        if (res2.Execute())
                            while (res2.Read())
                            {
                                sFees = res2.GetString("fees_code");
                                sCat = res2.GetString("fees_category");
                                sFeesDesc = AppSettingsManager.GetFeesDesc(sCat, sFees);
                                dFeesAmt = res2.GetDouble("fees_due");

                                blnDisplay = AppSettingsManager.FeesDisplayOnly(sCat, sFees);
                                if (blnDisplay == true) //skip display of fees if false
                                {
                                    dgvFees.Rows.Add();
                                    iRow2++;
                                    dgvFees[0, iRow2].Value = sFeesDesc;
                                    dgvFees[1, iRow2].Value = dFeesAmt;
                                    dgvFees[2, iRow2].Value = 0;
                                    dgvFees[3, iRow2].Value = dFeesAmt;
                                }

                            }
                        res2.Close();
                        dgvFees[1, iRow].Value = dFeesAmt2;

                        dFeesTotal = dFeesAmt2 + dSurch;
                        dgvFees[3, iRow].Value = dFeesTotal;

                        iRow++;
                    }
                }
                else //AFM 20211123 requested by binan as per rj - allow billing of additional fees only on any permit
                // proceeding this condition means additional fees are only billed on permit
                {
                    res.Query = $"select distinct fees_category, permit_code, fees_surch, fees_amt_due, sum(fees_due) as fees_due from payments_info where or_no = '{txtOR.Text.Trim()}' and fees_category = 'ADDITIONAL' group by fees_category, permit_code, fees_surch, fees_amt_due order by permit_code";
                    if(res.Execute())
                        if(res.Read())
                        {
                            dgvFees.Rows.Add();
                            iRow2 = iRow;
                            sFees = string.Empty;
                            sFeesDesc = string.Empty;
                            sCat = string.Empty;
                            dFeesAmt = 0;
                            dFeesAmt2 = 0;
                            sPermitCode = res.GetString("permit_code");
                            sPermitDesc = AppSettingsManager.GetPermitDesc(sPermitCode);

                            sCat = res.GetString("fees_category");
                            dFeesAmt = res.GetDouble("fees_due");
                            dSurch = res.GetDouble("fees_surch");
                            dFeesTotal = dFeesAmt + dSurch;

                            dgvFees[0, iRow].Value = sPermitDesc;
                            dgvFees[1, iRow].Value = dFeesAmt;
                            dgvFees[2, iRow].Value = dSurch;
                            dgvFees[3, iRow].Value = dFeesTotal;
                        }

                }

            }
                
            res.Close();

            res.Query = $"select distinct teller_code, bill_no, refno, or_date, fees_amt_due, payer_code from payments_info where or_no = '{txtOR.Text.Trim()}'";
            if(res.Execute())
                if(res.Read())
                {
                    txtTeller.Text = res.GetString("teller_code");
                    sBillNo = res.GetString("bill_no");
                    m_sAn = res.GetString("refno");
                    dtOR = res.GetDateTime("or_date");
                    sPayer = res.GetString("payer_code");
                    txtGrandTotal.Text = string.Format("{0:#,##0.00}", res.GetDouble("fees_amt_due"));
                }
            res.Close();

            res.Query = $"select * from account where acct_code = '{sPayer}'";
            if(res.Execute())
                if(res.Read())
                {
                    txtName.Text = res.GetString("acct_fn") + " " + res.GetString("acct_mi") + ". " + res.GetString("acct_ln");
                    txtAddr.Text = res.GetString("acct_hse_no") + ", " + res.GetString("acct_lot_no") + ", " + res.GetString("acct_blk_no") + ", " + res.GetString("acct_addr") + ", " + res.GetString("acct_brgy") + ", " + res.GetString("acct_city") + ", " + res.GetString("acct_prov");
                }
            res.Close();

            res.Query = $"select * from chk_tbl where or_no = '{txtOR.Text.Trim()}'";
            if(res.Execute())
                if(res.Read())
                {
                    txtChkNo.Text = res.GetString("chk_no");
                    txtBank.Text = AppSettingsManager.GetBankName(res.GetString("bank_code"));
                    txtBankAddr.Text = AppSettingsManager.GetBankAddress(res.GetString("bank_code"));
                    txtAmt.Text = string.Format("{0:#,##0.00}", res.GetDouble("chk_amt"));
                }
            btnPrint.Enabled = true;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            frmReport form = new frmReport();
            form.ReportName = "PRINT OR";
            form.OR = txtOR.Text.Trim();
            form.BillNo = sBillNo;
            form.dtOR = dtOR;
            form.An = m_sAn;
            form.Teller = txtTeller.Text.Trim();
            form.ShowDialog();

            ClearControls();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
