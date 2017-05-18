using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CefSharp.WinForms;
using CefSharp;
using System.ServiceModel;
using System.Reflection;
using System.ServiceModel.Web;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Threading;

namespace Browser
{
    public partial class frmMain : Form
    {
        Thread fThread = null;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            ThreadStart thr_start_func = new ThreadStart(fun);
            if (fThread != null) fThread.Abort();
            fThread = new Thread(thr_start_func);
            fThread.Name = "funWcf";
            fThread.Start(); //starting the thread
        }

        private void fun()
        {
            for (int i = 0; i < 1000000000; i++)
            {
                try
                {
                    string res = PayPost("http://www.tcloudit.com/InfoContentService.svc/GetCarouselInfo");
                    Thread.Sleep(2000);
                }
                catch {
                    continue;
                }
            }
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="requestaddress">请求地址</param>
        /// <returns></returns>
        public static string PayPost(string requestaddress)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接
            ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;
            string responseString = string.Empty;
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values.Add("InfoCategoryID", "7");
                values.Add("IsIndex", "1");
                values.Add("IsTop", "0");
                values.Add("MenuID", "1001");
                values.Add("PageNumber", "1");
                values.Add("PageSize", "2");
                values.Add("ParentID", "1");
                client.Headers.Add("Token", Guid.NewGuid().ToString());
                byte[] response = client.UploadValues(requestaddress, "POST", values);
                responseString = Encoding.UTF8.GetString(response);
            }
            return responseString;
        }

        /// <summary>
        /// 验证证书
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
