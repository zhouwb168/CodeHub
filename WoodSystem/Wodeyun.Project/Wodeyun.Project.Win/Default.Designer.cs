namespace Wodeyun.Project.Win
{
    partial class Default
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnPmyk = new System.Windows.Forms.Button();
            this.btnZhhwy = new System.Windows.Forms.Button();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSingle = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblCommand = new System.Windows.Forms.Label();
            this.lstMessage = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btnPmyk);
            this.groupBox1.Controls.Add(this.btnZhhwy);
            this.groupBox1.Controls.Add(this.txtServer);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnSingle);
            this.groupBox1.Location = new System.Drawing.Point(24, 144);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(807, 78);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "连接";
            // 
            // btnPmyk
            // 
            this.btnPmyk.ForeColor = System.Drawing.Color.Red;
            this.btnPmyk.Location = new System.Drawing.Point(511, 33);
            this.btnPmyk.Name = "btnPmyk";
            this.btnPmyk.Size = new System.Drawing.Size(134, 23);
            this.btnPmyk.TabIndex = 10;
            this.btnPmyk.Text = "连接报警器";
            this.btnPmyk.UseVisualStyleBackColor = true;
            this.btnPmyk.Click += new System.EventHandler(this.btnPmyk_Click);
            // 
            // btnZhhwy
            // 
            this.btnZhhwy.ForeColor = System.Drawing.Color.Red;
            this.btnZhhwy.Location = new System.Drawing.Point(649, 33);
            this.btnZhhwy.Name = "btnZhhwy";
            this.btnZhhwy.Size = new System.Drawing.Size(134, 23);
            this.btnZhhwy.TabIndex = 12;
            this.btnZhhwy.Text = "连接短信猫";
            this.btnZhhwy.UseVisualStyleBackColor = true;
            this.btnZhhwy.Click += new System.EventHandler(this.btnZhhwy_Click);
            // 
            // txtServer
            // 
            this.txtServer.BackColor = System.Drawing.Color.LightGray;
            this.txtServer.Location = new System.Drawing.Point(97, 33);
            this.txtServer.Name = "txtServer";
            this.txtServer.ReadOnly = true;
            this.txtServer.Size = new System.Drawing.Size(134, 21);
            this.txtServer.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "服务器：";
            // 
            // btnSingle
            // 
            this.btnSingle.ForeColor = System.Drawing.Color.Red;
            this.btnSingle.Location = new System.Drawing.Point(235, 33);
            this.btnSingle.Name = "btnSingle";
            this.btnSingle.Size = new System.Drawing.Size(134, 23);
            this.btnSingle.TabIndex = 1;
            this.btnSingle.Text = "连接单返回服务器";
            this.btnSingle.UseVisualStyleBackColor = true;
            this.btnSingle.Click += new System.EventHandler(this.btnSingle_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.lblCommand);
            this.groupBox2.Controls.Add(this.lstMessage);
            this.groupBox2.Location = new System.Drawing.Point(24, 241);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(807, 333);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据";
            // 
            // lblCommand
            // 
            this.lblCommand.AutoSize = true;
            this.lblCommand.Location = new System.Drawing.Point(38, 25);
            this.lblCommand.Name = "lblCommand";
            this.lblCommand.Size = new System.Drawing.Size(65, 12);
            this.lblCommand.TabIndex = 6;
            this.lblCommand.Text = "短信列表：";
            // 
            // lstMessage
            // 
            this.lstMessage.FormattingEnabled = true;
            this.lstMessage.ItemHeight = 12;
            this.lstMessage.Location = new System.Drawing.Point(40, 42);
            this.lstMessage.Name = "lstMessage";
            this.lstMessage.Size = new System.Drawing.Size(743, 268);
            this.lstMessage.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("黑体", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(50, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(417, 40);
            this.label7.TabIndex = 8;
            this.label7.Text = "百色丰林智慧工厂系统";
            // 
            // Default
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Wodeyun.Project.Win.Properties.Resources.Ground;
            this.ClientSize = new System.Drawing.Size(854, 598);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Default";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "百色丰林智慧工厂系统";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Default_FormClosing);
            this.Load += new System.EventHandler(this.Default_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSingle;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblCommand;
        private System.Windows.Forms.ListBox lstMessage;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnZhhwy;
        private System.Windows.Forms.Button btnPmyk;
    }
}

