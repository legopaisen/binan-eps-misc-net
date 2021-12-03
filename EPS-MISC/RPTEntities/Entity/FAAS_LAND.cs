﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPTEntities.Entity
{
    public class FAAS_LAND
    {
        public string LAND_PIN { get; set; }
        public string ARP_LAND { get; set; }
        public string OCT_NO { get; set; }
        public string LOT_NO { get; set; }
        public string BLK_NO { get; set; }
        public string SURVEY_NO { get; set; }
        public string OWN_CODE { get; set; }
        public string UPDATE_CODE { get; set; }
        public string ZONE_CODE { get; set; }
        public string LOCN_CODE { get; set; }
        public string PB_N { get; set; }
        public string PB_E { get; set; }
        public string PB_S { get; set; }
        public string PB_W { get; set; }
        public string LAND_FMT { get; set; }
        public Nullable<decimal> LAND_AREA { get; set; }
        public Nullable<decimal> MARKET_VALUE { get; set; }
        public Nullable<decimal> ADJUSTED_MV { get; set; }
        public string ACT_USE { get; set; }
        public Nullable<decimal> ASS_LEV { get; set; }
        public Nullable<decimal> ASS_VAL { get; set; }
        public string TAXABILITY { get; set; }
        public string PREV_OWN_NM { get; set; }
        public Nullable<decimal> PREV_ASS_VAL { get; set; }
        public Nullable<System.DateTime> EFF_DATE { get; set; }
        public string APP_BY_NM { get; set; }
        public Nullable<System.DateTime> APP_BY_DT { get; set; }
        public string ASS_BY_NM { get; set; }
        public Nullable<System.DateTime> ASS_BY_DT { get; set; }
        public string REC_APP_NM { get; set; }
        public Nullable<System.DateTime> REC_APP_DT { get; set; }
        public string APPROVED_BY { get; set; }
        public Nullable<System.DateTime> APPROVED_DT { get; set; }
        public string MEMORANDA { get; set; }
        public string MEMO2 { get; set; }
        public string PREV_PIN { get; set; }
        public string PREV_ARP { get; set; }
        public string UPDATED_BY { get; set; }
        public Nullable<System.DateTime> ENCODE_DT { get; set; }
        public Nullable<System.DateTime> UPDATED_DT { get; set; }
        public string ENCODE_TM { get; set; }
        public string TD_NUM { get; set; }
        public string PREV_TD { get; set; }
        public Nullable<System.DateTime> BT_DATE { get; set; }
        public string EXEMPT_CODE { get; set; }
        public string CAD_NO { get; set; }
    }
}