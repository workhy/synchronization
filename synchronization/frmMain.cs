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
            //btnStart.Enabled = false;
            //btnStart.Text = "数据同步中";
            //timSynch.Enabled = true;
            //timSynch.Interval = interval * 1000 * 60;
            //timSynch.Start();

            HttpClient client = new HttpClient();
            string txt = System.IO.File.ReadAllText("d:\\log.txt");
            var _lst = client.GetBillModels(txt);
           ReturnMsg   ret = BillCreater.CreateBill(_lst);
            if (!ret.flag)
            {
                HyTools.LogTools.WriteLog("timSynch_Tick", ret.info);
            }
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
