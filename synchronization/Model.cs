using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronizer
{
    #region 订单
    public class Model
    {
        public string no { get; set; }

        public string cus_no { get; set; }

        public string cus_name { get; set; }

        public string region { get; set; }

        public string address { get; set; }

        public string mobile { get; set; }

        public string invoice { get; set; }

        public string scyf { get; set; }

        public string zk { get; set; }

        public List<ModelItem> items { get; set; }
    }

    public class ModelItem {
        
        public string prd_no { get; set; }

        public decimal qty { get; set; }

        public decimal price { get; set; }
    }
    #endregion

    //[{"cus_no":"客户的编码,现在用手机号","cus_name":"客户的名称","address":"客户的地址",
    //    "cnt_man1":"联系人","tel1":"联系人电话1","adr1":"发票地址","adr2":"公司地址","fp_name":"发票名称"},
    //{"cus_no":"客户的编码,现在用手机号","cus_name":"客户的名称","address":"客户的地址",
    //"cnt_man1":"联系人","tel1":"联系人电话1","adr1":"发票地址","adr2":"公司地址","fp_name":"发票名称"}]
    /// <summary>
    /// 网站同步客户信息的客户信息交换结构
    /// </summary>
    public class CustModel {
        /// <summary>
        /// 客户的编码,现在用手机号
        /// </summary>
        public string cus_no { get; set; }
        /// <summary>
        /// 客户的名称
        /// </summary>
        public string cus_name { get; set; }
        /// <summary>
        /// 客户的地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string cnt_man1 { get; set; }
        /// <summary>
        /// 联系人电话1
        /// </summary>
        public string tel1 { get; set; }
        /// <summary>
        /// 发票地址
        /// </summary>
        public string adr1 { get; set; }
        /// <summary>
        /// 公司地址
        /// </summary>
        public string adr2 { get; set; }
        /// <summary>
        /// 发票名称
        /// </summary>
        public string fp_name { get; set; }
    }

    public class ReturnMsg {
        public bool flag { get; set; }

        public string info { get; set; }
    }
}
