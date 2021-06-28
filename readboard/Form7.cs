using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace readboard
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
            if (!Program.isChn) {
                this.label1.Text = "Notice: Short cut \"Crtl+A\".Foreground is not supported.after displayed on original";
                this.label2.Text = "board,you can't place stone.Check BothSync may solve the issue.";
                this.Text = "Information";
                this.button1.Text = "Confirm";
                this.button2.Text = "NotAskAgain";
                this.button2.Size = new Size((int)(80 * Program.factor), (int)(23 * Program.factor));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.showInBoardHint = false;
            string result1 = "config_readboard.txt";
            FileStream fs = new FileStream(result1, FileMode.Create);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            wr.WriteLine(Program.blackPC.ToString() + "_" + Program.blackZB.ToString() + "_" + Program.whitePC.ToString() + "_" + Program.whiteZB.ToString() + "_" + (Program.useMag ? "1" : "0") + "_" + (Program.doubleClick ? "1" : "0") + "_" + (Program.showScaleHint ? "1" : "0") + "_" + (Program.showInBoard ? "1" : "0") + "_" + (Program.showInBoardHint ? "1" : "0") + "_" + (Program.autoMin ? "1" : "0")+"_" + (Program.isAdvScale ? "1" : "0") + "_" + Environment.GetEnvironmentVariable("computername").Replace("_", "")+"_" + Form1.type);
            wr.Close();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
