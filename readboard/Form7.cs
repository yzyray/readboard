using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace readboard
{
    public partial class TipsForm : Form
    {
        public TipsForm()
        {
            InitializeComponent();
            this.Text = getLangStr("TipsForm_title");
            this.lblTips.Text = getLangStr("TipsForm_lblTips");
            this.lblTips1.Text = getLangStr("TipsForm_lblTips1");
            this.btnConfirm.Text = getLangStr("TipsForm_btnConfirm");
            this.btnNotAskAgain.Text = getLangStr("TipsForm_btnNotAskAgain");            
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

        private void button2_Click(object sender, EventArgs e)
        {
            Program.showInBoardHint = false;
            string result1 = "config_readboard.txt";
            FileStream fs = new FileStream(result1, FileMode.Create);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            wr.WriteLine(Program.blackPC.ToString() + "_" + Program.blackZB.ToString() + "_" + Program.whitePC.ToString() + "_" + Program.whiteZB.ToString() + "_" + (Program.useMag ? "1" : "0") + "_" + (Program.verifyMove ? "1" : "0") + "_" + (Program.showScaleHint ? "1" : "0") + "_" + (Program.showInBoard ? "1" : "0") + "_" + (Program.showInBoardHint ? "1" : "0") + "_" + (Program.autoMin ? "1" : "0") + "_" + Environment.GetEnvironmentVariable("computername").Replace("_", "")+"_" + MainForm.type);
            wr.Close();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
