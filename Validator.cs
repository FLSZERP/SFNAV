using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Web;
using System.Security;
using System.Security.Cryptography;
namespace SFAPP
{
    public class Validator
    {
        #region 验证邮箱
        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsEmail(string source)
        {
            if (source == null) return false;
            else return Regex.IsMatch(source, @"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", RegexOptions.IgnoreCase);
        }
        public static bool HasEmail(string source)
        {
            return Regex.IsMatch(source, @"[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证网址
        /// <summary>
        /// 验证网址
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsUrl(string source)
        {
            return Regex.IsMatch(source, @"^(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&amp;%_\./-~-]*)?$", RegexOptions.IgnoreCase);
        }
        public static bool HasUrl(string source)
        {
            return Regex.IsMatch(source, @"(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&amp;%_\./-~-]*)?", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证日期
        /// <summary>
        /// 验证日期
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsDateTime(string source)
        {
            try
            {
                DateTime time = Convert.ToDateTime(source);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 验证手机号
        /// <summary>
        /// 验证手机号
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsMobile(string source)
        {
            return Regex.IsMatch(source, @"^1[35]\d{9}$", RegexOptions.IgnoreCase);
        }
        public static bool HasMobile(string source)
        {
            return Regex.IsMatch(source, @"1[35]\d{9}", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证IP
        /// <summary>
        /// 验证IP
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsIP(string source)
        {
            return Regex.IsMatch(source, @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$", RegexOptions.IgnoreCase);
        }
        public static bool HasIP(string source)
        {
            return Regex.IsMatch(source, @"(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])", RegexOptions.IgnoreCase);
        }
        public static bool IsIp(string ip)
        {
            bool result = false;
            try
            {
                string[] iparg = ip.Split('.');
                if (string.Empty != ip && ip.Length < 16 && iparg.Length == 4)
                {
                    int intip;
                    for (int i = 0; i < 4; i++)
                    {
                        intip = Convert.ToInt16(iparg[i]);
                        if (intip > 255)
                        {
                            result = false;
                            return result;
                        }
                    }
                    result = true;
                }
            }
            catch
            {
                return result;
            }
            return result;
        }
        #endregion

        #region 验证身份证是否有效
        /// <summary>
        /// 验证身份证是否有效
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool IsIDCard(string Id)
        {
            if (Id.Length == 18)
            {
                bool check = IsIDCard18(Id);
                return check;
            }
            else if (Id.Length == 15)
            {
                bool check = IsIDCard15(Id);
                return check;
            }
            else
            {
                return false;
            }
        }

        public static bool IsIDCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }

        public static bool IsIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }
        #endregion

        #region 是不是Int型的
        /// <summary>
        /// 是不是Int型的
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsInt(string source)
        {
            Regex regex = new Regex(@"^(-){0,1}\d+$");
            if (regex.Match(source).Success)
            {
                if ((long.Parse(source) > 0x7fffffffL) || (long.Parse(source) < -2147483648L))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        #endregion

        #region 看字符串的长度是不是在限定数之间 一个中文为两个字符
        /// <summary>
        /// 看字符串的长度是不是在限定数之间 一个中文为两个字符
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="begin">大于等于</param>
        /// <param name="end">小于等于</param>
        /// <returns></returns>
        public static bool IsLengthStr(string source, int begin, int end)
        {
            int length = Regex.Replace(source, @"[^\x00-\xff]", "OK").Length;
            if ((length <= begin) && (length >= end))
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 是不是中国电话，格式010-85849685
        /// <summary>
        /// 是不是中国电话，格式010-85849685
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsTel(string source)
        {
            return Regex.IsMatch(source, @"^\d{3,4}-?\d{6,8}$", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 邮政编码 6个数字
        /// <summary>
        /// 邮政编码 6个数字
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsPostCode(string source)
        {
            return Regex.IsMatch(source, @"^\d{6}$", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 中文
        /// <summary>
        /// 中文
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsChinese(string source)
        {
            return Regex.IsMatch(source, @"^[\u4e00-\u9fa5]+$", RegexOptions.IgnoreCase);
        }
        public static bool hasChinese(string source)
        {
            return Regex.IsMatch(source, @"[\u4e00-\u9fa5]+", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证是不是正常字符 字母，数字，下划线的组合
        /// <summary>
        /// 验证是不是正常字符 字母，数字，下划线的组合
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNormalChar(string source)
        {
            return Regex.IsMatch(source, @"[\w\d_]+", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证用户名：必须以字母开头，可以包含字母、数字、“_”、“.”，至少5个字符
        /// <summary>
        /// 验证用户名：必须以字母开头，可以包含字母、数字、“_”、“.”，至少5个字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool checkUserId(string str)
        {
            Regex regex = new Regex("[a-zA-Z]{1}([a-zA-Z0-9]|[._]){4,19}");
            if (regex.Match(str).Success)
                if (regex.Matches(str)[0].Value.Length == str.Length)
                    return true;
            return false;
        }

        #endregion

        #region 验证是否为小数
        bool IsValidDecimal(string strIn)
        {

            return Regex.IsMatch(strIn, @"[0].d{1,2}|[1]");
        }

        #endregion

        #region 验证年月日
        bool IsValidDate(string strIn)
        {
            return Regex.IsMatch(strIn, @"^2d{3}-(?:0?[1-9]|1[0-2])-(?:0?[1-9]|[1-2]d|3[0-1])(?:0?[1-9]|1d|2[0-3]):(?:0?[1-9]|[1-5]d):(?:0?[1-9]|[1-5]d)$");
        }
        #endregion

        #region 验证日期格式
        //检察是否正确的日期格式
        public static bool IsDate(string str)
        {
            //考虑到了4年一度的366天，还有特殊的2月的日期
            Regex reg = new Regex(@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$");
            return reg.IsMatch(str);
        }
        #endregion

        #region 验证后缀名
        bool IsValidPostfix(string strIn)
        {
            return Regex.IsMatch(strIn, @".(?i:gif|jpg)$");
        }
        #endregion

        #region 验证字符是否在4至12之间
        bool IsValidByte(string strIn)
        {
            return Regex.IsMatch(strIn, @"^[a-z]{4,12}$");
        }
        #endregion

        #region dd-mm-yy 的日期形式代替 mm/dd/yy 的日期形式
        string MDYToDMY(String input)
        {
            return Regex.Replace(input, "http://www.cnblogs.com/tiantangwater/admin/file://b(/?\\d{1,2})/(?\\d{1,2})/(?\\d{2,4})\\b", "${day}-${month}-${year}");
        }

        #endregion


        #region MD5加密
        /// <summary>
        /// MD5加密，返回加密过的字符串
        /// </summary>
        /// <param name="InEncrypt">要加密的字符串</param>
        /// <returns></returns>
        public static string Md5(string InEncrypt)
        {
            string pwd = "";
            //pwd为加密结果
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(InEncrypt));
            //这里的UTF8是编码方式，你可以采用你喜欢的方式进行，比如UNcode等等
            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString();
            }
            return pwd;
        }
        #endregion

        #region 判断输入是否为空 并弹出错误信息
        /// <summary>
        /// 判断输入是否为空 并弹出错误信息
        /// </summary>
        /// <param name="str">验证内容</param>
        /// <param name="erroStr">错误提示内容</param>
        /// <returns>bool</returns>
        //public static bool IsEmpty(string str, string erroStr)
        //{
        //    bool result = false;
        //    if (string.Empty == str)
        //    {
        //        Validator.(erroStr);
        //        result = true;
        //    }
        //    return result;
        //}
        #endregion

        #region 判断字符串是否为数字
        /// <summary>
        /// 判断字符串是否为数字
        /// </summary>
        /// <param name="str">待验证的字符窜</param>
        /// <returns>bool</returns>
        public static bool IsNumber(string str)
        {
            bool result = true;
            foreach (char ar in str)
            {
                if (!char.IsNumber(ar))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        #endregion

        #region 是否为数字型
        /// <summary>
        /// 是否为数字型
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsDecimal(string strNumber)
        {
            return new System.Text.RegularExpressions.Regex(@"^([0-9])[0-9]*(\.\w*)?$").IsMatch(strNumber);
        }
        #endregion

        #region 字符串转换\数字转换 转换为安全格式

        /// <summary>
        /// 返回安全整数类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>int16 整形数字</returns>
        public static string safeInt(string str)
        {
            string constr = "0";
            try
            {
                constr = (Convert.ToInt16(str)).ToString();
                return constr;
            }
            catch
            {
                return constr;
            }
        }
        ///////////////////////////////////////////////
        //
        //  功能：安全处理用户提供的字符串内容
        //
        //  作者：依依秋寒
        //  时间：2008-10-27
        //
        ///////////////////////////////////////////////

        /// <summary>
        /// 转化为安全字符串 去除其中的英文单引号
        /// </summary>
        /// <param name="str">字符串内容</param>
        /// <returns>string 处理后的</returns>
        public static string safeStr(string str)
        {
            try
            {
                str = str.Replace("'", "'");
                str = str.Replace("\"", "&quot;");
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
                return str;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 把字符串转化为安全的text内容
        /// </summary>
        /// <param name="str">传入的字符串内容</param>
        /// <returns>转化后结果 string</returns>
        public static string safeText(string str)
        {
            try
            {
                str = str.Replace("'", "'");
                str = str.Replace("\"", "&quot;");
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
                str = str.Replace("\n", "<br />");
                return str;
            }
            catch
            {
                return "";
            }
        }

        ///////////////////////////////////////////////
        //
        //  功能：把经过安全处理的字符串返原成用户提交的内容
        //
        //  作者：依依秋寒
        //  时间：2008-10-27
        //
        ///////////////////////////////////////////////

        /// <summary>
        /// 把'还原成单引号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string safeStrToSource(string str)
        {
            try
            {
                str = str.Replace("&lt;", "<");
                str = str.Replace("&gt;", ">");
                str = str.Replace("&quot;", "\"");
                str = str.Replace("'", "'");
                str = str.Replace("&nbsp;", " ");
                return str;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 把经过安全的text内容转化为源字符串
        /// </summary>
        /// <param name="str">传入的字符串内容</param>
        /// <returns></returns>
        public static string safeTextToSource(string str)
        {
            try
            {
                str = str.Replace("<br />", "\n");
                str = str.Replace("&lt;", "<");
                str = str.Replace("&gt;", ">");
                str = str.Replace("&quot;", "\"");
                str = str.Replace("'", "'");
                str = str.Replace("&nbsp;", " ");
                return str;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 是否安全
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsSafeSql(string str)
        {
            string sql = str.ToLower();
            if (sql.Contains("delete"))
            {
                return false;
            }
            else if (sql.Contains("select"))
            {
                return false;
            }
            else if (sql.Contains("update"))
            {
                return false;
            }
            else if (sql.Contains("drop"))
            {
                return false;
            }
            else if (sql.Contains("execute"))
            {
                return false;
            }
            else if (sql.Contains("exec"))
            {
                return false;
            }
            else if (sql.Contains("'"))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 替换不安全sql
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SafeSql(string str)
        {
            System.Text.StringBuilder sb = new StringBuilder(str.ToLower());
            sb.Replace("delete", "");
            sb.Replace("select", "");
            sb.Replace("update", "");
            sb.Replace("drop", "");
            sb.Replace("execute", "");
            sb.Replace("exec", "");
            sb.Replace("'", "");
            return sb.ToString();
        }
        #endregion

        #region 全角\半角转换
        /// <summary>
        /// 全角转半角的函数(DBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        /// <summary>
        /// 半角转全角的函数(SBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>全角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>        
        public static string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }
        #endregion

        #region 弹出信息\跳转

        /// <summary>
        /// 弹出警告信息并跳转到指定页面地址
        /// </summary>
        /// <param name="str">警告信息内容</param>
        /// <param name="url">指定要跳转的页面抵制</param>
        //public static void alert(string str, string url)
        //{
        //    System.Web.HttpContext.Current.Response.Write("<script language='javascript'>alert('" + str + "');location.href='" + url + "'</script>");
        //}


        ///// <summary>
        ///// 弹出警告信息并返回申请页
        ///// </summary>
        ///// <param name="str">警告信息内容</param>
        //public static void alert(string str)
        //{
        //    System.Web.HttpContext.Current.Response.Write("<script language='javascript'>alert('" + str + "');location.href='" + System.Web.HttpContext.Current.Request.RawUrl + "'</script>");
        //}

        ///// <summary>
        ///// 弹出信息 无跳转动作
        ///// </summary>
        ///// <param name="str">信息内容</param>
        //public static void erro(string str)
        //{
        //    System.Web.HttpContext.Current.Response.Write("<script language='javascript'>alert('" + str + "');</script>");
        //}

        ///// <summary>
        ///// 后退一页
        ///// </summary>
        //public static void goback()
        //{
        //    System.Web.HttpContext.Current.Response.Write("<script language='javascript'>history.go(-1);</script>");
        //}

        ///// <summary>
        ///// 执行js命令
        ///// </summary>
        ///// <param name="function">传入要执行的js函数[包含参数的函数]</param>
        //public static void DoJsFunction(string function)
        //{
        //    System.Web.HttpContext.Current.Response.Write("<script language='javascript'>" + function + "</script>");
        //}
        #endregion

        #region 删除文本中带的HTML标记
        /// <summary>
        /// 删除文本中带的HTML标记
        /// </summary>
        /// <param name="InString">输入要删除带HTML的字符串</param>    
        /// <returns>返回处理过的字符串</returns>
        public static string DelHtmlCode(string InString)
        {
            string strTemp = InString;
            int htmlBeginNum = 0;
            int htmlEndNum = 0;
            while (strTemp.Contains("<"))
            {
                if (!strTemp.Contains(">")) { break; }    //当字符串内不包含">"时退出循环

                htmlBeginNum = strTemp.IndexOf("<");
                htmlEndNum = strTemp.IndexOf(">");
                //删除从"<"到">"之间的所有字符串
                strTemp = strTemp.Remove(htmlBeginNum, htmlEndNum - htmlBeginNum + 1);
            }
            strTemp = strTemp.Replace("\n", "");
            strTemp = strTemp.Replace("\r", "");
            strTemp = strTemp.Replace("\n\r", "");
            strTemp = strTemp.Replace("&nbsp;", "");
            strTemp = strTemp.Replace(" ", "");
            strTemp = strTemp.Trim();
            return strTemp;
        }

        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="NoHTML">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        //public static string NoHTML(string Htmlstring)
        //{
        //    string strOutput = Htmlstring;

        //    strOutput = Regex.Replace(strOutput, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
        //    strOutput = Regex.Replace(strOutput, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
        //    strOutput = Regex.Replace(strOutput, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
        //    strOutput = Regex.Replace(strOutput, @"-->", "", RegexOptions.IgnoreCase);
        //    strOutput = Regex.Replace(strOutput, @"<!--.*", "", RegexOptions.IgnoreCase);

        //    strOutput = Regex.Replace(strOutput, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
        //    strOutput = Regex.Replace(strOutput, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
        //    strOutput = Regex.Replace(strOutput, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
        //    strOutput = Regex.Replace(strOutput, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
        //    strOutput = Regex.Replace(strOutput, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
        //    strOutput = Regex.Replace(strOutput, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
        //    strOutput = Regex.Replace(strOutput, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
        //    strOutput = Regex.Replace(strOutput, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
        //    strOutput = Regex.Replace(strOutput, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
        //    strOutput = Regex.Replace(strOutput, @"&#(\d+);", "", RegexOptions.IgnoreCase);

        //    strOutput.Replace("<", "");
        //    strOutput.Replace(">", "");
        //    strOutput.Replace("\r\n", "");
        //    strOutput = HttpContext.Current.Server.HtmlEncode(strOutput).Trim();

        //    return HttpContext.Current.Server.HtmlEncode(strOutput).Trim();
        //}

        #endregion

        #region 截取字符串
        /// <summary>
        /// 从字符串左边起给定的位置截取字符串
        /// </summary>
        /// <param name="str">给定要截取的字符串内容</param>
        /// <param name="len">截取的起始位置</param>
        /// <returns>截取后的字符串 string</returns>
        public static string leftstr(string str, int len)
        {

            if (str.Length > len && len > 0)
            {
                return str.Substring(0, len - 1) + "…";
            }
            else
            {
                return str;
            }

        }
        #endregion

        #region 截取字符串
        /// <summary>
        /// 截取相应数目的字符
        /// </summary>
        public static string MyLeftFunction(string mText, int byteCount)
        {
            if (byteCount < 1) return mText;

            if (System.Text.Encoding.Default.GetByteCount(mText) <= byteCount)
            {
                return mText;
            }
            else
            {
                byte[] txtBytes = System.Text.Encoding.Default.GetBytes(mText);
                byte[] newBytes = new byte[byteCount - 4];

                for (int i = 0; i < byteCount - 4; i++)
                    newBytes[i] = txtBytes[i];

                return System.Text.Encoding.Default.GetString(newBytes) + "...";
            }
        }
        #endregion

        #region 编码解码
        ///// <summary>
        ///// 解码得到url值
        ///// </summary>
        ///// <param name="str">string</param>
        ///// <returns>string</returns>
        //public static string Decode(string str)
        //{
        //    return HttpContext.Current.Server.UrlDecode(str);
        //}

        ///// <summary>
        ///// 编码传入url
        ///// </summary>
        ///// <param name="str">string</param>
        ///// <returns>string</returns>
        //public static string Encode(string str)
        //{
        //    return HttpContext.Current.Server.UrlEncode(str);
        //}
        #endregion

        #region 返回组合的查询条件
        /// <summary>
        /// 返回组合的查询条件
        /// </summary>
        /// <param name="i"> 1 and = ,2 or = ,3  and like ,4  or like, 5 时间段</param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string BackSearchReson(int i, string columnName, string value)
        {
            string reson = "";
            if (columnName != string.Empty && value != string.Empty)
            {
                switch (i)
                {
                    case 1:
                        reson += " and " + columnName + "='" + value + "'";
                        break;
                    case 2:
                        reson += " or " + columnName + "='" + value + "'";
                        break;
                    case 3:
                        reson += " and " + columnName + " like'%" + value + "%'";
                        break;
                    case 4:
                        reson += " or " + columnName + " like'%" + value + "%'";
                        break;
                    case 5:
                        reson += " and (" + columnName + ">'" + Convert.ToDateTime(value).ToString() + "' and " + columnName + "<'" + Convert.ToDateTime(value).AddDays(1).ToString() + "')";
                        break;
                }
            }
            return reson;
        }
        #endregion

        #region 过滤字符串,过滤掉从输入框输入的一些非法字符，防注入！
        public static string InputText(string text, int maxLength)
        {
            text = text.Trim();
            if (text.Length == 0)
                return string.Empty;
            //if (text.Length > maxLength)
            //text = text.Substring(0, maxLength);
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");//<br/>
            text = Regex.Replace(text, "(http://www.cnblogs.com/tiantangwater/admin/file://s*&[n%7cn][b%7cb][s%7cs][p%7cp];//s*)+", " ");//&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);//any other tags
            text = text.Replace("'", "");
            text = text.Replace(" ", "");
            text = text.Replace("%", "");
            text = text.Replace("!", "");
            text = text.Replace("~", "");
            text = text.Replace("`", "");
            text = text.Replace("#", "");
            text = text.Replace("$", "");
            text = text.Replace("^", "");
            text = text.Replace("&", "");
            text = text.Replace("*", "");
            text = text.Replace("(", "");
            text = text.Replace(")", "");
            text = text.Replace("{", "");
            text = text.Replace("}", "");
            text = text.Replace("[", "");
            text = text.Replace("]", "");
            text = text.Replace(",", "");
            text = text.Replace("/", "");
            text = text.Replace("@", "");
            text = text.Replace("-", "");
            text = text.Replace("=", "");
            text = text.Replace("+", "");
            text = text.Replace("|", "");
            text = text.Replace(".", "");
            return text;
        }
        #endregion

        #region 验证是否包含汉语/全部汉语
        /// <summary>
        /// 验证是否包含汉语
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsHanyu(string str)
        {
            Regex regex = new Regex("[\u4e00-\u9fa5]");
            if (regex.Match(str).Success)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 验证是否全部汉语
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsHanyuAll(string str)
        {
            Regex regex = new Regex("[\u4e00-\u9fa5]");
            //匹配的内容长度和被验证的内容长度相同时，验证通过
            if (regex.Match(str).Success)
                if (regex.Matches(str).Count == str.Length)
                    return true;
            //其它，未通过
            return false;
        }
        #endregion
    }
}

