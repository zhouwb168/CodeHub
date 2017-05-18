using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Xml;

namespace SendEmail
{
    public partial class SmtpSet : Form
    {
        INIClass iniCls = new INIClass(Application.StartupPath+"\\congfig.ini");
        public SmtpSet()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAddr.Text.Trim() != "" && txtPwd.Text.Trim() != "" && txtSmtp.Text.Trim() != "" && txtPort.Text.Trim() != "")
                {
                    iniCls.IniWriteValue("SMTP","txtAddr", txtAddr.Text.Trim());
                    iniCls.IniWriteValue("SMTP", "txtPwd", txtPwd.Text.Trim());
                    iniCls.IniWriteValue("SMTP", "txtSmtp", txtSmtp.Text.Trim());
                    iniCls.IniWriteValue("SMTP", "txtPort", txtPort.Text.Trim());
                    MessageBox.Show("修改成功！");
                    this.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void SmtpSet_Load(object sender, EventArgs e)
        {
            try
            {
                string MailAddr = iniCls.IniReadValue("SMTP","txtAddr");
                string MailPwd = iniCls.IniReadValue("SMTP","txtPwd");
                string MailSmtp = iniCls.IniReadValue("SMTP","txtSmtp");
                string MailPort = iniCls.IniReadValue("SMTP","txtPort");

                txtAddr.Text = MailAddr;
                txtPwd.Text = MailPwd;
                txtSmtp.Text = MailSmtp;
                txtPort.Text = MailPort;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

    }
}
