using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Validation;
//using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using AutomationStandards.Models.DatabaseModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutomationStandards.Models.CustomModels
{
    public class StandardModel : Standard
    {
        //public int StandardID { get; set; }
        //public int StandardGroupID { get; set; }
        public string StandardGroupName { get; set; }
        //public string StandardName { get; set; }
        //public string DBTableName { get; set; }
        //public string StandardDefinition { get; set; }
        //public string ManageRoles { get; set; }
        
        public List<string> ManageRolesArray { get; set; }

        //public string ViewerRoles { get; set; }
        public List<string> ViewerRolesArray { get; set; }


        public int StandardCount { get; set; }
        //public int VersionConfig { get; set; }
        //public int VersionValue { get; set; }
        
        //public string Tags { get; set; }
        //public bool NotifiyOwner { get; set; }
        //public string OwnerEmail { get; set; }
        //public int UsageCount { get; set; }


        public string SelectStanardname(int StandardID)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            string Name = "";

            try
            {
                Name = db.Standard.First(s => s.StandardID == StandardID).StandardName;
            }
            catch
            { }

            return Name;
        }

        public List<SelectListItem> SelectAllStandardsDDL()
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            List<SelectListItem> Result = (from d in db.Standard
                                           select new SelectListItem
                                          {
                                              Value = SqlFunctions.StringConvert((double)d.StandardID).Trim(),
                                              Text = d.StandardName
                                          }).OrderBy(t => t.Text).ToList();

            SelectListItem n = new SelectListItem() { Value = "0", Text = "Please Select One" };
            Result.Insert(0, n);

            return Result;
        }

        public List<StandardModel> SelectAllStandards()
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            List<StandardModel> Result = (from d in db.Standard
                                          select new StandardModel
                                          {
                                              StandardID = d.StandardID,
                                              StandardName = d.StandardName,
                                              StandardDefinition = d.StandardDefinition,
                                              DBTableName = d.DBTableName,
                                              StandardGroupID = d.StandardGroupID,
                                              StandardGroupName = d.StandardGroup.StandardGroupName,
                                              ManageRoles = d.ManageRoles,
                                              ViewerRoles = d.ViewerRoles,
                                              VersionConfig = d.VersionConfig,
                                              VersionValue = d.VersionValue,
                                              Tags = d.Tags,
                                              NotifiyOwner = d.NotifiyOwner,
                                              OwnerEmail = d.OwnerEmail,
                                              UsageCount = d.UsageCount

                                          }).ToList();

            StandardSQLManagement sql = new StandardSQLManagement();
            foreach (StandardModel item in Result)
            {
                item.StandardCount = sql.GetStandardRowsCount(item.DBTableName);

            }

            return Result;
        }

        public List<StandardModel> SelectAllManagedStandardsByRoles(List<string> CurrentRoles)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            List<StandardModel> Result = new List<StandardModel>();
            List<string> QueryRoles = new List<string>();
            foreach (string Role in CurrentRoles)
            {
                QueryRoles.Add(Role.ToLower());
            }


            var AllStandards = SelectAllStandards();
            foreach (var Standard in AllStandards)
            {
                List<string> StandardRoles = Standard.ManageRoles.ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                List<string> ManageRoles = new List<string>();
                foreach (string Role in StandardRoles)
                {
                    ManageRoles.Add(Role.Trim().ToLower());
                }


                if (ManageRoles.Intersect(QueryRoles).Any())
                {
                    Result.Add(Standard);
                }
            }

            return Result;
        }


        public List<StandardModel> SelectAllViewerStandardsByRoles(List<string> CurrentRoles)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            List<StandardModel> Result = new List<StandardModel>();
            List<string> QueryRoles = new List<string>();
            foreach (string Role in CurrentRoles)
            {
                QueryRoles.Add(Role.ToLower());
            }


            var AllStandards = SelectAllStandards();
            foreach (var Standard in AllStandards)
            {
                List<string> StandardRoles = Standard.ViewerRoles.ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                List<string> ViewerRoles = new List<string>();
                foreach (string Role in StandardRoles)
                {
                    ViewerRoles.Add(Role.Trim().ToLower());
                }


                if (ViewerRoles.Intersect(QueryRoles).Any())
                {
                    Result.Add(Standard);
                }

                
            }

            return Result;
        }

        public StandardModel SelectSingleStandard(int StandardID)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            StandardModel Result = new StandardModel();

            //if (StandardID != 0)
            //{
            //    Result = (from d in db.Standard
            //                            where d.StandardID == StandardID
            //                            select new StandardModel
            //                            {
            //                                StandardID = d.StandardID,
            //                                StandardName = d.StandardName,
            //                                StandardDefinition = d.StandardDefinition,
            //                                DBTableName = d.DBTableName,
            //                                StandardGroupID = d.StandardGroupID,
            //                                StandardGroupName = d.StandardGroup.StandardGroupName,
            //                                ManageRoles = d.ManageRoles,
            //                                ViewerRoles = d.ViewerRoles,
            //                                VersionConfig = d.VersionConfig,
            //                                VersionValue = d.VersionValue,
            //                                Tags = d.Tags,
            //                                NotifiyOwner = d.NotifiyOwner,
            //                                OwnerEmail = d.OwnerEmail,
            //                                UsageCount = d.UsageCount
            //                            }).First();
            //}

            Standard CurrentStandard = db.Standard.First(s => s.StandardID == StandardID);
            CurrentStandard.UsageCount = CurrentStandard.UsageCount + 1;
            try
            {
                db.SaveChanges();
            }
            catch
            {
                string body = "";
                body += string.Format("Failed to increment usage counter for Standard: \"{0}\" ID: \"{1}\" ", CurrentStandard.DBTableName, StandardID);
                //GlobalFunctions gf = new //GlobalFunctions();
                //gf.SendErrorModelEmail(this.GetType().Name, body);
                return Result;
            }

            return Result;
        }
        public Standard SelectSingleStandardAsDbStandard(int StandardID)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            StandardModel Result = new StandardModel();

            //if (StandardID != 0)
            //{
            //    Result = (from d in db.Standard
            //                            where d.StandardID == StandardID
            //                            select new StandardModel
            //                            {
            //                                StandardID = d.StandardID,
            //                                StandardName = d.StandardName,
            //                                StandardDefinition = d.StandardDefinition,
            //                                DBTableName = d.DBTableName,
            //                                StandardGroupID = d.StandardGroupID,
            //                                StandardGroupName = d.StandardGroup.StandardGroupName,
            //                                ManageRoles = d.ManageRoles,
            //                                ViewerRoles = d.ViewerRoles,
            //                                VersionConfig = d.VersionConfig,
            //                                VersionValue = d.VersionValue,
            //                                Tags = d.Tags,
            //                                NotifiyOwner = d.NotifiyOwner,
            //                                OwnerEmail = d.OwnerEmail,
            //                                UsageCount = d.UsageCount
            //                            }).First();
            //}

            Standard CurrentStandard = db.Standard.First(s => s.StandardID == StandardID);
            CurrentStandard.UsageCount = CurrentStandard.UsageCount + 1;
            try
            {
             
                db.SaveChanges();
            }
            catch
            {
                string body = "";
                body += string.Format("Failed to increment usage counter for Standard: \"{0}\" ID: \"{1}\" ", CurrentStandard.DBTableName, StandardID);
                //GlobalFunctions gf = new //GlobalFunctions();
                //gf.SendErrorModelEmail(this.GetType().Name, body);
                return CurrentStandard;
            }

            return Result;
        }

        public StandardModel SelectSingleStandardByName(string StandardName)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            StandardModel Result = new StandardModel();
            try
            {
                Result = (from d in db.Standard
                          where d.StandardName == StandardName
                          select new StandardModel
                          {
                              StandardID = d.StandardID,
                              StandardName = d.StandardName,
                              StandardDefinition = d.StandardDefinition,
                              DBTableName = d.DBTableName,
                              StandardGroupID = d.StandardGroupID,
                              StandardGroupName = d.StandardGroup.StandardGroupName,
                              ManageRoles = d.ManageRoles,
                              ViewerRoles = d.ViewerRoles,
                              VersionConfig = d.VersionConfig,
                              VersionValue = d.VersionValue,
                              Tags = d.Tags,
                              NotifiyOwner = d.NotifiyOwner,
                              OwnerEmail = d.OwnerEmail,
                              UsageCount = d.UsageCount
                              
                          }).First();
            }
            catch
            {
                return new StandardModel();
            }

            return Result;
        }
        public Standard SelectSingleStandardAsDbStandardByName(string StandardName)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            Standard Result = new Standard();
            try
            {
                Result = db.Standard.Where(x => x.StandardName == StandardName).FirstOrDefault();

            }
            catch
            {
                return new Standard();
            }

            return Result;
        }

        public StandardModel SelectSingleStandardByDBTableName(string DBTableName)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            StandardModel Result = new StandardModel();
            try
            {
                Result = (from d in db.Standard
                          where d.DBTableName == DBTableName
                          select new StandardModel
                          {
                              StandardID = d.StandardID,
                              StandardName = d.StandardName,
                              StandardDefinition = d.StandardDefinition,
                              DBTableName = d.DBTableName,
                              StandardGroupID = d.StandardGroupID,
                              StandardGroupName = d.StandardGroup.StandardGroupName,
                              ManageRoles = d.ManageRoles,
                              ViewerRoles = d.ViewerRoles,
                              VersionConfig = d.VersionConfig,
                              VersionValue = d.VersionValue,
                              Tags = d.Tags,
                              NotifiyOwner = d.NotifiyOwner,
                              OwnerEmail = d.OwnerEmail,
                              UsageCount = d.UsageCount
                          }).First();
            }
            catch
            {
                return new StandardModel();
            }

            return Result;
        }

        public bool InsertStandard(StandardModel model)
        {
            bool OkToCreate = false;
            AutomationStandardsContext db = new AutomationStandardsContext();
            var CurrentStds = (from s in db.Standard where s.DBTableName.ToLower() == model.DBTableName.ToLower() select s).ToList();

            if (CurrentStds.Count == 0)
            {
                OkToCreate = true;
            }


            if (OkToCreate)
            {




                Standard Result = new Standard();
                Result.StandardName = model.StandardName;
                Result.DBTableName = model.DBTableName;
                Result.StandardDefinition = model.StandardDefinition;
                Result.StandardGroupID = model.StandardGroupID;
                Result.ManageRoles = model.ManageRoles;
                Result.ViewerRoles = model.ViewerRoles;
                Result.VersionConfig = model.VersionConfig;
                Result.VersionValue = model.VersionValue;
                Result.Tags = model.Tags;
                Result.NotifiyOwner = model.NotifiyOwner;
                Result.OwnerEmail = model.OwnerEmail;
                Result.UsageCount = 0;

                db.Standard.Add(Result);

                try
                {
                    db.SaveChanges();
                    model.StandardID = Result.StandardID;

                    //Create Table in DB with MODEL Values
                    StandardSQLManagement sql = new StandardSQLManagement();
                    bool SQLOK = sql.CreateStandardTable(model.DBTableName);
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

                    //GlobalFunctions gf = new //GlobalFunctions();
                    //gf.SendErrorModelEmail(this.GetType().Name, body);
                    return false;
                }
            }


            return true;
        }

        public bool UpdateStandard(StandardModel model)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            Standard Result = db.Standard.First(d => d.StandardID == model.StandardID);

            Result.StandardName = model.StandardName;
            Result.StandardGroupID = model.StandardGroupID;
            Result.StandardDefinition = model.StandardDefinition;
            Result.ManageRoles = model.ManageRoles;
            Result.ViewerRoles = model.ViewerRoles;
            Result.VersionConfig = model.VersionConfig;
            Result.VersionValue = model.VersionValue;
            Result.Tags = model.Tags;
            Result.NotifiyOwner = model.NotifiyOwner;
            Result.OwnerEmail = model.OwnerEmail;
            
            try
            {
                db.SaveChanges();
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

                //GlobalFunctions gf = new //GlobalFunctions();
                //gf.SendErrorModelEmail(this.GetType().Name, body);
                return false;
            }

        }

        public bool DeleteStandard(int StandardID)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            var Standards = db.Standard.Where(d => d.StandardID == StandardID).ToList();
            if (Standards.Count > 0)
            {

                var Configs = db.StandardConfig.Where(c => c.StandardID == StandardID).ToList();
                foreach (var Config in Configs)
                {
                    db.StandardConfig.Remove(Config);
                    db.SaveChanges();
                }

                db.Standard.Remove(Standards.First());



                try
                {
                    db.SaveChanges();

                    //DELETE TABLE
                    StandardSQLManagement sql = new StandardSQLManagement();
                    bool SQLOK = sql.DeleteStandardTable(Standards.First().DBTableName);

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

                    //GlobalFunctions gf = new //GlobalFunctions();
                    //gf.SendErrorModelEmail(this.GetType().Name, body);
                    return false;
                }
            }

            return true;
        }

        

    }

}