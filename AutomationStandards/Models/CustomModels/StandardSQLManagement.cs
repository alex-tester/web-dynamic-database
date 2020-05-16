using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
//using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using AutomationStandards.Models.DatabaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutomationStandards.Models.CustomModels
{
    public class StandardSQLManagement
    {

        //public List<SelectListItem> GetStandardValuesDDL(int StandardID, string ValueFieldName, string TextFieldName)
        //{
        //    List<SelectListItem> Result = new List<SelectListItem>();
        //    StandardModel model = new StandardModel().SelectSingleStandard(StandardID);

        //    DataTable sValues = new DataTable(model.DBTableName);

        //    #region Execute SQL
        //    string SQL = "SELECT * FROM " + model.DBTableName + " WHERE enabled = 1 ORDER BY SortOrder";

        //    AutomationStandardsContext db = new AutomationStandardsContext();



        //    using (SqlConnection sqlConn = new SqlConnection(AutomationStandardsContext.ConnectionString))


        //    using (SqlCommand cmd = new SqlCommand(SQL, sqlConn))
        //    {
        //        sqlConn.Open();
        //        sValues.Load(cmd.ExecuteReader());
        //        sqlConn.Close();
        //    }
        //    #endregion

        //    foreach (DataRow dr in sValues.Rows)
        //    {
        //        Result.Add(new SelectListItem() { Text = dr[TextFieldName].ToString(), Value = dr[ValueFieldName].ToString() });
        //    }


        //    return Result;

        //}

        public List<SelectListItem> GetStandardValuesDDL(int StandardID, string ValueFieldName, string TextFieldName, bool filterResults, string filterColumn, string filterValue)
        {


            List<SelectListItem> Result = new List<SelectListItem>();
            StandardModel model = new StandardModel().SelectSingleStandard(StandardID);

            DataTable sValues = GetStandardValues(model.DBTableName);


            foreach (DataRow dr in sValues.Rows)
            {
                if (dr["Enabled"].ToString().ToLower() == "true")
                {
                    if (filterResults)
                    {
                        if (dr[filterColumn].ToString().ToLower().Contains(";"))
                        {
                            if (dr[filterColumn].ToString().ToLower().Contains(";" + filterValue.ToLower() + ";"))
                            {
                                Result.Add(new SelectListItem() { Text = dr[TextFieldName].ToString(), Value = dr[ValueFieldName].ToString() });
                            }
                        }
                        else
                        {
                            if (dr[filterColumn].ToString().ToLower().Contains(filterValue.ToLower()))
                            {
                                Result.Add(new SelectListItem() { Text = dr[TextFieldName].ToString(), Value = dr[ValueFieldName].ToString() });
                            }
                        }
                    }
                    else
                    {
                        Result.Add(new SelectListItem() { Text = dr[TextFieldName].ToString(), Value = dr[ValueFieldName].ToString() });
                    }
                }
            }


            return Result;

        }

        public List<SelectListItem> GetStandardValueDDLCustomFilter(int StandardID, string ValueFieldName, string TextFieldName, bool UseFilter, string SQLFilter)
        {

            List<SelectListItem> Result = new List<SelectListItem>();
            //StandardModel model = new StandardModel().SelectSingleStandard(StandardID);
            AutomationStandardsContext db = new AutomationStandardsContext();
            Standard model = db.Standard.Find(StandardID);

            DataTable sValues = GetStandardValuesCustomFilter(model.DBTableName, UseFilter, SQLFilter);


            foreach (DataRow dr in sValues.Rows)
            {
                if (dr["Enabled"].ToString().ToLower() == "true")
                {

                    Result.Add(new SelectListItem() { Text = dr[TextFieldName].ToString(), Value = dr[ValueFieldName].ToString() });

                }
            }

            return Result;
        }

        public List<SelectListItem> GetStandardValueDDLFiltered(int StandardID, string ValueFieldName, string TextFieldName, bool UseFilter, string SQLFilter, string FilterFieldName, string FilterValue)
        {

            List<SelectListItem> Result = new List<SelectListItem>();
            StandardModel model = new StandardModel().SelectSingleStandard(StandardID);

            DataTable sValues = GetStandardValuesCustomFilter(model.DBTableName, UseFilter, SQLFilter);


            foreach (DataRow dr in sValues.Rows)
            {
                if (dr["Enabled"].ToString().ToLower() == "true")
                {
                    if (dr[FilterFieldName].ToString().ToLower().Contains(";"))
                    {
                        if (dr[FilterFieldName].ToString().ToLower().Contains(";" + FilterValue.ToLower() + ";"))
                        {
                            Result.Add(new SelectListItem() { Text = dr[TextFieldName].ToString(), Value = dr[ValueFieldName].ToString() });
                        }
                    }
                    else
                    {
                        if (dr[FilterFieldName].ToString().ToLower().Contains(FilterValue.ToLower()))
                        {
                            Result.Add(new SelectListItem() { Text = dr[TextFieldName].ToString(), Value = dr[ValueFieldName].ToString() });
                        }
                    }
                }
            }

            return Result;
        }


        public List<SelectListItem> GetStandardValueDDLCustomFilter(string StandardName, string ValueFieldName, string TextFieldName, bool UseFilter, string SQLFilter)
        {

            StandardModel model = new StandardModel().SelectSingleStandardByName(StandardName);
            List<SelectListItem> Result = GetStandardValueDDLCustomFilter(model.StandardID, ValueFieldName, TextFieldName, UseFilter, SQLFilter);

            return Result;
        }

        public DataTable GetStandardValues(string TableName)
        {
            DataTable Result = new DataTable(TableName);

            string SQL = "SELECT * FROM " + TableName + " ORDER BY SortOrder";

            AutomationStandardsContext db = new AutomationStandardsContext();

            #region Execute SQL

            using (SqlConnection sqlConn = new SqlConnection(AutomationStandardsContext.ConnectionString))


            using (SqlCommand cmd = new SqlCommand(SQL, sqlConn))
            {
                sqlConn.Open();
                Result.Load(cmd.ExecuteReader());
                sqlConn.Close();
            }
            #endregion


            return Result;
        }

        public DataTable GetStandardValuesCustomFilter(string TableName, bool UseFilter, string FilterSQL)
        {
            DataTable Result = new DataTable(TableName);

            string SQL = "SELECT * FROM " + TableName;

            if (UseFilter)
            {
                SQL += " WHERE Enabled = 1 AND " + FilterSQL;
            }

            SQL += " ORDER BY SortOrder";

            AutomationStandardsContext db = new AutomationStandardsContext();

            #region Execute SQL

            using (SqlConnection sqlConn = new SqlConnection(AutomationStandardsContext.ConnectionString))


            using (SqlCommand cmd = new SqlCommand(SQL.Replace("&#39;", "'"), sqlConn))
            {
                sqlConn.Open();
                Result.Load(cmd.ExecuteReader());
                sqlConn.Close();
            }
            #endregion


            return Result;
        }

        public int GetStandardRowsCount(string TableName)
        {


            string SQL = "SELECT COUNT(ID) FROM " + TableName;

            AutomationStandardsContext db = new AutomationStandardsContext();

            #region Execute SQL

            int count = 0;

            using (SqlConnection thisConnection = new SqlConnection(AutomationStandardsContext.ConnectionString))
            {
                using (SqlCommand cmdCount = new SqlCommand(SQL, thisConnection))
                {
                    thisConnection.Open();
                    count = (int)cmdCount.ExecuteScalar();
                }
            }


            #endregion


            return count;
        }
        public DataRow GetStandardRow(string TableName, string RecordID)
        {
            DataTable dt = GetStandardValues(TableName);
            int newRecordSortOrder = dt.Rows.Count + 1;
            DataRow Result;

            if (RecordID == "0")
            {
                Result = dt.NewRow();
                Result["ID"] = "0";
                Result["Enabled"] = 1;
                Result["SortOrder"] = newRecordSortOrder;
            }
            else
            {
                string filter = "ID = " + RecordID;
                List<DataRow> CurrentRows = dt.Select(filter).ToList();

                if (CurrentRows.Count == 0)
                {
                    Result = dt.NewRow();
                }
                else
                {
                    Result = CurrentRows.First();
                }
            }

            return Result;
        }

        public bool InsertStandardValue(FormCollection fc, string UserID)
        {
            int StandardID = Convert.ToInt32(fc["StandardID"]);
            string RecordID = fc["ID"];
            List<StandardConfigModel> Configs = new StandardConfigModel().SelectStandardConfigs(StandardID);
            StandardModel model = new StandardModel().SelectSingleStandard(StandardID);

            //if (model.NotifiyOwner)
            //{
            //    NotifyStandardOwner("INSERT", fc, Configs, model, UserID);
            //}

            SetStandardVersionValue(model.StandardID);

            string InsertFields = "INSERT INTO " + model.DBTableName + " ( ";
            string ValuesFields = " VALUES (";

            InsertFields += "CreatedOn ,";
            ValuesFields += "'" + DateTime.Now + "' , ";

            InsertFields += "CreatedBy ,";
            ValuesFields += "'" + UserID + "' , ";

            foreach (var formKey in fc.Keys) // fc.AllKeys)
            {
                List<StandardConfigModel> foundConfig = Configs.Where(c => c.FieldName == formKey).ToList();
                if (foundConfig.Count > 0)
                {
                    StandardConfigModel config = foundConfig.First();
                    string value;
                    if (config.AllowMultiSelect)
                    {
                        value = ";" + fc[config.FieldName]; //.Replace(",", ";") + ";";
                        value = value.Replace(",", ";") + ";";
                    }
                    else
                    {
                        value = fc[config.FieldName];
                    }
                    InsertFields += " " + config.FieldName + " ,";
                    ValuesFields += "CAST('" + value + "' AS " + config.SQLDataType + " ) , ";

                }
            }

            InsertFields += "Enabled ,";
            ValuesFields += "CAST('" + fc["Enabled"] + "' AS BIT) , ";

            InsertFields += "SortOrder )";
            ValuesFields += " " + fc["SortOrder"] + " )";

            string SQL = InsertFields + " " + ValuesFields;

            bool Result = ExecuteSql(SQL);
            return Result;


        }

        public bool InsertStandardValueAsIFormCollecction(IFormCollection fc, string UserID)
        {
            int StandardID = Convert.ToInt32(fc["StandardID"]);
            string RecordID = fc["ID"];
            List<StandardConfigModel> Configs = new StandardConfigModel().SelectStandardConfigs(StandardID);
            //StandardModel model = new StandardModel().SelectSingleStandard(StandardID);
            AutomationStandardsContext db = new AutomationStandardsContext();
            Standard model = db.Standard.Where(x => x.StandardID == StandardID).FirstOrDefault();

            //if (model.NotifiyOwner)
            //{
            //    NotifyStandardOwner("INSERT", fc, Configs, model, UserID);
            //}

            SetStandardVersionValue(model.StandardID);

            string InsertFields = "INSERT INTO " + model.DBTableName + " ( ";
            string ValuesFields = " VALUES (";

            InsertFields += "CreatedOn ,";
            ValuesFields += "'" + DateTime.Now + "' , ";

            InsertFields += "CreatedBy ,";
            ValuesFields += "'" + UserID + "' , ";

            foreach (var formKey in fc.Keys) // fc.AllKeys)
            {
                List<StandardConfigModel> foundConfig = Configs.Where(c => c.FieldName == formKey).ToList();
                if (foundConfig.Count > 0)
                {
                    StandardConfigModel config = foundConfig.First();
                    string value;
                    if (config.AllowMultiSelect)
                    {
                        value = ";" + fc[config.FieldName]; //.Replace(",", ";") + ";";
                        value = value.Replace(",", ";") + ";";
                    }
                    else
                    {
                        value = fc[config.FieldName];
                    }
                    InsertFields += " " + config.FieldName + " ,";
                    ValuesFields += "CAST('" + value + "' AS " + config.SQLDataType + " ) , ";

                }
            }

            InsertFields += "Enabled ,";
            ValuesFields += "CAST('" + fc["Enabled"] + "' AS BIT) , ";

            InsertFields += "SortOrder )";
            ValuesFields += " " + fc["SortOrder"] + " )";

            string SQL = InsertFields + " " + ValuesFields;

            bool Result = ExecuteSql(SQL);
            return Result;


        }


        public bool UpdateStandardValue(FormCollection fc, string UserID)
        {
            int StandardID = Convert.ToInt32(fc["StandardID"]);
            string RecordID = fc["ID"];
            List<StandardConfigModel> Configs = new StandardConfigModel().SelectStandardConfigs(StandardID);
            StandardModel model = new StandardModel().SelectSingleStandard(StandardID);

            //if (model.NotifiyOwner)
            //{
            //    NotifyStandardOwner("UPDATE", fc, Configs, model, UserID);
            //}
            SetStandardVersionValue(model.StandardID);

            string SQL = "UPDATE " + model.DBTableName + " SET ";
            SQL += " ModifiedOn =  '" + DateTime.Now + "', ";
            SQL += " ModifiedBy =  '" + UserID + "', ";




            foreach (var formKey in fc.Keys) //AllKeys)
            {
                List<StandardConfigModel> foundConfig = Configs.Where(c => c.FieldName == formKey).ToList();
                if (foundConfig.Count > 0)
                {
                    StandardConfigModel config = foundConfig.First();
                    string value;

                    if (string.IsNullOrEmpty(fc[config.FieldName]))
                    {
                        value = "";
                    }
                    else
                    {


                        if (config.AllowMultiSelect)
                        {
                            value = ";" + fc[config.FieldName]; //.Replace(",", ";") + ";";
                            value = value.Replace(",", ";") + ";";
                        }
                        else
                        {
                            value = fc[config.FieldName];
                        }
                    }
                    SQL += " " + config.FieldName + " = CAST('" + value + "' AS " + config.SQLDataType + ") , ";

                }
            }

            SQL += " Enabled =  CAST('" + fc["Enabled"] + "' AS BIT) , ";
            SQL += " SortOrder =  " + fc["SortOrder"] + " ";


            SQL += " WHERE ID = " + RecordID;

            bool Result = ExecuteSql(SQL);
            return Result;
        }

        public bool UpdateStandardValueAsIFormCollection(IFormCollection fc, string UserID)
        {
            int StandardID = Convert.ToInt32(fc["StandardID"]);
            string RecordID = fc["ID"];
            List<StandardConfigModel> Configs = new StandardConfigModel().SelectStandardConfigs(StandardID);
            //StandardModel model = new StandardModel().SelectSingleStandard(StandardID);
            AutomationStandardsContext db = new AutomationStandardsContext();
            //List<StandardConfigModel> Configs = new List<StandardConfigModel>();
            List<StandardConfig> dbConfigs = db.StandardConfig.Where(x => x.StandardID == StandardID).ToList();
            foreach (var c in dbConfigs)
            {
                StandardConfigModel scm = new StandardConfigModel();
                scm.SQLDataType = db.StandardDataType.Where(x => x.DataTypeID == c.DataTypeID).First().SQLDataType;

                scm.FieldName = c.FieldName;
                Configs.Add(scm);


            }
            Standard model = db.Standard.Where(x => x.StandardID == StandardID).FirstOrDefault();

            //if (model.NotifiyOwner)
            //{
            //    NotifyStandardOwner("UPDATE", fc, Configs, model, UserID);
            //}
            SetStandardVersionValue(model.StandardID);

            string SQL = "UPDATE " + model.DBTableName + " SET ";
            SQL += " ModifiedOn =  '" + DateTime.Now + "', ";
            SQL += " ModifiedBy =  '" + UserID + "', ";




            foreach (var formKey in fc.Keys) //AllKeys)
            {
                List<StandardConfigModel> foundConfig = Configs.Where(c => c.FieldName == formKey).ToList();
                if (foundConfig.Count > 0)
                {
                    StandardConfigModel config = foundConfig.First();
                    string value;

                    if (string.IsNullOrEmpty(fc[config.FieldName]))
                    {
                        value = "";
                    }
                    else
                    {


                        if (config.AllowMultiSelect)
                        {
                            value = ";" + fc[config.FieldName]; //.Replace(",", ";") + ";";
                            value = value.Replace(",", ";") + ";";
                        }
                        else
                        {
                            value = fc[config.FieldName];
                        }
                    }
                    SQL += " " + config.FieldName + " = CAST('" + value + "' AS " + config.SQLDataType + ") , ";

                }
            }

            SQL += " Enabled =  CAST('" + fc["Enabled"] + "' AS BIT) , ";
            SQL += " SortOrder =  " + fc["SortOrder"] + " ";


            SQL += " WHERE ID = " + RecordID;

            bool Result = ExecuteSql(SQL);
            return Result;
        }

        public bool DeleteStandardValue(string StandardID, string RecordID, string UserID)
        {
            //List<StandardConfigModel> Configs = new StandardConfigModel().SelectStandardConfigs(Convert.ToInt32(StandardID));
            //StandardModel model = new StandardModel().SelectSingleStandard(Convert.ToInt32(StandardID));
            AutomationStandardsContext db = new AutomationStandardsContext();
            Standard model = db.Standard.Find(Convert.ToInt32(StandardID));
            if (string.IsNullOrEmpty(UserID))
            {
                UserID = "WebUser";
            }

            //if (model.NotifiyOwner)
            //{
            //    FormCollection newfc = new FormCollection();
                
            //    newfc.Add("StandardID", StandardID);
            //    newfc.Add("ID", RecordID);
            //    NotifyStandardOwner("DELETE", newfc, Configs, model, UserID);
            //}
            SetStandardVersionValue(model.StandardID);


            string SQL = "DELETE FROM " + model.DBTableName + " WHERE ID = " + RecordID;
            bool Result = ExecuteSql(SQL);
            return Result;
        }


        //public bool NotifyStandardOwner(string EntryType, FormCollection fc, List<StandardConfigModel> Configs, StandardModel StandardInfo, string CurrentUser)
        //{
        //    int StandardID = Convert.ToInt32(fc["StandardID"]);
        //    string RecordID = fc["ID"];

        //    string To, Subject, Body, Type;

        //    ActiveDirectoryUserInfo UserInfo = new ActiveDirectoryUserInfo(CurrentUser);

        //    To = StandardInfo.OwnerEmail;
        //    Subject = "OneClick Standard - " + StandardInfo.StandardName;
        //    Body = "";
        //    Type = "";

        //    #region Build Body
        //    switch (EntryType)
        //    {
        //        case "INSERT":
        //            Subject = Subject + " - New Standard Added";
        //            Type = "Inserted";
        //            Body = "A new standard has been added to <b>" + StandardInfo.StandardName + "</b>.  Below is the new standard added.<br><br><br>";

        //            Body += @"<table class=""details-table"" width=""80%""><tr><td class=""titles"" style=""font-family:Calibri,Arial;"">Field</td><td class=""titles"" style=""font-family:Calibri,Arial;"">New Value</td></tr>";

        //            foreach (var formKey in fc.Keys) //AllKeys)
        //            {
        //                List<StandardConfigModel> foundConfig = Configs.Where(c => c.FieldName == formKey).ToList();
        //                if (foundConfig.Count > 0)
        //                {
        //                    StandardConfigModel config = foundConfig.First();
        //                    string newValue;
        //                    if (config.AllowMultiSelect)
        //                    {
        //                        newValue = ";" + fc[config.FieldName].Replace(",", ";") + ";";
        //                    }
        //                    else
        //                    {
        //                        newValue = fc[config.FieldName];
        //                    }

        //                    Body += @"<tr><td class=""values"" style=""font-family:Calibri,Arial;"">" + config.DisplayName + @"</td><td class=""values"" style=""font-family:Calibri,Arial;"">" + newValue + "</td></tr>";
        //                }
        //            }
        //            Body += "</table>";

        //            break;

        //        case "UPDATE":

        //            DataRow OldData = GetStandardRow(StandardInfo.DBTableName, RecordID);

        //            Subject = Subject + " - Existing Standard Updated";
        //            Type = "Updated";
        //            Body = "An existing standard has been updated to <b>" + StandardInfo.StandardName + "</b>.  Below is the old and new standard values.<br><br><br>";

        //            Body += @"<table class=""details-table"" width=""80%""><tr><td class=""titles"" style=""font-family:Calibri,Arial;"">Field</td><td class=""titles"" style=""font-family:Calibri,Arial;"">Old Value</td><td class=""titles"" style=""font-family:Calibri,Arial;"">New Value</td></tr>";

        //            foreach (var formKey in fc.Keys) //AllKeys)
        //            {
        //                List<StandardConfigModel> foundConfig = Configs.Where(c => c.FieldName == formKey).ToList();
        //                if (foundConfig.Count > 0)
        //                {
        //                    StandardConfigModel config = foundConfig.First();

        //                    string newValue;
        //                    if (config.AllowMultiSelect)
        //                    {
        //                        newValue = ";" + fc[config.FieldName].Replace(",", ";") + ";";
        //                    }
        //                    else
        //                    {
        //                        newValue = fc[config.FieldName];
        //                    }

        //                    string OldValue = OldData[config.FieldName].ToString();

        //                    Body += @"<tr><td class=""values"" style=""font-family:Calibri,Arial;"">" + config.DisplayName + @"</td><td class=""values"" style=""font-family:Calibri,Arial;"">" + OldValue + @"</td><td class=""values"" style=""font-family:Calibri,Arial;"">" + newValue + "</td></tr>";
        //                }
        //            }
        //            Body += "</table>";



        //            break;

        //        case "DELETE":
        //            DataRow DeletedData = GetStandardRow(StandardInfo.DBTableName, RecordID);

        //            Subject = Subject + " - Standard Deleted";
        //            Type = "Deleted";
        //            Body = "A standard has been deleted from <b>" + StandardInfo.StandardName + "</b>.  Below is the standard that was deleted.<br><br><br>";

        //            Body += @"<table class=""details-table"" width=""80%""><tr><td class=""titles"" style=""font-family:Calibri,Arial;"">Field</td><td class=""titles"" style=""font-family:Calibri,Arial;"">Deleted Value</td></tr>";

        //            foreach (var config in Configs.OrderBy(c => c.SortOrder))
        //            {

        //                string DeletedValue = DeletedData[config.FieldName].ToString();

        //                Body += @"<tr><td class=""values"" style=""font-family:Calibri,Arial;"">" + config.DisplayName + @"</td><td class=""values"" style=""font-family:Calibri,Arial;"">" + DeletedValue + "</td></tr>";
        //            }
        //            Body += "</table>";

        //            break;
        //    }
        //    #endregion

        //    NotificationTemplateModel tempinfo = new NotificationTemplateModel().SelectSingleNotificationTemplateByName("Standard");

        //    Body = tempinfo.NotificationBody.Replace("{#NotificationBody#}", Body)
        //        .Replace("{#Type#}", Type)
        //        .Replace("{#ChangedBy#}", UserInfo.FirstName + " " + UserInfo.LastName)
        //        .Replace("{#StandardName#}", StandardInfo.StandardName)
        //        .Replace("{#RequestUrl#}", "http://util.obs.org/OneClick/Standard/Viewer/" + StandardInfo.StandardName);


        //    wsESEUtil.ESEUtilitySoapClient ws = new wsESEUtil.ESEUtilitySoapClient();
        //    bool result = ws.sendEmail(Subject, Body, To, true, "eseautomation", "", "", "", "");
        //    return result;
        //}

        public bool SetStandardVersionValue(int StandardID)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            var Standards = db.Standard.Where(d => d.StandardID == StandardID).ToList();
            if (Standards.Count > 0)
            {
                Standard CurrentStandard = Standards.First();
                int NewVersion = CurrentStandard.VersionValue + 1;

                CurrentStandard.VersionValue = NewVersion;

                db.SaveChanges();
            }

            return true;
        }





        public bool SetStandardValueOrder(string RecordID, string SortOrder, string TableName)
        {
            string SQL = "UPDATE " + TableName + " SET SortOrder = " + SortOrder + " WHERE ID = " + RecordID;
            bool Result = ExecuteSql(SQL);
            return Result;
        }

        public bool AddColumn(string TableName, string ColumnName, string DataType, string DefaultValue, bool AllowNull)
        {
            string SQL = @"ALTER TABLE " + TableName + "  ";

            if (AllowNull)
            {
                SQL += @" ADD " + ColumnName + " " + DataType + " NULL ";
            }
            else
            {
                SQL += @"ADD " + ColumnName + " " + DataType + " NOT NULL ";
            }

            if (DefaultValue != "" && DefaultValue != null)
            {
                SQL += @"CONSTRAINT [DF_" + TableName + "_" + ColumnName + "] DEFAULT (" + DefaultValue + ") ";
            }

            bool Result = ExecuteSql(SQL);
            return Result;

        }
        public bool RenameColumn(string TableName, string OldColumnName, string NewColumnName)
        {
            string SQL = "EXEC sp_rename '" + TableName + "." + OldColumnName + "', '" + NewColumnName + "', 'COLUMN';";
            bool Result = ExecuteSql(SQL);
            return Result;

        }
        public bool DeleteColumn(string TableName, string ColumnName)
        {
            string SQL = @"IF OBJECT_ID('DF_" + TableName + "_" + ColumnName + @"', 'D') IS NULL
                          BEGIN
                              ALTER TABLE dbo." + TableName + @"
                                DROP COLUMN " + ColumnName + @"
                          END
                        ELSE
                          BEGIN
                              ALTER TABLE dbo." + TableName + @"
                                DROP CONSTRAINT DF_" + TableName + @"_" + ColumnName + @", 
                                         COLUMN " + ColumnName + @" 
                          END ";
            bool Result = ExecuteSql(SQL);
            return Result;
        }
        public bool CreateStandardTable(string TableName)
        {
            string SQL = @"CREATE TABLE [dbo].[" + TableName + @"](
	                        [ID] [INT] IDENTITY(1,1) NOT NULL,
	                        [CreatedOn] [DATETIME] NULL,
	                        [CreatedBy] [VARCHAR](50) NULL,
	                        [ModifiedOn] [DATETIME] NULL,
	                        [ModifiedBy] [VARCHAR](50) NULL,
	                        [Enabled] [BIT] NOT NULL,
	                        [SortOrder] [INT] NOT NULL,
	                        
                            CONSTRAINT [PK_" + TableName + @"] PRIMARY KEY CLUSTERED 
                        (
	                        [ID] ASC
                        )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                        ) ON [PRIMARY]

                        ALTER TABLE [dbo].[" + TableName + @"] ADD  CONSTRAINT [DF_" + TableName + @"_CreatedOn]  DEFAULT (GETDATE()) FOR [CreatedOn]

                        ALTER TABLE [dbo].[" + TableName + @"] ADD  CONSTRAINT [DF_" + TableName + @"_ModifiedOn]  DEFAULT (GETDATE()) FOR [ModifiedOn]

                        ALTER TABLE [dbo].[" + TableName + @"] ADD  CONSTRAINT [DF_" + TableName + @"_Enabled]  DEFAULT ((0)) FOR [Enabled]

                        ALTER TABLE [dbo].[" + TableName + @"] ADD  CONSTRAINT [DF_" + TableName + @"_SortOrder]  DEFAULT ((0)) FOR [SortOrder]
                        ";

            bool Result = ExecuteSql(SQL);
            return Result;

        }
        public bool DeleteStandardTable(string TableName)
        {
            string SQL = @"IF OBJECT_ID('dbo." + TableName + @"', 'U') IS NOT NULL 
                            DROP TABLE dbo." + TableName + "; ";
            bool Result = ExecuteSql(SQL);
            return Result;
        }
        public bool ExecuteSql(string sql)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            SqlConnection conn = new SqlConnection(AutomationStandardsContext.ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;

            conn.Open();
            int output = cmd.ExecuteNonQuery();

            bool Result = false;

            if (output >= 0)
            {
                Result = true;
            }
            conn.Close();

            return Result;


            //var entityConnection = (System.Data.EntityClient.EntityConnection)db.Database.Connection;
            //DbConnection conn = entityConnection.StoreConnection;
            //ConnectionState initialState = conn.State;
            //try
            //{
            //    if (initialState != ConnectionState.Open)
            //        conn.Open();  // open connection if not already open
            //    using (DbCommand cmd = conn.CreateCommand())
            //    {
            //        cmd.CommandText = sql;
            //        cmd.ExecuteNonQuery();

            //    }
            //}
            //catch
            //{
            //    return false;
            //}
            //finally
            //{
            //    if (initialState != ConnectionState.Open)
            //    {
            //        conn.Close(); // only close connection if not initially open
            //    }

            //}

            //return true;
        }

        public DataTable ExecuteSelectSql(string SQL)
        {
            DataTable Result = new DataTable();

           
            AutomationStandardsContext db = new AutomationStandardsContext();

            #region Execute SQL

            using (SqlConnection sqlConn = new SqlConnection(AutomationStandardsContext.ConnectionString))


            using (SqlCommand cmd = new SqlCommand(SQL, sqlConn))
            {
                sqlConn.Open();
                Result.Load(cmd.ExecuteReader());
                sqlConn.Close();
            }
            #endregion


            return Result;
        }

    }

    public class StandardValueSingle
    {
        public int StandardID { get; set; }
        public string TableName { get; set; }
        public string StandardName { get; set; }
        public DataRow dtRow { get; set; }
        public List<StandardConfigModel> Config { get; set; }

    }
    public class StandardValueSet
    {
        public int StandardID { get; set; }
        public string TableName { get; set; }
        public string StandardName { get; set; }
        public string ManageRoles { get; set; }
        public DataTable TableValues { get; set; }
        public List<StandardConfigModel> Config { get; set; }
    }
}