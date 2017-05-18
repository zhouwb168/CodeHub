using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;

namespace TC.Tools
{
    public partial class frmGpsOffSet : Form
    {
        private GPSBLL gpsbll = new GPSBLL();
        private Thread thread = null;

        public string MapType { get; set; }
        public string Url { get; set; }
        public string Accuracy { get; set; }
        public string MinLng { get; set; }
        public string MaxLng { get; set; }
        public string MinLat { get; set; }
        public string MaxLat { get; set; }

        //最后一次读取的经纬度
        public double lastLng { get; set; }
        public double lastLat { get; set; }

        public frmGpsOffSet()
        {
            InitializeComponent();
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            MapType = this.cbxMapType.Text;
            if (string.IsNullOrWhiteSpace(MapType))
            {
                MessageBox.Show("请选择类型");
                return;
            }
            Url = getUrl(MapType);
            if (string.IsNullOrWhiteSpace(Url))
            {
                MessageBox.Show("Url不能为空");
                return;
            }
            Accuracy = this.cbxAccuracy.Text;
            if (string.IsNullOrWhiteSpace(Accuracy))
            {
                MessageBox.Show("请选择精度");
                return;
            }
            MinLng = this.txtMinLng.Text;
            MaxLng = this.txtMaxLng.Text;
            if (string.IsNullOrWhiteSpace(MinLng) || string.IsNullOrWhiteSpace(MaxLng))
            {
                MessageBox.Show("经度不能为空");
                return;
            }
            MinLat = this.txtMinLat.Text;
            MaxLat = this.txtMaxLat.Text;
            if (string.IsNullOrWhiteSpace(MinLat) || string.IsNullOrWhiteSpace(MaxLat))
            {
                MessageBox.Show("纬度不能为空");
                return;
            }
            if (null == thread)
            {
                thread = new Thread(new ThreadStart(ThreadProc));
                thread.IsBackground = true;
                thread.Start();
            }
        }

        #region 查询百度纠偏坐标
        /// <summary>
        /// 获取URL
        /// </summary>
        /// <param name="MapType"></param>
        /// <returns></returns>
        private string getUrl(string MapType)
        {
            string url = string.Empty;
            switch (MapType)
            {
                case "百度":
                    url = "http://api.map.baidu.com/ag/coord/convert?from=0&to=4&x={0}&y={1}";
                    break;
            }
            return url;
        }

        ///<summary>
        ///计算百度地图坐标
        ///</summary>
        ///<param name="Lng">原始经度</param>
        ///<param name="Lat">原始纬度</param>
        ///<returns>百度地图纠偏后的坐标</returns>
        public Point GetBaiduPosOff(double lng, double lat, string url)
        {
            Point pos = new Point(0, 0);//还原到度供接口使用
            try
            {
                url = string.Format(url, lng, lat);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 5000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                byte[] bytes = new byte[1024];
                int n = stream.Read(bytes, 0, 1024);
                response.Close();
                if (n < 2)
                {
                    DispMsg(string.Format("返回长度不正确经度:{0},纬度:{1}", lng, lat));
                }
                else
                {
                    string s = System.Text.Encoding.UTF8.GetString(bytes).Substring(1, n - 2).Replace("\"", "");
                    foreach (string team in s.Split(','))
                    {

                        string[] infos = team.Split(':');
                        if (infos.Length < 2)
                        {
                            DispMsg(string.Format("格式不正确:{0},纬度:{1}", lng, lat));
                            break;
                        }

                        string strValue = infos[1];
                        byte[] outputb;
                        switch (infos[0])
                        {

                            case "error":
                                if (strValue != "0")
                                {
                                    DispMsg(string.Format("返回了错误号,经度:{0},纬度:{1},错误号:{2}", lng, lat, strValue));
                                }
                                break;
                            case "x":
                                outputb = Convert.FromBase64String(strValue);
                                strValue = Encoding.Default.GetString(outputb);
                                pos.X = (int)(double.Parse(strValue) * 1000000);
                                break;
                            case "y":
                                outputb = Convert.FromBase64String(strValue);
                                strValue = Encoding.Default.GetString(outputb);
                                pos.Y = (int)(double.Parse(strValue) * 1000000);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DispMsg(string.Format("错误,经度:{0},纬度:{1},错误信息:{2}", lng, lat, ex.Message));
            }
            return pos;
        }
        #endregion

        #region 获取纠偏经纬度数据
        /// <summary>
        /// 获取纠偏经纬度数据
        /// </summary>
        /// <param name="MinLng"></param>
        /// <param name="MaxLng"></param>
        /// <param name="MinLat"></param>
        /// <param name="MaxLat"></param>
        /// <param name="Accuracy"></param>
        /// <param name="Url"></param>
        private void getOffset(double MinLng, double MaxLng, double MinLat, double MaxLat, double Accuracy, string Url)
        {
            int LngOffset = 0, LatOffset = 0, step = 1000000;
            for (double i = MinLng; i <= MaxLng; i = i + Accuracy)
            {
                for (double j = MinLat; j <= MaxLat; j = j + Accuracy)
                {
                    Point pt = GetBaiduPosOff(i, j, Url);
                    int Lng = (int)(i * step);
                    int Lat = (int)(j * step);
                    int isok = 0;
                    if (pt.X > 0 && pt.Y > 0)
                    {
                        LngOffset = pt.X - Lng;
                        LatOffset = pt.Y - Lat;
                        isok = 1;
                    }
                    lastLng = i;
                    lastLat = j;
                    gpsbll.SaveGpsOffset(Lng, Lat, LngOffset, LatOffset, isok);
                    DispMsg(string.Format("成功,经度:{0},纬度:{1},经度偏移:{2},纬度偏移:{3},状态:{4}", Lng, Lat, LngOffset, LatOffset, isok));
                    Thread.Sleep(500);
                }
            }
        }
        #endregion

        public void ThreadProc()
        {
            getOffset(double.Parse(MinLng), double.Parse(MaxLng), double.Parse(MinLat), double.Parse(MaxLat), double.Parse(Accuracy), Url);
        }

        /// <summary>
        /// 定义一个代理
        /// </summary>
        /// <param name="MSG"></param>
        private delegate void DispMSGDelegate(string MSG);

        /// <summary>
        /// 定义一个函数，用于向窗体上的ListView控件添加内容
        /// </summary>
        /// <param name="strMsg"></param>
        private void DispMsg(string strMsg)
        {
            if (this.lvDataList.InvokeRequired == false)
            {
                //直接将内容添加到窗体的控件上
                ListViewItem lvi = new ListViewItem();
                lvi.SubItems[0].Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lvi.SubItems.Add(strMsg);
                this.lvDataList.Items.Insert(0, lvi);
            }
            else
            {
                DispMSGDelegate DMSGD = new DispMSGDelegate(DispMsg);
                this.lvDataList.Invoke(DMSGD, strMsg);
            }
        }

        private void frmGpsOffSet_Load(object sender, EventArgs e)
        {
            string lastlng = Util.Readini("GPS", "LastLng");
            string lastlat = Util.Readini("GPS", "LastLat");
            if (lastlng != "" && lastlat != "")
            {
                this.txtMinLng.Text = lastlng;
                this.txtMinLat.Text = lastlat;
            }
            else
            {
                //广西的经纬度范围
                this.txtMinLng.Text = "104.26";
                this.txtMinLat.Text = "20.54";
            }

            this.txtMaxLng.Text = "112.04";
            this.txtMaxLat.Text = "26.24";

            this.lvDataList.GridLines = true; //显示表格线
            this.lvDataList.View = View.Details;//显示表格细节
            this.lvDataList.LabelEdit = false; //是否可编辑,ListView只可编辑第一列。
            this.lvDataList.Scrollable = true;//有滚动条
            this.lvDataList.HeaderStyle = ColumnHeaderStyle.Nonclickable;//对表头进行设置
            this.lvDataList.FullRowSelect = true;//是否可以选择行
            //添加表头
            this.lvDataList.Columns.Add("date", "时间", 150);
            this.lvDataList.Columns.Add("msg", "消息", 500);
        }

        private void frmGpsOffSet_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != thread)
            {
                thread.Abort();
                thread = null;
            }
            Util.Writeini("GPS", "LastLng", lastLng.ToString());
            Util.Writeini("GPS", "LastLat", lastLat.ToString());
        }
    }
}
