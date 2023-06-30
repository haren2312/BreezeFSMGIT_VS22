﻿/***************************************************************************************************************************
 * Rev 1.0      Sanchita    V2.0.41     30-06-2023      The Export Format is not Proper in PJP Vs Actual Details Report. refer: 26436
 ****************************************************************************************************************************/
using BusinessLogicLayer.SalesmanTrack;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using MyShop.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.Areas.MYSHOP.Controllers
{
    public class PJPvsActualDetailsController : Controller
    {
        PJPvsActuualDetailsBL obj = new PJPvsActuualDetailsBL();
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Rendermainpage()
        {
            return PartialView();
        }

        public PartialViewResult Rendergrid(string ispageload)
        {
            return PartialView(GetReport(ispageload));
        }

        [HttpPost]
        public ActionResult GenerateTable(CustomerDetailsModel model)
        {
            try
            {
                string Is_PageLoad = string.Empty;
                DataTable dt = new DataTable();
                if (model.Fromdate == null)
                {
                    model.Fromdate = DateTime.Now.ToString("dd-MM-yyyy");
                }

                if (model.Todate == null)
                {
                    model.Todate = DateTime.Now.ToString("dd-MM-yyyy");
                }

                if (model.is_pageload != "1") Is_PageLoad = "Ispageload";

                ViewData["ModelData"] = model;

                string datfrmat = model.Fromdate.Split('-')[2] + '-' + model.Fromdate.Split('-')[1] + '-' + model.Fromdate.Split('-')[0];
                string dattoat = model.Todate.Split('-')[2] + '-' + model.Todate.Split('-')[1] + '-' + model.Todate.Split('-')[0];
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
                string Desgid = "";
                k = 1;
                if (model.desgid != null && model.desgid.Count > 0)
                {
                    foreach (string item in model.desgid)
                    {
                        if (k > 1)
                            Desgid = Desgid + "," + item;
                        else
                            Desgid = item;
                        k++;
                    }
                }

                double days = (Convert.ToDateTime(dattoat) - Convert.ToDateTime(datfrmat)).TotalDays;
                if (days <= 30)
                {
                    dt = obj.GetReportPJPActualDetails(datfrmat, dattoat, Userid, state, empcode, Desgid);
                }
                return Json(Userid, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        public IEnumerable GetReport(string ispageload)
        {
            string connectionString = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            string Userid = Convert.ToString(Session["userid"]);

            if (ispageload != "1")
            {
                ReportsDataContext dc = new ReportsDataContext(connectionString);
                var q = from d in dc.FTS_PJPvsActualDetailsReports
                        where d.USERID == Convert.ToInt32(Userid)
                        //orderby d.EMPLOYEE ascending
                        select d;
                return q;
            }
            else
            {
                ReportsDataContext dc = new ReportsDataContext(connectionString);
                var q = from d in dc.FTS_PJPvsActualDetailsReports
                        where d.USERID == 0
                        select d;
                return q;
            }
        }

        public ActionResult ExportPJPActualSummaryReport(int type, string IsPageload)
        {
            switch (type)
            {
                // Rev 1.0
                //case 1:
                //    return GridViewExtension.ExportToXlsx(GetGridViewSettings(), GetReport(IsPageload));
                ////break;
                //case 2:
                //    return GridViewExtension.ExportToXls(GetGridViewSettings(), GetReport(IsPageload));
                ////break;
                //case 3:
                //    return GridViewExtension.ExportToPdf(GetGridViewSettings(), GetReport(IsPageload));
                //case 4:
                //    return GridViewExtension.ExportToCsv(GetGridViewSettings(), GetReport(IsPageload));
                //case 5:
                //    return GridViewExtension.ExportToRtf(GetGridViewSettings(), GetReport(IsPageload));
                ////break;

                //default:
                //    break;

                case 1:
                    return GridViewExtension.ExportToPdf(GetGridViewSettings(), GetReport(IsPageload));
                case 2:
                    return GridViewExtension.ExportToXlsx(GetGridViewSettings(), GetReport(IsPageload));
                case 3:
                    return GridViewExtension.ExportToXls(GetGridViewSettings(), GetReport(IsPageload));
                case 4:
                    return GridViewExtension.ExportToRtf(GetGridViewSettings(), GetReport(IsPageload));
                case 5:
                    return GridViewExtension.ExportToCsv(GetGridViewSettings(), GetReport(IsPageload));
                default:
                    break;
            }

            return null;
        }

        private GridViewSettings GetGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "PJPvsActualDetailsReport";
            settings.CallbackRouteValues = new { Controller = "PJPvsActualDetails", Action = "GenerateTable" };
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "PJPvsActualDetailsReport";

            settings.Columns.Add(column =>
            {
                column.Caption = "Employee Name";
                column.FieldName = "EMPLOYEE";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 180;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Designation";
                column.Caption = "Designation";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 180;
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Phone";
                column.FieldName = "Phone";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 100;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Supervisor";
                column.Caption = "Supervisor";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 150;
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "State";
                column.FieldName = "State";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 180;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "PJPCustomer";
                column.Caption = "PJP Customer";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 150;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "CustomerType";
                column.Caption = "Customer Type";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 100;
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Customer Phone";
                column.FieldName = "CustomerPhone";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 100;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Location";
                column.Caption = "Location";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 100;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "From_Time";
                column.Caption = "From Time";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 100;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "To_Time";
                column.Caption = "To Time";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 100;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "PJPRemarks";
                column.Caption = "PJP Remarks";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 200;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "AchvCustomer";
                column.Caption = "Achv. Customer";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 100;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "AchvCustomerType";
                column.Caption = "Customer Type";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 100;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "AchvCustomerPhone";
                column.Caption = "Customer Phone";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 100;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "AchvLocation";
                column.Caption = "Location";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 100;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "AchvCustomerAddress";
                column.Caption = "Customer Address";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 200;
            });

            //settings.Columns.Add(column =>
            //{
            //    column.FieldName = "AchvGPSAddress";
            //    column.Caption = "GPS Address";
            //    column.ColumnType = MVCxGridViewColumnType.TextBox;
            //    column.Width = 200;
            //});

            settings.Columns.Add(column =>
            {
                column.FieldName = "AchvVisit_Time";
                column.Caption = "Visit Time";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 100;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Status";
                column.Caption = "Status";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 100;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Ordervalue";
                column.Caption = "Order Value";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 100;
                column.PropertiesEdit.DisplayFormatString = "0.00";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "AchvRemarks";
                column.Caption = "Feedback/Remarks";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 200;
            });

            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }
	}
}