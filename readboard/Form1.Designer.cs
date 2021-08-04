namespace readboard
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnKeepSync = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoFore = new System.Windows.Forms.RadioButton();
            this.rdoSina = new System.Windows.Forms.RadioButton();
            this.rdoBack = new System.Windows.Forms.RadioButton();
            this.rdoTygem = new System.Windows.Forms.RadioButton();
            this.rdoFox = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtBoardHeight = new System.Windows.Forms.TextBox();
            this.txtBoardWidth = new System.Windows.Forms.TextBox();
            this.rdoOtherBoard = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.rdo9x9 = new System.Windows.Forms.RadioButton();
            this.rdo13x13 = new System.Windows.Forms.RadioButton();
            this.rdo19x19 = new System.Windows.Forms.RadioButton();
            this.radioWhite = new System.Windows.Forms.RadioButton();
            this.radioBlack = new System.Windows.Forms.RadioButton();
            this.chkBothSync = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkAutoPlay = new System.Windows.Forms.CheckBox();
            this.btnSettings = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.chkShowInBoard = new System.Windows.Forms.CheckBox();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(155, 125);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(83, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "框选棋盘";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("宋体", 9F);
            this.button3.Location = new System.Drawing.Point(7, 125);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(145, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "选择棋盘(点击棋盘内部)";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(117, 152);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(82, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "单次同步";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnKeepSync
            // 
            this.btnKeepSync.Location = new System.Drawing.Point(7, 152);
            this.btnKeepSync.Name = "btnKeepSync";
            this.btnKeepSync.Size = new System.Drawing.Size(107, 23);
            this.btnKeepSync.TabIndex = 4;
            this.btnKeepSync.Text = "持续同步(200ms)";
            this.btnKeepSync.UseVisualStyleBackColor = true;
            this.btnKeepSync.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(372, 152);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(80, 23);
            this.button6.TabIndex = 5;
            this.button6.Text = "清空棋盘";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoFore);
            this.groupBox1.Controls.Add(this.rdoSina);
            this.groupBox1.Controls.Add(this.rdoBack);
            this.groupBox1.Controls.Add(this.rdoTygem);
            this.groupBox1.Controls.Add(this.rdoFox);
            this.groupBox1.Location = new System.Drawing.Point(7, -2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(326, 32);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            // 
            // rdoFore
            // 
            this.rdoFore.AutoSize = true;
            this.rdoFore.Location = new System.Drawing.Point(239, 11);
            this.rdoFore.Name = "rdoFore";
            this.rdoFore.Size = new System.Drawing.Size(83, 16);
            this.rdoFore.TabIndex = 14;
            this.rdoFore.TabStop = true;
            this.rdoFore.Text = "其他(前台)";
            this.rdoFore.UseVisualStyleBackColor = true;
            this.rdoFore.CheckedChanged += new System.EventHandler(this.rdoqiantai_CheckedChanged);
            // 
            // rdoSina
            // 
            this.rdoSina.AutoSize = true;
            this.rdoSina.Location = new System.Drawing.Point(104, 11);
            this.rdoSina.Name = "rdoSina";
            this.rdoSina.Size = new System.Drawing.Size(47, 16);
            this.rdoSina.TabIndex = 13;
            this.rdoSina.TabStop = true;
            this.rdoSina.Text = "新浪";
            this.rdoSina.UseVisualStyleBackColor = true;
            this.rdoSina.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // rdoBack
            // 
            this.rdoBack.AutoSize = true;
            this.rdoBack.Location = new System.Drawing.Point(154, 11);
            this.rdoBack.Name = "rdoBack";
            this.rdoBack.Size = new System.Drawing.Size(83, 16);
            this.rdoBack.TabIndex = 12;
            this.rdoBack.TabStop = true;
            this.rdoBack.Text = "其他(后台)";
            this.rdoBack.UseVisualStyleBackColor = true;
            this.rdoBack.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // rdoTygem
            // 
            this.rdoTygem.AutoSize = true;
            this.rdoTygem.Location = new System.Drawing.Point(54, 11);
            this.rdoTygem.Name = "rdoTygem";
            this.rdoTygem.Size = new System.Drawing.Size(47, 16);
            this.rdoTygem.TabIndex = 11;
            this.rdoTygem.TabStop = true;
            this.rdoTygem.Text = "弈城";
            this.rdoTygem.UseVisualStyleBackColor = true;
            this.rdoTygem.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // rdoFox
            // 
            this.rdoFox.AutoSize = true;
            this.rdoFox.Location = new System.Drawing.Point(5, 11);
            this.rdoFox.Name = "rdoFox";
            this.rdoFox.Size = new System.Drawing.Size(47, 16);
            this.rdoFox.TabIndex = 10;
            this.rdoFox.TabStop = true;
            this.rdoFox.Text = "野狐";
            this.rdoFox.UseVisualStyleBackColor = true;
            this.rdoFox.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtBoardHeight);
            this.groupBox2.Controls.Add(this.txtBoardWidth);
            this.groupBox2.Controls.Add(this.rdoOtherBoard);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.rdo9x9);
            this.groupBox2.Controls.Add(this.rdo13x13);
            this.groupBox2.Controls.Add(this.rdo19x19);
            this.groupBox2.Location = new System.Drawing.Point(87, 29);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(246, 31);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(213, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 12);
            this.label6.TabIndex = 36;
            this.label6.Text = "*";
            // 
            // txtBoardHeight
            // 
            this.txtBoardHeight.BackColor = System.Drawing.SystemColors.Menu;
            this.txtBoardHeight.Location = new System.Drawing.Point(224, 8);
            this.txtBoardHeight.Name = "txtBoardHeight";
            this.txtBoardHeight.Size = new System.Drawing.Size(21, 21);
            this.txtBoardHeight.TabIndex = 35;
            this.txtBoardHeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_KeyPressHeight);
            this.txtBoardHeight.Leave += new System.EventHandler(this.parseHeight);
            // 
            // txtBoardWidth
            // 
            this.txtBoardWidth.BackColor = System.Drawing.SystemColors.Menu;
            this.txtBoardWidth.Location = new System.Drawing.Point(191, 8);
            this.txtBoardWidth.Name = "txtBoardWidth";
            this.txtBoardWidth.Size = new System.Drawing.Size(21, 21);
            this.txtBoardWidth.TabIndex = 34;
            this.txtBoardWidth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_KeyPressWidth);
            this.txtBoardWidth.Leave += new System.EventHandler(this.parseWidth);
            // 
            // rdoOtherBoard
            // 
            this.rdoOtherBoard.AutoSize = true;
            this.rdoOtherBoard.Location = new System.Drawing.Point(176, 12);
            this.rdoOtherBoard.Name = "rdoOtherBoard";
            this.rdoOtherBoard.Size = new System.Drawing.Size(14, 13);
            this.rdoOtherBoard.TabIndex = 33;
            this.rdoOtherBoard.TabStop = true;
            this.rdoOtherBoard.UseVisualStyleBackColor = true;
            this.rdoOtherBoard.CheckedChanged += new System.EventHandler(this.radioButton8_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "棋盘:";
            // 
            // rdo9x9
            // 
            this.rdo9x9.AutoSize = true;
            this.rdo9x9.Location = new System.Drawing.Point(138, 11);
            this.rdo9x9.Name = "rdo9x9";
            this.rdo9x9.Size = new System.Drawing.Size(41, 16);
            this.rdo9x9.TabIndex = 16;
            this.rdo9x9.TabStop = true;
            this.rdo9x9.Text = "9*9";
            this.rdo9x9.UseVisualStyleBackColor = true;
            this.rdo9x9.CheckedChanged += new System.EventHandler(this.radioButton7_CheckedChanged);
            // 
            // rdo13x13
            // 
            this.rdo13x13.AutoSize = true;
            this.rdo13x13.Location = new System.Drawing.Point(88, 11);
            this.rdo13x13.Name = "rdo13x13";
            this.rdo13x13.Size = new System.Drawing.Size(53, 16);
            this.rdo13x13.TabIndex = 15;
            this.rdo13x13.TabStop = true;
            this.rdo13x13.Text = "13*13";
            this.rdo13x13.UseVisualStyleBackColor = true;
            this.rdo13x13.CheckedChanged += new System.EventHandler(this.radioButton6_CheckedChanged);
            // 
            // rdo19x19
            // 
            this.rdo19x19.AutoSize = true;
            this.rdo19x19.Location = new System.Drawing.Point(38, 11);
            this.rdo19x19.Name = "rdo19x19";
            this.rdo19x19.Size = new System.Drawing.Size(53, 16);
            this.rdo19x19.TabIndex = 14;
            this.rdo19x19.TabStop = true;
            this.rdo19x19.Text = "19*19";
            this.rdo19x19.UseVisualStyleBackColor = true;
            this.rdo19x19.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // radioWhite
            // 
            this.radioWhite.AutoSize = true;
            this.radioWhite.Location = new System.Drawing.Point(84, 38);
            this.radioWhite.Name = "radioWhite";
            this.radioWhite.Size = new System.Drawing.Size(47, 16);
            this.radioWhite.TabIndex = 23;
            this.radioWhite.TabStop = true;
            this.radioWhite.Text = "执白";
            this.radioWhite.UseVisualStyleBackColor = true;
            this.radioWhite.CheckedChanged += new System.EventHandler(this.radioWhite_CheckedChanged);
            // 
            // radioBlack
            // 
            this.radioBlack.AutoSize = true;
            this.radioBlack.Location = new System.Drawing.Point(84, 13);
            this.radioBlack.Name = "radioBlack";
            this.radioBlack.Size = new System.Drawing.Size(47, 16);
            this.radioBlack.TabIndex = 22;
            this.radioBlack.TabStop = true;
            this.radioBlack.Text = "执黑";
            this.radioBlack.UseVisualStyleBackColor = true;
            this.radioBlack.CheckedChanged += new System.EventHandler(this.radioBlack_CheckedChanged);
            // 
            // chkBothSync
            // 
            this.chkBothSync.AutoSize = true;
            this.chkBothSync.Location = new System.Drawing.Point(6, 12);
            this.chkBothSync.Name = "chkBothSync";
            this.chkBothSync.Size = new System.Drawing.Size(72, 16);
            this.chkBothSync.TabIndex = 20;
            this.chkBothSync.Text = "双向同步";
            this.chkBothSync.UseVisualStyleBackColor = true;
            this.chkBothSync.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(193, 36);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(50, 21);
            this.textBox1.TabIndex = 25;
            this.textBox1.TextChanged += new System.EventHandler(this.textbox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(137, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 24;
            this.label2.Text = "每手用时";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(202, 152);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(82, 23);
            this.button7.TabIndex = 21;
            this.button7.Text = "停止/分析";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(249, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 26;
            this.label3.Text = "最大计算量(选填)";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(356, 10);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(84, 21);
            this.textBox2.TabIndex = 27;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(249, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 28;
            this.label4.Text = "首位计算量(选填)";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(356, 36);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(85, 21);
            this.textBox3.TabIndex = 29;
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(409, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(44, 26);
            this.button1.TabIndex = 30;
            this.button1.Text = "帮助";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(136, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 12);
            this.label5.TabIndex = 31;
            this.label5.Text = "双向自动落子条件:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkAutoPlay);
            this.groupBox4.Controls.Add(this.radioWhite);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.radioBlack);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.chkBothSync);
            this.groupBox4.Controls.Add(this.textBox3);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.textBox1);
            this.groupBox4.Controls.Add(this.textBox2);
            this.groupBox4.Location = new System.Drawing.Point(7, 59);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(446, 63);
            this.groupBox4.TabIndex = 32;
            this.groupBox4.TabStop = false;
            // 
            // chkAutoPlay
            // 
            this.chkAutoPlay.AutoSize = true;
            this.chkAutoPlay.Location = new System.Drawing.Point(6, 38);
            this.chkAutoPlay.Name = "chkAutoPlay";
            this.chkAutoPlay.Size = new System.Drawing.Size(72, 16);
            this.chkAutoPlay.TabIndex = 32;
            this.chkAutoPlay.Text = "自动落子";
            this.chkAutoPlay.UseVisualStyleBackColor = true;
            this.chkAutoPlay.CheckedChanged += new System.EventHandler(this.chkAutoPlay_CheckedChanged);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(335, 4);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(71, 26);
            this.btnSettings.TabIndex = 33;
            this.btnSettings.Text = "参数设置";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(287, 152);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(82, 23);
            this.button8.TabIndex = 34;
            this.button8.Text = "交换顺序";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // chkShowInBoard
            // 
            this.chkShowInBoard.AutoSize = true;
            this.chkShowInBoard.Location = new System.Drawing.Point(338, 129);
            this.chkShowInBoard.Name = "chkShowInBoard";
            this.chkShowInBoard.Size = new System.Drawing.Size(120, 16);
            this.chkShowInBoard.TabIndex = 35;
            this.chkShowInBoard.Text = "原棋盘上显示选点";
            this.chkShowInBoard.UseVisualStyleBackColor = true;
            this.chkShowInBoard.CheckedChanged += new System.EventHandler(this.chkShowInBoard_CheckedChanged);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(335, 35);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(118, 25);
            this.button9.TabIndex = 36;
            this.button9.Text = "6.5目规则设置方法";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(7, 35);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(78, 25);
            this.button10.TabIndex = 37;
            this.button10.Text = "一键同步";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(241, 125);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(92, 23);
            this.button11.TabIndex = 38;
            this.button11.Text = "框选1路线";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 178);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.chkShowInBoard);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.btnKeepSync);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "棋盘同步工具";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnKeepSync;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoSina;
        private System.Windows.Forms.RadioButton rdoBack;
        private System.Windows.Forms.RadioButton rdoTygem;
        private System.Windows.Forms.RadioButton rdoFox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdo9x9;
        private System.Windows.Forms.RadioButton rdo13x13;
        private System.Windows.Forms.RadioButton rdo19x19;
        private System.Windows.Forms.RadioButton radioWhite;
        private System.Windows.Forms.RadioButton radioBlack;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.CheckBox chkBothSync;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rdoFore;
        private System.Windows.Forms.CheckBox chkAutoPlay;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.RadioButton rdoOtherBoard;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.CheckBox chkShowInBoard;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtBoardHeight;
        private System.Windows.Forms.TextBox txtBoardWidth;
    }
}

