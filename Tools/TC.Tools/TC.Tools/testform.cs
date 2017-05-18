using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;

namespace TC.Tools
{
    public partial class testform : Form
    {
        public testform()
        {
            InitializeComponent();
        }

        private void testform_Load(object sender, EventArgs e)
        {
            //申请还款日期
            DateTime EyRepayTime = Convert.ToDateTime("2017-02-09 00:00:00.000");
            DateTime PrevRepayTime = Convert.ToDateTime("2017-02-08 00:00:00.000");

            int interval = new TimeSpan(EyRepayTime.Ticks - PrevRepayTime.Ticks).Days;

            MessageBox.Show(this, interval.ToString());
            double monthRate = 8 / 100.00 / 12.00;//月利率
            double BorrowAmount = 1000000;
            decimal RepayInterest = (decimal)(BorrowAmount * monthRate);

            decimal PenaltyAmt = RepayInterest / 2;
            decimal aaa = (RepayInterest / 30) * 17;
            decimal tt = PenaltyAmt + aaa;
            string sTime = System.DateTime.Now.ToString("yyyy-MM-dd mm:hh:ss:fff");
            label1.Text = sTime;
            string str;
            for (int i = 0; i < 10; i++)
            {
                str = i.ToString();
                Thread.Sleep(200);
            }
            string eTime = System.DateTime.Now.ToString("yyyy-MM-dd mm:hh:ss:fff");
            label2.Text = eTime;

            string sTime1 = System.DateTime.Now.ToString("yyyy-MM-dd mm:hh:ss:fff");
            label3.Text = sTime1;
            //并行计算
            Parallel.For(0, 10, (i) =>
            {
                str = i.ToString();
                Thread.Sleep(200);
            });
            string eTime1 = System.DateTime.Now.ToString("yyyy-MM-dd mm:hh:ss:fff");
            label4.Text = eTime1;
        }

        /// <summary>
        /// 获取枚举对象 
        /// 调用示例:ConvertEnum(((Enum)new baseenum.IsOnline()).GetType());
        /// </summary>
        /// <param name="enumObj"></param>
        /// <returns></returns>
        public static object ConvertEnum(Type enumObj)
        {
            Type objType = System.Reflection.Assembly.Load("TC.Tools").GetType("TC.Tools.BaseEnum");
            object instance = Activator.CreateInstance(objType, null);
            Type t = instance.GetType().GetNestedType("IsOnline");
            foreach (string name in Enum.GetNames(t))
            {
                string EnumKey = name;
                int EnumValue = (int)Enum.Parse(t, name);
            }
            return null;
        }
    }
    public class BaseEnum
    {
        //是否在线
        public enum IsOnline
        {
            离线 = 0,
            在线 = 1
        }
    }

}
