using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace Synchronizer
{
    public class HttpClient
    {
        private string billurl = string.Empty;
        private string custurl = string.Empty;

        public HttpClient() {
            billurl = HyTools.ConfigTools.GetAppSetting("TargetWeb");
            custurl = HyTools.ConfigTools.GetAppSetting("CustWeb");
        }

        /// <summary>
        /// 获得返回信息
        /// </summary>
        /// <param name="style">bill,cust</param>
        /// <returns></returns>
        public string GetContent(string style) {
            HttpWebResponse rep = null;
            string content = "";
            switch (style) {
                case "bill":
                    rep = HttpHelper.CreateGetHttpResponse(billurl, 500, "", null);
                    content= HttpHelper.GetResponseString(rep);
                    break;
                case "cust":
                    rep = HttpHelper.CreateGetHttpResponse(custurl, 500, "", null);
                    content = HttpHelper.GetResponseString(rep);
                    break;
            }
            return content;
        }

        public List<Model> GetBillModels(string msg) {
            List<Model> lst = null;
            if (!string.IsNullOrEmpty(msg))
            {
                try
                {
                    lst = JsonConvert.DeserializeObject<List<Model>>(msg);
                }
                catch (Exception ex)
                {
                    HyTools.LogTools.WriteLog("getmodels", ex.ToString());
                    lst = new List<Model>();
                }
            }
            else {
                lst = new List<Model>(); 
            }
            return lst;
        }

        public List<CustModel> GetCustModels(string msg) {
            List<CustModel> lst = null;
            if (!string.IsNullOrEmpty(msg))
            {
                try
                {
                    lst = JsonConvert.DeserializeObject<List<CustModel>>(msg);
                }
                catch (Exception ex)
                {
                    HyTools.LogTools.WriteLog("getmodels", ex.ToString());
                    lst = new List<CustModel>();
                }
            }
            else
            {
                lst = new List<CustModel>();
            }
            return lst;
        }

    }
}
