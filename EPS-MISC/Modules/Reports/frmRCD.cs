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
    public partial class frmRCD : Form
    {
        public frmRCD()
        {
            InitializeComponent();
        }

        public bool IsReprint = false;
        private string RCDNo = string.Empty;

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void lblTotalDenom_Click(object sender, EventArgs e)
        {

        }

        private void frmRCD_Load(object sender, EventArgs e)
        {
            LoadTellers();
            LoadGrid();
            if (IsReprint)
            {
                cmbSeries.Enabled = true;
                btnGenerate.Enabled = false;
                btnGenerate.Text = "Reprint";
                dgvBills.ReadOnly = true;
                dgvCoins.ReadOnly = true;
            }
            else
            {
                cmbSeries.Enabled = false;
                btnGenerate.Enabled = false;
                dgvBills.ReadOnly = false;
                dgvCoins.ReadOnly = false;
            }

        }

        private void LoadGrid()
        {
            dgvBills.Rows.Clear();
            dgvCoins.Rows.Clear();

            dgvBills.Rows.Add("1,000.00" , "", "");
            dgvBills.Rows.Add("500.00" , "", ""); 
            dgvBills.Rows.Add("200.00" , "", "");
            dgvBills.Rows.Add("100.00" , "", "");
            dgvBills.Rows.Add("50.00" , "", "");
            dgvBills.Rows.Add("20.00" , "", "");

            dgvCoins.Rows.Add("10.00", "", "");
            dgvCoins.Rows.Add("5.00", "", "");
            dgvCoins.Rows.Add("1.00", "", "");
            dgvCoins.Rows.Add("0.50", "", "");
            dgvCoins.Rows.Add("0.25", "", "");
            dgvCoins.Rows.Add("0.10", "", "");
            dgvCoins.Rows.Add("0.05", "", "");
            dgvCoins.Rows.Add("0.01", "", "");

        }

        private void LoadTellers()
        {
            cmbTeller.Items.Clear();
            OracleResultSet res = new OracleResultSet();
            res.Query = "select teller_code from tellers order by teller_code";
            if(res.Execute())
                while(res.Read())
                {
                    cmbTeller.Items.Add(res.GetString("teller_code"));
                }
            res.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClearControls()
        {
            txtBillTotal.Text = "";
            txtCoinsTotal.Text = "";
            txtColTotal.Text = "";
            txtTotalCash.Text = "";
            txtTotalCheck.Text = "";
            lblTotalDenom.Text = "0.00";
            cmbTeller.SelectedIndex = -1;
            cmbSeries.Text = string.Empty;
            cmbSeries.Items.Clear();
        }

        private void cmbTeller_SelectedIndexChanged(object sender, EventArgs e)
        {
            OracleResultSet res = new OracleResultSet();
            if (!IsReprint)
            {
                res.Query = "select nvl(sum(cash_amt),0) as cash_amt, nvl(sum(check_amt),0) as check_amt, nvl(sum(total_amt_tendered),0) as total_amt_tendered FROM ";
                res.Query += "(SELECT DISTINCT * FROM ";
                res.Query += $@"(select PT.*
                            from payments_tendered PT, payments_info PI where PT.or_no = PI.or_no 
                            and PI.teller_code = '{cmbTeller.Text.Trim()}'
                            and PT.or_no not in (select or_no from rcd_remit where PT.or_no = rcd_remit.or_no)
                            and PI.FEES_CATEGORY <> 'OTHERS'))"; // removed date condition based on arcs binan rcd
                if (res.Execute())
                    if (res.Read())
                    {
                        txtTotalCash.Text = string.Format("{0:#,##0.00}", res.GetDouble("cash_amt"));
                        txtTotalCheck.Text = string.Format("{0:#,##0.00}", res.GetDouble("check_amt"));
                        txtColTotal.Text = string.Format("{0:#,##0.00}", res.GetDouble("total_amt_tendered"));
                    }
                res.Close();
            }
            else
            {
                cmbSeries.Items.Clear();
                res.Query = $"select distinct rcd_series from partial_remit where teller_code = '{cmbTeller.Text}' and dt_save = '{dtDate.Value.ToString("dd/MMM/yy")}'";
                if(res.Execute())
                    while(res.Read())
                    {
                        cmbSeries.Items.Add(res.GetString("rcd_series").Trim());
                    }
                res.Close();
            }
        }


        private void dtDate_ValueChanged(object sender, EventArgs e)
        {
            ClearControls();
            LoadGrid();
        }

        private void dgvBills_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgvCoins_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvBills_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            double dDenom = 0;
            int iQty = 0;
            double dAmt = 0;
            
            try
            {
                int.TryParse(dgvBills[1, e.RowIndex].Value.ToString(), out iQty);
            }
            catch { }

            try
            {
                double.TryParse(dgvBills[0, e.RowIndex].Value.ToString(), out dDenom);
            }
            catch { }

            try
            {
                dAmt = dDenom * iQty;
                if(dAmt == 0)
                    dgvBills[2, e.RowIndex].Value = "";
                else
                 dgvBills[2, e.RowIndex].Value = dAmt.ToString("#,##0.00");
            }
            catch { }

            try
            {
                if (dgvBills.Rows[e.RowIndex].Cells[1].Selected == true)
                    ComputeTotal();
            }
            catch { }
        }

        private void ComputeTotal()
        {
            double dBills = 0;
            double dCoins = 0;
            int index = 0;
            foreach(DataGridViewRow row in dgvBills.Rows)
            {
                index = row.Index;
                try
                {
                    dBills += Convert.ToDouble(dgvBills[2, index].Value);
                }
                catch { }
            }
            txtBillTotal.Text = string.Format("{0:##,##0.00}", dBills);

            foreach (DataGridViewRow row in dgvCoins.Rows)
            {
                index = row.Index;
                try
                {
                    dCoins += Convert.ToDouble(dgvCoins[2, index].Value);
                }
                catch { }
            }
            txtCoinsTotal.Text = string.Format("{0:##,##0.00}", dCoins);

            lblTotalDenom.Text = string.Format("{0:##,##0.00}", dBills + dCoins);

            if (lblTotalDenom.Text.Trim() == txtTotalCash.Text.Trim())
                btnGenerate.Enabled = true;
            else
                btnGenerate.Enabled = false;
        }

        private void dgvBills_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (dgvBills.CurrentCell.ColumnIndex == 1)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }
        }


        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dgvBills_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void dgvCoins_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (dgvCoins.CurrentCell.ColumnIndex == 1)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }
        }

        private void dgvCoins_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            double dDenom = 0;
            int iQty = 0;
            double dAmt = 0;

            try
            {
                int.TryParse(dgvCoins[1, e.RowIndex].Value.ToString(), out iQty);
            }
            catch { }

            try
            {
                double.TryParse(dgvCoins[0, e.RowIndex].Value.ToString(), out dDenom);
            }
            catch { }

            try
            {
                dAmt = dDenom * iQty;
                if (dAmt == 0)
                    dgvCoins[2, e.RowIndex].Value = "";
                else
                    dgvCoins[2, e.RowIndex].Value = dAmt.ToString("#,##0.00");
            }
            catch { }

            try
            {
                if (dgvCoins.Rows[e.RowIndex].Cells[1].Selected == true)
                    ComputeTotal();
            }
            catch { }
        }

        private bool ValidateReturn()
        {
            OracleResultSet res = new OracleResultSet();
            int cnt = 0;
            res.Query = $@"select count(distinct or_no)
                            from payments_info where 
                            teller_code = '{cmbTeller.Text.Trim()}'
                            and or_no not in (select or_no from rcd_remit where payments_info.or_no = rcd_remit.or_no)
                            and or_no between (select from_or_no from or_assigned) and (select to_or_no from or_assigned)
                            and FEES_CATEGORY <> 'OTHERS'";
            int.TryParse(res.ExecuteScalar().ToString(), out cnt);
            res.Close();
            if (cnt > 0)
            {
                MessageBox.Show("O.R. series not yet returned!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            else
                return true;

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (!IsReprint)
            {
                if (!ValidateReturn())
                    return;
                OracleResultSet res = new OracleResultSet();
                OracleResultSet res2 = new OracleResultSet();
                string sFromOr = string.Empty;
                string sLastOr = string.Empty;
                string sFormType = string.Empty;
                int iCnt = 0;

                res.Query = $"DELETE FROM RCD_REMIT_TMP WHERE teller_code = '{cmbTeller.Text.Trim()}'";
                if (res.ExecuteNonQuery() == 0)
                { }
                res.Close();

                //res.Query = $"select from_or_no, last_or_used, form_type from OR_ASSIGNED_HIST where teller_code = '{cmbTeller.Text.Trim()}' and NOT EXISTS(select cc.or_no from cancelled_payments cc where cc.or_no between or_assigned_hist.from_or_no and or_assigned_hist.to_or_no) order by date_assigned desc";
                res.Query = $"select from_or_no, last_or_used, form_type from OR_ASSIGNED_HIST where teller_code = '{cmbTeller.Text.Trim()}'  order by date_assigned desc"; // include cancelled
                if (res.Execute())
                    while (res.Read())
                    {
                        sFromOr = res.GetString("from_or_no");
                        sLastOr = res.GetString("last_or_used");
                        sFormType = res.GetString("form_type");

                        if (string.IsNullOrEmpty(sLastOr))
                            continue;

                        iCnt = 0;
                        res2.Query = $"select count(*) from rcd_remit where or_no between '{sFromOr}' and '{sLastOr}'";
                        int.TryParse(res2.ExecuteScalar(), out iCnt);
                        if (iCnt > 0)
                            continue;
                        res2.Close();


                        res2.Query = "INSERT INTO RCD_REMIT_TMP VALUES(";
                        res2.Query += $"'{cmbTeller.Text.Trim()}', ";
                        res2.Query += $"'{sFromOr}', ";
                        res2.Query += $"'{sLastOr}', ";
                        res2.Query += $"'{sFormType}') ";
                        if (res2.ExecuteNonQuery() == 0)
                        { }
                        res2.Close();
                    }
            }

            long qty1000 = 0;
            long qty500 = 0;
            long qty200 = 0;
            long qty100 = 0;
            long qty50 = 0;
            long qty20 = 0;
            long qty10 = 0;

            long qtyc10 = 0;
            long qtyc5 = 0;
            long qtyc1 = 0;
            long qtyc005 = 0;
            long qtyc0025 = 0;
            long qtyc001 = 0;
            long qtyc0005 = 0;
            long qtyc0001 = 0;

            double Coins = 0;
            double dChkAmt = 0;
            double dCashAmt = 0;

            double.TryParse(txtTotalCash.Text.Trim(), out dCashAmt);
            double.TryParse(txtTotalCheck.Text.Trim(), out dChkAmt);

            long.TryParse(Convert.ToString(dgvBills[1, 0].Value), out qty1000);
            long.TryParse(Convert.ToString(dgvBills[1, 1].Value), out qty500);
            long.TryParse(Convert.ToString(dgvBills[1, 2].Value), out qty200);
            long.TryParse(Convert.ToString(dgvBills[1, 3].Value), out qty100);
            long.TryParse(Convert.ToString(dgvBills[1, 4].Value), out qty50);
            long.TryParse(Convert.ToString(dgvBills[1, 5].Value), out qty20);
                
            long.TryParse(Convert.ToString(dgvCoins[1, 0].Value), out qtyc10);
            long.TryParse(Convert.ToString(dgvCoins[1, 1].Value), out qtyc5);
            long.TryParse(Convert.ToString(dgvCoins[1, 2].Value), out qtyc1);
            long.TryParse(Convert.ToString(dgvCoins[1, 3].Value), out qtyc005);
            long.TryParse(Convert.ToString(dgvCoins[1, 4].Value), out qtyc0025);
            long.TryParse(Convert.ToString(dgvCoins[1, 5].Value), out qtyc001);
            long.TryParse(Convert.ToString(dgvCoins[1, 6].Value), out qtyc0005);
            long.TryParse(Convert.ToString(dgvCoins[1, 7].Value), out qtyc0001);

            double.TryParse(txtCoinsTotal.Text.Trim(), out Coins);

            frmReport form = new frmReport();
            form.ReportName = "RCD";
            form.Teller = cmbTeller.Text.Trim();
            form.dtDate = AppSettingsManager.GetSystemDate();
            form.isRCDReprint = false;
            if (IsReprint)
            {
                form.RCDNo = RCDNo;
                form.isRCDReprint = true;
            }
            else
                form.RCDNo = "";
            form.qty1000 = qty1000;
            form.qty500 = qty500;
            form.qty200 = qty200;
            form.qty100 = qty100;
            form.qty50 = qty50;
            form.qty20 = qty20;
            form.qty10 = qty10;

            form.qtyc10 = qtyc10;
            form.qtyc5 = qtyc5;
            form.qtyc1 = qtyc1;
            form.qtyc005 = qtyc005;
            form.qtyc0025 = qtyc0025;
            form.qtyc001 = qtyc001;
            form.qtyc0005 = qtyc0005;
            form.qtyc0001 = qtyc0001;

            form.Coins = Coins;
            form.CashAmt = dCashAmt;
            form.CheckAmt = dChkAmt;
            form.ShowDialog();


            btnGenerate.Enabled = false; //AFM 20211103
            ClearControls();
            LoadTellers();
            LoadGrid();
            
        }

        private void cmbSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            OracleResultSet res = new OracleResultSet();
            string sRCDNo = cmbSeries.Text.Trim();
            RCDNo = sRCDNo;
            long qty1000 = 0;
            long qty500 = 0;
            long qty200 = 0;
            long qty100 = 0;
            long qty50 = 0;
            long qty20 = 0;

            long qtyc10 = 0;
            long qtyc5 = 0;
            long qtyc1 = 0;
            long qtyc005 = 0;
            long qtyc0025 = 0;
            long qtyc001 = 0;
            long qtyc0005 = 0;
            long qtyc0001 = 0;

            double Coins = 0;
            double dChkAmt = 0;
            double dCashAmt = 0;

            res.Query = "select nvl(sum(cash_amt),0) as cash_amt, nvl(sum(check_amt),0) as check_amt, nvl(sum(total_amt_tendered),0) as total_amt_tendered FROM ";
            res.Query += "(SELECT DISTINCT * FROM ";
            res.Query += $@"(select PT.*
                            from payments_tendered PT, payments_info PI where PT.or_no = PI.or_no 
                            and PI.teller_code = '{cmbTeller.Text.Trim()}' and PT.or_no in ";
            res.Query += $"(select or_no from rcd_remit where PT.or_no = rcd_remit.or_no and rcd_remit.rcd_series = '{cmbSeries.Text.Trim()}')";
            res.Query += $"and PI.FEES_CATEGORY <> 'OTHERS')) "; // removed date condition based on arcs binan rcd
            if (res.Execute())
                if (res.Read())
                {
                    txtTotalCash.Text = string.Format("{0:#,##0.00}", res.GetDouble("cash_amt"));
                    txtTotalCheck.Text = string.Format("{0:#,##0.00}", res.GetDouble("check_amt"));
                    txtColTotal.Text = string.Format("{0:#,##0.00}", res.GetDouble("total_amt_tendered"));
                }
            res.Close();

            res.Query = $"select * from partial_remit where rcd_series = '{sRCDNo}'";
            if(res.Execute())
                if(res.Read())
                {
                    qty1000 = res.GetInt("P1000");
                    qty500 = res.GetInt("P500");
                    qty200 = res.GetInt("P200");
                    qty100 = res.GetInt("P100");
                    qty50 = res.GetInt("P50");
                    qty20 = res.GetInt("P20");

                    qtyc10 = res.GetInt("C10");
                    qtyc5 = res.GetInt("C5");
                    qtyc1 = res.GetInt("C1");
                    qtyc005 = res.GetInt("C005");
                    qtyc0025 = res.GetInt("C0025");
                    qtyc001 = res.GetInt("C001");
                    qtyc0005 = res.GetInt("C0005");
                    qtyc0001 = res.GetInt("C0001");

                    dgvBills.Rows.Clear();
                    dgvCoins.Rows.Clear();

                    dgvBills.Rows.Add("1,000.00", qty1000, (qty1000 * 1000).ToString("#,##0.00"));
                    dgvBills.Rows.Add("500.00", qty500, (qty500 * 500).ToString("#,##0.00"));
                    dgvBills.Rows.Add("200.00", qty200, (qty200 * 200).ToString("#,##0.00"));
                    dgvBills.Rows.Add("100.00", qty100, (qty100 * 100).ToString("#,##0.00"));
                    dgvBills.Rows.Add("50.00", qty50, (qty50 * 50).ToString("#,##0.00"));
                    dgvBills.Rows.Add("20.00", qty20, (qty20 * 20).ToString("#,##0.00"));

                    dgvCoins.Rows.Add("10.00", qtyc10, (qtyc10 * 10).ToString("#,##0.00"));
                    dgvCoins.Rows.Add("5.00", qtyc5, (qtyc5 * 5).ToString("#,##0.00"));
                    dgvCoins.Rows.Add("1.00", qtyc1, qtyc1.ToString("#,##0.00"));
                    dgvCoins.Rows.Add("0.50", qtyc005, (qtyc005 * 0.50).ToString("#,##0.00"));
                    dgvCoins.Rows.Add("0.25", qtyc0025, (qtyc0025 * 0.25).ToString("#,##0.00"));
                    dgvCoins.Rows.Add("0.10", qtyc001, (qtyc001 * 0.10).ToString("#,##0.00"));
                    dgvCoins.Rows.Add("0.05", qtyc0005, (qtyc0005 * 0.05).ToString("#,##0.00"));
                    dgvCoins.Rows.Add("0.01", qtyc0001, (qtyc0001 * 0.01).ToString("#,##0.00"));

                    ComputeTotal();
                }

        }
    }
}
