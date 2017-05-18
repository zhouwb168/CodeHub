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
    public partial class SetEmailList : Form
    {
        ConnDbForAcccess conn = new ConnDbForAcccess();
        public SetEmailList()
        {
            InitializeComponent();
        }

        private void SetEmailList_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.AutoGenerateColumns = false;
                LoadType();
                LoadEmailList(int.Parse(comboBox1.SelectedValue.ToString()));
                this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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
        
        //加载邮件列表
        private void LoadEmailList(int TypeId)
        {
            string sql = "select * from SMails where EType=" + TypeId;
            DataSet ds = conn.ReturnDataSet(sql);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadEmailList(int.Parse(comboBox1.SelectedValue.ToString()));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("确实要删除选中的行吗？   注意：删除后就无法恢复了！", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value != null) //判断该行的复选框是否存在
                        {
                            if (dataGridView1.Rows[i].Cells[0].Value.ToString() == "True") //判断该复选框是否被选中
                            {
                                DeleMail(int.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString()));
                            }
                        }
                    }
                    LoadEmailList(int.Parse(comboBox1.SelectedValue.ToString()));
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        
        private void DeleMail(int SMid)
        {
            string sql = "delete from SMails where SMid=" + SMid;
            if (conn.ExeSQL(sql) == true)
            {
                //MessageBox.Show("删除成功！");
            }
            else { MessageBox.Show("删除发生异常，请重试！"); }
        }

        //全选
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (button2.Text == "选")
                {
                    for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                    {
                        this.dataGridView1["Column3", i].Value = true;
                        button2.Text = "反";
                    }
                }
                else
                {
                    for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                    {
                        this.dataGridView1["Column3", i].Value = false;
                        button2.Text = "选";
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
