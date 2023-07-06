﻿/****************************************************************************************************************************************
    Written by Sanchita  for    V2.0.41      06-07-2023       A new report is required in FSM as "Performance Summary(MTD) 
                                                              Mantis: 26427
 ***************************************************************************************************************************************/

using BusinessLogicLayer.SalesTrackerReports;
using DataAccessLayer;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting;
using Models;
using MyShop.Models;
using SalesmanTrack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace MyShop.Areas.MYSHOP.Controllers
{
    public class PerformanceSummaryMTDController : Controller
    {
        UserList lstuser = new UserList();
        PerformanceSummaryMonthWise_List objgps = null;  // to get the Year List

        // GET: MYSHOP/PerformanceSummaryMTD
        public ActionResult Index()
        {
            PerformanceSummaryMTDModel omodel = new PerformanceSummaryMTDModel();
            string userid = Session["userid"].ToString();

            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();

            ds.Tables.Add(dt1);
            ds.Tables.Add(dt2);
            ds.Tables.Add(dt3);
            return View(ds);

        }

        public ActionResult GetStateList()
        {
            try
            {
                List<GetUsersStates> modelstate = new List<GetUsersStates>();
                DataTable dtstate = lstuser.GetStateList();
                modelstate = APIHelperMethods.ToModelList<GetUsersStates>(dtstate);
                return PartialView("~/Areas/MYSHOP/Views/SearchingInputs/_StatesPartial.cshtml", modelstate);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        public ActionResult GetDesigList()
        {
            try
            {
                DataTable dtdesig = lstuser.Getdesiglist();
                List<GetDesignation> modeldesig = new List<GetDesignation>();
                modeldesig = APIHelperMethods.ToModelList<GetDesignation>(dtdesig);

                return PartialView("~/Areas/MYSHOP/Views/SearchingInputs/_DesigPartial.cshtml", modeldesig);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        public PartialViewResult PerformanceSummeryMTDGridViewCallback(PerformanceSummaryMTDModel model)
        {

            string IsPageLoad = string.Empty;

            string month = model.Month;
            string year = model.Year;


            string Userid = Convert.ToString(Session["userid"]);

            string state = "";
            int i = 1;

            if (model.StateId != null && model.StateId.Count > 0)
            {
                foreach (string item in model.StateId)
                {
                    if (i > 1)
                        state = state + "," + item;
                    else
                        state = item;
                    i++;
                }

            }

            string desig = "";
            int j = 1;

            if (model.desgid != null && model.desgid.Count > 0)
            {
                foreach (string item in model.desgid)
                {
                    if (j > 1)
                        desig = desig + "," + item;
                    else
                        desig = item;
                    j++;
                }

            }

            string empcode = "";

            int k = 1;

            if (model.empcode != null && model.empcode.Count > 0)
            {
                foreach (string item in model.empcode)
                {
                    if (k > 1)
                        empcode = empcode + "," + item;
                    else
                        empcode = item;
                    k++;
                }

            }

            DataSet ds = new DataSet();
            if (model.is_pageload != "0" && model.is_pageload != null)
            {
                ProcedureExecute proc = new ProcedureExecute("PRC_FTSPERFORMANCESUMMARYMTD_REPORT");
                proc.AddPara("@MONTH", month);
                proc.AddPara("@YEARS", year);
                proc.AddPara("@STATEID", state);
                proc.AddPara("@DESIGNID", desig);
                proc.AddPara("@EMPID", empcode);
                proc.AddPara("@USERID", Userid);
                ds = proc.GetDataSet();
            }
            else
            {
                ds.Tables.Add(new DataTable());
                ds.Tables.Add(new DataTable());
                ds.Tables.Add(new DataTable());
            }
            TempData["PerformanceSummeryMTDGridView"] = ds;
            return PartialView(ds);
        }
    }
}