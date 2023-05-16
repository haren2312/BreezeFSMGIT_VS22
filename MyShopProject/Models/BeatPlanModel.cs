﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyShop.Models
{
    public class BeatPlanModel
    {
        public string AssignedTo { get; set; }
        public string AssignedEMPCode { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public string date_AssignedFrom { get; set; }
        public string date_AssignedTo { get; set; }
        public string Beat_Old { get; set; }
        public string Area_Old { get; set; }
        public string Route_Old { get; set; }
        public string Remarks { get; set; }

        public string Beat { get; set; }
        public string BeatId { get; set; }
        public List<clsBeat> BeatList { get; set; }

        public string Area { get; set; }
        public string AreaId { get; set; }
        public List<clsArea> AreaList { get; set; }

        public string Route { get; set; }
        public string RouteId { get; set; }
        public List<clsRoute> RouteList { get; set; }
    }

    public class clsBeat
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class clsArea
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class clsRoute
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}