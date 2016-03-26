using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WinSystem;

namespace Pet
{
    public partial class FishForm : Form
    {
        private const uint WS_EX_LAYERED = 0x80000;
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int GWL_STYLE = (-16);
        private const int GWL_EXSTYLE = (-20);
        private const int LWA_ALPHA = 0;

        [DllImport("user32", EntryPoint = "SetWindowLong")]
        private static extern uint SetWindowLong(
        IntPtr hwnd,
        int nIndex,
        uint dwNewLong
        );

        [DllImport("user32", EntryPoint = "GetWindowLong")]
        private static extern uint GetWindowLong(
        IntPtr hwnd,
        int nIndex
        );

        [DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
        private static extern int SetLayeredWindowAttributes(
        IntPtr hwnd,
        int crKey,
        int bAlpha,
        int dwFlags
        );

        /// <summary> 
        /// 设置窗体具有鼠标穿透效果 
        /// </summary> 
        public void SetPenetrate()
        {
            this.TopMost = true;
            GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, WS_EX_TRANSPARENT | WS_EX_LAYERED);
            SetLayeredWindowAttributes(this.Handle, 0, 100, LWA_ALPHA);
        }


        Point oldPoint = new Point(0, 0);
        bool mouseDown = false;
        bool haveHandle = false;
        Timer timerSpeed = new Timer();
        int MaxCount = 50;
        float stepX = 2f;
        float stepY = 0f;
        int count = 0;
        bool speedMode = false;
        float left = 0f, top = 0f;

        bool toRight = true;        //是否向右
        int frameCount = 20;        //总帧数
        int frame = 0;              //当前帧
        int frameWidth = 100;       //每帧宽度
        int frameHeight = 100;      //每帧高度

        public FishForm()
        {
            InitializeComponent();
            toRight = true;
            frame = 20;
            frame = 0;
            frameWidth = FullImage.Width / 20;
            frameHeight = FullImage.Height;
            left = -frameWidth;
            top = Screen.PrimaryScreen.WorkingArea.Height / 2f;

            timerSpeed.Interval = 50;
            timerSpeed.Enabled = true;
            timerSpeed.Tick += new EventHandler(timerSpeed_Tick);

            this.DoubleClick += new EventHandler(Form2_DoubleClick);
            this.MouseDown += new MouseEventHandler(Form2_MouseDown);
            this.MouseUp += new MouseEventHandler(Form2_MouseUp);
            this.MouseMove += new MouseEventHandler(Form2_MouseMove);
        }

        #region 重载

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
            haveHandle = false;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            InitializeStyles();
            base.OnHandleCreated(e);
            haveHandle = true;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParms = base.CreateParams;
                cParms.ExStyle |= 0x00080000; // WS_EX_LAYERED
                return cParms;
            }
        }

        #endregion

        void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            count = 0;
            MaxCount = new Random().Next(70) + 40;
            timerSpeed.Interval = new Random().Next(20) + 2;
            speedMode = true;
            mouseDown = false;
        }

        private void InitializeStyles()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        void timerSpeed_Tick(object sender, EventArgs e)
        {
            if (!mouseDown)
            {
                count++;
                if (count > MaxCount)
                {
                    MaxCount = new Random().Next(70) + 30;
                    if (speedMode) timerSpeed.Interval = 50;

                    count = 0;
                    stepX = (float)new Random().NextDouble() * 3f + 1f;
                    stepY = (float)new Random().NextDouble() * 0.5f;
                    if (stepY < 0.3f) stepY = 0f;
                    stepY = (new Random().Next(2) == 0 ? -1 : 1) * stepY;
                }
                Console.WriteLine(left + "   " + top);
                left = (left + (toRight ? 1 : -1) * stepX);
                top = (top + stepY);
                FixLeftTop();
                this.Left = (int)left;
                this.Top = (int)top;
            }
            frame++;
            if (frame >= frameCount) frame = 0;

            SetBits(FrameImage);
        }

        private void FixLeftTop()
        {
            if (toRight && left > Screen.PrimaryScreen.WorkingArea.Width)
            {
                toRight = false;
                frame = 0;
                count = 0;
            }
            else if (!toRight && left < -frameWidth)
            {
                toRight = true;
                frame = 0;
                count = 0;
            }
            if (top < -frameHeight)
            {
                stepY = 1f;
                count = 0;
            }
            else if (top > Screen.PrimaryScreen.WorkingArea.Height)
            {
                stepY = -1f;
                count = 0;
            }
        }

        /// <summary>
        /// 背景图片
        /// </summary>
        private Image FullImage
        {
            get
            {
                if (toRight)
                    return RotateTransformDemo.Properties.Resources.Right;
                else
                    return RotateTransformDemo.Properties.Resources.Left;
            }
        }

        /// <summary>
        /// 返回当前帧图片
        /// </summary>
        public Bitmap FrameImage
        {
            get
            {
                Bitmap bitmap = new Bitmap(frameWidth, frameHeight);
                Graphics g = Graphics.FromImage(bitmap);
                g.DrawImage(FullImage, new Rectangle(0, 0, bitmap.Width, bitmap.Height), new Rectangle(frameWidth * frame, 0, frameWidth, frameHeight), GraphicsUnit.Pixel);
                return bitmap;
            }
        }

        void Form2_DoubleClick(object sender, EventArgs e)
        {
            this.Dispose();
        }

        void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Left += (e.X - oldPoint.X);
                this.Top += (e.Y - oldPoint.Y);
                left = this.Left;
                top = this.Top;
                FixLeftTop();
            }
        }

        void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) this.Dispose();
            oldPoint = e.Location;
            mouseDown = true;
        }

        public void SetBits(Bitmap bitmap)
        {
            if (!haveHandle) return;

            if (!Bitmap.IsCanonicalPixelFormat(bitmap.PixelFormat) || !Bitmap.IsAlphaPixelFormat(bitmap.PixelFormat))
                throw new ApplicationException("图片必须是32位带Alhpa通道的图片。");

            IntPtr oldBits = IntPtr.Zero;
            IntPtr screenDC = Win32Api.GetDC(IntPtr.Zero);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr memDc = Win32Api.CreateCompatibleDC(screenDC);

            try
            {
                Win32Api.POINT topLoc = new Win32Api.POINT(Left, Top);
                Win32Api.Size bitMapSize = new Win32Api.Size(bitmap.Width, bitmap.Height);
                Win32Api.BLENDFUNCTION blendFunc = new Win32Api.BLENDFUNCTION();
                Win32Api.POINT srcLoc = new Win32Api.POINT(0, 0);

                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                oldBits = Win32Api.SelectObject(memDc, hBitmap);

                blendFunc.BlendOp = Win32Api.AC_SRC_OVER;
                blendFunc.SourceConstantAlpha = 255;
                blendFunc.AlphaFormat = Win32Api.AC_SRC_ALPHA;
                blendFunc.BlendFlags = 0;

                Win32Api.UpdateLayeredWindow(Handle, screenDC, ref topLoc, ref bitMapSize, memDc, ref srcLoc, 0, ref blendFunc, Win32Api.ULW_ALPHA);
            }
            finally
            {
                if (hBitmap != IntPtr.Zero)
                {
                    Win32Api.SelectObject(memDc, oldBits);
                    Win32Api.DeleteObject(hBitmap);
                }
                Win32Api.ReleaseDC(IntPtr.Zero, screenDC);
                Win32Api.DeleteDC(memDc);
            }
        }

        private void FishForm_Load(object sender, EventArgs e)
        {
            //SetPenetrate();
        }
    }
}