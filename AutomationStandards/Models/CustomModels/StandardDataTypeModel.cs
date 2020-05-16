using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using AutomationStandards.Models.DatabaseModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutomationStandards.Models.CustomModels
{
    public class StandardDataTypeModel
    {
        public int StandardDataTypeID { get; set; }
        public string StandardDataTypeName { get; set; }
        public string SQLDataType { get; set; }



        public IEnumerable<SelectListItem> DDLStandardDataTypes()
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            List<SelectListItem> Result = new List<SelectListItem>();


            var items = (from d in db.StandardDataType
                         select new
                         {
                             Value = d.DataTypeID,
                             Text = d.DataTypeName
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

        public List<StandardDataTypeModel> SelectStandardDataTypes()
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            List<StandardDataTypeModel> Result = (from d in db.StandardDataType
                                                  select new StandardDataTypeModel
                                                  {
                                                      StandardDataTypeID = d.DataTypeID,
                                                      StandardDataTypeName = d.DataTypeName,
                                                      SQLDataType = d.SQLDataType

                                                  }).ToList();

            return Result;
        }

        public StandardDataTypeModel SelectSingleStandardDataType(int DataTypeID)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            StandardDataTypeModel Result = (from d in db.StandardDataType
                                            where d.DataTypeID == DataTypeID
                                            select new StandardDataTypeModel
                                                     {
                                                         StandardDataTypeID = d.DataTypeID,
                                                         StandardDataTypeName = d.DataTypeName,
                                                         SQLDataType = d.SQLDataType

                                                     }).First();

            return Result;
        }


        public bool InsertStandardDataType(StandardDataTypeModel model)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            StandardDataType Result = new StandardDataType();
            Result.DataTypeName = model.StandardDataTypeName;
            Result.SQLDataType = model.SQLDataType;

            db.StandardDataType.Add(Result);

            try
            {
                db.SaveChanges();
                model.StandardDataTypeID = Result.DataTypeID;
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

        public bool UpdateStandardDataType(StandardDataTypeModel model)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            StandardDataType Result = db.StandardDataType.First(d => d.DataTypeID == model.StandardDataTypeID);

            Result.DataTypeName = model.StandardDataTypeName;
            Result.SQLDataType = model.SQLDataType;

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

        public bool DeleteStandardDataType(int DataTypeID)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            var DataTypes = db.StandardDataType.Where(d => d.DataTypeID == DataTypeID).ToList();
            if (DataTypes.Count > 0)
            {
                db.StandardDataType.Remove(DataTypes.First());

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