using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace readboard
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
            int x = -9999;
            int y = 9999;
            this.StartPosition = FormStartPosition.Manual; //窗体的位置由Location属性决定
            this.Location = (Point)new Size(x, y);         //窗体的起始位置为(x,y)
            this.ShowInTaskbar = false;
        }
        public int getHwnd()
        {
            return (int)this.Handle;
        }
    }
}
