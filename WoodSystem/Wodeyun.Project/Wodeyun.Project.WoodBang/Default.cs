using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using System.Configuration;
using System.IO;

using Wodeyun.Project.WoodBang.Modules;
using System.Timers;

namespace Wodeyun.Project.WoodBang
{
    public partial class Default : Form
    {
        private bool startWork = false; // 是否正在操作执行定时任务
        private int num = 0;

        private static LogHelper helper = new LogHelper(ConfigurationManager.AppSettings["LogFilePath"].ToString());

        SynchronousData synchronousModule = new SynchronousData();

        System.Timers.Timer time = new System.Timers.Timer();

        public Default()
        {
            InitializeComponent();
        }

        private void Default_Load(object sender, EventArgs e)
        {
            int timespan = int.Parse(ConfigurationManager.AppSettings["TimeSpan"].ToString());
            time.Elapsed += new ElapsedEventHandler(TimeEvent);
            time.Interval = 1000 * timespan;
            helper.WriteLine("系统启动成功...");
            time.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要手动更新吗？", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (this.startWork)
                {
                    MessageBox.Show("同步器正忙，请过1分钟后再试", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                try
                {
                    int rows = synchronousModule.Delete(this.dtpStartDate.Value);
                    string eorrMsg = rows > 0 ? "更新成功" : "更新失败，找不到可更新的数据，请查询要更新的过磅日期是否正确后再次重试一次";
                    MessageBox.Show(eorrMsg, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                catch (Exception ex)
                {
                    helper.WriteLine("更新数据异常：" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 定时执行任务把旧地磅系统的数据库文件复制备份到指定路径，
        /// 并且同步上次首磅的回皮数据、本次首磅、本次回皮的数据 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void TimeEvent(object source, ElapsedEventArgs e)
        {
            if (num == 0) helper.WriteLine("同步数据开始....");
            num++;
            try
            {
                startWork = true;
                CopyDataFile();
                if (num == 12)
                {
                    /* 每12小时就同步近3个月以来的回皮数据一次 */
                    num = 0;
                    synchronousModule.UpdateDataAgainForChange();
                    helper.WriteLine("每隔12小时同步成功。");
                }
                else
                {
                    synchronousModule.UpdateDataForSecondWeigh();
                    synchronousModule.SynchronousDataFromBangToWood();
                    helper.WriteLine("定时同步成功。");
                }
            }
            catch (Exception ex)
            {
                helper.WriteLine("同步数据异常：" + ex.Message);
            }
            startWork = false;
        }

        /// <summary>
        /// 把旧地磅系统的数据库文件复制备份到指定路径
        /// </summary>
        void CopyDataFile()
        {
            helper.WriteLine("正在复制文件...");
            try
            {
                int dt1 = System.DateTime.Now.Second;
                string dataFileSourcePath = ConfigurationManager.AppSettings["SourceFilePath"].ToString();
                string dataTargetPathPath = ConfigurationManager.AppSettings["TargetFilePath"].ToString();
                File.Copy(dataFileSourcePath, dataTargetPathPath, true);
                int dt2 = System.DateTime.Now.Second;
                helper.WriteLine("复制文件完成.总共用时：" + (dt2 - dt1) + "s");
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                helper.WriteLine("复制文件：" + msg);
            }
        }

        private void Default_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("您确认要退出吗？", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                this.Dispose();
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
