using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace readboard
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            this.txtBpc.Text = Program.blackPC.ToString();
            this.txtBzb.Text = Program.blackZB.ToString();
            this.txtWpc.Text = Program.whitePC.ToString();
            this.txtWzb.Text = Program.whiteZB.ToString();
            this.chkMag.Checked = Program.useMag;
            this.chkDoubleCheck.Checked = Program.doubleClick;
            this.chkAutoMin.Checked = Program.autoMin;
            if (!Program.isScaled)
            {
                rdoAdvanceScale.Visible = false;
                rdoNormalScale.Visible = false;
                scaleGroup.Visible = false;
                button3.Visible = false;
                    label9.Location = new System.Drawing.Point(10, 65);
                label1.Location = new System.Drawing.Point(10, 92);
                label2.Location = new System.Drawing.Point(10, 119);
                label3.Location = new System.Drawing.Point(196, 92);
                label4.Location = new System.Drawing.Point(196, 119);
                txtBpc.Location = new System.Drawing.Point(109, 87);
                txtBzb.Location = new System.Drawing.Point(297, 87);
                txtWpc.Location = new System.Drawing.Point(109, 113);
                txtWzb.Location = new System.Drawing.Point(297, 113);
                label8.Location = new System.Drawing.Point(10, 139);
                label7.Location = new System.Drawing.Point(10, 159);
                label6.Location = new System.Drawing.Point(10, 177);
            }
            if (Program.isAdvScale)
                rdoAdvanceScale.Checked = true;
            else
                rdoNormalScale.Checked = true;
            if (!Program.isChn) {
                this.Text = "Settings";
                this.chkMag.Text = "UseMagnifier";
                this.chkDoubleCheck.Text = "DoubleClickOnForeground(uncheck if sabaki/gogui)";
                this.chkAutoMin.Text = "Auto Minimize After Sync";
                this.rdoNormalScale.Text = "NormalScale";
                this.rdoAdvanceScale.Text = "AdvanceScale";
                this.button3.Text = "HowToKnowMyScaleType";
                this.label9.Text = "The following options only be effective on foreground/background";
                this.label1.Text = "B Offset(0-255)";
                this.label3.Text = "B Ratio(0-100)";
                this.label2.Text = "W Offset(0-255)";
                this.label4.Text = "W Ratio(0-100)";
                this.label8.Text = "Notice: all parameter must be integer.";
                this.label7.Text = "If got some unnecessary stones,try to decrease offset or increase ratio.";//如某种颜色棋子识别过多,可尝试降低偏差或增大占比
                this.label6.Text = "If lost some stones,try to increase offset or decrease ratio";//如某种颜色棋子识别丢失,可尝试增大偏差或降低占比
                this.button4.Text = "ResetAll";
                this.button1.Text = "Confirm";
                this.button2.Text = "Cancel";
                this.Size= new Size((int)(461 *Program.factor), (int)(292 * Program.factor));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int Bpc=Program.blackPC;
            int Bzb = Program.blackZB;
            int Wpc = Program.whitePC;
            int Wzb = Program.whiteZB;
            Boolean useMag = chkMag.Checked;
            Boolean doubleClick = chkDoubleCheck.Checked;
            Boolean chkAuto = chkAutoMin.Checked;
            try
            {
                Bpc=Convert.ToInt32(this.txtBpc.Text);
                Bzb = Convert.ToInt32(this.txtBzb.Text);
                Wpc = Convert.ToInt32(this.txtWpc.Text);
                Wzb = Convert.ToInt32(this.txtWzb.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(Program.isChn?"必须输入整数":"Must be integer");
                return;
            }
            if (Bpc > 255 || Bpc < 0 || Bzb > 100 || Bzb < 0 || Wpc > 255 || Wpc < 0 || Wzb > 100 || Wzb < 0)
            {
                MessageBox.Show(Program.isChn ? "输入的值超过范围":"Out of range");
                return;
            }
            Program.blackPC = Bpc;
            Program.whitePC = Wpc;
            Program.blackZB = Bzb;
            Program.whiteZB = Wzb;
            Program.useMag = useMag;
            Program.doubleClick = doubleClick;
            Program.autoMin = chkAuto;
            Program.isAdvScale = rdoAdvanceScale.Checked;
            string result1 = "config_readboard.txt";
            FileStream fs = new FileStream(result1, FileMode.Create);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            wr.WriteLine(Bpc.ToString()+"_"+Bzb.ToString()+"_"+Wpc.ToString()+"_"+Wzb.ToString()+"_"+ (useMag ? "1":"0")+"_"+ (doubleClick ? "1" : "0")+"_"+(Program.showScaleHint?"1":"0") + "_" + (Program.showInBoard ? "1" : "0") + "_" + (Program.showInBoardHint ? "1" : "0") + "_" + (chkAuto ? "1" : "0")+"_"+(rdoAdvanceScale.Checked?"1":"0") + "_" + Environment.GetEnvironmentVariable("computername").Replace("_", "") + "_" + Form1.type);
            wr.Close();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Process process1 = new Process();
                process1.StartInfo.FileName = "readboard\\scalehelper.rtf";
                process1.StartInfo.Arguments = "";
                process1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                process1.Start();
            }
            catch (Exception)
            {
                MessageBox.Show(Program.isChn ? "找不到说明文档,请检查Lizzie目录下[readboard]文件夹内的[scalehelper.rtf]文件是否存在" : "Can not find file,Please check [scalehelper.rtf] file is in the folder [readboard]");

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string result1 = "config_readboard.txt";
            FileStream fs = new FileStream(result1, FileMode.Create);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            wr.WriteLine(Program.blackPC + "_" + Program.blackZB + "_" + Program.whitePC + "_" + Program.whiteZB + "_" + (Program.useMag ? "1" : "0") + "_" + (Program.doubleClick ? "1" : "0") + "_" + (Program.showScaleHint ? "1" : "0") + "_" + (Program.showInBoard ? "1" : "0") + "_" + (Program.showInBoardHint ? "1" : "0") + "_" + (Program.autoMin ? "1" : "0") + "_" + (Program.isAdvScale ? "1" : "0") + "_" +"yyyyyyssssk$#" + "_" + Form1.type);
            wr.Close();
            MessageBox.Show(Program.isChn ? "已恢复默认设置,请重新打开": "Reset successfully,please restart.");
            Application.Exit();
            System.Environment.Exit(0);
        }
    }
}
