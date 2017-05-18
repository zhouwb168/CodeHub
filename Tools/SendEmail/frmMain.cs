using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Runtime.InteropServices;

namespace SendEmail
{
    public partial class frmMain : Form
    {
        ConnDbForAcccess conn = new ConnDbForAcccess();
        INIClass iniCls = new INIClass(Application.StartupPath + "\\congfig.ini");
        SendEmails sdms = new SendEmails();
        Thread th = null;
        int SendSucc = 0;//发送成功个数
        int SendFail = 0;//发送失败个数
        string arrStrTO = "";//收件人地址
        string Megs = "";
        int status = 0;
        string strBody = "";//邮件内容
        string strFilePath = "";//附件地址
        public frmMain()
        {
            InitializeComponent();
            decimal RepayInterest = 0M;

            DateTime dtNow = DateTime.Parse("2016-08-10");
            DateTime dtNow1 = DateTime.Parse("2016-08-10");
            int interval = new TimeSpan(dtNow1.Ticks - dtNow.Ticks).Days;
            int days = DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
            if (interval > (days - 15))
            {
                //提前还款日期距离计划还款日期天数大于15天，以天计算利息，否则按整月计算。
                RepayInterest = (RepayInterest / 30) * (days - interval);
            }


            double monthRate = 11.16 / 100.00 / 12.00;//月利率
            RepayInterest = (decimal)(127100 * monthRate);
            decimal PenaltyAmt = RepayInterest / 2;
            RepayInterest = (RepayInterest / 30) * (days - interval);
            decimal InterestAmt = 3M;
            //decimal InterestAmt1 = decimal.Round(decimal.Parse(InterestAmt.ToString()), 2);
            decimal InterestAmt1 = decimal.Parse(double.Parse(InterestAmt.ToString()).ToString("F2"));
        }

        //电脑主机报警声
        [DllImport("kernel32.dll")]
        private static extern int Beep(int dwFreq, int dwDuration);

        public void Didi()
        {
            int a = 0X7FF;
            int b = 30000;
            Beep(a, b);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text.Trim() == "")
            {
                MessageBox.Show("邮件标题或内容不能为空");
                return;
            }
            PlayExecuteSend();
        }

        /// <summary>
        /// 执行开始发送命令
        /// </summary>
        private void PlayExecuteSend()
        {
            try
            {
                status = 1;
                string htmlurl = iniCls.IniReadValue("SMTP", "htmlUrl");
                strBody = htmlurl == "" ? htmlEditor1.Text.Trim() : SendEmails.GetHtmlSource(htmlurl);//;//邮件内容
                this.btnSend.Enabled = false;
                comboBox1.Enabled = false;
                th = new Thread(new ThreadStart(Sends));
                th.IsBackground = true;
                th.Start();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Sends()
        {
            try
            {
                string strForm = iniCls.IniReadValue("SMTP", "txtAddr");//发件人地址
                string senderDisplayName = iniCls.IniReadValue("SMTP", "DisplayName");//发件人名称
                string userPswd = iniCls.IniReadValue("SMTP", "txtPwd");//用户密码
                string strHost = iniCls.IniReadValue("SMTP", "txtSmtp");//服务器地址
                int port = int.Parse(iniCls.IniReadValue("SMTP", "txtPort"));//服务器端口
                int sendCount = int.Parse(iniCls.IniReadValue("SMTP", "sendCount"));//每次发送数量
                strFilePath = txtAtt.Text.Trim();//附件路径
                string strSubject = txtTitle.Text.Trim();//邮件标题
                //if (CHKSave.Checked)
                //{
                //    InsertEmailContent(strSubject, strBody);//保存邮件内容
                //}
                while (lstMail.Items.Count > 0)
                {
                    arrStrTO = lstMail.GetItemText(lstMail.Items[0]);
                    Megs = "正发送:" + arrStrTO + ", 已成功发送" + SendSucc + "封邮件，失败" + SendFail + "封邮件";
                    MethodInvoker In = new MethodInvoker(lblValue);
                    this.BeginInvoke(In);

                    if (sdms.SendEmailTo(arrStrTO.Trim(), strForm.Trim(), senderDisplayName, userPswd, strSubject, strBody, strHost, port, strFilePath) == true)
                    {
                        MethodInvoker Ins = new MethodInvoker(DeleMailLst);
                        this.BeginInvoke(Ins);
                        SendSucc++;
                    }
                    else
                    {
                        MethodInvoker Ins = new MethodInvoker(DeleMailLst);
                        this.BeginInvoke(Ins);
                        SendFail++;
                    }
                    if (SendSucc + SendFail > sendCount)
                    {
                        SendSucc = 0;
                        SendFail = 0;
                        break;
                    }
                }
                MethodInvoker Inss = new MethodInvoker(BtnEnabled);
                this.BeginInvoke(Inss);

                //MethodInvoker InCombox = new MethodInvoker(changeComboxSelectIndex);
                //this.BeginInvoke(InCombox);
            }
            catch
            {
                Didi();
            }
        }

        private void lblValue()
        {
            label4.Text = Megs;
        }

        private void DeleMailLst()
        {
            try
            {
                if (lstMail.Items.Count != 0)
                    lstMail.Items.RemoveAt(0);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnEnabled()
        {
            this.btnSend.Enabled = true;
            comboBox1.Enabled = true;
        }

        private void changeComboxSelectIndex()
        {
            if (getEtypeCount() > 0 && comboBox1.SelectedIndex < (getEtypeCount() - 1))
            {
                comboBox1.SelectedIndex++;
                this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (status == 1)
                {
                    string fileName = this.comboBox1.Text + ".txt";
                    while (lstMail.Items.Count > 0)
                    {
                        StreamWriter sw = new StreamWriter(fileName, true, System.Text.Encoding.UTF8);
                        sw.Write(lstMail.Items[0].ToString() + "\r\n");
                        sw.Close();
                        lstMail.Items.RemoveAt(0);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void 导入邮件地址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportAdd imp = new ImportAdd();
            imp.ShowDialog();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            try
            {
                timer1.Interval = int.Parse(iniCls.IniReadValue("TimerSend", "InterVal"));//定时发送时间间隔
                //加载群组
                LoadType();
                //加载邮件列表
                LoadEmailList(comboBox1.SelectedValue.ToString());
                getEmailCountbyEtype(int.Parse(comboBox1.SelectedValue.ToString()));
                //getEmailContent();
                this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        //加载邮件类型
        public void LoadType()
        {
            try
            {
                string sql = "select * from Etypes";
                DataSet ds = conn.ReturnDataSet(sql);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.DisplayMember = "ETypeName";
                comboBox1.ValueMember = "ETid";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        //加载邮件列表
        private void LoadEmailList(string TypeId)
        {
            try
            {
                string sql = "select * from SMails where EType=" + TypeId;
                DataSet ds = conn.ReturnDataSet(sql);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    lstMail.Items.Add(ds.Tables[0].Rows[i][2].ToString());
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SendSucc = 0;
                lstMail.Items.Clear();
                LoadEmailList(comboBox1.SelectedValue.ToString());
                getEmailCountbyEtype(int.Parse(comboBox1.SelectedValue.ToString()));
                this.btnSend.Enabled = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void sMTP账户设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SmtpSet sst = new SmtpSet();
            sst.ShowDialog();
        }

        private void 群组设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MailTypes mts = new MailTypes(this);
            mts.ShowDialog();
        }

        private void 管理地址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetEmailList slt = new SetEmailList();
            slt.ShowDialog();
        }

        //删除选定项
        private void btndelete_Click(object sender, EventArgs e)
        {
            if (lstMail.SelectedIndex >= 0)
            {
                lstMail.Items.RemoveAt(lstMail.SelectedIndex);
                btnSend.Enabled = true;
                comboBox1.Enabled = true;
            }
        }

        private void lstMail_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog opd = new OpenFileDialog();
            opd.ShowDialog();
            if (opd.FileName != "")
            {
                StreamReader SR = new StreamReader(opd.FileName);
                string ss = SR.ReadLine();
                while (SR.EndOfStream == false)
                {
                    lstMail.Items.Add(SR.ReadLine());
                    ss = SR.ReadLine();
                }
            }
        }

        private void 添加邮件地址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEmail add = new AddEmail();
            add.ShowDialog();
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                notifyIcon1.Visible = true;
                this.Hide();
                this.ShowInTaskbar = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.ShowInTaskbar == false)
                notifyIcon1.Visible = true;
            this.ShowInTaskbar = true;
            this.Show();
            this.Activate();
            this.WindowState = FormWindowState.Normal;
        }

        private void btnAddAttment_Click(object sender, EventArgs e)
        {
            OpenFileDialog opd = new OpenFileDialog();
            opd.Filter = "附件|*.txt;*.gif;*.bmp;*.jpg;*.rar";
            opd.ShowDialog();
            txtAtt.Text = opd.FileName;
        }

        /// <summary>
        /// 读取最新发送的邮件内容
        /// </summary>
        private void getEmailContent()
        {
            try
            {
                string sql = "select top 1 * from EContent order by ID desc";
                DataSet ds = conn.ReturnDataSet(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtTitle.Text = ds.Tables[0].Rows[0]["EmailTitle"].ToString();
                    htmlEditor1.Text = ds.Tables[0].Rows[0]["EmailContent"].ToString();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// 获取分组下面的EMAIL总数
        /// </summary>
        /// <param name="etype"></param>
        private void getEmailCountbyEtype(int etype)
        {
            try
            {
                string sql = "select * from SMails where EType=" + etype;
                DataSet ds = conn.ReturnDataSet(sql);
                lbl_emailCount.Text = ds.Tables[0].Rows.Count.ToString();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        /// <summary>
        /// 保存发送的内容到数据库
        /// </summary>
        /// <param name="strContent">邮件内容</param>
        private void InsertEmailContent(string strTitle, string strContent)
        {
            try
            {
                string sql = "insert into EContent (EmailTitle,EmailContent) values ('" + strTitle + "','" + strContent + "')";
                conn.ExeSQL(sql);
            }
            catch
            {

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            PlayExecuteSend();
            timer1.Start();
        }
        /// <summary>
        /// 获取分类总数
        /// </summary>
        /// <returns></returns>
        private int getEtypeCount()
        {
            int etypeCount = 0;
            try
            {
                string sql = "select * from Etypes";
                DataSet ds = conn.ReturnDataSet(sql);
                etypeCount = ds.Tables[0].Rows.Count;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return etypeCount;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox1.Text = "关闭定时发送";
                lblTime.Visible = true;
                lblTime.Text = "(时间间隔：" + (float)timer1.Interval / 60000 + "分钟)";
                timer1.Enabled = true;
                timer1.Start();
            }
            else
            {
                checkBox1.Text = "开启定时发送";
                lblTime.Visible = false;
                timer1.Enabled = false;
                timer1.Stop();
            }
        }
    }
}
