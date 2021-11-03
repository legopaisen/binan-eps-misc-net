using System;
using System.Collections.Generic;
using System.Text;
using Common.DataConnector;
using Common.EncryptUtilities;

namespace Common.AppSettings
{
    public class Teller:User
    {
        private string m_strMemo;
        private string m_strORCode;
        private string m_strDistCode;

        private string m_strErrorCode;

        public Teller()
        {
            this.Clear();
        }

        public Teller(string strUserCode, string strLastName, string strFirstName,
            string strMI, string strMemo, string strORCode, string strDistCode)
        {
            this.Clear();
            m_strUserCode = strUserCode;
            m_strLastName = strLastName;
            m_strFirstName = strFirstName;
            m_strMI = strMI;
            m_strMemo = strMemo;
            m_strORCode = strORCode;
            m_strDistCode = strDistCode;
        }


        public bool Load(string strTellerCode)
        {
            this.Clear();
            string strConfigDistCode = AppSettingsManager.GetDistrictCode();

            OracleResultSet result = new OracleResultSet();
            result.Query = "SELECT tel_ln, tel_fn, tel_mi, tel_memo, or_code, CASE WHEN dist_code IS NULL THEN ' ' ELSE dist_code END dist_code FROM tellers a LEFT OUTER JOIN teller_assign b ON a.sys_teller = b.teller_code WHERE a.sys_teller = RPAD(:1, 20)";
            result.AddParameter(":1", strTellerCode);
            if (result.Execute())
            {
                if (result.Read())
                {
                    string strDistCode = result.GetString("dist_code").Trim();

                    if (strDistCode == string.Empty)
                    {
                        m_strErrorCode = "This teller is not assigned under any District.";
                        result.Close();
                        System.Windows.Forms.MessageBox.Show(m_strErrorCode);
                        return false;
                    }
                    //add district confirmation here
                    else if (strConfigDistCode != "00" && strDistCode  != strConfigDistCode)
                    {
                       m_strErrorCode = "This teller is not assigned under this District.";
                       result.Close();
                       System.Windows.Forms.MessageBox.Show(m_strErrorCode);
                       return false;
                    }

                    //else if (strDistCode != "00" && strDistCode != ?)
                    
                    m_strUserCode = strTellerCode;
                    m_strLastName = result.GetString("tel_ln").Trim();
                    m_strFirstName = result.GetString("tel_fn").Trim();
                    m_strMI = result.GetString("tel_mi").Trim();
                    m_strMemo = result.GetString("tel_memo").Trim();
                    m_strORCode = result.GetString("or_code").Trim();
                    m_strDistCode = strDistCode;
                    result.Close();
                    return true;
                }
            }
            result.Close();
            return false;
        }

        public static int TellerTransaction(OracleResultSet result, string strTellerCode, string strReceipt,
            //OfficialReceipt.OfficialReceipt receipt,
            string strPaymentMode, string strPin, double dblTotalAmount, double dblCreditMemo,
            double dblCheck, double dblCashTender, double dblChange, string strPaymentType)
        {
            result.Query = "SELECT COUNT(*) FROM teller_transaction WHERE or_no = RPAD(:1, 20)";
            result.AddParameter(":1", strReceipt); //receipt.ToString());
            int intCount = 0;
            int.TryParse(result.ExecuteScalar(), out intCount);
            if (intCount != 0)
            {
                result.QueryText = "DELETE FROM teller_transaction WHERE or_no = RPAD(:1, 20)";
                if (result.ExecuteNonQuery() == 0)
                {
                    return 0;
                }
            }

            //RDO 011108 (s) temporary fix for invalid entry on teller_transaction
            if (strPaymentType == "CS" && dblCreditMemo <= 0.0 && dblCheck <= 0.0 && dblCashTender <= 0.0)
                dblCashTender = dblTotalAmount;
            //RDO 011108 (e) temporary fix for invalid entry on teller_transaction

            result.Query = "INSERT INTO teller_transaction VALUES (:1, :2, :3, :4, :5, :6, :7, :8, :9, :10, :11)";
            result.AddParameter(":1", strTellerCode);
            result.AddParameter(":2", strPaymentMode);
            result.AddParameter(":3", strPin); //only applicable for single pin transaction
            result.AddParameter(":4", strReceipt); //receipt.ToString());
            result.AddParameter(":5", dblTotalAmount);
            result.AddParameter(":6", dblCreditMemo);
            result.AddParameter(":7", dblCheck);
            result.AddParameter(":8", dblCashTender);
            result.AddParameter(":9", dblChange);
            result.AddParameter(":10", strPaymentType);
            result.AddParameter(":11", AppSettingsManager.GetCurrentDate());

            return result.ExecuteNonQuery();
        }

        public bool Authenticate(string strPassword, string strTeller)
        {
            if (strPassword != string.Empty)
            {
                //Common.EncryptUtilities.Encrypt enc = new Common.EncryptUtilities.Encrypt();
                //string strEncrypted = enc.EncryptPassword(strPassword);

                Encryption enc = new Encryption();
                string strEncrypted = enc.EncryptString(strPassword);

                int intCount = 0;
                OracleResultSet result = new OracleResultSet();
                //use trim to capture � character //JVL mal
                result.Query = "SELECT COUNT(*) FROM tellers WHERE trim(teller_code) = :1 AND trim(teller_pwd) = :2";
                result.AddParameter(":1", strTeller);
                result.AddParameter(":2", strPassword);
                int.TryParse(result.ExecuteScalar().ToString(), out intCount);
                if (intCount != 0)
                {
                    return true;
                }

                result.Query = "SELECT COUNT(*) FROM tellers WHERE trim(teller_code) = :1 AND trim(teller_pwd) = :2";
                result.AddParameter(":1", strTeller);
                result.AddParameter(":2", strEncrypted);
                int.TryParse(result.ExecuteScalar().ToString(), out intCount);
                if (intCount != 0)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }


        public string District
        {
            get { return m_strDistCode; }
        }

        public string ErrorCode
        {
            get { return m_strErrorCode; }
        }

        //RDO 032508 (s)
        public string ORCode
        {
            get { return m_strORCode; }
        }
        //RDO 032508 (e)

        public override void Clear()
        {
            base.Clear();
            m_strMemo = string.Empty;
            m_strORCode = string.Empty;
            m_strErrorCode = string.Empty;
        }
    }
}
