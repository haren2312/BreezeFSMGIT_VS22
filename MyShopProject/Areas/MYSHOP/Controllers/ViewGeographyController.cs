﻿/*******************************************************************************************************************
 * Written  by  Sanchita  V2.0.46  20/03/2024   0027322: A new dashboard floating menu is required as "View Geography".
 ********************************************************************************************************************/
using SalesmanTrack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using BusinessLogicLayer.SalesTrackerReports;
using System.Data;
using UtilityLayer;
using System.Collections;
using System.Configuration;
using MyShop.Models;
using DevExpress.Web.Mvc;
using DevExpress.Web;
using DataAccessLayer;
using System.Web.UI.WebControls;
using DevExpress.Xpo.DB;
using BusinessLogicLayer;
using DocumentFormat.OpenXml.EMMA;

namespace MyShop.Areas.MYSHOP.Controllers
{
    public class ViewGeographyController : Controller
    {
        UserList lstuser = new UserList();
        CommonBL objSystemSettings = new CommonBL();

        // GET: MYSHOP/ViewGeography
        public ActionResult Index()
        {
            try
            {

                ViewGeographyModel model = new ViewGeographyModel();
                string userid = Session["userid"].ToString();

                //DataTable dtstate = lstuser.GetStateList(userid);
                //model.modelstates = APIHelperMethods.ToModelList<GetUsersState>(dtstate);


                DataTable dtbranch = lstuser.GetHeadBranchList(Convert.ToString(Session["userbranchHierarchy"]), "HO");
                DataTable dtBranchChild = new DataTable();
                if (dtbranch.Rows.Count > 0)
                {
                    dtBranchChild = lstuser.GetChildBranch(Convert.ToString(Session["userbranchHierarchy"]));
                }
                model.modelbranch = APIHelperMethods.ToModelList<GetUsersBranch>(dtbranch);
                string h_id = model.modelbranch.First().BRANCH_ID.ToString();
                ViewBag.h_id = h_id;

                //DataTable dtpartytype = GetPartyTypeList();
                //model.modelpartytypes = APIHelperMethods.ToModelList<GetPartyType>(dtpartytype);

                return View(model);

            }
            catch(Exception ex)
            {
                throw (ex);
            }
        }

        
        public ActionResult GetPartyTypeList()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
            try
            {
                ViewGeographyModel model = new ViewGeographyModel();

                DataTable dtpartytype = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("FTS_VIEWGEOGRAPHY");
                proc.AddPara("@ACTION", "GETPARTYTYPE");
                dtpartytype = proc.GetTable();


                model.modelpartytypes = APIHelperMethods.ToModelList<GetPartyType>(dtpartytype);

                return PartialView("~/Areas/MYSHOP/Views/ViewGeography/_PartyTypeList.cshtml", model.modelpartytypes);

            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        public JsonResult GetPartyTypeListSelectAll()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
            try
            {
                ViewGeographyModel model = new ViewGeographyModel();

                DataTable dtpartytype = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("FTS_VIEWGEOGRAPHY");
                proc.AddPara("@ACTION", "GETPARTYTYPE");
                dtpartytype = proc.GetTable();

                model.modelpartytypes = APIHelperMethods.ToModelList<GetPartyType>(dtpartytype);

                return Json(model.modelpartytypes, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetOutlets(string stateid, string branchid, string partytypeid)
        {
            List<OutletList> AddressList = new List<OutletList>();

            //string newdate = Date.Split('-')[2] + '-' + Date.Split('-')[1] + '-' + Date.Split('-')[0];

            ViewGeographyModel model = new ViewGeographyModel();
            string userid = Session["userid"].ToString();

            DataTable dtmodellatest = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("FTS_VIEWGEOGRAPHY");
            proc.AddPara("@ACTION", "GETOUTLETLOCATION");
            proc.AddPara("@State", stateid);
            proc.AddPara("@Branch", branchid);
            proc.AddPara("@PartyType", partytypeid);
            proc.AddPara("@USER_ID", userid);
            dtmodellatest = proc.GetTable();

            AddressList = APIHelperMethods.ToModelList<OutletList>(dtmodellatest);
            //return Json(AddressList);
            var jsonResult = Json(AddressList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            

        }

    }
}