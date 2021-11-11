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
    public partial class frmQuarterlyCollection : Form
    {
        public frmQuarterlyCollection()
        {
            InitializeComponent();
        }

        private void frmQuarterlyCollection_Load(object sender, EventArgs e)
        {
            rdoForm51.Checked = true;
            ClearControls();
            LoadTellers();
            LoadMonthsAndQuarters();
            LoadYears();
        }

        private void LoadYears()
        {
            cmbYear.Items.Clear();
            for(int i = 2020; i <= AppSettingsManager.GetSystemDate().Year; i++) {
                cmbYear.Items.Add(i);

            }
        }

        private void LoadMonthsAndQuarters()
        {
            cmbQuarters.Items.Clear();
            cmbQuarters.Items.Add("1");
            cmbQuarters.Items.Add("2");
            cmbQuarters.Items.Add("3");
            cmbQuarters.Items.Add("4");

            cmbMonths.Items.Clear();
            cmbMonths.Items.Add("JANUARY");
            cmbMonths.Items.Add("FEBRUARY");
            cmbMonths.Items.Add("MARCH");
            cmbMonths.Items.Add("APRIL");
            cmbMonths.Items.Add("MAY");
            cmbMonths.Items.Add("JUNE");
            cmbMonths.Items.Add("JULY");
            cmbMonths.Items.Add("AUGUST");
            cmbMonths.Items.Add("SEPTEMBER");
            cmbMonths.Items.Add("OCTOBER");
            cmbMonths.Items.Add("NOVEMBER");
            cmbMonths.Items.Add("DECEMBER");
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
            cmbTeller.SelectedIndex = -1;
            cmbMonths.SelectedIndex = -1;
            cmbQuarters.SelectedIndex = -1;
            cmbMonths.Enabled = false;
            cmbQuarters.Enabled = false;
            cmbYear.Enabled = false;
            rdoMonthly.Checked = false;
            rdoDateRange.Checked = false;
            rdoQuarterly.Checked = false;
            dtpFrom.Value = AppSettingsManager.GetSystemDate();
            dtpTo.Value = AppSettingsManager.GetSystemDate();

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            
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

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(cmbTeller.Text))
            {
                frmReport frmreport = new frmReport();
                string sMonth = string.Empty;

                if (rdoMonthly.Checked == true && !string.IsNullOrEmpty(cmbMonths.Text) && !string.IsNullOrEmpty(cmbYear.Text))
                {
                    int index = cmbMonths.SelectedIndex + 1;
                    sMonth = index.ToString();
                    frmreport.Month = sMonth; 
                    frmreport.Year = cmbYear.Text;
                }
                else if (rdoQuarterly.Checked == true && !string.IsNullOrEmpty(cmbQuarters.Text) && !string.IsNullOrEmpty(cmbYear.Text))
                {
                    frmreport.Quarter = cmbQuarters.Text;
                    frmreport.Year = cmbYear.Text;
                }
                else if (rdoDateRange.Checked == true)
                {
                    frmreport.dtFrom = dtpFrom.Value;
                    frmreport.dtTo = dtpTo.Value;
                }
                else
                {
                    MessageBox.Show("Missing Entries!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                frmreport.Teller = cmbTeller.Text;
                frmreport.ReportName = "Quarterly Collections";
                frmreport.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select Teller!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

        }

        private void rdoMonthly_CheckedChanged(object sender, EventArgs e)
        {
            if(rdoMonthly.Checked == true)
            {
                rdoQuarterly.Checked = false;
                rdoDateRange.Checked = false;

                cmbMonths.Enabled = true;
                cmbQuarters.Enabled = false;
                cmbYear.Enabled = true;
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;

                cmbQuarters.SelectedIndex = -1;
            }
        }

        private void rdoQuarterly_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoQuarterly.Checked == true)
            {
                rdoMonthly.Checked = false;
                rdoDateRange.Checked = false;

                cmbMonths.Enabled = false;
                cmbQuarters.Enabled = true;
                cmbYear.Enabled = true;
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;

                cmbMonths.SelectedIndex = -1;
            }
        }

        private void rdoDateRange_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoDateRange.Checked == true)
            {
                rdoMonthly.Checked = false;
                rdoQuarterly.Checked = false;

                cmbMonths.Enabled = false;
                cmbQuarters.Enabled = false;
                cmbYear.Enabled = false;
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
            }
        }
    }
}
