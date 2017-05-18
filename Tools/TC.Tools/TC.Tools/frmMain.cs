using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;

namespace TC.Tools
{
    public partial class frmMain : Form
    {
        private static string server;        //测试页面地址
        private static int intusernum;        //虚拟测试人数
        private System.Windows.Forms.Timer timer1;
        private static bool isend;            //定义是否中断程序(线程)运行

        public frmMain()
        {
            server = "";
            intusernum = 0;
            isend = true;
            InitializeComponent();
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            this.button1.Enabled = true;
            this.button2.Enabled = false;
            this.lvDataList.GridLines = true; //显示表格线
            this.lvDataList.View = View.Details;//显示表格细节
            this.lvDataList.LabelEdit = false; //是否可编辑,ListView只可编辑第一列。
            this.lvDataList.Scrollable = true;//有滚动条
            this.lvDataList.HeaderStyle = ColumnHeaderStyle.Nonclickable;//对表头进行设置
            this.lvDataList.FullRowSelect = true;//是否可以选择行
            //添加表头
            this.lvDataList.Columns.Add("date", "时间", 150);
            this.lvDataList.Columns.Add("msg", "消息", 500);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            isend = false;
            this.lvDataList.Items.Clear();
            this.button1.Enabled = false;
            this.button2.Enabled = true;

            if (this.url.Text.Equals(""))
            {
                MessageBox.Show("请输入您要测试的URL地址!", "警告");
                this.url.Focus();
                isend = true;
                this.button1.Enabled = true;
                this.button2.Enabled = false;
                return;
            }
            else
            {
                server = url.Text;
            }
            try
            {
                if (this.usernum.Text.Equals(""))
                {
                    MessageBox.Show("请输入您希望的虚拟测试人数！", "警告");
                    isend = true;
                    this.button1.Enabled = true;
                    this.button2.Enabled = false;
                    this.usernum.Focus();
                    return;
                }
                else
                {
                    intusernum = int.Parse(this.usernum.Text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("您输入的测试人数不合法，请重新输入！", "警告");
                this.usernum.Text = "";
                isend = true;
                this.button1.Enabled = true;
                this.button2.Enabled = false;
                this.usernum.Focus();
                return;
            }

            //开始启动线程
            MainStart();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            isend = true;
            this.button1.Enabled = true;
            this.button2.Enabled = false;
        }

        public void MainStart()
        {
            if (server.Equals("") || intusernum == 0)
            {
                return;
            }
            //开始创建子线程
            for (int i = 0; i < intusernum; i++)
            {
                Thread Users = null;
                try
                {
                    Users = new Thread(new ThreadStart(this.ceshi));
                    Users.Priority = ThreadPriority.Normal;
                    Users.Start();
                }
                catch (Exception e)
                {
                    DispMsg("发生错误:" + e);
                    isend = true;
                    this.button1.Enabled = true;
                    this.button2.Enabled = false;
                    return;
                }
            }
        }

        //子线程：检测得到页面时间
        public void ceshi()
        {
            while (!isend)
            {
                Random rdm1 = new Random(unchecked((int)DateTime.Now.Ticks));
                string tname = rdm1.Next().ToString();
                try
                {
                    DateTime temptime = new DateTime();
                    temptime = DateTime.Now;
                    WebRequest myRequest = WebRequest.Create(server);        //初始化WEB页面
                    WebResponse myResponse = myRequest.GetResponse();        //返回RESPONSE对象
                    Stream stream = myResponse.GetResponseStream();
                    StreamReader sr = new StreamReader(stream);
                    string retstr = "";
                    string tmp = "";
                    while ((tmp = sr.ReadLine()) != null)
                    {
                        retstr += tmp;
                    }
                    myResponse.Close();        //管理RESPONSE对象
                    DispMsg("线程" + tname + "执行时间为：" + DateTime.Now.Subtract(temptime) + "豪秒");
                }
                catch (Exception)
                {
                    DispMsg("错误：您输入的URL地址不正确，或该页面不存在！");
                    break;
                }
            }
        }

        /// <summary>
        /// 定义一个代理
        /// </summary>
        /// <param name="MSG"></param>
        private delegate void DispMSGDelegate(string MSG);

        /// <summary>
        /// 定义一个函数，用于向窗体上的ListView控件添加内容
        /// </summary>
        /// <param name="strMsg"></param>
        private void DispMsg(string strMsg)
        {
            if (this.lvDataList.InvokeRequired == false)
            {
                //直接将内容添加到窗体的控件上
                ListViewItem lvi = new ListViewItem();
                lvi.SubItems[0].Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lvi.SubItems.Add(strMsg);
                this.lvDataList.Items.Insert(0, lvi);
            }
            else
            {
                DispMSGDelegate DMSGD = new DispMSGDelegate(DispMsg);
                this.lvDataList.Invoke(DMSGD, strMsg);
            }
        }
    }
}
