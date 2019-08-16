﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EPSEntities.Entity;
using System.Data;
using System.Collections;
using Oracle.ManagedDataAccess.EntityFramework;
using Oracle.ManagedDataAccess.Client;
using Common.AppSettings;
using System.Windows.Forms;
using Common.StringUtilities;
using EPSEntities.Connection;

namespace Modules.Utilities
{
    public class TaskManager
    {
        public static ConnectionString dbConn = new ConnectionString();

        public bool IsObjectLock(string strModuleCode, string strMode, string strObject)
        {
            string strQuery = string.Empty;
            int intCnt = 0;
            string strUserCode = AppSettingsManager.SystemUser.UserCode;

            var db = new EPSConnection(dbConn);

            DateTime dtCurrent = AppSettingsManager.GetSystemDate();
            string strTime = string.Format("{0:MM/dd/yyyy hh:mm:ss}", dtCurrent);

            try {
                if (strMode == "DELETE")
                {
                    strQuery = $"delete from active_modules where module_code = '{strModuleCode}' and user_code = '{strUserCode}'";
                    db.Database.ExecuteSqlCommand(strQuery);

                    return true;
                }
                else
                {
                    strQuery = $"select count(*) from active_modules where module_code = '{strMode}' and user_code = '{strUserCode}'";
                    intCnt = db.Database.SqlQuery<Int32>(strQuery).SingleOrDefault();

                    if (intCnt > 0)
                    {
                        MessageBox.Show(strUserCode + " already logged in this module", "EPS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return true;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(strModuleCode))
                        {
                            strQuery = $"insert into active_modules values (:1,:2,:3,:4)";
                            db.Database.ExecuteSqlCommand(strQuery,
                                new OracleParameter(":1", strModuleCode),
                                new OracleParameter(":2", strUserCode),
                                new OracleParameter(":3", strTime),
                                new OracleParameter(":4", strObject));
                            return false;
                        }
                    }
                }
                        
            }
            catch (Exception ex)
            {
                ex.ToString(); 
                MessageBox.Show("Unable to save to the database", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        public bool AddTask(string strModuleCode, string strObject)
        {
            string strQuery = string.Empty;
            string strUserCode = string.Empty;
            
            var db = new EPSConnection(dbConn);

            DateTime dtCurrent = AppSettingsManager.GetSystemDate();
            string strTime = string.Format("{0:MM/dd/yyyy hh:mm:ss}", dtCurrent);

            strQuery = $"select * from active_modules where object = '{strObject}'";
            var epsrec = db.Database.SqlQuery<ACTIVE_MODULES>(strQuery);

            foreach (var items in epsrec)
            {
                strUserCode = items.USER_CODE;
            }

            if(!string.IsNullOrEmpty(strUserCode))
            {
                MessageBox.Show("ARN: " + strObject + " is being accessed by: "+strUserCode+".", "EPS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            else
            {
                strUserCode = AppSettingsManager.SystemUser.UserCode;

                try
                {
                    strQuery = $"insert into active_modules values (:1,:2,:3,:4)";
                    db.Database.ExecuteSqlCommand(strQuery,
                        new OracleParameter(":1", strModuleCode),
                        new OracleParameter(":2", strUserCode),
                        new OracleParameter(":3", strTime),
                        new OracleParameter(":4", strObject));
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    MessageBox.Show("Unable to save to the database", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

            }

            return true;
        }

        public void RemTask(string strObject)
        {
            string strQuery = string.Empty;
            string strUserCode = string.Empty;
            strUserCode = AppSettingsManager.SystemUser.UserCode;

            var db = new EPSConnection(dbConn);

            try
            {
                strQuery = $"delete from active_modules where object = '{strObject}' and user_code = '{strUserCode}'";
                db.Database.ExecuteSqlCommand(strQuery);
            }
            catch (Exception ex)
            {
                ex.ToString();
                MessageBox.Show("Unable to save to the database", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        public void RemTask(string strObject, string strModule, string strUserCode)
        {
            string strQuery = string.Empty;

            var db = new EPSConnection(dbConn);

            try
            {
                strQuery = $"delete from active_modules where user_code = '{strUserCode}' and module_code = '{strModule}'";
                if (!string.IsNullOrEmpty(strObject))
                    strQuery += $" and object = '{strObject}' ";
                db.Database.ExecuteSqlCommand(strQuery);
            }
            catch (Exception ex)
            {
                ex.ToString();
                MessageBox.Show("Unable to save to the database", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        public DataTable GetRecords()
        {
            var db = new EPSConnection(dbConn);
            DataTable dataTable = new DataTable();
            string sQuery = string.Empty;
                
            dataTable.Columns.Add("ModCode", typeof(String));
            dataTable.Columns.Add("Object", typeof(String));
            dataTable.Columns.Add("UserCode", typeof(String));
            dataTable.Columns.Add("TimeIn", typeof(String));
            //module_code, user_code, time_in

            sQuery = "select module_code, object, user_code, time_in from active_modules order by time_in";
            var epsrec = db.Database.SqlQuery<ACTIVE_MODULES>(sQuery);

            foreach (var items in epsrec)
            {
                dataTable.Rows.Add(items.MODULE_CODE,items.USER_CODE,items.TIME_IN);
            }
            return dataTable;
        }
    }
}