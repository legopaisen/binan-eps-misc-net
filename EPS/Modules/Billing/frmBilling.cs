﻿using System;
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

namespace Modules.Billing
{
    public partial class frmBilling : Form
    {
        TaskManager taskman = new TaskManager();
        public string Source { get; set; }
        private FormClass RecordClass = null;
        public string m_sModule = string.Empty;
        public string m_sAN = string.Empty;
        public string PermitCode { get; set; }
        public string ModuleCode { get; set; }

        public frmBilling()
        {
            InitializeComponent();
        }

        private void frmBilling_Load(object sender, EventArgs e)
        {
            PermitList permit = new PermitList(null);
            PermitCode = string.Empty;
            m_sModule = "BILLING";
            PermitCode = permit.GetPermitCode(Source);

            if (Source == "BUILDING PERMIT")
            {
                RecordClass = new Building(this);
            }
            else if (Source == "CERTIFICATE OF OCCUPANCY" || Source == "CERTIFICATE OF ANNUAL INSPECTION")
            {
                RecordClass = new BillCertificate(this);
            }
            else
            {
                RecordClass = new OtherPermit(this);
            }

            RecordClass.FormLoad();
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

            if (!RecordClass.ValidatePermitNo())
                return;

            if (!taskman.AddTask(m_sModule, m_sAN))
                return;

            if (!RecordClass.DisplayData())
            {    
                RecordClass.ClearControls();
                return;
            }
        }

        private void dgvAssessment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RecordClass.CellClick(sender, e);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            RecordClass.ClearControls();
            taskman.RemTask(m_sAN);
            an1.GetCode = "";
            an1.GetTaxYear = "";
            an1.GetMonth = "";
            an1.GetSeries = "";
            m_sAN = "";
        }

        private void dgvAssessment_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            RecordClass.CellLeave(sender, e);
        }

        private void dgvParameter_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnCompute_Click(object sender, EventArgs e)
        {
            if(!RecordClass.Compute())
            {
                MessageBox.Show("Please complete all parameters","Billing",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            RecordClass.ButtonOk();
        }

        private void dgvAssessment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            RecordClass.CellClick(sender, e);
        }

        private void frmBilling_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                taskman.RemTask(m_sAN);
                an1.GetCode = "";
                an1.GetTaxYear = "";
                an1.GetMonth = "";
                an1.GetSeries = "";
                m_sAN = "";

            }
            else
                return;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            RecordClass.Save();
        }

        private void dgvPermit_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            RecordClass.PermitCellClick(sender, e);
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
                    an1.GetTaxYear = "";
                    an1.GetMonth = "";
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
            form.Arn = m_sAN;
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
    }
}
