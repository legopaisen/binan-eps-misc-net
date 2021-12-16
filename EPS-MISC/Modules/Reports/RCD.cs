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

namespace Modules.Reports
{
    public class RCD:FormReportClass
    {
        private DataSet dtSet;
        private DataSet dtSetORBegBal;
        private DataSet dtSetORReceipts;
        private DataSet dtSetORIssued;
        private DataSet dtSetOREndBal;
        private DataSet dtSetListChecks;
        private DataSet dtSetTellerTrans;
        private DataSet dtSetCancelledOR;
        double dOrigAmtDue = 0;
        double dTotalAmtDue = 0;
        double dTaxCred = 0;
        double dCashAmt = 0;
        double dCheckAmt = 0;

        public RCD(frmReport Form) : base(Form)
        { }


        public override void LoadForm()
        {
            DateTime dtPrinted = AppSettingsManager.GetSystemDate();
            DateTime dtSignatory = AppSettingsManager.GetSystemDate();
            dtSet = new DataSet();
            dtSetORBegBal = new DataSet();
            dtSetORReceipts = new DataSet();
            dtSetORIssued = new DataSet();
            dtSetOREndBal = new DataSet();
            dtSetListChecks = new DataSet();
            dtSetTellerTrans = new DataSet();
            dtSetCancelledOR = new DataSet();

            if (ReportForm.isRCDReprint) //get save date
            {
                DateTime dtSave = AppSettingsManager.GetSystemDate();
                OracleResultSet res = new OracleResultSet();
                res.Query = $"select dt_save from partial_remit where rcd_series = '{ReportForm.RCDNo}' and teller_code = '{ReportForm.Teller}'";
                if(res.Execute())
                    if(res.Read())
                    {
                        dtSave = res.GetDateTime("dt_save");
                    }
                dtSignatory = dtSave;
                ReportForm.dtDate = dtSave;
            }

            CreateDataSet();
            CashAndCheckAmt();

            Microsoft.Reporting.WinForms.ReportParameter[] para = new Microsoft.Reporting.WinForms.ReportParameter[]
            {
                new Microsoft.Reporting.WinForms.ReportParameter("IsReprint", ReportForm.isRCDReprint.ToString()),
                new Microsoft.Reporting.WinForms.ReportParameter("Teller", AppSettingsManager.GetTellerName(ReportForm.Teller)),
                new Microsoft.Reporting.WinForms.ReportParameter("RCDNo", ReportForm.RCDNo),
                new Microsoft.Reporting.WinForms.ReportParameter("Total", dOrigAmtDue.ToString("#,##0.00")),
                new Microsoft.Reporting.WinForms.ReportParameter("LessTaxCred", dTaxCred.ToString("#,##0.00")),
                new Microsoft.Reporting.WinForms.ReportParameter("GrandTotal", dTotalAmtDue.ToString("#,##0.00")),
                new Microsoft.Reporting.WinForms.ReportParameter("DenomQty1000", ReportForm.qty1000.ToString()),
                new Microsoft.Reporting.WinForms.ReportParameter("DenomQty500", ReportForm.qty500.ToString()),
                new Microsoft.Reporting.WinForms.ReportParameter("DenomQty200", ReportForm.qty200.ToString()),
                new Microsoft.Reporting.WinForms.ReportParameter("DenomQty100", ReportForm.qty100.ToString()),
                new Microsoft.Reporting.WinForms.ReportParameter("DenomQty50", ReportForm.qty50.ToString()),
                new Microsoft.Reporting.WinForms.ReportParameter("DenomQty20", ReportForm.qty20.ToString()),
                new Microsoft.Reporting.WinForms.ReportParameter("DenomQty10", ReportForm.qty10.ToString()),
                new Microsoft.Reporting.WinForms.ReportParameter("DenomCoins", ReportForm.Coins.ToString()),
                new Microsoft.Reporting.WinForms.ReportParameter("PrintDate", dtPrinted.ToShortDateString()),
                new Microsoft.Reporting.WinForms.ReportParameter("SignatoryDate", dtSignatory.ToShortDateString()),
                new Microsoft.Reporting.WinForms.ReportParameter("CashAmt", dCashAmt.ToString()),
                new Microsoft.Reporting.WinForms.ReportParameter("CheckAmt", dCheckAmt.ToString()),
                new Microsoft.Reporting.WinForms.ReportParameter("Date", ReportForm.dtDate.ToString("MMMM dd, yyyy"))

            };
            ReportForm.reportViewer1.LocalReport.SetParameters(para);

            ReportDataSource ds = new ReportDataSource("dtSetORLIST", dtSet.Tables[0]);

            ReportDataSource dsORBeg = new ReportDataSource("dtSetORBegBal", dtSetORBegBal.Tables[0]);
            ReportDataSource dsORRec = new ReportDataSource("dtSetORReceipts", dtSetORReceipts.Tables[0]);
            ReportDataSource dsORIssued = new ReportDataSource("dtSetORIssued", dtSetORIssued.Tables[0]);
            ReportDataSource dsOREnd = new ReportDataSource("dtSetOREndBal", dtSetOREndBal.Tables[0]);
            ReportDataSource dsListChecks = new ReportDataSource("dtSetListChecks", dtSetListChecks.Tables[0]);
            ReportDataSource dsTellerTrans = new ReportDataSource("dtSetTellerTrans", dtSetTellerTrans.Tables[0]);
            ReportDataSource dsCancelledOR = new ReportDataSource("dtSetCancelledOR", dtSetCancelledOR.Tables[0]); //same model with teller trans

            this.ReportForm.reportViewer1.LocalReport.DataSources.Clear();
            this.ReportForm.reportViewer1.LocalReport.DataSources.Add(ds);
            this.ReportForm.reportViewer1.LocalReport.DataSources.Add(dsORBeg);
            this.ReportForm.reportViewer1.LocalReport.DataSources.Add(dsORRec);
            this.ReportForm.reportViewer1.LocalReport.DataSources.Add(dsORIssued);
            this.ReportForm.reportViewer1.LocalReport.DataSources.Add(dsOREnd);
            this.ReportForm.reportViewer1.LocalReport.DataSources.Add(dsListChecks);
            this.ReportForm.reportViewer1.LocalReport.DataSources.Add(dsTellerTrans);
            this.ReportForm.reportViewer1.LocalReport.DataSources.Add(dsCancelledOR);
        }

        private void CreateDataSet()
        {
            DataTable dtTable = new DataTable("List");
            DataColumn dtColumn;
            DataRow myDataRow;
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            OracleResultSet result3 = new OracleResultSet();

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "FormType";
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            dtTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(int);
            dtColumn.ColumnName = "NoIssued";
            dtColumn.ReadOnly = false;
            dtTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(string);
            dtColumn.ColumnName = "FromOR";
            dtColumn.ReadOnly = false;
            dtTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(string);
            dtColumn.ColumnName = "ToOR";
            dtColumn.ReadOnly = false;
            dtTable.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(Double);
            dtColumn.ColumnName = "Amount";
            dtColumn.ReadOnly = false;
            dtTable.Columns.Add(dtColumn);        

            dtSet = new DataSet();
            dtSet.Tables.Add(dtTable);

            string sFromOR = string.Empty;
            string sToOR = string.Empty;
            string sFormType = string.Empty;
            double dAmt = 0;
            long lNoIssued = 0;
            int iCnt = 0;
            dOrigAmtDue = 0;
            dTotalAmtDue = 0;
            dTaxCred = 0;
            if(ReportForm.isRCDReprint) //for reprint
                result.Query = $"select or_from as from_or_no, or_to as to_or_no, form_type from partial_remit where teller_code = '{ReportForm.Teller}' and rcd_series = '{ReportForm.RCDNo}' order by or_from";
            else
                result.Query = $"select * from rcd_remit_tmp where teller_code = '{ReportForm.Teller}' order by from_or_no";

            if (result.Execute())
                while(result.Read())
                {
                    sFromOR = result.GetString("from_or_no");
                    sToOR = result.GetString("to_or_no");
                    sFormType = result.GetString("form_type");

                    if (sFromOR == sToOR)
                        lNoIssued = 1;
                    else
                    {
                        lNoIssued = Convert.ToInt64(sToOR) - Convert.ToInt64(sFromOR);
                        if (lNoIssued == 1)
                            lNoIssued = 2;
                    }

                    result2.Query = $"select sum(total_amt_tendered) from payments_tendered where or_no between '{sFromOR}' and '{sToOR}'";
                    double.TryParse(result2.ExecuteScalar(), out dAmt);
                    result2.Close();

                    //get orig amt and less tax cred, total amt for totals
                    result2.Query = $"select nvl(sum(orig_amt_due),0) as orig_amt_due, nvl(sum(tax_credit_used),0) as tax_credit_used, nvl(sum(total_amt_tendered),0) as total_amt_tendered from payments_tendered where or_no between '{sFromOR}' and '{sToOR}'";
                    if(result2.Execute())
                        if(result2.Read())
                        {
                            dOrigAmtDue += result2.GetDouble("orig_amt_due");
                            dTotalAmtDue += result2.GetDouble("total_amt_tendered");
                            dTaxCred += result2.GetDouble("tax_credit_used");
                        }
                    result2.Close();

                    myDataRow = dtTable.NewRow();
                    myDataRow["FormType"] = sFormType;
                    myDataRow["NoIssued"] = lNoIssued;
                    myDataRow["FromOR"] = sFromOR;
                    myDataRow["ToOR"] = sToOR;
                    myDataRow["Amount"] = dAmt;
                    dtTable.Rows.Add(myDataRow);

                    iCnt++;
                }

            //for fixed rows - maximum of 10 rows
            for(int iRow = 10; iCnt < iRow; iCnt++)
            {
                myDataRow = dtTable.NewRow();
                myDataRow["FormType"] = "";
                myDataRow["NoIssued"] = DBNull.Value;
                myDataRow["FromOR"] = "";
                myDataRow["ToOR"] = "";
                myDataRow["Amount"] = DBNull.Value;
                dtTable.Rows.Add(myDataRow);
            }

            //////////////// accountable forms
            //beginning balance
            DataTable dtTableBegBalance = new DataTable("List");
            DataTable dtTableReceipts = new DataTable("List");
            DataTable dtTableIssued = new DataTable("List");
            DataTable dtTableEndBalance = new DataTable("List");

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "FormTypeBegBal";
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            dtTableBegBalance.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "FormTypeReceipts";
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            dtTableReceipts.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "FormTypeIssued";
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            dtTableIssued.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "FormTypeEndBal";
            dtColumn.ReadOnly = false;
            dtColumn.Unique = false;
            dtTableEndBalance.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(int);
            dtColumn.ColumnName = "QtyBegBal";
            dtColumn.ReadOnly = false;
            dtTableBegBalance.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(int);
            dtColumn.ColumnName = "QtyReceipts";
            dtColumn.ReadOnly = false;
            dtTableReceipts.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(int);
            dtColumn.ColumnName = "QtyIssued";
            dtColumn.ReadOnly = false;
            dtTableIssued.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(int);
            dtColumn.ColumnName = "QtyEndBal";
            dtColumn.ReadOnly = false;
            dtTableEndBalance.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "ORFromBegBal";
            dtColumn.ReadOnly = false;
            dtTableBegBalance.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "ORFromReceipts";
            dtColumn.ReadOnly = false;
            dtTableReceipts.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "ORFromIssued";
            dtColumn.ReadOnly = false;
            dtTableIssued.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "ORFromEndBal";
            dtColumn.ReadOnly = false;
            dtTableEndBalance.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "ORToBegBal";
            dtColumn.ReadOnly = false;
            dtTableBegBalance.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "ORToReceipts";
            dtColumn.ReadOnly = false;
            dtTableReceipts.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "ORToIssued";
            dtColumn.ReadOnly = false;
            dtTableIssued.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "ORToEndBal";
            dtColumn.ReadOnly = false;
            dtTableEndBalance.Columns.Add(dtColumn);

            dtSetORBegBal = new DataSet();
            dtSetORBegBal.Tables.Add(dtTableBegBalance);
            dtSetORReceipts = new DataSet();
            dtSetORReceipts.Tables.Add(dtTableReceipts);
            dtSetORIssued = new DataSet();
            dtSetORIssued.Tables.Add(dtTableIssued);
            dtSetOREndBal = new DataSet();
            dtSetOREndBal.Tables.Add(dtTableEndBalance);


            //////////ListofChecks
            DataTable dtTableListChecks = new DataTable("List");

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "CheckNo";
            dtColumn.ReadOnly = false;
            dtTableListChecks.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Bank";
            dtColumn.ReadOnly = false;
            dtTableListChecks.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(Double);
            dtColumn.ColumnName = "Amount";
            dtColumn.ReadOnly = false;
            dtTableListChecks.Columns.Add(dtColumn);

            dtSetListChecks = new DataSet();
            dtSetListChecks.Tables.Add(dtTableListChecks);

            //teller trans (rcd 2nd page)
            DataTable dtTableTellerTrans = new DataTable("List");
            DataTable dtTableCancelledOR = new DataTable("List");
            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "OR";
            dtColumn.ReadOnly = false;
            dtTableTellerTrans.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "OR";
            dtColumn.ReadOnly = false;
            dtTableCancelledOR.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Payor";
            dtColumn.ReadOnly = false;
            dtTableTellerTrans.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Payor";
            dtColumn.ReadOnly = false;
            dtTableCancelledOR.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Particulars";
            dtColumn.ReadOnly = false;
            dtTableTellerTrans.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(String);
            dtColumn.ColumnName = "Particulars";
            dtColumn.ReadOnly = false;
            dtTableCancelledOR.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(Double);
            dtColumn.ColumnName = "Amount";
            dtColumn.ReadOnly = false;
            dtTableTellerTrans.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = typeof(Double);
            dtColumn.ColumnName = "Amount";
            dtColumn.ReadOnly = false;
            dtTableCancelledOR.Columns.Add(dtColumn);

            dtSetTellerTrans = new DataSet();
            dtSetTellerTrans.Tables.Add(dtTableTellerTrans);

            dtSetCancelledOR = new DataSet();
            dtSetCancelledOR.Tables.Add(dtTableCancelledOR);

            long lQty = 0;
            int iRowCnt = 0;
            sFromOR = string.Empty;
            sToOR = string.Empty;
            sFormType = string.Empty;
            iCnt = 0;
            if (ReportForm.isRCDReprint) //for reprint
                result.Query = $"select or_from as from_or_no, or_to as to_or_no, form_type from partial_remit where teller_code = '{ReportForm.Teller}' and rcd_series = '{ReportForm.RCDNo}' order by or_from";
            else
                result.Query = $"select * from rcd_remit_tmp where teller_code = '{ReportForm.Teller}' order by from_or_no"; //mother query
            if(result.Execute())
                while(result.Read())
                {
                    sFromOR = result.GetString("from_or_no");
                    sToOR = result.GetString("to_or_no");
                    sFormType = result.GetString("form_type");

                    if (sFromOR == sToOR)
                        lQty = 1;
                    else
                    {
                        lQty = Convert.ToInt64(sToOR) - Convert.ToInt64(sFromOR);
                        if (lQty == 1)
                            lQty = 2;
                    }

                    result2.Query = $"select count(*) from or_used where or_no between '{sFromOR}' and '{sToOR}'";
                    int.TryParse(result2.ExecuteScalar(), out iCnt);
                    result2.Close();
                    if (iCnt == 0) // beginning balance
                    {
                        myDataRow = dtTableBegBalance.NewRow();

                        myDataRow["FormTypeBegBal"] = sFormType;
                        myDataRow["QtyBegBal"] = lQty;
                        myDataRow["ORFromBegBal"] = sFromOR;
                        myDataRow["ORToBegBal"] = sToOR;
                        dtTableBegBalance.Rows.Add(myDataRow);

                        //////
                        myDataRow = dtTableReceipts.NewRow();
                        myDataRow["FormTypeReceipts"] = "";
                        myDataRow["QtyReceipts"] = DBNull.Value;
                        myDataRow["ORFromReceipts"] = "";
                        myDataRow["ORToReceipts"] = "";
                        dtTableReceipts.Rows.Add(myDataRow);
                    }
                    else //Receipts
                    {
                        myDataRow = dtTableReceipts.NewRow();

                        myDataRow["FormTypeReceipts"] = "";
                        myDataRow["QtyReceipts"] = lQty;
                        myDataRow["ORFromReceipts"] = sFromOR;
                        myDataRow["ORToReceipts"] = sToOR;
                        dtTableReceipts.Rows.Add(myDataRow);

                        ////////
                        myDataRow = dtTableBegBalance.NewRow();
                        myDataRow["FormTypeBegBal"] = sFormType;
                        myDataRow["QtyBegBal"] = DBNull.Value;
                        myDataRow["ORFromBegBal"] = "";
                        myDataRow["ORToBegBal"] = "";
                        dtTableBegBalance.Rows.Add(myDataRow);
                    }

                    ////Issued
                    myDataRow = dtTableIssued.NewRow();
                    myDataRow["FormTypeIssued"] = "";
                    myDataRow["QtyIssued"] = lQty;
                    myDataRow["ORFromIssued"] = sFromOR;
                    myDataRow["ORToIssued"] = sToOR;
                    dtTableIssued.Rows.Add(myDataRow);

                    ////End Balance
                    string sLastOR = string.Empty;
                    string sBegFromOR = string.Empty;
                    string sBegToOR = string.Empty;
                    long lLastOr = 0;
                    result2.Query = $"select from_or_no, to_or_no, last_or_used from or_assigned_hist where from_or_no = '{sFromOR}' and last_or_used = '{sToOR}' AND teller_code = '{ReportForm.Teller}'  AND last_or_used is not null order by from_or_no";
                    if (result2.Execute())
                        if (result2.Read())
                        {
                            sBegFromOR = result2.GetString("from_or_no");
                            sBegToOR = result2.GetString("to_or_no");
                            sLastOR = result2.GetString("last_or_used");

                            if(sBegToOR == sLastOR)
                            {
                                myDataRow = dtTableEndBalance.NewRow();
                                myDataRow["FormTypeEndBal"] = "";
                                myDataRow["QtyEndBal"] = 0;
                                myDataRow["ORFromEndBal"] = "X";
                                myDataRow["ORToEndBal"] = "X";
                                dtTableEndBalance.Rows.Add(myDataRow);
                            }
                            else
                            {
                                lLastOr = Convert.ToInt64(sLastOR) + 1;

                                lQty = Convert.ToInt64(sBegToOR) - lLastOr;

                                myDataRow = dtTableEndBalance.NewRow();
                                myDataRow["FormTypeEndBal"] = "";
                                myDataRow["QtyEndBal"] = lQty;
                                myDataRow["ORFromEndBal"] = lLastOr.ToString();
                                myDataRow["ORToEndBal"] = sBegToOR;
                                dtTableEndBalance.Rows.Add(myDataRow);
                            }

                            
                        }
                    result2.Close();

                    iRowCnt++;
                    //check list
                    string sChkNo = string.Empty;
                    double dChkAmt = 0;
                    string sBank = string.Empty;
                    result2.Query = $"select distinct chk_no, bank_code, chk_amt from chk_tbl where or_no between '{sFromOR}' and '{sToOR}' and form_type = '{sFormType}' and teller = '{ReportForm.Teller}'";
                    if (result2.Execute())
                        while(result2.Read())
                        {
                            sChkNo = result2.GetString("chk_no");
                            sBank = AppSettingsManager.GetBankName(result2.GetString("bank_code"));
                            dChkAmt = result2.GetDouble("chk_amt");
                            myDataRow = dtTableListChecks.NewRow();
                            myDataRow["CheckNo"] = sChkNo;
                            myDataRow["Bank"] = sBank;
                            myDataRow["Amount"] = dChkAmt;
                            dtTableListChecks.Rows.Add(myDataRow);
                        }

                    result2.Close();


                    //2nd page- OR LIST
                    string sPermitDesc = string.Empty;
                    string sOR = string.Empty;
                    string sPayor = string.Empty;
                    double dAmount = 0;
                    double dTotalAmt = 0;
                    string sArn = string.Empty;
                    result2.Query = $"select distinct pa.*, pi.payer_code from payments pa, payments_info pi where pa.or_no = pi.or_no and pa.or_no between '{sFromOR}' and '{sToOR}' and pa.teller_code = '{ReportForm.Teller}' order by pa.or_no";
                    if(result2.Execute())
                        while(result2.Read())
                        {
                            sArn = result2.GetString("arn");
                            sOR = result2.GetString("or_no");
                            dAmount = result2.GetDouble("fees_due");
                            dTotalAmt += dAmount;
                            sPermitDesc = AppSettingsManager.GetPermitDesc(result2.GetString("permit_code"));

                            Accounts accounts = new Accounts();
                            accounts.GetOwner(result2.GetString("payer_code"));
                            if (sArn == "DOLE")
                            {
                                sPayor = "DOLE";
                                sPermitDesc = "DOLE";
                            }
                            else
                                sPayor = accounts.FirstName + " " + accounts.MiddleInitial + ". " + accounts.LastName;

                            myDataRow = dtTableTellerTrans.NewRow();
                            myDataRow["OR"] = "FORM 51-" + sOR;
                            myDataRow["Payor"] = sPayor;
                            myDataRow["Particulars"] = sPermitDesc;
                            myDataRow["Amount"] = dAmount;
                            dtTableTellerTrans.Rows.Add(myDataRow);

                        }
                    result2.Close();

                    result2.Query = $"select distinct ca.*, pi.payer_code from cancelled_payments ca, canc_payments_info pi where ca.or_no = pi.or_no and ca.or_no between '{sFromOR}' and '{sToOR}' order by ca.or_no";
                    if (result2.Execute())
                        while (result2.Read())
                        {
                            sOR = result2.GetString("or_no");
                            dAmount = result2.GetDouble("fees_due");
                            sPermitDesc = AppSettingsManager.GetPermitDesc(result2.GetString("permit_code"));

                            Accounts accounts = new Accounts();
                            accounts.GetOwner(result2.GetString("payer_code"));
                            if (sArn == "DOLE")
                                sPayor = "DOLE";
                            else
                                sPayor = accounts.FirstName + " " + accounts.MiddleInitial + ". " + accounts.LastName;

                            myDataRow = dtTableCancelledOR.NewRow();
                            myDataRow["OR"] = "FORM 51-" + sOR;
                            myDataRow["Payor"] = sPayor;
                            myDataRow["Particulars"] = sPermitDesc;
                            myDataRow["Amount"] = dAmount;
                            dtTableCancelledOR.Rows.Add(myDataRow);

                        }
                }

            for(int iRows = 8; iRowCnt < iRows; iRowCnt++)
            {
                myDataRow = dtTableBegBalance.NewRow();
                myDataRow["FormTypeBegBal"] = "";
                myDataRow["QtyBegBal"] = DBNull.Value;
                myDataRow["ORFromBegBal"] = "";
                myDataRow["ORToBegBal"] = "";
                dtTableBegBalance.Rows.Add(myDataRow);
              
                myDataRow = dtTableReceipts.NewRow();
                myDataRow["FormTypeReceipts"] = "";
                myDataRow["QtyReceipts"] = DBNull.Value;
                myDataRow["ORFromReceipts"] = "";
                myDataRow["ORToReceipts"] = "";
                dtTableReceipts.Rows.Add(myDataRow);

                myDataRow = dtTableIssued.NewRow();
                myDataRow["FormTypeIssued"] = "";
                myDataRow["QtyIssued"] = DBNull.Value;
                myDataRow["ORFromIssued"] = "";
                myDataRow["ORToIssued"] = "";
                dtTableIssued.Rows.Add(myDataRow);

                myDataRow = dtTableEndBalance.NewRow();
                myDataRow["FormTypeEndBal"] = "";
                myDataRow["QtyEndBal"] = DBNull.Value;
                myDataRow["ORFromEndBal"] = "";
                myDataRow["ORToEndBal"] = "";
                dtTableEndBalance.Rows.Add(myDataRow);

            }

        }

        private void CashAndCheckAmt()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            long lQty = 0;
            int iRowCnt = 0;
            string sFromOR = string.Empty;
            string sToOR = string.Empty;
            string sFormType = string.Empty;
            dCashAmt = 0;
            dCheckAmt = 0;
            if (ReportForm.isRCDReprint) //for reprint
                result.Query = $"select or_from as from_or_no, or_to as to_or_no, form_type from partial_remit where teller_code = '{ReportForm.Teller}' and rcd_series = '{ReportForm.RCDNo}' order by or_from";
            else
                result.Query = $"select * from rcd_remit_tmp where teller_code = '{ReportForm.Teller}'"; //mother query

            if (result.Execute())
                while (result.Read())
                {
                    sFromOR = result.GetString("from_or_no");
                    sToOR = result.GetString("to_or_no");
                    sFormType = result.GetString("form_type");

                    result2.Query = $"select nvl(sum(cash_amt),0) as cash_amt, nvl(sum(check_amt),0) as check_amt from payments_tendered where or_no between '{sFromOR}' and '{sToOR}'";
                    if(result2.Execute())
                        if(result2.Read())
                        {
                            dCashAmt += result2.GetDouble("cash_amt");
                            dCheckAmt += result2.GetDouble("check_amt");
                        }
                    result2.Close();
                }
            result.Close();
        }

    }
}
