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
using System.Collections.ObjectModel;

namespace Modules.Reports
{
    public class AbstractOfCollections : FormReportClass
    {
        public AbstractOfCollections(frmReport Form) : base(Form)
        { }

        private DataSet dtSet;
        ObservableCollection<Reports.Model.AbstractOfCollectionsFees> FeesList = new ObservableCollection<Model.AbstractOfCollectionsFees>();

        DateTime dtDate1 = AppSettingsManager.GetSystemDate();
        DateTime dtDate2 = AppSettingsManager.GetSystemDate();
        string sDate3 = string.Empty;

        string sTeller = string.Empty;

        public override void LoadForm()
        {
            dtSet = new DataSet();

            sTeller = ReportForm.Teller;

            if (ReportForm.report_desc == "Abstract of Collections")
            {
                sDate3 = ReportForm.dtFrom.ToShortDateString() + " - " + ReportForm.dtTo.ToShortDateString();
            }
            else if (ReportForm.report_desc == "Quarterly Collections")
            {
                if (!string.IsNullOrEmpty(ReportForm.Month))
                {
                    dtDate1 = Convert.ToDateTime(ReportForm.Month + "/1/" + ReportForm.Year);
                    dtDate2 = Convert.ToDateTime(ReportForm.Month + "/" + DateTime.DaysInMonth(Convert.ToInt32(ReportForm.Year), Convert.ToInt32(ReportForm.Month)) + "/" + ReportForm.Year);
                    sDate3 = dtDate1.ToShortDateString() + " - " + dtDate2.ToShortDateString();
                }
                else if (!string.IsNullOrEmpty(ReportForm.Quarter))
                {
                    if (ReportForm.Quarter == "1")
                    {
                        dtDate1 = Convert.ToDateTime("1/1/" + ReportForm.Year);
                        dtDate2 = Convert.ToDateTime("3/31/" + ReportForm.Year);
                    }
                    else if (ReportForm.Quarter == "2")
                    {
                        dtDate1 = Convert.ToDateTime("4/1/" + ReportForm.Year);
                        dtDate2 = Convert.ToDateTime("6/30/" + ReportForm.Year);
                    }
                    else if (ReportForm.Quarter == "3")
                    {
                        dtDate1 = Convert.ToDateTime("7/1/" + ReportForm.Year);
                        dtDate2 = Convert.ToDateTime("9/30/" + ReportForm.Year);
                    }
                    else if (ReportForm.Quarter == "4")
                    {
                        dtDate1 = Convert.ToDateTime("10/1/" + ReportForm.Year);
                        dtDate2 = Convert.ToDateTime("12/31/" + ReportForm.Year);
                    }
                    sDate3 = dtDate1.ToShortDateString() + " - " + dtDate2.ToShortDateString();
                }
                else // date range filter
                {
                    sDate3 = ReportForm.dtFrom.ToShortDateString() + " - " + ReportForm.dtTo.ToShortDateString();
                }
            }

            CreateDataset();

            if (sTeller != "ALL")
                sTeller = AppSettingsManager.GetTellerName(ReportForm.Teller);
            Microsoft.Reporting.WinForms.ReportParameter[] para = new Microsoft.Reporting.WinForms.ReportParameter[]
            {
                new Microsoft.Reporting.WinForms.ReportParameter("Teller", sTeller),
                new Microsoft.Reporting.WinForms.ReportParameter("ReportName", "ABSTRACT OF COLLECTIONS"),
                new Microsoft.Reporting.WinForms.ReportParameter("DateCovered", sDate3)
            };

            ReportForm.reportViewer1.LocalReport.SetParameters(para);

            ReportDataSource ds = new ReportDataSource("dtSetFees", FeesList);

            this.ReportForm.reportViewer1.LocalReport.DataSources.Clear();
            this.ReportForm.reportViewer1.LocalReport.DataSources.Add(ds);
        }

        private void CreateDataset()
        {
            FeesList = new ObservableCollection<Model.AbstractOfCollectionsFees>();
            Reports.Model.AbstractOfCollectionsFees FeesModel = new Model.AbstractOfCollectionsFees();
            OracleResultSet res = new OracleResultSet();
            OracleResultSet res2 = new OracleResultSet();
            OracleResultSet res3 = new OracleResultSet();
            string sPermitCode = string.Empty;
            string sPermitDesc = string.Empty;
            string sORNo = string.Empty;
            string sFormType = string.Empty;
            string sPrevOr = string.Empty;
            string sPayor = string.Empty;
            string sQuery = string.Empty;
            double dFeesAmt = 0;
            string sTellerTmp = string.Empty;
            int iFeeCnt = 1;

            DateTime dtIssued = AppSettingsManager.GetSystemDate();

            res2.Query = "select * from permit_tbl order by permit_code";
            if (res2.Execute())
                while (res2.Read())
                {
                    if (ReportForm.Teller == "ALL") //requested by rj to include all teller and all rcd
                        sTellerTmp = "%";
                    else
                        sTellerTmp = sTeller;
                    sQuery = "select payer_code, or_no, or_date, form_type, permit_code, nvl(sum(fees_due + fees_int + fees_surch),0) as amount from payments_info ";
                    sQuery += $"where teller_code like '{sTellerTmp}'  and permit_code = '{res2.GetString("permit_code")}' ";

                    if (ReportForm.report_desc == "Abstract of Collections")
                    {
                        sQuery += $" and or_no in (select or_no from rcd_remit where or_no = payments_info.or_no and rcd_series = '{ReportForm.RCDNo}') ";
                    }
                    else if (ReportForm.report_desc == "Quarterly Collections") 
                    {
                        sQuery += $" and or_date between '{string.Format("{0:dd-MMM-yy}", dtDate1)}' and '{string.Format("{0:dd-MMM-yy}", dtDate2)}' ";

                    }
                    sQuery += " group by payer_code, or_no, or_date, form_type, permit_code order by permit_code, OR_NO, or_date";

                    res.Query = sQuery;
                    if (res.Execute())
                        if(res.Read())
                        {
                            res3.Query = sQuery;
                            if(res3.Execute())
                                while (res3.Read())
                                {
                                    FeesModel = new Model.AbstractOfCollectionsFees();
                                    sPermitCode = res3.GetString("permit_code");
                                    sPermitDesc = AppSettingsManager.GetPermitDesc(sPermitCode);
                                    dtIssued = res3.GetDateTime("or_date");
                                    sOR = res3.GetString("or_no");
                                    sFormType = res3.GetString("form_type");
                                    sPayor = AppSettingsManager.GetAcctName("AcctName", res3.GetString("payer_code").Trim());
                                    dFeesAmt = res3.GetDouble("amount");

                                    FeesModel.DateIssued = dtIssued;
                                    FeesModel.OR = sOR;
                                    FeesModel.Payor = sPayor;
                                    FeesModel.Fees = res2.GetString("permit_desc");
                                    if (res2.GetString("permit_code") == sPermitCode)
                                    {
                                        FeesModel.FeesAmt = dFeesAmt;

                                    }
                                    else
                                    {
                                        FeesModel.FeesAmt = 0;
                                    }

                                    FeesList.Add(FeesModel);
                                }
                            res3.Close();
                        }
                       else
                        {
                            FeesModel = new Model.AbstractOfCollectionsFees();
                            FeesModel.DateIssued = null;
                            FeesModel.OR = string.Empty;
                            FeesModel.Payor = string.Empty;
                            FeesModel.FeesAmt = null;
                            FeesModel.Fees = res2.GetString("permit_desc");

                            FeesList.Add(FeesModel);
                        }
                    res.Close();

                }

            
        }
    }
}
