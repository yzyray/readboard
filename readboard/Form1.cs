using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using OpenCvSharp;
using Point = OpenCvSharp.Point;
using System.Threading.Tasks;

namespace readboard
{
    public partial class Form1 : Form
    {
       // Boolean showDebugImage = true;
        TcpClient client;
        NetworkStream io;
        Thread threadReceive;
        string readStr;
        Boolean pressed = false;
        Boolean clicked = false;
        
        public static int ox1;
         int ox2;
        public static int oy1;
        int oy2;
        public static CDmSoft dm;
        public static CDmSoft dm2;
        int hwnd=0;
        Form2 form2;
        Form3 form3;
        Form8 form8;

        Boolean startedSync = false;
        Boolean isContinuousSyncing = false;
        object qx1;
        object qy1;

        String timename;
        int timeinterval;
        Boolean keepSync = false;
        int sx1;
        int sy1;
        int width;
        int height;
        int sx1ty5=0;
        int sy1ty5=0;
        int all;
          //  System.Timers.Timer t;
       public static int type=5;
       // Boolean isQTYC = false;
       // int boardWidth=19;
        int boardH = 19;
        int boardW = 19;
       static double widthMagrin=0;
        static double heightMagrin =0;
        Boolean noticeLast = true;
        Boolean syncBoth = false;
        Boolean canUseLW = false;
        Boolean noLw = false;
        Boolean useTcp = false;
        Boolean savedPlace = false;
        int savedX;
        int savedY;
        Thread thread;
        Boolean isMannulCircle = false;
        float factor = 1.0f;
        private KeyboardHookListener hookListener;
        private int port = 24781;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        [StructLayout(LayoutKind.Sequential)]


        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private void Send(String strMsg)
        {
            if (useTcp)
            {
                try
                {
                    byte[] by = Encoding.UTF8.GetBytes(strMsg + "\r\n");
                    io.Write(by, 0, by.Length);
                    io.Flush();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else {
                Console.WriteLine(strMsg);
            }
        }

        private Boolean isAltDown = false;

        private void HookListener_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
           // Console.Out.WriteLine(e.KeyValue);
            if (e.KeyValue == 164|| e.KeyValue==165)
                isAltDown = true;
            if (isAltDown && e.KeyValue == 65)
                chkShowInBoard.Checked = !chkShowInBoard.Checked;
        }

        private void HookListener_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
          if (e.KeyValue == 164 || e.KeyValue == 165)
             isAltDown = false;
        }

        private void Receive()
        {
            try
            {               
                while (true)
                {
                    byte[] data = new byte[20000];
                    int numBytesRead = io.Read(data, 0, data.Length);
                    if (numBytesRead > 0)
                    {
                        readStr = Encoding.UTF8.GetString(data, 0, numBytesRead);
                        Console.WriteLine(readStr);
                    }
                    this.Invoke(new Action(() => { readPlace(readStr); }));
                }
            }
            catch
            {
                Console.WriteLine("err");
            }
        }
        private void readPlace(String a) {
            if (a.StartsWith("place"))
            {
                char[] separator = { ' ' }; string[] arr = a.Split(separator);
              place(int.Parse(arr[1]), int.Parse(arr[2]));
            }
            if (a.StartsWith("loss"))
            {
              lossFocus();
            }
            if (a.StartsWith("notinboard"))
            {
                stopInBoard();
            }
        }

        public Form1(String time, String last, String both, String aitime, String playouts, String firstpo, String nolw, String usetcp,String serverPort)
        {
            InitializeComponent();
            GlobalHooker hooker = new GlobalHooker();
            hookListener = new KeyboardHookListener(hooker);
            hookListener.KeyDown += HookListener_KeyDown;
            hookListener.KeyUp += HookListener_KeyUp;
            hookListener.Start();
            System.Drawing.Bitmap bitmap = new Bitmap(1, 1);
            System.Drawing.Graphics graphics2 = Graphics.FromImage(bitmap);
            factor = graphics2.DpiX / 96;
            if (factor > 1.0f)
            {  Program.isScaled = true;
            Program.factor = factor; }
            if (File.Exists("config_readboard.txt"))
            {
                StreamReader sr = new StreamReader("config_readboard.txt", Encoding.UTF8);
                String line;
                if ((line = sr.ReadLine()) != null)
                {
                    string[] arr = line.Split('_');
                    if (arr.Length == 13)
                    {
                        try
                        {
                            if (Environment.GetEnvironmentVariable("computername").Replace("_", "").Equals(arr[11]))
                            {
                                Program.blackPC = Convert.ToInt32(arr[0]);
                                Program.blackZB = Convert.ToInt32(arr[1]);
                                Program.whitePC = Convert.ToInt32(arr[2]);
                                Program.whiteZB = Convert.ToInt32(arr[3]);
                                Program.useMag = (Convert.ToInt32(arr[4]) == 1);
                                Program.doubleClick = (Convert.ToInt32(arr[5]) == 1);
                                Program.showScaleHint = (Convert.ToInt32(arr[6]) == 1);
                                Program.showInBoard = (Convert.ToInt32(arr[7]) == 1);
                                Program.showInBoardHint = (Convert.ToInt32(arr[8]) == 1);
                                Program.autoMin = (Convert.ToInt32(arr[9]) == 1);
                                Program.isAdvScale = (Convert.ToInt32(arr[10]) == 1);
                                type = Convert.ToInt32(arr[12]);
                                Program.hasConfigFile = true;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                sr.Close();
            }

            if (File.Exists("config_readboard_boardsize.txt"))
            {
                int customW = -1;
                int customH = -1;
                StreamReader sr = new StreamReader("config_readboard_boardsize.txt", Encoding.UTF8);
                String line;
                if ((line = sr.ReadLine()) != null)
                {
                    string[] arr = line.Split('_');
                    if (arr.Length == 4)
                    {
                        try
                        {
                            this.boardW = Convert.ToInt32(arr[0]);
                            this.boardH = Convert.ToInt32(arr[1]);
                            customW = Convert.ToInt32(arr[2]);
                            customH = Convert.ToInt32(arr[3]);
                        }
                        catch (Exception)
                        {
                        }                      
                    }
                }
                sr.Close();
                if (boardW == boardH)
                {
                    if (boardW == 19)
                    {
                        this.rdo19x19.Checked = true;
                        if(customW>0)
                            this.txtBoardWidth.Text = customW + "";
                        if (customH > 0)
                            this.txtBoardHeight.Text = customH + "";
                    }
                    else if (boardW == 13)
                    {
                        this.rdo13x13.Checked = true;
                        if (customW > 0)
                            this.txtBoardWidth.Text = customW + "";
                        if (customH > 0)
                            this.txtBoardHeight.Text = customH + "";
                    }
                    else if (boardW == 9)
                    {
                        this.rdo9x9.Checked = true;
                        if (customW > 0)
                            this.txtBoardWidth.Text = customW + "";
                        if (customH > 0)
                            this.txtBoardHeight.Text = customH + "";
                    }
                    else
                    {
                        this.txtBoardWidth.Text = this.boardW + "";
                        this.txtBoardHeight.Text = this.boardH + "";
                        this.rdoOtherBoard.Checked = true;
                    }
                }
                else
                {
                    this.txtBoardWidth.Text = this.boardW + "";
                    this.txtBoardHeight.Text = this.boardH + "";
                    this.rdoOtherBoard.Checked = true;                
                }
            }
            else
                this.rdo19x19.Checked = true;
            switch (type) {
                case 0:
                    rdoFox.Checked = true;
                    break;
                case 1:
                    rdoTygem.Checked = true;
                    break;
                case 2:
                    rdoSina.Checked = true;
                    break;
                case 5:
                    rdoFore.Checked = true;
                    break;
                case 3:
                    rdoBack.Checked = true;
                    break;


            }
            this.chkShowInBoard.Checked = Program.showInBoard;
            Program.showInBoard = chkShowInBoard.Checked;
            if (Program.showScaleHint && factor > 1)
            {

                Form6 form6 = new Form6();
                form6.ShowDialog();

            }
            dm = new CDmSoft();
            dm2 = new CDmSoft();
            this.MaximizeBox = false;
            pcurrentWin = this;  

            if (type == 0 || type == 1 || type == 2)
                this.button10.Enabled = true;
            else
                this.button10.Enabled = false;

            timeinterval =  int.Parse(time);
            timename = time;
            if (last.Equals("0"))
                {
                noticeLast = true;
            }
            if (last.Equals("1"))
            {
                noticeLast = false;
            }
            if (both.Equals("0"))
            {
                syncBoth = true;
                checkBox1.Checked = true;
            }
            if (both.Equals("1"))
            {
                syncBoth = false;
                checkBox1.Checked = false;
            }
            if (syncBoth)
            { Send("bothSync"); }
            else
            {
                Send("nobothSync");
            }
            if (nolw.Equals("1"))
            {
                noLw = true;
            }
            if (usetcp.Equals("1"))
            {
                useTcp = true;
            }            
            if (!aitime.Equals(" "))
                textBox1.Text = aitime;
            if(!playouts.Equals(" "))
            textBox2.Text = playouts;
            if (!firstpo.Equals(" "))
                textBox3.Text = firstpo;
            if (!noLw)
            {
                try
                {
                  //  int s = DllRegisterServer();
                  //  if (s >= 0)
                  //  {
                        //注册成功!             
                        try
                        {
                            Thread.Sleep(300);
                            lw.lwsoft lw;
                            lw = new lw.lwsoft();
                            canUseLW = true;
                        }
                        catch (Exception )
                        {
                            canUseLW = false;
                        }
                  //  }
                  //  else
                  //  {
                        //注册失败}
                  //      canUseLW = false;
                  //  }
                }
                catch (Exception)
                {
                    canUseLW = false;
                }
            }
            radioWhite.Enabled = false;
            radioBlack.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            if (useTcp)
            {
                try
                {
                    port= Convert.ToInt32(serverPort); 
                    client = new TcpClient("127.0.0.1", port);
                    threadReceive = new Thread(new ThreadStart(Receive));
                    threadReceive.IsBackground = true;
                    threadReceive.Start();
                    io = client.GetStream();
                }
                catch
                {
                    MessageBox.Show(Program.isChn?"棋盘同步工具与Lizzie连接失败":"Can not connect to Lizzie");
                }
            }
            if (!Program.isChn) {
                this.rdoFox.Text = "FoxWQ";
                this.rdoTygem.Text = "Tygem";
                this.rdoBack.Text = "Background";
                this.rdoSina.Text = "Sina";
                this.rdoFore.Text = "Foreground";
                this.btnSettings.Text = "Settings";
                this.button1.Text = "Help";
                this.button10.Text = "FastSync";
                this.label1.Text = "Size:";
                this.button9.Text = "HowToSetKomi6.5";
                this.checkBox1.Text = "BothSync";
                this.chkAutoPlay.Text = "AutoPlay";
                this.radioBlack.Text = "PlayB";
                this.radioWhite.Text = "PlayW";
                this.label5.Text = "PlayCondition:";
                this.label2.Text = "MoveTime";
                this.label3.Text = "MaxPlayouts";
                this.label4.Text = "BestMovePlayouts";
                this.button3.Text = "ChooseGoban(clickInside)";
                this.button2.Text = "SelectBoard";
                this.button7.Text = "Ponder/off";
                this.chkShowInBoard.Text = "DisplayOnOriginal";
                this.button5.Text = "KeepSync(200ms)";
                this.button4.Text = "OneTimeSync";
                this.button8.Text = "Exchange";
                this.button6.Text = "ClearBoard";
                this.button11.Text = "CircleRow1";
               this.Text = "Board Synchronization Tool";
            }           
        }
        MouseHookListener mh;
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="strCmd"></param>
        /// <returns></returns>
        static void AutoRegCom(string strCmd)
        {
           // string rInfo;
            try
            {               
                Process proc = new Process();
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.Arguments = "C:\\Windows\\System32\\cmd.exe";
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.Verb = "RunAs";
                proc.StartInfo.UseShellExecute = false;
                proc.Start();
                proc.StandardInput.WriteLine(strCmd);
                proc.Close();
            }
            catch (Exception )
            {
                return;
            }
        }

        public static Form1
           pcurrentWin = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            string startup = Application.ExecutablePath;
            int pp = startup.LastIndexOf("\\");
            startup = startup.Substring(0, pp);
          
            dm.SetShowErrorMsg(0);
            mh = new MouseHookListener(new GlobalHooker());
            
            mh.MouseMove += mh_MouseMoveEvent;
            mh.MouseClick += mh_MouseMoveEvent2;
            mh.Enabled = false;
            this.button5.Text = (Program.isChn ? "持续同步(" : "KeepSync(") + timename + "ms)";
        }

        //[DllImport("user32.dll")]
        //static extern void BlockInput(bool Block);
        public void Snap( int sx1, int sy1, int sx2, int sy2)
        {            
            pressed = true;
            ox1 = Math.Min(sx1, sx2);
            oy1 = Math.Min(sy1, sy2); 
            ox2 = Math.Max(sx1, sx2);
            oy2 = Math.Max(sy1, sy2);
            if (!isMannulCircle)
            {
                if (!reCalculateRow1(ox1, oy1, ox2, oy2))
                    MessageBox.Show(Program.isChn ? "不能识别棋盘,请调整被同步棋盘大小后重新选择或尝试[框选1路线]" : "Can not detect board,Please zoom the board and try again or use [CircleRow1]");
                else if(type==3)
                {
                    object curX, curY;
                    dm2.GetCursorPos(out curX, out curY);
                   // BlockInput(true);
                    dm2.MoveTo((ox1 + ox2) / 2, (oy1 + oy2) / 2);
                    Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(35);
                        hwnd = dm2.GetMousePointWindow();
                    });
                    Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(30);
                        dm2.MoveTo((ox1 + ox2) / 2, (oy1 + oy2) / 2);
                        Thread.Sleep(20);
                        dm2.MoveTo((ox1 + ox2) / 2, (oy1 + oy2) / 2);
                        Thread.Sleep(50);
                        dm2.MoveTo((int)curX, (int)curY);
                    });
                  //  BlockInput(false);                   
                }
            }
            else { 
            int gapX= (int)Math.Round((ox2 - ox1) / ((boardW- 1)*2f));
            int gapY= (int)Math.Round((oy2 - oy1) / ((boardH - 1)*2f));
            ox1 = ox1 - gapX;
            oy1 = oy1 - gapY;
            ox2 = ox2 + gapX;
            oy2 = oy2 + gapY;
                }
            this.WindowState = FormWindowState.Normal;
            //mh.Enabled = false;
        }   

        void mh_MouseMoveEvent(object sender, MouseEventArgs e)
        {
            if (type == 3 && !isMannulCircle)
                return;
            if (pressed)
            {
                pressed = false;
                hwnd = dm.GetMousePointWindow();
            }
        }

        void mh_MouseMoveEvent2(object sender, MouseEventArgs e)
        {
            if (type == 3 && !isMannulCircle)
                return;
            if (clicked)
            {
                //if (!isKuangxuan)
               //     mh.Enabled = false;
                clicked = false;
                hwnd = dm.GetMousePointWindow();
            }
         

        }
        
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mh.Enabled = true;
            clicked = true;
        }

        private Boolean CaptureScreen(int x, int y, int x2, int y2)
        {
            if (x2 > x && y2 > y)
            {
                int width = x2 - x;
                int height = y2 - y;
               Program.bitmap = new Bitmap(width, height);
                using (System.Drawing.Graphics graphics = Graphics.FromImage(Program.bitmap))
                {                    
                    graphics.CopyFromScreen(x, y, 0, 0, new System.Drawing.Size((int)(width * factor), (int)(height * factor)));
                    //bitmap.Save("screen.bmp");
                }
              //  dm2.Capture(x, y, x2, y2, "screen.bmp");
                return true;
            }
            else
            { MessageBox.Show(Program.isChn ? "未选择棋盘":"No board has been choosen");
                return false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
            if (type == 5)
            {
                if (!CaptureScreen(ox1, oy1, ox2, oy2))
                    return;
                //m.Capture(ox1, oy1, ox2, oy2, "screen.bmp");
                IntPtr hwnds;
                if (form3 != null)
                {
                    hwnds = form3.setPic();
                }
                else
                {
                    form3 = new Form3();
                    hwnds = form3.setPic();
                    form3.Show();
                    dm.MoveWindow(form3.getHwnd(), 9999, 9999);
                }
                Send("sync");
                // MessageBox.Show(hwnds.ToString());
                dm.BindWindow((int)hwnds, "gdi", "normal", "normal", 0);
                sx1ty5 = ox1;
                sy1ty5 = oy1;
                sx1 = 0;
                sy1 = 0;
                width = ox2 - ox1;
                height = oy2 - oy1;
                all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
                OutPut3(true, null);
            }
            else
            {
                int x1;
                int y1;
                int x2;
                int y2;

                RECT rect = new RECT();
                IntPtr p = new IntPtr(hwnd);
                GetWindowRect(p, ref rect);
                x2 = rect.Right;
                y2 = rect.Bottom;
                x1 = rect.Left;
                y1 = rect.Top;

                if ((int)x1 == 0 && (int)x2 == 0 && (int)y1 == 0 && (int)y2 == 0)
                {                  
                    p = new IntPtr(hwnd);
                    GetWindowRect(p, ref rect);
                    x2 = rect.Right;
                    y2 = rect.Bottom;
                    x1 = rect.Left;
                    y1 = rect.Top;
                    if ((int)x1 == 0 && (int)x2 == 0 && (int)y1 == 0 && (int)y2 == 0)
                    {
                        MessageBox.Show(Program.isChn?"未选择棋盘,同步失败": "No board has been choosen,Sync failed");
                        stopKeepingSync();
                        return;
                    }
                }

                if (type == 0)
                    dm.BindWindow(hwnd, "gdi", "normal", "normal", 0);
                else
                {
                    dm.BindWindow(hwnd, "gdi", "windows", "normal", 0);
                }
           //     startKeepingSync();
                Send("sync");
                if (type == 0)
                {
                    if (!dm.GetWindowClass(hwnd).ToLower().Equals("#32770"))
                    {
                        MessageBox.Show(Program.isChn ? "未选择正确的棋盘":"Not right board");
                    }
                    dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "313131-050505", "0|1|2E2E2E-050505,0|2|2E2E2E-050505,-1|0|2E2E2E-050505,-2|0|2E2E2E-050505,-2|1|-2E2E2E-050505,-1|1|-2E2E2E-050505,-1|2|-2E2E2E-050505", 1.0, 2, out qx1, out qy1);
                    object qx4;
                    object qy4;
                    dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "313131-050505", "0|1|2E2E2E-050505,0|2|2E2E2E-050505,1|0|2E2E2E-050505,2|0|2E2E2E-050505,2|1|-2E2E2E-050505,1|1|-2E2E2E-050505,1|2|-2E2E2E-050505", 1.0, 0, out qx4, out qy4);
                    // dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "313131-050505", "0|-1|2E2E2E-050505,0|-2|2E2E2E-050505,1|0|2E2E2E-050505,2|0|2E2E2E-050505,2|-1|-2E2E2E-050505,1|-1|-2E2E2E-050505,1|-2|-2E2E2E-050505", 1.0, 1, out qx4, out qy4);
                    sx1 = (int)qx4;
                    sy1 = (int)qy1;
                    width = (int)qx1 - (int)qx4;
                    height = width;
                    all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
                }
                if (type == 1)
                {
                    if (!dm.GetWindowClass(hwnd).ToLower().Equals("afxwnd140u"))
                    {
                        MessageBox.Show(Program.isChn ? "未选择正确的棋盘" : "Not right board");
                    }
                    sx1 = 0;
                    sy1 = 0;
                    width = x2 - x1;
                    height = y2 - y1;
                    if (!Program.isAdvScale)
                    {
                        width =(int)(width / factor);
                        height = (int)(height / factor);
                            }
                    all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
                }
                if (type == 2)
                {
                    if (!(dm.GetWindowClass(hwnd).ToLower().Equals("tlmdsimplepanel")|| dm.GetWindowClass(hwnd).ToLower().Equals("tpanel")))
                    {
                        MessageBox.Show(Program.isChn ? "未选择正确的棋盘" : "Not right board");
                    }
                    dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "FBDAA2-050505", "0|1|FBDAA2-050505,0|2|FBDAA2-050505", 1.0, 0, out qx1, out qy1);
                    object qx4;
                    object qy4;
                    dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "FBDAA2-050505", "0|1|FBDAA2-050505,0|2|FBDAA2-050505", 1.0, 1, out qx4, out qy4);
                    sx1 = (int)qx1 - 1;
                    sy1 = (int)qy1 - 1;
                    height = (int)qy4 - (int)qy1 + 4;
                    width = height;
                    all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
                }
                if (type == 3)
                {
                    sx1 = (int)(ox1 - x1);
                    sy1 = (int)(oy1 - y1);
                    width = (int)((ox2 - ox1) );
                    height = (int)((oy2 - oy1) );
                    if (!Program.isAdvScale)
                    {
                        sx1 = (int)(sx1 / factor);
                        sy1 = (int)(sy1 / factor);
                        width = (int)(width / factor);
                        height = (int)(height / factor);
                    }
                    //if (true) {
                    //    sx1 = (int)(sx1 / factor);
                    //    sy1 = (int)(sy1 / factor);
                    //   // width = (int)(width / factor);
                    //  //  height = (int)(height / factor);
                    //}
                    all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
                    //dm.Capture(sx1, sy1, sx1+width, sy1+ height, "backboard.bmp");
                    OutPut3(true, null);
                }
                else
                    OutPut(true, null);
                // if (t != null)
                //  {
                //       t.Enabled = false;
                //   }
            }
        }

        private void minWindow(String a) {
            this.WindowState = FormWindowState.Minimized;
        }
        private void startKeepingSync(String a)
        {
            this.button5.Text = Program.isChn ? "停止同步" : "StopSync";
            this.button10.Text = Program.isChn ? "停止同步" : "StopSync";
            startedSync = true;
            this.rdoFox.Enabled = false;
            this.rdoTygem.Enabled = false;
            //checkBox1.Enabled = false;
           // chkAutoPlay.Enabled = checkBox1.Checked;
            this.rdoBack.Enabled = false;
            this.rdoSina.Enabled = false;
            this.rdo19x19.Enabled = false;
            this.rdo13x13.Enabled = false;
            this.rdo9x9.Enabled = false;
            this.rdoOtherBoard.Enabled = false;
            this.rdoFore.Enabled = false;
            this.button11.Enabled = false;
            this.button2.Enabled = false;
            this.button3.Enabled = false;
            this.button4.Enabled = false;
        }

        private void setContinuousSync(String a)
        {
            //this.button5.Text = "停止同步";
            this.button10.Text = Program.isChn ? "停止同步" : "StopSync";
            //  button5click = true;
            this.rdoFox.Enabled = false;
            this.rdoTygem.Enabled = false;
            //checkBox1.Enabled = false;
            // chkAutoPlay.Enabled = checkBox1.Checked;
            this.rdoBack.Enabled = false;
            this.rdoSina.Enabled = false;
            this.rdo19x19.Enabled = false;
            this.rdo13x13.Enabled = false;
            this.rdo9x9.Enabled = false;
            this.rdoOtherBoard.Enabled = false;
            this.rdoFore.Enabled = false;
          //  this.button2.Enabled = false;
        //    this.button3.Enabled = false;
        //    this.button4.Enabled = false;
        }

        public void Action2Test(String t)
        {
            this.button5.Text = t;
            if(!isContinuousSyncing)
            this.button10.Text = "一键同步";
            //if (this.factor <= 1)
            //{ 
            this.button3.Enabled = true;
            this.button4.Enabled = true;
            this.button5.Enabled = true;
            this.rdoFox.Enabled = true;
            this.rdoSina.Enabled = true;
            //} 
            this.rdoTygem.Enabled = true;
           
            this.rdoBack.Enabled = true;           
            this.rdo19x19.Enabled = true;
           // checkBox1.Enabled = true;
          //  chkAutoPlay.Enabled = true;
            this.rdo13x13.Enabled = true;
            this.rdo9x9.Enabled = true;
            this.rdoOtherBoard.Enabled = true;
            this.rdoFore.Enabled = true;
            //
            if (type == 3 || type == 5)
            {
                this.button11.Enabled = true;
                this.button2.Enabled = true;               
            }
            else
                this.button3.Enabled = true;
            this.button4.Enabled = true;
        }

        private void stopKeepingSync()
        {
            startedSync = false;
            if (thread!=null&&thread.IsAlive)
                thread.Abort();
            Action2<String> a = new Action2<String>(Action2Test);
            Invoke(a, (Program.isChn ? "持续同步(" : "KeepSync(") + timename + "ms)");      
        }

        private void startContinuous() {
            Action2<String> a = new Action2<String>(setContinuousSync);
            Invoke(a, "");
            RECT rect = new RECT();
            IntPtr p = new IntPtr(hwnd);
            while (isContinuousSyncing) {
                if (!startedSync) {
                    hwnd = -1;
                    int finalWidth = 0;
                    int x1;
                    int y1;
                    int x2;
                    int y2;
                    if (type == 0)
                {
                    //hwnd = dm.FindWindow("#32770", "");
                    String hwnds = dm.EnumWindowByProcess("foxwq.exe", "", "#32770", 16);
                    string[] hwndArray = hwnds.Split(',');
                    foreach (string oneHwnd in hwndArray)
                    {
                        if (oneHwnd.Length == 0)
                            continue;
                        if (dm.GetWindowTitle(int.Parse(oneHwnd)).Equals("CChessboardPanel"))
                        {
                            int hwndTemp = int.Parse(oneHwnd);
                            p = new IntPtr(hwndTemp);
                            GetWindowRect(p, ref rect);
                            x2 = rect.Right;
                            y2 = rect.Bottom;
                            x1 = rect.Left;
                            y1 = rect.Top;
                            if (x1 >= -9999 && y1 >= -9999 && x2 - x1 > finalWidth || finalWidth == 0)
                            {
                                hwnd = hwndTemp;
                                finalWidth = x2 - x1;
                            }
                        }
                    }
                }
                else if (type == 1)
                {
                    String hwnds = dm.EnumWindowByProcess("TygemEweiqi.exe", "", "AfxWnd140u", 16);
                    string[] hwndArray = hwnds.Split(',');
                    foreach (string oneHwnd in hwndArray)
                    {
                        if (oneHwnd.Length == 0)
                            continue;
                        if (dm.GetWindowClass(int.Parse(oneHwnd)).Equals("AfxWnd140u") && dm.GetWindowTitle(int.Parse(oneHwnd)).Length == 0)
                        {
                            int hwndTemp = int.Parse(oneHwnd);
                            p = new IntPtr(hwndTemp);
                            GetWindowRect(p, ref rect);
                            x2 = rect.Right;
                            y2 = rect.Bottom;
                            x1 = rect.Left;
                            y1 = rect.Top;
                            if (x1 >= -9999 && y1 >= -9999 && (x2 - x1) > 0 && (y2 - y1) > 0 && (float)(x2 - x1) / (float)(y2 - y1) < 1.05 && (float)(x2 - x1) / (float)(y2 - y1) > 0.95)
                            {
                                if (x2 - x1 < finalWidth || finalWidth == 0)
                                {
                                    hwnd = hwndTemp;
                                    finalWidth = x2 - x1;
                                }
                            }
                        }
                    }
                }
                else if (type == 2)
                {
                    String hwnds = dm.EnumWindowByProcess("Sina.exe", "", "TLMDSimplePanel", 16);
                    string[] hwndArray = hwnds.Split(',');
                    foreach (string oneHwnd in hwndArray)
                    {
                        if (oneHwnd.Length == 0)
                            continue;
                        int hwndTemp = int.Parse(oneHwnd);
                        p = new IntPtr(hwndTemp);
                        GetWindowRect(p, ref rect);
                        x2 = rect.Right;
                        y2 = rect.Bottom;
                        x1 = rect.Left;
                        y1 = rect.Top;
                        if (!dm.GetWindowClass(hwndTemp).Equals("TLMDSimplePanel"))
                            continue;

                        if (x1 >= -9999 && y1 >= -9999 && x2 - x1 > finalWidth || finalWidth == 0)
                        {
                            hwnd = hwndTemp;
                            finalWidth = x2 - x1;
                        }

                    }
                }
                if (hwnd > 0&& isContinuousSyncing) {
                    startContinuousSync(true);
                }
                }
                try
                {
                    Thread.Sleep(100);
                }
                catch (Exception) { }
            }
        }

        private void startContinuousSync(Boolean isSimpleSync) {
            Boolean isRightGoban = true;
            if (type == 5)
            {
                if (!CaptureScreen(ox1, oy1, ox2, oy2))
                    return;
                //m.Capture(ox1, oy1, ox2, oy2, "screen.bmp");
                IntPtr hwnds;
                if (form3 != null)
                {
                    //form3.Close();
                    // form3.Dispose();
                    hwnds = form3.setPic();
                }
                else
                {
                    form3 = new Form3();
                    hwnds = form3.setPic();
                    form3.Show();
                    dm.MoveWindow(form3.getHwnd(), 9999, 9999);
                }
                Send("sync");
                dm.BindWindow((int)hwnds, "gdi", "normal", "normal", 0);
                Action2<String> a = new Action2<String>(startKeepingSync);
                Invoke(a,"");
              //  startKeepingSync();
                sx1ty5 = ox1;
                sy1ty5 = oy1;
                sx1 = 0;
                sy1 = 0;
                width = ox2 - ox1;
                height = oy2 - oy1;
                all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
                OutPut3(true, null);
            }
            else
            {
                int x1;
                int y1;
                int x2;
                int y2;

                RECT rect = new RECT();
                IntPtr p = new IntPtr(hwnd);
                GetWindowRect(p, ref rect);
                x2 = rect.Right;
                y2 = rect.Bottom;
                x1 = rect.Left;
                y1 = rect.Top;

                if ((int)x1 == 0 && (int)x2 == 0 && (int)y1 == 0 && (int)y2 == 0)
                {
                    if(!isSimpleSync&&startedSync)
                    MessageBox.Show(Program.isChn ? "未选择棋盘,同步失败" : "No board has been choosen,Sync failed");
                    stopKeepingSync();
                    return;
                }
                if (type == 0)
                    dm.BindWindow(hwnd, "gdi", "normal", "normal", 0);
                else
                {
                    dm.BindWindow(hwnd, "gdi", "windows", "normal", 0);
                }
                Action2<String> a = new Action2<String>(startKeepingSync);
                Invoke(a, "");
                Send("sync");
                if (type == 0)
                {
                    if (!dm.GetWindowClass(hwnd).ToLower().Equals("#32770"))
                    {
                        if (!isSimpleSync && startedSync)
                            MessageBox.Show(Program.isChn ? "未选择棋盘,同步失败" : "No board has been choosen,Sync failed");
                        isRightGoban = false;
                    }
                    dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "313131-050505", "0|1|2E2E2E-050505,0|2|2E2E2E-050505,-1|0|2E2E2E-050505,-2|0|2E2E2E-050505,-2|1|-2E2E2E-050505,-1|1|-2E2E2E-050505,-1|2|-2E2E2E-050505", 1.0, 2, out qx1, out qy1);
                    object qx4;
                    object qy4;
                    dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "313131-050505", "0|1|2E2E2E-050505,0|2|2E2E2E-050505,1|0|2E2E2E-050505,2|0|2E2E2E-050505,2|1|-2E2E2E-050505,1|1|-2E2E2E-050505,1|2|-2E2E2E-050505", 1.0, 0, out qx4, out qy4);
                    // dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "313131-050505", "0|-1|2E2E2E-050505,0|-2|2E2E2E-050505,1|0|2E2E2E-050505,2|0|2E2E2E-050505,2|-1|-2E2E2E-050505,1|-1|-2E2E2E-050505,1|-2|-2E2E2E-050505", 1.0, 1, out qx4, out qy4);
                    sx1 = (int)qx4;
                    sy1 = (int)qy1;
                    width = (int)qx1 - (int)qx4;
                    height = width;
                    all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
                }

                if (type == 1)
                {
                    if (!dm.GetWindowClass(hwnd).ToLower().Equals("afxwnd140u"))
                    {
                        if (!isSimpleSync && startedSync)
                            MessageBox.Show(Program.isChn ? "未选择棋盘,同步失败" : "No board has been choosen,Sync failed");
                        isRightGoban = false;
                    }
                    sx1 = 0;
                    sy1 = 0;
                    width = x2 - x1;
                    height = y2 - y1;
                    if (!Program.isAdvScale)
                    {
                        width = (int)(width / factor);
                        height = (int)(height / factor);
                    }
                    all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
                }
                if (type == 2)
                {
                    if (!(dm.GetWindowClass(hwnd).ToLower().Equals("tlmdsimplepanel") || dm.GetWindowClass(hwnd).ToLower().Equals("tpanel")))
                    {
                        if (!isSimpleSync)
                            MessageBox.Show(Program.isChn ? "未选择棋盘,同步失败" : "No board has been choosen,Sync failed");
                        isRightGoban = false;
                    }
                    dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "FBDAA2-050505", "0|1|FBDAA2-050505,0|2|FBDAA2-050505", 1.0, 0, out qx1, out qy1);
                    object qx4;
                    object qy4;
                    dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "FBDAA2-050505", "0|1|FBDAA2-050505,0|2|FBDAA2-050505", 1.0, 1, out qx4, out qy4);
                    sx1 = (int)qx1 - 1;
                    sy1 = (int)qy1 - 1;
                    height = (int)qy4 - (int)qy1 + 4;
                    width = height;
                    all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
                }
                if (type == 3)
                {
                    sx1 = (int)(ox1 - x1);
                    sy1 = (int)(oy1 - y1);
                    width = (int)((ox2 - ox1));
                    height = (int)((oy2 - oy1));
                    if (!Program.isAdvScale)
                    {
                        sx1 = (int)(sx1 / factor);
                        sy1 = (int)(sy1 / factor);
                        width = (int)(width / factor);
                        height = (int)(height / factor);
                    }
                    all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
                    OutPut3(true, null);
                }
                else
                    OutPut(true, null);
            }

            keepSync = true;
            widthMagrin = width / (float)boardW;
            heightMagrin = height / (float)boardH;
            if (syncBoth)
            {
                if (radioBlack.Checked)
                {
                    Send("play>black>" + (textBox1.Text.Equals("") ? "0" : textBox1.Text) + " " + (textBox2.Text.Equals("") ? "0" : textBox2.Text) + " " + (textBox3.Text.Equals("") ? "0" : textBox3.Text));
                }
                else if (radioWhite.Checked)
                {
                    Send("play>white>" + (textBox1.Text.Equals("") ? "0" : textBox1.Text) + " " + (textBox2.Text.Equals("") ? "0" : textBox2.Text) + " " + (textBox3.Text.Equals("") ? "0" : textBox3.Text));
                }
            }
            if (Program.autoMin && isRightGoban && this.WindowState != FormWindowState.Minimized)
            {
                Action2<String> a = new Action2<String>(minWindow);
                Invoke(a, "");
            }
            if (!isRightGoban && !isContinuousSyncing)
            {
                stopSync();
            }
            ThreadStart threadStart = new ThreadStart(OutPutTime);
            thread = new Thread(OutPutTime);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            if (Program.showInBoard && type != 5)
            {
                object startX, startY;
                object rectWidth, rectHeight;
                dm.GetClientRect(hwnd, out (startX), out (startY), out (rectWidth), out (rectHeight));
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (isContinuousSyncing)
            {
                isContinuousSyncing = false;
                this.button10.Text = Program.isChn ? "一键同步" : "FastSync";
            }
            if (!startedSync)
            {
                startContinuousSync(false);
            }
            else
            {
                stopSync();
            }
        }

        private void stopSync()
        {
            dm.UnBindWindow();
            if (canUseLW)
            {
                lw.lwsoft lw = new lw.lwsoft();
                lw.UnBindWindow();
            }
            Send("stopsync");
            stopKeepingSync();
            keepSync = false;
        }

        public delegate void Action2<in T>(T t);

   

        private void OutPutTime()
        {                               
            while (keepSync)
            {
                  if (Program.showInBoard&&type!=5)
                {
                    object startX, startY;
                    object rectWidth, rectHeight;
                    dm.GetClientRect(hwnd, out(startX), out (startY), out (rectWidth), out (rectHeight));
                    if (!Program.isAdvScale)
                    {
                        if (type == 1)
                            Send("inboard " + (int)((sx1 + (int)startX) / factor) + " " + (int)((sy1 + (int)startY) / factor) + " " + (int)(width ) + " " + (int)(height ) + " " + (factor > 1 ? "99_" + factor + "_" + type.ToString() : type.ToString()));
                        else if (type == 2)
                            Send("inboard " + (int)((sx1 + (int)startX / factor) ) + " " + (int)((sy1 + (int)startY / factor) ) + " " + (int)(width ) + " " + (int)(height ) + " " + (factor > 1 ? "99_" + factor + "_" + type.ToString() : type.ToString()));
                       else if (type == 3)
                            Send("inboard " + (int)((sx1 + (int)startX / factor) ) + " " + (int)((sy1 + (int)startY / factor) ) + " " + (int)(width ) + " " + (int)(height ) + " " + (factor > 1 ? "99_" + factor + "_" + type.ToString() : type.ToString()));
                        else
                            Send("inboard " + (int)((sx1 + (int)startX) / factor) + " " + (int)((sy1 + (int)startY) / factor) + " " + (int)(width / factor) + " " + (int)(height / factor) + " " + (factor > 1 ? "99_" + factor + "_" + type.ToString() : type.ToString()));
                    }
                    else { 
                    if (type == 1 )
                        Send("inboard " + (int)((sx1 + (int)startX) / factor) + " " + (int)((sy1 + (int)startY) / factor) +" " + (int)(width / factor) + " " + (int)(height / factor) + " " + (factor>1?"99_"+factor+"_"+ type.ToString(): type.ToString()));
                    else if (type == 2)
                        Send("inboard " + (int)((sx1 + (int)startX) / factor) + " " + (int)((sy1 + (int)startY) / factor ) + " " + (int)(width / factor) + " " + (int)(height / factor) + " " + (factor > 1 ? "99_" + factor + "_" + type.ToString() : type.ToString()));
                    else
                        Send("inboard " + (int)((sx1 + (int)startX) / factor) + " " + (int)((sy1 + (int)startY) / factor) + " " + (int)(width / factor) + " " + (int)(height / factor) + " " + (factor > 1 ? "99_" + factor + "_" + type.ToString() : type.ToString()));
                    }
                    //    sx1, sy1,width,height
                }
                if (type == 5)
                {
                    if (!CaptureScreen(ox1, oy1, ox2, oy2))
                    {
                        return;
                    }
                    //m.Capture(ox1, oy1, ox2, oy2, "screen.bmp");
                    IntPtr hwnds;
                    if (form3 != null)
                    {
                        //form3.Close();
                        // form3.Dispose();
                        hwnds = form3.setPic();
                    }
                    else
                    {
                        form3 = new Form3();
                        hwnds = form3.setPic();
                        form3.Show();
                        dm.MoveWindow(form3.getHwnd(), 9999, 9999);
                    }
                    
                    dm.BindWindow((int)hwnds, "dx2", "windows", "normal", 0);
                    sx1 = 0;
                    sy1 = 0;
                    width = ox2 - ox1;
                    height = oy2 - oy1;
                    all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
                    OutPut3(false, null);
                }
                else
                {
                    int x1;
                    int y1;
                    int x2;
                    int y2;

                    RECT rect = new RECT();
                    IntPtr p = new IntPtr(hwnd);
                    GetWindowRect(p, ref rect);
                    x2 = rect.Right ;
                    y2 = rect.Bottom ;
                    x1 = rect.Left ;
                    y1 = rect.Top;
                    if ((int)x1 == 0 && (int)x2 == 0 && (int)y1 == 0 && (int)y2 == 0)
                    {
                        //MessageBox.Show("请选择棋盘,同步停止");
                        Send("stopsync");
                        stopKeepingSync();
                        return;
                    }
                    //    int m = dm.GetWindowRect(hwnd, out x1, out y1, out x2, out y2);
                    //if (m == 0)
                    //{       
                    //        if(canUseLW)
                    //        {
                    //            lw.lwsoft lwh;
                    //            lwh = new lw.lwsoft();
                    //            lwh.SetShowErrorMsg(0);
                    //            lwh.UnBindWindow();
                    //        }
                    //        button5click = false;
                            
                    //        Send("stopsync");
                    //        keepSync = false;
                    //        dm.UnBindWindow();
                    //        return;                
                    //}
                    int oldall = all;
                    if (type == 0)
                    {                        
                        dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "313131-050505", "0|1|2E2E2E-050505,0|2|2E2E2E-050505,-1|0|2E2E2E-050505,-2|0|2E2E2E-050505,-2|1|-2E2E2E-050505,-1|1|-2E2E2E-050505,-1|2|-2E2E2E-050505", 1.0, 2, out qx1, out qy1);
                        object qx4;
                        object qy4;
                        dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "313131-050505", "0|1|2E2E2E-050505,0|2|2E2E2E-050505,1|0|2E2E2E-050505,2|0|2E2E2E-050505,2|1|-2E2E2E-050505,1|1|-2E2E2E-050505,1|2|-2E2E2E-050505", 1.0, 0, out qx4, out qy4);
                        // dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "313131-050505", "0|-1|2E2E2E-050505,0|-2|2E2E2E-050505,1|0|2E2E2E-050505,2|0|2E2E2E-050505,2|-1|-2E2E2E-050505,1|-1|-2E2E2E-050505,1|-2|-2E2E2E-050505", 1.0, 1, out qx4, out qy4);
                        sx1 = (int)qx4;
                        sy1 = (int)qy1;
                        width = (int)qx1 - (int)qx4;
                        height = width;
                        all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
                    }
                    if (type == 1)
                    {
                        sx1 = 0;
                        sy1 = 0;
                        width = x2 - x1;
                        height = y2 - y1;
                        if (!Program.isAdvScale)
                        {
                            width = (int)(width / factor);
                            height = (int)(height / factor);
                        }
                        all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
                    }
                    if (type == 2)
                    {
                        dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "FBDAA2-050505", "0|1|FBDAA2-050505,0|2|FBDAA2-050505", 1.0, 0, out qx1, out qy1);
                        object qx4;
                        object qy4;
                        dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "FBDAA2-050505", "0|1|FBDAA2-050505,0|2|FBDAA2-050505", 1.0, 1, out qx4, out qy4);
                        sx1 = (int)qx1 - 1;
                        sy1 = (int)qy1 - 1;
                        height = (int)qy4 - (int)qy1 + 4;
                        width = height;
                        all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
                    }
                    //if (type == 3)
                    //{
                    //    //sx1 = (int)(ox1  - (int)x1);
                    //    //sy1 = (int)(oy1 - (int)y1);
                    //  //  width = (int)((ox2 - ox1) );
                    //    //height = (int)((oy2 - oy1) );
                    //    if (!Program.isAdvScale)
                    //    {
                    //        sx1 = (int)(sx1 / factor);
                    //        sy1 = (int)(sy1 / factor);
                    //        width = (int)(width / factor);
                    //        height = (int)(height / factor);
                    //    }
                    //    all = width / boardWidth * height / boardWidth;
                    //    all = width / boardWidth * height / boardWidth;
                    //}
                    if (oldall != all)
                    {
                        Send("clear");
                    }
                    if (type == 3)
                    {
                        if (canUseLW && syncBoth)
                        {
                            lw.lwsoft lwh;
                            lwh = new lw.lwsoft();
                            lwh.SetShowErrorMsg(0);

                            Thread.Sleep(100);
                            lwh.BindWindow(hwnd, 0, 4, 0, 0, 0);
                            OutPut3(false, lwh);
                        }
                        else
                            OutPut3(false, null);
                    }
                    // dm.BindWindowEx(hwnd, "gdi", "normal", "normal", "", 0);     
                    else
                   if (canUseLW && syncBoth)
                    {
                        lw.lwsoft lwh;
                        lwh = new lw.lwsoft();
                        lwh.SetShowErrorMsg(0);
                        Thread.Sleep(100);
                        lwh.BindWindow(hwnd, 0, 4, 0, 0, 0);
                        OutPut(false, lwh);
                    }
                    else
                        OutPut(false, null);
                }
                try
                {
                    Thread.Sleep(timeinterval);
                }
                catch (Exception) { }
            }
            Send("stopsync");
        }
    

    private void OutPut(Boolean first,lw.lwsoft lwh) {           
            if (lwh!=null&&savedPlace)
            {
                savedPlace = false;
                lwh.MoveTo((int)Math.Round(sx1 + widthMagrin * (savedX + 0.5)), (int)Math.Round(sy1 + heightMagrin * (savedY + 0.5)));
                lwh.LeftClick();
            }
            if (!(width <= boardW || height <= boardH))
            {
                String result = "";
                if(first)
                Send("start "+ boardW+" "+boardH+" " + hwnd);
                for (int i = 0; i < boardH; i++)
                {
                    for (int j = 0; j < boardW; j++)
                    {
                        int numw = 0;
                        if (type == 1)
                        {
                            numw = dm.GetColorNum(sx1 + (int)Math.Round((width * j) / (float)boardW) , sy1 + (int)Math.Round((height * i )/ (float)boardH) , sx1 + (int)Math.Round((width* (j + 1)) / (float)boardW) , sy1 + (int)Math.Round((height * (i + 1)) / (float)boardH) , "FFFFFF-505050", 1.0);
                        }
                        else
                            numw = dm.GetColorNum(sx1 + (int)Math.Round((width * j) / (float)boardW), sy1 + (int)Math.Round((height * i) / (float)boardH), sx1 + (int)Math.Round((width * (j + 1)) / (float)boardW), sy1 + (int)Math.Round((height * (i + 1)) / (float)boardH), "FFFFFF-707070", 1.0);
                     if ((type!=1&&numw < all * 0.32)||(type == 1&& numw < all * 0.30))
                        {
                            int numb = dm.GetColorNum(sx1 + (int)Math.Round((width * j) / (float)boardW), sy1 + (int)Math.Round((height * i) / (float)boardH), sx1 + (int)Math.Round((width * (j + 1)) / (float)boardW), sy1 + (int)Math.Round((height * (i + 1)) / (float)boardH), "000000-606060", 1.0);
                            if (numb < all * 0.37)
                            {
                                result = result + "0,";
                            }
                            else
                            {
                                if (noticeLast)
                                {
                                    if (type == 0)
                                    {
                                        if (all * 0.1 < numw && numw < all * 0.17)
                                            result = result + "3,";
                                        else
                                        {
                                            int numblue = dm.GetColorNum(sx1 + (int)Math.Round((width * j) / (float)boardW), sy1 + (int)Math.Round((height * i) / (float)boardH), sx1 + (int)Math.Round((width * (j + 1)) / (float)boardW), sy1 + (int)Math.Round((height * (i + 1)) / (float)boardH), "FF0000-202020", 1.0);
                                            if (numblue > all * 0.02)
                                                result = result + "3,";
                                            else
                                                result = result + "1,";
                                        }
                                    }
                                    else if (type == 1)
                                    {
                                        if ( numw > all * 0.05)
                                            result = result + "3,";
                                        else
                                        {
                                            int numred = dm.GetColorNum(sx1 + (int)Math.Round((width * j) / (float)boardW), sy1 + (int)Math.Round((height * i) / (float)boardH), sx1 + (int)Math.Round((width * (j + 1)) / (float)boardW), sy1 + (int)Math.Round((height * (i + 1)) / (float)boardH), "FF0000-202020", 1.0);
                                            //  dm.GetColorNum(sx1 + (width / boardWidth) * j, sy1 + (height / boardWidth) * i, sx1 + (width / boardWidth) * (j + 1), sy1 + (height / boardWidth) * (i + 1), "FF0000-202020", 1.0);
                                            if (numred > all * 0.005)
                                                result = result + "3,";
                                            else
                                                result = result + "1,";
                                        }
                                    }
                                    else if (type == 2) {
                                        int numred = dm.GetColorNum(sx1 + (int)Math.Round((width * j) / (float)boardW), sy1 + (int)Math.Round((height * i) / (float)boardH), sx1 + (int)Math.Round((width * (j + 1)) / (float)boardW), sy1 + (int)Math.Round((height * (i + 1)) / (float)boardH), "FFFF00-202020", 1.0);

                                        //dm.GetColorNum(sx1 + (width / boardWidth) * j, sy1 + (height / boardWidth) * i, sx1 + (width / boardWidth) * (j + 1), sy1 + (height / boardWidth) * (i + 1), "FFFF00-202020", 1.0);
                                        if (numred > all * 0.02)
                                            result = result + "3,";
                                        else
                                            result = result + "1,";
                                    }
                                }
                                else {
                                    result = result + "1,";
                                }
                            }
                        }
                        else
                        {
                            if (noticeLast)
                            {
                                if (type == 0)
                                {
                                    int numb = dm.GetColorNum(sx1 + (int)Math.Round((width * j) / (float)boardW), sy1 + (int)Math.Round((height * i) / (float)boardH), sx1 + (int)Math.Round((width * (j + 1)) / (float)boardW), sy1 + (int)Math.Round((height * (i + 1)) / (float)boardH), "000000-202020", 1.0);
                                     if (all * 0.1 < numb && numb < all * 0.2)
                                        result = result + "4,";
                                    else
                                    {
                                        int numblue = dm.GetColorNum(sx1 + (int)Math.Round((width * j) / (float)boardW), sy1 + (int)Math.Round((height * i) / (float)boardH), sx1 + (int)Math.Round((width * (j + 1)) / (float)boardW), sy1 + (int)Math.Round((height * (i + 1)) / (float)boardH), "0000FF-202020", 1.0);

                                        if (numblue > all * 0.02)
                                            result = result + "4,";
                                        else
                                            result = result + "2,";
                                    }
                                }
                                else if (type == 1)
                                {
                                    int numblue = dm.GetColorNum(sx1 + (int)Math.Round((width * j) / (float)boardW), sy1 + (int)Math.Round((height * i) / (float)boardH), sx1 + (int)Math.Round((width * (j + 1)) / (float)boardW), sy1 + (int)Math.Round((height * (i + 1)) / (float)boardH), "0000FF-202020", 1.0);

                                    if (numblue > all * 0.005)
                                        result = result + "4,";
                                    else
                                    {
                                        int numred = dm.GetColorNum(sx1 + (int)Math.Round((width * j) / (float)boardW), sy1 + (int)Math.Round((height * i) / (float)boardH), sx1 + (int)Math.Round((width * (j + 1)) / (float)boardW), sy1 + (int)Math.Round((height * (i + 1)) / (float)boardH), "FF0000-202020", 1.0);
                                                                                
                                        if (numred > all * 0.005)
                                            result = result + "4,";
                                        else
                                            result = result + "2,";
                                    }
                                }

                                else if (type == 2)
                                {
                                    int numred = dm.GetColorNum(sx1 + (int)Math.Round((width * j) / (float)boardW), sy1 + (int)Math.Round((height * i) / (float)boardH), sx1 + (int)Math.Round((width * (j + 1)) / (float)boardW), sy1 + (int)Math.Round((height * (i + 1)) / (float)boardH), "000080-202020", 1.0);

                                    if (numred > all * 0.02)
                                        result = result + "4,";
                                    else
                                        result = result + "2,";
                                }
                            }
                            else {
                                result = result + "2,";
                            }
                        }
                        if (j == (boardW-1))
                        {
                            if(result.Length>1)
                            result = result.Substring(0, result.Length - 1);
                            Send("re=" + result);
                            result = "";
                        }
                    }
                }
                Send("end");
            }
        }

        private void OutPut3(Boolean first, lw.lwsoft lwh)
        {
            double zhanbiB = Program.blackZB / 100.0;
            double zhanbiW = Program.whiteZB / 100.0;
            String pianyiB = Convert.ToString(Program.blackPC, 16);
            String pianyiW = Convert.ToString(Program.whitePC, 16);           
            if (lwh != null && savedPlace)
            {
                savedPlace = false;
                lwh.MoveTo((int)Math.Round(sx1 + widthMagrin * (savedX + 0.5)), (int)Math.Round(sy1 + heightMagrin * (savedY + 0.5)));
                lwh.LeftClick();
            }
            if (width <= boardW || height <= boardH)
                return;
            String result = "";
            if (first)
                Send("start " + boardW + " " + boardH +" "+ hwnd);
            for (int i = 0; i < boardH; i++)
            {
                for (int j = 0; j < boardW; j++)
                {
                    int numb = dm.GetColorNum(sx1 + (int)Math.Round((width * j) / (float)boardW), sy1 + (int)Math.Round((height * i) / (float)boardH), sx1 + (int)Math.Round((width * (j + 1)) / (float)boardW), sy1 + (int)Math.Round((height * (i + 1)) / (float)boardH), "000000-" + pianyiB + pianyiB + pianyiB, 1.0);

                    if (numb < all * zhanbiB)
                    {
                        int numw = dm.GetColorNum(sx1 + (int)Math.Round((width * j) / (float)boardW), sy1 + (int)Math.Round((height * i) / (float)boardH), sx1 + (int)Math.Round((width * (j + 1)) / (float)boardW), sy1 + (int)Math.Round((height * (i + 1)) / (float)boardH), "FFFFFF-" + pianyiW + pianyiW + pianyiW, 1.0); 
                        
                        if (numw < all * zhanbiW)
                        {

                            result = result + "0,";
                        }
                        else
                        {
                            if (j == 0 || j == boardW - 1 || i == 0 || i == boardH - 1) {
                                if (numw > all * 0.85)
                                    result = result + "0,";
                                else
                                    result = result + "2,";
                            }
                            else
                            {
                                if (numw > all * 0.8)
                                    result = result + "0,";
                                else
                                    result = result + "2,";
                            }
                        }
                    }
                    else
                    {                       
                            result = result + "1,";                      
                    }
                    if (j == (boardW - 1))
                    {
                        result = result.Substring(0, result.Length - 1);
                        Send("re=" + result);
                        result = "";
                    }
                }
            }
            Send("end");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Send("clear");          
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoFox.Checked)
            {
                type = 0;             
                this.button2.Enabled = false;
                this.button11.Enabled = false;
                this.button3.Enabled = true;
                this.button10.Enabled = true;
                if (this.rdoOtherBoard.Checked)
                    this.rdo19x19.Checked = true;
                this.rdoOtherBoard.Enabled = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoTygem.Checked)
            {
                type = 1;
                this.button2.Enabled = false;
                this.button11.Enabled = false;
                this.button3.Enabled = true;
                this.button10.Enabled = true;
                if (this.rdoOtherBoard.Checked)
                    this.rdo19x19.Checked = true;
                this.rdoOtherBoard.Enabled = false;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoBack.Checked)
            {
                type = 3;
                this.button3.Enabled = false;
                this.button11.Enabled = true;
                this.button2.Enabled = true;
                this.button10.Enabled = false;
                this.rdoOtherBoard.Enabled = true;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

            if (this.rdoSina.Checked)
            {
                type = 2;
                this.button2.Enabled = false;
                this.button11.Enabled =false;
                this.button3.Enabled = true;
                this.button10.Enabled = true;
                if (this.rdoOtherBoard.Checked)
                    this.rdo19x19.Checked = true;
                this.rdoOtherBoard.Enabled = false;
            }
           
        }

        public void saveBoardSize() {
            string result1 = "config_readboard_boardsize.txt";
            FileStream fs = new FileStream(result1, FileMode.Create);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            int customW = -1;
            int customH = -1;
            try {
                customW = int.Parse(txtBoardWidth.Text);
                customH = int.Parse(txtBoardHeight.Text);
            }
            catch (Exception )
            {
            }
            wr.WriteLine(this.boardW+"_"+this.boardH+"_"+ customW + "_"+ customH);
            wr.Close();
        }

        public void shutdown() {
            isContinuousSyncing = false;
            keepSync = false;
            string result1 = "config_readboard.txt";
            FileStream fs = new FileStream(result1, FileMode.Create);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            wr.WriteLine(Program.blackPC + "_" + Program.blackZB + "_" + Program.whitePC + "_" + Program.whiteZB + "_" + (Program.useMag ? "1" : "0") + "_" + (Program.doubleClick ? "1" : "0") + "_" + (Program.showScaleHint ? "1" : "0") + "_" + (Program.showInBoard ? "1" : "0") + "_" + (Program.showInBoardHint ? "1" : "0") + "_" + (Program.autoMin ? "1" : "0") + "_" + (Program.isAdvScale ? "1" : "0") + "_" + Environment.GetEnvironmentVariable("computername").Replace("_", "") + "_" + type);
            wr.Close();
            mh.Enabled = false;
            if (dm.IsBind(hwnd) > 0)
            {
                dm.UnBindWindow();
                dm.Dispose();
            }
            dm2.Dispose();
            if (canUseLW)
            {
                lw.lwsoft lw = new lw.lwsoft();
                if (lw.IsBind(hwnd) > 0)
                    lw.UnBindWindow();
            }
            Send("stopsync");
            Send("nobothSync");
            Send("endsync");
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            Application.Exit();
        }

        private void form_closing(object sender, FormClosingEventArgs e)
        {
            shutdown();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdo19x19.Checked)
            {
                boardW = 19;
                boardH = 19;
                this.txtBoardHeight.BackColor = System.Drawing.SystemColors.Menu;
                this.txtBoardWidth.BackColor = System.Drawing.SystemColors.Menu;
            }
            saveBoardSize();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdo13x13.Checked)
            {
                boardW = 13;
                boardH = 13;
                this.txtBoardHeight.BackColor = System.Drawing.SystemColors.Menu;
                this.txtBoardWidth.BackColor = System.Drawing.SystemColors.Menu;
            }
            saveBoardSize();
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdo9x9.Checked)
            {
                boardW = 9;
                boardH = 9;
                this.txtBoardHeight.BackColor = System.Drawing.SystemColors.Menu;
                this.txtBoardWidth.BackColor = System.Drawing.SystemColors.Menu;
            }
            saveBoardSize();
        }

        public void sendVersion() {
            Send("version: "+Program.version);
        }

        public void stopInBoard()
        {
            this.chkShowInBoard.Checked = false;
        }
        public void lossFocus()
        {
            int hwnd8;
            if (form8 != null)
            {
                hwnd8 = form8.getHwnd();
            }
            else
            {
                form8 = new Form8();
                hwnd8 = form8.getHwnd();
                //form8.Show();
               
            }
            if (dm.GetWindowState(hwnd8, 1) == 0)
            {
                dm.SetWindowState(hwnd8, 1);               
            }
          //  dm.MoveWindow(hwnd8, -9999, -5000);
         //   dm.SetWindowState(hwnd8, 0);
        }

        public void place(int x,int y)
        {
            if (!keepSync)
                return;
            if (syncBoth&&type == 5)
            {
                object xo, yo;
                dm2.GetCursorPos(out xo, out yo);
                dm2.MoveTo(sx1ty5 + (int)(widthMagrin * (x + 0.5)), sy1ty5 + (int)(heightMagrin * (y + 0.5)));
                //Thread.Sleep(50);
                dm2.LeftClick();
                //Thread.Sleep(50);
                if(Program.doubleClick)
                dm2.LeftClick();
                dm2.MoveTo((int)xo, (int)yo);
            }
            else
            if (syncBoth&&hwnd!=0)
            {
                if (type == 0 && canUseLW)
                {
                    savedPlace = true;
                    savedX = x;
                    savedY = y;
                }
                else
                {
                    if (type == 0)
                    {
                        object xo, yo;
                        dm2.GetCursorPos(out xo, out yo);

                        dm.MoveTo((int)Math.Round(sx1 + widthMagrin * (x + 0.5)), (int)Math.Round(sy1 + heightMagrin * (y + 0.5)));
                        //Thread.Sleep(20);
                        dm.LeftClick();
                        dm.MoveTo((int)Math.Round(sx1 + widthMagrin * (x + 0.5)), (int)Math.Round(sy1 + heightMagrin * (y + 0.5)));
                        dm.LeftClick();
                        //Thread.Sleep(20);
                        dm2.MoveTo((int)xo, (int)yo);
                    }
                    else
                    {         
                        if(Program.isAdvScale)
                            dm.MoveTo((int)Math.Round(sx1 + widthMagrin * (x + 0.5)), (int)Math.Round(sy1 + heightMagrin * (y + 0.5)));
                        else
                            dm.MoveTo((int)Math.Round((sx1 + widthMagrin * (x + 0.5))*factor), (int)Math.Round((sy1 + heightMagrin * (y + 0.5))*factor));
                        dm.LeftClick();
                    }
                }
            }
        }
 

        private void textbox1_TextChanged(object sender, EventArgs e)
        {
            var reg = new Regex("^[0-9]*$");
            var str = textBox1.Text.Trim();
            var sb = new StringBuilder();
            if (!reg.IsMatch(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (reg.IsMatch(str[i].ToString()))
                    {
                        sb.Append(str[i].ToString());
                    }
                }
                textBox1.Text = sb.ToString();
                textBox1.SelectionStart = textBox1.Text.Length;
            }
            Send("timechanged "+ (textBox1.Text.Equals("") ? "0" : textBox1.Text));
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            Send("noponder");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            var reg = new Regex("^[0-9]*$");
            var str = textBox2.Text.Trim();
            var sb = new StringBuilder();
            if (!reg.IsMatch(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (reg.IsMatch(str[i].ToString()))
                    {
                        sb.Append(str[i].ToString());
                    }
                }
                textBox2.Text = sb.ToString();
                textBox2.SelectionStart = textBox2.Text.Length;
            }
            Send("playoutschanged " + (textBox2.Text.Equals("") ? "0" : textBox2.Text));
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            var reg = new Regex("^[0-9]*$");
            var str = textBox3.Text.Trim();
            var sb = new StringBuilder();
            if (!reg.IsMatch(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (reg.IsMatch(str[i].ToString()))
                    {
                        sb.Append(str[i].ToString());
                    }
                }
                textBox3.Text = sb.ToString();
                textBox3.SelectionStart = textBox3.Text.Length;
            }
            Send("firstchanged " + (textBox3.Text.Equals("") ? "0" : textBox3.Text));
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            syncBoth = checkBox1.Checked;
            if (syncBoth)
            { Send("bothSync"); }
            else
            { Send("nobothSync"); }
            if (keepSync)
            {
                keepSync = false;
                Thread.Sleep(timeinterval*3/2 + 50);               
                keepSync = true;
                Send("sync");
                if (radioBlack.Checked)
                    {
                        Send("play>black>" + (textBox1.Text.Equals("") ? "0" : textBox1.Text) + " " + (textBox2.Text.Equals("") ? "0" : textBox2.Text) + " " + (textBox3.Text.Equals("") ? "0" : textBox3.Text));
                    }
                    else if (radioWhite.Checked)
                    {
                        Send("play>white>" + (textBox1.Text.Equals("") ? "0" : textBox1.Text) + " " + (textBox2.Text.Equals("") ? "0" : textBox2.Text) + " " + (textBox3.Text.Equals("") ? "0" : textBox3.Text));
                    }             
                ThreadStart threadStart = new ThreadStart(OutPutTime);
                thread = new Thread(OutPutTime);
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {           
            try {
                Process  process1 = new Process();
                process1.StartInfo.FileName = "readboard\\readme.rtf";
                process1.StartInfo.Arguments = "";
                process1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                process1.Start();
            }
            catch (Exception)
            {
                MessageBox.Show(Program.isChn ?"找不到说明文档,请检查Lizzie目录下[readboard]文件夹内的[readme.rtf]文件是否存在":"Can not find file,Please check [readme.rtf] file is in the folder [readboard]");
            }
        }

        private void rdoqiantai_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoFore.Checked)
            {
                type = 5;
                this.button3.Enabled = false;
                this.button11.Enabled = true;
                this.button2.Enabled = true;
                this.button10.Enabled = false;
                this.rdoOtherBoard.Enabled = true;
            }
        }

        private void chkAutoPlay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoPlay.Checked)
            {
                radioWhite.Enabled = true;
                radioBlack.Enabled = true;
                textBox1.Enabled = true;
                    textBox2.Enabled = true;
                textBox3.Enabled = true;
            }
            else {
                radioWhite.Checked = false;
                radioBlack.Checked = false;
                radioWhite.Enabled = false;
                radioBlack.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                Send("stopAutoPlay");
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Form4 form4=new Form4();
            form4.Show();
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoOtherBoard.Checked)
            {
                try
                {
                    this.boardW = int.Parse(txtBoardWidth.Text);
                    this.boardH = int.Parse(txtBoardHeight.Text);
                }
                catch (Exception)
                {
                  // MessageBox.Show(Program.isChn?"错误的棋盘大小":"Wrong goban size!");
                }
            }
            saveBoardSize();
        }



        private void parseWidth(object sender, EventArgs e)        
        {
            try
            {
                if (this.rdoOtherBoard.Checked)
                {
                    this.boardW = int.Parse(txtBoardWidth.Text);
                    saveBoardSize();
                }
                else
                {
                    int w = int.Parse(txtBoardWidth.Text);
                }
            }
            catch (Exception)
            {
                txtBoardWidth.BackColor = Color.Red;
            }            
        }

        private void parseHeight(object sender, EventArgs e)
        {
            try
            {
                if (this.rdoOtherBoard.Checked)
                {
                    this.boardH = int.Parse(txtBoardHeight.Text);
                    saveBoardSize();
                }
                else
                {
                    int h = int.Parse(txtBoardHeight.Text);
                }
            }
            catch (Exception)
            {
                txtBoardHeight.BackColor = Color.Red;
            }
        }

        private void tb_KeyPressWidth(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar == '\b' || (e.KeyChar >= '0' && e.KeyChar <= '9')))
            {
                e.Handled = true;
            }
            txtBoardWidth.BackColor = System.Drawing.SystemColors.Menu;
        }

        private void tb_KeyPressHeight(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar == '\b' || (e.KeyChar >= '0' && e.KeyChar <= '9')))
            {
                e.Handled = true;
            }
            txtBoardHeight.BackColor = System.Drawing.SystemColors.Menu;
        }

        private void radioBlack_CheckedChanged(object sender, EventArgs e)
        {
            if (keepSync)
                if (radioBlack.Checked)
            {
                Send("play>black>" + (textBox1.Text.Equals("") ? "0" : textBox1.Text) + " " + (textBox2.Text.Equals("") ? "0" : textBox2.Text) + " " + (textBox3.Text.Equals("") ? "0" : textBox3.Text));
            }
            else if (radioWhite.Checked)
            {
                Send("play>white>" + (textBox1.Text.Equals("") ? "0" : textBox1.Text) + " " + (textBox2.Text.Equals("") ? "0" : textBox2.Text) + " " + (textBox3.Text.Equals("") ? "0" : textBox3.Text));
            }
        }

        private void radioWhite_CheckedChanged(object sender, EventArgs e)
        {
            if(keepSync)
            if (radioBlack.Checked)
            {
                Send("play>black>" + (textBox1.Text.Equals("") ? "0" : textBox1.Text) + " " + (textBox2.Text.Equals("") ? "0" : textBox2.Text) + " " + (textBox3.Text.Equals("") ? "0" : textBox3.Text));
            }
            else if (radioWhite.Checked)
            {
                Send("play>white>" + (textBox1.Text.Equals("") ? "0" : textBox1.Text) + " " + (textBox2.Text.Equals("") ? "0" : textBox2.Text) + " " + (textBox3.Text.Equals("") ? "0" : textBox3.Text));
            }
        }

     
        private void button8_Click(object sender, EventArgs e)
        {
            Send("pass");
        }

        private void chkShowInBoard_CheckedChanged(object sender, EventArgs e)
        {
            Program.showInBoard = chkShowInBoard.Checked;
            string result1 = "config_readboard.txt";
            FileStream fs = new FileStream(result1, FileMode.Create);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            wr.WriteLine(Program.blackPC + "_" + Program.blackZB + "_" + Program.whitePC + "_" + Program.whiteZB + "_" + (Program.useMag ? "1" : "0") + "_" + (Program.doubleClick ? "1" : "0") + "_" + (Program.showScaleHint ? "1" : "0") + "_" + (Program.showInBoard ? "1" : "0") + "_" + (Program.showInBoardHint ? "1" : "0") + "_" + (Program.autoMin ? "1" : "0") + "_" + (Program.isAdvScale ? "1" : "0") + "_" + Environment.GetEnvironmentVariable("computername").Replace("_", "")+"_"+type);
            wr.Close();
            if (chkShowInBoard.Checked)
            {
                if (Program.showInBoardHint)
                {

                    Form7 form7 = new Form7();
                    form7.ShowDialog();

                }
                if (this.keepSync && type != 5)
                {
                    object startX, startY;
                    object rectWidth, rectHeight;
                    dm.GetClientRect(hwnd, out (startX), out (startY), out (rectWidth), out (rectHeight));
                       }
            }
            else {
                Send("notinboard");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                Process process1 = new Process();
                process1.StartInfo.FileName = "readboard\\65komi.rtf";
                process1.StartInfo.Arguments = "";
                process1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                process1.Start();
            }
            catch (Exception)
            {
                MessageBox.Show(Program.isChn?"找不到说明文档,请检查Lizzie目录下[readboard]文件夹内的[65komi.rtf]文件是否存在": "Can not find file,Please check [65komi.rtf] file is in the folder [readboard]");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (!isContinuousSyncing)
            {
                isContinuousSyncing = true;
                ThreadStart threadStart = new ThreadStart(startContinuous);
                this.button3.Enabled = false;
                this.button4.Enabled = false;
                this.button5.Enabled = false;
            thread = new Thread(startContinuous);
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                if (Program.autoMin  && this.WindowState != FormWindowState.Minimized)
                {
                    minWindow("");
                }              
            }
            else
            {
                isContinuousSyncing = false;
                dm.UnBindWindow();
                if (canUseLW)
                {
                    lw.lwsoft lw = new lw.lwsoft();
                    lw.UnBindWindow();
                }
                Send("stopsync");
                stopKeepingSync();               
                this.button10.Text = Program.isChn ? "一键同步" : "FastSync";
                keepSync = false;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            isMannulCircle = false;
            //Form5 form5 = new Form5();
            //object x;
            //object y;
            //dm.GetCursorPos(out x, out y);

            //form5.setPic((int)x, (int)y);
            //form5.Show();
            selectBoard();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            isMannulCircle = true;
            selectBoard();
        }

        private Boolean reCalculateRow1(int x, int y, int x2, int y2)
        {
            if (x2 > x && y2 > y)
            {
                int width = x2 - x;
                int height = y2 - y;
                Program.bitmap = new Bitmap(width, height);
                using (System.Drawing.Graphics graphics = Graphics.FromImage(Program.bitmap))
                {
                    graphics.CopyFromScreen(x, y, 0, 0, new System.Drawing.Size((int)(width * factor), (int)(height * factor)));
                  //  bitmap.Save("screen.bmp");
                }
                try {
                    boardLineAjust(boardW, boardH);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                //  dm2.Capture(x, y, x2, y2, "screen.bmp");
                return true;
            }
            else
            {
             //   MessageBox.Show(Program.isChn ? "未选择棋盘" : "No board has been choosen");
                return false;
            }
        }

        private void selectBoard() {
            mh.Enabled = true;
            this.WindowState = FormWindowState.Minimized;
            form2 = new Form2(isMannulCircle);
            form2.ShowDialog();
        }

        private List<verticalLine> verticalLines = new List<verticalLine>();
        private List<horizonLine> horizonLines = new List<horizonLine>();

        private void boardLineAjust(int boardWidth1,int boardHeight1)
        {
            Mat srcImage = OpenCvSharp.Extensions.BitmapConverter.ToMat(Program.bitmap);
            //Mat src_gray = new Mat();
            //   Mat copy = new Mat();
            //  Mat copy2 = new Mat();
            //if (showDebugImage)
            //{
            //    srcImage.CopyTo(copy);
            //    srcImage.CopyTo(copy2);
            //Cv2.CvtColor(srcImage, srcImage, ColorConversionCodes.BGR2HSV);  // 色彩空间转换为hsv，便于分离
            //Scalar lowerb = new Scalar(0, 0,0);
            //Scalar upperb = new Scalar(50, 50, 50);
            //Cv2.InRange(srcImage, lowerb, upperb, src_gray);
            //Cv2.ImShow("黑色", src_gray);
            double lowCanny = 200; double upCanny = 550;
          
            LineSegmentPoint[] linesPoint = FindLines(srcImage, boardWidth1, boardHeight1, (int)lowCanny, (int)upCanny);
            CalculateLines(linesPoint);
            int lines = verticalLines.Count+ horizonLines.Count;
            int maxTimes = 10;
            while (lines < 10&& maxTimes>0)
            {
                lowCanny = lowCanny * 0.9;
                upCanny = upCanny * 0.9;
                linesPoint = FindLines(srcImage, boardWidth1, boardHeight1, (int)lowCanny, (int)upCanny);
                CalculateLines(linesPoint);
                lines = verticalLines.Count + horizonLines.Count;
                maxTimes--;
            }
          
            //if (showDebugImage)
            //{
            //    for (int i = 0; i < horizonLines.Count; i++)
            //    {
            //        Cv2.Line(copy, horizonLines[i].startPoint, horizonLines[i].endPoint, new Scalar(0, 255, 0), 1);
            //    }
            //    for (int i = 0; i < verticalLines.Count; i++)
            //    {
            //        Cv2.Line(copy, verticalLines[i].startPoint, verticalLines[i].endPoint, new Scalar(0, 255, 0), 1);
            //    }
            //    Cv2.ImShow("line", copy);
            //}
            List<int> verticalGaps = new List<int>();
            for (int s = 0; s < verticalLines.Count - 1; s++)
            {
                verticalLine line = verticalLines[s];
                verticalLine line2 = verticalLines[s + 1];
                int gap = line2.position - line.position;
                verticalGaps.Add(gap);
            }

            List<gapCount> verticalGapCounts = new List<gapCount>();
            for (int s = 0; s < verticalGaps.Count; s++)
            {
                int gap = verticalGaps[s];
                Boolean isNew = true;
                for (int n = 0; n < verticalGapCounts.Count; n++)
                {
                    gapCount gapCount = verticalGapCounts[n];
                    if (gap == gapCount.gap)
                    {
                        isNew = false;
                    }
                }
                if (isNew)
                {
                    gapCount gapCount = new gapCount();
                    gapCount.gap = gap;
                    verticalGapCounts.Add(gapCount);
                }
            }
            for (int s = 0; s < verticalGaps.Count; s++)
            {
                int gap = verticalGaps[s];
                for (int m = 0; m < verticalGapCounts.Count; m++)
                {
                    gapCount gapCount = verticalGapCounts[m];
                    if (gap == gapCount.gap)
                    {
                        gapCount.looseCounts++;
                        gapCount.uniqueCounts++;
                    }
                    else if (Math.Abs(gap - gapCount.gap) <= 3)
                    {
                        gapCount.looseCounts++;
                    }
                }
            }

            verticalGapCounts.Sort(delegate (gapCount obj1, gapCount obj2) {
                int res = 0;
                if ((null == obj1) && (null == obj2)) return 0;
                else if ((null != obj1) && (null == obj2)) return 1;
                else if ((null == obj1) && (null != obj2)) return -1;

                if (obj1.uniqueCounts > obj2.uniqueCounts) res = -1;
                else if (obj1.uniqueCounts == obj2.uniqueCounts)
                {
                    if (obj1.looseCounts > obj2.looseCounts) res = -1;
                    else res = 1;
                }
                else res = 1;
                return res;
            });   

            List<int> horizonGaps = new List<int>();
            for (int s = 0; s < horizonLines.Count - 1; s++)
            {
                horizonLine line = horizonLines[s];
                horizonLine line2 = horizonLines[s + 1];
                int gap = line2.position - line.position;
                horizonGaps.Add(gap);
            }

            List<gapCount> horizonGapCounts = new List<gapCount>();
            for (int s = 0; s < horizonGaps.Count; s++)
            {
                int gap = horizonGaps[s];
                Boolean isNew = true;
                for (int n = 0; n < horizonGapCounts.Count; n++)
                {
                    gapCount gapCount = horizonGapCounts[n];
                    if (gap == gapCount.gap)
                    {
                        isNew = false;
                    }
                }
                if (isNew)
                {
                    gapCount gapCount = new gapCount();
                    gapCount.gap = gap;
                    horizonGapCounts.Add(gapCount);
                }
            }
            for (int s = 0; s < horizonGaps.Count; s++)
            {
                int gap = horizonGaps[s];
                for (int m = 0; m < horizonGapCounts.Count; m++)
                {
                    gapCount gapCount = horizonGapCounts[m];
                    if (gap == gapCount.gap)
                    {
                        gapCount.looseCounts++;
                        gapCount.uniqueCounts++;
                    }
                    else if (Math.Abs(gap - gapCount.gap) <= 3)
                    {
                        gapCount.looseCounts++;
                    }
                }
            }

            horizonGapCounts.Sort(delegate (gapCount obj1, gapCount obj2) {
                int res = 0;
                if ((null == obj1) && (null == obj2)) return 0;
                else if ((null != obj1) && (null == obj2)) return 1;
                else if ((null == obj1) && (null != obj2)) return -1;

                if (obj1.uniqueCounts > obj2.uniqueCounts) res = -1;
                else if (obj1.uniqueCounts == obj2.uniqueCounts)
                {
                    if (obj1.looseCounts > obj2.looseCounts) res = -1;
                    else res = 1;
                }
                else res = 1;
                return res;
            });

            int verticalGap = verticalGapCounts[0].gap;
            int horizonGap = horizonGapCounts[0].gap;

            if (verticalGapCounts.Count >= 3)
            {
                if ((Math.Abs(verticalGapCounts[0].gap - verticalGapCounts[1].gap) <= 2) && (Math.Abs(verticalGapCounts[0].gap - verticalGapCounts[2].gap) <= 2))
                    if (((verticalGapCounts[1].uniqueCounts / (float)verticalGapCounts[0].uniqueCounts) > 0.5) && ((verticalGapCounts[2].uniqueCounts / (float)verticalGapCounts[0].uniqueCounts) > 0.5))
                    {
                        int gap0 = verticalGapCounts[0].gap;
                        int gap1 = verticalGapCounts[1].gap;
                        int gap2 =verticalGapCounts[2].gap;
                        if ((gap0 > gap1 && gap1 > gap2) || (gap2 > gap1 && gap1 > gap0))
                        {
                            verticalGap = gap1;
                        }
                        else if ((gap1 > gap0 && gap0 > gap2) || (gap2 > gap0 && gap0 > gap1))
                        {
                            verticalGap = gap0;
                        }
                        else
                        {
                            verticalGap = gap2;
                        }
                    }
            }
       
            if (horizonGapCounts.Count >= 3)
            {
                if ((Math.Abs(horizonGapCounts[0].gap - horizonGapCounts[1].gap) <= 2) && (Math.Abs(horizonGapCounts[0].gap - horizonGapCounts[2].gap) <= 2))
                    if (((horizonGapCounts[1].uniqueCounts / (float)horizonGapCounts[0].uniqueCounts) > 0.5) && ((horizonGapCounts[2].uniqueCounts / (float)horizonGapCounts[0].uniqueCounts) > 0.5))
                    {
                        int gap0 = horizonGapCounts[0].gap;
                        int gap1 = horizonGapCounts[1].gap;
                        int gap2 = horizonGapCounts[2].gap;
                        if ((gap0 > gap1 && gap1 > gap2) || (gap2 > gap1 && gap1 > gap0))
                        {
                            horizonGap = gap1;
                        }
                        else if ((gap1 > gap0 && gap0 > gap2) || (gap2 > gap0 && gap0 > gap1))
                        {
                            horizonGap = gap0;
                        }
                        else
                        {
                            horizonGap = gap2;
                        }
                    }
            }
            //    Console.WriteLine("V gap: " + verticalGap);
            //      Console.WriteLine("H gap: " + horizonGap);
            for (int n = 0; n < verticalLines.Count; n++)
            {
                verticalLine line1 = verticalLines[n];
                for (int m = n + 1; m < verticalLines.Count; m++)
                {
                    verticalLine line2 = verticalLines[m];
                    if (Math.Abs(line2.position - line1.position - verticalGap) <= Math.Max(1,verticalGap/8))
                    {      line1.validate = true;
                    line2.validate = true;
                }
                }
            }

            for (int n = 0; n < horizonLines.Count; n++)
            {
                horizonLine line1 = horizonLines[n];
                for (int m = n + 1; m < horizonLines.Count; m++)
                {
                    horizonLine line2 = horizonLines[m];
                    if (Math.Abs(line2.position - line1.position - horizonGap) <= Math.Max(1,horizonGap /8))
                    {
                        line1.validate = true;
                        line2.validate = true;
                    }
                }
            }


            int boardW = verticalGap * (boardWidth1 - 1);
            int boardH = horizonGap * (boardHeight1 - 1);

            List<wholeOffset> verticalOffsets = new List<wholeOffset>();
            for (int s = 0; s < verticalLines.Count; s++)
            {
                verticalLine line = verticalLines[s];
                for (int n = s + 1; n < verticalLines.Count; n++)
                {
                    verticalLine line2 = verticalLines[n];
                    wholeOffset vOffset = new wholeOffset();
                    vOffset.minPos = line.position;
                    vOffset.maxPos = line2.position;
                    vOffset.offset = Math.Abs(line2.position - line.position - boardW);
                    //  Console.WriteLine(bais);   
                    verticalOffsets.Add(vOffset);
                }
            }
            verticalOffsets.Sort(delegate (wholeOffset a, wholeOffset b) {
                return a.offset.CompareTo(b.offset);
            });

            while (verticalOffsets.Count >= 2 &&Math.Abs( verticalOffsets[0].offset - verticalOffsets[1].offset)<= verticalGap/3)
            {
                int v0Min = verticalOffsets[0].minPos + verticalGap / 2;
                int v0MinCounts = 0;
                int v0Max = verticalOffsets[0].maxPos - verticalGap / 2;
                int v0MaxCounts = 0;

                int v1Min = verticalOffsets[1].minPos + verticalGap / 2;
                int v1MinCounts = 0;
                int v1Max = verticalOffsets[1].maxPos - verticalGap / 2;
                int v1MaxCounts = 0;

                for (int s = 0; s < horizonLines.Count; s++)
                {
                    horizonLine line = horizonLines[s];
                    if (!line.validate)
                        continue;
                    if (line.startPoint.X < v0Min&& line.endPoint.X> v0Min)
                        v0MinCounts++;
                    if (line.endPoint.X > v0Max&&line.startPoint.X< v0Max)
                        v0MaxCounts++;
                    if (line.startPoint.X < v1Min && line.endPoint.X > v1Min)
                        v1MinCounts++;
                    if (line.endPoint.X > v1Max && line.startPoint.X < v1Max)
                        v1MaxCounts++;
                }

                int v0MinOutLinesCounts = Math.Min(v0MinCounts, v0MaxCounts);
                int v1MinOutLinesCounts = Math.Min(v1MinCounts, v1MaxCounts);
                if (v0MinOutLinesCounts < v1MinOutLinesCounts)
                    verticalOffsets.RemoveAt(0);
                else
                    verticalOffsets.RemoveAt(1);
            }

            List<wholeOffset> horizonOffsets = new List<wholeOffset>();
            for (int s = 0; s < horizonLines.Count; s++)
            {
                horizonLine line = horizonLines[s];
                for (int n = s + 1; n < horizonLines.Count; n++)
                {
                    horizonLine line2 = horizonLines[n];
                    wholeOffset hOffset = new wholeOffset();
                    hOffset.minPos = line.position;
                    hOffset.maxPos = line2.position;
                    hOffset.offset = Math.Abs(line2.position - line.position - boardH);
                    //  Console.WriteLine(bais);   
                    horizonOffsets.Add(hOffset);
                }
            }
            horizonOffsets.Sort(delegate (wholeOffset a, wholeOffset b) {
                return a.offset.CompareTo(b.offset);
            });

            while (horizonOffsets.Count >= 2 && Math.Abs(horizonOffsets[0].offset - horizonOffsets[1].offset)<= horizonGap/3)
            {
                int h0Min = horizonOffsets[0].minPos + horizonGap / 2;
                int h0MinCounts = 0;
                int h0Max = horizonOffsets[0].maxPos - horizonGap / 2;
                int h0MaxCounts = 0;

                int h1Min = horizonOffsets[1].minPos + horizonGap / 2;
                int h1MinCounts = 0;
                int h1Max = horizonOffsets[1].maxPos - horizonGap / 2;
                int h1MaxCounts = 0;

                for (int s = 0; s < verticalLines.Count; s++)
                {
                    verticalLine line = verticalLines[s];
                    if (!line.validate)
                        continue;
                    if (line.startPoint.Y < h0Min && line.endPoint.Y > h0Min)
                        h0MinCounts++;
                    if (line.endPoint.Y > h0Max && line.startPoint.Y < h0Max)
                        h0MaxCounts++;
                    if (line.startPoint.Y < h1Min && line.endPoint.Y > h1Min)
                        h1MinCounts++;
                    if (line.endPoint.Y > h1Max && line.startPoint.Y < h0Max)
                        h1MaxCounts++;
                }

                int h0MinOutLinesCounts = Math.Min(h0MinCounts, h0MaxCounts);
                int h1MinOutLinesCounts = Math.Min(h1MinCounts, h1MaxCounts);
                if (h0MinOutLinesCounts < h1MinOutLinesCounts)
                    horizonOffsets.RemoveAt(0);
                else
                    horizonOffsets.RemoveAt(1);
            }
            
            int leftPos = verticalOffsets[0].minPos;
            int rightPos = verticalOffsets[0].maxPos;
            int topPos = horizonOffsets[0].minPos;
            int bottomPos = horizonOffsets[0].maxPos;

            int vGap = (int)Math.Ceiling(verticalGap / 2f);
            int hGap = (int)Math.Ceiling(horizonGap / 2f);
            ox2 = ox1 + rightPos+ hGap;
            oy2 = oy1 + bottomPos+ vGap;

            ox1 = ox1+leftPos- hGap;
            oy1 = oy1+topPos-vGap;


          //  Cv2.Canny(srcImage, contours, 100, 250);
            //Rect roi = new Rect(leftPos - vGap, topPos - vGap, rightPos-leftPos + hGap*2, bottomPos- topPos + vGap * 2);//首先要用个rect确定我们的兴趣区域在哪
            //Mat ImageROI = new Mat(contours, roi);
            //Mat ImageShow = new Mat(srcImage, roi);
            //CircleSegment[] circles = Cv2.HoughCircles(ImageROI, HoughMethods.Gradient,1,550);
            //foreach (CircleSegment circle in circles)
            //{
            //    // Point2f center = circle.Center;//圆心
            //    int radius = (int)circle.Radius;//半径

            //    ImageShow.Circle((int)circle.Center.X, (int)circle.Center.Y, radius, new Scalar(0, 255, 0), 1);
            //}
            //Cv2.ImShow("兴趣区域", ImageShow);

            //if (showDebugImage)
            //{
            //    Cv2.Line(copy2, new Point(leftPos, topPos), new Point(rightPos, topPos), new Scalar(0, 255, 0), 1);
            //    Cv2.Line(copy2, new Point(leftPos, bottomPos), new Point(rightPos, bottomPos), new Scalar(0, 255, 0), 1);
            //    Cv2.Line(copy2, new Point(leftPos, topPos), new Point(leftPos, bottomPos), new Scalar(0, 255, 0), 1);
            //    Cv2.Line(copy2, new Point(rightPos, topPos), new Point(rightPos, bottomPos), new Scalar(0, 255, 0), 1);
            //    Cv2.ImShow("1路线", copy2);
            //}
            //for (int s = 0; s < verticalLines.Count; s++)
            //{
            //    verticalLine line = verticalLines[s];
            //    Cv2.Line(copy, line.startPoint, line.endPoint, new Scalar(0, 255, 0), 1);
            //   // Console.WriteLine(line.startPoint.X);
            //}

            //for (int s = 0; s < horizonLines.Count; s++)
            //{
            //    horizonLine line = horizonLines[s];
            //    Cv2.Line(copy, line.startPoint, line.endPoint, new Scalar(0, 255, 0), 1);
            //}
        }

        private void CalculateLines(LineSegmentPoint[] linesPoint)
        {
            for (int i = 0; i < linesPoint.Length; i++)
            {
                int x1 = linesPoint[i].P1.X;
                int x2 = linesPoint[i].P2.X;

                int y1 = linesPoint[i].P1.Y;
                int y2 = linesPoint[i].P2.Y;
                if (Math.Abs(x1 - x2) >= 2 && Math.Abs(y1 - y2) >= 2)
                    continue;

                if (Math.Abs(y1 - y2) >= 2)
                {
                    verticalLine newLine = new verticalLine();
                    newLine.position = (x1 + x2) / 2;
                    newLine.startPoint = new Point(newLine.position, Math.Min(y1, y2));
                    newLine.endPoint = new Point(newLine.position, Math.Max(y1, y2));
                    verticalLines.Add(newLine);
                }
                else
                {
                    horizonLine newLine = new horizonLine();
                    newLine.position = (y1 + y2) / 2;
                    newLine.startPoint = new Point(Math.Min(x1, x2), newLine.position);
                    newLine.endPoint = new Point(Math.Max(x1, x2), newLine.position);
                    horizonLines.Add(newLine);
                }
                // Cv2.Line(copy, linesPoint[i].P1, linesPoint[i].P2, new Scalar(0, 255, 0), 1);
            }
            verticalLines.Sort(delegate (verticalLine a, verticalLine b) {
                return a.position.CompareTo(b.position);
            });
            for (int s = 0; s < verticalLines.Count / 2; s++)
            {
                verticalLine line = verticalLines[s];
                verticalLine line2 = verticalLines[s + 1];
                if (Math.Abs(line.position - line2.position) <= 4)
                {
                    line2.position = (line.position + line2.position) / 2;
                    line2.startPoint.X = line2.position;
                    line2.startPoint.Y = Math.Min(line.startPoint.Y, line2.startPoint.Y);
                    line2.endPoint.X = line2.position;
                    line2.endPoint.Y = Math.Max(line.endPoint.Y, line2.endPoint.Y);
                    line.needDelete = true;
                }
            }

            for (int s = verticalLines.Count; s > verticalLines.Count / 2; s--)
            {
                verticalLine line = verticalLines[s - 1];
                verticalLine line2 = verticalLines[s - 2];
                if (line2.needDelete)
                    continue;
                if (Math.Abs(line.position - line2.position) <= 4)
                {
                    line2.position = (line.position + line2.position) / 2;
                    line2.startPoint.X = line2.position;
                    line2.startPoint.Y = Math.Min(line.startPoint.Y, line2.startPoint.Y);
                    line2.endPoint.X = line2.position;
                    line2.endPoint.Y = Math.Max(line.endPoint.Y, line2.endPoint.Y);
                    line.needDelete = true;
                }
            }
            for (int s = 0; s < verticalLines.Count; s++)
            {
                if (verticalLines[s].needDelete)
                {
                    verticalLines.RemoveAt(s);
                    s--;
                }
            }

            horizonLines.Sort(delegate (horizonLine a, horizonLine b) {
                return a.position.CompareTo(b.position);
            });
            for (int s = 0; s < horizonLines.Count / 2; s++)
            {
                horizonLine line = horizonLines[s];
                horizonLine line2 = horizonLines[s + 1];
                if (Math.Abs(line.position - line2.position) <= 4)
                {
                    line2.position = (line.position + line2.position) / 2;
                    line2.startPoint.Y = line2.position;
                    line2.startPoint.X = Math.Min(line.startPoint.X, line2.startPoint.X);
                    line2.endPoint.Y = line2.position;
                    line2.endPoint.X = Math.Max(line.endPoint.X, line2.endPoint.X);
                    line.needDelete = true;
                }
            }
            for (int s = horizonLines.Count; s > horizonLines.Count / 2; s--)
            {
                horizonLine line = horizonLines[s - 1];
                horizonLine line2 = horizonLines[s - 2];
                if (line2.needDelete)
                    continue;
                if (Math.Abs(line.position - line2.position) <= 4)
                {
                    line2.position = (line.position + line2.position) / 2;
                    line2.startPoint.Y = line2.position;
                    line2.startPoint.X = Math.Min(line.startPoint.X, line2.startPoint.X);
                    line2.endPoint.Y = line2.position;
                    line2.endPoint.X = Math.Max(line.endPoint.X, line2.endPoint.X);
                    line.needDelete = true;
                }
            }
            for (int s = 0; s < horizonLines.Count; s++)
            {
                if (horizonLines[s].needDelete)
                {
                    horizonLines.RemoveAt(s);
                    s--;
                }
            }
        }

        private LineSegmentPoint[] FindLines(Mat srcImage, int boardWidth1, int boardHeight1, int lowCanny,int upCanny)
        {    Mat contours = new Mat();
            Cv2.Canny(srcImage, contours, lowCanny, upCanny);
            int length = Math.Min(srcImage.Width, srcImage.Height);
            //  Cv2.ImShow("轮廓", contours);         
            int size = Math.Min(boardWidth1, boardHeight1);
            LineSegmentPoint[] linesPoint = Cv2.HoughLinesP(contours, 1, Cv2.PI / 180, (int)(length / (((size + 1) * 1.5f))), (int)(length / (((size + 1) * 1.5f))), 1);
            //  Console.WriteLine(linesPoint.Length);
            return linesPoint;
        }
    }
}

class wholeOffset
{
    public int minPos;
    public int maxPos;
    public int offset;
}

class gapCount
{
    public int gap;
    public int looseCounts = 1;
    public int uniqueCounts = 1;
}

class verticalLine
{
    public Point startPoint;
    public Point endPoint;
    public int position;
    public Boolean needDelete = false;
    public Boolean validate = false;
}

class horizonLine
{
    public Point startPoint;
    public Point endPoint;
    public int position;
    public Boolean needDelete = false;
    public Boolean validate = false;
}

