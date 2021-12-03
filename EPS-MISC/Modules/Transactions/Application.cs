﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Utilities;
using System.Data;
using Common.AppSettings;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using Modules.SearchAccount;
using EPSEntities.Connection;
using Modules.Records;
using RPTEntities.Connection;
using RPTEntities.Entity;
using ARCSEntities.Connection;
using ARCSEntities.Entity;
using Common.DataConnector;

namespace Modules.Transactions
{
    public class Application : RecordForm
    {
        public static ConnectionString dbConn = new ConnectionString();
        public static ARCSConnectionString dbConnArcs = new ARCSConnectionString();
        public static RPTConnectionString dbRPTConn = new RPTConnectionString();
        TaskManager taskman = new TaskManager();

        private bool m_isNotSaved = false;

        public Application(frmRecords Form) : base(Form)
        { }

        public override void FormLoad()
        {
            RecordFrm.lblApplDate.Text = "Date of Application";
            RecordFrm.dtpDateApproved.Visible = false;
            RecordFrm.lblAppvdDate.Visible = false;
            RecordFrm.EnableControl(false);

            if (RecordFrm.SourceClass == "NEW_ADD")
            {
                RecordFrm.arn1.Enabled = false;
                RecordFrm.Text = "New Application";
            }
            else if (RecordFrm.SourceClass == "NEW_EDIT")
            {
                RecordFrm.arn1.Enabled = true;
                RecordFrm.arn1.ArnCode.Enabled = true;
                RecordFrm.Text = "New Application - Edit";
            }
            else if (RecordFrm.SourceClass == "NEW_VIEW")
            {
                RecordFrm.arn1.Enabled = true;
                RecordFrm.arn1.ArnCode.Enabled = true;
                RecordFrm.Text = "New Application - View";
            }
            else if(RecordFrm.SourceClass == "NEW_CANCEL")
            {
                RecordFrm.arn1.Enabled = true;
                RecordFrm.arn1.ArnCode.Enabled = true;
                RecordFrm.Text = "New Application - Cancel";
            }
            else if (RecordFrm.SourceClass == "REN_ADD")
            {
                RecordFrm.arn1.Enabled = false;
                RecordFrm.Text = "Renewal Application";
            }
            else if (RecordFrm.SourceClass == "REN_EDIT")
            {
                RecordFrm.arn1.Enabled = false;
                RecordFrm.arn1.ArnCode.Enabled = true;
                RecordFrm.Text = "Renewal Application - Edit";
            }
            else if (RecordFrm.SourceClass == "REN_VIEW")
            {
                RecordFrm.arn1.Enabled = false;
                RecordFrm.arn1.ArnCode.Enabled = true;
                RecordFrm.Text = "Renewal Application - View";
            }
            else if (RecordFrm.SourceClass == "REN_CANCEL")
            {
                RecordFrm.arn1.Enabled = false;
                RecordFrm.arn1.ArnCode.Enabled = true;
                RecordFrm.Text = "Renewal Application - Cancel";
            }

            ClearControl();
        }

        public override void PopulatePermit()
        {
            RecordFrm.cmbPermit.Items.Clear();

            m_lstPermit = new PermitList("where other_type = 'FALSE' and permit_desc not like 'CERTIFICATE%'");
            int iCnt = m_lstPermit.PermitLst.Count;

            DataTable dataTable = new DataTable("Permit");
            dataTable.Columns.Clear();
            dataTable.Columns.Add("PermitCode", typeof(String));
            dataTable.Columns.Add("PermitDesc", typeof(String));

            dataTable.Rows.Add(new String[] { "", "" });

            for (int i = 0; i < iCnt; i++)
            {
                dataTable.Rows.Add(new String[] { m_lstPermit.PermitLst[i].PermitCode, m_lstPermit.PermitLst[i].PermitDesc });
            }

            RecordFrm.cmbPermit.DataSource = dataTable;
            RecordFrm.cmbPermit.DisplayMember = "PermitDesc";
            RecordFrm.cmbPermit.ValueMember = "PermitDesc";
            RecordFrm.cmbPermit.SelectedIndex = 0;
        }

        public override void ClearControl()
        {
            RecordFrm.cmbPermit.Text = "";
            RecordFrm.cmbScope.Text = "";
            RecordFrm.dtDateApplied.Text = AppSettingsManager.GetCurrentDate().ToShortDateString();
            RecordFrm.dtpDateApproved.Text = AppSettingsManager.GetCurrentDate().ToShortDateString();
            RecordFrm.ButtonAdd.Text = "Save";
            RecordFrm.ButtonEdit.Text = "Update";
            RecordFrm.ButtonExit.Text = "Exit";
            RecordFrm.ButtonClear.Visible = true;
            
            RecordFrm.ButtonDelete.Enabled = false;
            RecordFrm.ButtonPrint.Enabled = true;
            RecordFrm.ButtonExit.Enabled = true;

            RecordFrm.dtDateApplied.Enabled = false;
            RecordFrm.tabControl1.Enabled = false;

            RecordFrm.formProject.ClearControls();
            RecordFrm.formBldgDate.ClearControls();
            RecordFrm.formStrucOwn.ClearControls();
            RecordFrm.formLotOwn.ClearControls();
            RecordFrm.formEngr.ClearControls();
            if (RecordFrm.SourceClass == "NEW_ADD")
            {
                RecordFrm.ButtonAdd.Enabled = true;
                RecordFrm.ButtonEdit.Enabled = false;
                RecordFrm.cmbPermit.Enabled = true;
                RecordFrm.cmbScope.Enabled = true;
                RecordFrm.btnSearch.Enabled = false;
                RecordFrm.btnClear.Enabled = false;
            }
            else if(RecordFrm.SourceClass == "NEW_EDIT")
            {
                RecordFrm.cmbPermit.Enabled = false;
                RecordFrm.cmbScope.Enabled = false;
                RecordFrm.ButtonAdd.Enabled = false;
                RecordFrm.ButtonEdit.Enabled = true;
                RecordFrm.ButtonSearch.Enabled = true;
            }
            else if(RecordFrm.SourceClass == "NEW_VIEW")
            {
                RecordFrm.cmbPermit.Enabled = false;
                RecordFrm.cmbScope.Enabled = false;
                RecordFrm.ButtonAdd.Enabled = false;
                RecordFrm.ButtonEdit.Enabled = false;
                RecordFrm.ButtonSearch.Enabled = true;
            }
            else if (RecordFrm.SourceClass == "NEW_CANCEL")
            {
                RecordFrm.cmbPermit.Enabled = false;
                RecordFrm.cmbScope.Enabled = false;
                RecordFrm.ButtonAdd.Enabled = false;
                RecordFrm.ButtonEdit.Enabled = false;
                RecordFrm.ButtonSearch.Enabled = true;
                RecordFrm.ButtonDelete.Enabled = true;
                RecordFrm.ButtonDelete.Text = "Cancel App";
            }
            else if (RecordFrm.SourceClass == "REN_ADD")
            {
                RecordFrm.ButtonAdd.Enabled = true;
                RecordFrm.ButtonEdit.Enabled = false;
                RecordFrm.cmbPermit.Enabled = false;
                RecordFrm.cmbScope.Enabled = false;
                RecordFrm.btnSearch.Enabled = true;
                RecordFrm.btnClear.Enabled = true;
            }
            else if (RecordFrm.SourceClass == "REN_EDIT")
            {
                RecordFrm.ButtonAdd.Enabled = false;
                RecordFrm.ButtonEdit.Enabled = true;
                RecordFrm.cmbPermit.Enabled = false;
                RecordFrm.cmbScope.Enabled = false;
                RecordFrm.btnSearch.Enabled = true;
                RecordFrm.btnClear.Enabled = true;
            }
            else if (RecordFrm.SourceClass == "REN_VIEW")
            {
                RecordFrm.ButtonAdd.Enabled = false;
                RecordFrm.ButtonEdit.Enabled = false;
                RecordFrm.cmbPermit.Enabled = false;
                RecordFrm.cmbScope.Enabled = false;
                RecordFrm.btnSearch.Enabled = true;
                RecordFrm.btnClear.Enabled = true;
            }
            else if (RecordFrm.SourceClass == "REN_CANCEL")
            {
                RecordFrm.ButtonAdd.Enabled = false;
                RecordFrm.ButtonEdit.Enabled = false;
                RecordFrm.cmbPermit.Enabled = false;
                RecordFrm.cmbScope.Enabled = false;
                RecordFrm.btnSearch.Enabled = true;
                RecordFrm.btnClear.Enabled = true;
                RecordFrm.ButtonDelete.Enabled = true;
                RecordFrm.ButtonDelete.Text = "Cancel App";
            }

            taskman.RemTask(RecordFrm.AN);
            //RecordFrm.arn1.GetMonth = ""; // disabled for new arn of binan
            RecordFrm.arn1.GetSeries = "";
            //RecordFrm.arn1.Clear();
        }

        public override void EnableRecordEntry()
        {
            if (RecordFrm.SourceClass != "NEW_VIEW" && RecordFrm.SourceClass != "REN_VIEW")
            {
                RecordFrm.tabControl1.Enabled = true;
                RecordFrm.ButtonExit.Text = "Cancel";
                RecordFrm.ButtonPrint.Enabled = false;
            }
        }

        public override void ButtonAddClick(string sender)
        {
            if (RecordFrm.ValidateData())
                Save();
            //MessageBox.Show("Record saved", RecordFrm.DialogText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        public override void ButtonEditClick(string sender)
        {
            if (RecordFrm.ValidateData())
                Save();
            //MessageBox.Show("Record updated", RecordFrm.DialogText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        public override void Save()
        {
            var db = new EPSConnection(dbConn);
            string strQuery = string.Empty;
            m_isNotSaved = false;

            strQuery = $"delete from application_que where arn = '{RecordFrm.AN}'";
            db.Database.ExecuteSqlCommand(strQuery);

            //if (string.IsNullOrEmpty(RecordFrm.ARN))

            if (RecordFrm.SourceClass == "NEW_ADD")
                //RecordFrm.arn1.CreateAN(RecordFrm.cmbPermit.Text.ToString());
                RecordFrm.arn1.CreateAN(((DataRowView)RecordFrm.cmbPermit.SelectedItem)["PermitCode"].ToString()); // requested new eps format

            if (RecordFrm.SourceClass == "REN_ADD")
                //RecordFrm.arn1.CreateAN(RecordFrm.cmbPermit.Text.ToString());
                RecordFrm.arn1.CreateAN(((DataRowView)RecordFrm.cmbPermit.SelectedItem)["PermitCode"].ToString()); // requested new eps format

            if (string.IsNullOrEmpty(RecordFrm.AN))
            {
                MessageBox.Show("Error generating application no.\nPlease check permit table",RecordFrm.DialogText,MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }

            if (AppSettingsManager.GetConfigValue("25") == "Y" || AppSettingsManager.GetConfigValue("25") == "1")
            {
                if (ValidateLotDelinquency())
                {
                    if (MessageBox.Show("Land property is delinquent.\nAre you sure you want to continue?", RecordFrm.DialogText, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        MessageBox.Show("Application not saved", RecordFrm.DialogText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    if (Utilities.AuditTrail.InsertTrail("TA-LD", "APPLICATION_QUE", "ARN: " + RecordFrm.AN) == 0)
                    {
                        MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            

            for (int i = 0; i < RecordFrm.formBldgDate.dgvList.Rows.Count; i++)
            {
                string sPermitCode = string.Empty;
                string sPermit = string.Empty;
                string sPermitNo = string.Empty;
                string sStart = string.Empty;
                string sCompleted = string.Empty;

                m_lstPermit = new PermitList(null);

                try { sPermitCode = RecordFrm.formBldgDate.dgvList[0, i].Value.ToString(); }
                catch { }
                try
                {
                    if (RecordFrm.formBldgDate.dgvList[1, i] != null)
                        sPermit = RecordFrm.formBldgDate.dgvList[1, i].Value.ToString();

                    if (string.IsNullOrEmpty(sPermitCode))
                        sPermitCode = m_lstPermit.GetPermitCode(sPermit);
                }
                catch { }
                try
                {
                    if (RecordFrm.formBldgDate.dgvList[2, i] != null)
                        sPermitNo = RecordFrm.formBldgDate.dgvList[2, i].Value.ToString();
                }
                catch { }

                try { 
                if (RecordFrm.formBldgDate.dgvList[3, i] != null)
                    sStart = RecordFrm.formBldgDate.dgvList[3, i].Value.ToString();
                }
                catch { }
                try { 
                if (RecordFrm.formBldgDate.dgvList[4, i] != null)
                    sCompleted = RecordFrm.formBldgDate.dgvList[4, i].Value.ToString();
                }
                catch { }

                if (!string.IsNullOrEmpty(sPermit) 
                    && !string.IsNullOrEmpty(sStart) && !string.IsNullOrEmpty(sCompleted))
                {
                    PermitList permit = new PermitList(null);
                    sPermitNo = permit.GetPermitAppCode(sPermit);

                    GetArchEngr(sPermit);
                    int iMainApp = 0;
                    if (sPermit.Contains("BUILDING"))
                        iMainApp = 1;

                    string sStruc = string.Empty;
                    string sScope = string.Empty;
                    string sCat = string.Empty;
                    string sOccu = string.Empty;
                    string sBrgy = string.Empty;

                    try { sStruc = ((DataRowView)RecordFrm.formProject.cmbStrucType.SelectedItem)["Code"].ToString(); } catch { sStruc = ""; }
                    try { sScope = ((DataRowView)RecordFrm.cmbScope.SelectedItem)["ScopeCode"].ToString(); } catch { sScope = ""; }
                    try { sCat = ((DataRowView)RecordFrm.formProject.cmbCategory.SelectedItem)["Code"].ToString(); } catch { sCat = ""; }
                    try { sOccu = ((DataRowView)RecordFrm.formProject.cmbOccupancy.SelectedItem)["Code"].ToString(); } catch { sOccu = ""; }
                    try { sBrgy = ((DataRowView)RecordFrm.formProject.cmbBrgy.SelectedItem)["Desc"]?.ToString(); } catch { sBrgy = ""; }

                    DateTime dtStart;
                    DateTime dtCompleted;
                    string sdTApplied = string.Empty;

                    // convert properly to prevent error
                    try
                    {
                        dtStart = Convert.ToDateTime(RecordFrm.formBldgDate.dgvList[3, i].Value);
                        sStart = dtStart.ToShortDateString();
                    }
                    catch { }

                    try
                    {
                        dtCompleted = Convert.ToDateTime(RecordFrm.formBldgDate.dgvList[3, i].Value);
                        sCompleted = dtCompleted.ToShortDateString();
                    }
                    catch { }

                    try
                    {
                        sdTApplied = RecordFrm.dtDateApplied.Value.ToShortDateString();
                    }
                    catch { }

                    try
                    {
                        strQuery = $"insert into application_que values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12,:13,:14,:15,:16,:17,:18,:19,:20,:21,:22,:23,:24,to_date(:25,'MM/dd/yyyy'),to_date(:26,'MM/dd/yyyy'),to_date(:27,'MM/dd/yyyy'),:28,:29,:30,:31)";
                        db.Database.ExecuteSqlCommand(strQuery,
                            new OracleParameter(":1", RecordFrm.AN),
                            new OracleParameter(":2", RecordFrm.formProject.txtProjDesc.Text.Trim()),
                            new OracleParameter(":3", sPermitCode),
                            new OracleParameter(":4", sPermitNo),
                            new OracleParameter(":5", sStruc),
                            new OracleParameter(":6", RecordFrm.formBldgDate.txtBldgNo.Text.Trim()),
                            new OracleParameter(":7", RecordFrm.Status),
                            new OracleParameter(":8", sScope),
							new OracleParameter(":9", sCat),
							new OracleParameter(":10", sOccu),
                            new OracleParameter(":11", RecordFrm.formProject.txtLotNo.Text.Trim()),
                            new OracleParameter(":12", RecordFrm.formProject.txtHseNo.Text.Trim()),
                            new OracleParameter(":13", RecordFrm.formProject.txtLotNo.Text.Trim()),
                            new OracleParameter(":14", RecordFrm.formProject.txtBlkNo.Text.Trim()),
                            new OracleParameter(":15", RecordFrm.formProject.txtStreet.Text.Trim()),
							new OracleParameter(":16", sBrgy),
                            new OracleParameter(":17", RecordFrm.formProject.txtMun.Text.Trim()),
                            new OracleParameter(":18", RecordFrm.formProject.txtProv.Text.Trim()),
                            new OracleParameter(":19", RecordFrm.formProject.txtZIP.Text.Trim()),
                            new OracleParameter(":20", RecordFrm.formStrucOwn.StrucAcctNo),
                            new OracleParameter(":21", RecordFrm.formLotOwn.LotAcctNo),
                            new OracleParameter(":22", RecordFrm.formProject.cmbOwnership.Text.ToString()),
                            new OracleParameter(":23", m_sEngr),
                            new OracleParameter(":24", m_sArch),
                            //new OracleParameter(":25", string.Format("{0:MM/dd/yyyy}", sStart)),
                            new OracleParameter(":25", sStart),
                            //new OracleParameter(":26", string.Format("{0:MM/dd/yyyy}", sCompleted)),
                            new OracleParameter(":26", sCompleted),
                            new OracleParameter(":27", sdTApplied),//string.Format("{0:MM/dd/yyyy}", RecordFrm.dtDateApplied.Value.ToShortDateString())),
                            new OracleParameter(":28", null),
                            new OracleParameter(":29", RecordFrm.formProject.txtMemo.Text.ToString()),
                            new OracleParameter(":30", iMainApp),
                            new OracleParameter(":31", RecordFrm.formProject.txtVillage.Text.ToString().Trim())); //ADDED REQUESTED subdivision

                    }
                    catch (Exception ex) // catches any error
                    {
                        MessageBox.Show(ex.Message.ToString());
                        m_isNotSaved = true;
                    }


                }
            }

            if(m_isNotSaved == false)
            {
                SaveBuilding();
                if (this.RecordFrm.SourceClass == "NEW_ADD")
                {
                    if (Utilities.AuditTrail.InsertTrail("TNA-SAVE", "APPLICATION_QUE", "ARN: " + RecordFrm.AN) == 0)
                    {
                        MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (this.RecordFrm.SourceClass == "REN_ADD")
                {
                    if (Utilities.AuditTrail.InsertTrail("TRA-SAVE", "APPLICATION_QUE", "ARN: " + RecordFrm.AN) == 0)
                    {
                        MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (this.RecordFrm.SourceClass == "NEW_EDIT")
                {
                    if (Utilities.AuditTrail.InsertTrail("TEA-UPDATE", "APPLICATION_QUE", "ARN: " + RecordFrm.AN) == 0)
                    {
                        MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (this.RecordFrm.SourceClass == "REN_EDIT")
                {
                    if (Utilities.AuditTrail.InsertTrail("TER-UPDATE", "APPLICATION_QUE", "ARN: " + RecordFrm.AN) == 0)
                    {
                        MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (this.RecordFrm.SourceClass == "NEW_ADD" || this.RecordFrm.SourceClass == "REN_ADD")
                    MessageBox.Show("ARN: " + RecordFrm.AN + "  is successfully saved.", RecordFrm.DialogText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Changes successfully saved.", RecordFrm.DialogText, MessageBoxButtons.OK, MessageBoxIcon.Information);


                RecordFrm.EnableControl(false);

                RecordFrm.ButtonAdd.Text = "Save";
                RecordFrm.ButtonEdit.Text = "Update";
                RecordFrm.ButtonExit.Text = "Exit";
                RecordFrm.ButtonAdd.Enabled = false;
                RecordFrm.ButtonPrint.Enabled = true;

                /*if (MessageBox("Print application form?", APP_NAME, MB_YESNO | MB_ICONQUESTION) == IDYES)
                {
                    CAppFormPrint printdlg;
                    printdlg.m_sARN = m_sARN;
                    printdlg.m_sAppForm = mv_sPermit;
                    printdlg.DoModal();
                }*/
            }
            else
                MessageBox.Show("ARN: " + RecordFrm.AN + "  is not saved.", RecordFrm.DialogText, MessageBoxButtons.OK, MessageBoxIcon.Warning);


        }

        private bool ValidateLotDelinquency()
        {
            var db = new RPTConnection(dbRPTConn);
            bool bReturn = false;

            string strQuery = string.Empty;
            int intYear = 0;

            strQuery = $"select decode(max(tax_year), null, 0, max(tax_year)) as tax_year from payment_hist where pin = '{RecordFrm.formProject.txtLotNo.Text.ToString()}' order by tax_year desc";
            var rec = db.Database.SqlQuery<PAYMENT_HIST>(strQuery);

            foreach (var items in rec)
            {
                intYear = items.TaxYear;

                if (intYear < RecordFrm.dtDateApplied.Value.Year)
                {
                    bReturn = true;
                }

            }

            return bReturn;

        }

        public override void ButtonSearchClick()
        {
            //if (string.IsNullOrEmpty(RecordFrm.arn1.GetTaxYear) && string.IsNullOrEmpty(RecordFrm.arn1.GetSeries))
            //if (string.IsNullOrEmpty(RecordFrm.arn1.GetMonth) && string.IsNullOrEmpty(RecordFrm.arn1.GetSeries)) // disabled for new arn of binan
            if (string.IsNullOrEmpty(RecordFrm.arn1.GetTaxYear) && string.IsNullOrEmpty(RecordFrm.arn1.GetSeries)) // disabled for new arn of binan
            {
                SearchAccount.frmSearchARN form = new SearchAccount.frmSearchARN();

                if (RecordFrm.SourceClass == "NEW_EDIT" || RecordFrm.SourceClass == "NEW_VIEW" || RecordFrm.SourceClass == "NEW_CANCEL")
                    form.SearchCriteria = "QUE-NEW";
                else if (RecordFrm.SourceClass == "REN_EDIT" || RecordFrm.SourceClass == "REN_VIEW" || RecordFrm.SourceClass == "REN_CANCEL")
                    form.SearchCriteria = "QUE-REN";
                else
                    form.SearchCriteria = "APP";
                form.ShowDialog();

                RecordFrm.arn1.SetAn(form.sArn);
            }

            if (!taskman.AddTask(RecordFrm.SourceClass, RecordFrm.AN))
                return;

            DisplayData();

            RecordFrm.tabControl1.Enabled = true;
            if (RecordFrm.SourceClass == "NEW_VIEW" || RecordFrm.SourceClass == "NEW_CANCEL"
                || RecordFrm.SourceClass == "REN_VIEW" || RecordFrm.SourceClass == "REN_CANCEL")
            {
                RecordFrm.formProject.EnableFormControls(false);
                RecordFrm.formBldgDate.EnableFormControls(false);
                RecordFrm.formStrucOwn.EnableFormControls(false);
                RecordFrm.formLotOwn.EnableFormControls(false);
                RecordFrm.formEngr.EnableFormControls(false);
            }
        }

        public override void DisplayData()
        {
            var db = new EPSConnection(dbConn);
            string strWhereCond = string.Empty;

            if (!string.IsNullOrEmpty(RecordFrm.AN))
                RecordFrm.arn1.Enabled = false;

            var result = (dynamic)null;

            if (RecordFrm.SourceClass == "REN_ADD")
            {
                strWhereCond = $" where arn = '{RecordFrm.AN}' and main_application = 1";
                result = from a in Records.ApplicationTblList.GetRecord(strWhereCond)
                         select a;
            }
            else
            {
                strWhereCond = $" where arn = '{RecordFrm.AN}' and main_application = 1";
                if (RecordFrm.SourceClass == "NEW_EDIT" || RecordFrm.SourceClass == "NEW_VIEW" || RecordFrm.SourceClass == "NEW_CANCEL")
                    strWhereCond += $" and status_code = 'NEW'";
                else
                    strWhereCond += $" and status_code = 'REN'";

                result = from a in Records.ApplicationQueList.GetApplicationQue(strWhereCond)
                             select a;
            }
            int iBldgNo = 0;

            foreach (var item in result)
            {
                Structure struc = new Structure();
                Category cat = new Category();
                Business buss = new Business();

                string sStatus = string.Empty;
                string sPermit = string.Empty;
                string sScope = string.Empty;
                string sArchitect = string.Empty;
                DateTime dtTmp;
                string sDateStart = string.Empty;
                string sDateComp = string.Empty;

                if (RecordFrm.SourceClass == "REN_ADD")
                {
                    RecordFrm.dtDateApplied.Value = AppSettingsManager.GetCurrentDate();
                }
                else
                {
                    DateTime.TryParse(item.DATE_APPLIED.ToString(), out dtTmp);
                    RecordFrm.dtDateApplied.Value = dtTmp;
                }
                RecordFrm.formProject.txtProjDesc.Text = item.PROJ_DESC;
                RecordFrm.formProject.cmbStrucType.Text = struc.GetStructureDesc(item.STRUC_CODE);
                iBldgNo = item.BLDG_NO;

                sStatus = item.STATUS_CODE;
                RecordFrm.formProject.txtPIN.Text = item.BNS_CODE;
                RecordFrm.formProject.txtHseNo.Text = item.PROJ_HSE_NO;
                RecordFrm.formProject.txtLotNo.Text = item.PROJ_LOT_NO;
                RecordFrm.formProject.txtBlkNo.Text = item.PROJ_BLK_NO;
                RecordFrm.formProject.txtStreet.Text = item.PROJ_ADDR;
                RecordFrm.formProject.txtZIP.Text = item.PROJ_ZIP;
                sPermit = item.PERMIT_CODE;
                sScope = item.SCOPE_CODE;

                string sWhereClause = " where permit_code = '";
                sWhereClause += sPermit;
                sWhereClause += "'";
                PermitList permitlist = new PermitList(sWhereClause);
                RecordFrm.cmbPermit.Text = permitlist.PermitLst[0].PermitDesc;

                ScopeList scopelist = new ScopeList();
                RecordFrm.cmbScope.Text = scopelist.ScopeLst[0].ScopeDesc;

                m_sLotOwner = item.PROJ_LOT_OWNER;
                m_sStrucOwner = item.PROJ_OWNER;
                sArchitect = item.ARCHITECT;

                RecordFrm.formProject.cmbOwnership.Text = item.OWN_TYPE;
                DateTime.TryParse(item.PROP_START.ToString(), out dtTmp);
                sDateStart = dtTmp.ToShortDateString();
                DateTime.TryParse(item.PROP_COMPLETE?.ToString(), out dtTmp);
                sDateComp = dtTmp.ToShortDateString();

                RecordFrm.formProject.txtMemo.Text = item.MEMO;
                RecordFrm.formProject.cmbBrgy.Text = item.PROJ_BRGY;
                RecordFrm.formProject.txtMun.Text = item.PROJ_CITY;
                RecordFrm.formProject.txtProv.Text = item.PROJ_PROV;

                RecordFrm.formProject.cmbCategory.Text = cat.GetCategoryDesc(item.CATEGORY_CODE);
                OccupancyList lstOccupancy = new OccupancyList(item.CATEGORY_CODE,item.OCCUPANCY_CODE);
                RecordFrm.formProject.cmbOccupancy.Text = lstOccupancy.OccupancyLst[0].Desc;
                RecordFrm.formProject.cmbBussKind.Text = buss.GetBusinessDesc(item.BNS_CODE);
                
            }

            if (string.IsNullOrEmpty(RecordFrm.formProject.txtProjDesc.Text.ToString()))
            {
                MessageBox.Show("No record found", RecordFrm.DialogText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                RecordFrm.arn1.Enabled = true; //AFM 20200811
                ClearControl();
                return;
            }
            
            DisplayBuilding(iBldgNo);
            DisplayOwners();
            DisplayEngrArch();

            if (RecordFrm.SourceClass == "REN_ADD")
            {
                RecordFrm.cmbScope.Enabled = true;
                RecordFrm.cmbScope.Text = "";
                RecordFrm.cmbPermit.Enabled = true;

                RecordFrm.arn1.GetTaxYear = "";
                //RecordFrm.arn1.GetMonth = ""; // disabled for new arn of binan
                //RecordFrm.arn1.GetDistCode = "";
                RecordFrm.arn1.GetSeries = "";
            }
            if(RecordFrm.SourceClass == "REN_EDIT")
            {
                RecordFrm.cmbScope.Enabled = true;
                RecordFrm.cmbPermit.Enabled = true;
            }
        }

        public override void DisplayBuilding(int iBldgNo)
        {
            var db = new EPSConnection(dbConn);
            string sQuery = string.Empty;

            var result = from a in Records.Building.GetBuildingRecord(iBldgNo)
                         select a;

            foreach (var item in result)
            {
                Material mat = new Material();
                //m_ppBldgDates.mv_sAncillaryArea = pApp->GetStrVariant(setTable->GetCollect("ancillary_area"));
                //m_sActualCost = pApp->GetStrVariant(setTable->GetCollect("actual_cost"));
                //m_sDateCompleted = pApp->GetStrVariant(setTable->GetCollect("date_completed"));
                RecordFrm.formBldgDate.txtBldgNo.Text = iBldgNo.ToString();
                RecordFrm.formBldgDate.txtBldgName.Text = item.BLDG_NM;
                RecordFrm.formBldgDate.txtPIN.Text = item.LAND_PIN;
                RecordFrm.formBldgDate.txtHeight.Text = item.BLDG_HEIGHT.ToString("#,###.00");
                RecordFrm.formBldgDate.txtArea.Text = item.TOTAL_FLR_AREA.ToString("#,###.00");
                RecordFrm.formBldgDate.txtUnits.Text = item.NO_UNITS.ToString("#,###");
                RecordFrm.formBldgDate.txtStoreys.Text = item.NO_STOREYS.ToString("#,###");
                RecordFrm.formBldgDate.txtCost.Text = item.EST_COST.ToString("#,###.00");
                RecordFrm.formBldgDate.cmbMaterials.Text = mat.GetMaterialDesc(item.MATERIAL_CODE);
                RecordFrm.formBldgDate.txtAssVal.Text = item.ASS_VAL.ToString("#,###.00");

                //m_ppBldgDates.m_sPermitCode = mc_cbPermit.GetItemText(mc_cbPermit.GetCurSel(), 1);
            }

            if (RecordFrm.SourceClass == "REN_ADD")
            { }
            else
            {
                string strWhereCond = string.Empty;

                strWhereCond = $" where arn = '{RecordFrm.AN}' order by main_application desc";

                var pset = from a in Records.ApplicationQueList.GetApplicationQue(strWhereCond)
                           select a;
                RecordFrm.formBldgDate.LoadGrid();

                foreach (var item in pset)
                {
                    string sPermitNo = string.Empty;
                    string sPermitName = string.Empty;
                    string sStart = string.Empty;
                    string sComplete = string.Empty;
                    sPermitNo = item.PERMIT_CODE;
                    string sWhereClause = string.Empty;
                    DateTime dtTmp;

                    if (!string.IsNullOrEmpty(sPermitNo))
                    {
                        sWhereClause = " where permit_code = '";
                        sWhereClause += sPermitNo;
                        sWhereClause += "'";
                    }
                    PermitList permitlist = new PermitList(sWhereClause);
                    sPermitName = permitlist.PermitLst[0].PermitDesc;

                    DateTime.TryParse(item.PROP_START.ToString(), out dtTmp);
                    sStart = dtTmp.ToShortDateString();
                    DateTime.TryParse(item.PROP_COMPLETE.ToString(), out dtTmp);
                    sComplete = dtTmp.ToShortDateString();

                    RecordFrm.formBldgDate.dgvList.Rows.Add(sPermitNo, sPermitName, null, sStart, sComplete);
                }
            }
        }

        public override void DisplayEngrArch()
        {
            string strWhereCond = string.Empty;
            string sEngrNo = string.Empty;

            strWhereCond = $" where arn = '{RecordFrm.AN}' order by main_application desc";

            var pset = from a in Records.ApplicationQueList.GetApplicationQue(strWhereCond)
                       select a;
            RecordFrm.formEngr.LoadGrid();

            foreach (var item in pset)
            {
                sEngrNo = item.ENGR_CODE;
                if (string.IsNullOrEmpty(sEngrNo))
                    sEngrNo = item.ARCHITECT;

                Engineers account = new Engineers();
                account.GetOwner(sEngrNo);

                RecordFrm.formEngr.dgvList.Rows.Add(account.OwnerCode, account.LastName, account.FirstName, account.MiddleInitial,
                    account.EngrType, account.Address, account.HouseNo, account.LotNo, account.BlkNo,
                    account.Barangay, account.City, account.Province, account.ZIP, account.TIN,
                    account.PTR, account.PRC);

            }
        }

        public override void ButtonDeleteClick()
        {
            if (!string.IsNullOrEmpty(RecordFrm.AN))
            {
                if (MessageBox.Show("Are you sure you want to cancel this application?", RecordFrm.DialogText, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                CancelApplication();

            }
        }

        private void CancelApplication()
        {
            var db = new EPSConnection(dbConn);
            string strQuery = string.Empty;

            strQuery = $"insert into cancel_application select * from application_que where arn = '{RecordFrm.AN}'";
            db.Database.ExecuteSqlCommand(strQuery);

            strQuery = $"delete from application_que where arn = '{RecordFrm.AN}'";
            db.Database.ExecuteSqlCommand(strQuery);

            strQuery = $"insert into billing_hist select * from billing where arn = '{RecordFrm.AN}'";
            db.Database.ExecuteSqlCommand(strQuery);

            strQuery = $"delete from billing where arn = '{RecordFrm.AN}'";
            db.Database.ExecuteSqlCommand(strQuery);

            strQuery = $"insert into taxdues_hist select * from taxdues where arn = '{RecordFrm.AN}'";
            db.Database.ExecuteSqlCommand(strQuery);

            strQuery = $"delete from taxdues where arn= '{RecordFrm.AN}'";
            db.Database.ExecuteSqlCommand(strQuery);

            try
            {
                //var dbarcs = new ARCSConnection(dbConnArcs);
                OracleResultSet result = new OracleResultSet();
                result.CreateANGARCS(); //connect to arcs database

                result.Query = $"delete from eps_billing where arn = '{RecordFrm.AN}'";
                result.ExecuteNonQuery();
                //strQuery = $"delete from eps_billing where arn = '{RecordFrm.AN}'";
                //dbarcs.Database.ExecuteSqlCommand(strQuery);
                
            }
            catch { }

            MessageBox.Show("Application is cancelled.", RecordFrm.DialogText, MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (Utilities.AuditTrail.InsertTrail("TDA-CANCEL", "APPLICATION_QUE", "ARN: " + RecordFrm.AN) == 0)
            {
                MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            taskman.RemTask(RecordFrm.AN);
            RecordFrm.Close();
        }

        public override bool ValidateData()
        {
            if (RecordFrm.SourceClass != "NEW_ADD" && RecordFrm.SourceClass != "REN_ADD")
            {
                if (string.IsNullOrEmpty(RecordFrm.AN))
                {
                    MessageBox.Show("Incomplete ARN", RecordFrm.DialogText, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            return true;
        }

        public override void SetPermitAN(string sPermit) //AFM 20201027 REQUESTED(10/23/2020) NEW BIN ARN FORMAT
        {
            string sPermitAcro = string.Empty;
            sPermitAcro = RecordFrm.arn1.ANCodeGenerator(sPermit);
            RecordFrm.arn1.SetAn(sPermitAcro);
        }

    }
}