using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DespiertaApp {
    static class Program {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint SetThreadExecutionState(uint esFlags);

        const uint ES_SYSTEM_REQUIRED = 0x00000001;
        const uint ES_DISPLAY_REQUIRED = 0x00000002;
        const uint ES_CONTINUOUS = 0x80000000;

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool createdNew;
            using (Mutex mutex = new Mutex(true, "Local\\DespiertaWindowsPC_SingleInstance", out createdNew)) {
                if (!createdNew) {
                    MessageBox.Show(
                        "El Modo Despierta ya se encuentra activo en tu bandeja del sistema (Systray).\nHaz clic izquierdo o derecho sobre el icono para desactivarlo.",
                        "Modo Despierta",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    return;
                }

                // Activate Awake mode (Prevent Sleep + Prevent Screen Turn-off)
                uint prev = SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);

                Icon appIcon = CreateAwakeIcon();
                NotifyIcon tray = new NotifyIcon();
                tray.Icon = appIcon;
                tray.Text = "Modo Despierta: Activo\nClic izquierdo para desactivar";
                tray.Visible = true;

                ContextMenuStrip menu = new ContextMenuStrip();
                ToolStripMenuItem header = new ToolStripMenuItem("☕ Modo Despierta: Activo");
                header.Font = new Font(header.Font, FontStyle.Bold);
                header.Enabled = false;
                menu.Items.Add(header);

                ToolStripMenuItem info = new ToolStripMenuItem("Pantalla y sistema siempre encendidos");
                info.Enabled = false;
                menu.Items.Add(info);

                menu.Items.Add(new ToolStripSeparator());

                ToolStripMenuItem exitItem = new ToolStripMenuItem("❌ Desactivar y salir", null, (s, e) => {
                    Application.Exit();
                });
                menu.Items.Add(exitItem);

                tray.ContextMenuStrip = menu;

                tray.MouseClick += (s, e) => {
                    if (e.Button == MouseButtons.Left) {
                        Application.Exit();
                    }
                };

                Application.ApplicationExit += (s, e) => {
                    // Restore default Windows power state
                    SetThreadExecutionState(ES_CONTINUOUS);
                    tray.Visible = false;
                    tray.Dispose();
                };

                tray.ShowBalloonTip(
                    2500,
                    "Modo Despierta: Activado ☕",
                    "Tu PC y pantalla se mantendrán despiertos.\nHaz clic en el icono del systray para desactivar.",
                    ToolTipIcon.Info
                );

                Application.Run();
            }
        }

        static Icon CreateAwakeIcon() {
            using (Bitmap bmp = new Bitmap(32, 32)) {
                using (Graphics g = Graphics.FromImage(bmp)) {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.Clear(Color.Transparent);

                    // Circular warm badge (orange/amber)
                    using (Brush bg = new SolidBrush(Color.FromArgb(245, 130, 32))) {
                        g.FillEllipse(bg, 1, 1, 30, 30);
                    }
                    using (Pen border = new Pen(Color.FromArgb(255, 180, 70), 1.5f)) {
                        g.DrawEllipse(border, 1, 1, 30, 30);
                    }

                    // Coffee cup glyph
                    using (Pen p = new Pen(Color.White, 2.2f)) {
                        p.StartCap = LineCap.Round;
                        p.EndCap = LineCap.Round;

                        // Cup body (U-shape / rounded box)
                        g.DrawLine(p, 8, 13, 8, 21);
                        g.DrawLine(p, 8, 21, 20, 21);
                        g.DrawLine(p, 20, 21, 20, 13);
                        g.DrawLine(p, 6, 13, 22, 13);

                        // Handle
                        g.DrawArc(p, 17, 14, 7, 6, -80, 160);

                        // Steam vapors
                        g.DrawLine(p, 11, 10, 11, 7);
                        g.DrawLine(p, 15, 10, 15, 6);
                    }
                }
                IntPtr hIcon = bmp.GetHicon();
                return Icon.FromHandle(hIcon);
            }
        }
    }
}
