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


namespace synchronization
{
    public class HttpClient
    {
        private string url = string.Empty;

        public HttpClient() {
            url = HyTools.ConfigTools.GetAppSetting("TargetWeb");
        }

        public string GetContent() {
            var rep = HttpHelper.CreateGetHttpResponse(url, 500, "", null);
            string content=  HttpHelper.GetResponseString(rep);
            return content;
        }

        public List<Model> GetModels(string msg) {
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

    }
}
