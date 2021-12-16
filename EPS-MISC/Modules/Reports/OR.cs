using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPSEntities.Connection;
using System.Data;
using Common.AppSettings;
using Microsoft.Reporting.WinForms;
using Modules.Records;
using Modules.Utilities;
using EPSEntities.Entity;
using Common.DataConnector;
using Common.StringUtilities;

namespace Modules.Reports
{
    public class OR:FormReportClass
    {
        public OR(frmReport Form) : base(Form)
        { }

        private DataSet dtSet;
        public string sOR = string.Empty;
        private double dSurch = 0;
        private double dTotalAmt = 0;
        private string sAmtInWords = string.Empty;
        private string sChkInfo = string.Empty;

        public override void LoadForm()
        {
            var result = (dynamic)null;
            var db = new EPSConnection(dbConn);
            OracleResultSet res = new OracleResultSet();
            string strWhereCond = string.Empty;
            string sPayor = string.Empty;
            string sBankCode = string.Empty;
            string sBankName = string.Empty;
            string sChkNo = string.Empty;
            string sChkAmt = string.Empty;
            string sPaymentType = string.Empty;
            string sTellerName = string.Empty;
            string sTreas = string.Empty;
            sOR = ReportForm.OR;

            //owner
            if(isDOLE || !string.IsNullOrEmpty(Payor))
            {
                sPayor = Payor;
            }
            else
            {
                strWhereCond = $" where arn = '{ReportForm.An}'";
                result = from a in Records.ApplicationTblList.GetRecord(strWhereCond)
                         select a;
                foreach (var item in result)
                {
                    Accounts account = new Accounts();
                    account.GetOwner(item.PROJ_OWNER);
                    sPayor = account.OwnerName;
                }
            }

            //total amt, payment type
            res.Query = $"select fees_amt_due,payment_type from payments_info where or_no = '{sOR}'";
            if(res.Execute())
                if(res.Read())
                {
                    dTotalAmt = res.GetDouble("fees_amt_due");
                    sPaymentType = res.GetString("payment_type");
                    if (sPaymentType == "CS")
                        sPaymentType = "CASH";
                    else if (sPaymentType == "CQ")
                        sPaymentType = "CHECK";
                    else if (sPaymentType == "CC")
                        sPaymentType = "CASH/CHECK";
                }
            res.Close();
            sAmtInWords = NumberWording.AmountInWords(dTotalAmt) + " only";

            //chk info
            res.Query = $"select chk_no, chk_amt, bank_code from chk_tbl where or_no = '{sOR}'";
            if (res.Execute())
                if (res.Read())
                {
                    sBankCode = res.GetString("bank_code");
                    sBankName = GetBank(sBankCode);
                    sChkNo = res.GetString("chk_no");
                    sChkAmt = string.Format("{0:#,##0.00}", res.GetDouble("chk_amt"));
                }
            res.Close();
            sChkInfo = sBankName + " - " + sChkNo + " - P " + sChkAmt;

            //teller
            sTellerName = GetTeller(ReportForm.Teller);

            //treasurer
            sTreas = AppSettingsManager.GetConfigValue("05");

            Microsoft.Reporting.WinForms.ReportParameter[] para = new Microsoft.Reporting.WinForms.ReportParameter[]
     {
                new Microsoft.Reporting.WinForms.ReportParameter("Payor", sPayor),
                new Microsoft.Reporting.WinForms.ReportParameter("BillNo", BillNo),
                new Microsoft.Reporting.WinForms.ReportParameter("OR", sOR),
                new Microsoft.Reporting.WinForms.ReportParameter("TotalAmt", dTotalAmt.ToString()),
                new Microsoft.Reporting.WinForms.ReportParameter("AmtInWords", sAmtInWords),
                new Microsoft.Reporting.WinForms.ReportParameter("ChkInfo", sChkInfo),
                new Microsoft.Reporting.WinForms.ReportParameter("PaymentType", sPaymentType),
                new Microsoft.Reporting.WinForms.ReportParameter("TellerName", sTellerName),
                new Microsoft.Reporting.WinForms.ReportParameter("Treasurer", sTreas),
                new Microsoft.Reporting.WinForms.ReportParameter("ORDate", ReportForm.dtOR.ToString("MMMM-dd-yyyy")),
                new Microsoft.Reporting.WinForms.ReportParameter("ApprovedBy", AppSettingsManager.GetConfigValue("08")),
     };
            ReportForm.reportViewer1.LocalReport.SetParameters(para);

            dtSet = new DataSet();
            CreateDataSet();

            ReportDataSource ds = new ReportDataSource("DataSet1", dtSet.Tables[0]);
            this.ReportForm.reportViewer1.LocalReport.DataSources.Clear();
            this.ReportForm.reportViewer1.LocalReport.DataSources.Add(ds);
        }

        private void CreateDataSet()
        {
            var db = new EPSConnection(dbConn);

            string sQuery = string.Empty;

            DataTable dtTable = new DataTable("List");
            DataColumn dtColumn;
            DataRow myDataRow;
            OracleResultSet res = new OracleResultSet();
            OracleResultSet res2 = new OracleResultSet();
            string sPermitCode = string.Empty;
            string sPermitDesc = string.Empty;
            string sFees = string.Empty;
            string sFeesDesc = string.Empty;
            string sCat = string.Empty;
            double dFeesAmt = 0;
            double dFeesAmt2 = 0;
            bool blnDisplay = false;

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "FeesDesc";
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            dtTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(Double);
            dtColumn.ColumnName = "Amount";
            dtColumn.ReadOnly = false;
            dtTable.Columns.Add(dtColumn);

            dtSet = new DataSet();
            dtSet.Tables.Add(dtTable);

            int cnt = 0;
            //must be same with query below
            res.Query = $"select count(*) from (select distinct fees_category, permit_code, fees_surch, fees_amt_due, sum(fees_due) as fees_due from payments_info where or_no = '{sOR}' and fees_category = 'MAIN' group by fees_category, permit_code, fees_surch, fees_amt_due order by permit_code)";
            int.TryParse(res.ExecuteScalar(), out cnt);

            res.Query = $"select distinct fees_category, permit_code, fees_surch, fees_amt_due, sum(fees_due) as fees_due from payments_info where or_no = '{sOR}' and fees_category = 'MAIN' group by fees_category, permit_code, fees_surch, fees_amt_due order by permit_code";
            if (res.Execute())
            {
                if (cnt > 0)
                { 
                    while (res.Read())
                    {
                        sFees = string.Empty;
                        sFeesDesc = string.Empty;
                        sCat = string.Empty;
                        dFeesAmt = 0;
                        dFeesAmt2 = 0;
                        myDataRow = dtTable.NewRow();
                        sPermitCode = res.GetString("permit_code");
                        if(isDOLE)
                            sPermitDesc = "DOLE"; //for description in OR
                        else
                            sPermitDesc = AppSettingsManager.GetPermitDesc(sPermitCode);

                        sCat = res.GetString("fees_category");
                        dFeesAmt = res.GetDouble("fees_due");
                        dSurch = res.GetDouble("fees_surch");

                        myDataRow["FeesDesc"] = sPermitDesc;
                        //myDataRow["Amount"] = dFeesAmt;
                        dtTable.Rows.Add(myDataRow);

                        //other fees - not displayed
                        dFeesAmt2 = dFeesAmt;
                        res2.Query = $"select distinct fees_category, permit_code, fees_surch, fees_amt_due, fees_code, sum(fees_due) as fees_due from payments_info where or_no = '{sOR}' and fees_category <> 'MAIN' and permit_code = '{sPermitCode}' group by fees_category, permit_code, fees_surch, fees_amt_due, fees_code order by permit_code";
                        if (res2.Execute())
                            while (res2.Read())
                            {
                                sFees = res2.GetString("fees_code");
                                sCat = res2.GetString("fees_category");
                                sFeesDesc = AppSettingsManager.GetFeesDesc(sCat, sFees);
                                dFeesAmt = res2.GetDouble("fees_due");

                                blnDisplay = AppSettingsManager.FeesDisplayOnly(sCat, sFees);
                                if (blnDisplay == false) //skip display of fees if false
                                {
                                    dFeesAmt2 += dFeesAmt;
                                    myDataRow["Amount"] = dFeesAmt2;
                                }

                            }
                        res2.Close();
                        myDataRow["Amount"] = dFeesAmt2;

                        myDataRow = dtTable.NewRow();
                        myDataRow["FeesDesc"] = "SURCHARGE";
                        myDataRow["Amount"] = dSurch;
                        dtTable.Rows.Add(myDataRow);

                        //other fees - displayed
                        dFeesAmt2 = dFeesAmt;
                        res2.Query = $"select distinct fees_category, permit_code, fees_surch, fees_amt_due, fees_code, sum(fees_due) as fees_due from payments_info where or_no = '{sOR}' and fees_category <> 'MAIN' and permit_code = '{sPermitCode}' group by fees_category, permit_code, fees_surch, fees_amt_due, fees_code order by permit_code";
                        if (res2.Execute())
                            while (res2.Read())
                            {
                                sFees = res2.GetString("fees_code");
                                sCat = res2.GetString("fees_category");
                                sFeesDesc = AppSettingsManager.GetFeesDesc(sCat, sFees);
                                dFeesAmt = res2.GetDouble("fees_due");

                                blnDisplay = AppSettingsManager.FeesDisplayOnly(sCat, sFees);
                                if (blnDisplay == true) //skip display of fees if false
                                {
                                    myDataRow = dtTable.NewRow();
                                    myDataRow["FeesDesc"] = sFeesDesc;
                                    myDataRow["Amount"] = dFeesAmt;

                                    dtTable.Rows.Add(myDataRow);
                                }

                            }
                        res2.Close();
                    }
                }
                else //AFM 20211123 requested by binan as per rj - allow billing of additional fees only on any permit
                // proceeding this condition means additional fees are only billed on permit
                {
                    res.Query = $"select distinct fees_category, permit_code, fees_surch, fees_amt_due, sum(fees_due) as fees_due from payments_info where or_no = '{sOR}' and fees_category = 'ADDITIONAL' group by fees_category, permit_code, fees_surch, fees_amt_due order by permit_code";
                    if(res.Execute())
                        if(res.Read())
                        {
                            sFees = string.Empty;
                            sFeesDesc = string.Empty;
                            sCat = string.Empty;
                            dFeesAmt = 0;

                            myDataRow = dtTable.NewRow();
                            sPermitCode = res.GetString("permit_code");
                            sPermitDesc = AppSettingsManager.GetPermitDesc(sPermitCode); //requested by RJ to display by Permit name if additional fees are only billed

                            sCat = res.GetString("fees_category");
                            dFeesAmt = res.GetDouble("fees_due");
                            dSurch = res.GetDouble("fees_surch");

                            myDataRow["Amount"] = dFeesAmt;
                            myDataRow["FeesDesc"] = sPermitDesc;
                            dtTable.Rows.Add(myDataRow);


                            myDataRow = dtTable.NewRow();
                            myDataRow["FeesDesc"] = "SURCHARGE";
                            myDataRow["Amount"] = dSurch;
                            dtTable.Rows.Add(myDataRow);
                        }
                }

            }
               
            res.Close();

            //res.Query = $"select distinct fees_category, permit_code, fees_surch, fees_amt_due, fees_code, sum(fees_due) as fees_due from payments_info where or_no = '{sOR}' and fees_category = 'ADDITIONAL' group by fees_category, permit_code, fees_surch, fees_amt_due, fees_code order by permit_code";
            //if(res.Execute())
            //    while (res.Read())
            //    {
            //        sFees = res.GetString("fees_code");
            //        sCat = res.GetString("fees_category");
            //        sFeesDesc = AppSettingsManager.GetFeesDesc(sCat, sFees);
            //        dFeesAmt = res.GetDouble("fees_due");

            //        blnDisplay = AppSettingsManager.FeesDisplayOnly(sCat, sFees);
            //        if (blnDisplay == false) //skip display of fees if false
            //            continue;
            //        else
            //        {
            //            myDataRow = dtTable.NewRow();
            //            myDataRow["FeesDesc"] = sFeesDesc;
            //            myDataRow["Amount"] = dFeesAmt;

            //            dtTable.Rows.Add(myDataRow);
            //        }

            //    }
            //res.Close();
        }

        private string GetBank(string sCode)
        {
            string sBankName = string.Empty;
            OracleResultSet res = new OracleResultSet();
            res.Query = $"select bank_nm from bank_table where bank_id = '{sCode}'";
            if(res.Execute())
                if(res.Read())
                {
                    sBankName =  res.GetString("bank_nm");
                }
            return sBankName;
        }

        private string GetTeller(string sTellerCode)
        {
            string sTeller = string.Empty;
            OracleResultSet res = new OracleResultSet();
            res.Query = $"select teller_fn, teller_mi, teller_fn from tellers where teller_code = '{sTellerCode}'";
            if (res.Execute())
                if (res.Read())
                {
                    sTeller = res.GetString("teller_fn") + " " + res.GetString("teller_mi") + ". " + res.GetString("teller_fn");
                }
            return sTeller;
        }

    }
}
