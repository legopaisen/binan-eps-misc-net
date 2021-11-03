using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Common.DataConnector;
using Common.AppSettings;
using Modules.Utilities;

namespace Common.User
{
    public partial class frmLoginTeller : Form
    {
        private bool m_blnIsFormDragged;
        private Point m_pntClicked;

        public enum LogInState
        {
            CancelState,
            OKState
        }

        private LogInState m_objState;
        public Teller m_objSystemUser;
        private bool m_blnIsAdjusting;

        public frmLoginTeller()
        {
            InitializeComponent();

            m_objSystemUser = new Teller();
            m_objState = LogInState.CancelState;

            btnLogin.Enabled = false;
            m_blnIsAdjusting = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_objState = LogInState.CancelState;
            this.Dispose();
        }

        public LogInState State
        {
            get { return m_objState; }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (m_objSystemUser.Authenticate(txtPassword.Text.Trim(), txtUserCode.Text.Trim()))
            {              
                m_objState = LogInState.OKState;
                m_objSystemUser.UserCode = txtUserCode.Text.Trim();
                this.Dispose();

                if (Modules.Utilities.AuditTrail.InsertTrail(txtUserCode.Text, "COL-STL", "TELLERS", txtUserCode.Text) == 0)
                {
                    MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Invalid password.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                txtPassword.Focus();
            }
        }

        private void txtUserCode_TextChanged(object sender, EventArgs e)
        {
            if (m_blnIsAdjusting)
                return;
            m_blnIsAdjusting = true;
            int intSelect = txtUserCode.SelectionStart;
            txtUserCode.Text = txtUserCode.Text.Trim().ToUpper();
            if (intSelect > txtUserCode.Text.Length)
            {
                txtUserCode.SelectionStart = txtUserCode.Text.Length + 1;
            }
            else
            {
                txtUserCode.SelectionStart = intSelect;
            }
            m_blnIsAdjusting = false;

            if (txtUserCode.Text.Trim() == string.Empty)
            {
                m_objSystemUser.Clear();
                txtUserName.Text = string.Empty;
                txtDesignation.Text = string.Empty;
                btnLogin.Enabled = false;
            }
            else
            {
                if (!CheckTeller(txtUserCode.Text.Trim()))
                {

                    txtUserName.Text = "UNKNOWN";
                    txtDesignation.Text = "UNKNOWN";
                    btnLogin.Enabled = false;
                }
            }
        }

        private bool CheckTeller(string sTellerCode)
        {
            OracleResultSet res = new OracleResultSet();
            res.Query = "select * from tellers where teller_code = '" + sTellerCode + "'";
            if (res.Execute())
                if (res.Read())
                {
                    txtUserName.Text = res.GetString("teller_ln") + ", " + res.GetString("teller_fn") + " " + res.GetString("teller_mi");
                    txtDesignation.Text = res.GetString("teller_pos");
                    btnLogin.Enabled = true;
                    return true;
                }
                else
                    return false;
            else
                return false;
        }

        private void txtUserCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPassword.Focus();
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (btnLogin.Enabled)
                {
                    btnLogin_Click(null, null);
                }
            }
        }

        private void frmLogin_MouseUp(object sender, MouseEventArgs e)
        {
            m_blnIsFormDragged = false;
        }

        private void frmLogin_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_blnIsFormDragged)
            {
                Point pntMoveTo;

                pntMoveTo = this.PointToScreen(new Point(e.X, e.Y));
                pntMoveTo.Offset(-m_pntClicked.X, -m_pntClicked.Y);

                this.Location = pntMoveTo;
            }
        }

        private void frmLogin_MouseDown(object sender, MouseEventArgs e)
        {
            m_blnIsFormDragged = true;
            m_pntClicked = new Point(e.X, e.Y);
        }

        private void frmLoginTeller_Load(object sender, EventArgs e)
        {

        }
    }
}