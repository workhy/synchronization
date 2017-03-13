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
        /// <summary>
        /// 折扣
        /// </summary>
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

    //[{"cus_no":"18988833632","cus_name":"郑文洪","address":"天河区大观中路95号科汇园512","cnt_man1":"郑文洪",
    //"tel1":"18988833632","adr1":"天河区大观中路95号科汇园512","adr2":"广州市互信网络科技有限公司",
    //"fp_name":"广州市互信网络科技有限公司"},
    //{"cus_no":"13533215985","cus_name":"章先生","address":"田顶工业区","cnt_man1":"章先生","tel1":"13533215985","adr1":"田顶工业区","adr2":"广州佰信纸业有限公司","fp_name":"广州佰信纸业有限公司"},{"cus_no":"15989152430","cus_name":"海灯法师","address":"dgdfhkjsddahfkjl","cnt_man1":"海灯法师","tel1":"15989152430","adr1":"dgdfhkjsddahfkjl","adr2":"火电发噶富士达","fp_name":"火电发噶富士达"},{"cus_no":"1418749772","cus_name":"佰信","address":"","cnt_man1":"佰信","tel1":"1418749772","adr1":"","adr2":"","fp_name":""}]
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
