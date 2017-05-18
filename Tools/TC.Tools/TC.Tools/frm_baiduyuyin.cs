using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;

namespace TC.Tools
{
    public partial class frm_baiduyuyin : Form
    {
        private static string testFileName = "/Kalimba.mp3";
        private static string apiKey = "Bgk7IgRG0OMzGc2whKBL8a64";
        private static string secretKey = "827e0df1eb98c4683cd551990f3050b0";
        private static string lan = "zh";//语言选择,填写 zh
        private static string per = "0";//发音人选择，取值 0-1  ；0 为女声，1 为男声，默认为女声
        private static string ctp = "1";//客户端类型选择，web 端填写 1
        private static string spd = "5";//语速，取值 0-9，默认为 5
        private static string pit = "6";//音调，取值 0-9，默认为 5
        private static string vol = "9";//音量，取值 0-9，默认为 5
        private static string cuid = "2C-60-0C-9C-D9-54";//用户唯一标识，用来区分用户，web 端参考填写机器 mac地址或 imei 码，长度为 60 以内
        private static string rest = "tex={0}&lan={1}&per={2}&ctp={3}&cuid={4}&tok={5}&spd={6}&pit={7}&vol={8}";
   
        private const int NULL = 0, ERROR_SUCCESS = NULL;
        [DllImport("WinMm.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        public frm_baiduyuyin()
        {
            InitializeComponent();
        }

        private void frm_baiduyuyin_Load(object sender, EventArgs e)
        {
            
        }

        private void getToken(string text)
        {
            try
            {
                string getTokenURL = "https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials" +
                        "&client_id=" + apiKey + "&client_secret=" + secretKey;

                HttpWebRequest proxyRequest = (HttpWebRequest)WebRequest.Create(getTokenURL);
                proxyRequest.Headers.Set("X-User-Agent", "");
                proxyRequest.KeepAlive = true;
                proxyRequest.Method = "POST";
                proxyRequest.ContentType = "application/json";
                using (HttpWebResponse proxyResponse = (HttpWebResponse)proxyRequest.GetResponse())
                {
                    Stream s = proxyResponse.GetResponseStream();
                    byte[] bytes = new byte[1024];
                    int n = s.Read(bytes, 0, 1024);
                    proxyResponse.Close();
                    string strJson = System.Text.Encoding.UTF8.GetString(bytes);
                    TokenModel tokenmodel = (TokenModel)FastJSON.ToObject(strJson, new TokenModel());
                    Play(tokenmodel.access_token, text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Play(string token, string text)
        {
            string strUpdateData = string.Format(rest, text, lan, per, ctp, cuid, token, spd, pit, vol);
            HttpWebRequest req = WebRequest.Create("http://tsn.baidu.com/text2audio") as HttpWebRequest;
            req.Method = "POST";
            req.ContentType = "audio/mp3";
            req.ContentLength = Encoding.UTF8.GetByteCount(strUpdateData);
            using (StreamWriter sw = new StreamWriter(req.GetRequestStream()))
            {
                sw.Write(strUpdateData);
            }
            HttpWebResponse res = req.GetResponse() as HttpWebResponse;
            using (Stream stream = res.GetResponseStream())
            {
                string strFullFileName = Application.StartupPath + testFileName;
                using (FileStream fs = new FileStream(strFullFileName, FileMode.Truncate | FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    stream.CopyTo(fs);
                }
                if (mciSendString(string.Format("open \"{0}\" alias app", strFullFileName), null, NULL, NULL) == ERROR_SUCCESS)
                    mciSendString("play app", null, NULL, NULL);
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            string text = this.txtText.Text;
            if (text == "") {
                MessageBox.Show("内容不能为空");
                return;
            }
            getToken(text);
        }
    }

    public class TokenModel
    {
        public string access_token { get; set; }
        public string session_key { get; set; }
        public string scope { get; set; }
        public string refresh_token { get; set; }
        public string session_secret { get; set; }
        public Int64 expires_in { get; set; }
    }
}

