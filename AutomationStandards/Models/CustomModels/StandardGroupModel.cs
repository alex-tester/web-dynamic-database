using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
//using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using AutomationStandards.Models.DatabaseModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutomationStandards.Models.CustomModels
{
    public class StandardGroupModel
    {
        public int StandardGroupID { get; set; }
        public string StandardGroupName { get; set; }



        public IEnumerable<SelectListItem> DDLStandardGroup()
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            List<SelectListItem> Result = new List<SelectListItem>();

            var items = (from d in db.StandardGroup
                         select new
                         {
                             Value = d.StandardGroupID,
                             Text = d.StandardGroupName
                         }
                  ).OrderBy(s => s.Text).ToList();

            foreach (var i in items)
            {
                SelectListItem l = new SelectListItem();
                l.Text = i.Text;
                l.Value = i.Value.ToString();
                Result.Add(l);
            }

            return Result;
        }

        public List<StandardGroupModel> SelectAllStandardGroup()
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            List<StandardGroupModel> Result = (from d in db.StandardGroup
                                               select new StandardGroupModel
                                               {
                                                   StandardGroupID = d.StandardGroupID,
                                                   StandardGroupName = d.StandardGroupName
                                               }).ToList();

            return Result;
        }

        public StandardGroupModel SelectSingleStandardGroup(int GroupID)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            StandardGroupModel Result = (from d in db.StandardGroup
                                         where d.StandardGroupID == GroupID
                                         select new StandardGroupModel
                                         {
                                             StandardGroupID = d.StandardGroupID,
                                             StandardGroupName = d.StandardGroupName
                                         }).First();

            return Result;
        }


        public bool InsertStandardGroup(StandardGroupModel model)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            StandardGroup Result = new StandardGroup();
            Result.StandardGroupName = model.StandardGroupName;

            db.StandardGroup.Add(Result);

            try
            {
                db.SaveChanges();
                model.StandardGroupID = Result.StandardGroupID;
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

        public bool UpdateStandardGroup(StandardGroupModel model)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            StandardGroup Result = db.StandardGroup.First(d => d.StandardGroupID == model.StandardGroupID);

            Result.StandardGroupName = model.StandardGroupName;

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

        public bool DeleteStandardGroup(int GroupID)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            var Groups = db.StandardGroup.Where(d => d.StandardGroupID == GroupID).ToList();
            if (Groups.Count > 0)
            {
                db.StandardGroup.Remove(Groups.First());

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

            return true;
        }

    }

}