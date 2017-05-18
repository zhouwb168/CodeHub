using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.IO;
using System.Text;

namespace Wodeyun.Project.Win
{
    static class Program
    {
        public static int Account;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += Application_ThreadException; 

            DialogResult result = (new Logon()).ShowDialog();

            if (result == DialogResult.OK)
                Application.Run(new Default());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            string msg = "";
            string traMessage = "";
            while (ex != null && ex.InnerException != null) ex = ex.InnerException;
            msg = ex.Message;
            traMessage = ex.StackTrace;
            DateTime nowDateTime = DateTime.Now;
            StringBuilder errBuild = new StringBuilder();
            errBuild.Append("#############################################################################################");
            errBuild.Append(Environment.NewLine);
            errBuild.Append(Environment.NewLine);
            errBuild.AppendFormat("时间：{0}", nowDateTime.ToString()); 
            errBuild.Append(Environment.NewLine);
            errBuild.AppendFormat("Message：{0}", msg); 
            errBuild.Append(Environment.NewLine);
            errBuild.AppendFormat("StackTrace：{0}", traMessage);
            errBuild.Append(Environment.NewLine);
            errBuild.Append(Environment.NewLine);
            errBuild.Append("#############################################################################################");

            string filePath = string.Format("{0}\\{1}_{2}_{3}.txt", Application.StartupPath, nowDateTime.Year, nowDateTime.Month, nowDateTime.Day);
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                sw.Write(msg);
                sw.Close();
            }

            MessageBox.Show("短信报备系统已停止工作，请查看异常日志分析原因", "异常提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
