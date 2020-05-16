using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using AutomationStandards.Models.DatabaseModels;
using AutomationStandards.Models;
using AutomationStandards.Models.CustomModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.Entity.Validation;
using System.Web.Helpers;

namespace AutomationStandards.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class ManageController : Controller
    {
        //
        // GET: /Manage/

        public ActionResult Index()
        {


            return View();
        }





        #region StandardDataType

        public ActionResult StandardDataType()
        {
            return View();
        }
        public PartialViewResult StandardDataTypeGrid()
        {
            StandardDataTypeModel L = new StandardDataTypeModel();
            List<StandardDataTypeModel> model = L.SelectStandardDataTypes();
            return PartialView(model);
        }

        //Select
        public PartialViewResult StandardDataTypeEditor(string id)
        {
            StandardDataTypeModel L = new StandardDataTypeModel();
            StandardDataTypeModel model = new StandardDataTypeModel();

            if (id != "0" && id != null)
            {
                model = L.SelectSingleStandardDataType(Convert.ToInt32(id));
            }
            else
            {
                model.StandardDataTypeID = 0;
            }
            return PartialView(model);
        }

        [HttpPost] //Update & Insert
        public ActionResult ProcessStandardDataTypeModel(StandardDataTypeModel model)
        {
            StandardDataTypeModel L = new StandardDataTypeModel();

            if (model.StandardDataTypeID == 0)
            {
                L.InsertStandardDataType(model);
            }
            else
            {
                L.UpdateStandardDataType(model);
            }

            return Json(new { result = true });
        }

        [HttpPost]  //Delete
        public ActionResult DeleteStandardDataTypeModel(string id)
        {
            StandardDataTypeModel L = new StandardDataTypeModel();

            L.DeleteStandardDataType(Convert.ToInt32(id));

            return Json(new { result = true, failure = false });
        }
        #endregion


        #region StandardGroup

        public ActionResult StandardGroup()
        {
            return View();
        }
        public PartialViewResult StandardGroupGrid()
        {
            StandardGroupModel L = new StandardGroupModel();
            List<StandardGroupModel> model = L.SelectAllStandardGroup();
            return PartialView(model);
        }

        //Select
        public PartialViewResult StandardGroupEditor(string id)
        {
            StandardGroupModel L = new StandardGroupModel();
            StandardGroupModel model = new StandardGroupModel();




            if (id != "0" && id != null)
            {
                model = L.SelectSingleStandardGroup(Convert.ToInt32(id));
            }
            else
            {
                model.StandardGroupID = 0;
            }
            return PartialView(model);
        }

        [HttpPost] //Update & Insert
        public ActionResult ProcessStandardGroupModel(StandardGroupModel model)
        {
            StandardGroupModel L = new StandardGroupModel();

            if (model.StandardGroupID == 0)
            {
                L.InsertStandardGroup(model);
            }
            else
            {
                L.UpdateStandardGroup(model);
            }

            return Json(new { result = true });
        }

        [HttpPost]  //Delete
        public ActionResult DeleteStandardGroupModel(string id)
        {
            StandardGroupModel L = new StandardGroupModel();

            //L.DeleteStandardGroup(Convert.ToInt32(id));

            AutomationStandardsContext db = new AutomationStandardsContext();
            var Groups = db.StandardGroup.Where(d => d.StandardGroupID == Convert.ToInt32(id)).ToList();
            if (Groups.Count > 0)
            {
                List<Standard> standardsUsingThisGroup = db.Standard.Where(x => x.StandardGroupID == Convert.ToInt32(id)).ToList();
                if (standardsUsingThisGroup.Count > 0)
                {
                    return Json(new { failure = true, message = "There are still standards using this group!" });
                }

                db.StandardGroup.Remove(Groups.First());

                try
                {
                    db.SaveChanges();
                    return Json(new { failure = false, message = "" });
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
                    return Json(new { failure = true, message = body });
                }
            }
            else
            {
                return Json(new { failure = true, message = "Unable to locate this standard group!" });
            }

            
        }
        #endregion


        #region Standard
        public ActionResult StandardModel()
        {
            return View();
        }

        public ActionResult Standard()
        {
            return View();
        }

        public PartialViewResult StandardGrid()
        {
            StandardModel L = new StandardModel();
            List<StandardModel> model = L.SelectAllStandards().OrderBy(f => f.StandardName).ToList();
            return PartialView(model);
        }

        //Select
        public PartialViewResult StandardEditor(string id)
        {
            StandardModel L = new StandardModel();
            //StandardModel model = new StandardModel();
            Standard model = new Standard();
            AutomationStandardsContext db = new AutomationStandardsContext();
            if (id != "0" && id != null)
            {
                //model = L.SelectSingleStandard(Convert.ToInt32(id));
                model = db.Standard.Where(x => x.StandardID == Convert.ToInt32(id)).FirstOrDefault();

            }
            else
            {
                model.StandardID = 0;

            }

            List<StandardGroup> sGroups = db.StandardGroup.ToList();
            ViewBag.sGroups = sGroups;
            return PartialView(model);
        }

        [HttpPost] //Update & Insert
        public JsonResult ProcessStandardModel(StandardModel model)
        {

            StandardModel L = new StandardModel();

            model.ManageRoles = string.Join(",", model.ManageRolesArray);
            model.ViewerRoles = string.Join(",", model.ViewerRolesArray);

            if (model.DBTableName == null)
            {
                model.DBTableName = "St" + model.StandardName.Replace(" ", "");
            }
            if (model.StandardID == 0)
            {
                //L.InsertStandard(model);
                bool OkToCreate = false;
                AutomationStandardsContext db = new AutomationStandardsContext();

                var CurrentStds = (from s in db.Standard where s.DBTableName.ToLower() == model.DBTableName.ToLower() select s).ToList();

                if (CurrentStds.Count == 0)
                {
                    OkToCreate = true;
                }
                else
                {
                    return Json(new { Failure = true, Message = "Another standard table with this name already exists" });
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

                            return Json(new { Failure = false, Message = "" });
   
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
                        return Json(new { Failure = true, Message = body});
                    }
                }


                //return true;
            }
            else
            {
                //L.UpdateStandard(model);
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
                    //return true;
                    return Json(new { Failure = false, Message = "" });
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
                    //return false;
                    return Json(new { Failure = true, Message = body });
                }

            }

            return Json(new { result = true });
        }

        [HttpPost]  //Delete
        public ActionResult DeleteStandardModel(string id)
        {
            StandardModel L = new StandardModel();


            AutomationStandardsContext db = new AutomationStandardsContext();
            Standard delStandard = db.Standard.Find(Convert.ToInt32(id));

            //check for associated standard configs
            List<StandardConfig> associatedConfigs = db.StandardConfig.Where(x => x.StandardLUID == delStandard.StandardID).ToList();
            if (associatedConfigs.Count > 0)
            {
                string body = "Unable to delete this standard since one or more of it's fields are being used by other standards as a lookup table. <br>Associated standards below. <br><br>";
                body += @"<table class='table table-sm table-striped table-vcenter'>
                            <thead class='thead-dark'>
                                <tr>
                                    <th>Standard Name</th>
                                    <th>Field Name</th>
                                </tr>
                            </thead>
                            <tbody>";
                foreach (var sc in associatedConfigs)
                {
                    string currentStandardName = db.Standard.Find(sc.StandardID).StandardName;
                    body += "<tr><td>" + currentStandardName + "</td><td>" + sc.StandardLUValue + "</td></tr>";

                }
                body += "</tbody></table>";

                return Json(new { failure = true, message = body });
            }
            
            try
            {
                L.DeleteStandard(Convert.ToInt32(id));
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content(ex.InnerException.Message);
            }


        }

        #endregion

        #region StandardConfig

        public PartialViewResult StandardConfigGrid(string id)
        {

            int ID = Convert.ToInt32(id);
            StandardConfigModel L = new StandardConfigModel();
            List<StandardConfigModel> model = L.SelectStandardConfigs(ID);

            ViewBag.StandardName = new StandardModel().SelectStanardname(ID);
            ViewBag.StandardID = id;
            return PartialView(model);
        }

        public PartialViewResult StandardConfigEditor(string id, string ParentId)
        {
            StandardConfigModel L = new StandardConfigModel();
            StandardConfigModel model = new StandardConfigModel();

            //List<SelectListItem> stdata = new StandardModel().SelectAllStandardsDDL().ToList();
            AutomationStandardsContext db = new AutomationStandardsContext();
            List<Standard> stdata = db.Standard.ToList();
            ViewBag.stdata = stdata;
            List<StandardDataType> dataTypes = db.StandardDataType.ToList();
            ViewBag.dataTypes = dataTypes;


            if (id != "0" && id != null)
            {
                model = L.SelectSingleStandardConfig(Convert.ToInt32(id));

            }
            else
            {
                int currentStandardConfigCount = db.StandardConfig.Where(x => x.StandardID == Convert.ToInt32(ParentId)).Count();
                model.SortOrder = currentStandardConfigCount + 1;
                model.StandardConfigID = 0;
                
            }

            model.StandardID = Convert.ToInt32(ParentId);
            return PartialView(model);
        }

        //Select
        public PartialViewResult StandardConfigEditorOLD(string id, string StandardID)
        {
            StandardConfigModel L = new StandardConfigModel();
            StandardConfigModel model = new StandardConfigModel();



            if (id != "0" && id != null)
            {
                model = L.SelectSingleStandardConfig(Convert.ToInt32(id));

            }
            else
            {
                model.StandardConfigID = 0;
                model.StandardID = Convert.ToInt32(StandardID);
            }

            return PartialView(model);
        }

        [HttpPost] //Update & Insert
        //[ValidateInput(false)]
        public ActionResult ProcessStandardConfigModel(StandardConfigModel model)
        {

            StandardConfigModel L = new StandardConfigModel();

            if (model.StandardConfigID == 0)
            {
                L.InsertStandardConfig(model);
            }
            else
            {
                L.UpdateStandardConfig(model);
            }

            return Json(new { result = true });
        }

        [HttpPost]  //Delete
        public JsonResult DeleteStandardConfigModel(string id)
        {
            StandardConfigModel L = new StandardConfigModel();

            //L.DeleteStandardConfig(Convert.ToInt32(id));

            //return Json(new { result = true });
            AutomationStandardsContext db = new AutomationStandardsContext();
            var StandardConfigs = db.StandardConfig.Where(d => d.StandardConfigID == Convert.ToInt32(id)).ToList();
            if (StandardConfigs.Count > 0)
            {
                List<StandardConfig> associatedLookups = db.StandardConfig.Where(x => x.StandardLUID == StandardConfigs.First().StandardID).ToList();
                if (associatedLookups.Count() > 0)
                {
                    string body = "Unable to delete this standard field since it is being used by other standards as a lookup table. <br>Associated standards below. <br><br>";
                    body += @"<table class='table table-sm table-striped table-vcenter'>
                            <thead class='thead-dark'>
                                <tr>
                                    <th>Standard Name</th>
                                    <th>Field Name</th>
                                </tr>
                            </thead>
                            <tbody>";
                    foreach (var sc in associatedLookups)
                    {
                        string currentStandardName = db.Standard.Find(sc.StandardID).StandardName;
                        body += "<tr><td>" + currentStandardName + "</td><td>" + sc.FieldName + "</td></tr>";

                    }
                    body += "</tbody></table>";

                    return Json(new { failure = true, message = body });
                }
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

                    L.SetStandardVersion(StandardID);

                    return Json(new { failure = false, message = "" });
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
                    return Json(new { failure = true, message = body });
                }
            }
            else
            {
                return Json(new { failure = true, message = "No standard field was found to delete!" });
            }


        }

        public string StandardColumns(string stdID)
        {
            string innerHTML = "<option value=''></option>";


            if (stdID.Trim() == "0")
            {
                return innerHTML;
            }
            List<SelectListItem> Result = new List<SelectListItem>();
            //StandardModel model = new StandardModel().SelectSingleStandard(Convert.ToInt32(stdID));
            AutomationStandardsContext db = new AutomationStandardsContext();
            Standard model = db.Standard.Where(x => x.StandardID == (Convert.ToInt32(stdID))).FirstOrDefault();

            StandardSQLManagement sql = new StandardSQLManagement();

            DataTable dt = sql.GetStandardValues(model.DBTableName);

            List<string> data = (from dc in dt.Columns.Cast<DataColumn>()

                                 select dc.ColumnName
                                 ).ToList();

            //return Json(data, JsonRequestBehavior.AllowGet);

            if (data.Count() > 0)
            {
                foreach (var item in data)
                {
                    innerHTML += string.Format("<option value='{0}'>{0}</option>", item);
                }
            }

            return innerHTML;

        }


        [HttpPost]
        public bool UpdateStandardConfigSortOrder(Dictionary<string, string> data)
        {
            bool Result = true;

            StandardConfigModel scm = new StandardConfigModel();

            bool wsResponse;
            foreach (var item in data)
            {
                int ID = Convert.ToInt32(item.Key.Replace("RowID-", ""));
                int RowID = Convert.ToInt32(item.Value);

                wsResponse = scm.SetStandardConfigSortOrder(ID, RowID);
                if (wsResponse == false)
                {
                    Result = false;
                }
            }

            return Result;
        }
        #endregion



        //#region Category
        //public ActionResult Category()
        //{
        //    return View();
        //}

        //public PartialViewResult CategoryGrid()
        //{
        //    CategoryModel c = new CategoryModel();
        //    var result = c.GetAllCategories();

        //    return PartialView(result);
        //}

        //public PartialViewResult CategoryEditor(string id)
        //{
        //    CategoryModel c = new CategoryModel();
        //    CategoryModel result = new CategoryModel();

        //    if (id != "0" && id != null)
        //    {
        //        result = c.GetSingleCategory(Convert.ToInt32(id));
        //    }
        //    else
        //    {
        //        result.CategoryID = 0;
        //    }
        //    return PartialView(result);
        //}


        //[HttpPost] //Update & Insert
        //public ActionResult ProcessCategoryModel(CategoryModel model)
        //{
        //    CategoryModel L = new CategoryModel();

        //    if (model.CategoryID == 0)
        //    {
        //        L.InsertCategory(model);
        //    }
        //    else
        //    {
        //        L.UpdateCategory(model);
        //    }

        //    return Json(new { result = true });
        //}

        //[HttpPost]  //Delete
        //public ActionResult DeleteCategoryModel(string id)
        //{
        //    CategoryModel L = new CategoryModel();

        //    L.DeleteCategory(Convert.ToInt32(id));

        //    return Json(new { result = true });
        //}

        //[HttpPost]
        //public bool UpdateCategorySortOrder(Dictionary<string, string> data)
        //{
        //    bool Result = true;

        //    CategoryModel cm = new CategoryModel();

        //    bool wsResponse;
        //    foreach (var item in data)
        //    {
        //        int ID = Convert.ToInt32(item.Key.Replace("RowID-", ""));
        //        int RowID = Convert.ToInt32(item.Value);

        //        wsResponse = cm.SetCategorySortOrder(ID, RowID);
        //        if (wsResponse == false)
        //        {
        //            Result = false;
        //        }
        //    }

        //    return Result;
        //}

        //public JsonResult GetIconDDL()
        //{
        //    List<SelectListItem> Result = new List<SelectListItem>();
        //    string currentdir = Server.MapPath("..");
        //    string oneuidir = currentdir + "\\assets\\css\\oneui.css";
        //    string bootstrapdir = currentdir + "\\assets\\css\\bootstrap.min.css";

        //    string oneuitext = System.IO.File.ReadAllText(oneuidir);
        //    string bootstraptext = System.IO.File.ReadAllText(bootstrapdir);


        //    string reg = "fa-.*:before";
        //    Regex r = new Regex(reg);
        //    MatchCollection matches = r.Matches(oneuitext);

        //    string sireg = "si-.*:before";
        //    Regex sir = new Regex(sireg);
        //    MatchCollection simatches = sir.Matches(oneuitext);

        //    List<string> glymatches = new List<string>();
        //    string[] splitbootstraptext = bootstraptext.Split('{');
        //    string glyreg = "glyphicon-.*:before";

        //    foreach (string line in splitbootstraptext)
        //    {
        //        Match GlyphMatch = Regex.Match(line, glyreg);
        //        glymatches.Add(GlyphMatch.Value.ToString());
        //    }
        //    glymatches = glymatches.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

        //    List<SelectListItem> sli = new List<SelectListItem>();
        //    foreach (Match m in matches)
        //    {

        //        SelectListItem sl = new SelectListItem();
        //        sl.Value = m.Value.Replace(":before", "");
        //        sl.Text = m.Value.Replace(":before", "");
        //        sli.Add(sl);
        //    }
        //    foreach (Match m in simatches)
        //    {
        //        SelectListItem sl = new SelectListItem();
        //        sl.Value = m.Value.Replace(":before", "");
        //        sl.Text = m.Value.Replace(":before", "");
        //        sli.Add(sl);
        //    }
        //    foreach (string m in glymatches)
        //    {
        //        SelectListItem sl = new SelectListItem();
        //        sl.Value = m.Replace(":before", "");
        //        sl.Text = m.Replace(":before", "");
        //        sli.Add(sl);
        //    }

        //    JsonResult returnData = Json(sli.ToList(), JsonRequestBehavior.AllowGet);
        //    //JsonResult returnData = Json(currentdir, JsonRequestBehavior.AllowGet);
        //    return returnData;
        //}



        //#endregion

        //#region SubCategory
        //public ActionResult SubCategory()
        //{
        //    return View();
        //}

        //public PartialViewResult SubCategoryGrid()
        //{
        //    SubCategoryModel c = new SubCategoryModel();
        //    var result = c.GetAllSubCategories();

        //    return PartialView(result);
        //}

        //public PartialViewResult SubCategoryEditor(string id)
        //{
        //    SubCategoryModel c = new SubCategoryModel();
        //    SubCategoryModel result = new SubCategoryModel();

        //    if (id != "0" && id != null)
        //    {
        //        result = c.GetSingleSubCategory(Convert.ToInt32(id));
        //    }
        //    else
        //    {
        //        result.SubCategoryID = 0;
        //    }
        //    return PartialView(result);
        //}


        //[HttpPost] //Update & Insert
        //public ActionResult ProcessSubCategoryModel(SubCategoryModel model)
        //{
        //    SubCategoryModel L = new SubCategoryModel();

        //    if (model.SubCategoryID == 0)
        //    {
        //        L.InsertSubCategory(model);
        //    }
        //    else
        //    {
        //        L.UpdateSubCategory(model);
        //    }

        //    return Json(new { result = true });
        //}

        //[HttpPost]  //Delete
        //public ActionResult DeleteSubCategoryModel(string id)
        //{
        //    SubCategoryModel L = new SubCategoryModel();

        //    L.DeleteSubCategory(Convert.ToInt32(id));

        //    return Json(new { result = true });
        //}

        //[HttpPost]
        //public bool UpdateSubCategorySortOrder(Dictionary<string, string> data)
        //{
        //    bool Result = true;

        //    SubCategoryModel cm = new SubCategoryModel();

        //    bool wsResponse;
        //    foreach (var item in data)
        //    {
        //        int ID = Convert.ToInt32(item.Key.Replace("RowID-", ""));
        //        int RowID = Convert.ToInt32(item.Value);

        //        wsResponse = cm.SetSubCategorySortOrder(ID, RowID);
        //        if (wsResponse == false)
        //        {
        //            Result = false;
        //        }
        //    }

        //    return Result;
        //}

        //#endregion

        //#region RequestType
        //public ActionResult RequestType()
        //{
        //    return View();
        //}

        //public PartialViewResult RequestTypeGrid()
        //{
        //    RequestTypeModel c = new RequestTypeModel();
        //    var result = c.SelectRequestTypes();

        //    return PartialView(result);
        //}

        //public PartialViewResult RequestTypeEditor(string id)
        //{
        //    RequestTypeModel c = new RequestTypeModel();
        //    RequestTypeModel result = new RequestTypeModel();

        //    if (id != "0" && id != null)
        //    {
        //        result = c.SelectSingleRequestType(Convert.ToInt32(id));
        //    }
        //    else
        //    {
        //        result.RequestTypeID = 0;
        //    }
        //    return PartialView(result);
        //}


        //[HttpPost] //Update & Insert
        //public ActionResult ProcessRequestTypeModel(RequestTypeModel model)
        //{
        //    RequestTypeModel L = new RequestTypeModel();

        //    if (model.RequestTypeID == 0)
        //    {
        //        L.InsertRequestType(model);
        //    }
        //    else
        //    {
        //        L.UpdateRequestType(model);
        //    }

        //    return Json(new { result = true });
        //}

        //[HttpPost]  //Delete
        //public ActionResult DeleteRequestTypeModel(string id)
        //{
        //    RequestTypeModel L = new RequestTypeModel();

        //    L.DeleteRequestType(Convert.ToInt32(id));

        //    return Json(new { result = true });
        //}


        //#endregion

        //#region Category Config
        //public PartialViewResult CategoryConfigGrid(string id)
        //{
        //    int CategoryID = Convert.ToInt32(id);

        //    CategoryConfigModel L = new CategoryConfigModel();
        //    List<CategoryConfigModel> model = L.GetAllCategoryConfigs(CategoryID);

        //    CategoryModel M = new CategoryModel().GetSingleCategory(CategoryID);
        //    ViewBag.CategoryID = CategoryID;
        //    ViewBag.CategoryName = M.CategoryName;



        //    return PartialView(model);
        //}
        ////Select
        //public PartialViewResult CategoryConfigEditor(string id, string CategoryID)
        //{
        //    CategoryConfigModel L = new CategoryConfigModel();
        //    CategoryConfigModel model = new CategoryConfigModel();

        //    if (id != "0" && id != null)
        //    {
        //        model = L.GetSingleCategoryConfig(Convert.ToInt32(id));
        //    }
        //    else
        //    {
        //        model.CategoryConfigID = 0;
        //        model.CategoryID = Convert.ToInt32(CategoryID);
        //    }
        //    return PartialView(model);
        //}

        //[HttpPost] //Update & Insert
        //public ActionResult ProcessCategoryConfigModel(CategoryConfigModel model)
        //{
        //    CategoryConfigModel L = new CategoryConfigModel();

        //    if (model.CategoryConfigID == 0)
        //    {
        //        L.InsertCategoryConfig(model);
        //    }
        //    else
        //    {
        //        L.UpdateCategoryConfig(model);
        //    }

        //    return Json(new { result = true });
        //}

        //[HttpPost]  //Delete
        //public ActionResult DeleteCategoryConfigModel(string id)
        //{
        //    CategoryConfigModel L = new CategoryConfigModel();

        //    L.DeleteCategoryConfig(Convert.ToInt32(id));

        //    return Json(new { result = true });
        //}
        //#endregion

        //#region Notification Sections


        //public PartialViewResult NotificationGrid(string id)
        //{

        //    List<NotificationModel> r = new NotificationModel().GetNotificationsByCategoryID(Convert.ToInt32(id));
        //    CategoryModel c = new CategoryModel().GetSingleCategory(Convert.ToInt32(id));

        //    ViewBag.CategoryID = id;
        //    ViewBag.CategoryName = c.CategoryName;

        //    return PartialView("NotificationGrid", r);
        //}


        //public PartialViewResult NotificationEditor(string id, string CategoryID, string FormMode)
        //{

        //    NotificationModel model = new NotificationModel();

        //    if (id != "" && id != "0")
        //    {
        //        model = model.GetNotificationDetails(Convert.ToInt32(id));
        //    }
        //    else
        //    {
        //        model.NotificationID = 0;
        //        model.CategoryID = Convert.ToInt32(CategoryID);
        //    }


        //    string ReturnView = "NotificationViewer";

        //    if (FormMode == "Edit")
        //    {
        //        ReturnView = "NotificationEditor";
        //    }

        //    return PartialView(ReturnView, model);
        //}


        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult ProcessNotificationModel(NotificationModel model, string Body)
        //{
        //    model.Body = Body;
        //    NotificationModel n = new NotificationModel();

        //    if (model.NotificationID == 0)
        //    {
        //        n.InsertNotification(model);
        //    }
        //    else
        //    {
        //        n.UpdateNotification(model);
        //    }

        //    return Json(new { result = model.NotificationID });
        //}
        //[HttpPost]
        //public ActionResult DeleteNotificationModel(string id)
        //{
        //    NotificationModel n = new NotificationModel();
        //    n.DeleteNotification(Convert.ToInt32(id));
        //    return Json(new { result = true });
        //}


        //public PartialViewResult NotificationDuplicate(int CatID)
        //{
        //    return PartialView(CatID);
        //}

        //[HttpPost]
        //public ActionResult ProcessDuplicateNotification(FormCollection fc)
        //{
        //    NotificationModel nm = new NotificationModel();
        //    int NotificationID = Convert.ToInt32(fc["NotificationID"].ToString());
        //    string NewName = fc["Name"].ToString();
        //    string NewSubject = fc["Subject"].ToString();

        //    bool Result = nm.DuplicateNotification(NotificationID, NewName, NewSubject);

        //    return Json(new { result = Result });
        //}

        //#endregion

        //#region Notification Template

        //public ActionResult NotificationTemplate()
        //{
        //    return View();
        //}
        //public ActionResult NotificationTemplateGrid()
        //{
        //    NotificationTemplateModel L = new NotificationTemplateModel();
        //    List<NotificationTemplateModel> model = L.SelectAllNotificationTemplates();
        //    //return PartialView(model);

        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}

        ////Select
        //public PartialViewResult NotificationTemplateEditor(string id, string FormMode)
        //{
        //    NotificationTemplateModel L = new NotificationTemplateModel();
        //    NotificationTemplateModel model = new NotificationTemplateModel();




        //    if (id != "0" && id != null)
        //    {
        //        model = L.SelectSingleNotificationTemplates(Convert.ToInt32(id));
        //    }
        //    else
        //    {
        //        model.NotificationTemplateID = 0;
        //    }

        //    string ReturnView = "NotificationTemplateViewer";

        //    if (FormMode == "Edit")
        //    {
        //        ReturnView = "NotificationTemplateEditor";
        //    }

        //    return PartialView(ReturnView, model);
        //}

        //[HttpPost] //Update & Insert
        //[ValidateInput(false)]
        //public ActionResult ProcessNotificationTemplateModel(NotificationTemplateModel model)
        //{
        //    NotificationTemplateModel L = new NotificationTemplateModel();

        //    if (model.NotificationTemplateID == 0)
        //    {
        //        L.InsertNotificationTemplate(model);
        //    }
        //    else
        //    {
        //        L.UpdateNotificationTemplate(model);
        //    }

        //    return Json(new { result = model.NotificationTemplateID });
        //}

        //[HttpPost]  //Delete
        //public ActionResult DeleteNotificationTemplateModel(string id)
        //{
        //    NotificationTemplateModel L = new NotificationTemplateModel();

        //    L.DeleteNotificationTemplate(Convert.ToInt32(id));

        //    return Json(new { result = true });
        //}


        //public ActionResult PreviewNotification(string id, string CatName, string NotName)
        //{
        //    WebService.OneClick ws = new WebService.OneClick();
        //    WebService.NotificationResult data = ws.BuildNotification(Convert.ToInt32(id), CatName, NotName);
        //    return Json(new { result = data });
        //}

        //#endregion

        //#region FormCollection
        //public ActionResult FormCollection()
        //{
        //    return View();
        //}

        //public PartialViewResult FormCollectionGrid()
        //{
        //    FormCollectionModel c = new FormCollectionModel();
        //    var result = c.GetAllFormCollections();

        //    return PartialView(result);
        //}

        //public PartialViewResult FormCollectionEditor(string id)
        //{
        //    FormCollectionModel c = new FormCollectionModel();
        //    FormCollectionModel result = new FormCollectionModel();

        //    if (id != "0" && id != null)
        //    {
        //        result = c.GetSingleFormColletion(Convert.ToInt32(id));
        //    }
        //    else
        //    {
        //        result.FormCollectionID = 0;
        //    }
        //    return PartialView(result);
        //}


        //[HttpPost] //Update & Insert
        //public ActionResult ProcessFormCollectionModel(FormCollectionModel model)
        //{
        //    FormCollectionModel L = new FormCollectionModel();

        //    model.WFRole = string.Join(",", model.WFRoleArray);
        //    if (model.DefaultReportFieldsArray == null)
        //    {
        //        model.DefaultReportFields = null;
        //    }
        //    else
        //    {
        //        model.DefaultReportFields = string.Join(",", model.DefaultReportFieldsArray);
        //    }
        //    if (model.FormCollectionID == 0)
        //    {
        //        L.InsertFormCollection(model);
        //    }
        //    else
        //    {
        //        L.UpdateFormCollection(model);
        //    }

        //    return Json(new { result = true });
        //}

        //[HttpPost]  //Delete
        //public ActionResult DeleteFormCollectionModel(string id)
        //{
        //    FormCollectionModel L = new FormCollectionModel();

        //    L.DeleteFormCollection(Convert.ToInt32(id));

        //    return Json(new { result = true });
        //}

        //public string GetCategoryDDL(string Selected)
        //{

        //    IEnumerable<SelectListItem> data = new CategoryModel().DDLCategories();

        //    string innerHTML = "<option value=\"\">Please Select an option</option>";
        //    foreach (SelectListItem o in data)
        //    {
        //        if (Selected != null)
        //        {
        //            if (o.Value == Selected)
        //            {
        //                innerHTML += "<option value = \"" + o.Value + "\" selected=\"selected\">" + o.Text + "</option>";
        //            }
        //            else
        //            {
        //                innerHTML += "<option value = \"" + o.Value + "\" >" + o.Text + "</option>";
        //            }
        //        }
        //        else //no option is selected
        //        {
        //            innerHTML += "<option value = \"" + o.Value + "\" >" + o.Text + "</option>";
        //        }

        //    }

        //    return innerHTML;

        //}

        //public string GetSubCategoryDDL(string Selected)
        //{

        //    IEnumerable<SelectListItem> data = new SubCategoryModel().DDLSubCategories();
        //    string innerHTML = "<option value=\"\">Please Select an option</option>";
        //    foreach (SelectListItem o in data)
        //    {
        //        if (Selected != null)
        //        {
        //            if (o.Value == Selected)
        //            {
        //                innerHTML += "<option value = \"" + o.Value + "\" selected=\"selected\">" + o.Text + "</option>";
        //            }
        //            else
        //            {
        //                innerHTML += "<option value = \"" + o.Value + "\" >" + o.Text + "</option>";
        //            }
        //        }
        //        else //no option is selected
        //        {
        //            innerHTML += "<option value = \"" + o.Value + "\" >" + o.Text + "</option>";
        //        }

        //    }

        //    return innerHTML;

        //}

        //public string GetFormCollectionImageTable()
        //{
        //    string iconurl = "https://util.obs.org/sharedimages/icons/";

        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(iconurl);
        //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //    {
        //        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        //        {
        //            string html = reader.ReadToEnd();

        //            Regex regex = new Regex("A HREF=\".*\">(.*)</A>");
        //            //for some reason regex.matches is only finding one match for all results.
        //            //MatchCollection matches = regex.Matches(html);

        //            string HTMLResult = "";
        //            HTMLResult += "<div class=\"block-content\"><div class=\"table-responsive\"><table class=\"table table-bordered table-striped js-dataTable-full dataTable\" id=\"image_datatable\"><thead><tr>";
        //            //HTMLResult += "<th width=\"150px;\">Image</th>";
        //            //HTMLResult += "<th width =\"150px;\">Button</th>";
        //            HTMLResult += "</tr></thead>";
        //            HTMLResult += "<tbody>";

        //            Match m = regex.Match(html);
        //            var splitres = m.Value.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
        //            int imagecount = splitres.Count();
        //            int counter = 0;
        //            int rowlength = 11;
        //            HTMLResult += "<tr>";
        //            foreach (var item in splitres)
        //            {
        //                if (counter == rowlength)
        //                {
        //                    HTMLResult += "</tr><tr>";
        //                }

        //                string s = item.Substring(item.LastIndexOf("\">"));
        //                s = s.Replace("\">", "").Replace("</A>", "");
        //                if (s != "[To Parent Directory]" && s != ".DS_Store" && s != "web.config" && s != "priority")
        //                {
        //                    string imageurl = iconurl + s;
        //                    HTMLResult += "<td align=\"center\"><a href=\"javascript:SelectImageURL('" + imageurl + "');\" id=\"FCImages\" ><img src=\"" + imageurl + "\" height=50 width=50\" /></a></td>";
        //                    if (counter == rowlength)
        //                    {
        //                        counter = 1;
        //                    }
        //                    else
        //                    {
        //                        counter++;
        //                    }
        //                }



        //            }
        //            HTMLResult += "</tr></tbody></table></div></div>";

        //            return HTMLResult;
        //        }
        //    }
        //}
        //#endregion

        //#region Form Collection Set
        //public PartialViewResult FormCollectionSetGrid(string id)
        //{
        //    int FormCollectionID = Convert.ToInt32(id);

        //    FormCollectionSetModel L = new FormCollectionSetModel();
        //    List<FormCollectionSetModel> model = L.GetAllFormCollectionSets(FormCollectionID);

        //    FormCollectionModel M = new FormCollectionModel().GetSingleFormColletion(FormCollectionID);
        //    ViewBag.FormCollectionID = FormCollectionID;
        //    ViewBag.FormCollectionName = M.Name;

        //    return PartialView(model);
        //}
        ////Select
        //public PartialViewResult FormCollectionSetEditor(string id, string FormCollectionID)
        //{
        //    FormCollectionSetModel L = new FormCollectionSetModel();
        //    FormCollectionSetModel model = new FormCollectionSetModel();

        //    if (id != "0" && id != null)
        //    {
        //        model = L.GetSingleFormColletionSet(Convert.ToInt32(id));
        //    }
        //    else
        //    {
        //        model.FormCollectionSetID = 0;
        //        model.FormCollectionID = Convert.ToInt32(FormCollectionID);
        //    }
        //    return PartialView(model);
        //}

        //[HttpPost] //Update & Insert
        //public ActionResult ProcessFormCollectionSetModel(FormCollectionSetModel model)
        //{
        //    FormCollectionSetModel L = new FormCollectionSetModel();

        //    if (model.FormCollectionSetID == 0)
        //    {
        //        L.InsertFormCollectionSet(model);
        //    }
        //    else
        //    {
        //        L.UpdateFormCollectionSet(model);
        //    }

        //    return Json(new { result = true });
        //}

        //[HttpPost]  //Delete
        //public ActionResult DeleteFormCollectionSetModel(string id)
        //{
        //    FormCollectionSetModel L = new FormCollectionSetModel();

        //    L.DeleteFormCollectionSet(Convert.ToInt32(id));

        //    return Json(new { result = true });
        //}

        //[HttpPost]
        //public bool UpdateFormCollectionSetSortOrder(Dictionary<string, string> data)
        //{
        //    bool Result = true;

        //    FormCollectionSetModel fcm = new FormCollectionSetModel();

        //    bool wsResponse;
        //    foreach (var item in data)
        //    {
        //        int ID = Convert.ToInt32(item.Key.Replace("RowID-", ""));
        //        int RowID = Convert.ToInt32(item.Value);

        //        wsResponse = fcm.SetFormCollectionSetSortOrder(ID, RowID);
        //        if (wsResponse == false)
        //        {
        //            Result = false;
        //        }
        //    }

        //    return Result;
        //}



        //#endregion

        //#region Form
        //public ActionResult Form()
        //{
        //    return View();
        //}

        //public PartialViewResult FormGrid()
        //{
        //    FormModel L = new FormModel();
        //    List<FormModel> model = L.SelectAllForms().OrderBy(f => f.Name).ToList();
        //    return PartialView(model);
        //}

        ////Select
        //public PartialViewResult FormEditor(string id)
        //{
        //    FormModel L = new FormModel();
        //    FormModel model = new FormModel();




        //    if (id != "0" && id != null)
        //    {
        //        model = L.SelectSingleForm(Convert.ToInt32(id));

        //    }
        //    else
        //    {
        //        model.FormID = 0;
        //    }


        //    return PartialView(model);
        //}

        //[HttpPost] //Update & Insert
        //public ActionResult ProcessFormModel(FormModel model)
        //{

        //    FormModel L = new FormModel();
        //    model.WFRole = string.Join(",", model.WFRoleArray);
        //    if (model.FormID == 0)
        //    {
        //        L.InsertForm(model);
        //    }
        //    else
        //    {
        //        L.UpdateForm(model);
        //    }

        //    return Json(new { result = true });
        //}

        //[HttpPost]  //Delete
        //public ActionResult DeleteFormModel(string id)
        //{
        //    FormModel L = new FormModel();


        //    L.DeleteForm(Convert.ToInt32(id));
        //    return Json(new { result = true });



        //}

        //public PartialViewResult FormDuplicate()
        //{
        //    return PartialView();
        //}

        //[HttpPost]
        //public ActionResult ProcessDuplicateForm(FormCollection fc)
        //{
        //    FormModel fm = new FormModel();
        //    int FormID = Convert.ToInt32(fc["FormID"].ToString());
        //    string NewName = fc["Name"].ToString();
        //    string NewDescription = fc["Description"].ToString();

        //    bool Result = fm.DuplicateForm(FormID, NewName, NewDescription);

        //    return Json(new { result = Result });
        //}


        //#endregion

        //#region FormClone Section

        ////Select
        //public PartialViewResult FormClone()
        //{
        //    FormModel L = new FormModel();
        //    List<FormModel> forms = L.SelectAllForms().ToList();

        //    FormClone model = new FormClone();
        //    model.FormListing = new List<SelectListItem>();

        //    foreach (var f in forms)
        //    {
        //        model.FormListing.Add(new SelectListItem() { Value = f.FormID.ToString(), Text = f.Name });
        //    }

        //    return PartialView(model);
        //}

        //[HttpPost]
        //public ActionResult ProcessFormClone(FormClone model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        FormClone f = new FormClone();
        //        f.ExecuteFormClone(model);
        //    }
        //    return Json(new { result = true });
        //}

        //#endregion

        //#region FormField

        //public PartialViewResult FormFieldGrid(string id)
        //{

        //    int ID = Convert.ToInt32(id);
        //    FormFieldModel L = new FormFieldModel();
        //    List<FormFieldModel> model = L.SelectFormFields(ID);

        //    ViewBag.FormName = new FormModel().SelectFormName(ID);
        //    ViewBag.FormID = id;
        //    return PartialView(model);
        //}

        ////Select
        //public PartialViewResult FormFieldEditor(string id, string FormID, string FieldID)
        //{
        //    FormFieldModel L = new FormFieldModel();
        //    FormFieldModel model = new FormFieldModel();



        //    if (id != "0" && id != null)
        //    {
        //        model = L.SelectSingleFormField(Convert.ToInt32(id));

        //    }
        //    else
        //    {
        //        if (FieldID == null || FieldID == "")
        //        {
        //            FieldID = "0";
        //        }
        //        model.FormFieldID = 0;
        //        model.FormID = Convert.ToInt32(FormID);
        //        model.FieldID = Convert.ToInt32(FieldID);
        //    }

        //    return PartialView(model);
        //}

        //[HttpPost] //Update & Insert
        //[ValidateInput(false)]
        //public ActionResult ProcessFormFieldModel(FormFieldModel model)
        //{

        //    FormFieldModel L = new FormFieldModel();

        //    if (model.FormFieldID == 0)
        //    {
        //        L.InsertFormField(model);
        //    }
        //    else
        //    {
        //        L.UpdateFormField(model);
        //    }

        //    return Json(new { result = true });
        //}

        //[HttpPost]  //Delete
        //public ActionResult DeleteFormFieldModel(string id)
        //{
        //    FormFieldModel L = new FormFieldModel();

        //    L.DeleteFormField(Convert.ToInt32(id));

        //    return Json(new { result = true });
        //}

        //[HttpPost]
        //public bool UpdateFormFieldSortOrder(Dictionary<string, string> data)
        //{
        //    bool Result = true;

        //    FormFieldModel ffm = new FormFieldModel();

        //    bool wsResponse;
        //    foreach (var item in data)
        //    {
        //        int ID = Convert.ToInt32(item.Key.Replace("RowID-", ""));
        //        int RowID = Convert.ToInt32(item.Value);

        //        wsResponse = ffm.SetFormFieldSortOrder(ID, RowID);
        //        if (wsResponse == false)
        //        {
        //            Result = false;
        //        }
        //    }

        //    return Result;
        //}

        //public string StandardColumns(string stdID)
        //{
        //    string innerHTML = "<option value=''></option>";


        //    if (stdID.Trim() == "0")
        //    {
        //        return innerHTML;
        //    }
        //    List<SelectListItem> Result = new List<SelectListItem>();
        //    StandardModel model = new StandardModel().SelectSingleStandard(Convert.ToInt32(stdID));


        //    StandardSQLManagement sql = new StandardSQLManagement();

        //    DataTable dt = sql.GetStandardValues(model.DBTableName);

        //    List<string> data = (from dc in dt.Columns.Cast<DataColumn>()

        //                         select dc.ColumnName
        //                         ).ToList();

        //    //return Json(data, JsonRequestBehavior.AllowGet);

        //    if (data.Count() > 0)
        //    {
        //        foreach (var item in data)
        //        {
        //            innerHTML += string.Format("<option value='{0}'>{0}</option>", item);
        //        }
        //    }

        //    return innerHTML;

        //}


        //public JsonResult GetFieldDetails(string FieldID)
        //{
        //    FieldModel model = new FieldModel();

        //    var data = model.SelectSingleField(Convert.ToInt32(FieldID));

        //    JsonResult returnData = Json(data, JsonRequestBehavior.AllowGet);


        //    return returnData;

        //}

        //[HttpPost]
        //public string GetChildFieldDDL(string FormID, string selected)
        //{
        //    string HTMLResult = "";
        //    if (FormID.ToLower() != "please select an option")
        //    {
        //        List<System.Web.Mvc.SelectListItem> cfname = new FormFieldModel().SelectDDLFormField(Convert.ToInt32(FormID));
        //        List<string> dbValue = new List<string>();

        //        if (selected != null && selected != "")
        //        {
        //            if (selected.Contains(","))
        //            {
        //                dbValue = selected.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //            }
        //            else
        //            {
        //                dbValue.Add(selected);
        //            }
        //        }

        //        foreach (SelectListItem item in cfname)
        //        {
        //            if (dbValue.Contains(item.Text))
        //            {
        //                HTMLResult += "<option value=\"" + item.Text + "\" selected=\"selected\">" + item.Text + "</option>";
        //            }
        //            else
        //            {
        //                HTMLResult += "<option value=\"" + item.Text + "\">" + item.Text + "</option>";
        //            }
        //        }
        //    }
        //    return HTMLResult;
        //}

        //#endregion

        ////#region   LookUpTypes

        ////public ActionResult LookUpType()
        ////{
        ////    return View();
        ////}


        ////public PartialViewResult LookUpTypeGrid()
        ////{
        ////    LookUpTypeModel L = new LookUpTypeModel();
        ////    List<LookUpTypeModel> model = L.SelectLookUpTypes();
        ////    return PartialView(model);
        ////}

        //////Select
        ////public PartialViewResult LookUpTypeEditor(string id)
        ////{
        ////    LookUpTypeModel L = new LookUpTypeModel();
        ////    LookUpTypeModel model = new LookUpTypeModel();

        ////    if (id != "0" && id != null)
        ////    {
        ////        model = L.SelectSingleLookUpType(Convert.ToInt32(id));
        ////    }
        ////    else
        ////    {
        ////        model.LookUpTypeID = 0;
        ////    }
        ////    return PartialView(model);
        ////}

        ////[HttpPost] //Update & Insert
        ////public ActionResult ProcessLookUpTypeModel(LookUpTypeModel model)
        ////{
        ////    LookUpTypeModel L = new LookUpTypeModel();

        ////    if (model.LookUpTypeID == 0)
        ////    {
        ////        L.InsertLookUpType(model);
        ////    }
        ////    else
        ////    {
        ////        L.UpdateLookUpType(model);
        ////    }

        ////    return Json(new { result = true });
        ////}

        ////[HttpPost]  //Delete
        ////public ActionResult DeleteLookUpTypeModel(string id)
        ////{
        ////    LookUpTypeModel L = new LookUpTypeModel();

        ////    L.DeleteLookUpType(Convert.ToInt32(id));

        ////    return Json(new { result = true });
        ////}
        ////#endregion

        ////#region LookUps
        ////public PartialViewResult LookUpGrid(string id)
        ////{
        ////    int LookUpTypeID = Convert.ToInt32(id);

        ////    LookUpModel L = new LookUpModel();
        ////    List<LookUpModel> model = L.SelectLookUps(LookUpTypeID);

        ////    LookUpTypeModel M = new LookUpTypeModel().SelectSingleLookUpType(LookUpTypeID);
        ////    ViewBag.LookUpTypeID = LookUpTypeID;
        ////    ViewBag.LookUpTypeName = M.LookUpTypeName;



        ////    return PartialView(model);
        ////}
        //////Select
        ////public PartialViewResult LookUpEditor(string id, string LookUpTypeID)
        ////{
        ////    LookUpModel L = new LookUpModel();
        ////    LookUpModel model = new LookUpModel();

        ////    if (id != "0" && id != null)
        ////    {
        ////        model = L.SelectSingleLookUp(Convert.ToInt32(id));
        ////    }
        ////    else
        ////    {
        ////        model.LookUpID = 0;
        ////        model.LookUpTypeID = Convert.ToInt32(LookUpTypeID);
        ////    }
        ////    return PartialView(model);
        ////}

        ////[HttpPost] //Update & Insert
        ////public ActionResult ProcessLookUpModel(LookUpModel model)
        ////{
        ////    LookUpModel L = new LookUpModel();

        ////    if (model.LookUpID == 0)
        ////    {
        ////        L.InsertLookUp(model);
        ////    }
        ////    else
        ////    {
        ////        L.UpdateLookUp(model);
        ////    }

        ////    return Json(new { result = true });
        ////}

        ////[HttpPost]  //Delete
        ////public ActionResult DeleteLookUpModel(string id)
        ////{
        ////    LookUpModel L = new LookUpModel();

        ////    L.DeleteLookUp(Convert.ToInt32(id));

        ////    return Json(new { result = true });
        ////}
        ////#endregion

        //#region   FieldTypes

        //public ActionResult FieldType()
        //{
        //    return View();
        //}


        //public PartialViewResult FieldTypeGrid()
        //{
        //    FieldTypeModel L = new FieldTypeModel();
        //    List<FieldTypeModel> model = L.SelectFieldTypes();
        //    return PartialView(model);
        //}

        ////Select
        //public PartialViewResult FieldTypeEditor(string id)
        //{
        //    FieldTypeModel L = new FieldTypeModel();
        //    FieldTypeModel model = new FieldTypeModel();

        //    if (id != "0" && id != null)
        //    {
        //        model = L.SelectSingleFieldType(Convert.ToInt32(id));
        //    }
        //    else
        //    {
        //        model.FieldTypeID = 0;
        //    }
        //    return PartialView(model);
        //}

        //[HttpPost] //Update & Insert
        //public ActionResult ProcessFieldTypeModel(FieldTypeModel model)
        //{
        //    FieldTypeModel L = new FieldTypeModel();

        //    if (model.FieldTypeID == 0)
        //    {
        //        L.InsertFieldType(model);
        //    }
        //    else
        //    {
        //        L.UpdateFieldType(model);
        //    }

        //    return Json(new { result = true });
        //}

        //[HttpPost]  //Delete
        //public ActionResult DeleteFieldTypeModel(string id)
        //{
        //    FieldTypeModel L = new FieldTypeModel();

        //    L.DeleteFieldType(Convert.ToInt32(id));

        //    return Json(new { result = true });
        //}
        //#endregion

        //#region Field

        //public ActionResult Field()
        //{
        //    return View();
        //}
        //public PartialViewResult FieldGrid()
        //{
        //    FieldModel L = new FieldModel();
        //    List<FieldModel> model = L.SelectFields();
        //    return PartialView(model);
        //}

        ////Select
        //public PartialViewResult FieldEditor(string id)
        //{
        //    FieldModel L = new FieldModel();
        //    FieldModel model = new FieldModel();




        //    if (id != "0" && id != null)
        //    {
        //        model = L.SelectSingleField(Convert.ToInt32(id));
        //    }
        //    else
        //    {
        //        model.FieldTypeID = 0;
        //    }
        //    return PartialView(model);
        //}

        //[HttpPost] //Update & Insert
        //public ActionResult ProcessFieldModel(FieldModel model)
        //{
        //    FieldModel L = new FieldModel();
        //    int FieldID = 0;
        //    if (model.FieldID == 0)
        //    {
        //        FieldID = L.InsertField(model);
        //    }
        //    else
        //    {
        //        FieldID = model.FieldID;
        //        L.UpdateField(model);
        //    }
        //    //var result = new { Result = FieldID}
        //    return Json(new { result = FieldID });
        //}

        //[HttpPost]  //Delete
        //public ActionResult DeleteFieldModel(string id)
        //{
        //    FieldModel L = new FieldModel();

        //    L.DeleteField(Convert.ToInt32(id));

        //    return Json(new { result = true });
        //}

        //public string GetFieldUsage(int FieldID)
        //{
        //    FieldUsageModel fum = new FieldUsageModel();
        //    List<FieldUsageModel> FieldInstances = fum.ShowFieldUsage(FieldID);

        //    string innerHTML = "<div class='text-left'>";
        //    innerHTML += "<p>This field is used in the following Forms/Collections:</p>";
        //    innerHTML += "<table class=\"table table-bordered table-striped js-dataTable-full dataTable\" id=\"fieldusage_datatable\"><thead><tr>";
        //    innerHTML += "<th>Form Name</th><th>Form Collection Name</th></tr ></thead><tbody>";
        //    foreach (FieldUsageModel Instance in FieldInstances)
        //    {
        //        innerHTML += "<tr><td class=\"font-w600\">" + Instance.FormName + "</td>";
        //        innerHTML += "<td class=\"font-w600\">" + Instance.FormCollectionName + "</td></tr>";
        //    }
        //    innerHTML += "</tbody>";

        //    return innerHTML;

        //}

        //#endregion


        //#region FormType

        //public ActionResult FormType()
        //{
        //    return View();
        //}
        //public PartialViewResult FormTypeGrid()
        //{
        //    FormTypeModel L = new FormTypeModel();
        //    List<FormTypeModel> model = L.SelectAllFormTypes();
        //    return PartialView(model);
        //}

        ////Select
        //public PartialViewResult FormTypeEditor(string id)
        //{
        //    FormTypeModel L = new FormTypeModel();
        //    FormTypeModel model = new FormTypeModel();




        //    if (id != "0" && id != null)
        //    {
        //        model = L.SelectSingleFormTypes(Convert.ToInt32(id));
        //    }
        //    else
        //    {
        //        model.FormTypeID = 0;
        //    }
        //    return PartialView(model);
        //}

        //[HttpPost] //Update & Insert
        //public ActionResult ProcessFormTypeModel(FormTypeModel model)
        //{
        //    FormTypeModel L = new FormTypeModel();

        //    if (model.FormTypeID == 0)
        //    {
        //        L.InsertFormType(model);
        //    }
        //    else
        //    {
        //        L.UpdateFormType(model);
        //    }

        //    return Json(new { result = true });
        //}

        //[HttpPost]  //Delete
        //public ActionResult DeleteFormTypeModel(string id)
        //{
        //    FormTypeModel L = new FormTypeModel();

        //    L.DeleteFormType(Convert.ToInt32(id));

        //    return Json(new { result = true });
        //}
        //#endregion


        //#region TaskType

        //public ActionResult TaskType()
        //{
        //    return View();
        //}
        //public PartialViewResult TaskTypeGrid()
        //{
        //    TaskTypeModel L = new TaskTypeModel();
        //    List<TaskTypeModel> model = L.SelectAllTaskTypes();
        //    return PartialView(model);
        //}

        ////Select
        //public PartialViewResult TaskTypeEditor(string id)
        //{
        //    TaskTypeModel L = new TaskTypeModel();
        //    TaskTypeModel model = new TaskTypeModel();




        //    if (id != "0" && id != null)
        //    {
        //        model = L.SelectSingleTaskTypes(Convert.ToInt32(id));
        //    }
        //    else
        //    {
        //        model.TaskTypeID = 0;
        //    }
        //    return PartialView(model);
        //}

        //[HttpPost] //Update & Insert
        //public ActionResult ProcessTaskTypeModel(TaskTypeModel model)
        //{
        //    TaskTypeModel L = new TaskTypeModel();

        //    if (model.TaskTypeID == 0)
        //    {
        //        L.InsertTaskType(model);
        //    }
        //    else
        //    {
        //        L.UpdateTaskType(model);
        //    }

        //    return Json(new { result = true });
        //}

        //[HttpPost]  //Delete
        //public ActionResult DeleteTaskTypeModel(string id)
        //{
        //    TaskTypeModel L = new TaskTypeModel();

        //    L.DeleteTaskType(Convert.ToInt32(id));

        //    return Json(new { result = true });
        //}
        //#endregion

        //#region Status

        //public ActionResult Status()
        //{
        //    return View();
        //}
        //public PartialViewResult StatusGrid()
        //{
        //    StatusModel L = new StatusModel();
        //    List<StatusModel> model = L.SelectAllStatuss();
        //    return PartialView(model);
        //}

        ////Select
        //public PartialViewResult StatusEditor(string id)
        //{
        //    StatusModel L = new StatusModel();
        //    StatusModel model = new StatusModel();




        //    if (id != "0" && id != null)
        //    {
        //        model = L.SelectSingleStatuss(Convert.ToInt32(id));
        //    }
        //    else
        //    {
        //        model.StatusID = 0;
        //    }
        //    return PartialView(model);
        //}

        //[HttpPost] //Update & Insert
        //public ActionResult ProcessStatusModel(StatusModel model)
        //{
        //    StatusModel L = new StatusModel();

        //    if (model.StatusID == 0)
        //    {
        //        L.InsertStatus(model);
        //    }
        //    else
        //    {
        //        L.UpdateStatus(model);
        //    }

        //    return Json(new { result = true });
        //}

        //[HttpPost]  //Delete
        //public ActionResult DeleteStatusModel(string id)
        //{
        //    StatusModel L = new StatusModel();

        //    L.DeleteStatus(Convert.ToInt32(id));

        //    return Json(new { result = true });
        //}
        //#endregion



        //#region Request

        //public ActionResult Request()
        //{
        //    return View();
        //}

        //public PartialViewResult RequestInitial()
        //{
        //    RequestHandler rh = new RequestHandler();
        //    var model = rh.GetManageInitialList();
        //    return PartialView("RequestGrid", model);
        //}

        //public PartialViewResult RequestGrid(string id)
        //{
        //    RequestHandler rh = new RequestHandler();

        //    List<RequestInfo> model = new List<RequestInfo>();

        //    try
        //    {
        //        var data = rh.GetSingleRequest(Convert.ToInt32(id));
        //        model.Add(data);
        //    }
        //    catch { }
        //    return PartialView(model);
        //}

        //public PartialViewResult RequestGridNew(string RequestID, string RequestType, string RequestBy)
        //{
        //    RequestHandler rh = new RequestHandler();

        //    List<RequestInfo> model = new List<RequestInfo>();

        //    try
        //    {
        //        model = rh.GetRequestToManage(RequestID, RequestType, RequestBy);
        //    }
        //    catch { }
        //    return PartialView("RequestGrid", model);
        //}

        ////Select
        //public PartialViewResult RequestEditor(string id)
        //{

        //    RequestHandler rh = new RequestHandler();
        //    var model = rh.GetRequestValuesByRequestID(Convert.ToInt32(id));
        //    return PartialView(model);

        //}


        //[HttpPost]  //Save Request Value
        //public ActionResult UpdateRequestValue(string id, int FormFieldID, string NewValue)
        //{
        //    RequestHandler rh = new RequestHandler();
        //    bool model = rh.UpdateRequestValue(Convert.ToInt32(id), FormFieldID, NewValue, User.Identity.Name);

        //    return Json(new { result = model });
        //}


        //[HttpPost]
        //public ActionResult RestartRequest(string id)
        //{

        //    bool init = RestartRequestByUserName(id, User.Identity.Name);

        //    return Json(new { result = init });

        //}

        //public bool RestartRequestByUserName(string id, string UserName)
        //{
        //    int rID = Convert.ToInt32(id);

        //    //Terminate Request
        //    CPORequestHandler cpo = new CPORequestHandler();
        //    bool term = cpo.TerminateRequest(rID);

        //    //New Workflow Log
        //    AutomationStandardsContext db = new AutomationStandardsContext();

        //    var OpenTasks = db.Tasks.Where(t => t.RequestID == rID && t.Status.StatusName != "Complete").ToList();
        //    foreach (var t in OpenTasks)
        //    {
        //        TaskHandler th = new TaskHandler();
        //        th.CloseTask(t.WFTaskID, UserName);
        //    }


        //    //Start the Request Again
        //    RequestController req = new RequestController();
        //    bool init = req.InitiateCPOAction(id);

        //    return init;

        //}



        //[HttpPost]
        //public ActionResult TerminateRequest(string id)
        //{
        //    RequestHandler rh = new RequestHandler();
        //    bool output = rh.TerminateRequestInSystem(Convert.ToInt32(id), User.Identity.Name);

        //    return Json(new { result = output });
        //}


        //[HttpPost]
        //public ActionResult DeleteRequest(string id)
        //{
        //    RequestHandler rh = new RequestHandler();
        //    bool output = rh.DeleteRequestInSystem(Convert.ToInt32(id), User.Identity.Name);

        //    return Json(new { result = output });
        //}


        //[HttpPost]
        //public ActionResult CompleteRequest(string id)
        //{
        //    RequestHandler rh = new RequestHandler();
        //    bool output = rh.CompleteRequestInSystem(Convert.ToInt32(id), User.Identity.Name);

        //    return Json(new { result = output });
        //}
        //#endregion

        //#region Controller Enumeration

        //public JsonResult GetControllerDDL()
        //{
        //    Assembly asm = Assembly.GetAssembly(typeof(AutomationStandards.MvcApplication));

        //    var controllerlist = asm.GetTypes()
        //            .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
        //            .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
        //            .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
        //            .Select(x => new { Controller = x.DeclaringType.Name.Replace("Controller", "") })
        //            .OrderBy(x => x.Controller).Distinct().ToList();

        //    List<SelectListItem> outlist = new List<SelectListItem>();

        //    foreach (var item in controllerlist)
        //    {
        //        SelectListItem entry = new SelectListItem();
        //        entry.Value = item.Controller;
        //        entry.Text = item.Controller;
        //        outlist.Add(entry);
        //    }

        //    JsonResult returnData = Json(outlist.ToList(), JsonRequestBehavior.AllowGet);
        //    return returnData;

        //}

        //public JsonResult GetControllerActionDDL(string controller)
        //{
        //    Assembly asm = Assembly.GetAssembly(typeof(AutomationStandards.MvcApplication));

        //    var controlleractionlist = asm.GetTypes()
        //            .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
        //            .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
        //            .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
        //            .Select(x => new { Controller = x.DeclaringType.Name.Replace("Controller", ""), Action = x.Name })
        //            .Where(x => x.Controller == controller)
        //            .OrderBy(x => x.Action).ToList();

        //    List<SelectListItem> outlist = new List<SelectListItem>();

        //    foreach (var item in controlleractionlist)
        //    {
        //        SelectListItem entry = new SelectListItem();
        //        entry.Value = item.Action;
        //        entry.Text = item.Action;
        //        outlist.Add(entry);
        //    }

        //    JsonResult returnData = Json(outlist.ToList(), JsonRequestBehavior.AllowGet);
        //    return returnData;
        //}
        //#endregion

        //#region TestEditor
        //public ActionResult MonacoEditorTest()
        //{
        //    return View();
        //}
        //#endregion
    }
}
