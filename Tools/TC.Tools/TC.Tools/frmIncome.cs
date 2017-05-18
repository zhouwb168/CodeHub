using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Maticsoft.DBUtility;

namespace TC.Tools
{
    public partial class frmIncome : Form
    {
        private BLL bll = new BLL();
        public frmIncome()
        {
            InitializeComponent();
        }

        private void btnIncome_Click(object sender, EventArgs e)
        {
            string BorrowID = this.txtBorrowID.Text;
            bool blbidstatus = this.chkbidstatus.Checked;
            if (string.IsNullOrWhiteSpace(BorrowID))
            {
                MessageBox.Show("借款ID不能为空", "提示");
                return;
            }
            //1、取借款信息
            DataTable dtBorrowInfo = DA.getBorrowInfo(BorrowID, "", "");
            if (null == dtBorrowInfo || dtBorrowInfo.Rows.Count == 0)
            {
                MessageBox.Show("借款ID不存在", "提示");
                return;
            }
            string BorrowAmount = dtBorrowInfo.Rows[0]["BorrowAmount"].ToString();

            //2、取投标记录
            DataTable dtBid = DA.getBidRecords("", BorrowID, "");
            if (null == dtBid || dtBid.Rows.Count == 0)
            {
                MessageBox.Show("暂无投标记录", "提示");
                return;
            }
            //更新投标状态
            if (blbidstatus)
            {
                DA.UpdateBidStatus(Convert.ToInt32(BorrowID));
            }
            if (DA.DelIncomePlan(Convert.ToInt32(BorrowID)) > 0)
            {
                foreach (DataRow dr in dtBid.Rows)
                {
                    string BidRecordID = dr["BidRecordID"].ToString();
                    string BidAmount = dr["BidAmount"].ToString();
                    bll.InsertIncomePlan(Convert.ToInt32(BorrowID), Convert.ToInt32(BidRecordID), Convert.ToDecimal(BidAmount), Convert.ToDecimal(BorrowAmount));
                }

                MessageBox.Show("生成收益计划成功", "提示");

                this.dataGv.AutoGenerateColumns = false;
                this.dataGv.DataSource = bll.getInComeInfo(Convert.ToInt32(BorrowID));
            }
        }

        private void frmIncome_Load(object sender, EventArgs e)
        {
            
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string BorrowID = this.txtBorrowID.Text;
            if (string.IsNullOrWhiteSpace(BorrowID))
            {
                MessageBox.Show("借款ID不能为空", "提示");
                return;
            }
            this.dataGv.AutoGenerateColumns = false;
            this.dataGv.DataSource = bll.getInComeInfo(Convert.ToInt32(BorrowID));
        }
    }
}
