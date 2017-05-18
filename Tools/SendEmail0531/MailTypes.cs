using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SendEmail
{
    public partial class MailTypes : Form
    {
        ConnDbForAcccess conn = new ConnDbForAcccess();
        frmMain parentForm = new frmMain();
        public MailTypes(frmMain form1)
        {
            parentForm = form1;
            InitializeComponent();
        }

        private void MailTypes_Load(object sender, EventArgs e)
        {
            LoadType();
        }

        //加载邮件类型
        private void LoadType()
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

        //检查群组是否还有邮件地址
        private bool CheckMail(int Etype)
        {
            string sql = "select SMid from SMails where EType=" + Etype;
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
        
        //删除
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int etype = int.Parse(comboBox1.SelectedValue.ToString());
                if (CheckMail(etype) == false)
                {
                    if (MessageBox.Show("确实要删除选中的行吗？   注意：删除后就无法恢复了！", "警告",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string sql = "delete from Etypes where ETid=" + etype;
                        if (conn.ExeSQL(sql) == true)
                        {
                            LoadType();
                            MessageBox.Show("删除成功！");
                            parentForm.LoadType();//更新父窗体中的群组分类
                        }
                        else { MessageBox.Show("删除发生异常，请重试！"); }
                    }
                }
                else
                {
                    MessageBox.Show("请先删除群组中的邮件地址!");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        
        //添加
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string etypename = comboBox1.Text.Trim();
                string sql = "insert into Etypes (ETypeName) values ('" + etypename + "')";
                if (conn.ExeSQL(sql) == true)
                {
                    LoadType();
                    MessageBox.Show("添加成功！");
                    parentForm.LoadType();//更新父窗体中的群组分类
                }
                else { MessageBox.Show("添加发生异常，请重试！"); }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
