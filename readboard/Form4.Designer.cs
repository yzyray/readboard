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
            this.chkVerifyMove = new System.Windows.Forms.CheckBox();
            this.chkAutoMin = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.lblSyncInterval = new System.Windows.Forms.Label();
            this.txtSyncInterval = new System.Windows.Forms.TextBox();
            this.lblGrayOffset = new System.Windows.Forms.Label();
            this.txtGrayOffset = new System.Windows.Forms.TextBox();
            this.chkEnhanceScreen = new System.Windows.Forms.CheckBox();
            this.chkPonder = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 106);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "黑色偏差(0-255)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 139);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "白色偏差(0-255)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(261, 106);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "黑色占比(0-100)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(261, 139);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "白色占比(0-100)";
            // 
            // txtBpc
            // 
            this.txtBpc.Location = new System.Drawing.Point(145, 100);
            this.txtBpc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBpc.Name = "txtBpc";
            this.txtBpc.Size = new System.Drawing.Size(104, 25);
            this.txtBpc.TabIndex = 4;
            // 
            // txtWpc
            // 
            this.txtWpc.Location = new System.Drawing.Point(145, 131);
            this.txtWpc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtWpc.Name = "txtWpc";
            this.txtWpc.Size = new System.Drawing.Size(104, 25);
            this.txtWpc.TabIndex = 5;
            // 
            // txtWzb
            // 
            this.txtWzb.Location = new System.Drawing.Point(396, 131);
            this.txtWzb.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtWzb.Name = "txtWzb";
            this.txtWzb.Size = new System.Drawing.Size(104, 25);
            this.txtWzb.TabIndex = 6;
            // 
            // txtBzb
            // 
            this.txtBzb.Location = new System.Drawing.Point(396, 100);
            this.txtBzb.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBzb.Name = "txtBzb";
            this.txtBzb.Size = new System.Drawing.Size(104, 25);
            this.txtBzb.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 81);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 15);
            this.label5.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 227);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(360, 15);
            this.label6.TabIndex = 9;
            this.label6.Text = "如某种颜色棋子识别丢失,可尝试增大偏差或降低占比";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(309, 254);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 29);
            this.button1.TabIndex = 10;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(417, 254);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 29);
            this.button2.TabIndex = 11;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // chkMag
            // 
            this.chkMag.AutoSize = true;
            this.chkMag.Location = new System.Drawing.Point(16, 41);
            this.chkMag.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkMag.Name = "chkMag";
            this.chkMag.Size = new System.Drawing.Size(104, 19);
            this.chkMag.TabIndex = 12;
            this.chkMag.Text = "使用放大镜";
            this.chkMag.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 202);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(360, 15);
            this.label7.TabIndex = 13;
            this.label7.Text = "如某种颜色棋子识别过多,可尝试降低偏差或增大占比";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 171);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(180, 15);
            this.label8.TabIndex = 14;
            this.label8.Text = "注:所有参数都必须为整数";
            // 
            // chkVerifyMove
            // 
            this.chkVerifyMove.AutoSize = true;
            this.chkVerifyMove.Location = new System.Drawing.Point(291, 15);
            this.chkVerifyMove.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkVerifyMove.Name = "chkVerifyMove";
            this.chkVerifyMove.Size = new System.Drawing.Size(164, 19);
            this.chkVerifyMove.TabIndex = 15;
            this.chkVerifyMove.Text = "验证落子以确保成功";
            this.chkVerifyMove.UseVisualStyleBackColor = true;
            // 
            // chkAutoMin
            // 
            this.chkAutoMin.AutoSize = true;
            this.chkAutoMin.Location = new System.Drawing.Point(16, 15);
            this.chkAutoMin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkAutoMin.Name = "chkAutoMin";
            this.chkAutoMin.Size = new System.Drawing.Size(149, 19);
            this.chkAutoMin.TabIndex = 16;
            this.chkAutoMin.Text = "同步后自动最小化";
            this.chkAutoMin.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 74);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(386, 15);
            this.label9.TabIndex = 17;
            this.label9.Text = "以下选项只对 其他(前台),其他(后台) 类型的同步生效:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(16, 253);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(140, 29);
            this.button4.TabIndex = 22;
            this.button4.Text = "恢复默认设置";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // lblSyncInterval
            // 
            this.lblSyncInterval.AutoSize = true;
            this.lblSyncInterval.Location = new System.Drawing.Point(288, 42);
            this.lblSyncInterval.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSyncInterval.Name = "lblSyncInterval";
            this.lblSyncInterval.Size = new System.Drawing.Size(137, 15);
            this.lblSyncInterval.TabIndex = 23;
            this.lblSyncInterval.Text = "同步时间间隔(ms):";
            // 
            // txtSyncInterval
            // 
            this.txtSyncInterval.Location = new System.Drawing.Point(436, 36);
            this.txtSyncInterval.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSyncInterval.Name = "txtSyncInterval";
            this.txtSyncInterval.Size = new System.Drawing.Size(73, 25);
            this.txtSyncInterval.TabIndex = 24;
            // 
            // lblGrayOffset
            // 
            this.lblGrayOffset.AutoSize = true;
            this.lblGrayOffset.Location = new System.Drawing.Point(261, 171);
            this.lblGrayOffset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGrayOffset.Name = "lblGrayOffset";
            this.lblGrayOffset.Size = new System.Drawing.Size(123, 15);
            this.lblGrayOffset.TabIndex = 25;
            this.lblGrayOffset.Text = "灰度偏色(0-255)";
            // 
            // txtGrayOffset
            // 
            this.txtGrayOffset.Location = new System.Drawing.Point(396, 165);
            this.txtGrayOffset.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGrayOffset.Name = "txtGrayOffset";
            this.txtGrayOffset.Size = new System.Drawing.Size(103, 25);
            this.txtGrayOffset.TabIndex = 26;
            // 
            // chkEnhanceScreen
            // 
            this.chkEnhanceScreen.AutoSize = true;
            this.chkEnhanceScreen.Location = new System.Drawing.Point(173, 41);
            this.chkEnhanceScreen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkEnhanceScreen.Name = "chkEnhanceScreen";
            this.chkEnhanceScreen.Size = new System.Drawing.Size(89, 19);
            this.chkEnhanceScreen.TabIndex = 27;
            this.chkEnhanceScreen.Text = "强化截图";
            this.chkEnhanceScreen.UseVisualStyleBackColor = true;
            // 
            // chkPonder
            // 
            this.chkPonder.AutoSize = true;
            this.chkPonder.Location = new System.Drawing.Point(173, 15);
            this.chkPonder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkPonder.Name = "chkPonder";
            this.chkPonder.Size = new System.Drawing.Size(89, 19);
            this.chkPonder.TabIndex = 28;
            this.chkPonder.Text = "后台思考";
            this.chkPonder.UseVisualStyleBackColor = true;
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 289);
            this.Controls.Add(this.chkPonder);
            this.Controls.Add(this.chkEnhanceScreen);
            this.Controls.Add(this.txtGrayOffset);
            this.Controls.Add(this.lblGrayOffset);
            this.Controls.Add(this.txtSyncInterval);
            this.Controls.Add(this.lblSyncInterval);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.chkAutoMin);
            this.Controls.Add(this.chkVerifyMove);
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
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form4";
            this.Text = "参数设置";
            this.TopMost = true;
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
        private System.Windows.Forms.CheckBox chkVerifyMove;
        private System.Windows.Forms.CheckBox chkAutoMin;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label lblSyncInterval;
        private System.Windows.Forms.TextBox txtSyncInterval;
        private System.Windows.Forms.Label lblGrayOffset;
        private System.Windows.Forms.TextBox txtGrayOffset;
        private System.Windows.Forms.CheckBox chkEnhanceScreen;
        private System.Windows.Forms.CheckBox chkPonder;
    }
}