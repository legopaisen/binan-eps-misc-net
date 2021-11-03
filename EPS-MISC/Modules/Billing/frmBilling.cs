using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Modules.Utilities;
using Modules.Reports;
using EPSEntities.Connection;
using Common.AppSettings;
using Amellar.Common.ImageViewer;
using Common.DataConnector;

namespace Modules.Billing
{
    public partial class frmBilling : Form
    {
        public static ConnectionString dbConn = new ConnectionString();
        TaskManager taskman = new TaskManager();
        public string Source { get; set; }
        private FormClass RecordClass = null;
        public string m_sModule = string.Empty;
        public string m_sAN = string.Empty;
        public string PermitCode { get; set; }
        public string ModuleCode { get; set; }

        protected frmImageList m_frmImageList;
        protected frmImageViewer m_frmImageViewer;
        public static int m_intImageListInstance;

        public frmBilling()
        {
            InitializeComponent();
        }

        private void frmBilling_Load(object sender, EventArgs e)
        {
            //AFM 20201104 COMMENTED - REQUESTED BY BINAN AND RJ 20201103 - NEW BILLING DESIGN AND FLOW (s)
            //PermitList permit = new PermitList(null);
            //PermitCode = string.Empty;
            //PermitCode = permit.GetPermitCode(Source);
            //AFM 20201104 COMMENTED - REQUESTED BY BINAN AND RJ 20201103 - NEW BILLING DESIGN AND FLOW (e)

            m_sModule = "BILLING";
            PopulatePermit();

            if (Source == "CERTIFICATE OF OCCUPANCY" || Source == "CERTIFICATE OF ANNUAL INSPECTION")
            {
                RecordClass = new BillCertificate(this);
            }
            else
            {
                RecordClass = new Building(this);
                EnableControls(false);
            }

            //AFM 20201104 COMMENTED CONDITION - REQUESTED BY BINAN AND RJ 20201103 - NEW BILLING DESIGN AND FLOW (S)
            //if (Source == "BUILDING PERMIT")
            //{
            //    RecordClass = new Building(this);
            //}
            //else if (Source == "CERTIFICATE OF OCCUPANCY" || Source == "CERTIFICATE OF ANNUAL INSPECTION")
            //{
            //    RecordClass = new BillCertificate(this);
            //}
            //else
            //{
            //    RecordClass = new OtherPermit(this);
            //}

            //this.Text = "Billing - " + Source;
            //AFM 20201104 COMMENTED CONDITION - REQUESTED BY BINAN AND RJ 20201103 - NEW BILLING DESIGN AND FLOW (E)


            RecordClass.FormLoad();

            if (AppSettingsManager.GetConfigValue("30") == "1")// AFM 20191016 prompting for build up mode
            {
                MessageBox.Show("System is in Build-up Mode");
                txtBillNo.ReadOnly = false;
            }
            else
            {
                txtBillNo.ReadOnly = true;
                btnImgView.Visible = false;
            }
        }

        public void EnableControls(bool bln)
        {
            groupBox1.Enabled = bln;
            grpAssessment.Enabled = bln;
            btnSave.Enabled = bln;
            grpAddOn.Enabled = bln;
            grpOther.Enabled = bln;
        }

        private void PopulatePermit() //AFM 20201104 REQUESTED BY BINAN AND RJ 20201103 - NEW BILLING DESIGN AND FLOW
        {
            cmbPermit.Items.Clear();
            OracleResultSet result = new OracleResultSet();
            result.Query = "select permit_desc from permit_tbl order by permit_code";
            if (result.Execute())
                while (result.Read())
                    cmbPermit.Items.Add(result.GetString("permit_desc"));
            result.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            m_sAN = an1.GetAn();
            TaskManager taskman = new TaskManager();

            if (string.IsNullOrEmpty(m_sAN))
            {
                SearchAccount.frmSearchARN form = new SearchAccount.frmSearchARN();
                if (Source == "CERTIFICATE OF OCCUPANCY" || Source == "CERTIFICATE OF ANNUAL INSPECTION")
                    form.SearchCriteria = "APP";
                else if (AppSettingsManager.GetConfigValue("30") == "1") //AFM 20200703 set config to 1 to get all arn in application table
                    form.SearchCriteria = "APP";
                else
                    form.SearchCriteria = "QUE";

                form.PermitCode = PermitCode;
                form.ShowDialog();

                an1.SetAn(form.sArn);

                m_sAN = an1.GetAn();
            }
            else
                m_sAN = an1.GetAn();

            if (string.IsNullOrEmpty(m_sAN))
                return;

            if (!RecordClass.ValidatePermitNo())
                return;

            if (!taskman.AddTask(m_sModule, m_sAN))
                return;

            if (AppSettingsManager.GetConfigValue("30") == "1") // validate posted payment in build up mode
            {
                if (RecordClass.ValidatePayment())
                    return;
            }

            if (!RecordClass.DisplayData())
            {
                RecordClass.ClearControls();
                return;
            }
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            RecordClass.ClearControls();
            taskman.RemTask(m_sAN);
            //an1.GetCode = "";
            //an1.GetLGUCode = "";
            an1.GetTaxYear = "";
            //an1.GetMonth = ""; // disabled for new arn of binan
            //an1.GetDistCode = "";
            an1.GetSeries = "";
            m_sAN = "";
        }

        private void dgvAssessment_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            //RecordClass.CellLeave(sender, e); //remove test
        }

        private void dgvParameter_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnCompute_Click(object sender, EventArgs e)
        {
            if (!RecordClass.Compute())
            {
                MessageBox.Show("Please complete all parameters", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            RecordClass.ButtonOk();
        }

        private void dgvAssessment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //bool entry = false;
            //for (int row = 0; row < dgvPermit.Rows.Count; row++)
            //{
            //    if ((bool)dgvPermit[0, row].Value == true)
            //    {
            //    }
            //}
            RecordClass.CellClick(sender, e);
        }

        private void frmBilling_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                taskman.RemTask(m_sAN);
                an1.GetCode = "";
                //an1.GetLGUCode = "";
                an1.GetTaxYear = "";
                //an1.GetMonth = ""; // disabled for new arn of binan
                //an1.GetDistCode = "";
                an1.GetSeries = "";
                m_sAN = "";

            }
            else
                return;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtAmtDue.Text == "0.00" || txtAmtDue.Text == "" || txtAmtDue.Text == string.Empty) //AFM 20191015 uncomputed fees validation
            {
                MessageBox.Show("No fees computed");
                return;
            }
            RecordClass.Save();
        }

        private bool ValidatePermit()
        {
            for (int i = 0; i < dgvPermit.Rows.Count; i++)
            {
                if ((bool)dgvPermit[0, i].Value == true)
                {
                    return true;
                }
            }
            return false;
        }

        private void dgvPermit_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (ValidatePermit())
            {
                btnSave.Enabled = true;
            }
            else { btnSave.Enabled = false; }

            try
            {
                if ((bool)dgvPermit.CurrentRow.Cells[0].Value == true)
                {
                    dgvAssessment.Enabled = true;
                    dgvAddOnFees.Enabled = true;
                    dgvOtherFees.Enabled = true;
                }
                else
                {
                    dgvAssessment.Enabled = false;
                    dgvAddOnFees.Enabled = false;
                    dgvOtherFees.Enabled = false;
                }
            }
            catch { }
        }

        private void dgvPermit_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ValidatePermit())
            {
                btnSave.Enabled = true;
            }
            else { btnSave.Enabled = false; }

            try
            {
                if ((bool)dgvPermit.CurrentRow.Cells[0].Value == true)
                {
                    dgvAssessment.Enabled = true;
                    dgvAddOnFees.Enabled = true;
                    dgvOtherFees.Enabled = true;
                }
                else
                {
                    dgvAssessment.Enabled = false;
                    dgvAddOnFees.Enabled = false;
                    dgvOtherFees.Enabled = false;
                }
            }
            catch { }
            try
            {
                RecordClass.PermitCellClick(sender, e);
                //AFM 20190911 permit checkbox (s)
                if ((bool)dgvPermit.CurrentCell.Value != true && (bool)dgvPermit.CurrentCell.Value != false) return;
                if ((bool)dgvPermit.CurrentRow.Cells[0].Value == true)
                    dgvPermit.CurrentRow.Cells[0].Value = false;
                else if ((bool)dgvPermit.CurrentRow.Cells[0].Value == false)
                    dgvPermit.CurrentCell.Value = true;
                //AFM 20190911 permit checkbox (e)
            }
            catch { }
            RecordClass.PermitCellClick(sender, e);
            RemoveUnbilled();
        }

        private void RemoveUnbilled() //AFM 20190913
        {
            var db = new EPSConnection(dbConn);
            string sQuery = string.Empty;
            List<string> feesCode = new List<string>();
            string PermitDlt = string.Empty;
            bool remove = false;
            for (int cnt = 0; cnt < dgvPermit.Rows.Count; cnt++)
            {
                if ((bool)dgvPermit[0, cnt].Value == false)
                {
                    if (dgvPermit[0, cnt].Selected == true)
                    {
                        for (int list = 0; list < dgvAssessment.Rows.Count; list++)
                        {
                            if ((bool)dgvAssessment[0, list].Value == true)
                            {
                                feesCode.Add(dgvAssessment[2, list].Value.ToString());
                            }
                        }
                    }
                }
            }
            foreach (var s in feesCode)
            {
                sQuery = $"delete from bill_tmp where arn = '{m_sAN}' and fees_code = '{s}'";
                db.Database.ExecuteSqlCommand(sQuery);
                remove = true;
            }
            dgvPermit.Refresh();
            if (remove == true)
            {
                for (int i = 0; i < dgvAssessment.Rows.Count; i++)
                {
                    dgvAssessment[0, i].Value = false;
                    dgvAssessment[7, i].Value = "0";
                    dgvAssessment[8, i].Value = "0";
                    dgvAssessment[11, i].Value = "0";

                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "Exit")
                this.Close();
            else
            {
                if (MessageBox.Show("Are you sure you want to cancel transaction?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    RecordClass.ClearControls();
                    taskman.RemTask(m_sAN);
                    an1.GetCode = "";
                    //an1.GetLGUCode = "";
                    an1.GetTaxYear = "";
                    //an1.GetMonth = ""; // disabled for new arn of binan
                    //an1.GetDistCode = "";
                    an1.GetSeries = "";
                    m_sAN = "";
                    this.Close();
                }
                else
                    return;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            frmReport form = new frmReport();
            form.ReportName = "ORDER OF PAYMENT";
            form.An = m_sAN;
            form.ShowDialog();
        }

        private void btnAddlAdd_Click(object sender, EventArgs e)
        {
            RecordClass.AdditionalFeesAdd(sender, e);
        }

        private void txtAddlAmt_Leave(object sender, EventArgs e)
        {
            double dAmt = 0;
            double.TryParse(txtAddlAmt.Text.ToString(), out dAmt);

            txtAddlAmt.Text = string.Format("{0:#,###.00}", dAmt);
        }

        private void dgvAssessment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnImgView_Click(object sender, EventArgs e) // view image
        {
            m_intImageListInstance = 0;
            m_frmImageList = new frmImageList();
            m_frmImageList.IsBuildUpPosting = true;

            if (m_frmImageList.ValidateImage(m_sAN, AppSettingsManager.GetSystemType)) //MCR 20141209
            {
                ImageInfo objImageInfo;
                objImageInfo = new ImageInfo();

                objImageInfo.TRN = m_sAN;
                //objImageInfo.System = "A"; 
                objImageInfo.System = AppSettingsManager.GetSystemType; //MCR 20121209
                m_frmImageList.isFortagging = false;
                m_frmImageList.setImageInfo(objImageInfo);
                m_frmImageList.Text = m_sAN;
                m_frmImageList.IsAutoDisplay = true;
                m_frmImageList.Source = "VIEW";
                m_frmImageList.Show();
                m_intImageListInstance += 1;
            }
            else
            {

                MessageBox.Show(string.Format("ARN {0} has no image", m_sAN));
            }

        }

        private void cmbPermit_SelectedIndexChanged(object sender, EventArgs e) //AFM 20201104 REQUESTED BY BINAN AND RJ 20201103 - NEW BILLING DESIGN AND FLOW
        {
            PermitList permit = new PermitList(null);
            string sPermitAcro = string.Empty;

            this.btnClear.PerformClick();
            PermitCode = string.Empty;
            Source = cmbPermit.Text;
            PermitCode = permit.GetPermitCode(Source);
            EnableControls(true);

            sPermitAcro = this.an1.ANCodeGenerator(PermitCode);
            this.an1.SetAn(sPermitAcro);
        }

        private void dgvAddOn_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RecordClass.CellClickAddOnCELL(sender, e);
        }

        private void btnAddOnAmt_Click(object sender, EventArgs e)
        {
            RecordClass.AdditionalFeesAddAddOn(sender, e);
        }

        private void dgvOtherFees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RecordClass.CellClickOthersCELL(sender, e);
        }

        private void dgvAddOn_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            RecordClass.CellClickAddOn(sender, e);
        }

        private void dgvOtherFees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            RecordClass.CellClickOthers(sender, e);
        }

        private void btnOtherAdd_Click(object sender, EventArgs e)
        {
            RecordClass.OtherFeesAddAddOn(sender, e);
        }
    }
}
