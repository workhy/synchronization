using synchronization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synchronizer
{
    public partial class frmMain : Form
    {
        private int interval = 1000;
        private string TargetWeb = "";
        private string usr = "";
        private string cus_no = "";
        private string dep = "";
        public static string wh = "";

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            string _interval = HyTools.ConfigTools.GetAppSetting("interval");
            TargetWeb = HyTools.ConfigTools.GetAppSetting("TargetWeb");
            usr = HyTools.ConfigTools.GetAppSetting("usr");
            dep = HyTools.ConfigTools.GetAppSetting("dep");
            cus_no = HyTools.ConfigTools.GetAppSetting("cus_no");
            wh = HyTools.ConfigTools.GetAppSetting("wh");

            int.TryParse(_interval, out interval);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStart.Text = "数据同步中";
            timSynch.Enabled = true;
            timSynch.Interval = interval * 1000 * 60;
            timSynch.Start();

            //HttpClient client = new HttpClient();
            //string msg = "[{\"cus_no\":\"13588889999\",\"cus_name\":\"王先生1\",\"address\":\"云城东路10号\",\"cnt_man1\":\"王先生\",\"tel1\":\"13588889999\",\"adr1\":\"云城东路10号\",\"adr2\":\"万达集团广州分公司\",\"fp_name\":\"万达集团广州分公司\"},{\"cus_no\":\"18988833632\",\"cus_name\":\"郑文洪\",\"address\":\"天河区大观中路95号科汇园512\",\"cnt_man1\":\"郑文洪\",\"tel1\":\"18988833632\",\"adr1\":\"天河区大观中路95号科汇园512\",\"adr2\":\"广州市互信网络科技有限公司\",\"fp_name\":\"广州市互信网络科技有限公司\"},{\"cus_no\":\"13533215985\",\"cus_name\":\"章先生\",\"address\":\"田顶工业区\",\"cnt_man1\":\"章先生\",\"tel1\":\"13533215985\",\"adr1\":\"田顶工业区\",\"adr2\":\"广州佰信纸业有限公司\",\"fp_name\":\"广州佰信纸业有限公司\"},{\"cus_no\":\"15989152430\",\"cus_name\":\"海灯法师\",\"address\":\"dgdfhkjsddahfkjl\",\"cnt_man1\":\"海灯法师\",\"tel1\":\"15989152430\",\"adr1\":\"dgdfhkjsddahfkjl\",\"adr2\":\"火电发噶富士达\",\"fp_name\":\"火电发噶富士达\"},{\"cus_no\":\"1418749772\",\"cus_name\":\"佰信\",\"address\":\"\",\"cnt_man1\":\"佰信\",\"tel1\":\"1418749772\",\"adr1\":\"\",\"adr2\":\"\",\"fp_name\":\"\"}]";
            //var lst = client.GetCustModels(msg);
            //var ret = CustCreater.CreateCust(lst);

            // HttpClient client = new HttpClient();
            // string txt = System.IO.File.ReadAllText(\"d:\\log.txt\");
            // var _lst = client.GetBillModels(txt);
            //ReturnMsg   ret = BillCreater.CreateBill(_lst);
            // if (!ret.flag)
            // {
            //     HyTools.LogTools.WriteLog(\"timSynch_Tick", ret.info);
            // }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            timSynch.Stop();
        }

        private void timSynch_Tick(object sender, EventArgs e)
        {
            timSynch.Stop();

            try
            {
                HttpClient client = new HttpClient();
                string msg = "";
                ReturnMsg ret = null;

                #region 同步客户信息
                msg = client.GetContent("cust");
                HyTools.LogTools.WriteLog("cust 数据:", msg);
                var lst = client.GetCustModels(msg);
                ret =CustCreater.CreateCust(lst);
                if (!ret.flag)
                {
                    HyTools.LogTools.WriteLog("客户同步", ret.info);
                }
                #endregion

                #region 同步受订单
                msg = client.GetContent("bill");
                msg=msg.Replace("<font color=\"#FF0000\">财付通</font>","财付通");
                HyTools.LogTools.WriteLog("bill 数据:",msg);
                var _lst = client.GetBillModels(msg);
                ret = BillCreater.CreateBill(_lst);
                if (!ret.flag)
                {
                    HyTools.LogTools.WriteLog("单据同步", ret.info);
                }
                #endregion
            }
            catch (Exception ex) {
                HyTools.LogTools.WriteLog("timSynch_Tick", ex.ToString());
            }
            
            timSynch.Start();
        }
    }
}
