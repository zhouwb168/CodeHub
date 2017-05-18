using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Browser
{
    public partial class MainBrowser : Form
    {
        MainBrowser main;
        List<string> list = new List<string>();
        int count = -1;
        public MainBrowser(string name = "http://113.12.66.76:85/yitiji/index.php", int w = 1200, int h = 800)
        {
            InitializeComponent();
            main = this;
            main.Width = w;
            main.Height = h;
            //list.Add(name);
            //count++;
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.AllowWebBrowserDrop = false;
            this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
            main = this;
            curUrl = "";
            this.webBrowser1.Navigate(name);

        }
        bool isRun;
        private void btnGo_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void Run()
        {
            if (this.txtUrl.Text == "")
            {
                MessageBox.Show("网址为空！");
                txtUrl.Focus();
                return;
            }


            if (btnGo.Text == "转到")
            {
                lblms.Text = "正在打开网页！";
                linkList = new List<Link>();
                btnGo.Text = "取消";
                isRun = true;
                curUrl = "";

                this.webBrowser1.Navigate(this.txtUrl.Text);
                main.Text = this.webBrowser1.DocumentTitle;

                //count++;
                //button1.Enabled = true;
                lblms.Text = "完成";
                btnGo.Text = "转到";

            }
        }
        List<Link> linkList = new List<Link>();

        string curUrl;
        bool btn1 = false;
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
            {
                return;
            }

            if (e.Url.ToString() != webBrowser1.Url.ToString())
            {
                return;
            }

            if (curUrl == "")
            {
                Link newLink = new Link();
                newLink.Content = "起始页";
                newLink.Size = this.webBrowser1.DocumentText.Length;
                newLink.Status = Link.LinkStatus.Complete;
                newLink.Name = this.webBrowser1.DocumentTitle;
                main = this;
                main.Text = this.webBrowser1.DocumentTitle;
                newLink.Url = e.Url.ToString();
                txtUrl.Text = e.Url.ToString();
                if (!btn1)
                {
                    list.Add(txtUrl.Text);
                    count++;
                }
                else
                {
                    btn1 = false;
                }
                button1.Enabled = true;
            }


        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {

            WebBrowser web = (WebBrowser)sender;
            //list.Add(web.StatusText);
            //count++;
            if (tcym.Checked)
            {
                main = this;
                MainBrowser m = new MainBrowser(web.StatusText, main.Width, main.Height);
                m.Show();
            }
            else
            {
                button1.Enabled = true;
                txtUrl.Text = web.StatusText;
                web.Navigate(web.StatusText);
                main = this;
                main.Text = web.DocumentTitle;
            }

            e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (list.Count > 1)
                {
                    btn1 = true;
                    button2.Enabled = true;
                    webBrowser1.Navigate(list[--count]);
                }
            }
            catch
            {
                webBrowser1.Navigate(list[0]);
                list.Clear();
                count = -1;
                button1.Enabled = false;
                button2.Enabled = false;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if ((list.Count - 2) > count)
                {
                    btn1 = true;
                    button1.Enabled = true;
                    webBrowser1.Navigate(list[++count]);
                }
            }
            catch
            {
                webBrowser1.Navigate(list[0]);
                list.Clear();
                count = -1;
                button1.Enabled = false;
                button2.Enabled = false;
            }
        }

        private void txtUrl_KeyDown(object sender, KeyEventArgs e)
        {
            Run();
        }

    }


    public class Link
    {
        public Link()
        {

        }

        public Link(string url, string content)
        {
            Url = url;
            Name = "";
            Content = content;
            Status = LinkStatus.Init;
            Size = 0;
        }

        public string Url;
        public string Name;
        public string Content;
        public LinkStatus Status;
        public long Size;
        public enum LinkStatus
        {
            Init,
            Loading,
            Complete,
            Error,
        }
    }
}
