﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Modules.Tables;
using Common.StringUtilities;
using Common.DataConnector;
using Common.User;
using Modules.ARN;
using Modules.Utilities;
using Modules.Transactions;
using Common.AppSettings;
using Modules.Billing;
using Modules.Utilities.Forms;
using Modules.Reports;
using System.Reflection;

namespace EPS
{
    public partial class frmMenu : Form
    {
        public frmMenu()
        {
            InitializeComponent();
        }

        //AFM 20210112 merged from malolos
        //JARS 20181127 (S)
        //Version version = Assembly.GetExecutingAssembly().GetName().Version;
        //DateTime creationDate = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.MinorRevision * 2);
        internal static class AssemblyCreationDate
        {
            public static readonly DateTime Value;
            public static readonly string version;

            static AssemblyCreationDate()
            {

                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                Value = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.MinorRevision * 2);
            }
        }
        //JARS 20181127 (E)

        private void frmMenu_Load(object sender, EventArgs e)
        {
            panelLoc();

            this.Text = "Engineering Permit System - COLLECT | Version: " + AssemblyCreationDate.Value.ToString(); //AFM 20210112

            bool blnIsConnectionOpen = false;
            blnIsConnectionOpen = DataConnectorManager.Instance.OpenConnection();
            if (!blnIsConnectionOpen)
            {
                MessageBox.Show("error connection");
                Dispose();
                this.Close();
            }

            frmLogin login = new frmLogin();
            login.ShowDialog();
            LabelPanel();

            AppSettingsManager.GetSystemType = "E"; // for blob

            if (login.State == frmLogin.LogInState.CancelState)
            {
                //this.Dispose();
                this.Close(); //AFM 20200630
            }

        }

        private void LabelPanel() //AFM20200504
        {
            string sUser = string.Empty;
            string sUserCode = string.Empty;
            try
            {
                sUser = AppSettingsManager.g_objSystemUser.UserName.ToString();
                sUserCode = AppSettingsManager.g_objSystemUser.UserCode.ToString();
            }
            catch { }

            lblUser.Visible = true;
            lblDesc.Visible = true;
            lblCode.Visible = true;
            lblDate.Visible = true;

            lblUser.Text = "USER: " + sUser;
            lblCode.Text = "USERCODE: " + sUserCode;
            lblDesc.Text = "AMELLAR SOLUTIONS: INFORMATION IS THE BUSINESS. TECHNOLOGY IS THE TOOL. PEOPLE MAKE THE SYSTEM WORK.";
            lblDate.Text = "Today is " + string.Format("{0:MMMM dd, yyyy}", AppSettingsManager.GetSystemDate());
        }

        private void panelLoc()
        {
            if (WindowState == FormWindowState.Maximized)
                pnlButton.Location = new Point(0, 570);
            if (WindowState == FormWindowState.Normal)
                pnlButton.Location = new Point(0, 424);
        }

        private void frmMenu_Resize(object sender, EventArgs e)
        {
            panelLoc();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //Application.ExitThread();
                this.Close();
        }

        private void systemConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("SS"))
            {
                frmConfig frmconfig = new frmConfig();
                frmconfig.ShowDialog();
            }
        }

        private void userLevelTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("SC"))
            {
                frmUserlvltbl frmuserlvltbl = new frmUserlvltbl();
                frmuserlvltbl.ShowDialog();
            }
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("STB"))
            {
                frmBarangay frmbarangay = new frmBarangay();
                frmbarangay.ShowDialog();
            }
        }

        private void structureTpeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("SST"))
            {
                frmStructureType frmstructuretype = new frmStructureType();
                frmstructuretype.ShowDialog();
            }
        }

        private void userLevelAccessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("SSS"))
            {
                frmUserlvlAccess frmuserlvlaccess = new frmUserlvlAccess();
                frmuserlvlaccess.ShowDialog();
            }
        }

        private void engineeringRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskManager taskman = new TaskManager();
            if (!taskman.IsObjectLock("ENG_REC","",""))
            {
                frmRecords form = new frmRecords();
                form.SourceClass = "ENG_REC";
                form.ShowDialog();
            }
            taskman.IsObjectLock("ENG_REC", "DELETE", "");
        }

        private void btnNewApp_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.ShowDialog();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("TNA"))
            {
                frmRecords form = new frmRecords();
                form.SourceClass = "NEW_ADD";
                form.ShowDialog();
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("TEA"))
            {
                frmRecords form = new frmRecords();
                form.SourceClass = "NEW_EDIT";
                form.ShowDialog();
            }
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("TVA"))
            {
                frmRecords form = new frmRecords();
                form.SourceClass = "NEW_VIEW";
                form.ShowDialog();
            }
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("TDA"))
            {
                frmRecords form = new frmRecords();
                form.SourceClass = "NEW_CANCEL";
                form.ShowDialog();
            }
        }

        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("TRA"))
            {
                frmRecords form = new frmRecords();
                form.SourceClass = "REN_ADD";
                form.ShowDialog();
            }
        }

        private void buildingPermitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(AppSettingsManager.Granted("TB-B"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("BILLING", "", ""))
                {
                    frmBilling form = new frmBilling();
                    form.Source = "BUILDING PERMIT";
                    form.ModuleCode = "TB-UBB";
                    form.ShowDialog();

                    taskman.IsObjectLock("BILLING", "DELETE", "");
                }
            }
        }

        private void mechanicalPermitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("TB-M"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("BILLING", "", ""))
                {
                    frmBilling form = new frmBilling();
                    form.Source = "MECHANICAL PERMIT";
                    form.ModuleCode = "TB-UBM";
                    form.ShowDialog();

                    taskman.IsObjectLock("BILLING", "DELETE", "");
                }
            }
        }

        private void electricalPermitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("TB-E"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("BILLING", "", ""))
                {
                    frmBilling form = new frmBilling();
                    form.Source = "ELECTRICAL PERMIT";
                    form.ModuleCode = "TB-UBE";
                    form.ShowDialog();

                    taskman.IsObjectLock("BILLING", "DELETE", "");
                }
            }

        }

        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("TER"))
            {
                frmRecords form = new frmRecords();
                form.SourceClass = "REN_EDIT";
                form.ShowDialog();
            }
        }

        private void viewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("TVR"))
            {
                frmRecords form = new frmRecords();
                form.SourceClass = "REN_VIEW";
                form.ShowDialog();
            }
        }

        private void cancelToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("TDAR"))
            {
                frmRecords form = new frmRecords();
                form.SourceClass = "REN_CANCEL";
                form.ShowDialog();
            }
        }

        private void taskManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("SUM"))
            {
                frmTaskManager form = new frmTaskManager();
                form.ShowDialog();   
            }
        }

        private void postingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("P"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("POSTING", "", ""))
                {
                    frmPosting form = new frmPosting();
                    form.ShowDialog();

                    taskman.IsObjectLock("POSTING", "DELETE", "");
                }
                
            }
        }

        private void cancelPostedPaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(AppSettingsManager.Granted("PCP"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("CANCEL PAYMENT", "", ""))
                {
                    frmCancelPayment form = new frmCancelPayment();
                    form.ShowDialog();

                    taskman.IsObjectLock("CANCEL PAYMENT", "DELETE", "");
                }
            }   
        }

        private void occupancyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("TB-O"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("BILLING", "", ""))
                {
                    frmBilling form = new frmBilling();
                    form.Source = "CERTIFICATE OF OCCUPANCY";
                    form.ModuleCode = "TB-UBO";
                    form.ShowDialog();

                    taskman.IsObjectLock("BILLING", "DELETE", "");
                }
            }
        }

        private void zoningPermitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("TB-B"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("BILLING", "", ""))
                {
                    frmBilling form = new frmBilling();
                    form.Source = "ZONING";
                    form.ModuleCode = "TB-Z";
                    form.ShowDialog();

                    taskman.IsObjectLock("BILLING", "DELETE", "");
                }
            }
        }

        private void annualInspectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("TB-AI"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("BILLING", "", ""))
                {
                    frmBilling form = new frmBilling();
                    form.Source = "CERTIFICATE OF ANNUAL INSPECTION";
                    form.ModuleCode = "TB-UBAI";
                    form.ShowDialog();

                    taskman.IsObjectLock("BILLING", "DELETE", "");
                }
            }
        }

        private void permitNoadjustmentToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void auditTrailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAuditTrail frmaudittrail = new frmAuditTrail();
            frmaudittrail.ShowDialog();
        }

        private void connectivityToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("SC"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("CATEGORY", "", ""))
                {
                    frmCategory form = new frmCategory();
                    form.ShowDialog();
                    taskman.IsObjectLock("CATEGORY", "DELETE", "");
                }
            }
        }

        private void occupancyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(AppSettingsManager.Granted("SOC"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("OCCUPANCY", "", ""))
                {
                    frmOccupancy form = new frmOccupancy();
                    form.ShowDialog();
                    taskman.IsObjectLock("OCCUPANCY", "DELETE", "");
                }
            }
        }

        private void scheduleOfFeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("STS"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("FEES", "", ""))
                {
                    frmScheduleFees form = new frmScheduleFees();
                    form.ScheduleMode = "MAIN";
                    form.ShowDialog();
                    taskman.IsObjectLock("FEES", "DELETE", "");
                }
            }
        }

        private void scopeOfWorkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(AppSettingsManager.Granted("SSC"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("SCOPE", "", ""))
                {
                    frmScope form = new frmScope();
                    form.ShowDialog();
                    taskman.IsObjectLock("SCOPE", "DELETE", "");
                }
            }
        }

        private void typesOfMaterialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("SMT"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("MATERIAL", "", ""))
                {
                    frmMaterial form = new frmMaterial();
                    form.ShowDialog();
                    taskman.IsObjectLock("MATERIAL", "DELETE", "");
                }
            }
        }

        private void lotOwnerApplicantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("SLS"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("LOT OWNER", "", ""))
                {
                    frmOwner form = new frmOwner();
                    form.SourceClass = "OWNER";
                    form.ShowDialog();
                    taskman.IsObjectLock("LOT OWNER", "DELETE", "");
                }
            }
        }

        private void engineersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("SE"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("ENGINEER", "", ""))
                {
                    frmOwner form = new frmOwner();
                    form.SourceClass = "ENGR";
                    form.ShowDialog();
                    taskman.IsObjectLock("ENGINEER", "DELETE", "");
                }
            }
        }

        private void permitTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("SPT"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("PERMIT TYPE", "", ""))
                {
                    frmPermitType form = new frmPermitType();
                    form.ShowDialog();
                    taskman.IsObjectLock("PERMIT TYPE", "DELETE", "");
                }
            }
        }

        private void usersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("SUA"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("USERS", "", ""))
                {
                    frmUsers form = new frmUsers();
                    form.ShowDialog();
                    taskman.IsObjectLock("USERS", "DELETE", "");
                }
            }
        }
		private void btnLO_Click(object sender, EventArgs e)
        {

        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                frmLogin frmlogin = new frmLogin();
                frmlogin.ShowDialog();

                if (frmlogin.State.ToString() == "CancelState") //AFM 20200903
                    this.Close();
            }
        }

		private void permitBillingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //angono ver.
            //if (AppSettingsManager.Granted("TB-B"))
            //{
            //    TaskManager taskman = new TaskManager();
            //    if (!taskman.IsObjectLock("BILLING", "", ""))
            //    {
            //        frmBilling form = new frmBilling();
            //        form.Source = "BUILDING PERMIT";
            //        form.ModuleCode = "TB-UBB";
            //        form.ShowDialog();

            //        taskman.IsObjectLock("BILLING", "DELETE", "");
            //    }
            //}
        }
		
		private void assessmentOfFeesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void reassessmentOfPermitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void listOfSummaryPermitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSummaryOfReport frmsummaryofreport = new frmSummaryOfReport();
            frmsummaryofreport.ShowDialog();
        }

        private void paymentHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPaymentHist frmpaymenthist = new frmPaymentHist();
            frmpaymenthist.ShowDialog();
        }

        private void printCertificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("UNDER CONSTRUCTION");
            return;
            frmCertification frmcertification = new frmCertification();
            frmcertification.ShowDialog();
        }

        private void engineerTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEngineerTypes frmengineertypes = new frmEngineerTypes();
            frmengineertypes.ShowDialog();
        }

        private void lblUser_Click(object sender, EventArgs e)
        {

        }

        private void moduleSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmModuleSetup frmmodulesetup = new frmModuleSetup();
            frmmodulesetup.ShowDialog();
        }

        private void encodersLogToolStripMenuItem_Click(object sender, EventArgs e) //AFM 20201029
        {
            frmEncoderLog frmencoderslog = new frmEncoderLog();
            frmencoderslog.ShowDialog();
        }

        private void additionalFeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("STS"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("FEES", "", ""))
                {
                    frmScheduleFees form = new frmScheduleFees();
                    form.ScheduleMode = "ADDITIONAL";
                    form.ShowDialog();
                    taskman.IsObjectLock("FEES", "DELETE", "");
                }
            }
        }

        private void otherFeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("STS"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("FEES", "", ""))
                {
                    frmScheduleFees form = new frmScheduleFees();
                    form.ScheduleMode = "OTHERS";
                    form.ShowDialog();
                    taskman.IsObjectLock("FEES", "DELETE", "");
                }
            }
        }

        private void paymentsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tellersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("COL-S-TT"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("TELLERS", "", ""))
                {
                    frmTellers form = new frmTellers();               
                    form.ShowDialog();
                    taskman.IsObjectLock("TELLERS", "DELETE", "");
                }
            }
        }

        private void fornTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("COL-SFT"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("FORM TYPE", "", ""))
                {
                    frmFormType form = new frmFormType();
                    form.ShowDialog();
                    taskman.IsObjectLock("FORM TYPE", "DELETE", "");
                }
            }
        }

        private void oRInventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("COL-SOV"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("OR INV", "", ""))
                {
                    frmORInventory form = new frmORInventory();
                    form.ShowDialog();
                    taskman.IsObjectLock("OR INV", "DELETE", "");
                }
            }
        }

        private void paymentsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            

        }
        private bool CheckOR(string sTeller)
        {
            OracleResultSet res = new OracleResultSet();
            res.Query = $"select * from or_assigned where teller_code = '{sTeller}' and form_type = 'FORM 51'"; //fixed form type to 51
            if (res.Execute())
                if (res.Read())
                {
                    return true;
                }
                else
                    return false;
            else
                return false;
        }

        private void oRDeclarationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("COL-SOD"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("OR DEC", "", ""))
                {
                    frmORDeclaration form = new frmORDeclaration();
                    form.ShowDialog();
                    taskman.IsObjectLock("OR DEC", "DELETE", "");
                }
            }
        }

        private void cancelPaymentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("COL-P-C"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("PAYMENT-CANCEL", "", ""))
                {
                    frmCancelPayment form = new frmCancelPayment();
                    form.ShowDialog();
                    taskman.IsObjectLock("PAYMENT-CANCEL", "DELETE", "");
                }
            }
        }

        private void bankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("SBK"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("BANK-TBL", "", ""))
                {
                    frmBank form = new frmBank();
                    form.isEdit = true;
                    form.ShowDialog();
                    taskman.IsObjectLock("BANK-TBL", "DELETE", "");
                }
            }
        }

        private void reprintORToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (AppSettingsManager.Granted("COL-RO"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("REPRINT-OR", "", ""))
                {
                    frmReprintOR form = new frmReprintOR();
                    form.ShowDialog();
                    taskman.IsObjectLock("REPRINT-OR", "DELETE", "");
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("COL-RCD"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("RCD-NEW", "", ""))
                {
                    frmRCD form = new frmRCD();
                    form.IsReprint = false;
                    form.ShowDialog();
                    taskman.IsObjectLock("RCD-NEW", "DELETE", "");
                }
            }
        }

        private void reprintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("COL-RCD"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("RCD-REPRINT", "", ""))
                {
                    frmRCD form = new frmRCD();
                    form.IsReprint = true;
                    form.ShowDialog();
                    taskman.IsObjectLock("RCD-REPRINT", "DELETE", "");
                }
            }
        }

        private void electricalOccupancyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("COL-PO"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("ONL PAY", "", ""))
                {
                    frmLoginTeller login = new frmLoginTeller();
                    login.ShowDialog();

                    if (login.State == frmLoginTeller.LogInState.CancelState)
                    {
                        taskman.IsObjectLock("ONL PAY", "DELETE", "");
                        return;
                    }
                    frmPayments form = new frmPayments();
                    form.SearchCriteria = "ELEC-OCC";
                    form.m_sTeller = login.m_objSystemUser.UserCode.Trim();
                    if (!CheckOR(login.m_objSystemUser.UserCode.Trim()))
                    {
                        MessageBox.Show("No OR Declared for this Teller!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        taskman.IsObjectLock("ONL PAY", "DELETE", "");
                        return;
                    }
                    form.ShowDialog();
                    taskman.IsObjectLock("ONL PAY", "DELETE", "");
                }
            }
        }

        private void buildingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("COL-PO"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("ONL PAY", "", ""))
                {
                    frmLoginTeller login = new frmLoginTeller();
                    login.ShowDialog();

                    if (login.State == frmLoginTeller.LogInState.CancelState)
                    {
                        taskman.IsObjectLock("ONL PAY", "DELETE", "");
                        return;
                    }
                    frmPayments form = new frmPayments();
                    form.m_sTeller = login.m_objSystemUser.UserCode.Trim();
                    if (!CheckOR(login.m_objSystemUser.UserCode.Trim()))
                    {
                        MessageBox.Show("No OR Declared for this Teller!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        taskman.IsObjectLock("ONL PAY", "DELETE", "");
                        return;
                    }
                    form.ShowDialog();
                    taskman.IsObjectLock("ONL PAY", "DELETE", "");
                }
            }
        }

        private void mechanicalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("COL-PO"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("ONL PAY", "", ""))
                {
                    frmLoginTeller login = new frmLoginTeller();
                    login.ShowDialog();

                    if (login.State == frmLoginTeller.LogInState.CancelState)
                    {
                        taskman.IsObjectLock("ONL PAY", "DELETE", "");
                        return;
                    }
                    frmPayments form = new frmPayments();
                    form.SearchCriteria = "MECH";
                    form.m_sTeller = login.m_objSystemUser.UserCode.Trim();
                    if (!CheckOR(login.m_objSystemUser.UserCode.Trim()))
                    {
                        MessageBox.Show("No OR Declared for this Teller!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        taskman.IsObjectLock("ONL PAY", "DELETE", "");
                        return;
                    }
                    form.ShowDialog();
                    taskman.IsObjectLock("ONL PAY", "DELETE", "");
                }
            }
        }

        private void cFEIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("COL-PO"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("ONL PAY", "", ""))
                {
                    frmLoginTeller login = new frmLoginTeller();
                    login.ShowDialog();

                    if (login.State == frmLoginTeller.LogInState.CancelState)
                    {
                        taskman.IsObjectLock("ONL PAY", "DELETE", "");
                        return;
                    }
                    frmPayments form = new frmPayments();
                    form.SearchCriteria = "CFEI";
                    form.m_sTeller = login.m_objSystemUser.UserCode.Trim();
                    if (!CheckOR(login.m_objSystemUser.UserCode.Trim()))
                    {
                        MessageBox.Show("No OR Declared for this Teller!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        taskman.IsObjectLock("ONL PAY", "DELETE", "");
                        return;
                    }
                    form.ShowDialog();
                    taskman.IsObjectLock("ONL PAY", "DELETE", "");
                }
            }
        }

        private void abstractOfCollectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("COL-R"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("REPORTS-COLLECTIONS", "", ""))
                {
                    frmAbstractOfCollection form = new frmAbstractOfCollection();
                    form.ShowDialog();

                    taskman.IsObjectLock("REPORTS-COLLECTIONS", "DELETE", "");
                }
            }
        }

        private void summaryOfDailyCollectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("COL-R"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("REPORTS-COLLECTIONS", "", ""))
                {
                    frmSummaryOfCollection form = new frmSummaryOfCollection();
                    form.ShowDialog();

                    taskman.IsObjectLock("REPORTS-COLLECTIONS", "DELETE", "");
                }
            }
        }

        private void quarterlyToolStripMenuItem_Click(object sender, EventArgs e) //AFM 20211110 REQUESTED by RJ/MITCH
        {
            if (AppSettingsManager.Granted("COL-R"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("REPORTS-COLLECTIONS", "", ""))
                {
                    frmQuarterlyCollection form = new frmQuarterlyCollection();
                    form.ShowDialog();

                    taskman.IsObjectLock("REPORTS-COLLECTIONS", "DELETE", "");
                }
            }
        }

        private void dOLEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("COL-PO"))
            {
                TaskManager taskman = new TaskManager();
                if (!taskman.IsObjectLock("ONL PAY", "", ""))
                {
                    frmLoginTeller login = new frmLoginTeller();
                    login.ShowDialog();

                    if (login.State == frmLoginTeller.LogInState.CancelState)
                    {
                        taskman.IsObjectLock("ONL PAY", "DELETE", "");
                        return;
                    }
                    frmPayments form = new frmPayments();
                    form.m_sTeller = login.m_objSystemUser.UserCode.Trim();
                    form.isDOLE = true;
                    if (!CheckOR(login.m_objSystemUser.UserCode.Trim()))
                    {
                        MessageBox.Show("No OR Declared for this Teller!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        taskman.IsObjectLock("ONL PAY", "DELETE", "");
                        return;
                    }
                    form.ShowDialog();
                    taskman.IsObjectLock("ONL PAY", "DELETE", "");
                }
            }
        }
    }
}