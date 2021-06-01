namespace readboard
{
    partial class Form6
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form6));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rdoNormalScale = new System.Windows.Forms.RadioButton();
            this.rdoAdvanceScale = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(144, 167);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 34);
            this.button1.TabIndex = 0;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(250, 167);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(105, 34);
            this.button2.TabIndex = 1;
            this.button2.Text = "不再提示";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(440, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "windows显示缩放不为100%,双向同步可能无法准确落子";
            // 
            // rdoNormalScale
            // 
            this.rdoNormalScale.AutoSize = true;
            this.rdoNormalScale.Location = new System.Drawing.Point(7, 20);
            this.rdoNormalScale.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoNormalScale.Name = "rdoNormalScale";
            this.rdoNormalScale.Size = new System.Drawing.Size(105, 22);
            this.rdoNormalScale.TabIndex = 3;
            this.rdoNormalScale.TabStop = true;
            this.rdoNormalScale.Text = "普通缩放";
            this.rdoNormalScale.UseVisualStyleBackColor = true;
            this.rdoNormalScale.CheckedChanged += new System.EventHandler(this.rdoNormalScale_CheckedChanged);
            // 
            // rdoAdvanceScale
            // 
            this.rdoAdvanceScale.AutoSize = true;
            this.rdoAdvanceScale.Location = new System.Drawing.Point(232, 20);
            this.rdoAdvanceScale.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoAdvanceScale.Name = "rdoAdvanceScale";
            this.rdoAdvanceScale.Size = new System.Drawing.Size(105, 22);
            this.rdoAdvanceScale.TabIndex = 4;
            this.rdoAdvanceScale.TabStop = true;
            this.rdoAdvanceScale.Text = "高级缩放";
            this.rdoAdvanceScale.UseVisualStyleBackColor = true;
            this.rdoAdvanceScale.CheckedChanged += new System.EventHandler(this.rdoAdvanceScale_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoNormalScale);
            this.groupBox1.Controls.Add(this.rdoAdvanceScale);
            this.groupBox1.Location = new System.Drawing.Point(18, 67);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(471, 58);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "请设置正确的缩放类型";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(248, 36);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(242, 36);
            this.button3.TabIndex = 7;
            this.button3.Text = "如何判断我的缩放类型";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(404, 18);
            this.label3.TabIndex = 8;
            this.label3.Text = "注:选择不再提示后,可在参数设置中修改缩放类型";
            // 
            // Form6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 218);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form6";
            this.Text = "提示";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdoNormalScale;
        private System.Windows.Forms.RadioButton rdoAdvanceScale;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label3;
    }
}