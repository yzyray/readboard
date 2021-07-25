namespace readboard
{
    partial class Form4
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form4));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBpc = new System.Windows.Forms.TextBox();
            this.txtWpc = new System.Windows.Forms.TextBox();
            this.txtWzb = new System.Windows.Forms.TextBox();
            this.txtBzb = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.chkMag = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.chkDoubleCheck = new System.Windows.Forms.CheckBox();
            this.chkAutoMin = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.rdoNormalScale = new System.Windows.Forms.RadioButton();
            this.rdoAdvanceScale = new System.Windows.Forms.RadioButton();
            this.scaleGroup = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.lblSyncInterval = new System.Windows.Forms.Label();
            this.txtSyncInterval = new System.Windows.Forms.TextBox();
            this.scaleGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "黑色偏差(0-255)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "白色偏差(0-255)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(196, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "黑色占比(0-100)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(196, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "白色占比(0-100)";
            // 
            // txtBpc
            // 
            this.txtBpc.Location = new System.Drawing.Point(109, 107);
            this.txtBpc.Name = "txtBpc";
            this.txtBpc.Size = new System.Drawing.Size(79, 21);
            this.txtBpc.TabIndex = 4;
            this.txtBpc.TextChanged += new System.EventHandler(this.txtBpc_TextChanged);
            // 
            // txtWpc
            // 
            this.txtWpc.Location = new System.Drawing.Point(109, 133);
            this.txtWpc.Name = "txtWpc";
            this.txtWpc.Size = new System.Drawing.Size(79, 21);
            this.txtWpc.TabIndex = 5;
            // 
            // txtWzb
            // 
            this.txtWzb.Location = new System.Drawing.Point(297, 133);
            this.txtWzb.Name = "txtWzb";
            this.txtWzb.Size = new System.Drawing.Size(79, 21);
            this.txtWzb.TabIndex = 6;
            // 
            // txtBzb
            // 
            this.txtBzb.Location = new System.Drawing.Point(297, 107);
            this.txtBzb.Name = "txtBzb";
            this.txtBzb.Size = new System.Drawing.Size(79, 21);
            this.txtBzb.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 12);
            this.label5.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 197);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(287, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "如某种颜色棋子识别丢失,可尝试增大偏差或降低占比";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(228, 220);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(309, 220);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // chkMag
            // 
            this.chkMag.AutoSize = true;
            this.chkMag.Location = new System.Drawing.Point(12, 12);
            this.chkMag.Name = "chkMag";
            this.chkMag.Size = new System.Drawing.Size(84, 16);
            this.chkMag.TabIndex = 12;
            this.chkMag.Text = "使用放大镜";
            this.chkMag.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 179);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(287, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "如某种颜色棋子识别过多,可尝试降低偏差或增大占比";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 159);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(143, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "注:所有参数都必须为整数";
            // 
            // chkDoubleCheck
            // 
            this.chkDoubleCheck.AutoSize = true;
            this.chkDoubleCheck.Location = new System.Drawing.Point(109, 12);
            this.chkDoubleCheck.Name = "chkDoubleCheck";
            this.chkDoubleCheck.Size = new System.Drawing.Size(276, 16);
            this.chkDoubleCheck.TabIndex = 15;
            this.chkDoubleCheck.Text = "前台同步时双击(SABAKI/GOGUI需要去掉此选项)";
            this.chkDoubleCheck.UseVisualStyleBackColor = true;
            // 
            // chkAutoMin
            // 
            this.chkAutoMin.AutoSize = true;
            this.chkAutoMin.Location = new System.Drawing.Point(184, 33);
            this.chkAutoMin.Name = "chkAutoMin";
            this.chkAutoMin.Size = new System.Drawing.Size(198, 16);
            this.chkAutoMin.TabIndex = 16;
            this.chkAutoMin.Text = "一键同步/持续同步后自动最小化";
            this.chkAutoMin.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 85);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(305, 12);
            this.label9.TabIndex = 17;
            this.label9.Text = "以下选项只对 其他(前台),其他(后台) 类型的同步生效:";
            // 
            // rdoNormalScale
            // 
            this.rdoNormalScale.AutoSize = true;
            this.rdoNormalScale.Location = new System.Drawing.Point(3, 9);
            this.rdoNormalScale.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rdoNormalScale.Name = "rdoNormalScale";
            this.rdoNormalScale.Size = new System.Drawing.Size(71, 16);
            this.rdoNormalScale.TabIndex = 18;
            this.rdoNormalScale.TabStop = true;
            this.rdoNormalScale.Text = "普通缩放";
            this.rdoNormalScale.UseVisualStyleBackColor = true;
            // 
            // rdoAdvanceScale
            // 
            this.rdoAdvanceScale.AutoSize = true;
            this.rdoAdvanceScale.Location = new System.Drawing.Point(108, 9);
            this.rdoAdvanceScale.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rdoAdvanceScale.Name = "rdoAdvanceScale";
            this.rdoAdvanceScale.Size = new System.Drawing.Size(71, 16);
            this.rdoAdvanceScale.TabIndex = 19;
            this.rdoAdvanceScale.TabStop = true;
            this.rdoAdvanceScale.Text = "高级缩放";
            this.rdoAdvanceScale.UseVisualStyleBackColor = true;
            // 
            // scaleGroup
            // 
            this.scaleGroup.Controls.Add(this.rdoNormalScale);
            this.scaleGroup.Controls.Add(this.rdoAdvanceScale);
            this.scaleGroup.Location = new System.Drawing.Point(9, 47);
            this.scaleGroup.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.scaleGroup.Name = "scaleGroup";
            this.scaleGroup.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.scaleGroup.Size = new System.Drawing.Size(221, 29);
            this.scaleGroup.TabIndex = 20;
            this.scaleGroup.TabStop = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(235, 53);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(140, 23);
            this.button3.TabIndex = 21;
            this.button3.Text = "如何判断我的缩放类型";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(8, 219);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(105, 23);
            this.button4.TabIndex = 22;
            this.button4.Text = "恢复默认设置";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // lblSyncInterval
            // 
            this.lblSyncInterval.AutoSize = true;
            this.lblSyncInterval.Location = new System.Drawing.Point(10, 34);
            this.lblSyncInterval.Name = "lblSyncInterval";
            this.lblSyncInterval.Size = new System.Drawing.Size(101, 12);
            this.lblSyncInterval.TabIndex = 23;
            this.lblSyncInterval.Text = "同步时间间隔(ms)";
            // 
            // txtSyncInterval
            // 
            this.txtSyncInterval.Location = new System.Drawing.Point(116, 29);
            this.txtSyncInterval.Name = "txtSyncInterval";
            this.txtSyncInterval.Size = new System.Drawing.Size(56, 21);
            this.txtSyncInterval.TabIndex = 24;
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 253);
            this.Controls.Add(this.txtSyncInterval);
            this.Controls.Add(this.lblSyncInterval);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.scaleGroup);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.chkAutoMin);
            this.Controls.Add(this.chkDoubleCheck);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.chkMag);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBzb);
            this.Controls.Add(this.txtWzb);
            this.Controls.Add(this.txtWpc);
            this.Controls.Add(this.txtBpc);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form4";
            this.Text = "参数设置";
            this.TopMost = true;
            this.scaleGroup.ResumeLayout(false);
            this.scaleGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBpc;
        private System.Windows.Forms.TextBox txtWpc;
        private System.Windows.Forms.TextBox txtWzb;
        private System.Windows.Forms.TextBox txtBzb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox chkMag;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkDoubleCheck;
        private System.Windows.Forms.CheckBox chkAutoMin;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton rdoNormalScale;
        private System.Windows.Forms.RadioButton rdoAdvanceScale;
        private System.Windows.Forms.GroupBox scaleGroup;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label lblSyncInterval;
        private System.Windows.Forms.TextBox txtSyncInterval;
    }
}