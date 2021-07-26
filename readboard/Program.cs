using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace readboard
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static string[] arg;
        public static int blackPC = 96;
        public static int whitePC = 96;
        public static int blackZB = 33;
        public static int whiteZB = 33;
        public static Boolean useMag = true;
        public static Boolean verifyMove = true;
        public static Boolean showScaleHint = true;
        public static Boolean showInBoard = false;
        public static Boolean showInBoardHint = true;
        public static Boolean autoMin = true;
        public static Boolean isAdvScale = false;
        public static Boolean isScaled = false;
        public static String version = "708";
        public static Boolean isChn = false;
        public static String timename="200";
        public static int timeinterval=200;
        public static int grayOffset = 50;

        public static double factor = 1.0;

        public static Boolean hasConfigFile = false;
        public static System.Drawing.Bitmap bitmap;
       //public static Thread th;
       //public  static Boolean run =true;
       // static Form Form1;
       [STAThread]
        static void Main(string[] args)
        {
            arg = args;
            //Start();
            ThreadStart threadStart = new ThreadStart(Start);
            Thread thread = new Thread(Start);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();//启动界面
            if (arg.Length < 8)
                System.Environment.Exit(0);
            if (arg[6].Equals("0"))
            {
                isChn = true;
            }
            else {
                isChn = false;
            }
                if (arg[5].Equals("0"))
            {
                AllocConsole();
                hideConsole();
                while (true)
                {
                    String a = Console.ReadLine();
                    if (a.StartsWith("place"))
                    {
                        char[] separator = { ' ' }; string[] arr = a.Split(separator);
                        Form1.pcurrentWin.place(int.Parse(arr[1]), int.Parse(arr[2]));
                    }
                    if (a.StartsWith("loss"))
                    {
                        Form1.pcurrentWin.lossFocus();
                    }
                    if (a.StartsWith("notinboard"))
                    {
                        Form1.pcurrentWin.stopInBoard();
                    }
                    if (a.StartsWith("version"))
                    {
                        Form1.pcurrentWin.sendVersion();
                    }
                    if (a.Equals("quit"))
                    {
                        Control.CheckForIllegalCrossThreadCalls = false;
                        Form1.pcurrentWin.shutdown();
                    }
                    }
            }
        }
        // 启动控制台
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        private static void hideConsole(string ConsoleTitle = "")
        {
            ConsoleTitle = String.IsNullOrEmpty(ConsoleTitle) ? Console.Title : ConsoleTitle;
            IntPtr hWnd = FindWindow("ConsoleWindowClass", ConsoleTitle);
            if (hWnd != IntPtr.Zero)
            {
                ShowWindow(hWnd, 0);
            }
        }


        private static void Start()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                if (arg[0].Equals("yzy"))
                {
                        Application.Run(new Form1(arg[1], arg[2], arg[3], arg[4], arg[5], arg[7]));                
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                System.Environment.Exit(0);
            }
        }
    }

}
