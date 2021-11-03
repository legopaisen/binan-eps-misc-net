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
    public partial class frmORInventory : Form
    {
        public frmORInventory()
        {
            InitializeComponent();
        }

        private string sFromOR = string.Empty;
        private string sToOR = string.Empty;
        private string sFormtype = string.Empty;

        private void frmORInventory_Load(object sender, EventArgs e)
        {
            PopulateInv();
            LoadFormTypes();
            EnableControls(false);
            ClearControls();
        }

        private void ClearControls()
        {
            cmbFormType.SelectedIndex = -1;
            txtFrom.Text = string.Empty;
            txtTo.Text = string.Empty;
            btnAdd.Text = "Add";
            btnEdit.Text = "Edit";
            btnExit.Text = "Exit";
            dgvORInv.Enabled = true;
            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
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

        private void PopulateInv()
        {
            dgvORInv.Rows.Clear();
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from or_inv order by or_dt";
            if(result.Execute())
                while(result.Read())
                {
                    dgvORInv.Rows.Add(result.GetString("form_type"), result.GetString("from_or"), result.GetString("to_or"), result.GetDateTime("or_dt").ToShortDateString());
                }
            result.Close();
        }

        private void EnableControls(bool bln)
        {
            cmbFormType.Enabled = bln;
            txtTo.Enabled = bln;
            txtFrom.Enabled = bln;
            dgvORInv.ReadOnly = true;
        }

        private bool ValidateOR()
        {
            OracleResultSet res = new OracleResultSet();
            res.Query = $"select * from or_inv where '{txtFrom.Text.Trim()}' between (select from_or from or_inv) and (select to_or from or_inv) ";
            res.Query += $" OR '{txtTo.Text.Trim()}' between (select from_or from or_inv) and (select to_or from or_inv) ";
            if (res.Execute())
                if (res.Read())
                    return false;
                else
                    return true;
            else
                return true;
        }

        private bool ValidateUsage()
        {
            OracleResultSet res = new OracleResultSet();
            res.Query = $"select * from or_assigned where '{txtFrom.Text.Trim()}' between from_or_no and to_or_no and '{txtTo.Text.Trim()}' between from_or_no and to_or_no";
            if (res.Execute())
                if (res.Read())
                    return true;

            res.Query = $"select * from or_used where or_no between '{txtFrom.Text.Trim()}' and '{txtTo.Text.Trim()}'";
            if (res.Execute())
                if (res.Read())
                    return true;

            return false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (btnAdd.Text == "Add")
            {
                btnAdd.Text = "Save";
                EnableControls(true);
                btnEdit.Enabled = false;
                btnExit.Text = "Cancel";
                btnDelete.Enabled = false;
                dgvORInv.Enabled = false;
                txtFrom.Text = "";
                txtTo.Text = "";
                cmbFormType.Text = "";
            }
            else if (btnAdd.Text == "Save")
            {
                if (!ValidateOR())
                {
                    MessageBox.Show("OR range already exists or is in conflict with existing ranges", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (MessageBox.Show("Save record?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleResultSet res = new OracleResultSet();
                    res.Query = "INSERT INTO OR_INV VALUES(";
                    res.Query += $"'{AppSettingsManager.SystemUser.UserCode.ToString()}', ";
                    res.Query += $"'{cmbFormType.Text}', ";
                    res.Query += $"'{txtFrom.Text}', ";
                    res.Query += $"'{txtTo.Text}', ";
                    res.Query += $"to_date('{AppSettingsManager.GetSystemDate().ToShortDateString()}','MM/dd/yyyy'))";
                    if (res.ExecuteNonQuery() == 0)
                    { }
                    res.Close();
                    MessageBox.Show("Record Saved!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    return;

                string sObj = string.Empty;
                sObj = "OR Inventory: " + txtFrom.Text.Trim() + "-" + txtTo.Text.Trim();
                if (Utilities.AuditTrail.InsertTrail("COL-SOV-A", "or_inventory", sObj) == 0)
                {
                    MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                dgvORInv.Enabled = true;
                ClearControls();
                PopulateInv();
                EnableControls(false);
            }
        }

        private void dgvORInv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                cmbFormType.Text = dgvORInv[0, e.RowIndex].Value.ToString();
                sFormtype = dgvORInv[0, e.RowIndex].Value.ToString();
            }
            catch { }
            try
            {
                txtFrom.Text = dgvORInv[1, e.RowIndex].Value.ToString();
                sFromOR = dgvORInv[1, e.RowIndex].Value.ToString();
            }
            catch { }
            try
            {
                txtTo.Text = dgvORInv[2, e.RowIndex].Value.ToString();
                sToOR = dgvORInv[2, e.RowIndex].Value.ToString();
            }
            catch { }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (btnExit.Text == "Exit")
                this.Close();
            else
            {
                EnableControls(false);
                ClearControls();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(btnEdit.Text == "Edit")
            {
                if(txtFrom.Text == "" || txtTo.Text == "")
                {
                    MessageBox.Show("No OR Range Selected!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                EnableControls(true);
                btnEdit.Text = "Update";
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                btnExit.Text = "Cancel";
                dgvORInv.Enabled = false;
            }
            else
            {
                if (!ValidateOR())
                {
                    MessageBox.Show("OR range already exists or is in conflict with existing ranges", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if(ValidateUsage())
                {
                    MessageBox.Show("Editing not allowed. OR range is currently used!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (MessageBox.Show("Update record?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleResultSet res = new OracleResultSet();
                    res.Query = $"UPDATE OR_INV SET FROM_OR = '{txtFrom.Text}', TO_OR = '{txtTo.Text}', FORM_TYPE = '{cmbFormType.Text}' WHERE ";
                    res.Query += $" FROM_OR = '{sFromOR}' AND";
                    res.Query += $" TO_OR = '{sToOR}' AND";
                    res.Query += $" FORM_TYPE = '{sFormtype}'";
                    if(res.ExecuteNonQuery() == 0)
                    { }
                    res.Close();
                    MessageBox.Show("Record Saved!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearControls();
                    PopulateInv();
                    dgvORInv.Enabled = true;
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = true;
                    EnableControls(false);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtFrom.Text == "" || txtTo.Text == "")
            {
                MessageBox.Show("No OR Range Selected!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (ValidateUsage())
            {
                MessageBox.Show("Cannot delete. OR range used or is assigned to Teller!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (MessageBox.Show("Delete record?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OracleResultSet res = new OracleResultSet();
                res.Query = $"DELETE FROM OR_INV WHERE FROM_OR = '{sFromOR}' and TO_OR = '{sToOR}' and FORM_TYPE = '{sFormtype}'";
                if (res.ExecuteNonQuery() == 0)
                { }
                res.Close();
                MessageBox.Show("Record Deleted!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string sObj = string.Empty;
                sObj = "OR Inventory: " + txtFrom.Text.Trim() + "-" + txtTo.Text.Trim();
                if (Utilities.AuditTrail.InsertTrail("COL-SOV-D", "or_inventory", sObj) == 0)
                {
                    MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ClearControls();
                PopulateInv();
            }

        }
    }
}
