﻿using ERP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class ProductsBranchMapList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "PRODUCTBRANCHMAP_ID";
            int User_id = Convert.ToInt32(Session["userid"]);
            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (IsFilter == "Y")
            {

                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.PRODUCTBRANCHMAPLISTs
                        where d.USERID == User_id
                        orderby d.SEQ descending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.PRODUCTBRANCHMAPLISTs
                        where d.SEQ == 0
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string returnPara = Convert.ToString(e.Parameter);
            string strProduct_hiddenID = (Convert.ToString(txtProduct_hidden.Value) == "") ? "0" : Convert.ToString(txtProduct_hidden.Value);
            Task PopulateStockTrialDataTask = new Task(() => GetProductsBranchMapData(strProduct_hiddenID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetProductsBranchMapData(string Products)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_BRANCHWISEPRODUCTMAP_LIST", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@Products", Products);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));
                cmd.Parameters.AddWithValue("@ACTION", hFilterType.Value);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
    }
}