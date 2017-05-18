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

namespace SendEmail
{
    public partial class ImportAdd : Form
    {
        ConnDbForAcccess conn = new ConnDbForAcccess();
        int Etype = 0;
        string s = "";
        int LineNum =0;
        public ImportAdd()
        {
            InitializeComponent();
        }

        //邮件格式验证
        public static bool IsEmail(string strEmail)
        {
            if (!Regex.IsMatch(strEmail.Trim(), @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                LineNum = 0;
                OpenFileDialog opd = new OpenFileDialog();
                opd.Filter = "文本文件|*.txt";
                opd.ShowDialog();
                textBox1.Text = opd.FileName;

                Thread th = new Thread(new ThreadStart(Imptxt));
                th.IsBackground = true;
                th.Start();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        //private

        private void Imptxt()
        {
            if (textBox1.Text.Trim() != "")
            {
                FileStream f = new FileStream(textBox1.Text.Trim(), FileMode.OpenOrCreate);
                StreamReader reader = new StreamReader(f);
                MethodInvoker Ins = new MethodInvoker(IntEtype);
                this.BeginInvoke(Ins);
                s = reader.ReadLine();
                while (s != null)
                {
                    if (s.IndexOf("@") == -1) 
                        s = s + "@qq.com";
                    if (IsEmail(s) == true)
                    {
                        if (CheakEmail(s) == false)
                        {
                            string sql = "insert into SMails (EType,SMail) values (" + Etype + ",'" + s + "')";
                            conn.ExeSQL(sql);
                            LineNum++;
                            MethodInvoker Inss = new MethodInvoker(Lbltxt);
                            this.BeginInvoke(Inss);
                        }
                    }
                    s = reader.ReadLine();
                }
                reader.Close();
                f.Close();
                MethodInvoker Ins2 = new MethodInvoker(Lbltxt2);
                this.BeginInvoke(Ins2);
            }
        }

        private void IntEtype()
        {
            Etype = int.Parse(comboBox1.SelectedValue.ToString());
        }

        private void Lbltxt()
        {
            label4.Text = "正在导入：" + s + ",请稍后操作...";
        }

        private void Lbltxt2()
        {
            label4.Text = "导入完成，共导入：" + LineNum + "条";
        }

        private bool CheakEmail(string email)
        {
            string sql = "select SMid from SMails where SMail='" + email + "'";
            int sqlNum = conn.ReturnSqlResultCount(sql);
            if (sqlNum > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ImportAdd_Load(object sender, EventArgs e)
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
    }
}
