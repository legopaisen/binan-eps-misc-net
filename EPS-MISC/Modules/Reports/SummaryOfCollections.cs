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
    public class SummaryOfCollections : FormReportClass
    {
        public SummaryOfCollections(frmReport Form) : base(Form)
        { }

        ObservableCollection<Reports.Model.SummaryOfCollectionsFees> FeesList = new ObservableCollection<Model.SummaryOfCollectionsFees>();

        public override void LoadForm()
        {
            CreateDataset();
            Microsoft.Reporting.WinForms.ReportParameter[] para = new Microsoft.Reporting.WinForms.ReportParameter[]
            {
                new Microsoft.Reporting.WinForms.ReportParameter("PrintedBy", AppSettingsManager.SystemUser.UserName),
                new Microsoft.Reporting.WinForms.ReportParameter("DatePrinted", AppSettingsManager.GetSystemDate().ToShortDateString()),
                new Microsoft.Reporting.WinForms.ReportParameter("dtCovered", ReportForm.dtFrom.ToShortDateString() + " - " + ReportForm.dtTo.ToShortDateString())
            };

            ReportForm.reportViewer1.LocalReport.SetParameters(para);

            ReportDataSource ds = new ReportDataSource("dtSetFees", FeesList);

            this.ReportForm.reportViewer1.LocalReport.DataSources.Clear();
            this.ReportForm.reportViewer1.LocalReport.DataSources.Add(ds);
        }

        private void CreateDataset()
        {
            Reports.Model.SummaryOfCollectionsFees FeesModel = new Model.SummaryOfCollectionsFees();
            FeesList = new ObservableCollection<Model.SummaryOfCollectionsFees>();
            OracleResultSet res = new OracleResultSet();

            string sPermitCode = string.Empty;
            double dTotalAmt = 0;
            string sPermitDesc = string.Empty;
            res.Query = "select permit_code, nvl(sum(fees_due + fees_int + fees_surch),0) as amount from payments_info where (";
            for(int icnt = 0; icnt < ReportForm.PermitList.Count; icnt++)
            {
                res.Query += $" permit_code = '{ReportForm.PermitList[icnt]}' ";
                if (ReportForm.PermitList.Count > 1 && icnt != ReportForm.PermitList.Count - 1)
                    res.Query += $"or ";
            }
            res.Query += $") and teller_code = '{ReportForm.Teller}' and or_no in (select or_no from rcd_remit where or_no = payments_info.or_no and rcd_series = '{ReportForm.RCDNo}')";
            res.Query += " group by permit_code order by permit_code";

            if(res.Execute())
                while(res.Read())
                {
                    FeesModel = new Model.SummaryOfCollectionsFees();
                    sPermitCode = res.GetString("permit_code");
                    sPermitDesc = AppSettingsManager.GetPermitDesc(sPermitCode);
                    dTotalAmt = res.GetDouble("amount");

                    FeesModel.Code = sPermitCode;
                    FeesModel.Desc = sPermitDesc;
                    FeesModel.TotalAmt = dTotalAmt;

                    FeesList.Add(FeesModel);
                }
            res.Close();

        }

    }
}
