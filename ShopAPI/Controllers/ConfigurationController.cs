﻿using ShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace ShopAPI.Controllers
{
    public class ConfigurationController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage fetch()
        {

            ConfigurationModel odata = new ConfigurationModel();

            if (!ModelState.IsValid)
            {
                odata.status = "213";
                odata.message = "Some input parameters are missing.";
                return Request.CreateResponse(HttpStatusCode.BadRequest, odata);
            }
            else
            {

                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                string sessionId = "";

                List<Locationupdate> omedl2 = new List<Locationupdate>();

                DataTable dt = new DataTable();
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("proc_FTS_Configuration", sqlcon);
                sqlcmd.Parameters.Add("@Action", "GlobalCheck");
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt.Rows.Count > 0)
                {
                    odata = APIHelperMethods.ToModel<ConfigurationModel>(dt);
                    odata.status = "200";
                    odata.message = "Success";

                }
                else
                {
                    odata.status = "205";
                    odata.message = "No data found";

                }

                var message = Request.CreateResponse(HttpStatusCode.OK, odata);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage Userwise(ConfigurationModelInput model)
        {
            List<ConfigurationUser> omodel = new List<ConfigurationUser>();
            ConfigurationModelUser odata = new ConfigurationModelUser();

            if (!ModelState.IsValid)
            {
                odata.status = "213";
                odata.message = "Some input parameters are missing.";
                return Request.CreateResponse(HttpStatusCode.BadRequest, odata);
            }
            else
            {

                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                string sessionId = "";

                List<Locationupdate> omedl2 = new List<Locationupdate>();

                DataTable dt = new DataTable();
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("proc_FTS_Configuration", sqlcon);
                sqlcmd.Parameters.Add("@Action", "UserCheck");
                sqlcmd.Parameters.Add("@UserID", model.user_id);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt.Rows.Count > 0)
                {
                    omodel = APIHelperMethods.ToModelList<ConfigurationUser>(dt);
                    odata.status = "200";
                    odata.message = "Success";
                    odata.getconfigure = omodel;
                }
                else
                {
                    odata.status = "205";
                    odata.message = "No data found";

                }

                var message = Request.CreateResponse(HttpStatusCode.OK, odata);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage MeetingType(MeetingtypeInput model)
        {
            MeetingTypeOutput omodel = new MeetingTypeOutput();
            List<meetingList> oview = new List<meetingList>();

            if (!ModelState.IsValid)
            {
                omodel.status = "213";
                omodel.message = "Some input parameters are missing.";
                return Request.CreateResponse(HttpStatusCode.BadRequest, omodel);
            }
            else
            {
                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];

                DataSet ds = new DataSet();
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_API_MEETINGTYPE", sqlcon);
                sqlcmd.Parameters.Add("@user_id", model.user_id);
               
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(ds);
                sqlcon.Close();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    oview = APIHelperMethods.ToModelList<meetingList>(ds.Tables[0]);

                    omodel.status = "200";
                    omodel.message = "Successfully get Meeting Type list.";
                    omodel.meeting_type_list = oview;
                }
                else
                {
                    omodel.status = "205";
                    omodel.message = "No data found";
                }
                var message = Request.CreateResponse(HttpStatusCode.OK, omodel);
                return message;
            }
        }

        [HttpPost]
        public HttpResponseMessage LoginSettings(LoginSettingsInput model)
        {
            LoginSettingsOutput odata = new LoginSettingsOutput();

            if (!ModelState.IsValid)
            {
                odata.status = "213";
                odata.message = "Some input parameters are missing.";
                return Request.CreateResponse(HttpStatusCode.BadRequest, odata);
            }
            else
            {
                Encryption epasswrd = new Encryption();
                string Encryptpass = epasswrd.Encrypt(model.password);

                String token = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                string sessionId = "";

                DataTable dt = new DataTable();
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlCommand sqlcmd = new SqlCommand();
                SqlConnection sqlcon = new SqlConnection(con);
                sqlcon.Open();
                sqlcmd = new SqlCommand("PRC_APIBeforeLoginSettings", sqlcon);
                sqlcmd.Parameters.Add("@userName",model.user_name);
                sqlcmd.Parameters.Add("@password", Encryptpass);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                sqlcon.Close();
                if (dt.Rows.Count > 0)
                {
                    odata.status = "200";
                    odata.message = "Successfully get settings.";
                    odata.isAddAttendence = Convert.ToBoolean(dt.Rows[0]["isAddAttendence"].ToString());
                    odata.isFingerPrintMandatoryForAttendance = Convert.ToBoolean(dt.Rows[0]["isFingerPrintMandatoryForAttendance"].ToString()); ;
                    odata.isFingerPrintMandatoryForVisit = Convert.ToBoolean(dt.Rows[0]["isFingerPrintMandatoryForVisit"].ToString()); ;
                    odata.isSelfieMandatoryForAttendance = Convert.ToBoolean(dt.Rows[0]["isSelfieMandatoryForAttendance"].ToString()); ;
                }
                else
                {
                    odata.status = "205";
                    odata.message = "Either Login ID or Password is Invalid or your login is Inactive. Please correct login details or talk to administrator.";
                }
                var message = Request.CreateResponse(HttpStatusCode.OK, odata);
                return message;
            }
        }
	}
}