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
        public String ProductTargetDate { get; set; }


        
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

        public DataSet ProductTargetEntryInsertUpdate(String action, DateTime? SalesTargetDate, Int64 SALESTARGET_ID, String SalesTargetLevel, String SalesTargetNo,
            DataTable dtSalesTarget, Int64 userid = 0
            )
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_PRODUCTTARGETASSIGN");

            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddVarcharPara("@SalesTargetLevel", 100, SalesTargetLevel);
            proc.AddDateTimePara("@SalesTargetDate",  Convert.ToDateTime(SalesTargetDate));
            proc.AddBigIntegerPara("@SALESTARGET_ID", SALESTARGET_ID);
            proc.AddVarcharPara("@SalesTargetNo", 100, SalesTargetNo);

            if (action == "INSERTMAINPRODUCT" || action == "UPDATEMAINPRODUCT")
            {
                proc.AddPara("@UDTSalesTarget", dtSalesTarget);
            }
            ds = proc.GetDataSet();
            return ds;
        }


    }

    public class udtProductAddTarget
    {
        public string SlNO { get; set; }
        public Int64 PRODUCTADDTARGETDETAILS_ID { get; set; }
        public Int64 TARGETLEVELID { get; set; }
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