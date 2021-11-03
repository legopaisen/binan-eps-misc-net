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
    public partial class frmFormType : Form
    {
        public frmFormType()
        {
            InitializeComponent();
        }
        private string m_sFormTypee = string.Empty;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void frmFormType_Load(object sender, EventArgs e)
        {
            PopulateForms();
            EnableControls(false);
        }

        private void PopulateForms()
        {
            dgvFormList.Rows.Clear();
            OracleResultSet result = new OracleResultSet();
            string sForm = string.Empty;
            string sDesc = string.Empty;
            result.Query = "select * from form_tbl order by form_fld";
            if(result.Execute())
                while(result.Read())
                {
                    sForm = result.GetString("form_fld");
                    sDesc = result.GetString("form_desc");
                    dgvFormList.Rows.Add(sForm,sDesc);
                }
            result.Close();
        }

        private void EnableControls(bool bln)
        {
            txtFormType.Enabled = bln;
            txtDesc.Enabled = bln;
            dgvFormList.ReadOnly = true;
        }

        private void ClearControls()
        {
            txtFormType.Text = string.Empty;
            txtDesc.Text = string.Empty;
            m_sFormTypee = string.Empty;
            btnAdd.Text = "Add";
            btnEdit.Text = "Edit";
            btnExit.Text = "Exit";
        }

        private void dgvFormList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ClearControls();

            try
            {
                if (dgvFormList[0, e.RowIndex].Value != null)
                {
                    m_sFormTypee = dgvFormList[0, e.RowIndex].Value.ToString();
                    txtFormType.Text = dgvFormList[0, e.RowIndex].Value.ToString();
                }

                if (dgvFormList[1, e.RowIndex].Value != null)
                    txtDesc.Text = dgvFormList[1, e.RowIndex].Value.ToString();
            }
            catch (Exception ex) { }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(btnAdd.Text == "Add")
            {
                btnAdd.Text = "Save";
                btnExit.Text = "Cancel";
                btnDelete.Enabled = false;
                EnableControls(true);
                dgvFormList.Enabled = false;
                txtDesc.Text = "";
                txtFormType.Text = "";
            }
            else
            {
                if(Validate())
                {
                    MessageBox.Show("Form type already exists!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (MessageBox.Show("Save record?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleResultSet res = new OracleResultSet();
                    res.Query = "INSERT INTO FORM_TBL VALUES(";
                    res.Query += $"'{txtFormType.Text.Trim()}', ";
                    res.Query += $"'{txtDesc.Text.Trim()}') ";
                    if(res.ExecuteNonQuery() == 0)
                    { }

                    MessageBox.Show("Record Saved!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    string sObj = string.Empty;
                    sObj = "Form Type: " + txtFormType.Text;
                    if (Utilities.AuditTrail.InsertTrail("COL-SFT-A", "form_tbl", sObj) == 0)
                    {
                        MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    btnDelete.Enabled = true;
                    EnableControls(false);
                    dgvFormList.Enabled = true;
                    ClearControls();
                    PopulateForms();

                }

            }
        }

        private bool Validate()
        {
            OracleResultSet res = new OracleResultSet();
            res.Query = $"select * from form_tbl where form_fld = '{txtFormType.Text.Trim()}'";
            if (res.Execute())
                if (res.Read())
                    return true;

            return false;
        }

        private bool ValidateUsage()
        {
            OracleResultSet res = new OracleResultSet();
            res.Query = $"select * from or_inv where form_type = '{txtFormType.Text.Trim()}'";
            if (res.Execute())
                if (res.Read())
                    return true;

            return false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(ValidateUsage())
            {
                MessageBox.Show("Form type currently used!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Delete record?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OracleResultSet res = new OracleResultSet();
                res.Query = $"DELETE FROM FORM_TBL WHERE FORM_FLD = '{txtFormType.Text.Trim()}' ";
                if(res.ExecuteNonQuery() == 0)
                { }

                MessageBox.Show("Record Deleted!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);


                string sObj = string.Empty;
                sObj = "Form Type: " + txtFormType.Text;
                if (Utilities.AuditTrail.InsertTrail("COL-SFT-E", "form_tbl", sObj) == 0)
                {
                    MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ClearControls();
                PopulateForms();
            }

        }
    }
}
