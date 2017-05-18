namespace TC.Tools
{
    partial class frmGpsOffSet
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMaxLat = new System.Windows.Forms.TextBox();
            this.txtMaxLng = new System.Windows.Forms.TextBox();
            this.btnParse = new System.Windows.Forms.Button();
            this.lblAccuracy = new System.Windows.Forms.Label();
            this.cbxAccuracy = new System.Windows.Forms.ComboBox();
            this.txtMinLat = new System.Windows.Forms.TextBox();
            this.lblLat = new System.Windows.Forms.Label();
            this.lblLng = new System.Windows.Forms.Label();
            this.txtMinLng = new System.Windows.Forms.TextBox();
            this.lblMapType = new System.Windows.Forms.Label();
            this.cbxMapType = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvDataList = new System.Windows.Forms.ListView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMaxLat);
            this.groupBox1.Controls.Add(this.txtMaxLng);
            this.groupBox1.Controls.Add(this.btnParse);
            this.groupBox1.Controls.Add(this.lblAccuracy);
            this.groupBox1.Controls.Add(this.cbxAccuracy);
            this.groupBox1.Controls.Add(this.txtMinLat);
            this.groupBox1.Controls.Add(this.lblLat);
            this.groupBox1.Controls.Add(this.lblLng);
            this.groupBox1.Controls.Add(this.txtMinLng);
            this.groupBox1.Controls.Add(this.lblMapType);
            this.groupBox1.Controls.Add(this.cbxMapType);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(620, 84);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // txtMaxLat
            // 
            this.txtMaxLat.Location = new System.Drawing.Point(359, 48);
            this.txtMaxLat.Name = "txtMaxLat";
            this.txtMaxLat.Size = new System.Drawing.Size(61, 21);
            this.txtMaxLat.TabIndex = 10;
            // 
            // txtMaxLng
            // 
            this.txtMaxLng.Location = new System.Drawing.Point(150, 48);
            this.txtMaxLng.Name = "txtMaxLng";
            this.txtMaxLng.Size = new System.Drawing.Size(61, 21);
            this.txtMaxLng.TabIndex = 9;
            // 
            // btnParse
            // 
            this.btnParse.Location = new System.Drawing.Point(479, 23);
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new System.Drawing.Size(101, 41);
            this.btnParse.TabIndex = 8;
            this.btnParse.Text = "获取偏移数据";
            this.btnParse.UseVisualStyleBackColor = true;
            this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
            // 
            // lblAccuracy
            // 
            this.lblAccuracy.AutoSize = true;
            this.lblAccuracy.Location = new System.Drawing.Point(237, 22);
            this.lblAccuracy.Name = "lblAccuracy";
            this.lblAccuracy.Size = new System.Drawing.Size(41, 12);
            this.lblAccuracy.TabIndex = 7;
            this.lblAccuracy.Text = "精度：";
            // 
            // cbxAccuracy
            // 
            this.cbxAccuracy.FormattingEnabled = true;
            this.cbxAccuracy.Items.AddRange(new object[] {
            "1",
            "0.1",
            "0.01"});
            this.cbxAccuracy.Location = new System.Drawing.Point(284, 18);
            this.cbxAccuracy.Name = "cbxAccuracy";
            this.cbxAccuracy.Size = new System.Drawing.Size(136, 20);
            this.cbxAccuracy.TabIndex = 6;
            // 
            // txtMinLat
            // 
            this.txtMinLat.Location = new System.Drawing.Point(284, 48);
            this.txtMinLat.Name = "txtMinLat";
            this.txtMinLat.Size = new System.Drawing.Size(61, 21);
            this.txtMinLat.TabIndex = 5;
            // 
            // lblLat
            // 
            this.lblLat.AutoSize = true;
            this.lblLat.Location = new System.Drawing.Point(238, 52);
            this.lblLat.Name = "lblLat";
            this.lblLat.Size = new System.Drawing.Size(41, 12);
            this.lblLat.TabIndex = 4;
            this.lblLat.Text = "纬度：";
            // 
            // lblLng
            // 
            this.lblLng.AutoSize = true;
            this.lblLng.Location = new System.Drawing.Point(28, 52);
            this.lblLng.Name = "lblLng";
            this.lblLng.Size = new System.Drawing.Size(41, 12);
            this.lblLng.TabIndex = 3;
            this.lblLng.Text = "经度：";
            // 
            // txtMinLng
            // 
            this.txtMinLng.Location = new System.Drawing.Point(75, 48);
            this.txtMinLng.Name = "txtMinLng";
            this.txtMinLng.Size = new System.Drawing.Size(61, 21);
            this.txtMinLng.TabIndex = 2;
            // 
            // lblMapType
            // 
            this.lblMapType.AutoSize = true;
            this.lblMapType.Location = new System.Drawing.Point(28, 23);
            this.lblMapType.Name = "lblMapType";
            this.lblMapType.Size = new System.Drawing.Size(41, 12);
            this.lblMapType.TabIndex = 1;
            this.lblMapType.Text = "类型：";
            // 
            // cbxMapType
            // 
            this.cbxMapType.FormattingEnabled = true;
            this.cbxMapType.Items.AddRange(new object[] {
            "百度",
            "谷歌",
            "高德"});
            this.cbxMapType.Location = new System.Drawing.Point(75, 19);
            this.cbxMapType.Name = "cbxMapType";
            this.cbxMapType.Size = new System.Drawing.Size(136, 20);
            this.cbxMapType.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvDataList);
            this.groupBox2.Location = new System.Drawing.Point(13, 102);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(619, 253);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据";
            // 
            // lvDataList
            // 
            this.lvDataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDataList.Location = new System.Drawing.Point(3, 17);
            this.lvDataList.Name = "lvDataList";
            this.lvDataList.Size = new System.Drawing.Size(613, 233);
            this.lvDataList.TabIndex = 0;
            this.lvDataList.UseCompatibleStateImageBehavior = false;
            // 
            // frmGpsOffSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 367);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "frmGpsOffSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "构建GPS偏移数据";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGpsOffSet_FormClosing);
            this.Load += new System.EventHandler(this.frmGpsOffSet_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbxMapType;
        private System.Windows.Forms.ListView lvDataList;
        private System.Windows.Forms.Label lblMapType;
        private System.Windows.Forms.Label lblLng;
        private System.Windows.Forms.TextBox txtMinLng;
        private System.Windows.Forms.Label lblAccuracy;
        private System.Windows.Forms.ComboBox cbxAccuracy;
        private System.Windows.Forms.TextBox txtMinLat;
        private System.Windows.Forms.Label lblLat;
        private System.Windows.Forms.Button btnParse;
        private System.Windows.Forms.TextBox txtMaxLat;
        private System.Windows.Forms.TextBox txtMaxLng;
    }
}