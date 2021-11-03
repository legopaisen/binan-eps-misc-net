using Common.DataConnector;
using Common.EncryptUtilities;
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
    public partial class frmTellers : Form
    {
        public frmTellers()
        {
            InitializeComponent();
        }

        private string m_sTellerCode = string.Empty;
        private string m_sPassword = string.Empty;
        private string m_sConfirmPass = string.Empty;

        private void frmTellers_Load(object sender, EventArgs e)
        {
            PopulateTellers();
            LoadFormTypes();
            EnableControls(false);
        }

        private void LoadFormTypes()
        {
            cmbFormType.Items.Clear();
            OracleResultSet result = new OracleResultSet();
            result.Query = "select form_fld from form_tbl order by form_fld";
            if (result.Execute())
                while (result.Read())
                {
                    cmbFormType.Items.Add(result.GetString("form_fld"));
                }
            result.Close();
        }

        private void PopulateTellers()
        {
            dgvTellerList.Rows.Clear();
            string sTellerCode = string.Empty;
            string sLN = string.Empty;
            string sFN = string.Empty;
            string sMI = string.Empty;
            string sPos = string.Empty;
            string sType = string.Empty;
            string sMemo = string.Empty;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from tellers order by teller_code";
            if(result.Execute())
                while(result.Read())
                {
                    sTellerCode = result.GetString("teller_code");
                    sLN = result.GetString("teller_ln");
                    sFN = result.GetString("teller_fn");
                    sMI = result.GetString("teller_mi");
                    sPos = result.GetString("teller_pos");
                    sMemo = result.GetString("teller_memo");
                    sType = result.GetString("form_fld");
                    dgvTellerList.Rows.Add(sTellerCode, sLN, sFN, sMI, sPos, "", sMemo ,sType);
                }
            result.Close();
        }

        private void EnableControls(bool bln)
        {
            txtTellerCode.ReadOnly = !bln;
            txtLastName.ReadOnly = !bln;
            txtFirstName.ReadOnly = !bln;
            txtMI.ReadOnly = !bln;
            txtPosition.ReadOnly = !bln;
            txtMemo.ReadOnly = !bln;
            txtPassword.ReadOnly = !bln;
            txtConfirmPass.ReadOnly = !bln;
            cmbFormType.Enabled = bln;
            dgvTellerList.ReadOnly = !bln;
            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            dgvTellerList.Enabled = !bln;
        }

        private void ClearControls()
        {
            m_sTellerCode = string.Empty;
            txtTellerCode.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtMI.Text = string.Empty;
            txtPosition.Text = string.Empty;
            txtMemo.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPass.Text = string.Empty;
            btnAdd.Text = "Add";
            btnEdit.Text = "Edit";
            btnExit.Text = "Exit";
        }

        private void dgvTellerList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ClearControls();

            try
            {
                if (dgvTellerList[0, e.RowIndex].Value != null)
                {
                    m_sTellerCode = dgvTellerList[0, e.RowIndex].Value.ToString();
                    txtTellerCode.Text = dgvTellerList[0, e.RowIndex].Value.ToString();
                }

                if (dgvTellerList[1, e.RowIndex].Value != null)
                    txtLastName.Text = dgvTellerList[1, e.RowIndex].Value.ToString();

                if (dgvTellerList[2, e.RowIndex].Value != null)
                    txtFirstName.Text = dgvTellerList[2, e.RowIndex].Value.ToString();

                if (dgvTellerList[3, e.RowIndex].Value != null)
                    txtMI.Text = dgvTellerList[3, e.RowIndex].Value.ToString();

                if (dgvTellerList[4, e.RowIndex].Value != null)
                    txtPosition.Text = dgvTellerList[4, e.RowIndex].Value.ToString();

                if (dgvTellerList[6, e.RowIndex].Value != null)
                    txtMemo.Text = dgvTellerList[6, e.RowIndex].Value.ToString();

                if (dgvTellerList[7, e.RowIndex].Value != null)
                    cmbFormType.Text = dgvTellerList[7, e.RowIndex].Value.ToString();
            }

            catch(Exception ex) { }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(btnAdd.Text == "Add")
            {
                EnableControls(true);
                ClearControls();
                btnAdd.Text = "Save";
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnExit.Text = "Cancel";
            }
            else
            {
                if(string.IsNullOrEmpty(txtTellerCode.Text.Trim()) && string.IsNullOrEmpty(txtLastName.Text.Trim()) && string.IsNullOrEmpty(txtMI.Text.Trim()) && string.IsNullOrEmpty(txtPosition.Text.Trim()))
                {
                    MessageBox.Show("Incomplete details!","",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (string.IsNullOrEmpty(txtPassword.Text.ToString()))
                {
                    MessageBox.Show("Password is required", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (string.IsNullOrEmpty(txtConfirmPass.Text.ToString()))
                {
                    MessageBox.Show("Please Confirm User's Password", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (txtPassword.Text.ToString().Trim() != txtConfirmPass.Text.ToString().Trim())
                {
                    MessageBox.Show("Password entries do not match", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                OracleResultSet res = new OracleResultSet();
                res.Query = $"select * from tellers where teller_code = '{txtTellerCode.Text.ToString().Trim()}'";
                if(res.Execute())
                    if(res.Read())
                    {
                        MessageBox.Show("Teller Code " + txtTellerCode.Text.ToString().Trim() + " already exists.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                if(MessageBox.Show("Save record?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    res.Query = "INSERT INTO TELLERS VALUES(";
                    res.Query += $"'{txtTellerCode.Text.Trim()}', ";
                    res.Query += $"'{txtLastName.Text.Trim()}', ";
                    res.Query += $"'{txtFirstName.Text.Trim()}', ";
                    res.Query += $"'{txtMI.Text.Trim()}', ";
                    res.Query += $"'{txtPosition.Text.Trim()}', ";
                    res.Query += $"'{m_sPassword}', ";
                    res.Query += $"'{txtMemo.Text.Trim()}', ";
                    res.Query += $"'{cmbFormType.Text.Trim()}')";
                    if(res.ExecuteNonQuery() == 0)
                    { }
                    res.Close();
                    MessageBox.Show("Record Saved!","",MessageBoxButtons.OK, MessageBoxIcon.Information);


                    string sObj = string.Empty;
                    sObj = "Teller Code: " + txtTellerCode.Text.ToString().Trim();
                    sObj += "/Teller Name: " + txtLastName.Text.Trim() + " " + txtFirstName.Text.Trim() + " " + txtMI.Text.Trim();
                    if (Utilities.AuditTrail.InsertTrail("COL-STT-A", "tellers", sObj) == 0)
                    {
                        MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }



                    PopulateTellers();
                    ClearControls();
                    EnableControls(false);
                }
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            Encryption enc = new Encryption();
            m_sPassword = enc.EncryptString(txtPassword.Text.ToString().Trim());
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(btnEdit.Text == "Edit")
            {
                if (string.IsNullOrEmpty(txtTellerCode.Text.Trim()))
                {
                    MessageBox.Show("Select teller to edit!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                btnEdit.Text = "Update";
                btnExit.Text = "Cancel";
                EnableControls(true);
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                txtTellerCode.Enabled = false;
            }

            else
            {
                if (string.IsNullOrEmpty(txtTellerCode.Text.Trim()) && string.IsNullOrEmpty(txtLastName.Text.Trim()) && string.IsNullOrEmpty(txtMI.Text.Trim()) && string.IsNullOrEmpty(txtPosition.Text.Trim()))
                {
                    MessageBox.Show("Incomplete details!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (!string.IsNullOrEmpty(txtPassword.Text.Trim()) && string.IsNullOrEmpty(txtConfirmPass.Text.ToString()))
                {
                    MessageBox.Show("Please Confirm User's Password", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (txtPassword.Text.ToString().Trim() != txtConfirmPass.Text.ToString().Trim())
                {
                    MessageBox.Show("Password entries do not match", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (MessageBox.Show("Update record?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleResultSet res = new OracleResultSet();
                    res.Query = "UPDATE TELLERS SET ";
                    res.Query += $"TELLER_LN = '{txtLastName.Text.Trim()}', ";
                    res.Query += $"TELLER_FN = '{txtFirstName.Text.Trim()}', ";
                    res.Query += $"TELLER_MI = '{txtMI.Text.Trim()}', ";
                    res.Query += $"TELLER_POS = '{txtPosition.Text.Trim()}', ";
                    if (!string.IsNullOrEmpty(txtPassword.Text.Trim()))
                        res.Query += $"TELLER_PWD = '{m_sPassword}', ";

                    res.Query += $"TELLER_MEMO = '{txtMemo.Text.Trim()}', ";
                    res.Query += $"FORM_FLD = '{cmbFormType.Text.Trim()}' ";
                    res.Query += $" WHERE TELLER_CODE = '{txtTellerCode.Text.Trim()}'";
                    if (res.ExecuteNonQuery() == 0)
                    { }
                    res.Close();

                    MessageBox.Show("Record Saved!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    string sObj = string.Empty;
                    sObj = "Teller Code: " + txtTellerCode.Text.ToString().Trim();
                    sObj += "/Teller Name: " + txtLastName.Text.Trim() + " " + txtFirstName.Text.Trim() + " " + txtMI.Text.Trim();
                    if (Utilities.AuditTrail.InsertTrail("COL-STT-E", "tellers", sObj) == 0)
                    {
                        MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    PopulateTellers();
                    ClearControls();
                    EnableControls(false);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (btnExit.Text == "Exit")
                this.Close();
            else
            {
                btnExit.Text = "Exit";
                ClearControls();
                EnableControls(false);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTellerCode.Text.Trim()))
            {
                MessageBox.Show("Select teller to delete!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if(CheckTransactions())
            {
                MessageBox.Show("Cannot delete. Teller has existing transactions!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Delete record?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OracleResultSet res = new OracleResultSet();
                res.Query = $"DELETE FROM TELLERS WHERE TELLER_CODE = '{txtTellerCode.Text.Trim()}' ";
                if(res.ExecuteNonQuery() == 0)
                { }
                res.Close();

                MessageBox.Show("Record Deleted!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);


                string sObj = string.Empty;
                sObj = "Teller Code: " + txtTellerCode.Text.ToString().Trim();
                sObj += "/Teller Name: " + txtLastName.Text.Trim() + " " + txtFirstName.Text.Trim() + " " + txtMI.Text.Trim();
                if (Utilities.AuditTrail.InsertTrail("COL-STT-D", "tellers", sObj) == 0)
                {
                    MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                PopulateTellers();
                ClearControls();

            }


        }

        private bool CheckTransactions()
        {
            OracleResultSet res = new OracleResultSet();
            res.Query = $"select * from teller_transaction where teller = '{txtTellerCode.Text.Trim()}'";
            if (res.Execute())
                if (res.Read())
                    return true;
                else
                    return false;
            else
                return false;
        }
    }
}
