namespace TC.Tools
{
    partial class frmIncome
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
            this.btnIncome = new System.Windows.Forms.Button();
            this.lblBorrowID = new System.Windows.Forms.Label();
            this.txtBorrowID = new System.Windows.Forms.TextBox();
            this.btnQuery = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGv = new System.Windows.Forms.DataGridView();
            this.BidRecordID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PeriodNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IncomeAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IncomeInterest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IncomeTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RealIncomeTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IncomeStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkbidstatus = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGv)).BeginInit();
            this.SuspendLayout();
            // 
            // btnIncome
            // 
            this.btnIncome.Location = new System.Drawing.Point(510, 64);
            this.btnIncome.Name = "btnIncome";
            this.btnIncome.Size = new System.Drawing.Size(113, 23);
            this.btnIncome.TabIndex = 0;
            this.btnIncome.Text = "生成收益计划";
            this.btnIncome.UseVisualStyleBackColor = true;
            this.btnIncome.Click += new System.EventHandler(this.btnIncome_Click);
            // 
            // lblBorrowID
            // 
            this.lblBorrowID.AutoSize = true;
            this.lblBorrowID.Location = new System.Drawing.Point(22, 69);
            this.lblBorrowID.Name = "lblBorrowID";
            this.lblBorrowID.Size = new System.Drawing.Size(53, 12);
            this.lblBorrowID.TabIndex = 1;
            this.lblBorrowID.Text = "借款ID：";
            // 
            // txtBorrowID
            // 
            this.txtBorrowID.Location = new System.Drawing.Point(82, 65);
            this.txtBorrowID.Name = "txtBorrowID";
            this.txtBorrowID.Size = new System.Drawing.Size(115, 21);
            this.txtBorrowID.TabIndex = 2;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(380, 64);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(113, 23);
            this.btnQuery.TabIndex = 0;
            this.btnQuery.Text = "查询收益计划";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkbidstatus);
            this.groupBox1.Location = new System.Drawing.Point(12, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(714, 79);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "收益计划操作";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGv);
            this.groupBox2.Location = new System.Drawing.Point(13, 129);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(716, 272);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "收益列表";
            // 
            // dataGv
            // 
            this.dataGv.AllowUserToAddRows = false;
            this.dataGv.AllowUserToDeleteRows = false;
            this.dataGv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BidRecordID,
            this.UserName,
            this.PeriodNo,
            this.IncomeAmount,
            this.IncomeInterest,
            this.IncomeTime,
            this.RealIncomeTime,
            this.IncomeStatus});
            this.dataGv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGv.Location = new System.Drawing.Point(3, 17);
            this.dataGv.Name = "dataGv";
            this.dataGv.ReadOnly = true;
            this.dataGv.RowTemplate.Height = 23;
            this.dataGv.Size = new System.Drawing.Size(710, 252);
            this.dataGv.TabIndex = 0;
            // 
            // BidRecordID
            // 
            this.BidRecordID.DataPropertyName = "BidRecordID";
            this.BidRecordID.HeaderText = "投标ID";
            this.BidRecordID.Name = "BidRecordID";
            this.BidRecordID.ReadOnly = true;
            this.BidRecordID.Width = 72;
            // 
            // UserName
            // 
            this.UserName.DataPropertyName = "UserName";
            this.UserName.HeaderText = "收益人";
            this.UserName.Name = "UserName";
            this.UserName.ReadOnly = true;
            this.UserName.Width = 72;
            // 
            // PeriodNo
            // 
            this.PeriodNo.DataPropertyName = "PeriodNo";
            this.PeriodNo.HeaderText = "期数";
            this.PeriodNo.Name = "PeriodNo";
            this.PeriodNo.ReadOnly = true;
            this.PeriodNo.Width = 60;
            // 
            // IncomeAmount
            // 
            this.IncomeAmount.DataPropertyName = "IncomeAmount";
            this.IncomeAmount.HeaderText = "收益本金";
            this.IncomeAmount.Name = "IncomeAmount";
            this.IncomeAmount.ReadOnly = true;
            this.IncomeAmount.Width = 90;
            // 
            // IncomeInterest
            // 
            this.IncomeInterest.DataPropertyName = "IncomeInterest";
            this.IncomeInterest.HeaderText = "收益利息";
            this.IncomeInterest.Name = "IncomeInterest";
            this.IncomeInterest.ReadOnly = true;
            this.IncomeInterest.Width = 90;
            // 
            // IncomeTime
            // 
            this.IncomeTime.DataPropertyName = "IncomeTime";
            this.IncomeTime.HeaderText = "收益时间";
            this.IncomeTime.Name = "IncomeTime";
            this.IncomeTime.ReadOnly = true;
            this.IncomeTime.Width = 90;
            // 
            // RealIncomeTime
            // 
            this.RealIncomeTime.DataPropertyName = "RealIncomeTime";
            this.RealIncomeTime.HeaderText = "实际收益时间";
            this.RealIncomeTime.Name = "RealIncomeTime";
            this.RealIncomeTime.ReadOnly = true;
            this.RealIncomeTime.Width = 120;
            // 
            // IncomeStatus
            // 
            this.IncomeStatus.DataPropertyName = "IncomeStatus";
            this.IncomeStatus.HeaderText = "收益状态";
            this.IncomeStatus.Name = "IncomeStatus";
            this.IncomeStatus.ReadOnly = true;
            this.IncomeStatus.Width = 80;
            // 
            // chkbidstatus
            // 
            this.chkbidstatus.AutoSize = true;
            this.chkbidstatus.Location = new System.Drawing.Point(206, 39);
            this.chkbidstatus.Name = "chkbidstatus";
            this.chkbidstatus.Size = new System.Drawing.Size(144, 16);
            this.chkbidstatus.TabIndex = 0;
            this.chkbidstatus.Text = "更新投标状态(回收中)";
            this.chkbidstatus.UseVisualStyleBackColor = true;
            // 
            // frmIncome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 413);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txtBorrowID);
            this.Controls.Add(this.lblBorrowID);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.btnIncome);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "frmIncome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "收益计划";
            this.Load += new System.EventHandler(this.frmIncome_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnIncome;
        private System.Windows.Forms.Label lblBorrowID;
        private System.Windows.Forms.TextBox txtBorrowID;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGv;
        private System.Windows.Forms.DataGridViewTextBoxColumn BidRecordID;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PeriodNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn IncomeAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn IncomeInterest;
        private System.Windows.Forms.DataGridViewTextBoxColumn IncomeTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn RealIncomeTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn IncomeStatus;
        private System.Windows.Forms.CheckBox chkbidstatus;
    }
}

