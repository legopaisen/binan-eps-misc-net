using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EPSEntities.Connection;
using Common.DataConnector;
using Common.AppSettings;

namespace Modules.Reports
{
    public partial class frmReport : Form
    {
        public string ReportName { get; set; }
        public string An { get; set; }
        FormReportClass ReportClass = null;
        private static ConnectionString dbConn = new ConnectionString();

        public DateTime dtTo { get; set; }
        public DateTime dtFrom { get; set; }
        public DateTime dtOR { get; set; }
        public DateTime dtDate { get; set; }
        public bool isRCDReprint { get; set; }
        public string RCDNo { get; set; }
        public string User { get; set; }
        public string Module { get; set; }
        public string TrailMode { get; set; }
        public string Barangay { get; set; }
        public string OR { get; set; }
        public string BillNo { get; set; }
        public string Teller { get; set; }

        private string report_cd = string.Empty;
        public string report_desc = string.Empty;

        //RCD
        public long qty1000 { get; set; }
        public long qty500 { get; set; }
        public long qty200 { get; set; }
        public long qty100 { get; set; }
        public long qty50 { get; set; }
        public long qty20 { get; set; }
        public long qty10 { get; set; }
               
        public long qtyc10 { get; set; }
        public long qtyc5 { get; set; }
        public long qtyc1 { get; set; }
        public long qtyc005 { get; set; }
        public long qtyc0025 { get; set; }
        public long qtyc001 { get; set; }
        public long qtyc0005 { get; set; }
        public long qtyc0001 { get; set; }

        public double Coins { get; set; }
        public double CashAmt { get; set; }
        public double CheckAmt { get; set; }

        //Summary of collections
        public List<string> PermitList = new List<string>();

        //Quarterly Collection
        public string Month { get; set; }
        public string Quarter { get; set; }
        public string Year { get; set; }

        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            var db = new EPSConnection(dbConn);
            if (ReportName == "ORDER OF PAYMENT")
            {
                string sQuery = string.Empty;
                string sBillNo = string.Empty;

                try
                {
                    sQuery = $"select distinct bill_no from billing where arn = '{An}'";
                    sBillNo = db.Database.SqlQuery<string>(sQuery).SingleOrDefault();
                }
                catch (Exception ex)
                { }

                if(string.IsNullOrWhiteSpace(sBillNo))
                {
                    MessageBox.Show("Record has no existing billing","SOA",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    return;
                }

                this.reportViewer1.LocalReport.ReportEmbeddedResource = "Modules.Reports.Report.SOA.rdlc";
                this.Text = ReportName;

                ReportClass = new SOA(this);
                ReportClass.BillNo = sBillNo;
            }
            else if(ReportName == "Records" || ReportName == "Application")
            {
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "Modules.Reports.Report.Application.rdlc";
                this.Text = ReportName;

                ReportClass = new Application(this);
                ReportClass.AN = An;
            }
            else if(ReportName == "Building Information") //AFM 20190930
            {
                report_cd = string.Empty;
                report_desc = string.Empty;
                report_cd = "001";
                report_desc = "BUILDING INFORMATION";
                if (!GeneratedReport())
                    return;

                this.reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "Modules.Reports.Report.BuildingInformation.rdlc";
                this.Text = ReportName;

                ReportClass = new BuildingInformation(this);
                ReportClass.dtTo = dtTo;
                ReportClass.dtFrom = dtFrom;
            }

            else if (ReportName == "List of Approved Permit Application") //AFM 20190930
            {
                report_cd = string.Empty;
                report_desc = string.Empty;
                report_cd = "002";
                report_desc = "LIST OF APPROVED PERMIT APPLICATION";
                if (!GeneratedReport())
                    return;

                this.reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "Modules.Reports.Report.ApprovedPermitApplication.rdlc";
                this.Text = ReportName;

                ReportClass = new ApprovedPermitApplication(this);
                ReportClass.dtTo = dtTo;
                ReportClass.dtFrom = dtFrom;
            }

            else if (ReportName == "Issued Permits Summary") //AFM 20190930
            {
                report_desc = string.Empty;
                report_cd = string.Empty;
                report_cd = "003";
                report_desc = "SUMMARY OF PERMITS ISSUED";
                if (!GeneratedReport())
                    return;
                this.reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "Modules.Reports.Report.AuditTrail.rdlc";
                this.Text = ReportName;

                ReportClass = new PermitsSummary(this);
                ReportClass.dtTo = dtTo;
                ReportClass.dtFrom = dtFrom;
            }
            else if(ReportName == "AUDIT TRAIL") //AFM 20200812
            {
                report_desc = "AUDIT TRAIL";
                if (!GeneratedReport())
                    return;
                this.reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "Modules.Reports.Report.AuditTrail.rdlc";
                this.Text = ReportName;

                ReportClass = new AuditTrail(this);
                ReportClass.dtTo = dtTo;
                ReportClass.dtFrom = dtFrom;
            }
            else if (ReportName == "ENCODER LOG") //AFM 20201029
            {
                report_desc = "AUDIT TRAIL";
                this.reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "Modules.Reports.Report.EncoderLog.rdlc";
                this.Text = ReportName;

                ReportClass = new EncodersLog(this);
                ReportClass.dtTo = dtTo;
                ReportClass.dtFrom = dtFrom;
            }
            else if (ReportName == "PRINT OR")
            {
                report_desc = "PRINT OR";
                this.reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "Modules.Reports.Report.PrintOR.rdlc";
                this.Text = ReportName;

                ReportClass = new OR(this);
                ReportClass.sOR = OR;
                ReportClass.BillNo = BillNo;
                ReportClass.dt = dtOR;
                ReportClass.AN = An;
                ReportClass.sTeller = Teller;
            }
            else if (ReportName == "RCD")
            {
                report_desc = "RCD";
                this.reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "Modules.Reports.Report.RCD.rdlc";

                if(!isRCDReprint)
                {
                    btnRemit.Visible = true;
                    btnExit.Visible = true;
                }


                ReportClass = new RCD(this);
            }
            else if (ReportName == "Abstract of Collections")
            {
                report_desc = "Abstract of Collections";
                this.reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "Modules.Reports.Report.AbstractOfCollections.rdlc";

                ReportClass = new AbstractOfCollections(this);

            }
            else if (ReportName == "Summary of Collections")
            {
                report_desc = "Summary of Collections";
                this.reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "Modules.Reports.Report.SummaryOfCollections.rdlc";

                ReportClass = new SummaryOfCollections(this);

            }
            else if (ReportName == "Quarterly Collections") //AFM 20211110 REQUESTED by RJ/MITCH
            {
                report_desc = "Quarterly Collections";
                this.reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "Modules.Reports.Report.AbstractOfCollections.rdlc"; // same format with Abstract as per RJ/Mitch

                ReportClass = new AbstractOfCollections(this);
            }

            ReportClass.LoadForm();

            this.reportViewer1.RefreshReport();
        }

        private bool GeneratedReport()
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from rpt_info where report_cd = '"+ report_cd + "'";
            if (result.Execute())
                if (result.Read())
                {
                    string msg = string.Empty;
                    msg = "REPORT PREVIOUSLY GENERATED\n";
                    msg += "REPORT: '" + result.GetString("report_desc") + "'\n";
                    msg += "GENERATED BY: '" + result.GetString("user_name") + "'\n";
                    msg += "DATE: '" + result.GetString("gen_date") + "'\n";
                    msg += "TIME: '" + result.GetString("gen_time") + "'\n";
                    msg += "WOULD YOU LIKE TO GENERATE REPORT?";

                    DialogResult dg = MessageBox.Show(msg, "ReportClass GENERATED", MessageBoxButtons.YesNo);
                    if (dg == DialogResult.No)
                    {
                        return false;
                    }
                }
            result.Query = "delete from rpt_info where report_cd = '" + report_cd + "'";
            result.ExecuteNonQuery();
            result.Close();

            result.Query = "INSERT INTO RPT_INFO values ('" + report_cd + "', '"+ report_desc + "', '"+ AppSettingsManager.GetCurrentDate().ToShortDateString() +"', '"+ AppSettingsManager.GetCurrentDate().ToString("HH:mm:ss") +"', '"+ AppSettingsManager.SystemUser.UserCode +"', 'As of "+ AppSettingsManager.GetCurrentDate().ToShortDateString() +"')";
            result.ExecuteNonQuery();
            result.Close();

            return true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRemit_Click(object sender, EventArgs e)
        {
            SaveRemittance();
        }
        
        private void SaveRemittance()
        {
            string sRCDNo = GenerateRCDNo();
            OracleResultSet res = new OracleResultSet();
            OracleResultSet res2 = new OracleResultSet();
            OracleResultSet res3 = new OracleResultSet();
            
            //rcd_remit
            res.Query = $"select from_or_no, to_or_no from rcd_remit_tmp where teller_code = '{Teller}'";
            if(res.Execute())
                while(res.Read())
                {
                    string sFromOr = res.GetString("from_or_no");
                    string sToOr = res.GetString("to_or_no");
                    res2.Query = $"select or_no from or_used where or_no between '{sFromOr}' and '{sToOr}'";
                    if(res2.Execute())
                        while(res2.Read())
                        {
                            string sORNo = res2.GetString("or_no");
                            res3.Query = "INSERT INTO RCD_REMIT VALUES(";
                            res3.Query += $"'{sRCDNo}',";
                            res3.Query += $"'{Teller}',";
                            res3.Query += $"'{sORNo}',";
                            res3.Query += $"'FORM 51')";
                            if(res3.ExecuteNonQuery() == 0)
                            { }
                            res3.Close();
                        }
                    res2.Close();
                }
            res.Close();

            //partial_remit
            DateTime dtOR = AppSettingsManager.GetSystemDate();
            DateTime dtToday = AppSettingsManager.GetSystemDate();
            bool blnIsOK = false;
            res.Query = $"select * from rcd_remit_tmp where teller_code = '{Teller}'";
            if(res.Execute())
                while(res.Read())
                {
                    string sFromOr = res.GetString("from_or_no");
                    string sToOr = res.GetString("to_or_no");

                    if(!blnIsOK) //get latest transaction date of the series
                    {
                        res2.Query = $"select trn_date where or_no = '{sToOr}'";
                        if(res2.Execute())
                            if(res2.Read())
                                dtOR = res2.GetDateTime("trn_date");
                        res2.Close();
                        blnIsOK = true;
                    }


                    res3.Query = "INSERT INTO PARTIAL_REMIT VALUES(";
                    res3.Query += $"'{Teller}',";
                    res3.Query += $"'{sRCDNo}',";
                    res3.Query += $"'{qty1000}',";
                    res3.Query += $"'{qty500}',";
                    res3.Query += $"'{qty200}',";
                    res3.Query += $"'{qty100}',";
                    res3.Query += $"'{qty50}',";
                    res3.Query += $"'{qty20}',";
                    res3.Query += $"'',"; // php 10 cash is null
                    res3.Query += $"'{qtyc10}',"; 
                    res3.Query += $"'{qtyc5}',";
                    res3.Query += $"'{qtyc1}',";
                    res3.Query += $"'{qtyc005}',";
                    res3.Query += $"'{qtyc0025}',";
                    res3.Query += $"'{qtyc001}',";
                    res3.Query += $"'{qtyc0005}',";
                    res3.Query += $"'{qtyc0001}',";
                    res3.Query += $"'{CashAmt}',";
                    res3.Query += $"'{CheckAmt}',";
                    res3.Query += $"'{CashAmt + CheckAmt}',";
                    res3.Query += $"'{sFromOr}',";
                    res3.Query += $"'{sToOr}',";
                    res3.Query += $"'{string.Format("{0:dd-MMM-yy}", dtOR)}',";
                    res3.Query += $"'{string.Format("{0:dd-MMM-yy}", dtToday)}',";
                    res3.Query += $"'FORM 51')";
                    if(res3.ExecuteNonQuery() == 0)
                    { }
                    res3.Close();
                }
           
            RCDNo = sRCDNo;

            this.btnRemit.Visible = false;
            this.btnExit.Visible = false;

            ReportClass.LoadForm();
            this.reportViewer1.RefreshReport();

            res.Query = $"DELETE FROM RCD_REMIT_TMP WHERE TELLER_CODE = '{Teller}'";
            if (res.ExecuteNonQuery() == 0)
            { }
            res.Close();

        }

        private string GenerateRCDNo()
        {
            string sCurrYr = string.Empty;
            string sRCDSeries = string.Empty;
            string sLGUCode = string.Empty;
            string sYear = string.Empty;
            int cnt = 0;
            sCurrYr = AppSettingsManager.GetSystemDate().Year.ToString();
            OracleResultSet res = new OracleResultSet();
            res.Query = $"select count(*) from rcd_series where year = '{sCurrYr}'";
            int.TryParse(res.ExecuteScalar(), out cnt);
            res.Close();
            if(cnt == 0)
            {
                res.Query = $"INSERT INTO RCD_SERIES VALUES('{sCurrYr}', '1')";
                if(res.ExecuteNonQuery() == 0)
                { }
                res.Close();
            }

            string sSeries = string.Empty;
            res.Query = $"select * from rcd_series where year = '{sCurrYr}'";
            if(res.Execute())
                if(res.Read())
                {
                    sSeries = res.GetString("rcd_series");
                    sYear = res.GetString("year");
                }
            res.Close();

            int iCode = 0;
            int.TryParse(sSeries, out iCode);
            switch ((iCode).ToString().Length)
            {
                case 1:
                    {
                        sSeries = "00000" + (iCode).ToString();
                        break;
                    }
                case 2:
                    {
                        sSeries = "0000" + (iCode).ToString();
                        break;
                    }
                case 3:
                    {
                        sSeries = "000" + (iCode).ToString();
                        break;
                    }
                case 4:
                    {
                        sSeries = "00" + (iCode).ToString();
                        break;
                    }
                case 5:
                    {
                        sSeries = "0" + (iCode).ToString();
                        break;
                    }
                case 6:
                    {
                        sSeries = (iCode).ToString();
                        break;
                    }

            }

            sLGUCode = AppSettingsManager.GetConfigValue("04").ToString();
            sRCDSeries = sLGUCode + sYear.Substring(2, 2) + "-" + sSeries;

            int iSeries = 0;
            int.TryParse(sSeries, out iSeries);
            iSeries++;
            res.Query = $"UPDATE RCD_SERIES SET RCD_SERIES = '{iSeries}' WHERE YEAR = '{sCurrYr}'";
                if(res.ExecuteNonQuery() == 0)
                { }

            return sRCDSeries;
        }
    }
}
