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

namespace Modules.Utilities.Forms
{
    public partial class frmORDeclaration : Form
    {
        public frmORDeclaration()
        {
            InitializeComponent();
        }

        private void frmORDeclaration_Load(object sender, EventArgs e)
        {
            PopulateTeller();
            LoadFormType();
            EnableControls(false);
        }
        
        private void EnableControls(bool bln)
        {
            dgvTellerList.ReadOnly = true;
            dgvORRanges.ReadOnly = true;
            txtCurrOR.Enabled = false;
            txtLN.Enabled = false;
            txtFN.Enabled = false;
            txtMI.Enabled = false;
            txtORFrom.Enabled = false;
            txtORTo.Enabled = false;
        }

        private void PopulateTeller()
        {
            dgvTellerList.Rows.Clear();
            OracleResultSet res = new OracleResultSet();
            res.Query = "select or_assigned.*, tellers.teller_ln, tellers.teller_fn, tellers.teller_MI, or_current.cur_or_no from or_assigned, tellers, or_current where or_assigned.teller_code = tellers.teller_code and or_current.teller_code = tellers.teller_code order by or_assigned.teller_code";
            if(res.Execute())
                while(res.Read())
                {
                    dgvTellerList.Rows.Add(res.GetString("teller_code"), res.GetString("form_type"), res.GetString("from_or_no"), res.GetString("to_or_no"), res.GetDateTime("date_assigned").ToShortDateString(), res.GetString("assigned_by"), res.GetString("teller_ln"), res.GetString("teller_fn"), res.GetString("teller_mi"), res.GetString("cur_or_no"));
                }
            res.Close();
        }

        private void LoadFormType()
        {
            cmbFormType.Items.Clear();
            OracleResultSet res = new OracleResultSet();
            res.Query = "select * from form_tbl order by form_fld";
            if (res.Execute())
                while (res.Read())
                {
                    cmbFormType.Items.Add(res.GetString("form_fld"));
                }
            res.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtTellerCode.Text.Trim()) && !string.IsNullOrEmpty(cmbFormType.Text.Trim()))
            {
                OracleResultSet res = new OracleResultSet();
                OracleResultSet res2 = new OracleResultSet();
                res.Query = $"select * from tellers where teller_code = '{txtTellerCode.Text.Trim()}'";
                if(res.Execute())
                    if(res.Read())
                    {
                        txtLN.Text = res.GetString("teller_ln");
                        txtFN.Text = res.GetString("teller_fn");
                        txtMI.Text = res.GetString("teller_mi");

                        res2.Query = $"select * from or_current where teller_code = '{txtTellerCode.Text.Trim()}' and form_type = '{cmbFormType.Text.Trim()}'";
                        if (res2.Execute())
                            if (res2.Read())
                            {
                                txtORFrom.Text = res2.GetString("from_or_no");
                                txtORTo.Text = res2.GetString("to_or_no");
                                txtCurrOR.Text = res2.GetString("cur_or_no");
                            }
                        res2.Close();
                    }
                res.Close();
            }
        }

        private void ClearControls()
        {
            txtTellerCode.Text = string.Empty;
            txtLN.Text = string.Empty;
            txtFN.Text = string.Empty;
            txtMI.Text = string.Empty;
            txtORFrom.Text = string.Empty;
            txtORTo.Text = string.Empty;
            txtCurrOR.Text = string.Empty;
            dgvORRanges.Rows.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        private void cmbFormType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvORRanges.Rows.Clear();
            OracleResultSet res = new OracleResultSet();
            OracleResultSet res2 = new OracleResultSet();
            string sFromOr = string.Empty;
            string sToOr = string.Empty;
            string sCurrOR = string.Empty;
            //res.Query = $"select * from or_inv where NOT EXISTS (select from_or_no, to_or_no from or_assigned where or_assigned.from_or_no = or_inv.FROM_OR) OR NOT EXISTS (select OR_NO from OR_USED)";
            res.Query = $"select * from or_inv";
            if (res.Execute())
                while (res.Read())
                {
                    sFromOr = res.GetString("from_or");
                    sToOr = res.GetString("to_or");

                    res2.Query = $"select * from or_assigned where from_or_no between '{sFromOr}'  and '{sToOr}' and to_or_no between '{sFromOr}' and '{sToOr}'"; //skip if or range is assigned
                    if (res2.Execute())
                        if (res2.Read())
                        {
                            continue;
                        }

                    //res2.Query = $"select last_or_used from or_assigned_hist where from_or_no = '{sFromOr}' and to_or_no = '{sToOr}' and last_or_used in (select or_no from or_used)";
                    res2.Query = $"select nvl(max(last_or_used),0) as last_or_used from or_assigned_hist where from_or_no between '{sFromOr}' and '{sToOr}' and to_or_no between '{sFromOr}' and '{sToOr}' and last_or_used in (select or_no from or_used)";
                    if (res2.Execute())
                        if (res2.Read())
                        {
                            sCurrOR = res2.GetString("last_or_used");
                            if(sCurrOR == null || sCurrOR == "" || sCurrOR == null || sCurrOR == "0")
                            {
                                sCurrOR = sFromOr;                               
                            }
                            else
                                sCurrOR = (Convert.ToInt64(sCurrOR) + 1).ToString();
                        }
                        else
                        {
                            sCurrOR = sFromOr;
                        }

                    dgvORRanges.Rows.Add(res.GetString("form_type"), sCurrOR, sToOr);
                }
            res.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void cmbFormType_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtORFrom.Text) && !string.IsNullOrEmpty(txtORTo.Text))
            {
                if (string.IsNullOrEmpty(txtTellerCode.Text.Trim()) || string.IsNullOrEmpty(cmbFormType.Text))
                {
                    MessageBox.Show("Select teller and Form type!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (MessageBox.Show("Return OR", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleResultSet result = new OracleResultSet();
                    OracleResultSet result2 = new OracleResultSet();
                    string sORType = string.Empty;
                    result.Query = $"select * from or_current where teller_code = '{txtTellerCode.Text}' and cur_or_no = '{txtCurrOR.Text.Trim()}'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            string sFrom = result.GetString("from_or_no");
                            string sTo = result.GetString("to_or_no");
                            string sCurr = result.GetString("cur_or_no");
                            string sTeller = result.GetString("teller_code");
                            string sBlankCurr = string.Empty;
                            string sLastOR = string.Empty;
                            string sFormType = result.GetString("form_type");
                            sLastOR = GetLastORUsed(sTeller, sFormType);


                            result2.Query = $"INSERT INTO OR_ASSIGNED_HIST SELECT * FROM OR_ASSIGNED where teller_code = '{txtTellerCode.Text}' and from_or_no = '{sFrom}' and to_or_no = '{sTo}'";
                            if (result2.ExecuteNonQuery() == 0)
                            { }

                            if (sCurr == sFrom)
                                result2.Query = $"UPDATE OR_ASSIGNED_HIST SET LAST_OR_USED = '{sBlankCurr}' where teller_code = '{txtTellerCode.Text}' and from_or_no = '{sFrom}' and to_or_no = '{sTo}'";
                            else
                                result2.Query = $"UPDATE OR_ASSIGNED_HIST SET LAST_OR_USED = '{sLastOR}' where teller_code = '{txtTellerCode.Text}' and from_or_no = '{sFrom}' and to_or_no = '{sTo}'";

                            if (result2.ExecuteNonQuery() == 0)
                            { }

                            result2.Query = $"DELETE FROM OR_ASSIGNED where teller_code = '{txtTellerCode.Text}' and from_or_no = '{sFrom}' and to_or_no = '{sTo}'";
                            if (result2.ExecuteNonQuery() == 0)
                            { }

                            result2.Query = $"INSERT INTO OR_RETURNED VALUES(";
                            result2.Query += $"'{txtTellerCode.Text}', ";
                            result2.Query += $"'{sFrom}', ";
                            result2.Query += $"'{sTo}', ";
                            result2.Query += $"to_date('{AppSettingsManager.GetSystemDate().ToShortDateString()}','MM/dd/yyyy'), ";
                            result2.Query += $"'{AppSettingsManager.SystemUser.UserCode.ToString()}', ";
                            result2.Query += $"NULL, ";
                            result2.Query += $"'{cmbFormType.Text}')";
                            if (result2.ExecuteNonQuery() == 0)
                            { }

                            result2.Query = $"DELETE FROM OR_CURRENT where teller_code = '{txtTellerCode.Text}' and cur_or_no = '{txtCurrOR.Text.Trim()}'";
                            if (result2.ExecuteNonQuery() == 0)
                            { }



                            string sObj = string.Empty;
                            sObj = "Teller/OR Return: " + txtTellerCode.Text.Trim() + "/" + txtORFrom.Text.Trim() + "-" + txtORTo.Text.Trim();
                            if (Utilities.AuditTrail.InsertTrail("COL-SOD-R", "or_assigned", sObj) == 0)
                            {
                                MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    ClearControls();
                    PopulateTeller();               
                }
            }
        }

        private string GetLastORUsed(string sTeller, string sFormType)
        {
            OracleResultSet res = new OracleResultSet();
            string sOR = string.Empty;
            res.Query = $"select nvl(max(or_no), 0) as or_no from or_used where teller = '{sTeller}' and form_type = '{sFormType}'";
            if(res.Execute())
                if(res.Read())
                {
                    sOR = res.GetString("or_no");
                }
            return sOR;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDeclare_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtORFrom.Text) && !string.IsNullOrEmpty(txtORTo.Text))
            {
                if(CheckDeclare())
                {
                    return;
                }
                if (string.IsNullOrEmpty(txtTellerCode.Text.Trim()) || string.IsNullOrEmpty(cmbFormType.Text))
                {
                    MessageBox.Show("Select teller and Form type!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (MessageBox.Show("Declare OR?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleResultSet res = new OracleResultSet();
                    res.Query = $"INSERT INTO OR_ASSIGNED VALUES(";
                    res.Query += $"'{txtTellerCode.Text.Trim()}', ";
                    res.Query += $"'{txtORFrom.Text.Trim()}', ";
                    res.Query += $"'{txtORTo.Text.Trim()}', ";
                    res.Query += $"to_date('{AppSettingsManager.GetSystemDate().ToShortDateString()}','MM/dd/yyyy'), ";
                    res.Query += $"'{AppSettingsManager.SystemUser.UserCode.ToString()}', ";
                    res.Query += $"null, ";
                    res.Query += $"'{cmbFormType.Text.Trim()}', ";
                    res.Query += $"null) ";
                    if(res.ExecuteNonQuery() == 0)
                    { }
                    res.Close();

                    res.Query = $"INSERT INTO OR_CURRENT VALUES(";
                    res.Query += $"'{txtTellerCode.Text.Trim()}', ";
                    res.Query += $"'{txtORFrom.Text.Trim()}', ";
                    res.Query += $"'{txtORTo.Text.Trim()}', ";
                    res.Query += $"'{txtORFrom.Text.Trim()}', ";
                    res.Query += $"null, ";
                    res.Query += $"'{cmbFormType.Text.Trim()}')";
                    if (res.ExecuteNonQuery() == 0)
                    { }
                    res.Close();

                    string sObj = string.Empty;
                    sObj = "Teller/OR Declare: " + txtTellerCode.Text.Trim() + "/" + txtORFrom.Text.Trim() + "-" + txtORTo.Text.Trim();
                    if (Utilities.AuditTrail.InsertTrail("COL-SOD-A", "or_assigned", sObj) == 0)
                    {
                        MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    ClearControls();
                    PopulateTeller();
                }

            }
        }

        private bool CheckDeclare()
        {
            OracleResultSet res = new OracleResultSet();
            res.Query = $"select * from or_assigned where teller_code = '{txtTellerCode.Text.Trim()}' and from_or_no = '{txtORFrom.Text.Trim()}' and to_or_no = '{txtORTo.Text.Trim()}' and form_type = '{cmbFormType.Text.Trim()}'";
            if (res.Execute())
                if (res.Read())
                {
                    MessageBox.Show("OR already declared!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return true;
                }

            res.Query = $"select * from or_assigned where teller_code = '{txtTellerCode.Text.Trim()}' and form_type = '{cmbFormType.Text.Trim()}'";
            if (res.Execute())
                if (res.Read())
                {
                    MessageBox.Show("Teller already declared an OR with this form type!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return true;
                }
                else
                    return false;
            return false;

        }

        private void dgvORRanges_CellClicked(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtORFrom.Text = Convert.ToString(dgvORRanges[1, e.RowIndex].Value);
            }
            catch { }
            try
            {
                txtORTo.Text = Convert.ToString(dgvORRanges[2, e.RowIndex].Value);
            }
            catch { }
        }

        private void dgvTellerList_CellClck(object sender, DataGridViewCellEventArgs e)
        {
            txtORFrom.Enabled = false;
            txtORTo.Enabled = false;

            try
            {
                txtTellerCode.Text = Convert.ToString(dgvTellerList[0, e.RowIndex].Value);
            }
            catch { }
            try
            {
                txtLN.Text = Convert.ToString(dgvTellerList[6, e.RowIndex].Value);
            }
            catch { }
            try
            {
                txtFN.Text = Convert.ToString(dgvTellerList[7, e.RowIndex].Value);
            }
            catch { }
            try
            {
                txtMI.Text = Convert.ToString(dgvTellerList[8, e.RowIndex].Value);
            }
            catch { }
            try
            {
                txtORFrom.Text = Convert.ToString(dgvTellerList[2, e.RowIndex].Value);
            }
            catch { }
            try
            {
                txtORTo.Text = Convert.ToString(dgvTellerList[3, e.RowIndex].Value);
            }
            catch { }
            try
            {
                txtCurrOR.Text = Convert.ToString(dgvTellerList[9, e.RowIndex].Value);
            }
            catch { }
            try
            {
                cmbFormType.Text = Convert.ToString(dgvTellerList[1, e.RowIndex].Value);
            }
            catch { }
        }
    }
}
