using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.IO;

using System.Configuration;
using System.Threading;

using Wodeyun.Gf.Entities;
using Wodeyun.Gf.System.Utilities;
using Wodeyun.Project.Win.Modules;

namespace Wodeyun.Project.Win
{
    public partial class Default : Form
    {
        private PmykModule _PmykModule = new PmykModule();
        private ZhhwyModule _ZhhwyModule = new ZhhwyModule();

        public Default()
        {
            InitializeComponent();
            this._ZhhwyModule.DataReceived += ZhhwyModule_DataReceived;

            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Default_Load(object sender, EventArgs e)
        {
            this.txtServer.Text = ConfigurationManager.AppSettings["Server"];

            this.btnSingle_Click(sender, e);
            //this.btnPmyk_Click(sender, e);
            //this.btnZhhwy_Click(sender, e);

            this.lstMessage.Items.Add("类型  时间                 手机号       内容");
        }

        private void ZhhwyModule_DataReceived(object sender, EventArgs e)
        {
            Entity entity = (Entity)sender;
            string date = entity.GetValue("Date").ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            string mobile = entity.GetValue("Mobile").ToString().PadRight(11);
            string text = entity.GetValue("Text").ToString();

            this.lstMessage.Items.Insert(1, string.Format("接收  {0}  {1}  {2}", date, mobile, text));
        }

        private void ZhhwyModule_ErrorThrown(object sender, EventArgs e)
        {
            if (this._PmykModule.OpenError() == false) MessageBox.Show("报警器异常！");
            else this._PmykModule.Check();
        }

        private void btnSingle_Click(object sender, EventArgs e)
        {
            string address = ConfigurationManager.AppSettings["Server"];
            string uniques = string.Format("127.0.0.1,{0},{1}", ConfigurationManager.AppSettings["Pmyk"], ConfigurationManager.AppSettings["Zhhwy"]);
            HttpWebRequest single = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}/SingleDownloader.ashx?Uniques={1}", address, uniques));

            single.BeginGetResponse(new AsyncCallback(Single_Callback), single);
        }

        private void Single_Callback(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());

            while (reader.EndOfStream == false)
            {
                Entity entity = Helper.Deserialize(reader.ReadLine());
                string command = entity.GetValue("Command").ToString();

                if (command == "Connected")
                {
                    this.btnSingle.Enabled = false;
                    this.btnSingle.Text = "已连接单返回服务器";
                    this.btnSingle.ForeColor = Color.Green;
                }
                if (command == "SendMessage")
                {
                    string mobile = entity.GetValue("Mobile").ToString();
                    string text = entity.GetValue("Text").ToString();

                    this.lstMessage.Items.Insert(1, string.Format("发送  {0}  {1}  {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), mobile, text));

                    if (this._ZhhwyModule.SendMessage(mobile, text) == true)
                        this._ZhhwyModule.Update(ConfigurationManager.AppSettings["Zhhwy"], Helper.GetEntity(true, "发送短信成功！"));
                    else
                        this._ZhhwyModule.Update(ConfigurationManager.AppSettings["Zhhwy"], Helper.GetEntity(true, "发送短信失败！"));
                }

                Thread.Sleep(1);
            }

            reader.Close();
        }

        private void btnPmyk_Click(object sender, EventArgs e)
        {
            try
            {
                this._PmykModule.Connect(ConfigurationManager.AppSettings["Pmyk"]);

                this.btnPmyk.Enabled = false;
                this.btnPmyk.Text = "已连接报警器";
                this.btnPmyk.ForeColor = Color.Green;
            }
            catch { }
        }

        private void btnZhhwy_Click(object sender, EventArgs e)
        {
            try
            {
                this._ZhhwyModule.Connect(ConfigurationManager.AppSettings["Zhhwy"], ConfigurationManager.AppSettings["Sca"], ConfigurationManager.AppSettings["Reply"], ConfigurationManager.AppSettings["Forward"]);

                this.btnZhhwy.Enabled = false;
                this.btnZhhwy.Text = "已连接短信猫";
                this.btnZhhwy.ForeColor = Color.Green;
            }
            catch { }
        }

        private void Default_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._PmykModule.CloseError();

            this._PmykModule.Dispose();
            this._ZhhwyModule.Dispose();
        }
    }
}