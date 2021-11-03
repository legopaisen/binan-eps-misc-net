using Common.AppSettings;
using Common.DataConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modules.Reports
{
    public partial class frmAbstractOfCollection : Form
    {
        public frmAbstractOfCollection()
        {
            InitializeComponent();
        }

        private void frmAbstractOfCollection_Load(object sender, EventArgs e)
        {
            rdoForm51.Checked = true;
            ClearControls();
            PopulateFees();
            LoadTellers();
        }

        private void PopulateFees()
        {
            lstFees.Items.Clear();
            OracleResultSet res = new OracleResultSet();
            res.Query = "select permit_desc from permit_tbl order by permit_code";
            if(res.Execute())
                while(res.Read())
                {
                    lstFees.Items.Add(res.GetString("permit_desc"));
                }
            res.Close();
        }

        private void LoadTellers()
        {
            cmbTeller.Items.Clear();
            OracleResultSet res = new OracleResultSet();
            res.Query = "select teller_code from tellers order by teller_code";
            if (res.Execute())
                while (res.Read())
                {
                    cmbTeller.Items.Add(res.GetString("teller_code"));
                }
            res.Close();
        }

        private void ClearControls()
        {
            cmbRCDSeries.Items.Clear();
            cmbRCDSeries.Text = "";
            cmbTeller.SelectedIndex = -1;
            dtpFrom.Value = AppSettingsManager.GetSystemDate();
            dtpTo.Value = AppSettingsManager.GetSystemDate();

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LoadRCD();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadRCD();
        }

        private void LoadRCD()
        {
            cmbRCDSeries.Items.Clear();
            cmbRCDSeries.Text = "";
            OracleResultSet res = new OracleResultSet();
            res.Query = $"select distinct rcd_series from rcd_remit where teller_code = '{cmbTeller.Text.Trim()}' and rcd_series in(select rcd_series from partial_remit where rcd_remit.rcd_series = partial_remit.rcd_series and dt_save between '{string.Format("{0:dd-MMM-yy}", dtpFrom.Value)}' and '{string.Format("{0:dd-MMM-yy}", dtpTo.Value)}') order by rcd_series";
            if(res.Execute())
                while(res.Read())
                {
                    cmbRCDSeries.Items.Add(res.GetString("rcd_series"));
                }
            res.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbTeller_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbRCDSeries.Items.Clear();
            cmbRCDSeries.Text = "";
            dtpFrom.Value = AppSettingsManager.GetSystemDate();
            dtpTo.Value = AppSettingsManager.GetSystemDate();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(cmbTeller.Text) && !string.IsNullOrEmpty(cmbRCDSeries.Text))
            {
                frmReport frmreport = new frmReport();
                frmreport.dtFrom = dtpFrom.Value;
                frmreport.dtTo = dtpTo.Value;
                frmreport.RCDNo = cmbRCDSeries.Text;
                frmreport.Teller = cmbTeller.Text;
                frmreport.ReportName = "Abstract of Collections";
                frmreport.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select Teller and RCD No.!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }
    }
}
