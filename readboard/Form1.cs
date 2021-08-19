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
using System.Drawing.Imaging;

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
        //public static CDmSoft dm2;
        int hwnd = 0;
        //int hwndFoxPlace = 0;
        Form2 form2;
        Form8 form8;

        Boolean startedSync = false;
        Boolean isContinuousSyncing = false;
        //object qx1;
        // object qy1;

        Boolean keepSync = false;
        int sx1;
        int sy1;
        int width;
        int height;
        //int rectSize;
        int rectX1;
        int rectY1;
        //   int sx1ty5=0;
        //   int sy1ty5=0;
        //int all;
        //  System.Timers.Timer t;
        public static int type = 0;
        // Boolean isQTYC = false;
        // int boardWidth=19;
        int boardH = 19;
        int boardW = 19;
        static double widthMagrin = 0;
        static double heightMagrin = 0;
        //Boolean noticeLast = true;
        Boolean syncBoth = false;
        Boolean canUseLW = false;
        //Boolean noLw = false;
        Boolean useTcp = false;
        Thread thread;
        Boolean isMannulCircle = false;
        float factor = 1.0f;
        private KeyboardHookListener hookListener;
        private int port = 0;


        Boolean savedPlace = false;
        int savedX;
        int savedY;

        int posX = -1;
        int posY = -1;

        private Boolean isJavaFrame=false;
        private int javaX,javaY;
        lw.lwsoft lw;
        Boolean canUsePrintWindow = true;
        Boolean isFirstGetPos = true;
        Boolean cansetFirstGetPos = true;
        Boolean isSecondTime = false;
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


        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern IntPtr DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern IntPtr DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        public static extern bool BitBlt(IntPtr hdcDest, int nXDest,
        int nYDest, int nWidth, int nHeight, IntPtr hdcSrc,
        int nXSrc, int nYSrc, int dwRop);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc,
        int nWidth, int nHeight);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobjBmp);

        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        private Bitmap GetWindowImage(IntPtr hwnd)
        {
            return GetWindowImage(hwnd, false, - 1, -1, -1, -1,true);
        }
        private Bitmap GetWindowImage(IntPtr hwnd,Boolean usePrintWindow)
        {
            return GetWindowImage(hwnd, false, -1, -1, -1, -1, usePrintWindow);
        }

        private Bitmap GetWindowImage(IntPtr hwnd,Boolean calcRect,int x,int y,int w,int h,Boolean usePrintWindow)
        {
            if (isJavaFrame)
            {
                return GetWindowPrintImage(hwnd);
            }
            else
            {
                try
                {
                    cansetFirstGetPos = true; 
                    RECT rect = new RECT();
                    GetWindowRect(hwnd, ref rect);
                    int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                    int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                    if (usePrintWindow) { 
                    if (calcRect)
                    {
                        int startx = rect.Left + x;
                        int endx = rect.Left + x + w;
                        int starty = rect.Top + y;
                        int endy = rect.Top + y + h;
                        if (Program.useEnhanceScreen && canUsePrintWindow && (startx< 0 ||startx > screenWidth || starty < 0 ||endy > screenHeight))
                        {
                            return GetWindowPrintImage(hwnd);
                        }
                    }
                    else
                    if (Program.useEnhanceScreen && canUsePrintWindow && (rect.Left < 0 || rect.Left > screenWidth || rect.Top < 0 || rect.Bottom > screenHeight))
                    {
                        return GetWindowPrintImage(hwnd);
                    }
                    }
                    int rectWidth = rect.Right - rect.Left;
                    int rectHeight = rect.Bottom - rect.Top;
                    IntPtr windc = GetDC(hwnd);
                    IntPtr hDCMem = CreateCompatibleDC(windc);
                    IntPtr hbitmap = CreateCompatibleBitmap(windc, rectWidth, rectHeight);
                    IntPtr hOldBitmap = (IntPtr)SelectObject(hDCMem, hbitmap);
                    BitBlt(hDCMem, 0, 0, rectWidth, rectHeight, windc, 0, 0, 13369376);
                    hbitmap = (IntPtr)SelectObject(hDCMem, hOldBitmap);
                    Bitmap bitmap = Bitmap.FromHbitmap(hbitmap);
                    DeleteObject(hbitmap);
                    DeleteObject(hOldBitmap);
                    DeleteDC(hDCMem);
                    ReleaseDC(hwnd, windc);
                    return bitmap;
                }
                catch (Exception e)
                {
                    SendError(e.ToString());
                    return null;
                }
            }
        }        

        private Bitmap GetWindowPrintImage(IntPtr hWnd)
        {
            try
            {
                cansetFirstGetPos = false;
                RECT rect = new RECT();
                GetWindowRect(hWnd, ref rect);
                int rectWidth = rect.Right - rect.Left;
                int rectHeight = rect.Bottom - rect.Top;
                if (rectWidth <= 0 || rectHeight <= 0) return null;
                Bitmap bmp = new Bitmap(rectWidth, rectHeight);
                Graphics g = Graphics.FromImage(bmp);
                IntPtr dc = g.GetHdc();
                PrintWindow(hWnd, dc, 0);
                g.ReleaseHdc();
                g.Dispose();
                return bmp;
            }
            catch { return null; }
        }

        private Bitmap GetWindowBmp(IntPtr hWnd, int x, int y, int width, int height)
        {
            if (x < 0 || y < 0 || width <= 0 || height <= 0)
                return null;
            try
            {
                Bitmap bmp = GetWindowImage(hWnd,true,x,y,width,height,true);
                if (bmp == null)
                    return null;
                Bitmap mybit = new Bitmap(width, height);
                Graphics g1 = Graphics.FromImage(mybit);
                g1.DrawImage(bmp /* 原图 */,
                         new Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(width, height)), // 目标位置 
                         new Rectangle(new System.Drawing.Point(x, y), new System.Drawing.Size(width, height)), // 原图位置 
                        GraphicsUnit.Pixel);
                g1.Dispose();
                return mybit;
            }
            catch { return null; }
        }

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(
         IntPtr hwnd,               // Window to copy,Handle to the window that will be copied.
         IntPtr hdcBlt,             // HDC to print into,Handle to the device context.
         UInt32 nFlags              // Optional flags,Specifies the drawing options. It can be one of the following values.
         );

        private void SendError(String strMsg)
        {
            if (useTcp)
            {
                try
                {
                    byte[] by = Encoding.UTF8.GetBytes("error: "+strMsg + "\r\n");
                    io.Write(by, 0, by.Length);
                    io.Flush();
                }
                catch (Exception e)
                {
                    Console.OutputEncoding = System.Text.Encoding.UTF8;
                    Console.Error.WriteLine(e.Message);
                }
            }
            else
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.Error.WriteLine("error: " +strMsg);
            }
        }

        private void Send(String strMsg)
        {
            //Console.OutputEncoding = Encoding.UTF8;
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
                    Console.OutputEncoding = System.Text.Encoding.UTF8;
                    Console.Error.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine(strMsg);
            }
        }

        private Boolean isAltDown = false;

        private void HookListener_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // Console.Out.WriteLine(e.KeyValue);
            if (e.KeyValue == 164 || e.KeyValue == 165)
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
                    try
                    {
                        byte[] data = new byte[20000];
                        int numBytesRead = io.Read(data, 0, data.Length);
                        if (numBytesRead > 0)
                        {
                            readStr = Encoding.UTF8.GetString(data, 0, numBytesRead);
                            //Console.Error.WriteLine(readStr);
                        }
                      readPlace(readStr);
                    }
                    catch (Exception ex)
                    {
                        SendError(ex.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                SendError(e.ToString());
            }
        }
        private void readPlace(String a)
        {
            if (a.StartsWith("place"))
            {
                char[] separator = { ' ' }; string[] arr = a.Split(separator);
                try
                {
                    place(int.Parse(arr[1]), int.Parse(arr[2]));
                }
                catch (Exception e)
                {
                    SendError(e.ToString());
                }
            }
            if (a.StartsWith("loss"))
            {
               lossFocus();
            }
            if (a.StartsWith("notinboard"))
            {
              stopInBoard();
            }
            if (a.StartsWith("version"))
            {
                sendVersion();
            }
            if (a.StartsWith("quit"))
            {
                Control.CheckForIllegalCrossThreadCalls = false;
                Form1.pcurrentWin.shutdown();
            }
        }

        public Form1(String aitime, String playouts, String firstpo, String usetcp, String serverPort)
        {
            InitializeComponent();
            if (usetcp.Equals("1"))
            {
                useTcp = true;
            }
            if (useTcp)
            {
                try
                {
                    port = Convert.ToInt32(serverPort);
                    client = new TcpClient("127.0.0.1", port);
                    io = client.GetStream();
                    threadReceive = new Thread(new ThreadStart(Receive));
                    threadReceive.IsBackground = true;
                    threadReceive.Start();
                }
                catch
                {
                    MessageBox.Show(Program.isChn ? "棋盘同步工具与Lizzie连接失败" : "Can not connect to Lizzie");
                }
            }
            GlobalHooker hooker = new GlobalHooker();
            hookListener = new KeyboardHookListener(hooker);
            hookListener.KeyDown += HookListener_KeyDown;
            hookListener.KeyUp += HookListener_KeyUp;
            hookListener.Start();
            System.Drawing.Bitmap bitmap = new Bitmap(1, 1);
            System.Drawing.Graphics graphics2 = Graphics.FromImage(bitmap);
            factor = graphics2.DpiX / 96;
            if (factor > 1.0f)
            {
                Program.isScaled = true;
                Program.factor = factor;
            }
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
                                Program.verifyMove = (Convert.ToInt32(arr[5]) == 1);
                                Program.showScaleHint = (Convert.ToInt32(arr[6]) == 1);
                                Program.showInBoard = (Convert.ToInt32(arr[7]) == 1);
                                Program.showInBoardHint = (Convert.ToInt32(arr[8]) == 1);
                                Program.autoMin = (Convert.ToInt32(arr[9]) == 1);
                                Program.isAdvScale = (Convert.ToInt32(arr[10]) == 1);
                                type = Convert.ToInt32(arr[12]);
                                Program.hasConfigFile = true;
                            }
                            else
                            {
                                if (File.Exists("config_readboard_others.txt"))
                                    File.Delete("config_readboard_others.txt");
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                sr.Close();
            }

            if (File.Exists("config_readboard_others.txt"))
            {
                int customW = -1;
                int customH = -1;
                StreamReader sr = new StreamReader("config_readboard_others.txt", Encoding.UTF8);
                String line;
                if ((line = sr.ReadLine()) != null)
                {
                    string[] arr = line.Split('_');
                    if (arr.Length == 10)
                    {
                        try
                        {
                            this.boardW = Convert.ToInt32(arr[0]);
                            this.boardH = Convert.ToInt32(arr[1]);
                            customW = Convert.ToInt32(arr[2]);
                            customH = Convert.ToInt32(arr[3]);
                            Program.timeinterval = Convert.ToInt32(arr[4]);
                            Program.timename = Program.timeinterval + "";
                            this.syncBoth = (Convert.ToInt32(arr[5]) == 1);
                            Program.grayOffset = Convert.ToInt32(arr[6]);
                            posX = Convert.ToInt32(arr[7]);
                            posY = Convert.ToInt32(arr[8]);
                            Program.useEnhanceScreen = (Convert.ToInt32(arr[5]) == 9);
                            if (posX != -1 && posY != -1)
                            {
                                var h = Screen.PrimaryScreen.Bounds.Height;
                                var w = Screen.PrimaryScreen.Bounds.Width;
                                this.Location = new System.Drawing.Point(Math.Min(Math.Max(0, posX), w - 476), Math.Min(Math.Max(0, posY), h - 217));
                            }                         
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
                        if (customW > 0)
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
            switch (type)
            {
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
            dm.SetShowErrorMsg(0);
            this.MaximizeBox = false;
            pcurrentWin = this;

            if (type == 0 || type == 1 || type == 2)
                this.button10.Enabled = true;
            else
                this.button10.Enabled = false;
            if (!aitime.Equals(" "))
                textBox1.Text = aitime;
            if (!playouts.Equals(" "))
                textBox2.Text = playouts;
            if (!firstpo.Equals(" "))
                textBox3.Text = firstpo;
            try
            {
                //  int s = DllRegisterServer();
                //  if (s >= 0)
                //  {
                //注册成功!             
                try
                {
                    lw = new lw.lwsoft();
                    canUseLW = true;
                }
                catch (Exception ex)
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
            radioWhite.Enabled = false;
            radioBlack.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            if (syncBoth)
            {
                chkBothSync.Checked = true;
                chkAutoPlay.Enabled = true;
            }
            else
            {
                chkBothSync.Checked = false;
                chkAutoPlay.Enabled = false;
            }
            if (!Program.isChn)
            {
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
                this.chkBothSync.Text = "BothSync";
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
                this.btnKeepSync.Text = "KeepSync(200ms)";
                this.button4.Text = "OneTimeSync";
                this.button8.Text = "Exchange";
                this.button6.Text = "ClearBoard";
                this.button11.Text = "CircleRow1";
                this.Text = "Board Synchronization Tool";
            }           
                Send("ready");
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
            catch (Exception)
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

            mh = new MouseHookListener(new GlobalHooker());

            mh.MouseMove += mh_MouseMoveEvent;
            mh.MouseClick += mh_MouseMoveEvent2;
            mh.Enabled = false;
            this.btnKeepSync.Text = (Program.isChn ? "持续同步(" : "KeepSync(") + Program.timename + "ms)";
        }

        //[DllImport("user32.dll")]
        //static extern void BlockInput(bool Block);
        public void Snap(int x1, int y1, int x2, int y2)
        {
            pressed = true;
            ox1 = Math.Min(x1, x2);
            oy1 = Math.Min(y1, y2);
            ox2 = Math.Max(x1, x2);
            oy2 = Math.Max(y1, y2);
            if (!isMannulCircle)
            {
                if (!reCalculateRow1(ox1, oy1, ox2, oy2))
                    MessageBox.Show(Program.isChn ? "不能识别棋盘,请调整被同步棋盘大小后重新选择或尝试[框选1路线]" : "Can not detect board,Please zoom the board and try again or use [CircleRow1]");
                else if (type == 3)
                {
                    object curX, curY;
                    dm.GetCursorPos(out curX, out curY);
                    // BlockInput(true);
                    dm.MoveTo((ox1 + ox2) / 2, (oy1 + oy2) / 2);
                    Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(35);
                        hwnd = dm.GetMousePointWindow();
                        Console.WriteLine(hwnd);
                    });
                    Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(30);
                        dm.MoveTo((ox1 + ox2) / 2, (oy1 + oy2) / 2);
                        Thread.Sleep(20);
                        dm.MoveTo((ox1 + ox2) / 2, (oy1 + oy2) / 2);
                        Thread.Sleep(50);
                        dm.MoveTo((int)curX, (int)curY);
                    });
                    //  BlockInput(false);                   
                }
            }
            else
            {
                int gapX = (int)Math.Round((ox2 - ox1) / ((boardW - 1) * 2f));
                int gapY = (int)Math.Round((oy2 - oy1) / ((boardH - 1) * 2f));
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
                    //  graphics.CopyFromScreen(x, y, 0, 0, new System.Drawing.Size((int)(width * factor), (int)(height * factor)));
                    graphics.CopyFromScreen(x, y, 0, 0, new System.Drawing.Size(width, height));
                    //bitmap.Save("screen.bmp");
                }
                //  dm2.Capture(x, y, x2, y2, "screen.bmp");
                return true;
            }
            else
            {
                MessageBox.Show(Program.isChn ? "未选择棋盘" : "No board has been choosen");
                return false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            canUsePrintWindow = true;
            isFirstGetPos = true;
            isSecondTime = false;
            oneTimeSync();
        }

        private void oneTimeSync() {
            if (type == 5)
            {
                isJavaFrame = false;
                sx1 = ox1;
                sy1 = oy1;
                width = ox2 - ox1;
                height = oy2 - oy1;
                OutPut3(true);
                if (width <= this.boardW)
                {
                    MessageBox.Show(Program.isChn ? "未选择棋盘,同步失败" : "No board has been choosen,Sync failed");
                    return;
                }
            }
            else
            {
                if (hwnd <= 0)
                {
                    MessageBox.Show(Program.isChn ? "未选择棋盘,同步失败" : "No board has been choosen,Sync failed");
                    return;
                }
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
                        MessageBox.Show(Program.isChn ? "未选择棋盘,同步失败" : "No board has been choosen,Sync failed");
                        return;
                    }
                }
                StringBuilder className = new StringBuilder(256);
                GetClassName(new IntPtr(hwnd), className, className.Capacity);
                if (className.ToString().Equals("SunAwtFrame"))
                {
                    isJavaFrame = true;
                }
                else
                {
                    isJavaFrame = false;
                }
                RgbInfo[] rgbArray = new RgbInfo[0];
                if (type == 0)
                {
                    if (!dm.GetWindowClass(hwnd).ToLower().Equals("#32770"))
                    {
                        MessageBox.Show(Program.isChn ? "未选择正确的棋盘" : "Not right board");
                    }
                    //dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "313131-050505", "0|1|2E2E2E-050505,0|2|2E2E2E-050505,-1|0|2E2E2E-050505,-2|0|2E2E2E-050505,-2|1|-2E2E2E-050505,-1|1|-2E2E2E-050505,-1|2|-2E2E2E-050505", 1.0, 2, out qx1, out qy1);
                    //object qx4;
                    //object qy4;
                    //dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "313131-050505", "0|1|2E2E2E-050505,0|2|2E2E2E-050505,1|0|2E2E2E-050505,2|0|2E2E2E-050505,2|1|-2E2E2E-050505,1|1|-2E2E2E-050505,1|2|-2E2E2E-050505", 1.0, 0, out qx4, out qy4);
                    //// dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "313131-050505", "0|-1|2E2E2E-050505,0|-2|2E2E2E-050505,1|0|2E2E2E-050505,2|0|2E2E2E-050505,2|-1|-2E2E2E-050505,1|-1|-2E2E2E-050505,1|-2|-2E2E2E-050505", 1.0, 1, out qx4, out qy4);
                    //sx1 = (int)qx4;
                    //sy1 = (int)qy1;
                    //width = (int)qx1 - (int)qx4;
                    //height = width;

                    if (!getFoxPos(new IntPtr(hwnd), rgbArray))
                        return;
                    //  all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
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
                        width = (int)(width / factor);
                        height = (int)(height / factor);
                    }
                    //     all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
                }
                if (type == 2)
                {
                    if (!(dm.GetWindowClass(hwnd).ToLower().Equals("tlmdsimplepanel") || dm.GetWindowClass(hwnd).ToLower().Equals("tpanel")))
                    {
                        MessageBox.Show(Program.isChn ? "未选择正确的棋盘" : "Not right board");
                    }
                    if (!getSinaPos(new IntPtr(hwnd), rgbArray))
                        return;
                    //dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "FBDAA2-050505", "0|1|FBDAA2-050505,0|2|FBDAA2-050505", 1.0, 0, out qx1, out qy1);
                    //object qx4;
                    //object qy4;
                    //dm.FindMultiColor(0, 0, (int)x2 - (int)x1, (int)y2 - (int)y1, "FBDAA2-050505", "0|1|FBDAA2-050505,0|2|FBDAA2-050505", 1.0, 1, out qx4, out qy4);
                    //sx1 = (int)qx1 - 1;
                    //sy1 = (int)qy1 - 1;
                    //height = (int)qy4 - (int)qy1 + 4;
                    //width = height;
                    //      all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
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
                    //if (true) {
                    //    sx1 = (int)(sx1 / factor);
                    //    sy1 = (int)(sy1 / factor);
                    //   // width = (int)(width / factor);
                    //  //  height = (int)(height / factor);
                    //}
                    //   all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
                    //dm.Capture(sx1, sy1, sx1+width, sy1+ height, "backboard.bmp");
                    OutPut3(true);
                }
                else
                    OutPut(true, rgbArray, null);
                // if (t != null)
                //  {
                //       t.Enabled = false;
                //   }
            }
        }

        private void minWindow(String a)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void startKeepingSync(String a)
        {
            this.btnKeepSync.Text = Program.isChn ? "停止同步" : "StopSync";
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
            this.btnKeepSync.Text = t;
            if (!isContinuousSyncing)
                this.button10.Text = "一键同步";
            //if (this.factor <= 1)
            //{ 
            this.button4.Enabled = true;
            this.btnKeepSync.Enabled = true;
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
            Action2<String> a = new Action2<String>(Action2Test);
            if (!isContinuousSyncing)
            {
                Invoke(a, (Program.isChn ? "持续同步(" : "KeepSync(") + Program.timename + "ms)");
            }
            if (thread != null && thread.IsAlive)
                thread.Abort();           
        }

        public void resetBtnKeepSyncName()
        {
            if (!isContinuousSyncing)
                this.btnKeepSync.Text = (Program.isChn ? "持续同步(" : "KeepSync(") + Program.timename + "ms)";
        }

        [DllImport("user32", SetLastError = true)]
        private static extern int GetWindowText(
            IntPtr hWnd,//窗口句柄
            StringBuilder lpString,//标题
            int nMaxCount //最大值
            );

        //获取类的名字
        [DllImport("user32.dll")]
        private static extern int GetClassName(
            IntPtr hWnd,//句柄
            StringBuilder lpString, //类名
            int nMaxCount //最大值
            );

        private void startContinuous()
        {
            Action2<String> a = new Action2<String>(setContinuousSync);
            Invoke(a, "");
            RECT rect = new RECT();
            IntPtr p = new IntPtr(hwnd);
            while (isContinuousSyncing)
            {
                if (!startedSync)
                {
                    hwnd = -1;
                    int finalWidth = 0;
                    int x1;
                    int y1;
                    int x2;
                    int y2;
                    if(Program.showInBoard)
                    Send("noinboard");
                    if (type == 0)
                    {
                       // hwnd = dm.FindWindow("#32770", "");
                       // hwndFoxPlace = -1;
                        String hwnds = dm.EnumWindowByProcess("foxwq.exe", "", "#32770", 16);
                        string[] hwndArray = hwnds.Split(',');
                        foreach (string oneHwnd in hwndArray)
                        {
                            if (oneHwnd.Length == 0)
                                continue;

                            StringBuilder title = new StringBuilder(256);
                            GetWindowText(new IntPtr(int.Parse(oneHwnd)), title, title.Capacity);
                            if (title.ToString().Equals("CChessboardPanel"))
                            {
                                hwnd = int.Parse(oneHwnd);
                                break;
                                //int hwndTemp = int.Parse(oneHwnd);
                                //p = new IntPtr(hwndTemp);
                                //GetWindowRect(p, ref rect);
                                //x2 = rect.Right;
                                //y2 = rect.Bottom;
                                //x1 = rect.Left;
                                //y1 = rect.Top;
                                //if (x1 >= -9999 && y1 >= -9999 && x2 - x1 > finalWidth || finalWidth == 0)
                                //{
                                //    hwndFoxPlace = hwndTemp;
                                //    finalWidth = x2 - x1;
                                //}
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
                    if (hwnd > 0 && isContinuousSyncing)
                    {
                        startContinuousSync(true);
                    }
                }
                try
                {
                    Thread.Sleep(100);
                }
                catch (Exception)
                {

                }
            }
        }

        private void startContinuousSync(Boolean isSimpleSync)
        {
            Boolean isRightGoban = true;
            isFirstGetPos = true;
            canUsePrintWindow = true;
            if (type == 5)
            {
                isJavaFrame = false;
                Action2<String> a = new Action2<String>(startKeepingSync);
                Invoke(a, "");
                sx1 = ox1;
                sy1 = oy1;
                width = ox2 - ox1;
                height = oy2 - oy1;
                if (width <= this.boardW)
                {
                    MessageBox.Show(Program.isChn ? "未选择棋盘,同步失败" : "No board has been choosen,Sync failed");
                        stopKeepingSync();
                    return;
                }
                OutPut3(true);               
            }
            else
            {
                int x1;
                int y1;
                int x2;
                int y2;
                if (hwnd<=0)
                {
                    MessageBox.Show(Program.isChn ? "未选择棋盘,同步失败" : "No board has been choosen,Sync failed");
                    stopKeepingSync();
                    return;
                }

                RECT rect = new RECT();
                IntPtr p = new IntPtr(hwnd);
                GetWindowRect(p, ref rect);
                x2 = rect.Right;
                y2 = rect.Bottom;
                x1 = rect.Left;
                y1 = rect.Top;
                if ((int)x1 == 0 && (int)x2 == 0 && (int)y1 == 0 && (int)y2 == 0)
                {
                    if (!isSimpleSync && startedSync)
                        MessageBox.Show(Program.isChn ? "未选择棋盘,同步失败" : "No board has been choosen,Sync failed");
                    stopKeepingSync();
                    return;
                }
                StringBuilder className = new StringBuilder(256);
                GetClassName(new IntPtr(hwnd), className, className.Capacity);
                if (className.ToString().Equals("SunAwtFrame"))
                {
                    isJavaFrame = true;
                    javaX = x1;
                    javaY = y1;
                }
                else
                {
                    isJavaFrame = false;                   
                }
                    Action2<String> a = new Action2<String>(startKeepingSync);
                Invoke(a, "");
                if (type == 0)
                {
                    rectX1 = x1;
                    rectY1 = y1;
                    if (!dm.GetWindowClass(hwnd).ToLower().Equals("#32770"))
                    {
                        if (!isSimpleSync && startedSync)
                            MessageBox.Show(Program.isChn ? "未选择棋盘,同步失败" : "No board has been choosen,Sync failed");
                        isRightGoban = false;
                    }
                    if (!getFoxPos(new IntPtr(hwnd),null))
                    {
                        sx1 = 0;
                        sy1 = 0;
                        width = 0;
                        height = 0;
                        if (isContinuousSyncing)
                        {
                            startedSync = false;
                            return;
                        }
                    }
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
                    if (!getTygemPos(new IntPtr(hwnd),null))
                    {
                        sx1 = 0;
                        sy1 = 0;
                        width = 0;
                        height = 0;
                        if (isContinuousSyncing)
                        {
                            startedSync = false;
                            return;
                        }
                    }
                    //       all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
                }
                if (type == 2)
                {
                    if (!(dm.GetWindowClass(hwnd).ToLower().Equals("tlmdsimplepanel") || dm.GetWindowClass(hwnd).ToLower().Equals("tpanel")))
                    {
                        if (!isSimpleSync)
                            MessageBox.Show(Program.isChn ? "未选择棋盘,同步失败" : "No board has been choosen,Sync failed");
                        isRightGoban = false;
                    }
                    if (!getSinaPos(new IntPtr(hwnd), null))
                    {
                        sx1 = 0;
                        sy1 = 0;
                        width = 0;
                        height = 0;
                        if (isContinuousSyncing)
                        {
                            startedSync = false;
                            return;
                        }
                    }
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
                    //      all = (int)Math.Round(width / (float)boardW * height / (float)boardH);
                }
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
            Send("stopsync");
            stopKeepingSync();
            keepSync = false;
        }

        public delegate void Action2<in T>(T t);



        private void OutPutTime()
        {
            lw.lwsoft lwh=null;
            Send("sync");            
            while (keepSync)
            {
                if (Program.showInBoard && type != 5)
                {
                    object startX, startY;
                    object rectWidth, rectHeight;
                    dm.GetClientRect(hwnd, out (startX), out (startY), out (rectWidth), out (rectHeight));
                    if (!Program.isAdvScale)
                    {
                        if (type == 1)
                            Send("inboard " + (int)((sx1 + (int)startX) / factor) + " " + (int)((sy1 + (int)startY) / factor) + " " + (int)(width) + " " + (int)(height) + " " + (factor > 1 ? "99_" + factor + "_" + type.ToString() : type.ToString()));
                        else if (type == 2)
                            Send("inboard " + (int)((sx1 + (int)startX / factor)) + " " + (int)((sy1 + (int)startY / factor)) + " " + (int)(width) + " " + (int)(height) + " " + (factor > 1 ? "99_" + factor + "_" + type.ToString() : type.ToString()));
                        else if (type == 3)
                            Send("inboard " + (int)((sx1 + (int)startX / factor)) + " " + (int)((sy1 + (int)startY / factor)) + " " + (int)(width) + " " + (int)(height) + " " + (factor > 1 ? "99_" + factor + "_" + type.ToString() : type.ToString()));
                        else
                            Send("inboard " + (int)((sx1 + (int)startX) / factor) + " " + (int)((sy1 + (int)startY) / factor) + " " + (int)(width / factor) + " " + (int)(height / factor) + " " + (factor > 1 ? "99_" + factor + "_" + type.ToString() : type.ToString()));
                    }
                    else
                    {
                        if (type == 1)
                            Send("inboard " + (int)((sx1 + (int)startX) / factor) + " " + (int)((sy1 + (int)startY) / factor) + " " + (int)(width / factor) + " " + (int)(height / factor) + " " + (factor > 1 ? "99_" + factor + "_" + type.ToString() : type.ToString()));
                        else if (type == 2)
                            Send("inboard " + (int)((sx1 + (int)startX) / factor) + " " + (int)((sy1 + (int)startY) / factor) + " " + (int)(width / factor) + " " + (int)(height / factor) + " " + (factor > 1 ? "99_" + factor + "_" + type.ToString() : type.ToString()));
                        else
                            Send("inboard " + (int)((sx1 + (int)startX) / factor) + " " + (int)((sy1 + (int)startY) / factor) + " " + (int)(width / factor) + " " + (int)(height / factor) + " " + (factor > 1 ? "99_" + factor + "_" + type.ToString() : type.ToString()));
                    }
                    //    sx1, sy1,width,height
                }
                if (type == 5)
                {
                    OutPut3(false);
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

                    int size = width * height;
                    if ((int)x1 == 0 && (int)x2 == 0 && (int)y1 == 0 && (int)y2 == 0)
                    {
                        //MessageBox.Show("请选择棋盘,同步停止");
                        Send("stopsync");
                        stopKeepingSync();
                        return;
                    }
                    if (isJavaFrame)
                    {
                        javaX = x1;
                        javaY = y1;
                    }
                    RgbInfo[] rgbArray=new RgbInfo[0];
                    if (type == 0)
                    {
                        rectX1 = x1;
                        rectY1 = y1;
                        if (!getFoxPos(new IntPtr(hwnd), rgbArray))
                        {
                            sx1 = 0;
                            sy1 = 0;
                            width = 0;
                            height = 0;
                            if (isContinuousSyncing)
                            {
                                stopKeepingSync();
                                return;
                            }
                        }
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
                        if (!getTygemPos(new IntPtr(hwnd), rgbArray))
                        {
                            sx1 = 0;
                            sy1 = 0;
                            width = 0;
                            height = 0;
                            if (isContinuousSyncing)
                            {
                                stopKeepingSync();
                                return;
                            }
                        }
                    }
                    if (type == 2)
                    {
                        if (!getSinaPos(new IntPtr(hwnd), rgbArray))
                        {
                            sx1 = 0;
                            sy1 = 0;
                            width = 0;
                            height = 0;
                            if (isContinuousSyncing)
                            {
                                stopKeepingSync();
                                return;
                            }
                        }
                    }
                    Boolean changedSize = size != width * height;
                    if (changedSize)
                    {
                        Send("clear");
                    }                   
                    if (type == 3)
                    {
                        OutPut3(false);
                    }
                    else
                    {
                        if (canUseLW&&type==0)
                        {
                            OutPut(false, rgbArray, lwh);
                        }
                        else
                        OutPut(false, rgbArray,null);
                    }
                }
                try
                {
                    Thread.Sleep(Program.timeinterval);
                }
                catch (Exception) { }
            }
            Send("stopsync");
        }

        class RgbInfo
        {
            public Byte r;
            public Byte g;
            public Byte b;
        }

        private Boolean recognizeMove(Bitmap input, int x, int y)
        {
            if (input == null)
                return true;
            Rectangle rect = new Rectangle(0, 0, input.Width, input.Height);
            const int PixelWidth = 3;
            const PixelFormat PixelFormat = PixelFormat.Format24bppRgb;
            BitmapData data = input.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat);
            RgbInfo[] rgbArray = new RgbInfo[data.Height * data.Width];
            byte[] pixelData = new Byte[data.Stride];
            for (int scanline = 0; scanline < data.Height; scanline++)
            {
                Marshal.Copy(data.Scan0 + (scanline * data.Stride), pixelData, 0, data.Stride);
                for (int pixeloffset = 0; pixeloffset < data.Width; pixeloffset++)
                {
                    rgbArray[scanline * data.Width + pixeloffset] = new RgbInfo();
                    rgbArray[scanline * data.Width + pixeloffset].r = pixelData[pixeloffset * PixelWidth + 2];
                    rgbArray[scanline * data.Width + pixeloffset].g = pixelData[pixeloffset * PixelWidth + 1];
                    rgbArray[scanline * data.Width + pixeloffset].b = pixelData[pixeloffset * PixelWidth];
                }
            }
            input.UnlockBits(data);
            input.Dispose();
            int blackPercentStandard = Program.blackZB;
            int whitePercentStandard = Program.whiteZB;
            int blackOffsetStandard = Program.blackPC;
            int whiteOffsetStandard = Program.whitePC;
            int grayOffsetStandard = Program.grayOffset;
            if (type != 3 && type != 5)
            {
                blackPercentStandard = 37;

                whitePercentStandard = 30;
                blackOffsetStandard = 96;
                if (type == 1)
                    whiteOffsetStandard = 80;
                else
                    whiteOffsetStandard = 112;
                grayOffsetStandard = 50;
            }

            int blackPercent =
                       getWhiteBlackColorPercent(
                           rgbArray, 0, 0, input.Width, input.Height, true, blackOffsetStandard, grayOffsetStandard, input.Width, input.Height);
            if (blackPercent >= blackPercentStandard)
            {
                return true;
            }
            else
            {
                Boolean isWhite = false;
                int whitePercent =
                    getWhiteBlackColorPercent(
                        rgbArray, 0, 0, input.Width, input.Height, false, whiteOffsetStandard, grayOffsetStandard, input.Width, input.Height);
                if (whitePercent >= whitePercentStandard)
                {
                    if (x == 0
                        || x == boardW - 1
                        || y == 0
                        || y == boardH - 1)
                    {
                        if (whitePercent > 85) isWhite = false;
                        else isWhite = true;
                    }
                    else
                    {
                        if (whitePercent > 80) isWhite = false;
                        else isWhite = true;
                    }
                }
                return isWhite;
            }
        }

        private void recognizeBoard( RgbInfo[] rgbArray)
        {           
            if (rgbArray == null || rgbArray.Length == 0) {
                Bitmap input=null;
                if (type == 5)
                {
                    input = new Bitmap(width, height);
                    using (System.Drawing.Graphics graphics = Graphics.FromImage(input))
                    {
                        graphics.CopyFromScreen(sx1, sy1, 0, 0, new System.Drawing.Size(width, height));
                    }
                }
                else input = GetWindowBmp(new IntPtr(hwnd), sx1, sy1, width, height);
                if (input == null || input.Width <= boardW || input.Height <= boardH)
                    return;
                Rectangle rect = new Rectangle(0, 0, input.Width, input.Height);
            const int PixelWidth = 3;
            const PixelFormat PixelFormat = PixelFormat.Format24bppRgb;
            BitmapData data = input.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat);
            rgbArray = new RgbInfo[data.Height * data.Width];
            byte[] pixelData = new Byte[data.Stride];
            for (int scanline = 0; scanline < data.Height; scanline++)
            {
                Marshal.Copy(data.Scan0 + (scanline * data.Stride), pixelData, 0, data.Stride);
                for (int pixeloffset = 0; pixeloffset < data.Width; pixeloffset++)
                {
                    // PixelFormat.Format32bppRgb means the data is stored
                    // in memory as BGR. We want RGB, so we must do some 
                    // bit-shuffling.
                    rgbArray[scanline * data.Width + pixeloffset] = new RgbInfo();
                    rgbArray[scanline * data.Width + pixeloffset].r = pixelData[pixeloffset * PixelWidth + 2];
                    rgbArray[scanline * data.Width + pixeloffset].g = pixelData[pixeloffset * PixelWidth + 1];
                    rgbArray[scanline * data.Width + pixeloffset].b = pixelData[pixeloffset * PixelWidth];
                }
            }
            input.UnlockBits(data);
            input.Dispose();
            }
            float hGap = height / (float)boardH;
            float vGap = width / (float)boardW;
            int hGapInt = (int)Math.Round(hGap);
            int vGapInt = (int)Math.Round(vGap);

            int blackMinPercent = 200;
            int blackTotalPercent = 0;
            int blackMinX = -1;
            int blackMinY = -1;
            int blackCounts = 0;

            int whiteMinPercent = 200;
            int whiteTotalPercent = 0;
            int whiteMinX = -1;
            int whiteMinY = -1;
            int whiteCounts = 0;
            int[] resultValue = new int[boardH * boardW];

            int blackPercentStandard = Program.blackZB;
            int whitePercentStandard = Program.whiteZB;
            int blackOffsetStandard = Program.blackPC;
            int whiteOffsetStandard = Program.whitePC;
            int grayOffsetStandard = Program.grayOffset;
            //Boolean isPlatform = false;
            Boolean needCheckRedBlue = true;
            int redCount = 0;
            int blueCount = 0;
            int blueRedX = -1;
            int blueRedY = -1;
            String result = "";
            if (type != 3 && type != 5)
            {
                //isPlatform = true;
                blackPercentStandard = 37;

                whitePercentStandard = 30;
                blackOffsetStandard = 96;
                if (type == 1)
                    whiteOffsetStandard = 80;
                else
                    whiteOffsetStandard = 112;
                grayOffsetStandard = 50;
            }
            for (int y = 0; y < boardH; y++)
            {
                for (int x = 0; x < boardW; x++)
                {
                    Boolean isBlack = false;
                    Boolean isWhite = false;
                    Boolean isLastMove = false;
                    int blackPercent =
                        getWhiteBlackColorPercent(
                            rgbArray, (int)Math.Round(x * vGap), (int)Math.Round(y * hGap), vGapInt, hGapInt, true, blackOffsetStandard, grayOffsetStandard, width, height);
                    if (blackPercent >= blackPercentStandard)
                    {
                        isBlack = true;
                        blackTotalPercent += blackPercent;
                        blackCounts++;
                        if (blackPercent < blackMinPercent)
                        {
                            blackMinPercent = blackPercent;
                            blackMinX = x;
                            blackMinY = y;
                        }
                    }
                    else
                    {
                        int whitePercent =
                            getWhiteBlackColorPercent(
                                rgbArray, (int)Math.Round(x * vGap), (int)Math.Round(y * hGap), vGapInt, hGapInt, false, whiteOffsetStandard, grayOffsetStandard, width, height);
                        if (whitePercent >= whitePercentStandard)
                        {
                            if (x == 0
                                || x == boardW - 1
                                || y == 0
                                || y == boardH - 1)
                            {
                                if (whitePercent > 85) isWhite = false;
                                else isWhite = true;
                            }
                            else
                            {
                                if (whitePercent > 80) isWhite = false;
                                else isWhite = true;
                            }
                        }
                        if (isWhite)
                        {

                            whiteTotalPercent += whitePercent;
                            whiteCounts++;
                            if (whitePercent < whiteMinPercent)
                            {
                                whiteMinPercent = whitePercent;
                                whiteMinX = x;
                                whiteMinY = y;
                            }
                        }
                    }
                    if (needCheckRedBlue && (isWhite || isBlack))
                    {
                        int redPercent = getRedBlueColorPercent(
                                rgbArray, (int)Math.Round(x * vGap), (int)Math.Round(y * hGap), vGapInt, hGapInt, false, width, height);
                        int bluePercent = getRedBlueColorPercent(
                                rgbArray, (int)Math.Round(x * vGap), (int)Math.Round(y * hGap), vGapInt, hGapInt, true, width, height);
                        if (bluePercent >= 1 )
                        {
                            blueCount++;
                            if (redCount > 1 && blueCount > 1) needCheckRedBlue = false;
                            else
                            {
                                blueRedX = x;
                                blueRedY = y;
                            }
                        }
                        if (redPercent >= 1)
                        {
                            redCount++;
                            if (redCount > 1 && blueCount > 1) needCheckRedBlue = false;
                            else
                            {
                                blueRedX = x;
                                blueRedY = y;
                            }
                        }
                    }
                    if (isBlack)
                        if (isLastMove)
                            resultValue[y * boardW + x] = 3;
                        else
                            resultValue[y * boardW + x] = 1;
                    else if (isWhite)
                        if (isLastMove)
                            resultValue[y * boardW + x] = 4;
                        else
                            resultValue[y * boardW + x] = 2;
                    else
                        resultValue[y * boardW + x] = 0;
                }
            }
            if ((redCount == 1 && blueCount != 1) || (redCount != 1 && blueCount == 1))
            {
                if (resultValue[blueRedY * boardW + blueRedX] == 1)
                    resultValue[blueRedY * boardW + blueRedX] = 3;
                else if (resultValue[blueRedY * boardW + blueRedX] == 2)
                    resultValue[blueRedY * boardW + blueRedX] = 4;
            }
            else { 
               if (blackCounts >= 2 && whiteCounts >= 2)
                {
                    float blackMaxOffset =
                        Math.Abs(
                            blackMinPercent - (blackTotalPercent - blackMinPercent) / (float)(blackCounts - 1));
                    float whiteMaxOffset =
                        Math.Abs(
                            whiteMinPercent - (whiteTotalPercent - whiteMinPercent) / (float)(whiteCounts - 1));
                    if (blackMaxOffset >= whiteMaxOffset)
                    {
                        if (blackMinY >= 0 && blackMinX >= 0)
                            resultValue[blackMinY * boardW + blackMinX] = 3;
                    }
                    else
                    {
                        if (whiteMinY >= 0 && whiteMinX >= 0)
                            resultValue[whiteMinY * boardW + whiteMinX] = 4;
                    }
                }
                else if (blackCounts > 0 && whiteCounts > 0)
                {
                    if (blackCounts > whiteCounts)
                    {
                        if (blackMinY >= 0 && blackMinX >= 0)
                            resultValue[blackMinY * boardW + blackMinX] = 3;
                    }
                    if (blackCounts < whiteCounts)
                    {
                        if (whiteMinY >= 0 && whiteMinX >= 0)
                            resultValue[whiteMinY * boardW + whiteMinX] = 4;
                    }
                }
            }
            Boolean allBlack = true;
            Boolean allWhite = true;
            List<String> sendList = new List<string>();
            for (int i = 0; i < boardH; i++)
            {
                for (int j = 0; j < boardW; j++)
                {
                    int resultHere = resultValue[i * boardW + j];
                    if (resultHere ==2|| resultHere ==4)
                        allBlack = false;
                    if (resultHere == 1 || resultHere == 3)
                        allWhite = false;
                    result += resultHere + ",";
                    if (j == (boardW - 1))
                    {
                        result = result.Substring(0, result.Length - 1);
                        //Send("re=" + result);
                        sendList.Add("re=" + result);
                        result = "";
                    }
                }
            }
            if (!allWhite && !allBlack)
            {
                foreach(String line in sendList)
                {
                    Send(line);
                }
                Send("end");
            }         
            if (allBlack)
            {
                canUsePrintWindow = false;
                if (!startedSync&&!isSecondTime)
                {
                    isSecondTime = true;
                     oneTimeSync();
                }
            }
        }

        private int getRedBlueColorPercent(RgbInfo[] rgbArray, int startX, int startY, int width, int height, Boolean isBlue, int totalWidth, int totalHeight)
        {
            int sum = 0;
            if (startX + width > totalWidth) startX = totalWidth - width;
            if (startY + height > totalHeight) startY = totalHeight - height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    RgbInfo rgbInfo = rgbArray[(startY + y) * totalWidth + startX + x];
                    int red = rgbInfo.r;
                    int green = rgbInfo.g;
                    int blue = rgbInfo.b;
                    if (isBlue)
                    {
                        if (red <= 50 && green <= 50 && blue >= 150)
                        {
                            sum++;
                        }
                    }
                    else
                    {
                        if (red >= 150 && green <= 50 && blue <= 50)
                        {
                            sum++;
                        }
                    }
                }
            }
            return (100 * sum) / (width * height);
        }

        private int getWhiteBlackColorPercent(RgbInfo[] rgbArray, int startX, int startY, int width, int height, Boolean isBlack, int offset, int grayOffset, int totalWidth, int totalHeight)
        {
            int sum = 0;
            if (startX + width > totalWidth) startX = totalWidth - width;
            if (startY + height > totalHeight) startY = totalHeight - height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    RgbInfo rgbInfo = rgbArray[(startY + y) * totalWidth + startX + x];
                    int red = rgbInfo.r;
                    int green = rgbInfo.g;
                    int blue = rgbInfo.b;
                    if (Math.Abs(red - green) < grayOffset
                        && Math.Abs(green - blue) < grayOffset
                        && Math.Abs(blue - red) < grayOffset)
                    {
                        if (isBlack)
                        {
                            if (red <= offset && green <= offset && blue <= offset)
                            {
                                sum++;
                            }
                        }
                        else
                        {
                            int value = 255 - offset;
                            if (red >= value && green >= value && blue >= value)
                            {
                                sum++;
                            }
                        }
                    }
                }
            }
            return (100 * sum) / (width * height);
        }

        private void OutPut(Boolean first, RgbInfo[] rgbArray, lw.lwsoft lwh)
        {
            if (width < this.boardW || height < this.boardH)
                return;
            if (savedPlace && syncBoth)
            {
                lwh = new lw.lwsoft();
                lwh.BindWindow(hwnd, 0, 4, 0, 0, 0);                
                savedPlace = false;
                int times = 10;
                do
                {
                    lwh.MoveTo((int)Math.Round(sx1 + widthMagrin * (savedX + 0.5)), (int)Math.Round(sy1 + heightMagrin * (savedY + 0.5)));
                    lwh.LeftClick();
                    times--;
                } while (Program.verifyMove && !VerifyMove(savedX, savedY, true) && times > 0);
                lwh.UnBindWindow();
            }
            if (first)
                Send("start " + boardW + " " + boardH + " " + hwnd);         
            recognizeBoard(rgbArray);
        }

        private void OutPut3(Boolean first)
        {
            if (width < this.boardW || height < this.boardH)
                return;            
            if (first)
                Send("start " + boardW + " " + boardH);
            recognizeBoard(null);
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
                this.button11.Enabled = false;
                this.button3.Enabled = true;
                this.button10.Enabled = true;
                if (this.rdoOtherBoard.Checked)
                    this.rdo19x19.Checked = true;
                this.rdoOtherBoard.Enabled = false;
            }

        }

        public void saveOtherConfig()
        {
            string result1 = "config_readboard_others.txt";
            FileStream fs = new FileStream(result1, FileMode.Create);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            int customW = -1;
            int customH = -1;
            posX = this.Location.X;
            posY = this.Location.Y;
            try
            {
                customW = int.Parse(txtBoardWidth.Text);
                customH = int.Parse(txtBoardHeight.Text);
            }
            catch (Exception)
            {
            }
            wr.WriteLine(this.boardW + "_" + this.boardH + "_" + customW + "_" + customH + "_" + Program.timeinterval + "_" + (syncBoth ? "1" : "0") + "_" + Program.grayOffset + "_" + posX + "_" + posY+"_"+ (Program.useEnhanceScreen ? "1" : "0"));
            wr.Close();
        }

        public void shutdown()
        {
            isContinuousSyncing = false;
            keepSync = false;
            string result1 = "config_readboard.txt";
            FileStream fs = new FileStream(result1, FileMode.Create);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            wr.WriteLine(Program.blackPC + "_" + Program.blackZB + "_" + Program.whitePC + "_" + Program.whiteZB + "_" + (Program.useMag ? "1" : "0") + "_" + (Program.verifyMove ? "1" : "0") + "_" + (Program.showScaleHint ? "1" : "0") + "_" + (Program.showInBoard ? "1" : "0") + "_" + (Program.showInBoardHint ? "1" : "0") + "_" + (Program.autoMin ? "1" : "0") + "_" + (Program.isAdvScale ? "1" : "0") + "_" + Environment.GetEnvironmentVariable("computername").Replace("_", "") + "_" + type);
            wr.Close();
            saveOtherConfig();
            mh.Enabled = false;              
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
            saveOtherConfig();
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
            saveOtherConfig();
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
            saveOtherConfig();
        }

        public void sendVersion()
        {
            Send("version: " + Program.version);
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

        class MoveInfo
        {
            public int x;
            public int y;
        }

        private void placeStone(int x, int y)
        {
            if (isJavaFrame)
            {
                foreMouseClick(javaX+sx1 + (int)(widthMagrin * (x + 0.5)), javaY+sy1 + (int)(heightMagrin * (y + 0.5)));
            }
           else if (type == 5)
            {
                foreMouseClick(sx1 + (int)(widthMagrin * (x + 0.5)), sy1 + (int)(heightMagrin * (y + 0.5)));
            }
            else if (type == 0 && !canUseLW)
            {
                object xo, yo;
                dm.GetCursorPos(out xo, out yo);
                dm.MoveTo((int)Math.Round(rectX1 + sx1 + widthMagrin * (x + 0.5)), (int)Math.Round(rectY1 + sy1 + heightMagrin * (y + 0.5)));
                dm.LeftClick();
                dm.LeftClick();
                dm.MoveTo((int)xo, (int)yo);
            }
            else
            {
                int xx = 0;
                int yy = 0;
                if (Program.isAdvScale)
                {   // dm.MoveTo((int)Math.Round(sx1 + widthMagrin * (x + 0.5)), (int)Math.Round(sy1 + heightMagrin * (y + 0.5)));
                    xx = (int)Math.Round(sx1 + widthMagrin * (x + 0.5));
                    yy = (int)Math.Round(sy1 + heightMagrin * (y + 0.5));
                }
                else
                {
                    xx = (int)Math.Round((sx1 + widthMagrin * (x + 0.5)) * factor);
                    yy = (int)Math.Round((sy1 + heightMagrin * (y + 0.5)) * factor);
                }
                //dm.MoveTo((int)Math.Round((sx1 + widthMagrin * (x + 0.5)) * factor), (int)Math.Round((sy1 + heightMagrin * (y + 0.5)) * factor));
                backMouseClick(xx, yy, hwnd);
                //dm.LeftClick();
            }
        }

        private Boolean VerifyMove(int x, int y, Boolean isLw)
        {
            Thread.Sleep(200);
            int startX = (int)Math.Round(sx1 + widthMagrin * x);
            int startY = (int)Math.Round(sy1 + heightMagrin * y);
            int width = (int)Math.Round(widthMagrin);
            int height = (int)Math.Round(heightMagrin);
            Bitmap bmp = null;
            if (type == 5)
            {
                bmp = new Bitmap(width, height);
                using (System.Drawing.Graphics graphics = Graphics.FromImage(bmp))
                {
                    graphics.CopyFromScreen(startX, startY, 0, 0, new System.Drawing.Size(width, height));
                }
            }
            else
                bmp = GetWindowBmp(new IntPtr(hwnd), startX, startY, width, height);
            return recognizeMove(bmp, x, y);
        }

        private void placeMove(object obj)
        {
            if (!syncBoth || width < boardW)
                return;
            MoveInfo move = (MoveInfo)obj;
            int x = move.x;
            int y = move.y;
            int times = 10;
            if ((type == 0) && canUseLW)
            {
                savedPlace = true;
                savedX = x;
                savedY = y;
            }
            else
            {
                do
                {
                    placeStone(x, y);
                    times--;
                } while (Program.verifyMove && !VerifyMove(x, y, false) && times > 0);
            }
        }

        public void place(int x, int y)
        {
            if (!keepSync)
                return;
            MoveInfo move = new MoveInfo();
            move.x = x;
            move.y = y;
            Thread t = new Thread(placeMove);
            t.Start(move);
        }

        uint WM_LBUTTONDOWN = 0x201;
        uint WM_LBUTTONUP = 0x202;

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        private void backMouseClick(int x, int y, int hwnd)
        {
            PostMessage(new IntPtr(hwnd), WM_LBUTTONDOWN, 0, x + (y << 16));
            PostMessage(new IntPtr(hwnd), WM_LBUTTONUP, 0, x + (y << 16));
        }

        public enum MouseEventFlags
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            Wheel = 0x0800,
            Absolute = 0x8000
        }
        [DllImport("user32.dll")]
        private static extern int SetCursorPos(int x, int y);
        [DllImport("User32")]
        public extern static void mouse_event(int dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);

        private void foreMouseClick(int x, int y)
        {
            int curX = Control.MousePosition.X;
            int curY = Control.MousePosition.Y;
            SetCursorPos(x, y);
            mouse_event((int)(MouseEventFlags.LeftUp | MouseEventFlags.Absolute), 0, 0, 0, IntPtr.Zero);
            mouse_event((int)(MouseEventFlags.LeftDown | MouseEventFlags.Absolute), 0, 0, 0, IntPtr.Zero);
            mouse_event((int)(MouseEventFlags.LeftUp | MouseEventFlags.Absolute), 0, 0, 0, IntPtr.Zero);
            SetCursorPos(curX, curY);
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
            Send("timechanged " + (textBox1.Text.Equals("") ? "0" : textBox1.Text));
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
            syncBoth = chkBothSync.Checked;
            if (syncBoth)
            {
                Send("bothSync");
                chkAutoPlay.Enabled = true;
            }
            else
            {
                Send("nobothSync");
                chkAutoPlay.Enabled = false;
            }
            if (keepSync)
            {
                Send("sync");
                if (radioBlack.Checked)
                {
                    Send("play>black>" + (textBox1.Text.Equals("") ? "0" : textBox1.Text) + " " + (textBox2.Text.Equals("") ? "0" : textBox2.Text) + " " + (textBox3.Text.Equals("") ? "0" : textBox3.Text));
                }
                else if (radioWhite.Checked)
                {
                    Send("play>white>" + (textBox1.Text.Equals("") ? "0" : textBox1.Text) + " " + (textBox2.Text.Equals("") ? "0" : textBox2.Text) + " " + (textBox3.Text.Equals("") ? "0" : textBox3.Text));
                }
            }
            this.saveOtherConfig();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Process process1 = new Process();
                process1.StartInfo.FileName = "readboard\\readme.rtf";
                process1.StartInfo.Arguments = "";
                process1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                process1.Start();
            }
            catch (Exception)
            {
                MessageBox.Show(Program.isChn ? "找不到说明文档,请检查Lizzie目录下[readboard]文件夹内的[readme.rtf]文件是否存在" : "Can not find file,Please check [readme.rtf] file is in the folder [readboard]");
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
            else
            {
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
            Form4 form4 = new Form4();
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
            saveOtherConfig();
        }



        private void parseWidth(object sender, EventArgs e)
        {
            try
            {
                if (this.rdoOtherBoard.Checked)
                {
                    this.boardW = int.Parse(txtBoardWidth.Text);
                    saveOtherConfig();
                }
                else
                {
                    int w = int.Parse(txtBoardWidth.Text);
                }
            }
            catch (Exception)
            {
            }
        }

        private void parseHeight(object sender, EventArgs e)
        {
            try
            {
                if (this.rdoOtherBoard.Checked)
                {
                    this.boardH = int.Parse(txtBoardHeight.Text);
                    saveOtherConfig();
                }
                else
                {
                    int h = int.Parse(txtBoardHeight.Text);
                }
            }
            catch (Exception)
            {
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
            wr.WriteLine(Program.blackPC + "_" + Program.blackZB + "_" + Program.whitePC + "_" + Program.whiteZB + "_" + (Program.useMag ? "1" : "0") + "_" + (Program.verifyMove ? "1" : "0") + "_" + (Program.showScaleHint ? "1" : "0") + "_" + (Program.showInBoard ? "1" : "0") + "_" + (Program.showInBoardHint ? "1" : "0") + "_" + (Program.autoMin ? "1" : "0") + "_" + (Program.isAdvScale ? "1" : "0") + "_" + Environment.GetEnvironmentVariable("computername").Replace("_", "") + "_" + type);
            wr.Close();
            if (chkShowInBoard.Checked)
            {
                if (Program.showInBoardHint)
                {

                    Form7 form7 = new Form7();
                    form7.ShowDialog();

                }
            }
            else
            {
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
                MessageBox.Show(Program.isChn ? "找不到说明文档,请检查Lizzie目录下[readboard]文件夹内的[65komi.rtf]文件是否存在" : "Can not find file,Please check [65komi.rtf] file is in the folder [readboard]");
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
                this.btnKeepSync.Enabled = false;
                thread = new Thread(startContinuous);
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                if (Program.autoMin && this.WindowState != FormWindowState.Minimized)
                {
                    minWindow("");
                }
            }
            else
            {
                isContinuousSyncing = false;
                stopSync();
                this.button10.Text = Program.isChn ? "一键同步" : "FastSync";
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
                    graphics.CopyFromScreen(x, y, 0, 0, new System.Drawing.Size(width, height));
                    //  bitmap.Save("screen.bmp");
                }
                try
                {
                    boardLineAjust(boardW, boardH);
                }
                catch (Exception e)
                {
                    SendError(e.ToString());
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

        private void selectBoard()
        {
            mh.Enabled = true;
            this.WindowState = FormWindowState.Minimized;
            form2 = new Form2(isMannulCircle);
            form2.ShowDialog();
        }

        private List<verticalLine> verticalLines;
        private List<horizonLine> horizonLines;

        private void boardLineAjust(int boardWidth1, int boardHeight1)
        {
            verticalLines = new List<verticalLine>();
            horizonLines = new List<horizonLine>();
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
            int lines = verticalLines.Count + horizonLines.Count;
            int maxTimes = 10;
            while (lines < 10 && maxTimes > 0)
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
                        int gap2 = verticalGapCounts[2].gap;
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
                    if (Math.Abs(line2.position - line1.position - verticalGap) <= Math.Max(1, verticalGap / 8))
                    {
                        line1.validate = true;
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
                    if (Math.Abs(line2.position - line1.position - horizonGap) <= Math.Max(1, horizonGap / 8))
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

            while (verticalOffsets.Count >= 2 && Math.Abs(verticalOffsets[0].offset - verticalOffsets[1].offset) <= verticalGap / 3)
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
                    if (line.startPoint.X < v0Min && line.endPoint.X > v0Min)
                        v0MinCounts++;
                    if (line.endPoint.X > v0Max && line.startPoint.X < v0Max)
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

            while (horizonOffsets.Count >= 2 && Math.Abs(horizonOffsets[0].offset - horizonOffsets[1].offset) <= horizonGap / 3)
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
            ox2 = ox1 + rightPos + hGap;
            oy2 = oy1 + bottomPos + vGap;

            ox1 = ox1 + leftPos - hGap;
            oy1 = oy1 + topPos - vGap;
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

        private LineSegmentPoint[] FindLines(Mat srcImage, int boardWidth1, int boardHeight1, int lowCanny, int upCanny)
        {
            Mat contours = new Mat();
            Cv2.Canny(srcImage, contours, lowCanny, upCanny);
            int length = Math.Min(srcImage.Width, srcImage.Height);
            //  Cv2.ImShow("轮廓", contours);         
            int size = Math.Min(boardWidth1, boardHeight1);
            LineSegmentPoint[] linesPoint = Cv2.HoughLinesP(contours, 1, Cv2.PI / 180, (int)(length / (((size + 1) * 1.5f))), (int)(length / (((size + 1) * 1.5f))), 1);
            //  Console.WriteLine(linesPoint.Length);
            return linesPoint;
        }

        private Boolean getTygemPos(IntPtr hwnd, RgbInfo[] rgbArray)
        {
            Bitmap input = GetWindowImage(hwnd, isFirstGetPos);
            if (input == null || input.Width <= boardW || input.Height <= boardH)
                return false;
            Rectangle rect2 = new Rectangle(0, 0, input.Width, input.Height);
            const int PixelWidth = 3;
            const PixelFormat PixelFormat = PixelFormat.Format24bppRgb;
            BitmapData data = input.LockBits(rect2, ImageLockMode.ReadOnly, PixelFormat);
            rgbArray = new RgbInfo[data.Height * data.Width];
            byte[] pixelData = new Byte[data.Stride];
            for (int scanline = 0; scanline < data.Height; scanline++)
            {
                Marshal.Copy(data.Scan0 + (scanline * data.Stride), pixelData, 0, data.Stride);
                for (int pixeloffset = 0; pixeloffset < data.Width; pixeloffset++)
                {
                    // PixelFormat.Format32bppRgb means the data is stored
                    // in memory as BGR. We want RGB, so we must do some 
                    // bit-shuffling.
                    rgbArray[scanline * data.Width + pixeloffset] = new RgbInfo();
                    rgbArray[scanline * data.Width + pixeloffset].r = pixelData[pixeloffset * PixelWidth + 2];
                    rgbArray[scanline * data.Width + pixeloffset].g = pixelData[pixeloffset * PixelWidth + 1];
                    rgbArray[scanline * data.Width + pixeloffset].b = pixelData[pixeloffset * PixelWidth];
                }
            }
            int width = data.Width;
            int height = data.Height;
            input.UnlockBits(data);
            input.Dispose();
            Boolean found = false;
            try
            {
                RgbInfo cl = rgbArray[0];
                if (cl.r > 233 && cl.g > 203 && cl.b > 120 && cl.b < 180)
                {
                    found = true;
                    if(cansetFirstGetPos)
                    isFirstGetPos = false;
                }
            }
            catch (Exception)
            {
            }
            return found;
        }
        private Boolean getSinaPos(IntPtr hwnd, RgbInfo[] rgbArray)
        {
            Bitmap input = GetWindowImage(hwnd, isFirstGetPos);
            if (input == null || input.Width <= boardW || input.Height <= boardH)
                return false;
            Rectangle rect2 = new Rectangle(0, 0, input.Width, input.Height);
            const int PixelWidth = 3;
            const PixelFormat PixelFormat = PixelFormat.Format24bppRgb;
            BitmapData data = input.LockBits(rect2, ImageLockMode.ReadOnly, PixelFormat);
            rgbArray = new RgbInfo[data.Height * data.Width];
            byte[] pixelData = new Byte[data.Stride];
            for (int scanline = 0; scanline < data.Height; scanline++)
            {
                Marshal.Copy(data.Scan0 + (scanline * data.Stride), pixelData, 0, data.Stride);
                for (int pixeloffset = 0; pixeloffset < data.Width; pixeloffset++)
                {
                    // PixelFormat.Format32bppRgb means the data is stored
                    // in memory as BGR. We want RGB, so we must do some 
                    // bit-shuffling.
                    rgbArray[scanline * data.Width + pixeloffset] = new RgbInfo();
                    rgbArray[scanline * data.Width + pixeloffset].r = pixelData[pixeloffset * PixelWidth + 2];
                    rgbArray[scanline * data.Width + pixeloffset].g = pixelData[pixeloffset * PixelWidth + 1];
                    rgbArray[scanline * data.Width + pixeloffset].b = pixelData[pixeloffset * PixelWidth];
                }
            }
            int width = data.Width;
            int height = data.Height;
            input.UnlockBits(data);
            input.Dispose();
            //251,218,162 左到右 上到下 251,218,162 向下1,2 偏移 < 5(左上角)(实际找到正数第二的点sx1 - 1, sy1 - 1)
            //251,218,162 左到右 下到上251,218,162 向下1,2 偏移 < 5(左下角)(实际找到倒数第三的点, height + 4)
            // "FBDAA2-050505", "0|1|FBDAA2-050505,0|2|FBDAA2-050505", 1.0, 0, out qx1, out qy1);      
            // "FBDAA2-050505", "0|1|FBDAA2-050505,0|2|FBDAA2-050505", 1.0, 1, out qx4, out qy4);
            List<ColorInfo> colorInfos = new List<ColorInfo>();
            ColorInfo colorInfo1 = new ColorInfo();
            colorInfo1.r = 251;
            colorInfo1.g = 218;
            colorInfo1.b = 162;
            colorInfo1.rOffset = 5;
            colorInfo1.gOffset = 5;
            colorInfo1.bOffset = 5;
            colorInfos.Add(colorInfo1);
            ColorInfo colorInfo2 = new ColorInfo();
            colorInfo2.r = 251;
            colorInfo2.g = 218;
            colorInfo2.b = 162;
            colorInfo2.rOffset = 5;
            colorInfo2.gOffset = 5;
            colorInfo2.bOffset = 5;
            colorInfo2.xOffset = 0;
            colorInfo2.yOffset = 1;
            colorInfos.Add(colorInfo2);
            ColorInfo colorInfo3 = new ColorInfo();
            colorInfo3.r = 251;
            colorInfo3.g = 218;
            colorInfo3.b = 162;
            colorInfo3.rOffset = 5;
            colorInfo3.gOffset = 5;
            colorInfo3.bOffset = 5;
            colorInfo3.xOffset = 0;
            colorInfo3.yOffset = 2;
            colorInfos.Add(colorInfo3);

            Point upLeft = getMultiColorPos(rgbArray, width, height, colorInfos, 0);
            Point downLeft = getMultiColorPos(rgbArray, width, height, colorInfos, 2);
            if (upLeft.X < 0 || upLeft.Y < 0 || downLeft.X < 0 || downLeft.Y <= 0)
                return false;
            sx1 = upLeft.X - 1;
            sy1 = upLeft.Y - 1;
            this.height = downLeft.Y - upLeft.Y + 4;
            this.width = this.height;
            if (cansetFirstGetPos)
                isFirstGetPos = false;
            return true;
        }

        private Boolean getFoxPos(IntPtr hwnd, RgbInfo[] rgbArray)
        {
            Bitmap input = GetWindowImage(hwnd, isFirstGetPos);
            if (input == null || input.Width <= boardW || input.Height <= boardH)
                return false;
            Rectangle rect2 = new Rectangle(0, 0, input.Width, input.Height);
            const int PixelWidth = 3;
            const PixelFormat PixelFormat = PixelFormat.Format24bppRgb;
            BitmapData data = input.LockBits(rect2, ImageLockMode.ReadOnly, PixelFormat);
            rgbArray = new RgbInfo[data.Height * data.Width];
            byte[] pixelData = new Byte[data.Stride];
            for (int scanline = 0; scanline < data.Height; scanline++)
            {
                Marshal.Copy(data.Scan0 + (scanline * data.Stride), pixelData, 0, data.Stride);
                for (int pixeloffset = 0; pixeloffset < data.Width; pixeloffset++)
                {
                    // PixelFormat.Format32bppRgb means the data is stored
                    // in memory as BGR. We want RGB, so we must do some 
                    // bit-shuffling.
                    rgbArray[scanline * data.Width + pixeloffset] = new RgbInfo();
                    rgbArray[scanline * data.Width + pixeloffset].r = pixelData[pixeloffset * PixelWidth + 2];
                    rgbArray[scanline * data.Width + pixeloffset].g = pixelData[pixeloffset * PixelWidth + 1];
                    rgbArray[scanline * data.Width + pixeloffset].b = pixelData[pixeloffset * PixelWidth];
                }
            }
            int width = data.Width;
            int height = data.Height;
            input.UnlockBits(data);
            input.Dispose();
            //49,49,49 左到右 上到下 46,46,46 向下1,2 向右1,2 偏移 < 5(左上角)

            //  "313131-050505", "2|1|-2E2E2E-050505,1|1|-2E2E2E-050505,1|2|-2E2E2E-050505", 1.0, 0, out qx4, out qy4);


            List<ColorInfo> colorInfos = new List<ColorInfo>();
            ColorInfo colorInfo1 = new ColorInfo();
            colorInfo1.r = 49;
            colorInfo1.g = 49;
            colorInfo1.b = 49;
            colorInfo1.rOffset = 5;
            colorInfo1.gOffset = 5;
            colorInfo1.bOffset = 5;
            colorInfos.Add(colorInfo1);
            ColorInfo colorInfo2 = new ColorInfo();
            colorInfo2.r = 46;
            colorInfo2.g = 46;
            colorInfo2.b = 46;
            colorInfo2.rOffset = 5;
            colorInfo2.gOffset = 5;
            colorInfo2.bOffset = 5;
            colorInfo2.xOffset = 0;
            colorInfo2.yOffset = 1;
            colorInfos.Add(colorInfo2);
            ColorInfo colorInfo3 = new ColorInfo();
            colorInfo3.r = 46;
            colorInfo3.g = 46;
            colorInfo3.b = 46;
            colorInfo3.rOffset = 5;
            colorInfo3.gOffset = 5;
            colorInfo3.bOffset = 5;
            colorInfo3.xOffset = 0;
            colorInfo3.yOffset = 2;
            colorInfos.Add(colorInfo3);
            ColorInfo colorInfo4 = new ColorInfo();
            colorInfo4.r = 46;
            colorInfo4.g = 46;
            colorInfo4.b = 46;
            colorInfo4.rOffset = 5;
            colorInfo4.gOffset = 5;
            colorInfo4.bOffset = 5;
            colorInfo4.xOffset = 1;
            colorInfo4.yOffset = 0;
            colorInfos.Add(colorInfo4);
            ColorInfo colorInfo5 = new ColorInfo();
            colorInfo5.r = 46;
            colorInfo5.g = 46;
            colorInfo5.b = 46;
            colorInfo5.rOffset = 5;
            colorInfo5.gOffset = 5;
            colorInfo5.bOffset = 5;
            colorInfo5.xOffset = 2;
            colorInfo5.yOffset = 0;
            colorInfos.Add(colorInfo5);
            ColorInfo colorInfo6 = new ColorInfo();
            colorInfo6.r = 46;
            colorInfo6.g = 46;
            colorInfo6.b = 46;
            colorInfo6.rOffset = 5;
            colorInfo6.gOffset = 5;
            colorInfo6.bOffset = 5;
            colorInfo6.xOffset = 2;
            colorInfo6.yOffset = 1;
            colorInfo6.isReverse = true;
            colorInfos.Add(colorInfo6);
            ColorInfo colorInfo7 = new ColorInfo();
            colorInfo7.r = 46;
            colorInfo7.g = 46;
            colorInfo7.b = 46;
            colorInfo7.rOffset = 5;
            colorInfo7.gOffset = 5;
            colorInfo7.bOffset = 5;
            colorInfo7.xOffset = 1;
            colorInfo7.yOffset = 1;
            colorInfo7.isReverse = true;
            colorInfos.Add(colorInfo7);
            ColorInfo colorInfo8 = new ColorInfo();
            colorInfo8.r = 46;
            colorInfo8.g = 46;
            colorInfo8.b = 46;
            colorInfo8.rOffset = 5;
            colorInfo8.gOffset = 5;
            colorInfo8.bOffset = 5;
            colorInfo8.xOffset = 1;
            colorInfo8.yOffset = 2;
            colorInfo8.isReverse = true;
            colorInfos.Add(colorInfo8);
            // "313131-050505", "-2|1|-2E2E2E-050505,-1|1|-2E2E2E-050505,-1|2|-2E2E2E-050505", 1.0, 2, out qx1, out qy1);
            //49,49,49  右到左 上到下 46,46,46 向下1,2 向左1,2 偏移 < 5(右上角)
            List<ColorInfo> colorInfos2 = new List<ColorInfo>();
            ColorInfo colorInfo21 = new ColorInfo();
            colorInfo21.r = 49;
            colorInfo21.g = 49;
            colorInfo21.b = 49;
            colorInfo21.rOffset = 5;
            colorInfo21.gOffset = 5;
            colorInfo21.bOffset = 5;
            colorInfos2.Add(colorInfo21);
            ColorInfo colorInfo22 = new ColorInfo();
            colorInfo22.r = 46;
            colorInfo22.g = 46;
            colorInfo22.b = 46;
            colorInfo22.rOffset = 5;
            colorInfo22.gOffset = 5;
            colorInfo22.bOffset = 5;
            colorInfo22.xOffset = 0;
            colorInfo22.yOffset = 1;
            colorInfos2.Add(colorInfo22);
            ColorInfo colorInfo23 = new ColorInfo();
            colorInfo23.r = 46;
            colorInfo23.g = 46;
            colorInfo23.b = 46;
            colorInfo23.rOffset = 5;
            colorInfo23.gOffset = 5;
            colorInfo23.bOffset = 5;
            colorInfo23.xOffset = 0;
            colorInfo23.yOffset = 2;
            colorInfos2.Add(colorInfo23);
            ColorInfo colorInfo24 = new ColorInfo();
            colorInfo24.r = 46;
            colorInfo24.g = 46;
            colorInfo24.b = 46;
            colorInfo24.rOffset = 5;
            colorInfo24.gOffset = 5;
            colorInfo24.bOffset = 5;
            colorInfo24.xOffset = -1;
            colorInfo24.yOffset = 0;
            colorInfos2.Add(colorInfo24);
            ColorInfo colorInfo25 = new ColorInfo();
            colorInfo25.r = 46;
            colorInfo25.g = 46;
            colorInfo25.b = 46;
            colorInfo25.rOffset = 5;
            colorInfo25.gOffset = 5;
            colorInfo25.bOffset = 5;
            colorInfo25.xOffset = -2;
            colorInfo25.yOffset = 0;
            colorInfos2.Add(colorInfo25);
            ColorInfo colorInfo26 = new ColorInfo();
            colorInfo26.r = 46;
            colorInfo26.g = 46;
            colorInfo26.b = 46;
            colorInfo26.rOffset = 5;
            colorInfo26.gOffset = 5;
            colorInfo26.bOffset = 5;
            colorInfo26.xOffset = -2;
            colorInfo26.yOffset = 1;
            colorInfo26.isReverse = true;
            colorInfos2.Add(colorInfo26);
            ColorInfo colorInfo27 = new ColorInfo();
            colorInfo27.r = 46;
            colorInfo27.g = 46;
            colorInfo27.b = 46;
            colorInfo27.rOffset = 5;
            colorInfo27.gOffset = 5;
            colorInfo27.bOffset = 5;
            colorInfo27.xOffset = -1;
            colorInfo27.yOffset = 1;
            colorInfo27.isReverse = true;
            colorInfos2.Add(colorInfo27);
            ColorInfo colorInfo28 = new ColorInfo();
            colorInfo28.r = 46;
            colorInfo28.g = 46;
            colorInfo28.b = 46;
            colorInfo28.rOffset = 5;
            colorInfo28.gOffset = 5;
            colorInfo28.bOffset = 5;
            colorInfo28.xOffset = -1;
            colorInfo28.yOffset = 2;
            colorInfo28.isReverse = true;
            colorInfos2.Add(colorInfo28);

            Point upLeft = getMultiColorPos(rgbArray, width, height, colorInfos, 0);
            Point upRight = getMultiColorPos(rgbArray, width, height, colorInfos2, 1);
            if (upLeft.X < 0 || upLeft.Y < 0 || upRight.X < 0 || upRight.Y <= 0)
                return false;
           
                sx1 = upLeft.X;
                sy1 = upLeft.Y;
                this.width = upRight.X - upLeft.X;
                this.height = this.width;
            if (cansetFirstGetPos)
                isFirstGetPos = false;
            return true;
        }

        private Point getMultiColorPos(RgbInfo[] rgbArray, int width, int height, List<ColorInfo> colorInfos, int direction)
        {
            Point returnPoint = new Point();
            returnPoint.X = -1;
            returnPoint.Y = -1;
            switch (direction)
            {
                case 0:
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            if (getMultiColorPixel(rgbArray, x, y, width, height, colorInfos))
                            {
                                returnPoint.X = x;
                                returnPoint.Y = y;
                                return returnPoint;
                            }
                        }
                    }
                    break;
                case 1:
                    for (int x = width - 1; x >= 0; x--)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            if (getMultiColorPixel(rgbArray, x, y, width, height, colorInfos))
                            {
                                returnPoint.X = x;
                                returnPoint.Y = y;
                                return returnPoint;
                            }
                        }
                    }
                    break;
                case 2:
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = height - 1; y >= 0; y--)
                        {
                            if (getMultiColorPixel(rgbArray, x, y, width, height, colorInfos))
                            {
                                returnPoint.X = x;
                                returnPoint.Y = y;
                                return returnPoint;
                            }
                        }
                    }
                    break;
                case 3:
                    for (int x = width - 1; x >= 0; x--)
                    {
                        for (int y = height - 1; y >= 0; y--)
                        {
                            if (getMultiColorPixel(rgbArray, x, y, width, height, colorInfos))
                            {
                                returnPoint.X = x;
                                returnPoint.Y = y;
                                return returnPoint;
                            }
                        }
                    }
                    break;
            }
            return returnPoint;
        }
        private Boolean getMultiColorPixel(RgbInfo[] rgbArray, int x, int y, int width, int heigt, List<ColorInfo> colorInfos)
        {
            RgbInfo rgb1 = rgbArray[y * width + x];
            ColorInfo cl1 = colorInfos[0];
            if (Math.Abs(cl1.r - rgb1.r) < cl1.rOffset && Math.Abs(cl1.g - rgb1.g) < cl1.gOffset && Math.Abs(cl1.b - rgb1.b) < cl1.bOffset)
            {
                for (int i = 0; i < colorInfos.Count; i++)
                {
                    ColorInfo cl = colorInfos[i];
                    if (x + cl.xOffset >= 0 && y + cl.yOffset >= 0)
                    {
                        int index = (y + cl.yOffset) * width + x + cl.xOffset;
                        if (index < rgbArray.Length)
                        {
                            RgbInfo rgb = rgbArray[index];
                            if (cl.isReverse)
                            {
                                if (Math.Abs(cl.r - rgb.r) < cl.rOffset && Math.Abs(cl.g - rgb.g) < cl.gOffset && Math.Abs(cl.b - rgb.b) < cl.bOffset)
                                {
                                    return false;
                                }
                            }
                            else
                            if (Math.Abs(cl.r - rgb.r) >= cl.rOffset || Math.Abs(cl.g - rgb.g) >= cl.gOffset || Math.Abs(cl.b - rgb.b) >= cl.bOffset)
                            {
                                return false;
                            }
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
                return true;
            }
            else
                return false;
        }
    }
}

class ColorInfo
{
    public int xOffset;
    public int yOffset;
    public int r;
    public int g;
    public int b;
    public int rOffset;
    public int gOffset;
    public int bOffset;
    public Boolean isReverse = false;
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

