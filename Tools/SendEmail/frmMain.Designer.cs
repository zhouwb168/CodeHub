
namespace SendEmail
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lstMail = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入邮件地址ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sMTP账户设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.群组设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.管理地址ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加邮件地址ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btndelete = new System.Windows.Forms.Button();
            this.htmlEditor1 = new CNPOPSOFT.Controls.HtmlEditor();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnAddAttment = new System.Windows.Forms.Button();
            this.txtAtt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.CHKSave = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lbl_emailCount = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstMail
            // 
            this.lstMail.FormattingEnabled = true;
            this.lstMail.ItemHeight = 12;
            this.lstMail.Location = new System.Drawing.Point(747, 53);
            this.lstMail.Name = "lstMail";
            this.lstMail.Size = new System.Drawing.Size(174, 448);
            this.lstMail.TabIndex = 0;
            this.lstMail.DoubleClick += new System.EventHandler(this.lstMail_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(745, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "收件人列表：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "邮件标题：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "邮件主题：";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(67, 66);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(674, 21);
            this.txtTitle.TabIndex = 4;
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSend.Location = new System.Drawing.Point(67, 489);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(98, 29);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "开始发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label4.Location = new System.Drawing.Point(165, 494);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 14);
            this.label4.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(94, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 12);
            this.label5.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "发送群组：";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(67, 35);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(235, 20);
            this.comboBox1.TabIndex = 10;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.编辑ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(933, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导入邮件地址ToolStripMenuItem,
            this.sMTP账户设置ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 导入邮件地址ToolStripMenuItem
            // 
            this.导入邮件地址ToolStripMenuItem.Name = "导入邮件地址ToolStripMenuItem";
            this.导入邮件地址ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.导入邮件地址ToolStripMenuItem.Text = "导入邮件地址";
            this.导入邮件地址ToolStripMenuItem.Click += new System.EventHandler(this.导入邮件地址ToolStripMenuItem_Click);
            // 
            // sMTP账户设置ToolStripMenuItem
            // 
            this.sMTP账户设置ToolStripMenuItem.Name = "sMTP账户设置ToolStripMenuItem";
            this.sMTP账户设置ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.sMTP账户设置ToolStripMenuItem.Text = "系统设置";
            this.sMTP账户设置ToolStripMenuItem.Click += new System.EventHandler(this.sMTP账户设置ToolStripMenuItem_Click);
            // 
            // 编辑ToolStripMenuItem
            // 
            this.编辑ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.群组设置ToolStripMenuItem,
            this.管理地址ToolStripMenuItem,
            this.添加邮件地址ToolStripMenuItem});
            this.编辑ToolStripMenuItem.Name = "编辑ToolStripMenuItem";
            this.编辑ToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.编辑ToolStripMenuItem.Text = "编辑";
            // 
            // 群组设置ToolStripMenuItem
            // 
            this.群组设置ToolStripMenuItem.Name = "群组设置ToolStripMenuItem";
            this.群组设置ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.群组设置ToolStripMenuItem.Text = "设置邮件群组";
            this.群组设置ToolStripMenuItem.Click += new System.EventHandler(this.群组设置ToolStripMenuItem_Click);
            // 
            // 管理地址ToolStripMenuItem
            // 
            this.管理地址ToolStripMenuItem.Name = "管理地址ToolStripMenuItem";
            this.管理地址ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.管理地址ToolStripMenuItem.Text = "管理邮件地址";
            this.管理地址ToolStripMenuItem.Click += new System.EventHandler(this.管理地址ToolStripMenuItem_Click);
            // 
            // 添加邮件地址ToolStripMenuItem
            // 
            this.添加邮件地址ToolStripMenuItem.Name = "添加邮件地址ToolStripMenuItem";
            this.添加邮件地址ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.添加邮件地址ToolStripMenuItem.Text = "添加邮件地址";
            this.添加邮件地址ToolStripMenuItem.Click += new System.EventHandler(this.添加邮件地址ToolStripMenuItem_Click);
            // 
            // btndelete
            // 
            this.btndelete.Location = new System.Drawing.Point(878, 24);
            this.btndelete.Name = "btndelete";
            this.btndelete.Size = new System.Drawing.Size(43, 23);
            this.btndelete.TabIndex = 12;
            this.btndelete.Text = "删除";
            this.btndelete.UseVisualStyleBackColor = true;
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            // 
            // htmlEditor1
            // 
            this.htmlEditor1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.htmlEditor1.Location = new System.Drawing.Point(67, 102);
            this.htmlEditor1.Name = "htmlEditor1";
            this.htmlEditor1.Size = new System.Drawing.Size(674, 344);
            this.htmlEditor1.TabIndex = 13;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "群发邮件-老周(zhouwb)";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // btnAddAttment
            // 
            this.btnAddAttment.Location = new System.Drawing.Point(372, 455);
            this.btnAddAttment.Name = "btnAddAttment";
            this.btnAddAttment.Size = new System.Drawing.Size(63, 23);
            this.btnAddAttment.TabIndex = 16;
            this.btnAddAttment.Text = "浏览...";
            this.btnAddAttment.UseVisualStyleBackColor = true;
            this.btnAddAttment.Click += new System.EventHandler(this.btnAddAttment_Click);
            // 
            // txtAtt
            // 
            this.txtAtt.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtAtt.Location = new System.Drawing.Point(67, 457);
            this.txtAtt.Name = "txtAtt";
            this.txtAtt.ReadOnly = true;
            this.txtAtt.Size = new System.Drawing.Size(299, 21);
            this.txtAtt.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 462);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "选择附件：";
            // 
            // CHKSave
            // 
            this.CHKSave.AutoSize = true;
            this.CHKSave.Location = new System.Drawing.Point(313, 38);
            this.CHKSave.Name = "CHKSave";
            this.CHKSave.Size = new System.Drawing.Size(120, 16);
            this.CHKSave.TabIndex = 17;
            this.CHKSave.Text = "是否保存邮件内容";
            this.CHKSave.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lbl_emailCount
            // 
            this.lbl_emailCount.AutoSize = true;
            this.lbl_emailCount.Location = new System.Drawing.Point(820, 32);
            this.lbl_emailCount.Name = "lbl_emailCount";
            this.lbl_emailCount.Size = new System.Drawing.Size(0, 12);
            this.lbl_emailCount.TabIndex = 18;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(453, 38);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 16);
            this.checkBox1.TabIndex = 19;
            this.checkBox1.Text = "开启定时发送";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(555, 39);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(0, 12);
            this.lblTime.TabIndex = 20;
            // 
            // timer2
            // 
            this.timer2.Interval = 1000;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 536);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.lbl_emailCount);
            this.Controls.Add(this.CHKSave);
            this.Controls.Add(this.btnAddAttment);
            this.Controls.Add(this.txtAtt);
            this.Controls.Add(this.htmlEditor1);
            this.Controls.Add(this.btndelete);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstMail);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label7);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "群发邮件-老周(zhouwb)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstMail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导入邮件地址ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sMTP账户设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 编辑ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 群组设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 管理地址ToolStripMenuItem;
        private System.Windows.Forms.Button btndelete;
        private System.Windows.Forms.ToolStripMenuItem 添加邮件地址ToolStripMenuItem;
        private CNPOPSOFT.Controls.HtmlEditor htmlEditor1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button btnAddAttment;
        private System.Windows.Forms.TextBox txtAtt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox CHKSave;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lbl_emailCount;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Timer timer2;
    }
}

