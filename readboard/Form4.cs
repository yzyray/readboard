using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace readboard
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            this.txtBlackOffsets.Text = Program.blackPC.ToString();
            this.txtBlackPercents.Text = Program.blackZB.ToString();
            this.txtWhiteOffsets.Text = Program.whitePC.ToString();
            this.txtWhitePercents.Text = Program.whiteZB.ToString();
            this.chkMag.Checked = Program.useMag;
            this.chkVerifyMove.Checked = Program.verifyMove;
            this.chkAutoMin.Checked = Program.autoMin;
            this.txtSyncInterval.Text = Program.timeinterval.ToString();
            this.chkEnhanceScreen.Checked = Program.useEnhanceScreen;
            txtGrayOffsets.Text = Program.grayOffset.ToString();
            this.chkPonder.Checked = Program.playPonder;

            this.Text = getLangStr("SettingsForm_title");
            this.chkPonder.Text = getLangStr("SettingsForm_chkPonder");
            this.chkMag.Text = getLangStr("SettingsForm_chkMag");
            this.chkVerifyMove.Text = getLangStr("SettingsForm_chkVerifyMove");
            this.chkAutoMin.Text = getLangStr("SettingsForm_chkAutoMin");
            this.lblBackForeOnly.Text = getLangStr("SettingsForm_lblBackForeOnly");
            this.lblBlackOffsets.Text = getLangStr("SettingsForm_lblBlackOffsets");
            this.lblBlackPercents.Text = getLangStr("SettingsForm_lblBlackPercents");
            this.lblWhiteOffsets.Text = getLangStr("SettingsForm_lblWhiteffsets");
            this.lblWhitePercents.Text = getLangStr("SettingsForm_lblWhitePercents");
            this.lblGrayOffsets.Text = getLangStr("SettingsForm_lblGrayOffsets");
            this.lblTips.Text = getLangStr("SettingsForm_lblTips");
            this.lblTips1.Text = getLangStr("SettingsForm_lblTips1");
            this.lblTips2.Text = getLangStr("SettingsForm_lblTips2");
            this.lblSyncInterval.Text = getLangStr("SettingsForm_lblSyncInterval");
            this.btnReset.Text = getLangStr("SettingsForm_btnReset");
            this.btnConfirm.Text = getLangStr("SettingsForm_btnConfirm");
            this.btnCancel.Text = getLangStr("SettingsForm_btnCancel");
            this.chkEnhanceScreen.Text = getLangStr("SettingsForm_chkEnhanceScreen");

           // this.Size= new Size((int)(461 *Program.factor), (int)(270 * Program.factor));
           

            var toolTip1 = new ToolTip();
            toolTip1.SetToolTip(this.chkEnhanceScreen, getLangStr("SettingsForm_chkEnhanceScreen_ToolTip"));
            var toolTip2 = new ToolTip();
            toolTip2.SetToolTip(this.chkPonder, getLangStr("SettingsForm_chkPonder_ToolTip"));
        }

        private String getLangStr(String itemName)
        {
            String result = "";
            try
            {
                result = Program.langItems[itemName].ToString();
            }
            catch (Exception e)
            {                
                MainForm.pcurrentWin.SendError(e.ToString());
            }
            return result;
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
                Bpc=Convert.ToInt32(this.txtBlackOffsets.Text);
                Bzb = Convert.ToInt32(this.txtBlackPercents.Text);
                Wpc = Convert.ToInt32(this.txtWhiteOffsets.Text);
                Wzb = Convert.ToInt32(this.txtWhitePercents.Text);
                syncInterval = Convert.ToInt32(this.txtSyncInterval.Text);
                Program.grayOffset = Convert.ToInt32(this.txtGrayOffsets.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(getLangStr("SettingsForm_mustBeInteger"));
                return;
            }
            if (Bpc > 255 || Bpc < 0 || Bzb > 100 || Bzb < 0 || Wpc > 255 || Wpc < 0 || Wzb > 100 || Wzb < 0)
            {
                MessageBox.Show(getLangStr("SettingsForm_outOfRange"));
                return;
            }
            Program.blackPC = Bpc;
            Program.whitePC = Wpc;
            Program.blackZB = Bzb;
            Program.whiteZB = Wzb;
            Program.useMag = useMag;
            Program.verifyMove = enableVerifyMove;
            Program.autoMin = chkAuto;
         //   Program.isAdvScale = rdoAdvanceScale.Checked;
            string result1 = "config_readboard.txt";
            FileStream fs = new FileStream(result1, FileMode.Create);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            wr.WriteLine(Bpc.ToString()+"_"+Bzb.ToString()+"_"+Wpc.ToString()+"_"+Wzb.ToString()+"_"+ (useMag ? "1":"0")+"_"+ (enableVerifyMove ? "1" : "0")+"_"+(Program.showScaleHint?"1":"0") + "_" + (Program.showInBoard ? "1" : "0") + "_" + (Program.showInBoardHint ? "1" : "0") + "_" + (chkAuto ? "1" : "0") + "_" + Environment.GetEnvironmentVariable("computername").Replace("_", "") + "_" + MainForm.type);
            wr.Close();
            this.Close();
            Program.timeinterval = syncInterval;
            Program.timename = syncInterval.ToString();
            Program.useEnhanceScreen = chkEnhanceScreen.Checked;
            Program.playPonder = this.chkPonder.Checked;
            MainForm.pcurrentWin.resetBtnKeepSyncName();
            MainForm.pcurrentWin.saveOtherConfig();
            MainForm.pcurrentWin.sendPonderStatus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (File.Exists("config_readboard_others.txt"))
                File.Delete("config_readboard_others.txt");
            if (File.Exists("config_readboard.txt"))
                File.Delete("config_readboard.txt");
            MessageBox.Show(getLangStr("SettingsForm_resetDefaultTip"));// Program.isChn ? "已恢复默认设置,请重新打开": "Reset successfully,please restart.");
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            Application.Exit();
        }
    }
}
