﻿using DevExpress.Web.Mvc;
using DevExpress.Web;
using LMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using UtilityLayer;
using DevExpress.Utils;
using DevExpress.XtraSpreadsheet.Model;
using DevExpress.XtraExport;
//using DevExpress.DataAccess.Native.Data;
//using DevExpress.DataAccess.Native.Data;

namespace LMS.Areas.LMS.Controllers
{
    public class LMSQuestionsController : Controller
    {
        LMSQuestionsModel obj = new LMSQuestionsModel();
        // GET: LMS/LMSQuestions
        public ActionResult Index()
        {
            EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/LMSQuestions/Index");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;




            return View();
        }

        public ActionResult QuestionAdd()
        {
            EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/LMSQuestions/Index");
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;

            if (TempData["QUESTIONS_ID"] != null)
            {
                obj.QUESTIONS_ID = Convert.ToString(TempData["QUESTIONS_ID"]);
                TempData.Keep();

                DataSet output = new DataSet();
                output = obj.EditQuestion(obj.QUESTIONS_ID);
                if (output.Tables[1].Rows.Count > 0)
                {
                    string[] names = output.Tables[1].AsEnumerable().Select(r => r["QUESTIONS_TOPICID"].ToString()).ToArray();
                    ////var stringArr = output.Tables[1].Rows[0].ItemArray.Select(x => x.ToString()).ToArray();
                    //List<string[]> MyStringArrays = new List<string[]>();
                    ////ArrayList list = new ArrayList();
                    //foreach (DataRow row in output.Tables[1].Rows)//or similar
                    //{
                    //    var a = row["QUESTIONS_TOPICID"];
                    //    MyStringArrays.Add(new string[] { "11" });
                    //    //MyStringArrays.Add(row["QUESTIONS_TOPICID"]);
                    //}

                    var stringArray = new string[3] { "11", "12", "13" };
                    //ViewBag.Collection = stringArray;
                    ViewBag.QUESTIONS_TOPICIDS= names;
                }

                    
            }

            return View("~/Areas/LMS/Views/LMSQuestions/QuestionAdd.cshtml", obj);
           
        }
        public ActionResult PartialGridList(LMSCategoryModel model)
        {
            try
            {
                EntityLayer.CommonELS.UserRightsForPage rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/LMSQuestions/Index");
                ViewBag.CanAdd = rights.CanAdd;
                ViewBag.CanView = rights.CanView;
                ViewBag.CanExport = rights.CanExport;
                ViewBag.CanEdit = rights.CanEdit;
                ViewBag.CanDelete = rights.CanDelete;

                string Is_PageLoad = string.Empty;
                DataTable dt = new DataTable();

                if (model.Is_PageLoad != "1")
                    Is_PageLoad = "Ispageload";

                ViewData["ModelData"] = model;
                string Userid = Convert.ToString(Session["userid"]);

                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_LMS_QUESTIONS", sqlcon);
                sqlcmd.Parameters.Add("@ACTION", "GETLISTINGDETAILS");
                sqlcmd.Parameters.Add("@USER_ID", Userid);
                sqlcmd.Parameters.Add("@ISPAGELOAD", model.Is_PageLoad);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                return PartialView("PartialQuestionsListing", GetQuestionsDetailsList(Is_PageLoad));

            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        public IEnumerable GetQuestionsDetailsList(string Is_PageLoad)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERP_ConnectionString"].ConnectionString;
            string Userid = Convert.ToString(Session["userid"]);
            if (Is_PageLoad != "Ispageload")
            {
                LMSMasterDataContext dc = new LMSMasterDataContext(connectionString);
                var q = from d in dc.LMS_QUESTIONSMASTERLISTs
                        where d.USERID == Convert.ToInt32(Userid)
                        orderby d.SEQ ascending
                        select d;
                return q;
            }
            else
            {
                LMSMasterDataContext dc = new LMSMasterDataContext(connectionString);
                var q = from d in dc.LMS_QUESTIONSMASTERLISTs
                        where d.SEQ == 0
                        select d;
                return q;
            }
        }
        public JsonResult SaveQuestion(string name, string id, string description, string Option1, string Option2, string Option3, string Option4, string Point1
            , string Point2, string Point3, string Point4, string chkCorrect1, string chkCorrect2, string chkCorrect3, string chkCorrect4,string TOPIC_ID,string Category_ID)
        {
            int output = 0;
            string Userid = Convert.ToString(Session["userid"]);
            output = obj.SaveQuestion(name, Userid, id, description, Option1, Option2, Option3, Option4, Point1, Point2, Point3, Point4
            , chkCorrect1, chkCorrect2, chkCorrect3, chkCorrect4, TOPIC_ID, Category_ID);
            return Json(output, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetMapDataByID(Int64 ID = 0, Int16 IsView = 0)
        {
            Boolean Success = false;
            try
            {
                TempData["QUESTIONS_ID"] = ID;
                TempData["IsView"] = IsView;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }
        public JsonResult EditQuestion(string id)
        {
            DataSet output = new DataSet();
            output = obj.EditQuestion(id);

            if (output.Tables[0].Rows.Count > 0)
            {
                return Json(new
                {
                    NAME = Convert.ToString(output.Tables[0].Rows[0]["QUESTIONS_NAME"]),
                    DESCRIPTION = Convert.ToString(output.Tables[0].Rows[0]["QUESTIONS_DESCRIPTN"]),

                    OPTIONS_NUMBER1 = Convert.ToString(output.Tables[0].Rows[0]["OPTIONS_NUMBER1"]),
                    OPTIONS_NUMBER2 = Convert.ToString(output.Tables[0].Rows[0]["OPTIONS_NUMBER2"]),
                    OPTIONS_NUMBER3 = Convert.ToString(output.Tables[0].Rows[0]["OPTIONS_NUMBER3"]),
                    OPTIONS_NUMBER4 = Convert.ToString(output.Tables[0].Rows[0]["OPTIONS_NUMBER4"]),

                    OPTIONS_POINT1 = Convert.ToString(output.Tables[0].Rows[0]["OPTIONS_POINT1"]),
                    OPTIONS_POINT2 = Convert.ToString(output.Tables[0].Rows[0]["OPTIONS_POINT2"]),
                    OPTIONS_POINT3 = Convert.ToString(output.Tables[0].Rows[0]["OPTIONS_POINT3"]),
                    OPTIONS_POINT4 = Convert.ToString(output.Tables[0].Rows[0]["OPTIONS_POINT4"]),

                    OPTIONS_CORRECT1 = Convert.ToString(output.Tables[0].Rows[0]["OPTIONS_CORRECT1"]),
                    OPTIONS_CORRECT2 = Convert.ToString(output.Tables[0].Rows[0]["OPTIONS_CORRECT2"]),
                    OPTIONS_CORRECT3 = Convert.ToString(output.Tables[0].Rows[0]["OPTIONS_CORRECT3"]),
                    OPTIONS_CORRECT4 = Convert.ToString(output.Tables[0].Rows[0]["OPTIONS_CORRECT4"]),





                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { name = "" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult Delete(string ID)
        {
            int output = 0;
            output = obj.DeleteQuestion(ID);
            return Json(output, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ExporQuestionList(int type)
        {
            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToXlsx(GetQuestionsGridViewSettings(), GetQuestionsDetailsList(""));              
                default:
                    break;
            }
            return null;
        }

        private GridViewSettings GetQuestionsGridViewSettings()
        {


            var settings = new GridViewSettings();
            settings.Name = "PartialGridList";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Questions";

            settings.Columns.Add(x =>
            {
                x.FieldName = "QUESTIONS_NAME";
                x.Caption = "Question";
                x.VisibleIndex = 1;
                x.Width = 200;
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "QUESTIONS_DESCRIPTN";
                x.Caption = "Description";
                x.VisibleIndex = 2;
                x.Width = 200;
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "CNTTOPIC"; 
                x.Caption = "Topics";
                x.VisibleIndex = 3;
                x.Width = 200;
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "CNTCATEGORY"; 
                x.Caption = "Categories";
                x.VisibleIndex = 4;
                x.Width = 200;
            });


            settings.Columns.Add(x =>
            {
                x.FieldName = "CREATEUSER";
                x.Caption = "Created By";
                x.VisibleIndex = 5;
                x.Width = 200;
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "CREATEDATE";
                x.Caption = "Created On";
                x.VisibleIndex = 6;
                x.Width = 200;
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "MODIFYUSER";
                x.Caption = "Updated By";
                x.VisibleIndex = 7;
                x.Width = 200;
            });
            settings.Columns.Add(x =>
            {
                x.FieldName = "MODIFYDATE";
                x.Caption = "Updated On";
                x.VisibleIndex = 8;
                x.Width = 200;
            });
            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }


        public ActionResult GetTopicList(string QUESTIONS_TOPICIDS, String QUESTIONS_ID)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
            try
            {
                List<GetTopic> modelbranch = new List<GetTopic>();               
                DataTable ComponentTable = new DataTable();
                ComponentTable = obj.GETLOOKUPVALUE("GETTOPIC");                
                modelbranch = APIHelperMethods.ToModelList<GetTopic>(ComponentTable);
                ViewBag.QUESTIONS_TOPICIDS = QUESTIONS_TOPICIDS;
                return PartialView("~/Areas/LMS/Views/LMSQuestions/_TopicLookUpPartial.cshtml", modelbranch);

            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        public ActionResult GetCategoryList()
        {           
            try
            {
                List<GetCategory> modelbranch = new List<GetCategory>();
                DataTable ComponentTable = new DataTable();
                ComponentTable = obj.GETLOOKUPVALUE("GETCATEGORY");
                modelbranch = APIHelperMethods.ToModelList<GetCategory>(ComponentTable);
                return PartialView("~/Areas/LMS/Views/LMSQuestions/_CategoryLookUpPartial.cshtml", modelbranch);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }
        public ActionResult GetCategoryListJson()
        {           
            try
            {
                List<GetCategory> modelbranch = new List<GetCategory>();
                DataTable ComponentTable = new DataTable();
                ComponentTable = obj.GETLOOKUPVALUE("GETCATEGORY");
                modelbranch = APIHelperMethods.ToModelList<GetCategory>(ComponentTable);
                return Json(modelbranch, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }
        public ActionResult GetTopicListJson()
        {
            try
            {
                List<GetTopic> modelbranch = new List<GetTopic>();
                DataTable ComponentTable = new DataTable();
                ComponentTable = obj.GETLOOKUPVALUE("GETTOPIC");
                modelbranch = APIHelperMethods.ToModelList<GetTopic>(ComponentTable);
                return Json(modelbranch, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }
    }
}