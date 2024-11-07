﻿using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TargetVsAchievement.Models
{
    public class ProductTargetAddModel
    {
        public Int64 PRODUCTTARGET_ID { get; set; }
        public String ProductTargetLevel { get; set; }
        public String ProductTargetNo { get; set; }
        public DateTime ProductTargetDate { get; set; }


        
        

        public DataSet ProductTargetEntryInsertUpdate(String action, DateTime? ProductTargetDate, Int64 PRODCTTARGET_ID, String ProductTargetLevel, String ProductTargetNo,
            DataTable dtProductTarget, Int64 userid = 0
            )
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_PRODUCTTARGETASSIGN");

            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddVarcharPara("@ProductTargetLevel", 100, ProductTargetLevel);
            proc.AddDateTimePara("@ProductTargetDate",  Convert.ToDateTime(ProductTargetDate));
            proc.AddBigIntegerPara("@PRODUCTTARGET_ID", PRODUCTTARGET_ID);
            proc.AddVarcharPara("@ProductTargetNo", 100, ProductTargetNo);
            proc.AddBigIntegerPara("@USER_ID", userid);

            if (action == "INSERTMAINPRODUCT" || action == "UPDATEMAINPRODUCT")
            {
                proc.AddPara("@UDTProductTarget", dtProductTarget);
            }
            ds = proc.GetDataSet();
            return ds;
        }



        public DataTable GETTARGETASSIGNDETAILSBYID(String Action, Int64 DetailsID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PRODUCTTARGETASSIGN");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddBigIntegerPara("@PRODUCTTARGET_ID", DetailsID);
            ds = proc.GetTable();
            return ds;
        }

    }


    public class ProductTargetDetails
    {
        public string SlNO { get; set; }
        //public Int64 SALESTARGETDETAILS_ID { get; set; }
        public string TARGETDOCNUMBER { get; set; }
        public string TARGETLEVEL { get; set; }
        public string TIMEFRAME { get; set; }
        public DateTime STARTEDATE { get; set; }
        public DateTime ENDDATE { get; set; }
        public string TARGETPRODUCTCODE { get; set; }
        public string TARGETPRODUCTNAME { get; set; }
        public decimal ORDERAMOUNT { get; set; }
        public decimal ORDERQTY { get; set; }
        public string UpdateEdit { get; set; }
        public string TARGETLEVELID { get; set; }
        public String ActualSL { get; set; }
        public string INTERNALID { get; set; }
        public string PRODUCTID { get; set; }
    }

        public class udtProductAddTarget
    {
        public string SlNO { get; set; }
        public Int64 PRODUCTADDTARGETDETAILS_ID { get; set; }
        public Int64 TARGETLEVELID { get; set; }
        public string TARGETLEVEL { get; set; }
        public string INTERNALID { get; set; }
        public string TIMEFRAME { get; set; }
        public DateTime STARTEDATE { get; set; }
        public DateTime ENDDATE { get; set; }
        public string PRODUCTID { get; set; }
        public decimal ORDERAMOUNT { get; set; }
        public decimal ORDERQTY { get; set; }
        public Int64 UpdateEdit { get; set; }
    }

}