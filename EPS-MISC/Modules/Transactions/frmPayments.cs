using Common.AppSettings;
using Common.DataConnector;
using EPSEntities.Connection;
using EPSEntities.Entity;
using Modules.Reports;
using Modules.Utilities;
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
    public partial class frmPayments : Form
    {
        public frmPayments()
        {
            InitializeComponent();
        }

        public static ConnectionString dbConn = new ConnectionString();
        TaskManager taskman = new TaskManager();

        public string m_sAN = string.Empty;
        public string m_sModule = string.Empty;
        public string SearchCriteria = string.Empty;
        protected string m_sProjOwner = string.Empty;
        public string  m_sTeller = string.Empty;
        private double dCashTender = 0;
        private string sPaymentType = string.Empty;
        private bool blnAllUsed = false;

        private void frmPayments_Load(object sender, EventArgs e)
        {
            cmbMode.Items.Add("ONLINE");
            cmbMode.Items.Add("OFFLINE");
            m_sModule = "PAYMENT-ONLINE";
            cmbMode.SelectedIndex = 0; //set to ONLINE
            this.an1.ArnCode.Enabled = true;
            EnableControls(false);
            LoadCurrentOR(m_sTeller);
        }

        private void LoadCurrentOR(string sTeller)
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = $"select cur_or_no from or_current where teller_code = '{sTeller}'";
            if(result.Execute())
                if(result.Read())
                {
                    txtCurrOR.Text = result.GetString("cur_or_no");
                }
            result.Close();
        }

        private void EnableControls(bool bln)
        {
            grpApplicationInfo.Enabled = false;
            txtCurrOR.ReadOnly = false;
            dtOR.Enabled = false;
            grpPayType.Enabled = bln;
            dgvTaxDues.ReadOnly = true;
            grpMemo.Enabled = bln;
            grpTotal.Enabled = bln;
        }

        private void ClearControls()
        {
            taskman.RemTask(m_sAN);
            txtBillNo.Text = string.Empty;
            txtProjDesc.Text = string.Empty;
            txtProjLoc.Text = string.Empty;
            txtProjOwn.Text = string.Empty;
            txtOccupancy.Text = string.Empty;
            txtAppDate.Text = string.Empty;
            txtCashAmt.Text = string.Empty;
            txtChkAmt.Text = string.Empty;
            txtCredBal.Text = string.Empty;
            txtNewCredit.Text = string.Empty;
            txtTaxCredLess.Text = string.Empty;
            txtGrandTotal.Text = string.Empty;
            txtMemo.Text = string.Empty;
            m_sAN = string.Empty;
            an1.Clear();

            chkCash.Checked = false;
            chkCheque.Checked = false;

            dtOR.Text = AppSettingsManager.GetSystemDate().ToShortDateString();
            dgvTaxDues.Rows.Clear();
            dgvTaxDuesInfo.Rows.Clear();

            m_sProjOwner = string.Empty;
            btnExit.Text = "Exit";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (btnExit.Text == "Exit")
            {
                taskman.RemTask(m_sAN);
                this.Close();
            }
            else
            {

                OracleResultSet res = new OracleResultSet();
                res.Query = $"DELETE FROM CHK_TBL_TEMP WHERE OR_NO = '{txtCurrOR.Text.Trim()}'";
                if(res.ExecuteNonQuery() == 0)
                { }
                res.Close();

                ClearControls();
                EnableControls(false);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            m_sAN = an1.GetAn();
            TaskManager taskman = new TaskManager();

            if (string.IsNullOrEmpty(m_sAN))
            {
                SearchAccount.frmSearchARN form = new SearchAccount.frmSearchARN();
                if(SearchCriteria == "ELEC-OCC")
                    form.SearchCriteria = "QUE-ELEC-OCC";
                else
                    form.SearchCriteria = "QUE";

                form.ShowDialog();

                an1.SetAn(form.sArn);

                m_sAN = an1.GetAn();
            }
            else
                m_sAN = an1.GetAn();

            if (string.IsNullOrEmpty(m_sAN))
                return;

            if (!taskman.AddTask(m_sModule, m_sAN))
                return;

            if (!DisplayData())
            {

                ClearControls();
                return;
            }
            EnableControls(true);
        }

        private bool DisplayData()
        {
            var db = new EPSConnection(dbConn);
            DateTime dtApplied;
            string strWhereCond = string.Empty;
            int iRecCount = 0;
            var result = (dynamic)null;

            //strWhereCond = $" where arn = '{m_sAN}' and main_application = 1";
            strWhereCond = $" where arn = '{m_sAN}'";
            result = from a in Records.ApplicationQueList.GetApplicationQue(strWhereCond)
                     select a;

            foreach(var item in result)
            {
                iRecCount++;
                txtProjDesc.Text = item.PROJ_DESC;
                txtProjLoc.Text = string.Format("{0} {1} {2} {3} {4} {5} {6} ", item.PROJ_HSE_NO, item.PROJ_LOT_NO, item.PROJ_BLK_NO, item.PROJ_ADDR, item.PROJ_BRGY, item.PROJ_CITY, item.PROJ_PROV);

                m_sProjOwner = item.PROJ_OWNER;
                Accounts account = new Accounts();
                account.GetOwner(m_sProjOwner);

                txtProjOwn.Text = account.OwnerName;

                try
                {
                    DateTime.TryParse(item.DATE_APPLIED.ToString(), out dtApplied);

                    txtAppDate.Text = dtApplied.ToShortDateString();
                }
                catch
                {
                    txtAppDate.Text = "";
                }
                txtStatus.Text = item.STATUS_CODE;
            }

            if (iRecCount == 0)
            {
                MessageBox.Show("No application found", m_sModule, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            string sQuery = string.Empty;
            int iCnt = 0;

            sQuery = "select * from taxdues where ";
            sQuery += $" arn = '{m_sAN}' ";
            var epsrec = db.Database.SqlQuery<TAXDUES>(sQuery);

            foreach (var items in epsrec)
            {
                iCnt++;
                txtBillNo.Text = items.BILL_NO;
            }

            if (iCnt == 0)
            {
                MessageBox.Show("No Billing Found!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            GetTaxDues();
            btnExit.Text = "Cancel";
            return true;
        }

        private void GetTaxDues()
        {
            OracleResultSet res = new OracleResultSet();
            OracleResultSet res2 = new OracleResultSet();
            OracleResultSet res3 = new OracleResultSet();
            OracleResultSet res4 = new OracleResultSet();
            string sFees = string.Empty;
            double dFeesAmt = 0;
            string sFeesCat = string.Empty;
            string sPermitCode = string.Empty;
            string sFeesDesc = string.Empty;
            string sPermitDesc = string.Empty;
            float fSurch = 0;
            float fAddl = 0;
            float fOthers = 0;
            string sFeesAmt = string.Empty;
            string sSurch = string.Empty;
            string sOthers = string.Empty;
            string sTotalAmt = string.Empty;
            string sAllTotalAmt = string.Empty;
            bool IsOk = false;
            double dTotalAmt = 0;
            double dAllTotalAmt = 0;

            //main fees
            //res2.Query = $"select distinct TD.*, PT.permit_desc, S.fees_desc ";
            //res2.Query += $"from taxdues TD, permit_tbl PT, subcategories S ";
            //res2.Query += $"where substr(TD.permit_code,1,2) = PT.permit_code ";
            //res2.Query += $"and TD.arn = '{m_sAN}'";
            //res2.Query += $"and TD.fees_code = S.fees_code";
            //res2.Query += $" and TD.fees_category = 'MAIN'";
            //res2.Query += $" and TD.arn in (select application_que.arn from application_que where TD.arn = application_que.arn) ";
            //res2.Query += $" order by TD.fees_code";

            res2.Query = $"select distinct TD.permit_code, PT.permit_desc, SUM(TD.fees_amt) as fees_amt, TD.FEES_CATEGORY ";
            res2.Query += $"from taxdues TD, permit_tbl PT ";
            res2.Query += $"where substr(TD.permit_code,1,2) = PT.permit_code ";
            res2.Query += $"and TD.arn = '{m_sAN}'";
            res2.Query += $" and TD.fees_category = 'MAIN'";
            res2.Query += $" and TD.arn in (select application_que.arn from application_que where TD.arn = application_que.arn) ";
            res2.Query += $" group by TD.permit_code, PT.permit_desc, TD.FEES_CATEGORY";

            if (res2.Execute())
                while(res2.Read())
                {
                    dTotalAmt = 0;
                    //sFees = res2.GetString("fees_code");
                    dFeesAmt = res2.GetDouble("fees_amt");
                    sFeesCat = res2.GetString("fees_category");
                    sPermitCode = res2.GetString("permit_code");
                    sPermitDesc = res2.GetString("permit_desc");
                    //sFeesDesc = res2.GetString("fees_desc");

                    //surcharge
                    res3.Query = $"select sum(TD.fees_amt) as fees_amt from other_major_fees OM, taxdues TD ";
                    res3.Query += $"where fees_desc = 'SURCHARGE' ";
                    res3.Query += $"AND substr(TD.fees_code,1,2) = OM.fees_code and TD.arn = '{m_sAN}' ";
                    res3.Query += $"AND TD.fees_category = 'OTHERS' ";
                    res3.Query += $"AND TD.permit_code = '{sPermitCode}' ";
                    float.TryParse(res3.ExecuteScalar().ToString(), out fSurch);
                    res3.Close();

                    //additional
                    res3.Query = "select sum(taxdues.fees_amt) as fees_amt ";
                    res3.Query += "from taxdues ";
                    res3.Query += $"where taxdues.arn = '{m_sAN}' ";
                    res3.Query += $"AND TAXDUES.FEES_CATEGORY = 'ADDITIONAL' ";
                    res3.Query += $"AND taxdues.permit_code = '{sPermitCode}' ";
                    float.TryParse(res3.ExecuteScalar().ToString(), out fAddl);
                    res3.Close();


                    //add others value to total if is not tagged for display
                    //if (IsOk == false)
                    //{
                    res3.Query = "select O.fees_desc, sum(taxdues.fees_amt) as fees_amt, taxdues.fees_code as fees_code ";
                    res3.Query += "from taxdues, other_major_fees O ";
                    res3.Query += $"where substr(taxdues.fees_code,1,2) = O.fees_code and taxdues.arn = '{m_sAN}' ";
                    res3.Query += $"AND O.FEES_DESC <> 'SURCHARGE' ";
                    res3.Query += $"AND taxdues.fees_category = 'OTHERS' ";
                    res3.Query += $"AND taxdues.permit_code = '{sPermitCode}' ";
                    res3.Query += $"group by o.fees_desc, taxdues.fees_code";
                    if (res3.Execute())
                        while (res3.Read())
                        {
                            sFees = res3.GetString("fees_code");
                            float.TryParse(res3.GetDouble("fees_amt").ToString(), out fOthers);

                            if (ValidateTaggedDisplay(sFees))
                                dTotalAmt += fOthers;
                        }
                    //IsOk = true;
                    res3.Close();
                    //}


                    dTotalAmt += dFeesAmt;
                    dTotalAmt += fSurch;
                    dTotalAmt += fAddl;

                    sFeesAmt = string.Format("{0:#,##0.00}", dFeesAmt);
                    sSurch = string.Format("{0:#,##0.00}", fSurch);
                    sTotalAmt = string.Format("{0:#,##0.00}", dTotalAmt);

                    //for payments info table
                    res.Query = "select distinct TD.permit_code, TD.fees_code, TD.FEES_AMT, TD.FEES_CATEGORY ";
                    res.Query += "from taxdues TD, permit_tbl PT ";
                    res.Query += $"where substr(TD.permit_code,1,2) = PT.permit_code and TD.arn = '{m_sAN}' and TD.bill_no = '{txtBillNo.Text.Trim()}' ";
                    res.Query += $"and PT.permit_code = '{sPermitCode}' ";
                    if (res.Execute())
                        while (res.Read())
                        {
                            dgvTaxDuesInfo.Rows.Add(res.GetString("fees_code"), res.GetString("permit_code"), res.GetDouble("fees_amt").ToString("#,##0.00"), fSurch.ToString("#,##0.00"), 0, sTotalAmt, res.GetString("fees_category"));
                        }
                    res.Close();

                    dgvTaxDues.Rows.Add("",sPermitDesc, sFeesAmt, sSurch, 0, sTotalAmt, sPermitCode, sFeesCat);

                    dAllTotalAmt += dTotalAmt;

                }
            res2.Close();

            // for other fees tagged for display only
            string sDisplay = string.Empty;
            res.Query = "select O.fees_desc as fees_desc,sum(taxdues.fees_amt) as fees_amt, OS.display_amt, taxdues.fees_code, taxdues.PERMIT_CODE, taxdues.fees_category";
            res.Query += "from taxdues, other_major_fees O, other_subcategories OS ";
            res.Query += $"where substr(taxdues.fees_code,1,2) = O.fees_code and taxdues.fees_code = OS.fees_code and taxdues.arn = '{m_sAN}' ";
            res.Query += $"AND O.FEES_DESC <> 'SURCHARGE' ";
            res.Query += $"AND taxdues.fees_category = 'OTHERS' ";
            res.Query += $"group by O.fees_desc, OS.display_amt, taxdues.fees_code, taxdues.PERMIT_CODE, taxdues.fees_category";
            if (res.Execute())
                while (res.Read())
                {
                    sDisplay = res.GetString("display_amt");
                    if (sDisplay == "Y")
                    {
                        sFees = res.GetString("fees_code");
                        sFeesDesc = res.GetString("fees_desc");
                        dFeesAmt = res.GetDouble("fees_amt");
                        sPermitCode = res.GetString("permit_code");
                        sFeesCat = res.GetString("fees_category");

                        sFeesAmt = string.Format("{0:#,###.00}", dFeesAmt);

                        dgvTaxDues.Rows.Add(sFees, sPermitDesc, dFeesAmt, 0, 0, dFeesAmt, sPermitCode, sFeesCat);
                    }
                }
            res.Close();

            sAllTotalAmt = string.Format("{0:#,###.00}", dAllTotalAmt);

            txtGrandTotal.Text = sAllTotalAmt;


            //WIP
            txtCredBal.Text = string.Format("{0:#,##0.00}", 0);
            txtNewCredit.Text = string.Format("{0:#,##0.00}", 0);
            txtTaxCredLess.Text = string.Format("{0:#,##0.00}", 0);
        }


        private bool ValidateTaggedDisplay(string sFees) //check if fees is for display only and not computed to total amount
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = $"select display_amt from other_subcategories where display_amt = 'N' and fees_code = '{sFees}'";
            if (result.Execute())
                if (result.Read())
                    return true;
                else
                    return false;
            else
                return false;

        }

        private void btnSearchBill_Click(object sender, EventArgs e)
        {
            if(SearchBilling())
            {
                m_sAN = an1.GetAn();
                if (string.IsNullOrEmpty(m_sAN))
                    return;

                if (!taskman.AddTask(m_sModule, m_sAN))
                    return;

                if (!DisplayData())
                {
                    ClearControls();
                    return;
                }
                EnableControls(true);
            }     
            else
            {
                MessageBox.Show("No Billing Found!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }     
        }

        private void btnClear_Click(object sender, EventArgs e)
        {;
            ClearControls();
        }

        private bool SearchBilling()
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = $"select * from billing where bill_no = '{txtBillNo.Text.Trim()}'";
            if (result.Execute())
                if (result.Read())
                {
                    an1.SetAn(result.GetString("arn"));
                    return true;
                }
                else
                    return false;
            else
                return false;
        }

        private bool ValidateOR()
        {
            OracleResultSet res = new OracleResultSet();
            int iCnt = 0;
            res.Query = $"select count(*) from or_used where or_no = '{txtCurrOR.Text.Trim()}'";
            int.TryParse(res.ExecuteScalar(), out iCnt);
            if (iCnt != 0)
                return false;
            else
                return true;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(m_sAN))
            {
                MessageBox.Show("Select record for payment", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if(chkCash.Checked == false && chkCheque.Checked == false)
            {
                MessageBox.Show("Please select payment type", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if(!ValidateOR())
            {
                MessageBox.Show("Duplicate O.R.!", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }


            CheckPaymentType();
            double dChkAmt = 0;
            double dGrandTotal = 0;
            double dCashAmt = 0;
            double.TryParse(txtChkAmt.Text.Trim(), out dChkAmt);
            double.TryParse(txtGrandTotal.Text.Trim(), out dGrandTotal);
            double.TryParse(txtCashAmt.Text.Trim(), out dCashAmt);
            if (dCashAmt == 0 && dChkAmt != dGrandTotal)
            {
                MessageBox.Show("Insufficient check amount. Make sure you selected cash/check payment","", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (sPaymentType == "CS" || sPaymentType == "CC")
            {
                frmCashTender form = new frmCashTender();
                form.AmtDue = txtGrandTotal.Text;
                form.CashAmt = txtCashAmt.Text;
                form.ChkAmt = txtChkAmt.Text;
                form.PrevCred = txtTaxCredLess.Text;
                form.ShowDialog();
                if (form.isOK == false)
                    return;
                double.TryParse(form.CashTender, out dCashTender);
            }
          

            if (MessageBox.Show("Save Payment?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SavePayment();
            }
            

        }

        private bool CheckSurcharge(string sCode)
        {
            OracleResultSet res = new OracleResultSet();
            res.Query = $"select fees_desc from other_subcategories where fees_code = '{sCode}'";
            if (res.Execute())
                if (res.Read())
                {
                    if (res.GetString("fees_desc").Contains("SURCHARGE"))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            else
                return false;
        }

        private void SavePayment()
        {
            OracleResultSet res = new OracleResultSet();
            long iNextOR = 0;
            string sPermitCode = string.Empty;
            try
            {
                double dFeesdue = 0;
                double dFeesAmtdue = 0;
                double dGrandTotal = 0;
                double dCashAmt = 0;
                double dChkAmt = 0;
                double dTaxCredLess = 0;
                double dNewCred = 0;
                double dSurch = 0;
                string sORType = string.Empty;
                string sMode = string.Empty;
                sORType = GetFormType(txtCurrOR.Text.Trim());
                sMode = GetTransactionMode(cmbMode.Text.Trim());

                for (int iRow = 0; iRow < dgvTaxDues.Rows.Count; iRow++)
                {
                    sPermitCode = dgvTaxDues[6, iRow].Value.ToString();
                    double.TryParse(dgvTaxDues[5, iRow].Value.ToString(), out dFeesAmtdue);
                    double.TryParse(dgvTaxDues[2, iRow].Value.ToString(), out dFeesdue);
                    double.TryParse(dgvTaxDues[3, iRow].Value.ToString(), out dSurch);
                    
                    res.Query = $"INSERT INTO PAYMENTS VALUES( ";
                    res.Query += $"'{m_sAN}',";
                    res.Query += $"'{txtCurrOR.Text}', ";
                    res.Query += $"to_date('{dtOR.Text.ToString()}','MM/dd/yyyy'), ";
                    res.Query += $"'{txtBillNo.Text}', ";
                    res.Query += $"'{dgvTaxDues[0, iRow].Value.ToString()}', "; //fees code
                    res.Query += $"{dFeesdue}, "; //fees amt due
                    res.Query += $"'{sPaymentType}', "; 
                    res.Query += $"'{sMode}', "; 
                    res.Query += $"'{AppSettingsManager.GetSystemDate().ToString("HH:mm")}', "; 
                    res.Query += $"to_date('{AppSettingsManager.GetSystemDate().ToShortDateString()}','MM/dd/yyyy'), ";
                    res.Query += $"'{m_sTeller}', ";
                    res.Query += $"'{m_sTeller}', ";
                    res.Query += $"'{txtMemo.Text.Trim()}', ";
                    res.Query += $"'{dgvTaxDues[7, iRow].Value.ToString()}', "; //fees category
                    res.Query += $"'{dgvTaxDues[6, iRow].Value.ToString()}')"; // permit code
                    if (res.ExecuteNonQuery() == 0)
                    { }

                    //res.Query = $"INSERT INTO PAYMENTS_INFO VALUES( ";
                    //res.Query += $"'{m_sProjOwner}', ";
                    //res.Query += $"'{txtBillNo.Text}', ";
                    //res.Query += $"'EPS', ";
                    //res.Query += $"'{txtCurrOR.Text}', ";
                    //res.Query += $"to_date('{dtOR.Text.ToString()}','MM/dd/yyyy'), ";
                    //res.Query += $"'{sORType}', ";
                    //res.Query += $"'{dgvTaxDues[0, iRow].Value.ToString()}', "; //fees code
                    //res.Query += $"{dFeesdue}, "; //fees due
                    //res.Query += $"0, "; //int
                    //res.Query += $"{dSurch}, "; //surch
                    //res.Query += $"{dFeesAmtdue}, "; //fees amt due
                    //res.Query += $"'{sPaymentType}', ";
                    //res.Query += $"'{sMode}', ";
                    //res.Query += $"'{AppSettingsManager.GetSystemDate().Year.ToString()}', "; //tax year
                    //res.Query += $"'F', ";
                    //res.Query += $"'F', ";
                    //res.Query += $"to_date('{AppSettingsManager.GetSystemDate().ToShortDateString()}','MM/dd/yyyy'), ";
                    //res.Query += $"'{AppSettingsManager.GetSystemDate().ToString("HH:mm")}', "; 
                    //res.Query += $"'{m_sTeller}', ";
                    //res.Query += $"'{txtMemo.Text.Trim()}', ";
                    //res.Query += $"'{m_sAN}', ";
                    //res.Query += $"{dFeesdue})"; //fees due
                    //if (res.ExecuteNonQuery() == 0)
                    //{ }
                }

                double.TryParse(txtGrandTotal.Text, out dGrandTotal);
                double.TryParse(txtCashAmt.Text, out dCashAmt);
                double.TryParse(txtChkAmt.Text, out dChkAmt);
                double.TryParse(txtTaxCredLess.Text, out dTaxCredLess);
                double.TryParse(txtNewCredit.Text, out dNewCred);
                double dAmtTendered = 0;
                dAmtTendered = dCashAmt + dChkAmt + dTaxCredLess;


                for (int iRow = 0; iRow < dgvTaxDuesInfo.Rows.Count; iRow++)
                {
                    double.TryParse(dgvTaxDuesInfo[5, iRow].Value.ToString(), out dFeesAmtdue);
                    double.TryParse(dgvTaxDuesInfo[2, iRow].Value.ToString(), out dFeesdue);
                    double.TryParse(dgvTaxDuesInfo[3, iRow].Value.ToString(), out dSurch);
                    string sCode = dgvTaxDuesInfo[0, iRow].Value.ToString();

                    if (CheckSurcharge(sCode)) //skip surcharge on saving
                        continue;

                    res.Query = $"INSERT INTO PAYMENTS_INFO VALUES( ";
                    res.Query += $"'{m_sProjOwner}', ";
                    res.Query += $"'{txtBillNo.Text}', ";
                    res.Query += $"'EPS', ";
                    res.Query += $"'{txtCurrOR.Text.Trim()}', ";
                    res.Query += $"to_date('{dtOR.Text.ToString()}','MM/dd/yyyy'), ";
                    res.Query += $"'{sORType}', ";
                    res.Query += $"'{dgvTaxDuesInfo[0, iRow].Value.ToString()}', "; //fees code
                    res.Query += $"{dFeesdue}, "; //fees due
                    res.Query += $"0, "; //int
                    res.Query += $"{dSurch}, "; //surch
                    res.Query += $"{Convert.ToDouble(txtGrandTotal.Text.Trim())}, "; //fees amt due
                    res.Query += $"'{sPaymentType}', ";
                    res.Query += $"'{sMode}', ";
                    res.Query += $"'{AppSettingsManager.GetSystemDate().Year.ToString()}', "; //tax year
                    res.Query += $"'F', ";
                    res.Query += $"'F', ";
                    res.Query += $"to_date('{AppSettingsManager.GetSystemDate().ToShortDateString()}','MM/dd/yyyy'), ";
                    res.Query += $"'{AppSettingsManager.GetSystemDate().ToString("HH:mm")}', ";
                    res.Query += $"'{m_sTeller}', ";
                    res.Query += $"'{txtMemo.Text.Trim()}', ";
                    res.Query += $"'{m_sAN}', ";
                    res.Query += $"{dFeesdue}, "; //fees due
                    res.Query += $"'{dgvTaxDuesInfo[6, iRow].Value.ToString()}', "; //fees cat
                    res.Query += $"'{dgvTaxDuesInfo[1, iRow].Value.ToString()}')"; //permit code
                    if (res.ExecuteNonQuery() == 0)
                    { }
                }


                res.Query = $"INSERT INTO PAYMENTS_TENDERED VALUES( ";
                res.Query += $"'{m_sProjOwner}', ";
                res.Query += $"'{txtCurrOR.Text.Trim()}', ";
                res.Query += $"to_date('{dtOR.Text.ToString()}','MM/dd/yyyy'), ";
                res.Query += $"'{sORType}', ";
                res.Query += $"'{sPaymentType}', ";
                res.Query += $"{dGrandTotal}, "; //orig amt due
                res.Query += $"{dCashAmt}, ";
                res.Query += $"{dChkAmt}, ";
                res.Query += $"{dTaxCredLess}, "; //tax cr used
                res.Query += $"{dAmtTendered}, ";
                res.Query += $"{dNewCred}) ";
                if (res.ExecuteNonQuery() == 0)
                { }

                CheckUsedOR();
                res.Query = "INSERT INTO OR_USED VALUES(";
                res.Query += $"'{txtCurrOR.Text.Trim()}', ";
                res.Query += $"'{m_sTeller}', ";
                res.Query += $"to_date('{AppSettingsManager.GetSystemDate().ToShortDateString()}','MM/dd/yyyy'), ";
                res.Query += $"{txtCurrOR.Text.Trim()}, ";
                res.Query += $"NULL, ";
                res.Query += $"'{sORType}') ";
                if (res.ExecuteNonQuery() == 0)
                { }


                iNextOR = Convert.ToInt64(txtCurrOR.Text.Trim()) + 1;
                res.Query = "UPDATE OR_CURRENT SET ";
                res.Query += $"CUR_OR_NO = '{iNextOR}' ";
                res.Query += $"WHERE CUR_OR_NO = '{txtCurrOR.Text.Trim()}' ";
                res.Query += $"AND TELLER_CODE = '{m_sTeller}' ";
                res.Query += $"AND FORM_TYPE = 'FORM 51'"; //fixed form type to form 51
                if (res.ExecuteNonQuery() == 0)
                { }

                res.Query = "INSERT INTO TELLER_TRANSACTION VALUES(";
                res.Query += $"'{m_sTeller}', "; 
                res.Query += $"'{sMode}', ";
                res.Query += $"'{txtCurrOR.Text}', ";
                res.Query += $"{dGrandTotal}, "; //orig amt due
                res.Query += $"{dTaxCredLess}, "; //tax cr used
                res.Query += $"{dChkAmt}, ";
                res.Query += $"{dCashAmt}, ";
                res.Query += $"0, "; //WIP - excess amount for check transactions
                res.Query += $"'{sPaymentType}', ";
                res.Query += $"to_date('{AppSettingsManager.GetSystemDate().ToShortDateString()}','MM/dd/yyyy'), ";
                res.Query += $"'{sORType}') ";
                if (res.ExecuteNonQuery() == 0)
                { }

                if(sPaymentType == "CC" || sPaymentType == "CQ")
                {
                    res.Query = $"INSERT INTO CHK_TBL SELECT * FROM CHK_TBL_TEMP WHERE OR_NO = '{txtCurrOR.Text.Trim()}'";
                    if (res.ExecuteNonQuery() == 0)
                    { }
                }
                res.Query = $"DELETE FROM CHK_TBL_TEMP WHERE OR_NO = '{txtCurrOR.Text.Trim()}'";
                if (res.ExecuteNonQuery() == 0)
                { }
            }
            catch (Exception ex)
            {
                res.Rollback();
                MessageBox.Show(ex.ToString());
                return;
            }
            res.Close();

            for (int iRow = 0; iRow < dgvTaxDues.Rows.Count; iRow++)
            {
                sPermitCode = dgvTaxDues[6, iRow].Value.ToString();
                res.Query = $"INSERT INTO APPLICATION_HIST SELECT * FROM APPLICATION where ARN = '{m_sAN}' and permit_code = '{sPermitCode}'";
                if (res.ExecuteNonQuery() == 0)
                { }

                res.Query = $"DELETE FROM APPLICATION where ARN = '{m_sAN}' and permit_code = '{sPermitCode}'";
                if (res.ExecuteNonQuery() == 0)
                { }

                res.Query = $"INSERT INTO APPLICATION SELECT * FROM APPLICATION_QUE where ARN = '{m_sAN}' and permit_code = '{sPermitCode}'";
                if (res.ExecuteNonQuery() == 0)
                { }

                res.Query = $"DELETE FROM APPLICATION_QUE where ARN = '{m_sAN}' and permit_code = '{sPermitCode}'";
                if (res.ExecuteNonQuery() == 0)
                { }

                res.Query = $"DELETE FROM BILL_TMP where ARN = '{m_sAN}' and permit_code = '{sPermitCode}'";
                if (res.ExecuteNonQuery() == 0)
                { }

                res.Query = $"INSERT INTO TAXDUES_HIST SELECT * FROM TAXDUES where ARN = '{m_sAN}' AND BILL_NO = '{txtBillNo.Text.Trim()}' and permit_code = '{sPermitCode}'";
                if (res.ExecuteNonQuery() == 0)
                { }

                res.Query = $"INSERT INTO TAXDUES_PAID SELECT * FROM TAXDUES where ARN = '{m_sAN}' AND BILL_NO = '{txtBillNo.Text.Trim()}' and permit_code = '{sPermitCode}'";
                if (res.ExecuteNonQuery() == 0)
                { }

                res.Query = $"DELETE FROM TAXDUES where ARN = '{m_sAN}' AND BILL_NO = '{txtBillNo.Text.Trim()}' and permit_code = '{sPermitCode}'";
                if (res.ExecuteNonQuery() == 0)
                { }

                res.Query = $"INSERT INTO TAX_DETAILS_HIST SELECT * FROM TAX_DETAILS where ARN = '{m_sAN}' AND BILL_NO = '{txtBillNo.Text.Trim()}' and permit_code = '{sPermitCode}'";
                if (res.ExecuteNonQuery() == 0)
                { }

                res.Query = $"INSERT INTO TAX_DETAILS_PAID SELECT * FROM TAX_DETAILS where ARN = '{m_sAN}' AND BILL_NO = '{txtBillNo.Text.Trim()}' and permit_code = '{sPermitCode}'";
                if (res.ExecuteNonQuery() == 0)
                { }

                res.Query = $"DELETE FROM TAX_DETAILS where ARN = '{m_sAN}' AND BILL_NO = '{txtBillNo.Text.Trim()}' and permit_code = '{sPermitCode}'";
                if (res.ExecuteNonQuery() == 0)
                { }

            }
                      
            res.Query = $"INSERT INTO BILLING_HIST SELECT * FROM BILLING where ARN = '{m_sAN}' AND BILL_NO = '{txtBillNo.Text.Trim()}'";
            if (res.ExecuteNonQuery() == 0)
            { }

            res.Query = $"INSERT INTO BILLING_PAID_HIST SELECT * FROM BILLING_PAID where ARN = '{m_sAN}' AND BILL_NO = '{txtBillNo.Text.Trim()}'";
            if (res.ExecuteNonQuery() == 0)
            { }

            res.Query = $"DELETE FROM BILLING_PAID where ARN = '{m_sAN}' AND BILL_NO = '{txtBillNo.Text.Trim()}'";
            if (res.ExecuteNonQuery() == 0)
            { }

            res.Query = $"INSERT INTO BILLING_PAID SELECT * FROM BILLING where ARN = '{m_sAN}' AND BILL_NO = '{txtBillNo.Text.Trim()}'";
            if (res.ExecuteNonQuery() == 0)
            { }

            res.Query = $"DELETE FROM BILLING where ARN = '{m_sAN}' AND BILL_NO = '{txtBillNo.Text.Trim()}'";
            if (res.ExecuteNonQuery() == 0)
            { }
            ///
            




            MessageBox.Show("Payment Saved!", m_sModule, MessageBoxButtons.OK, MessageBoxIcon.Information);


            string sObj = string.Empty;
            sObj = "ONL PAYMENT: " + m_sAN + " OR NO: " + txtCurrOR.Text.Trim();
            if (Utilities.AuditTrail.InsertTrail("COL-PO", "payments_info", sObj) == 0)
            {
                MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmReport form = new frmReport();
            form.ReportName = "PRINT OR";
            form.OR = txtCurrOR.Text.Trim();
            form.BillNo = txtBillNo.Text.Trim();
            form.dtOR = dtOR.Value;
            form.An = m_sAN;
            form.Teller = m_sTeller;
            form.ShowDialog();

            if (blnAllUsed == true)
            {
                this.Close();
                return;
            }

            txtCurrOR.Text = iNextOR.ToString();
            ClearControls();
            EnableControls(false);
        }

        private string GetTransactionMode(string sMode)
        {
            if (sMode == "ONLINE")
                return "ONL";
            else if (sMode == "OFFLINE")
                return "OFL";
            return null;
        }

        private void CheckUsedOR()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            string sORType = string.Empty;
            blnAllUsed = false;
            sORType = GetFormType(txtCurrOR.Text.Trim());
            result.Query = $"select * from or_current where teller_code = '{m_sTeller}' and cur_or_no = '{txtCurrOR.Text.Trim()}'";
            if(result.Execute())
                if(result.Read())
                {
                    string sFrom = result.GetString("from_or_no");
                    string sTo = result.GetString("to_or_no");
                    string sCurr = result.GetString("cur_or_no");

                    if(sCurr == sTo)
                    {
                        MessageBox.Show("All ORs Used. OR will be returned!", m_sModule, MessageBoxButtons.OK, MessageBoxIcon.Information);


                        result2.Query = $"INSERT INTO OR_ASSIGNED_HIST SELECT * FROM OR_ASSIGNED where teller_code = '{m_sTeller}' and from_or_no = '{sFrom}' and to_or_no = '{sTo}'";
                        if (result2.ExecuteNonQuery() == 0)
                        { }

                        result2.Query = $"UPDATE OR_ASSIGNED_HIST SET LAST_OR_USED = '{txtCurrOR.Text.Trim()}' where teller_code = '{m_sTeller}' and from_or_no = '{sFrom}' and to_or_no = '{sTo}'";
                        if (result2.ExecuteNonQuery() == 0)
                        { }

                        result2.Query = $"DELETE FROM OR_ASSIGNED where teller_code = '{m_sTeller}' and from_or_no = '{sFrom}' and to_or_no = '{sTo}'";
                        if (result2.ExecuteNonQuery() == 0)
                        { }

                        result2.Query = $"INSERT INTO OR_RETURNED VALUES(";
                        result2.Query += $"'{m_sTeller}', ";
                        result2.Query += $"'{sFrom}', ";
                        result2.Query += $"'{sTo}', ";
                        result2.Query += $"to_date('{AppSettingsManager.GetSystemDate().ToShortDateString()}','MM/dd/yyyy'), ";
                        result2.Query += $"'{AppSettingsManager.SystemUser.UserCode.ToString()}', ";
                        result2.Query += $"NULL, ";
                        result2.Query += $"'{sORType}')";
                        if (result2.ExecuteNonQuery() == 0)
                        { }

                        result2.Query = $"DELETE FROM OR_CURRENT where teller_code = '{m_sTeller}' and cur_or_no = '{txtCurrOR.Text.Trim()}'";
                        if (result2.ExecuteNonQuery() == 0)
                        { }

                        blnAllUsed = true;
                    }
                }

        }

        private string GetFormType(string strOR)
        {
            OracleResultSet res = new OracleResultSet();
            res.Query = "select * from or_current C ";
            res.Query += $"where  C.cur_or_no = '{txtCurrOR.Text}'";
            if(res.Execute())
                if(res.Read())
                {
                    strOR = res.GetString("form_type");
                }
            return strOR;
        }

        private void CheckPaymentType()
        {
            if (chkCash.Checked == true && chkCheque.Checked == false)
                sPaymentType = "CS";
            else if ((chkCash.Checked == false && chkCheque.Checked == true))
                sPaymentType = "CQ";
            else if (chkCash.Checked == true && chkCheque.Checked == true)
                sPaymentType = "CC";
        }

        private void chkCash_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCash.Checked == true)
                ComputeDues();
            else
                txtCashAmt.Text = string.Format("{0:#,##0.00}", 0);
        }

        private void ComputeDues()
        {
            double dCreditLess = 0;
            double dGrandTotal = 0;
            double dCheckAmt = 0;
            double dCashAmt = 0;
            double.TryParse(txtGrandTotal.Text.Trim(), out dGrandTotal);
            double.TryParse(txtChkAmt.Text.Trim(), out dCheckAmt);
            //if (chkCash.Checked == true || chkCheque.Checked == false)
            //{
            double.TryParse(txtTaxCredLess.Text, out dCreditLess);
            if (dCreditLess > 0)
            {
                dGrandTotal -= dCreditLess;
            }
            txtGrandTotal.Text = string.Format("{0:#,##0.00}", dGrandTotal);
            txtCashAmt.Text = txtGrandTotal.Text;
            double.TryParse(txtCashAmt.Text.Trim(), out dCashAmt);
            dCashAmt -= dCheckAmt;
            txtCashAmt.Text = string.Format("{0:#,##0.00}", dCashAmt);
            //}
        }

        private void frmPayments_FormClosed(object sender, FormClosedEventArgs e)
        {
            taskman.RemTask(m_sAN);
        }

        private void cmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbMode.Text == "OFFLINE")
            {
                
            }
        }

        private void chkCheque_CheckedChanged(object sender, EventArgs e)
        {
            if(chkCheque.Checked == true)
            {
                btnBankInfo.PerformClick();
            }
            else
            {
                txtChkAmt.Text = string.Empty;
                ComputeDues();
            }
        }

        private void btnBankInfo_Click(object sender, EventArgs e)
        {
            frmCheck form = new frmCheck();
            form.m_sOrNo = txtCurrOR.Text.Trim();
            form.m_sAmount = txtGrandTotal.Text.Trim();
            form.dtOrDate = dtOR.Value;
            form.m_sFormType = GetFormType(txtCurrOR.Text.Trim());
            form.m_sTeller = m_sTeller;
            if (chkCash.Checked == false)
                form.m_sPayType = "CQ";
            else
                form.m_sPayType = "CC";
            form.ShowDialog();

            txtChkAmt.Text = form.m_sChkAmt;
            if(!string.IsNullOrEmpty(form.m_sCashAmount))
                txtCashAmt.Text = form.m_sCashAmount;
        }
    }
}