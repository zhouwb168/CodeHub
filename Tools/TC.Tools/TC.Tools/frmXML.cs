using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using TC.Tools.Model;

namespace TC.Tools
{
    public partial class frmXML : Form
    {
        private XmlBLL bll = new XmlBLL();

        public frmXML()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.btnQuery.Enabled = false;
            this.dataGv.AutoGenerateColumns = false;
            DataTable dt = bll.getUserInfo();
            this.dataGv.DataSource = dt;
            this.btnQuery.Enabled = true;
            this.lblmsg.Text = "记录数为：" + dt.Rows.Count + " 条";
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            this.lblmsg.Text = "";
            int Success = 0, Fail = 0, Exption = 0;
            this.btnReset.Enabled = false;
            DataTable dtTemp = bll.getUserInfo();
            foreach (DataRow dr in dtTemp.Rows)
            {
                int UserID = Convert.ToInt32(dr["UserID"].ToString());
                string CardNo = getCardNo(UserID);
                try
                {
                    if (bll.UpdateCardNo(UserID, CardNo) > 0)
                        Success++;
                    else
                        Fail++;
                }
                catch
                {
                    Exption++;
                }
            }
            this.lblmsg.Text = "记录总数：" + dtTemp.Rows.Count + " 条，成功：" + Success + " 条，失败：" + Fail + " 条，异常：" + Exption + " 条。";
            this.btnReset.Enabled = true;
            this.dataGv.AutoGenerateColumns = false;
            this.dataGv.DataSource = bll.getUserInfo();
        }

        /// <summary>
        /// 获取证件号
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        private string getCardNo(int UserID)
        {
            string CardNo = string.Empty;
            string path = this.txtFloder.Text;
            if (path == "") return "";
            foreach (string file in Directory.GetFiles(path))
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(file);
                //获取根节点<rootName>的所有子节点 
                XmlNodeList myNodeList = xmldoc.SelectSingleNode("LOG").ChildNodes;
                //遍历<根节点>的所有子节点 
                foreach (XmlNode myXmlNode in myNodeList)
                {
                    XmlNodeList subNodeList = myXmlNode.ChildNodes;
                    foreach (XmlNode subXmlNode in subNodeList)
                    {
                        if (subXmlNode.Name == "LOGINFO")
                        {
                            string retStr = subXmlNode.InnerText.Trim();
                            if (retStr.IndexOf("CardNo") == -1) continue;
                            CardNo = retStr.ToJosnDatabase(UserID, "CardNo");
                            if (!string.IsNullOrWhiteSpace(CardNo)) break;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(CardNo)) break;
                }
                if (!string.IsNullOrWhiteSpace(CardNo)) break;
            }
            return CardNo;
        }
    }
}
