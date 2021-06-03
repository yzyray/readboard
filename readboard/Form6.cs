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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
            this.ControlBox = false;
            if (Program.hasConfigFile)
            { rdoAdvanceScale.Checked = Program.isAdvScale;
                rdoNormalScale.Checked = !Program.isAdvScale;
            }
            if (!Program.isChn) {
                this.Text = "Information";
                this.label1.Text = "Windows scale is not 100%,DoubleSync may not place stone right.";
                this.label2.Text = "Select your scale type";
                this.button3.Text = "How To Know My Type";
                this.rdoNormalScale.Text = "NormalScale";
                this.rdoAdvanceScale.Text = "AdvancedScale";
                this.label3.Text = "Notice:You can also change scale tpye in settings.";
                this.button1.Text = "Confirm";
                this.button2.Text = "NotAskAgain";
                this.button2.Size = new Size((int)(80*Program.factor), (int)(23 * Program.factor) );
                this.Size = new Size((int)(410 * Program.factor), (int)(184 * Program.factor));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!rdoNormalScale.Checked && !rdoAdvanceScale.Checked)
            {
                MessageBox.Show(Program.isChn?"至少选择一项缩放方式!":"Please select scale type");
                return;
            }
            Program.showScaleHint = false;
            string result1 = "config_readboard.txt";
            FileStream fs = new FileStream(result1, FileMode.Create);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            wr.WriteLine(Program.blackPC.ToString() + "_" + Program.blackZB.ToString() + "_" + Program.whitePC.ToString() + "_" + Program.whiteZB.ToString() + "_" + (Program.useMag ? "1" : "0") + "_" + (Program.doubleClick ? "1" : "0") + "_" + (Program.showScaleHint ? "1" : "0") + "_" + (Program.showInBoard ? "1" : "0") + "_" + (Program.showInBoardHint ? "1" : "0") + "_" + (Program.autoMin ? "1" : "0") + "_" + (Program.isAdvScale ? "1" : "0") + "_" + Environment.GetEnvironmentVariable("computername").Replace("_", ""));
            wr.Close();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!rdoNormalScale.Checked && !rdoAdvanceScale.Checked)
            {
                MessageBox.Show(Program.isChn ? "至少选择一项缩放方式!" : "Please select scale type");
                return; }
            string result1 = "config_readboard.txt";
            FileStream fs = new FileStream(result1, FileMode.Create);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            wr.WriteLine(Program.blackPC.ToString() + "_" + Program.blackZB.ToString() + "_" + Program.whitePC.ToString() + "_" + Program.whiteZB.ToString() + "_" + (Program.useMag ? "1" : "0") + "_" + (Program.doubleClick ? "1" : "0") + "_" + (Program.showScaleHint ? "1" : "0") + "_" + (Program.showInBoard ? "1" : "0") + "_" + (Program.showInBoardHint ? "1" : "0") + "_" + (Program.autoMin ? "1" : "0") + "_" + (Program.isAdvScale ? "1" : "0") + "_" + Environment.GetEnvironmentVariable("computername").Replace("_", ""));
            wr.Close();
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

        private void rdoNormalScale_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoNormalScale.Checked)
                Program.isAdvScale = false;
            else
                Program.isAdvScale = true;
        }

        private void rdoAdvanceScale_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAdvanceScale.Checked)
                Program.isAdvScale = true;
            else
                Program.isAdvScale = false;
        }
    }
}
