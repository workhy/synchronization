using synchronization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronizer
{
    public class BillCreater
    {
        public static ReturnMsg CreateBill(List<Model> lst) {
            ReturnMsg ret = null;

            List<string> lstsucess = new List<string>(); 
            using (dbDataContext db = new dbDataContext(HyTools.ConfigTools.GetConnectionString())) {
                foreach (Model it in lst) {
                    #region MF_POS
                    DateTime dt = DateTime.Now;
                    DateTime os_dd = new DateTime(dt.Year, dt.Month, dt.Day);
                    string usr = HyTools.ConfigTools.GetAppSetting("usr");
                    MF_POS objMF_POS = new MF_POS {
                        OS_ID = "SO",
                        OS_NO = Utility.GetNewNoIn(db, "SO", "", dt, "MF_POS", "OS_NO"),
                        BAT_NO = "",
                        OS_DD = os_dd,
                        PAY_MTH = "5",
                        PAY_DAYS = 1,
                        CHK_DAYS = 30,
                        SEND_WH = "1",
                        EST_DD = new DateTime(1899, 12, 30),
                        CUS_NO = GetCus_No(db, it.cus_no),
                        CUR_ID = "",
                        EXC_RTO = 1,
                        CLS_ID = "F",
                        HIS_PRICE = "T",
                        TAX_ID = "2",
                        PAY_DD = os_dd,
                        INT_DAYS = 30,
                        USR = usr,
                        CHK_MAN = usr,
                        PRT_SW = "N",
                        PRE_ID = "F",
                        CLS_DATE = os_dd,
                        HS_ID = "T",
                        BIL_TYPE = "",
                        ISOVERSH = "F",
                        SYS_DATE = os_dd,
                        CUS_OS_NO=it.no,
                        REM = "客户名称:" + (string.IsNullOrEmpty(it.cus_name)?"":it.cus_name) 
                            + ",电话:" + (string.IsNullOrEmpty(it.mobile)?"":it.mobile) 
                            + ",地址:" + (string.IsNullOrEmpty(it.address)?"":it.address)
                            + ",订单号:" +(string.IsNullOrEmpty(it.no)?"":it.no)
                    };
                    db.MF_POS.InsertOnSubmit(objMF_POS);
                    #endregion

                    #region MF_POS_Z
                    decimal _scyf = 0;
                    decimal.TryParse(it.scyf, out _scyf);

                    decimal _zk = 0;
                    decimal.TryParse(it.zk, out _zk);
                        
                    MF_POS_Z objMF_POS_Z = new MF_POS_Z {
                         OS_ID=objMF_POS.OS_ID,
                         OS_NO=objMF_POS.OS_NO,
                         KPXX=it.invoice,
                         SCYF =_scyf,
                         ZK =_zk
                    };
                    db.MF_POS_Z.InsertOnSubmit(objMF_POS_Z);
                    #endregion

                    #region TF_POS
                    int itmindex = 1;
                    foreach (ModelItem itm in it.items) {
                        PRDT objPRDT = GetPRDT(db, itm.prd_no);
                        if (objPRDT == null)
                        {
                            ret = new ReturnMsg
                            {
                                flag = false,
                                info = itm.prd_no + "没有对应信息."
                            };
                            HyTools.LogTools.WriteLog("CreateBill", itm.prd_no + "没有对应信息.");
                            return ret;
                        }
                        else {
                            TF_POS objTF_POS = new TF_POS
                            {
                                OS_ID = "SO",
                                OS_NO = objMF_POS.OS_NO,
                                ITM = itmindex,
                                PRD_NO = itm.prd_no,
                                PRD_NAME = objPRDT.NAME,
                                PRD_MARK = "",
                                WH =(string.IsNullOrEmpty( objPRDT.WH)?frmMain.wh:objPRDT.WH),
                                UNIT="1",
                                OS_DD=objMF_POS.OS_DD,
                                QTY=itm.qty,
                                UP=  itm.price,
                                AMT= itm.qty * itm.price,
                                AMTN=itm.qty * itm.price,
                                TAX=0,
                                //EST_DD=new DateTime(1899,12,30),
                                EST_ITM=itmindex,
                                CST_STD=0,
                                QTY_PO=0,
                                QTY_PO_UNSH=0,
                                PRE_ITM=itmindex,
                                FREE_ID_DEF="F",
                                UP_STD=0,
                                REM = (string.IsNullOrEmpty(it.no) ? "" : it.no)
                            };
                            db.TF_POS.InsertOnSubmit(objTF_POS);

                            itmindex++;

                            #region PRDT1
                            PRDT1 objPRDT1 = db.PRDT1.Where(i => i.PRD_NO == itm.prd_no 
                                && i.WH == objTF_POS.WH)
                                .FirstOrDefault();
                            if (objPRDT1 != null) {
                                objPRDT1.QTY_ON_ODR = (objPRDT1.QTY_ON_ODR.HasValue ? Convert.ToDecimal(objPRDT1.QTY_ON_ODR) : 0)
                                    + (objTF_POS.QTY);
                            }
                            #endregion
                        }

                    }
                    #endregion

                    try
                    {
                        db.SubmitChanges();
                    }
                    catch (Exception ex) {
                        HyTools.LogTools.WriteLog("CreateBill",it.no 
                            + Environment.NewLine 
                            + ex.ToString() );
                        lstsucess.Add(it.no);
                    }
                }
            }

            if (lstsucess.Count > 0)
            {
                string strs = "";
                foreach (string no in lstsucess)
                {
                    strs = strs + no + ",";
                }
                ret = new ReturnMsg
                {
                    flag = false,
                    info = strs
                };
            }
            else {
                ret = new ReturnMsg {
                    flag =true,
                    info ="sucess"
                }; 
            }
            return ret;
        }

        /// <summary>
        /// 获得客户编码
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cus_no"></param>
        /// <returns></returns>
        public static string GetCus_No(dbDataContext db, string cus_no) {
            string result = "";
            CUST objCUST = db.CUST.SingleOrDefault(it => it.CUS_NO == cus_no);
            if (objCUST == null)
            {
                result = HyTools.ConfigTools.GetAppSetting("cus_no");
            }
            else {
                result = objCUST.CUS_NO;
            }
            return result;
        }


        public static PRDT GetPRDT(dbDataContext db, string prd_no) {
            return db.PRDT.SingleOrDefault(it => it.PRD_NO == prd_no);
        }
    }
}
