using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Xml;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Net;

namespace bll
{
   public class Tools
    {
        private const Int32 AW_HOR_POSITIVE = 0x00000001;//自左向右显示
        private const Int32 AW_HOR_NEGATIVE = 0x00000002;//自右向左
        private const Int32 AW_VER_POSITIVE = 0x00000004;//自顶向下显示
        private const Int32 AW_VER_NEGATIVE = 0x00000008;//自下向上
        private const Int32 AW_CENTER = 0x00000010;//若使用了AW_HIDE标志，则使窗口向内重叠，否则向外扩展
        private const Int32 AW_HIDE = 0x00010000;//隐藏窗口，缺省显示
        private const Int32 AW_ACTIVATE = 0x00020000;//激活，在使用了AQ_HIDE标志后不要使用这个标志
        private const Int32 AW_SLIDE = 0x00040000;//使用滑动类型，缺省则为滚动动画类型，当使用AW_CENTER标志时，这个标志被忽略
        private const Int32 AW_BLEND = 0x00080000; //使用淡入效果，只有当hWnd为顶层窗口时才可以用些标志
        private static int _dwFlag = 6;


        /// <summary>
        /// 窗口动画的样式
        /// </summary>
        public static int DwFlag
        {
            get { return _dwFlag; }
            set { _dwFlag = value; }
        }

        /// <summary>
        /// 获取键状态
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        [DllImportAttribute("user32.dll")]
        public static extern short GetKeyState(int keyCode);


        [DllImport("User32.dll ", EntryPoint = "SetParent")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll ", EntryPoint = "ShowWindow")]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);


        /// <summary>
        /// 窗体动画
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="dwTime"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImportAttribute("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        public static void WindowAnimateFormEnter(Form obj)
        {

            switch (DwFlag)
            {
                case 0://普通显示
                    AnimateWindow(obj.Handle, 300, AW_ACTIVATE);
                    break;
                case 1://从左向右显示
                    AnimateWindow(obj.Handle, 300, AW_HOR_POSITIVE);
                    break;
                case 2://从右向左显示
                    AnimateWindow(obj.Handle, 300, AW_HOR_NEGATIVE);
                    break;
                case 3://从上到下显示
                    AnimateWindow(obj.Handle, 300, AW_VER_POSITIVE);
                    break;
                case 4://从下到上显示
                    AnimateWindow(obj.Handle, 300, AW_VER_NEGATIVE);
                    break;
                case 5://透明渐变显示
                    AnimateWindow(obj.Handle, 300, AW_BLEND);
                    break;
                case 6://从中间向四周
                    AnimateWindow(obj.Handle, 300, AW_CENTER);
                    break;
                case 7://左上角伸展
                    AnimateWindow(obj.Handle, 300, AW_SLIDE | AW_HOR_POSITIVE | AW_VER_POSITIVE);
                    break;
                case 8://左下角伸展
                    AnimateWindow(obj.Handle, 300, AW_SLIDE | AW_HOR_POSITIVE | AW_VER_NEGATIVE);
                    break;
                case 9://右上角伸展
                    AnimateWindow(obj.Handle, 300, AW_SLIDE | AW_HOR_NEGATIVE | AW_VER_POSITIVE);
                    break;
                case 10://右下角伸展
                    AnimateWindow(obj.Handle, 300, AW_SLIDE | AW_HOR_NEGATIVE | AW_VER_NEGATIVE);
                    break;
            }



        }

        public static void WindowAnimateFormExit(Form obj)
        {
            AnimateWindow(obj.Handle, 300, AW_SLIDE | AW_HIDE | AW_CENTER);
        }


        public static void WindowAnimateUserControl(UserControl obj)
        {

            AnimateWindow(obj.Handle, 300, AW_SLIDE + AW_CENTER);

        }


        public static string ReplaceSpecialSigns(string msg)
        {
            string re = "";
            if (!string.IsNullOrEmpty(msg))
            {
                re = msg.Replace('\'', '‘');
            }
            return re;
        }




       /// <summary>
        /// 判断手机号码运行商，0未知，1移动，2联通，3电信
       /// </summary>
       /// <param name="mobile"></param>
       /// <returns></returns>
        public static int GetMobileType(String mobile)
        {

            mobile = mobile.Trim();
            
            string cm = "^((13[4-9])|(147)|(15[0-4,7-9])|(18[2-4,7-8])|(178))\\d{8}$";
            string cu = "^((13[0-2])|(145)|(15[5-6])|(18[5-6])|(176))\\d{8}$";
            string ct = "^((133)|(153)|(18[0,1,9])|(177))\\d{8}$";
            string cmext = "^(1705)\\d{7}$";
            string cuext = "^(1709)\\d{7}$";
            string ctext = "^(1700)\\d{7}$";
            Regex cmr = new Regex(cm);
            Regex cur = new Regex(cu);
            Regex ctr = new Regex(ct);
            Regex cmrext = new Regex(cmext);
            Regex curext = new Regex(cuext);
            Regex ctrext = new Regex(ctext);
            int flag = 0;

            if (cmr.IsMatch(mobile) || cmrext.IsMatch(cmext))
            {
                flag = 1;
            }
            else if (cur.IsMatch(mobile) ||  curext.IsMatch(mobile))
            {
                flag = 2;
            }
            else if (ctr.IsMatch(mobile) || ctrext.IsMatch(mobile))
            {
                flag = 3;
            }

            return flag;

        }


        /// <summary>
        /// 检测是否加签名
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool ValidateSign(string content)
        {
            string a = "【";
            string b = "】";
            bool result = true;
            int a1 = content.LastIndexOf(a) + 1;
            int b1 = content.LastIndexOf(b) + 1;
            if (a1 == 0 || b1 == 0)
            {
                result = false;
                return result;
            }
            if (b1 == a1 + 1)
            {
                result = false;
                return result;
            }
            string sn = content.Substring(a1, b1 - a1 - 1);
            if (string.IsNullOrEmpty(sn.Trim()))
            {
                result = false;
                return result;
            }
            if (b1 != content.Length)
            {
                result = false;
                return result;
            }
            return result;
        }

        /// <summary>
        /// 跳转到在线客服
        /// </summary>
        public static void OnlineCustomerService()
        {
            Process process = new Process();
            process.StartInfo.FileName = "http://chat.7k35.com/v2/chatJs/chatForm.jsp?eprId=huixun&eprUserId=kf01&eprUserName=&visitorId=13866462772905050&lan=zh-cn&style=5";
            process.Start();
        }

 

 
        
        
        /// <summary>
        /// 中文编码 HttpUtility
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlEncoder(string content, string en)
        {
            string re = "";
            if (!string.IsNullOrEmpty(content))
            {
                re = System.Web.HttpUtility.UrlEncode(content, Encoding.GetEncoding(en));
            }
            return re;

        }

        /// <summary>
        /// 判断本机是繁体系统还是简体系统
        /// </summary>
        /// <param content=""></param>
        /// <returns></returns>
        public static string GetSystemLanguage()
        {
            string result = System.Threading.Thread.CurrentThread.CurrentCulture.ThreeLetterWindowsLanguageName;
            return result;
        }

        /// <summary>
        /// 繁体转简体
        /// </summary>
        /// <param content=""></param>
        /// <returns></returns>
        public static string ToSimple(string content)
        {
            string result = Strings.StrConv(content, VbStrConv.SimplifiedChinese, 0); // 繁体转简体
            return result;
        }

        /// <summary>
        /// 简体转繁体
        /// </summary>
        /// <param content=""></param>
        /// <returns></returns>
        public static string ToComplex(string content)
        {
            string result = Strings.StrConv(content, VbStrConv.TraditionalChinese, 0); // 简体转繁体
            return result;
        }

        /// <summary>
        /// 解析登录返回值
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ParseLoginResult(string result)
        {
            if (string.IsNullOrEmpty(result))
            {
                return false;
            }
            string[] tmp = result.Split('\n');
            string[] arr;
            if (tmp.Length > 1)
            {
                arr = tmp[1].Split('#');
            }
            else
            {
                arr = result.Split('#');
            }
            return false;

        }
        /// <summary>
        /// 计算条数
        /// </summary>
        /// <param name="m"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static int GetSMSCount(int m, string content)
        {
            int p = 1;
            int len = content.Length;
            if (len <= 70)
            {
                p = 1;
            }
            else
            {
                p = (int)Math.Ceiling(Convert.ToDouble(len) / 67);
            }
            return p * m;
        }

        /// <summary>
        /// 计算语音条数
        /// </summary>
        /// <param name="m"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static int GetVoiceSMSCount(int m, string content)
        {
            int p = 1;
            int len = content.Length;
            if (len <= 180)
            {
                p = 1;
            }
            else
            {
                p = (int)Math.Ceiling(Convert.ToDouble(len) / 180);
            }
            return p * m;
        }


        /// <summary>
        /// 计算语音文件条数
        /// </summary>
        /// <param name="duration">时长(注意,时间计算时会加5)</param>
        /// <param name="mobileCount">手机号码数量</param>
        /// <returns></returns>
        public static int GetVoiceFileCount(int duration, int mobileCount)
        {
            int p = 1;
            //p = (int)Math.Ceiling(Convert.ToDouble(duration+5) / 30);
            p = (int)Math.Ceiling(Convert.ToDouble(duration) / 60);
            return p * mobileCount;
        }


        /// <summary>
        /// 短信内容分隔条数
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static int MsgSplitCount(string content)
        {
            int p = 1;
            int len = content.Length;
            if (len <= 70)
            {
                p = 1;
            }
            else
            {
                p = (int)Math.Ceiling(Convert.ToDouble(len) / 67);
            }
            return p;
        }

        /// <summary>
        /// 短信内容分隔条数
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static int MsgCount(string content)
        {
            int p = 1;
            int len = content.Length;
            if (len <= 180)
            {
                p = 1;
            }
            else
            {
                p = (int)Math.Ceiling(Convert.ToDouble(len) / 180);
            }
            return p;
        }

        /// <summary>
        /// 读取配置文件节点值
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static string ReadNodeValue(string nodeName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Application.StartupPath + "\\" + "MyApp.xml");
            string nodeValue = "";
            XmlNodeList nodes = doc.GetElementsByTagName(nodeName);
            foreach (XmlNode n in nodes)
            {
                nodeValue = n.InnerText;
            }

            return nodeValue;
        }

        /// <summary>
        /// 设置配置文件节点值
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="value"></param>
        public static void SetNodeValue(string nodeName, string value)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Application.StartupPath + "\\" + "MyApp.xml");
            XmlNodeList nodes = doc.GetElementsByTagName(nodeName);
            foreach (XmlNode n in nodes)
            {
                n.InnerText = value;
            }
            doc.Save(Application.StartupPath + "\\" + "MyApp.xml");

        }



        /// <summary>
        /// 中文编码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string UrlEncoder(string content)
        {
            string re = "";
            if (!string.IsNullOrEmpty(content))
            {
                re = System.Web.HttpUtility.UrlEncode(content, Encoding.GetEncoding("GBK"));
            }
            return re;

        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5(string str)
        {
            MD5 md5 = MD5.Create();
            Encoding en = Encoding.Default;
            byte[] buff = md5.ComputeHash(en.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < buff.Length; i++)
            {
                sb.Append(buff[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="encod">编码格式</param>
        /// <returns></returns>
        public static string GetBase64Str(string str, Encoding encod)
        {
            Encoding en = encod;
            byte[] buff = en.GetBytes(str);
            return Convert.ToBase64String(buff);
        }

        /// <summary>
        /// BASE64解密
        /// </summary>
        /// <param name="base64str">字符串</param>
        /// <param name="encod">编码格式</param>
        /// <returns></returns>
        public static string DEbASE64(string base64str, Encoding encod)
        {
            string decode = "";
            Encoding en = encod;
            byte[] bytes = Convert.FromBase64String(base64str);
            try
            {
                decode = en.GetString(bytes);
            }
            catch
            {
                decode = "";
            }
            return decode;
        }

        /// <summary>
        /// 去除HTML标记   
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            return Htmlstring;
        }

        /// <summary>
        /// 取本机IP
        /// </summary>
        /// <returns></returns>
        private static string GetLocalIp()
        {
            string localIp;
            string hostName = Dns.GetHostName(); //得到主机名
            IPHostEntry ipEntry = Dns.GetHostEntry(hostName);//得到主机IP

            if (ipEntry.AddressList.Count() > 1)
            {
                //win7 ip
                localIp = ipEntry.AddressList[1].ToString();
            }
            else
            {
                //xp
                localIp = ipEntry.AddressList[0].ToString();
            }

            return localIp;

        }



        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="filename"></param>
        public static void PlaySound(string filename)
        {
            System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(Application.StartupPath + "\\sound\\Key\\" + filename);
            //sndPlayer.PlayLooping();
            sndPlayer.Play();
        }

        public static bool IsPwd(string pwd)
        {
            Regex regex = new Regex(@"^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,18}$");
            //Regex regex = new Regex(@"[0-9]*(([a-zA-Z]+[0-9]+)|([0-9]+[a-zA-Z]+))+[a-zA-Z]*");
            return regex.IsMatch(pwd);
        }

        /// <summary>
        /// 判断是否为正整数
        /// </summary>
        /// <param name="instring">字符</param>
        /// <returns></returns>
        public static bool IsInt(string instring)
        {
            Regex regex = new Regex(@"^[0-9]*[1-9][0-9]*$");
            return regex.IsMatch(instring.Trim());
        }

        /// <summary>
        /// 判断是否是去电显示号码
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool IsDisplayNum(string num)
        {
            Regex regex = new Regex(@"^0?(13|15|18|14|17)\d{9}$");
            //Regex regex = new Regex(@"^0?(133|153|180|181|189)\d{8}$");
            bool a = regex.IsMatch(num);
            // Regex regex2 = new Regex(@"^0\d{2,3}\d{5,9}|0\d{2,3}a\d{5,9}");//分机  
            Regex regex2 = new Regex(@"^0\d{10,11}$");
            bool b = regex2.IsMatch(num);
            Regex regex3 = new Regex(@"^0\d{9,12}a\d{1,5}$");
            bool c = regex3.IsMatch(num);
            if (a || b || c)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断是否是手机号码格式
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsMobile(string mobile)
        {
            Regex regex = new Regex(@"^0?(13|15|18|14|17)\d{9}$");
            return regex.IsMatch(mobile.Trim());
        }



        /// <summary>
        /// 判断是否是电信手机号码格式
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsCtMobile(string mobile)
        {
            //1700号段为中国电信,1705号段为中国移动,1709号段为中国联通。电信177，移动178号段，联通176。
            Regex regex = new Regex(@"^0?(133|153|180|181|189|177)\d{8}$");
            bool re2 = false;
            if (mobile.StartsWith("1700") && mobile.Length == 11)
            {
                re2 = true;
            }
            bool re1 = regex.IsMatch(mobile.Trim());
            if (re1 || re2)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断座机号码，-要去掉
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public static bool IsTel(string tel)
        {
            if (!string.IsNullOrEmpty(tel))
            {
                Regex regex = new Regex(@"^0\d{9,12}$");
                return regex.IsMatch(tel);
            }
            return false;
        }

        /// <summary>
        /// 判断是否是分机号码
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public static bool IsExtTel(string tel)
        {
            if (!string.IsNullOrEmpty(tel))
            {
                Regex regex = new Regex(@"^0\d{9,12}a\d{1,5}$");
                return regex.IsMatch(tel);
            }
            return false;
        }

        /// <summary>
        /// 取随机数
        /// </summary>
        /// <returns></returns>
        public static string GetRandom()
        {
            int t = (int)DateTime.Now.Ticks;
            Random r = new Random(t);
            return r.Next().ToString();
        }

        /// <summary>
        /// 时间戳1970至今
        /// </summary>
        /// <returns></returns>
        public static string GetTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long timestamp = Convert.ToInt64(ts.TotalMilliseconds);
            return timestamp.ToString();
        }



 
 




        /// <summary>
        /// 运行CMD命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns></returns>
        public static void Cmd(string[] cmd)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.AutoFlush = true;
            for (int i = 0; i < cmd.Length; i++)
            {
                p.StandardInput.WriteLine(cmd[i].ToString());
            }
            p.StandardInput.WriteLine("exit");
            //string strRst = p.StandardOutput.ReadToEnd();     
            //   p.WaitForExit();
            p.Close();

        }

        /// <summary>
        /// 关闭进程
        /// </summary>
        /// <param name="ProcName">进程名称</param>
        /// <returns></returns>
        public static bool CloseProcess(string ProcName)
        {
            bool result = false;
            System.Collections.ArrayList procList = new System.Collections.ArrayList();
            string tempName = "";
            int begpos;
            int endpos;
            foreach (System.Diagnostics.Process thisProc in System.Diagnostics.Process.GetProcesses())
            {
                tempName = thisProc.ToString();
                begpos = tempName.IndexOf("(") + 1;
                endpos = tempName.IndexOf(")");
                tempName = tempName.Substring(begpos, endpos - begpos);
                procList.Add(tempName);
                if (tempName == ProcName)
                {
                    if (!thisProc.CloseMainWindow())
                        thisProc.Kill(); // 当发送关闭窗口命令无效时强行结束进程
                    result = true;
                }
            }
            return result;
        }


    }//end
}//end
