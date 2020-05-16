using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
//using Microsoft.AspNetCore.Mvc;
//using System.Web.Security;
using AutomationStandards.Models.CustomModels;
using Microsoft.AspNetCore.Http;
using AutomationStandards.Models.DatabaseModels;

namespace AutomationStandards.Controllers
{
    public class StandardController : Controller
    {
        //
        // GET: /Standard/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Viewer(string id)
        {
            //Standard StandardInfo = new StandardModel().SelectSingleStandardAsDbStandardByName(id);
            AutomationStandardsContext db = new AutomationStandardsContext();
            Standard standardInfo = db.Standard.Where(x => x.DBTableName == id).FirstOrDefault();

            return View(standardInfo.StandardID);

        }


        public PartialViewResult StandardGrid()
        {
            StandardModel L = new StandardModel();
            //use this for delegating authorization by roles
            //List<StandardModel> model = L.SelectAllViewerStandardsByRoles(Roles.GetRolesForUser().ToList());
            List<StandardModel> model = L.SelectAllStandards();
            int c = model.Count;
            return PartialView(model);
            
        }

        public PartialViewResult StandardViewer(string id)
        {
            StandardValueSet model = new StandardValueSet();

            if (id != "0" && id != null)
            {
                
                //Standard StandardInfo = new StandardModel().SelectSingleStandardAsDbStandard(Convert.ToInt32(id));
                AutomationStandardsContext db = new AutomationStandardsContext();
                StandardSQLManagement sql = new StandardSQLManagement();
                Standard StandardInfo = db.Standard.First(s => s.StandardID == Convert.ToInt32(id));

                model.TableValues = sql.GetStandardValues(StandardInfo.DBTableName);
                model.Config = new StandardConfigModel().SelectStandardConfigs(StandardInfo.StandardID);
                model.StandardID = StandardInfo.StandardID;
                model.TableName = StandardInfo.DBTableName;
                model.StandardName = StandardInfo.StandardName;
                model.ManageRoles = StandardInfo.ManageRoles;

                ViewBag.standardId = StandardInfo.StandardID;
                ViewBag.standardName = StandardInfo.StandardName;
            }

            return PartialView(model);
        }

        public PartialViewResult StandardEditor(string id, string ParentId)
        {
            AutomationStandardsContext db = new AutomationStandardsContext();
            StandardSQLManagement sql = new StandardSQLManagement();

            StandardValueSingle model = new StandardValueSingle();

            //StandardModel StandardInfo = new StandardModel().SelectSingleStandard(Convert.ToInt32(StandardID));
            Standard StandardInfo = db.Standard.Where(x => x.StandardID == Convert.ToInt32(ParentId)).FirstOrDefault();
            model.dtRow = sql.GetStandardRow(StandardInfo.DBTableName, id);
            

            model.Config = new StandardConfigModel().SelectStandardConfigs(StandardInfo.StandardID);
            model.StandardID = StandardInfo.StandardID;
            model.TableName = StandardInfo.DBTableName;
            model.StandardName = StandardInfo.StandardName;

            return PartialView(model);
        }

        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult ProcessStandardValue(IFormCollection id, string ParentId)
        {
            StandardSQLManagement sql = new StandardSQLManagement();
            if (id["ID"].ToString() == "0")
            {
                sql.InsertStandardValueAsIFormCollecction(id, User.Identity.Name);
            }
            else
            {
                sql.UpdateStandardValueAsIFormCollection(id, User.Identity.Name);
            }

            return Json(new { result = true, id = ParentId });
        }

        [HttpPost]
        public ActionResult DeleteStandardValue(string id, string ParentId) //string StandardID)
        {
            StandardSQLManagement sql = new StandardSQLManagement();
            sql.DeleteStandardValue(ParentId, id, User.Identity.Name);

            return Json(new { result = true });
        }

        [HttpPost]
        public bool UpdateStandardValueSortOrder(Dictionary<string, string> data, string StandardID)
        {
            bool Result = true;

            StandardSQLManagement sql = new StandardSQLManagement();
            //StandardModel model = new StandardModel().SelectSingleStandard(Convert.ToInt32(StandardID));
            AutomationStandardsContext db = new AutomationStandardsContext();
            Standard model = db.Standard.Where(x => x.StandardID == Convert.ToInt32(StandardID)).FirstOrDefault();
            bool wsResponse;
            foreach (var item in data)
            {
                string ID = item.Key.Replace("RowID-", "");
                string RowID = item.Value;

                wsResponse = sql.SetStandardValueOrder(ID, RowID, model.DBTableName);
                if (wsResponse == false)
                {
                    Result = false;
                }
            }

            return Result;
        }

    }


}
