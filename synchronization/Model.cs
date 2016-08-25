using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synchronization
{

    public class Model
    {
        public string no { get; set; }

        public string cus_no { get; set; }

        public string cus_name { get; set; }

        public string region { get; set; }

        public string address { get; set; }

        public List<ModelItem> items { get; set; }
    }

    public class ModelItem {
        
        public string prd_no { get; set; }

        public decimal qty { get; set; }

        public decimal price { get; set; }
    }

    public class ReturnMsg {
        public bool flag { get; set; }

        public string info { get; set; }
    }
}
