using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SendEmail
{
    public partial class AddEmail : Form
    {
        ConnDbForAcccess conn = new ConnDbForAcccess();
        public AddEmail()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int Etype = int.Parse(comboBox1.SelectedValue.ToString());
            if (txtEmail.Text == "")
            {
                MessageBox.Show("邮件地址不能为空");
                return;
            }
            if (!IsEmail(txtEmail.Text.Trim()))
            {
                MessageBox.Show("邮件格式不对");
                return;
            }
            if (CheakEmail(txtEmail.Text) == false)
            {
                string sql = "insert into SMails (EType,SMail) values (" + Etype + ",'" + txtEmail.Text.Trim() + "')";
                if (conn.ExeSQL(sql))
                {
                    MessageBox.Show("添加成功");
                    txtEmail.Text = "";
                    return;
                }
                else
                {
                    MessageBox.Show("添加失败");
                    return;
                }
            }
            else
            {
                MessageBox.Show("该邮件已经存在");
                return;
            }
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

        private void AddEmail_Load(object sender, EventArgs e)
        {
            LoadType();
        }

        //加载邮件类型
        private void LoadType()
        {
            string sql = "select * from Etypes";
            DataSet ds = conn.ReturnDataSet(sql);
            comboBox1.DataSource = ds.Tables[0];
            comboBox1.DisplayMember = "ETypeName";
            comboBox1.ValueMember = "ETid";
        }
    }

}
