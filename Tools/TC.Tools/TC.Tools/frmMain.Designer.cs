using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;            //线程类
using System.Collections;
using System.Net;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Drawing.Text;
namespace TC.Tools
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox url;
        private System.Windows.Forms.TextBox usernum;
        private System.Windows.Forms.Button button2;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.usernum = new System.Windows.Forms.TextBox();
            this.url = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lvDataList = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // usernum
            // 
            this.usernum.Location = new System.Drawing.Point(304, 326);
            this.usernum.Name = "usernum";
            this.usernum.Size = new System.Drawing.Size(48, 21);
            this.usernum.TabIndex = 5;
            // 
            // url
            // 
            this.url.Location = new System.Drawing.Point(48, 326);
            this.url.Name = "url";
            this.url.Size = new System.Drawing.Size(168, 21);
            this.url.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 328);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "URL:";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(232, 328);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "虚拟人数：";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(368, 324);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 24);
            this.button1.TabIndex = 6;
            this.button1.Text = "确定";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(440, 324);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 25);
            this.button2.TabIndex = 7;
            this.button2.Text = "中断";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lvDataList
            // 
            this.lvDataList.Location = new System.Drawing.Point(8, 8);
            this.lvDataList.Name = "lvDataList";
            this.lvDataList.Size = new System.Drawing.Size(496, 295);
            this.lvDataList.TabIndex = 8;
            this.lvDataList.UseCompatibleStateImageBehavior = false;
            // 
            // frmMain
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(514, 370);
            this.Controls.Add(this.lvDataList);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.usernum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.url);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "站点测试";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private ListView lvDataList;
    }
}