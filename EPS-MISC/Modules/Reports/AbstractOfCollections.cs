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

        public override void LoadForm()
        {
            dtSet = new DataSet();

            CreateDataset();
            Microsoft.Reporting.WinForms.ReportParameter[] para = new Microsoft.Reporting.WinForms.ReportParameter[]
            {
                new Microsoft.Reporting.WinForms.ReportParameter("Teller", AppSettingsManager.GetTellerName(ReportForm.Teller)),
                new Microsoft.Reporting.WinForms.ReportParameter("DateCovered", ReportForm.dtFrom.ToShortDateString() + " - " + ReportForm.dtTo.ToShortDateString())
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
            int iFeeCnt = 1;
            DateTime dtIssued = AppSettingsManager.GetSystemDate();

            res2.Query = "select * from permit_tbl order by permit_code";
            if (res2.Execute())
                while (res2.Read())
                {
                    sQuery = "select payer_code, or_no, or_date, form_type, permit_code, nvl(sum(fees_due + fees_int + fees_surch),0) as amount from payments_info ";
                    sQuery += $"where teller_code = '{ReportForm.Teller}' and or_no in (select or_no from rcd_remit where or_no = payments_info.or_no and rcd_series = '{ReportForm.RCDNo}') and permit_code = '{res2.GetString("permit_code")}'";
                    sQuery += "group by payer_code, or_no, or_date, form_type, permit_code order by permit_code, OR_NO, or_date";
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
