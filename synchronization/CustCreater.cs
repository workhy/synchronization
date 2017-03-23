using Synchronizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synchronization
{
    public class CustCreater
    {
        //[{"cus_no":"客户的编码,现在用手机号","cus_name":"客户的名称","address":"客户的地址","cnt_man1":"联系人",
        //"tel1":"联系人电话1","adr1":"发票地址","adr2":"公司地址","fp_name":"发票名称"},
        /// <summary>
        /// 处理客户信息
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static ReturnMsg CreateCust(List<CustModel> lst)
        {
            ReturnMsg ret = null;
            var dd = DateTime.Now;
            using (dbDataContext db = new dbDataContext(HyTools.ConfigTools.GetConnectionString())) {
                foreach (var item in lst) {
                    CUST objCUST = db.CUST.SingleOrDefault(it => it.CUS_NO ==item.cus_no);
                    string cus_name = item.cus_name.Length >= 100 ? item.cus_name.Substring(0, 100) : item.cus_name;
                    string cus_snm = cus_name.Length >= 30 ? cus_name.Substring(0, 30) : cus_name;
                    string name_py = ConvertToPinYing.GetPYString(cus_name);
                    if (name_py.Length > 50)
                        name_py = name_py.Substring(0, 50);

                    if (objCUST == null)
                    {
                        #region 新增
                        objCUST = new CUST {
                            CUS_NO=item.cus_no,
                            NAME=cus_name,
                            SNM=cus_snm,
                            CLS_MTH="2",
                            CLS_DD=3,
                            CHK_DD=0,
                            MAS_CUS="",
                            OBJ_ID="1",
                            BOS_NM=item.cnt_man1,
                            CNT_MAN1=item.cnt_man1,
                            TEL1=item.tel1,
                            ADR1=item.adr1,//发票地址
                            ADR2=item.adr2,//公司地址
                            FP_NAME=item.fp_name,//发票名称
                            STR_DD=new DateTime(dd.Year,dd.Month,dd.Day),
                            CRD_ID="1",
                            ID1_TAX="1",
                            ID2_TAX="F",
                            CLS2="1",
                            CHK_CRD="F",
                            SO_CRD="F",
                            CHK_FAX="F",
                            CHK_CUS_IDX="1",
                            CY_ID="F",
                            CHK_MAN="ADMIN",
                            CLS_DATE=new DateTime(dd.Year,dd.Month,dd.Day,dd.Hour,dd.Minute,dd.Second),
                            CHK_PAY1="F",
                            PAY_FLAG="1",
                            CHK_PAY2="F",
                            CHK_PAY3="F",
                            CHK_QK="1",
                            USR1="ADMIN",
                            SYS_DATE=new DateTime(dd.Year,dd.Month,dd.Day,dd.Hour,dd.Minute,dd.Second),
                            CHK_IRP2="F",
                            CHK_BARCODE="1",
                            DATEFLAG_FQSK="1",
                            CHK_KD="F",
                            NAME_PY=name_py,
                            CHK_ZHANG_ID2="T",
                            CHK_TYPE_MINXF="1",
                            CHK_CK="T"
                        };
                        db.CUST.InsertOnSubmit(objCUST);
                        #endregion
                    }
                    else {
                        #region 修改
                        objCUST.NAME = cus_name;
                        objCUST.SNM = cus_snm;
                        objCUST.NAME_PY = name_py;
                        objCUST.CNT_MAN1 = item.cnt_man1;
                        objCUST.TEL1 = item.tel1;
                        objCUST.ADR1 = item.adr1;
                        objCUST.ADR2 = item.adr2;
                        objCUST.FP_NAME = item.fp_name;
                        #endregion
                    }
                }

                try
                {
                    db.SubmitChanges();
                    ret = new ReturnMsg {
                        flag =true
                    }; 
                }
                catch (Exception ex) {
                    HyTools.LogTools.WriteLog("CreateBill", ex.ToString());
                    ret = new ReturnMsg {
                        flag =false,
                        info =ex.ToString()
                    }; 
                }
            }
            return ret;
        }
    }
}
