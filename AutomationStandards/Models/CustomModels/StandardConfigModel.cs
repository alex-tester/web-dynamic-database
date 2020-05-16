using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using AutomationStandards.Models.DatabaseModels;

namespace AutomationStandards.Models.CustomModels
{
    public class StandardConfigModel : StandardConfig
    {
        //public int StandardConfigID { get; set; }
        //public int StandardID { get; set; }
        public string StandardName { get; set; }
        //public string FieldName { get; set; }
        //public string DisplayName { get; set; }
        //public int DataTypeID { get; set; }
        public string DataTypeName { get; set; }
        public string SQLDataType { get; set; }
        //public int SortOrder { get; set; }

        //public int VersionNumber { get; set; }
        //public bool UseToolTip { get; set; }
        //public string ToolTip { get; set; }
        //public bool UseStandardData { get; set; }
        //public bool AllowMultiSelect { get; set; }
        //public int StandardLUID { get; set; }
        //public string StandardLUValue { get; set; }
        //public bool StandardUseFilter { get; set; }
        //public string StandardFilterSQL { get; set; }




        public List<StandardConfigModel> SelectStandardConfigs(int StandardID)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            List<StandardDataType> dtp = new List<StandardDataType>();
            dtp = db.StandardDataType.ToList();
            Standard st = db.Standard.Where(x => x.StandardID == StandardID).FirstOrDefault();

            List<StandardConfigModel> Result = new List<StandardConfigModel>(); //(from d in db.StandardConfig
            //                                    where d.StandardID == StandardID
            List<StandardConfig> scmResult = db.StandardConfig.Where(d => d.StandardID == StandardID).ToList();
            foreach (var d  in scmResult)
            {


                StandardConfigModel sc = new StandardConfigModel();


                sc.StandardConfigID = d.StandardConfigID;
                                                    sc.StandardID = d.StandardID;
                sc.StandardName = st.StandardName; //d.Standard.StandardName;
                                                    sc.FieldName = d.FieldName;
                                                    sc.DisplayName = d.DisplayName;
                                                    sc.DataTypeID = d.DataTypeID;
                                                    sc.DataTypeName = dtp.Where(x => x.DataTypeID == d.DataTypeID).First().DataTypeName; //d.DataType.DataTypeName; // StandardDataType.DataTypeName;
                sc.SQLDataType = dtp.Where(x => x.DataTypeID == d.DataTypeID).First().SQLDataType; //d.DataType.SQLDataType; //StandardDataType.SQLDataType;
                                                    sc.SortOrder = d.SortOrder;
                                                    sc.VersionNumber = d.VersionNumber;
                                                    sc.UseToolTip = d.UseToolTip;
                                                    sc.ToolTip = d.ToolTip;
                                                    sc.UseStandardData = d.UseStandardData;
                                                    sc.AllowMultiSelect = d.AllowMultiSelect;
                                                    sc.StandardLUID = d.StandardLUID;
                                                    sc.StandardLUValue = d.StandardLUValue;
                                                    sc.StandardUseFilter = d.StandardUseFilter;
                sc.StandardFilterSQL = d.StandardFilterSQL;
                Result.Add(sc);          

            }

            return Result;
        }

        public StandardConfigModel SelectSingleStandardConfig(int StandardConfigID)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            StandardConfigModel Result = (from d in db.StandardConfig
                                          where d.StandardConfigID == StandardConfigID
                                          select new StandardConfigModel
                                          {
                                              StandardConfigID = d.StandardConfigID,
                                              StandardID = d.StandardID,
                                              StandardName = d.Standard.StandardName,
                                              FieldName = d.FieldName,
                                              DisplayName = d.DisplayName,
                                              DataTypeID = d.DataTypeID,
                                              DataTypeName = d.DataType.DataTypeName, // StandardDataType.DataTypeName,
                                              SQLDataType = d.DataType.SQLDataType, // StandardDataType.SQLDataType,
                                              SortOrder = d.SortOrder,
                                              VersionNumber = d.VersionNumber,
                                              UseToolTip = d.UseToolTip,
                                              ToolTip = d.ToolTip,
                                              UseStandardData = d.UseStandardData,
                                              AllowMultiSelect = d.AllowMultiSelect,
                                              StandardLUID = d.StandardLUID,
                                              StandardLUValue = d.StandardLUValue,
                                              StandardUseFilter = d.StandardUseFilter,
                                              StandardFilterSQL = d.StandardFilterSQL
                                          }).First();

            return Result;
        }


        public bool InsertStandardConfig(StandardConfigModel model)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            StandardConfig Result = new StandardConfig();
            Result.StandardID = model.StandardID;
            //Result.FieldName = model.FieldName;
            Result.FieldName = model.DisplayName.Replace(" ", "");
            Result.DisplayName = model.DisplayName;
            Result.DataTypeID = model.DataTypeID;
            Result.SortOrder = model.SortOrder;
            Result.VersionNumber = model.VersionNumber;
            Result.UseToolTip = model.UseToolTip;
            Result.ToolTip = model.ToolTip;
            Result.UseStandardData = model.UseStandardData;
            Result.AllowMultiSelect = model.AllowMultiSelect;
            Result.StandardLUID = model.StandardLUID;
            Result.StandardLUValue = model.StandardLUValue;
            Result.StandardUseFilter = model.StandardUseFilter;
            Result.StandardFilterSQL = model.StandardFilterSQL;

            db.StandardConfig.Add(Result);

            try
            {
                db.SaveChanges();
                model.StandardConfigID = Result.StandardConfigID;

                //Add Column To the Table
                StandardSQLManagement sql = new StandardSQLManagement();

                var Standard = db.Standard.First(s => s.StandardID == model.StandardID);
                var DBType = db.StandardDataType.First(d => d.DataTypeID == model.DataTypeID);
                string DefaultValue = "";
                bool AllowNull = true;
                if (DBType.SQLDataType == "BIT")
                {
                    
                    DefaultValue = "(0)";
                }

                sql.AddColumn(Standard.DBTableName,Result.FieldName,DBType.SQLDataType,DefaultValue,AllowNull);

                SetStandardVersion(model.StandardID);

                return true;
            }
            catch (DbEntityValidationException ex)
            {
                string body = "";
                foreach (var eve in ex.EntityValidationErrors)
                {
                    body += string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    body += "<br>";
                    foreach (var ve in eve.ValidationErrors)
                    {
                        body += string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }

                ////GlobalFunctions gf = new //GlobalFunctions();
                ////gf.SendErrorModelEmail(this.GetType().Name, body);
                return false;
            }
        }

        public bool UpdateStandardConfig(StandardConfigModel model)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            StandardConfig Result = db.StandardConfig.First(d => d.StandardConfigID == model.StandardConfigID);

            Standard st = db.Standard.First(x => x.StandardID == model.StandardID);
            Result.Standard = st;
            bool RenameCol = false;
            string OldCol = "", NewCol = "", TableName = "";

            if (Result.FieldName != model.FieldName)
            {
                RenameCol = true;
                OldCol = Result.FieldName;
                NewCol = model.FieldName;
                TableName = Result.Standard.DBTableName;
            }

            Result.DisplayName = model.DisplayName;
            Result.FieldName = model.FieldName;
            Result.SortOrder = model.SortOrder;

            Result.VersionNumber = model.VersionNumber;
            Result.UseToolTip = model.UseToolTip;
            Result.ToolTip = model.ToolTip;
            Result.UseStandardData = model.UseStandardData;
            Result.AllowMultiSelect = model.AllowMultiSelect;
            Result.StandardLUID = model.StandardLUID;
            Result.StandardLUValue = model.StandardLUValue;
            Result.StandardUseFilter = model.StandardUseFilter;
            Result.StandardFilterSQL = model.StandardFilterSQL;

            try
            {
                db.SaveChanges();

                if (RenameCol)
                {
                    StandardSQLManagement sql = new StandardSQLManagement();
                    bool SQLOK = sql.RenameColumn(TableName, OldCol, NewCol);
                    return SQLOK;
                }

                SetStandardVersion(model.StandardID);

                return true;
            }
            catch (DbEntityValidationException ex)
            {
                string body = "";
                foreach (var eve in ex.EntityValidationErrors)
                {
                    body += string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    body += "<br>";
                    foreach (var ve in eve.ValidationErrors)
                    {
                        body += string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }

                ////GlobalFunctions gf = new //GlobalFunctions();
                ////gf.SendErrorModelEmail(this.GetType().Name, body);
                return false;
            }

        }

        public bool DeleteStandardConfig(int StandardConfigID)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            var StandardConfigs = db.StandardConfig.Where(d => d.StandardConfigID == StandardConfigID).ToList();
            if (StandardConfigs.Count > 0)
            {
                StandardConfigs.First().Standard = db.Standard.Where(x => x.StandardID == StandardConfigs.First().StandardID).FirstOrDefault();
                int StandardID = StandardConfigs.First().StandardID; 
                string TableName = StandardConfigs.First().Standard.DBTableName;
                string FieldName = StandardConfigs.First().FieldName;
                db.StandardConfig.Remove(StandardConfigs.First());



                try
                {
                    db.SaveChanges();

                    StandardSQLManagement sql = new StandardSQLManagement();
                    bool SQLOK = sql.DeleteColumn(TableName, FieldName);

                    SetStandardVersion(StandardID);

                    return SQLOK;
                }
                catch (DbEntityValidationException ex)
                {
                    string body = "";
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        body += string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        body += "<br>";
                        foreach (var ve in eve.ValidationErrors)
                        {
                            body += string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                        }
                    }

                    ////GlobalFunctions gf = new //GlobalFunctions();
                    ////gf.SendErrorModelEmail(this.GetType().Name, body);
                    return false;
                }
            }

            return true;
        }

        public bool SetStandardConfigSortOrder(int ConfigID, int SortOrder)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            var ConfigInfo = (from f in db.StandardConfig
                            where f.StandardConfigID == ConfigID
                            select f).ToList();

            if (ConfigInfo.Count == 0)
            {
                return false;
            }

            var Config = ConfigInfo.First();
            Config.SortOrder = SortOrder;

            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string body = "";
                foreach (var eve in ex.EntityValidationErrors)
                {
                    body += string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    body += "<br>";
                    foreach (var ve in eve.ValidationErrors)
                    {
                        body += string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }

                ////GlobalFunctions gf = new //GlobalFunctions();
                ////gf.SendErrorModelEmail(this.GetType().Name, body);
                return false;
            }

            return true;
        }

        public bool SetStandardVersion(int StandardID)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            var Standards = db.Standard.Where(d => d.StandardID == StandardID).ToList();
            if (Standards.Count > 0)
            {
                Standard CurrentStandard = Standards.First();
                int NewVersion = CurrentStandard.VersionConfig + 1;

                CurrentStandard.VersionConfig = NewVersion;

                foreach (StandardConfig config in CurrentStandard.StandardConfig)
                {
                    config.VersionNumber = NewVersion;
                }

                db.SaveChanges();

                
            }


            return true;
        }

    }

}