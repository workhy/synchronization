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
            
                    DateTime dt = DateTime.Now;
                    DateTime cur_dd = new DateTime(dt.Year, dt.Month, dt.Day,dt.Hour,dt.Minute,dt.Second);
                    DateTime est_dd = cur_dd.AddHours(48);

                    string usr = HyTools.ConfigTools.GetAppSetting("usr");
                    string os_no = Utility.GetNewNoIn(db, "SO", "", dt, "MF_POS", "OS_NO");
                    string td_no = Utility.GetNewNoIn(db, "TD", "", cur_dd, "TRAD_MTH", "TRAD_MTH");

                    #region MF_POS
                    MF_POS objMF_POS = new MF_POS {
                        OS_ID = "SO",
                        OS_NO = os_no,
                        BAT_NO = "",
                        OS_DD =new DateTime(cur_dd.Year,cur_dd.Month,cur_dd.Day),
                        TRAD_MTH=td_no,
                        PAY_MTH = "5",
                        PAY_DAYS = 1,
                        CHK_DAYS = 30,
                        PAY_REM=it.trade_model,
                        SEND_WH = "1",
                        EST_DD = new DateTime(est_dd.Year,est_dd.Month,est_dd.Day),//预交日
                        CUS_NO = GetCus_No(db, it.cus_no),
                        //CUS_NO=GetCus_No(db,it.cus_name),
                        CUR_ID = "",
                        EXC_RTO = 1,
                        CLS_ID = "F",
                        HIS_PRICE = "T",
                        ADR=it.sh_address,
                        TAX_ID = "3",//应税外加
                        PAY_DD = new DateTime(cur_dd.Year,cur_dd.Month,cur_dd.Day),
                        INT_DAYS = 30,
                        USR = usr,
                        CHK_MAN = usr,
                        PRT_SW = "N",
                        PRE_ID = "F",
                        CLS_DATE = new DateTime(cur_dd.Year,cur_dd.Month,cur_dd.Day),
                        CUS_NO_POS=it.no,
                        HS_ID = "T",
                        BIL_TYPE = "",
                        ISOVERSH = "F",
                        SYS_DATE = cur_dd,
                        CUS_OS_NO=it.no,
                        REM = "客户名称:" + (string.IsNullOrEmpty(it.cus_name)?"":it.cus_name) 
                            + ",电话:" + (string.IsNullOrEmpty(it.sh_tel)?"":it.sh_tel) 
                            + ",地址:" + (string.IsNullOrEmpty(it.sh_address)?"":it.sh_address)
                            + ",订单号:" +(string.IsNullOrEmpty(it.no)?"":it.no)
                    };
                    db.MF_POS.InsertOnSubmit(objMF_POS);
                    #endregion

                    #region TF_POS
                    int itmindex = 1;
                    decimal? totalamtn= 0;
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
                                CUS_OS_NO=it.no,
                                QTY=itm.qty,
                                UP=  itm.price,
                                AMT= itm.qty * itm.price,//金额
                                AMTN=itm.qty * itm.price,//未税本位币
                                TAX=itm.tax,//税额
                                TAX_RTO=itm.tax_rto,//税率
                                EST_DD=objMF_POS.EST_DD,//预交日
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

                            totalamtn = totalamtn + objTF_POS.AMTN;
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

                    #region TF_POS_RCV 发票送货部分
                    decimal _ysfy = 0;
                    decimal.TryParse(it.ysfy, out _ysfy);
                    var objTF_POS_RCV = new TF_POS_RCV {
                        OS_ID = objMF_POS.OS_ID,
                        OS_NO = objMF_POS.OS_NO,
                        CON_MAN = it.sh_usr,//收货人
                        FH_NO = "",
                        CUS_NO_KD = "",
                        ADR = it.sh_address,//收货地址
                        ZIP = "",
                        TEL_NO = "",
                        CELL_NO = it.sh_tel,//收货人电话
                        RCV_CHK = "T", //保存方式
                        SEND_WH_KD = "",//发货地址代号
                        INV_ID = (it.inv_style=="1"?"31":"32"),//发票类型(31普通发票，32增值税发票)
                        INV_NR = it.inv_content,//发票内容
                        INV_TT =  it.inv_topic,//发票抬头
                        DW_NAME = it.inv_name,//发票单位名称
                        NSR_CODE = it.inv_code,//纳税人识别号
                        ZC_ADR = it.inv_address,
                        ID_CODE = it.inv_bacc,//银行账号
                        KH_BANK=it.inv_bank,//开户银行
                        ZC_TEL="",//注册电话
                        AMT_EXPR=_ysfy,//运输费用
                        PAY_ASS="2",
                        COVER_ASS="2"//,
                        //SEND_MODEL="1"
                    };
                    db.TF_POS_RCV.InsertOnSubmit(objTF_POS_RCV);
                    #endregion

                    #region TRAD_MTH
                    var objTRAD_MTH = new TRAD_MTH {
                        TRAD_MTH1=td_no,
                        BILID="SO",
                        BIL_NO=objMF_POS.OS_NO,
                        ETD=cur_dd,
                        ETA=cur_dd,
                        USR_DEF1= "Usr_Def1",
                        USR_DEF2= "Usr_Def2",
                        SHIP_DATE=cur_dd
                    };
                    db.TRAD_MTH.InsertOnSubmit(objTRAD_MTH);
                    #endregion

                    #region MF_POS_Z
                    //decimal _scyf = 0;
                    //decimal.TryParse(it.scyf, out _scyf);

                    //decimal _zk = 0;
                    //decimal.TryParse(it.zk, out _zk);
                    //_zk = (totalamtn.HasValue? Convert.ToDecimal(totalamtn):0) % _zk;

                    //MF_POS_Z objMF_POS_Z = new MF_POS_Z
                    //{
                    //    OS_ID = objMF_POS.OS_ID,
                    //    OS_NO = objMF_POS.OS_NO,
                    //    KPXX = it.invoice,
                    //    SCYF = _scyf,
                    //    ZK = _zk
                    //};
                    //db.MF_POS_Z.InsertOnSubmit(objMF_POS_Z);
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
