using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace synchronization
{
    public class Utility
    {
        #region 单号生成
        /// <summary>
        /// 生成新的单据号
        /// </summary>
        /// <param name="db"></param>
        /// <param name="bil_id"></param>
        /// <param name="bil_type"></param>
        /// <param name="bil_dd"></param>
        /// <param name="chk_dt"></param>
        /// <param name="chk_no"></param>
        /// <returns></returns>
        public static string GetNewNoIn(dbDataContext db, string bil_id, string bil_type, DateTime? bil_dd, string chk_dt, string chk_no)
        {
            string result = "";
            int count_id = 0;
            result = GetNewNoIn(db, bil_id, bil_type, bil_dd, chk_dt, chk_no, ref count_id);
            return result;
        }


        /// <summary>
        /// 根据现有单号获得后续单据号码
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public static string GetNewNoIn(string no)
        {
            string result = string.Empty;
            if (no.Length < 10)
                return result;

            string _pre = no.Substring(0, no.Length - 4);
            string _beh = no.Replace(_pre, "");
            int _id = 0;
            if (int.TryParse(_beh, out _id))
            {
                _id = Convert.ToInt32(_beh);
            }
            else
            {
                return result;
            }
            result = _pre + (_id + 1).ToString("D4");

            return result;
        }


        //ok
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbstring"></param>
        /// <param name="bil_id"></param>
        /// <param name="bil_type"></param>
        /// <param name="bil_dd"></param>
        /// <param name="chk_dt"></param>
        /// <returns></returns>
        private static string GetNewNoIn(dbDataContext db, string bil_id, string bil_type, DateTime? bil_dd, string chk_dt, string chk_no, ref int count_id)
        {
            DateTime dateTime = bil_dd.HasValue ? Convert.ToDateTime(bil_dd) : DateTime.Now;
            string text = "YMDD";
            string text2 = "7213456";
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            BILN bILN = (
                from it in db.BILN
                where it.BIL_ID == bil_id
                select it).FirstOrDefault<BILN>();
            if (bILN != null)
            {
                text = bILN.SEL_DD;
                if (!string.IsNullOrEmpty(bILN.SEL_CX))
                {
                    text2 = bILN.SEL_CX;
                }
                num = (bILN.SEL_ID.HasValue ? Convert.ToInt32(bILN.SEL_ID) : 0);
                int arg_155_0 = bILN.SEL_DEP.HasValue ? Convert.ToInt32(bILN.SEL_DEP) : 0;
                int arg_17D_0 = bILN.SEL_USR.HasValue ? Convert.ToInt32(bILN.SEL_USR) : 0;
                num2 = (bILN.SEL_ACC.HasValue ? Convert.ToInt32(bILN.SEL_ACC) : 0);
                int arg_1CD_0 = bILN.SEL_OTH.HasValue ? Convert.ToInt32(bILN.SEL_OTH) : 0;
                bil_id = (string.IsNullOrEmpty(bILN.BIL_ID1) ? bil_id : bILN.BIL_ID1);
                num3 = (bILN.SEL_NO.HasValue ? Convert.ToInt32(bILN.SEL_NO) : 0);
            }
            bil_type = (string.IsNullOrEmpty(bil_type) ? "" : bil_type);
            if (num > 0)
            {
                if (string.IsNullOrEmpty(bil_type))
                {
                    bil_type = 0.ToString("D" + num.ToString());
                }
                else
                {
                    bil_type = (bil_type + "0000000").Substring(0, num);
                }
            }
            else
            {
                bil_type = "";
            }
            int appearTimes = Utility.GetAppearTimes(text, "Y");
            string text3 = dateTime.Year.ToString();
            switch (appearTimes)
            {
                case 1:
                    text = text.Replace("Y", text3.Substring(3));
                    break;
                case 2:
                    text = text.Replace("YY", text3.Substring(2));
                    break;
                case 3:
                    text = text.Replace("YYY", text3.Substring(1));
                    break;
                case 4:
                    text = text.Replace("YYY", text3);
                    break;
            }
            int appearTimes2 = Utility.GetAppearTimes(text, "M");
            int month = dateTime.Month;
            switch (appearTimes2)
            {
                case 1:
                    if (month < 10)
                    {
                        text = text.Replace("M", month.ToString());
                    }
                    else
                    {
                        switch (month)
                        {
                            case 10:
                                text = text.Replace("M", "A");
                                break;
                            case 11:
                                text = text.Replace("M", "B");
                                break;
                            case 12:
                                text = text.Replace("M", "C");
                                break;
                        }
                    }
                    break;
                case 2:
                    text = text.Replace("MM", ((month < 10) ? "0" : "") + month.ToString());
                    break;
            }
            int appearTimes3 = Utility.GetAppearTimes(text, "D");
            int day = dateTime.Day;
            if (appearTimes3 != 1)
            {
                text = text.Replace("DD", ((day < 10) ? "0" : "") + day.ToString());
            }
            string newValue = "";
            switch (num2)
            {
                case 1:
                    newValue = "0";
                    break;
            }
            string text4 = "NNNNNNNNNNNNNNNNNNNNNNNNNNNN".Substring(0, num3);
            text2 = text2.Replace("7", "_zishedanjubianma_").Replace("1", "_riqi_").Replace("2", "_danjuleibie_").Replace("3", "_bumen_").Replace("4", "_zhidanren_").Replace("5", "_qita_").Replace("6", "_zhangtaoxuhao_");
            string str = text2.Replace("_zishedanjubianma_", bil_id).Replace("_riqi_", text).Replace("_danjuleibie_", bil_type).Replace("_bumen_", "").Replace("_zhidanren_", "").Replace("_qita_", "").Replace("_zhangtaoxuhao_", newValue);
            string part = str + text4;
            int num4 = num3;
            int value = 1;
            BILN1 bILN2 = db.BILN1.SingleOrDefault((BILN1 it) => it.BIL_ID == bil_id && it.PAT == part);
            if (bILN2 != null)
            {
                value = (bILN2.SQ.HasValue ? (Convert.ToInt32(bILN2.SQ) + count_id + 1) : 1);
            }
            else
            {
                value = count_id + 1;
            }
            string str2 = value.ToString("D" + num4.ToString());
            string text5 = str + str2;
            string result;
            if (!string.IsNullOrEmpty(chk_dt) && !string.IsNullOrEmpty(chk_no))
            {
                string query = string.Concat(new string[]
        {
            "SELECT ",
            chk_no,
            " FROM ",
            chk_dt,
            " WHERE ",
            chk_no,
            "='",
            text5,
            "'"
        });
                List<string> list = db.ExecuteQuery<string>(query, new object[]
        {
            ""
        }).ToList<string>();
                if (list.Count > 0)
                {
                     HyTools.LogTools.WriteLog("生成单号", string.Concat(new string[]
            {
                "规则，",
                bil_id,
                text,
                text4,
                ",单号是:",
                text5
            }));
                    result = Utility.GetNewNoIn(db, bil_id, bil_type, bil_dd, chk_dt, chk_no, ref value);
                    return result;
                }
                if (bILN2 != null)
                {
                    bILN2.SQ = new int?(value);
                }
                else
                {
                    bILN2 = new BILN1();
                    bILN2.BIL_ID = bil_id;
                    bILN2.PAT = part;
                    bILN2.SQ = new int?(value);
                    db.BILN1.InsertOnSubmit(bILN2);
                }
            }
            result = text5;
            return result;
        }

        public static string DeleteNo(string bil_id, DateTime? bil_dd, int bill_num)
        {
            string result = "";
            DateTime dd = (bil_dd.HasValue ? Convert.ToDateTime(bil_dd) : DateTime.Now);
            string biln_rule = bil_id + "YMDDNNNN";
            string biln_seldd = "YMDD";
            string biln_selno = "NNNN";

            using (dbDataContext db = new dbDataContext())
            {
                BILN objBiln = db.BILN.Where(it => it.BIL_ID == bil_id).FirstOrDefault();
                if (objBiln != null)
                {
                    biln_rule = objBiln.PAT;
                    biln_seldd = objBiln.SEL_DD;
                    biln_selno = biln_rule.Replace(bil_id + biln_seldd, "");
                }

                #region 年
                int _y = GetAppearTimes(biln_seldd, "Y");
                string _year = dd.Year.ToString();
                switch (_y)
                {
                    case 1:
                        biln_seldd = biln_seldd.Replace("Y", _year.Substring(3));
                        break;
                    case 2:
                        biln_seldd = biln_seldd.Replace("YY", _year.Substring(2));
                        break;
                    case 3:
                        biln_seldd = biln_seldd.Replace("YYY", _year.Substring(1));
                        break;
                    case 4:
                        biln_seldd = biln_seldd.Replace("YYY", _year);
                        break;
                }
                #endregion

                #region 月
                int _m = GetAppearTimes(biln_seldd, "M");
                int _month = dd.Month;
                switch (_m)
                {
                    case 1:  //一位月
                        if (_month < 10)
                        {
                            biln_seldd = biln_seldd.Replace("M", _month.ToString());
                        }
                        else
                        {
                            switch (_month)
                            {
                                case 10:
                                    biln_seldd = biln_seldd.Replace("M", "A");
                                    break;
                                case 11:
                                    biln_seldd = biln_seldd.Replace("M", "B");
                                    break;
                                case 12:
                                    biln_seldd = biln_seldd.Replace("M", "C");
                                    break;
                            }
                        }
                        break;
                    case 2:  //两位月
                        biln_seldd = biln_seldd.Replace("MM", (_month < 10 ? "0" : "") + _month.ToString());
                        break;
                }
                #endregion

                #region 日
                int _d = GetAppearTimes(biln_seldd, "D");
                int _day = dd.Day;
                if (_d == 1)
                {
                    //一位日

                }
                else
                {
                    //二位日
                    biln_seldd = biln_seldd.Replace("DD", (_day < 10 ? "0" : "") + _day.ToString());
                }
                #endregion

                BILN1 objBiln1 = db.BILN1.SingleOrDefault(it => it.PAT == bil_id + biln_seldd + biln_selno);
                if (objBiln1 != null && objBiln1.SQ >= bill_num)
                {
                    objBiln1.SQ = objBiln1.SQ - bill_num;
                }

                try
                {
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {

                }

            }
            return result;
        }

        private static int GetAppearTimes(string str1, string str2)
        {
            Regex ex = new Regex(str2);
            return ex.Matches(str1).Count;
        }
        #endregion
    }
}
