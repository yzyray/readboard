using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
            this.chkVerifyMove.Checked = Program.verifyMove;
            this.chkAutoMin.Checked = Program.autoMin;
            this.txtSyncInterval.Text = Program.timeinterval + "";
            this.chkEnhanceScreen.Checked = Program.useEnhanceScreen;
            txtGrayOffset.Text = Program.grayOffset + "";
            this.chkPonder.Checked = Program.playPonder;
            if (!Program.isScaled)
            {
                rdoAdvanceScale.Visible = false;
                rdoNormalScale.Visible = false;
                scaleGroup.Visible = false;
                button3.Visible = false;
                    label9.Location = new System.Drawing.Point(10, 60);
                label1.Location = new System.Drawing.Point(10, 87);
                label2.Location = new System.Drawing.Point(10, 114);
                label3.Location = new System.Drawing.Point(196, 87);
                label4.Location = new System.Drawing.Point(196, 114);
                txtBpc.Location = new System.Drawing.Point(109, 82);
                txtBzb.Location = new System.Drawing.Point(297, 82);
                txtWpc.Location = new System.Drawing.Point(109, 108);
                txtWzb.Location = new System.Drawing.Point(297, 108);
                label8.Location = new System.Drawing.Point(10, 163);
                label7.Location = new System.Drawing.Point(10, 183);
                label6.Location = new System.Drawing.Point(10, 201);
                lblGrayOffset.Location=new System.Drawing.Point(10, 141);
                txtGrayOffset.Location = new System.Drawing.Point(Program.isChn ? 109 : 120, 135);
            }
            if (Program.isAdvScale)
                rdoAdvanceScale.Checked = true;
            else
                rdoNormalScale.Checked = true;
            if (!Program.isChn) {
                this.Text = "Settings";
                this.chkPonder.Text = "Ponder";
                this.chkMag.Text = "Show Magnifier";
                this.chkVerifyMove.Text = "Verify Placed Stone";
                this.chkAutoMin.Text = "Auto Minimize";
                this.rdoNormalScale.Text = "NormalScale";
                this.rdoAdvanceScale.Text = "AdvanceScale";
                this.button3.Text = "HowToKnowMyScaleType";
                this.label9.Text = "The following options only be effective on foreground/background";
                this.label1.Text = "B Offset(0-255)";
                this.label3.Text = "B Ratio(0-100)";
                this.label2.Text = "W Offset(0-255)";
                this.label4.Text = "W Ratio(0-100)";
                this.label8.Text = "all parameter must be integer";
                this.label7.Text = "If got some unnecessary stones,try to decrease offset or increase ratio.";//如某种颜色棋子识别过多,可尝试降低偏差或增大占比
                this.label6.Text = "If lost some stones,try to increase offset or decrease ratio";//如某种颜色棋子识别丢失,可尝试增大偏差或降低占比
                this.lblSyncInterval.Text = "Sync Interval(ms):";
                this.button4.Text = "ResetAll";
                this.button1.Text = "Confirm";
                this.button2.Text = "Cancel";
                this.lblGrayOffset.Text = "GrayOffset(0-255)";
                this.chkEnhanceScreen.Text = "EnhScreen";
                this.Size= new Size((int)(461 *Program.factor), (int)(292 * Program.factor));
            }
            var toolTip1 = new ToolTip();
            toolTip1.SetToolTip(this.chkEnhanceScreen, Program.isChn?@"关闭则无法获取桌面外窗口信息,如遇到原棋盘少子等情况可尝试关闭": @"If unchecked,can not get info out of scrren.If origin board comes up lack of stones try closing it.");
            var toolTip2 = new ToolTip();
            toolTip2.SetToolTip(this.chkPonder, Program.isChn ? @"双向同步自动落子时,引擎在对手的回合计算" : @"Engine thinking on opponent's turn when auto playing.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int Bpc=Program.blackPC;
            int Bzb = Program.blackZB;
            int Wpc = Program.whitePC;
            int Wzb = Program.whiteZB;
            Boolean useMag = chkMag.Checked;
            Boolean enableVerifyMove = chkVerifyMove.Checked;
            Boolean chkAuto = chkAutoMin.Checked;
            int syncInterval=Program.timeinterval;
            try
            {
                Bpc=Convert.ToInt32(this.txtBpc.Text);
                Bzb = Convert.ToInt32(this.txtBzb.Text);
                Wpc = Convert.ToInt32(this.txtWpc.Text);
                Wzb = Convert.ToInt32(this.txtWzb.Text);
                syncInterval = Convert.ToInt32(this.txtSyncInterval.Text);
                Program.grayOffset = Convert.ToInt32(this.txtGrayOffset.Text);
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
            Program.verifyMove = enableVerifyMove;
            Program.autoMin = chkAuto;
            Program.isAdvScale = rdoAdvanceScale.Checked;
            string result1 = "config_readboard.txt";
            FileStream fs = new FileStream(result1, FileMode.Create);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            wr.WriteLine(Bpc.ToString()+"_"+Bzb.ToString()+"_"+Wpc.ToString()+"_"+Wzb.ToString()+"_"+ (useMag ? "1":"0")+"_"+ (enableVerifyMove ? "1" : "0")+"_"+(Program.showScaleHint?"1":"0") + "_" + (Program.showInBoard ? "1" : "0") + "_" + (Program.showInBoardHint ? "1" : "0") + "_" + (chkAuto ? "1" : "0")+"_"+(rdoAdvanceScale.Checked?"1":"0") + "_" + Environment.GetEnvironmentVariable("computername").Replace("_", "") + "_" + Form1.type);
            wr.Close();
            this.Close();
            Program.timeinterval = syncInterval;
            Program.timename = syncInterval + "";
            Program.useEnhanceScreen = chkEnhanceScreen.Checked;
            Program.playPonder = this.chkPonder.Checked;
            Form1.pcurrentWin.resetBtnKeepSyncName();
            Form1.pcurrentWin.saveOtherConfig();
            Form1.pcurrentWin.sendPonderStatus();
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
            if (File.Exists("config_readboard_boardsize.txt"))
                File.Delete("config_readboard_boardsize.txt");
            if (File.Exists("config_readboard.txt"))
                File.Delete("config_readboard.txt");
            MessageBox.Show(Program.isChn ? "已恢复默认设置,请重新打开": "Reset successfully,please restart.");
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            Application.Exit();
        }
    }
}
