using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotKey
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("User32.dll ", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SendMessage(IntPtr HWnd, uint Msg, int WParam, int LParam);
        public const int WM_SYSCOMMAND = 0x112;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const uint WM_SYSCOMMAND2 = 0x0112;
        public const uint SC_MAXIMIZE2 = 0xF030;
        static UInt32 SW_HIDE = 0;
        static UInt32 SW_SHOWNORMAL = 1;
        static UInt32 SW_NORMAL = 1;
        static UInt32 SW_SHOWMINIMIZED = 2;
        const UInt32 SW_SHOWMAXIMIZED = 3;
        static UInt32 SW_MAXIMIZE = 3;
        static UInt32 SW_SHOWNOACTIVATE = 4;
        static UInt32 SW_SHOW = 5;
        static UInt32 SW_MINIMIZE = 6;
        static UInt32 SW_SHOWMINNOACTIVE = 7;
        static UInt32 SW_SHOWNA = 8;
        static UInt32 SW_RESTORE = 9;


        //获取窗体位置 最小化 最大化 隐藏 api
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        public Form1()
        {
            InitializeComponent();
        }
        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }


        public void OnHotkey(int HotkeyID) //home隐藏窗体，再按显示窗体。
        {
            var proc = Process.GetProcessesByName("WeChat");
            if (proc.Length > 0)
            {
                bool isNotepadMinimized = GetMinimized(proc[0].MainWindowHandle);
                if (isNotepadMinimized)
                {
                    IntPtr handle = FindWindow("微信", null);
                    richTextBox1.Text += Convert.ToString(handle);
                    SendMessage(handle, WM_SYSCOMMAND, SC_MAXIMIZE, 0);
                }
                else
                {
                    IntPtr handle = FindWindow("微信", null);
                    richTextBox1.Text += Convert.ToString(handle);
                    SendMessage(handle, WM_SYSCOMMAND, SC_MINIMIZE, 0);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Hotkey hotkey;
            hotkey = new Hotkey(this.Handle);
            Hotkey.Hotkey1 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.Home, Hotkey.KeyFlags.MOD_NONE);
            hotkey.OnHotkey += new HotkeyEventHandler(OnHotkey);
        }

        public static bool GetMinimized(IntPtr handle)
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(handle, ref placement);
            return placement.showCmd == SW_SHOWMINIMIZED;
            //1 = normal
            //2 = minimized
            //3 = maximized
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var proc = Process.GetProcessesByName("WeChat");
            MessageBox.Show(Convert.ToString(proc[0]));
            if (proc.Length > 0)
            {
                bool isNotepadMinimized = GetMinimized(proc[0].MainWindowHandle);
                if (isNotepadMinimized)
                {
                    MessageBox.Show("窗口被最小化了");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}
