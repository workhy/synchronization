using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronizer
{
    #region 订单
    //[{"no":"2016111581689","cus_no":"5","cus_name":"xiaweiliang","rem":"","trade_model":"货到付款(现金)",
    //    "sh_usr":"收货人","sh_address":"广东省广州市天河区华景新城1507","sh_tel":"1302121212","ysfy":"8.00",
    //    "inv_flag":"T","inv_style":"1","inv_topic":"1"，"inv_content":"","inv_name":"inv_code","inv_address":"",
    //    "inv_tel","inv_bank":"","inv_bacc":"","items":[{"prd_no":"1801450081","qty":1, "price":175.70,"amtn":175.70，
    //    "tax_rto":"0.17", tax":0.5,"rem":""}, {"prd_no":"1801450081","qty":2, "price":30,"amtn":60,"tax_rto":"0.17","tax":1,"rem":""},]}]

    public class Model
    {
        public string no { get; set; }
        public string cus_no { get; set; }
        public string cus_name { get; set; }
        public string rem { get; set; }
        //交易方式
        public string trade_model { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string sh_usr { get; set; }
        /// <summary>
        /// 收货地址（含省市区详细地址）
        /// </summary>
        public string sh_address { get; set; }
        /// <summary>
        /// 收货电话
        /// </summary>
        public string sh_tel { get; set; }
        /// <summary>
        /// 运输费用
        /// </summary>
        public string ysfy { get; set; }
        /// <summary>
        /// 是否开票(开票为T,不开票为F)
        /// </summary>
        public string inv_flag { get; set; }
        /// <summary>
        /// 发票类型(1:普通发票,2:增值税发票)
        /// </summary>
        public string inv_style { get; set; }
        /// <summary>
        /// 发票抬头(1:单位,2:个人)
        /// </summary>
        public string inv_topic { get; set; }
        /// <summary>
        /// 发票抬头(1:单位,2:个人)
        /// </summary>
        public string inv_content { get; set; }
        /// <summary>
        /// 购货单位发票抬头
        /// </summary>
        public string inv_name { get; set; }
        /// <summary>
        ///纳税人识别号
        /// </summary>
        public string inv_code { get; set; }
        /// <summary>
        /// 发票地址
        /// </summary>
        public string inv_address { get; set; }
        /// <summary>
        /// 发票电话
        /// </summary>
        public string inv_tel { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string inv_bank { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        public string inv_bacc { get; set; }
        public List<ModelItem> items { get; set; }
    }
    //"items":[{"prd_no":"1801450081","qty":1, "price":175.70,"amtn":175.70，
    //    "tax_rto":"0.17", tax":0.5,"rem":""}, {"prd_no":"1801450081","qty":2, "price":30,"amtn":60,"tax_rto":"0.17","tax":1,"rem":""},]}
    public class ModelItem {
        public string prd_no { get; set; }
        public decimal qty { get; set; }
        public decimal price { get; set; }
        public decimal amtn { get; set; }
        public decimal tax_rto { get; set; }
        public decimal tax { get; set; }
        public string rem { get; set; }
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
