﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Mvc;
namespace TargetVsAchievement.Models
{
    public class SalesTargetModel
    {
        public Int64 SALESTARGET_ID { get; set; }
        public String SalesTargetLevel { get; set; }
        public String SalesTargetNo { get; set; }
        public DateTime SalesTargetDate { get; set; }
       // public List<SalesTargetProduct> ListSalesTargetProduct { get; set; }
        public DataSet SalesTargetEntryInsertUpdate(String action, DateTime? SalesTargetDate, Int64 SALESTARGET_ID, String SalesTargetLevel,String SalesTargetNo,
            DataTable dtSalesTarget,Int64 userid = 0
            )
        {
            DataSet ds = new DataSet();           
            ProcedureExecute proc = new ProcedureExecute("PRC_SALESTARGETASSIGN");
            
            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddVarcharPara("@TargetType", 100, SalesTargetLevel);
            proc.AddDateTimePara("@TargetDate", Convert.ToDateTime(SalesTargetDate));
            proc.AddBigIntegerPara("@TARGET_ID", SALESTARGET_ID);           
            proc.AddVarcharPara("@TargetNo", 100, SalesTargetNo);
            proc.AddBigIntegerPara("@USER_ID", userid); 


            if (action == "INSERTSALESTARGET" || action == "UPDATESALESTARGET")
            {
                proc.AddPara("@FSM_UDT_SALESTARGETASSIGN", dtSalesTarget);
            }           
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable GETSALESTARGETASSIGNDETAILSBYID(String Action, Int64 DetailsID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_SALESTARGETASSIGN");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddBigIntegerPara("@SALESTARGET_ID", DetailsID);
            ds = proc.GetTable();
            return ds;
        }

    }
    public class SalesTargetProduct
    {
        public String ActualSL { get; set; }
        public string SlNO { get; set; }
        //public Int64 SALESTARGETDETAILS_ID { get; set; }
        

        public string TARGETDOCNUMBER { get; set; }

        public string TARGETLEVELID { get; set; }

        public string TARGETLEVEL { get; set; }

        public string INTERNALID { get; set; }


        public string TIMEFRAME { get; set; }

        public DateTime STARTEDATE { get; set; }

        public DateTime ENDDATE { get; set; }

        public Int64 NEWVISIT { get; set; }

        public Int64 REVISIT { get; set; }

        public decimal ORDERAMOUNT { get; set; }

        public decimal COLLECTION { get; set; }

        public decimal ORDERQTY { get; set; }

        public string UpdateEdit { get; set; }
        

    }

    public class udtSalesTarget
    {
        public string SlNO { get; set; }
        public Int64 SALESTARGETDETAILS_ID { get; set; }
        public Int64 TARGETLEVELID { get; set; }
        public string TARGETLEVEL { get; set; }
        public string INTERNALID { get; set; }
        public string TIMEFRAME { get; set; }

        public DateTime STARTEDATE { get; set; }

        public DateTime ENDDATE { get; set; }

        public Int64 NEWVISIT { get; set; }

        public Int64 REVISIT { get; set; }

        public decimal ORDERAMOUNT { get; set; }

        public decimal COLLECTION { get; set; }

        public decimal ORDERQTY { get; set; }

        public Int64 UpdateEdit { get; set; }
    }



   
}