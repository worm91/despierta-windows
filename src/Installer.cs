using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace DespiertaInstaller {
    static class Program {
        const string APP_PAYLOAD_B64 = "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AAAAAAQAAAAAAAAAAAAAAAEAlQEAAAAABAAAAAAAAAAAAAAAAQA2AAAAAAADAAIAAAAAAAA8TW9kdWxlPgBkZXNwaWVydGEuZXhlAFByb2dyYW0ARGVzcGllcnRhQXBwAG1zY29ybGliAFN5c3RlbQBPYmplY3QARVNfU1lTVEVNX1JFUVVJUkVEAEVTX0RJU1BMQVlfUkVRVUlSRUQARVNfQ09OVElOVU9VUwBTZXRUaHJlYWRFeGVjdXRpb25TdGF0ZQBNYWluAFN5c3RlbS5EcmF3aW5nAEljb24AQ3JlYXRlQXdha2VJY29uAGVzRmxhZ3MAU3lzdGVtLlJ1bnRpbWUuQ29tcGlsZXJTZXJ2aWNlcwBDb21waWxhdGlvblJlbGF4YXRpb25zQXR0cmlidXRlAC5jdG9yAFJ1bnRpbWVDb21wYXRpYmlsaXR5QXR0cmlidXRlAGRlc3BpZXJ0YQBTeXN0ZW0uUnVudGltZS5JbnRlcm9wU2VydmljZXMARGxsSW1wb3J0QXR0cmlidXRlAGtlcm5lbDMyLmRsbABTVEFUaHJlYWRBdHRyaWJ1dGUAPD5jX19EaXNwbGF5Q2xhc3M1AFN5c3RlbS5XaW5kb3dzLkZvcm1zAE5vdGlmeUljb24AdHJheQBFdmVudEFyZ3MAPE1haW4+Yl9fMgBzAGUAPE1haW4+Yl9fMABFdmVudEhhbmRsZXIAQ1MkPD45X19DYWNoZWRBbm9ueW1vdXNNZXRob2REZWxlZ2F0ZTMAQ29tcGlsZXJHZW5lcmF0ZWRBdHRyaWJ1dGUATW91c2VFdmVudEFyZ3MAPE1haW4+Yl9fMQBNb3VzZUV2ZW50SGFuZGxlcgBDUyQ8PjlfX0NhY2hlZEFub255bW91c01ldGhvZERlbGVnYXRlNABBcHBsaWNhdGlvbgBFeGl0AE1vdXNlQnV0dG9ucwBnZXRfQnV0dG9uAHNldF9WaXNpYmxlAFN5c3RlbS5Db21wb25lbnRNb2RlbABDb21wb25lbnQARGlzcG9zZQBFbmFibGVWaXN1YWxTdHlsZXMAU2V0Q29tcGF0aWJsZVRleHRSZW5kZXJpbmdEZWZhdWx0AFN5c3RlbS5UaHJlYWRpbmcATXV0ZXgATWVzc2FnZUJveABEaWFsb2dSZXN1bHQATWVzc2FnZUJveEJ1dHRvbnMATWVzc2FnZUJveEljb24AU2hvdwBzZXRfSWNvbgBzZXRfVGV4dABDb250ZXh0TWVudVN0cmlwAFRvb2xTdHJpcE1lbnVJdGVtAFRvb2xTdHJpcEl0ZW0ARm9udABnZXRfRm9udABGb250U3R5bGUAc2V0X0ZvbnQAc2V0X0VuYWJsZWQAVG9vbFN0cmlwAFRvb2xTdHJpcEl0ZW1Db2xsZWN0aW9uAGdldF9JdGVtcwBBZGQAVG9vbFN0cmlwU2VwYXJhdG9yAEltYWdlAHNldF9Db250ZXh0TWVudVN0cmlwAGFkZF9Nb3VzZUNsaWNrAGFkZF9BcHBsaWNhdGlvbkV4aXQAVG9vbFRpcEljb24AU2hvd0JhbGxvb25UaXAAUnVuAElEaXNwb3NhYmxlAEJpdG1hcABHcmFwaGljcwBGcm9tSW1hZ2UAU3lzdGVtLkRyYXdpbmcuRHJhd2luZzJEAFNtb290aGluZ01vZGUAc2V0X1Ntb290aGluZ01vZGUAQ29sb3IAZ2V0X1RyYW5zcGFyZW50AENsZWFyAEZyb21BcmdiAFNvbGlkQnJ1c2gAQnJ1c2gARmlsbEVsbGlwc2UAUGVuAERyYXdFbGxpcHNlAGdldF9XaGl0ZQBMaW5lQ2FwAHNldF9TdGFydENhcABzZXRfRW5kQ2FwAERyYXdMaW5lAERyYXdBcmMAR2V0SGljb24ARnJvbUhhbmRsZQAAAE9MAG8AYwBhAGwAXABEAGUAcwBwAGkAZQByAHQAYQBXAGkAbgBkAG8AdwBzAFAAQwBfAFMAaQBuAGcAbABlAEkAbgBzAHQAYQBuAGMAZQAAgRlFAGwAIABNAG8AZABvACAARABlAHMAcABpAGUAcgB0AGEAIAB5AGEAIABzAGUAIABlAG4AYwB1AGUAbgB0AHIAYQAgAGEAYwB0AGkAdgBvACAAZQBuACAAdAB1ACAAYgBhAG4AZABlAGoAYQAgAGQAZQBsACAAcwBpAHMAdABlAG0AYQAgACgAUwB5AHMAdAByAGEAeQApAC4ACgBIAGEAegAgAGMAbABpAGMAIABpAHoAcQB1AGkAZQByAGQAbwAgAG8AIABkAGUAcgBlAGMAaABvACAAcwBvAGIAcgBlACAAZQBsACAAaQBjAG8AbgBvACAAcABhAHIAYQAgAGQAZQBzAGEAYwB0AGkAdgBhAHIAbABvAC4AAB1NAG8AZABvACAARABlAHMAcABpAGUAcgB0AGEAAGtNAG8AZABvACAARABlAHMAcABpAGUAcgB0AGEAOgAgAEEAYwB0AGkAdgBvAAoAQwBsAGkAYwAgAGkAegBxAHUAaQBlAHIAZABvACAAcABhAHIAYQAgAGQAZQBzAGEAYwB0AGkAdgBhAHIAADEVJiAATQBvAGQAbwAgAEQAZQBzAHAAaQBlAHIAdABhADoAIABBAGMAdABpAHYAbwABS1AAYQBuAHQAYQBsAGwAYQAgAHkAIABzAGkAcwB0AGUAbQBhACAAcwBpAGUAbQBwAHIAZQAgAGUAbgBjAGUAbgBkAGkAZABvAHMAAClMJyAARABlAHMAYQBjAHQAaQB2AGEAcgAgAHkAIABzAGEAbABpAHIAATVNAG8AZABvACAARABlAHMAcABpAGUAcgB0AGEAOgAgAEEAYwB0AGkAdgBhAGQAbwAgABUmAYC5VAB1ACAAUABDACAAeQAgAHAAYQBuAHQAYQBsAGwAYQAgAHMAZQAgAG0AYQBuAHQAZQBuAGQAcgDhAG4AIABkAGUAcwBwAGkAZQByAHQAbwBzAC4ACgBIAGEAegAgAGMAbABpAGMAIABlAG4AIABlAGwAIABpAGMAbwBuAG8AIABkAGUAbAAgAHMAeQBzAHQAcgBhAHkAIABwAGEAcgBhACAAZABlAHMAYQBjAHQAaQB2AGEAcgAuAAEARSNgl4h6g0CNhGJDmWJ3VQAIt3pcVhk04IkCBgkEAQAAAAQCAAAABAAAAIAEAAEJCQMAAAEIsD9ffxHVCjoEAAASCQQgAQEIAyAAAQQgAQEOAwYSHQYgAgEcEiEGAAIBHBIhAwYSJQYAAgEcEi0DBhIxBCAAETkEIAEBAgQAAQECByADAQIOEAIKAAQRSQ4OEU0RUQUgAQESCQQgABJhByACARJhEWUFIAEBEmEEIAASbQUgAQgSXQUgAgEcGAggAwEOEnUSJQUgAQESVQUgAQESMQUAAQESJQggBAEIDg4ReREHCAISQRIJElUSWRJZElkSDAUgAgEICAcAARKAhRJ1BiABARGAiQUAABGAjQYgAQERgI0IAAMRgI0ICAgKIAUBEoCVCAgICAcgAgERgI0MCiAFARKAmQgICAgGIAEBEYCdDCAHARKAmQgICAgICAMgABgFAAESCRgUBwcSgIESgIUSgJUSgJkSgJkYEgkIAQAIAAAAAAAeAQABAFQCFldyYXBOb25FeGNlcHRpb25UaHJvd3MBiDMAAAAAAAAAAAAAnjMAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAJAzAAAAAAAAAABfQ29yRXhlTWFpbgBtc2NvcmVlLmRsbAAAAAAA/yUAIEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAQAAAAIAAAgBgAAAA4AACAAAAAAAAAAAAAAAAAAAABAAEAAABQAACAAAAAAAAAAAAAAAAAAAABAAEAAABoAACAAAAAAAAAAAAAAAAAAAABAAAAAACAAAAAAAAAAAAAAAAAAAAAAAABAAAAAACQAAAAoEAAAEwCAAAAAAAAAAAAAPBCAADqAQAAAAAAAAAAAABMAjQAAABWAFMAXwBWAEUAUgBTAEkATwBOAF8ASQBOAEYATwAAAAAAvQTv/gAAAQAAAAAAAAAAAAAAAAAAAAAAPwAAAAAAAAAEAAAAAQAAAAAAAAAAAAAAAAAAAEQAAAABAFYAYQByAEYAaQBsAGUASQBuAGYAbwAAAAAAJAAEAAAAVAByAGEAbgBzAGwAYQB0AGkAbwBuAAAAAAAAALAErAEAAAEAUwB0AHIAaQBuAGcARgBpAGwAZQBJAG4AZgBvAAAAiAEAAAEAMAAwADAAMAAwADQAYgAwAAAALAACAAEARgBpAGwAZQBEAGUAcwBjAHIAaQBwAHQAaQBvAG4AAAAAACAAAAAwAAgAAQBGAGkAbABlAFYAZQByAHMAaQBvAG4AAAAAADAALgAwAC4AMAAuADAAAAA8AA4AAQBJAG4AdABlAHIAbgBhAGwATgBhAG0AZQAAAGQAZQBzAHAAaQBlAHIAdABhAC4AZQB4AGUAAAAoAAIAAQBMAGUAZwBhAGwAQwBvAHAAeQByAGkAZwBoAHQAAAAgAAAARAAOAAEATwByAGkAZwBpAG4AYQBsAEYAaQBsAGUAbgBhAG0AZQAAAGQAZQBzAHAAaQBlAHIAdABhAC4AZQB4AGUAAAA0AAgAAQBQAHIAbwBkAHUAYwB0AFYAZQByAHMAaQBvAG4AAAAwAC4AMAAuADAALgAwAAAAOAAIAAEAQQBzAHMAZQBtAGIAbAB5ACAAVgBlAHIAcwBpAG8AbgAAADAALgAwAC4AMAAuADAAAAAAAAAA77u/PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiIHN0YW5kYWxvbmU9InllcyI/Pg0KPGFzc2VtYmx5IHhtbG5zPSJ1cm46c2NoZW1hcy1taWNyb3NvZnQtY29tOmFzbS52MSIgbWFuaWZlc3RWZXJzaW9uPSIxLjAiPg0KICA8YXNzZW1ibHlJZGVudGl0eSB2ZXJzaW9uPSIxLjAuMC4wIiBuYW1lPSJNeUFwcGxpY2F0aW9uLmFwcCIvPg0KICA8dHJ1c3RJbmZvIHhtbG5zPSJ1cm46c2NoZW1hcy1taWNyb3NvZnQtY29tOmFzbS52MiI+DQogICAgPHNlY3VyaXR5Pg0KICAgICAgPHJlcXVlc3RlZFByaXZpbGVnZXMgeG1sbnM9InVybjpzY2hlbWFzLW1pY3Jvc29mdC1jb206YXNtLnYzIj4NCiAgICAgICAgPHJlcXVlc3RlZEV4ZWN1dGlvbkxldmVsIGxldmVsPSJhc0ludm9rZXIiIHVpQWNjZXNzPSJmYWxzZSIvPg0KICAgICAgPC9yZXF1ZXN0ZWRQcml2aWxlZ2VzPg0KICAgIDwvc2VjdXJpdHk+DQogIDwvdHJ1c3RJbmZvPg0KPC9hc3NlbWJseT4NCgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAwAAAMAAAAsDMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";

        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args != null && args.Length > 0) {
                string arg = args[0].ToLowerInvariant().Trim('-', '/');
                if (arg == "silent" || arg == "s" || arg == "install" || arg == "i") {
                    InstallLogic(true);
                    return;
                }
                if (arg == "uninstall" || arg == "u") {
                    UninstallLogic(true);
                    return;
                }
            }

            Application.Run(new InstallerForm());
        }

        public static bool IsInstalled() {
            string targetExe = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Despierta", "despierta.exe");
            return File.Exists(targetExe);
        }

        public static string GetTargetDir() {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Despierta");
        }

        public static string GetTargetExe() {
            return Path.Combine(GetTargetDir(), "despierta.exe");
        }

        public static void InstallLogic(bool silent) {
            try {
                // 1. Kill running instances
                Process[] procs = Process.GetProcessesByName("despierta");
                foreach (var p in procs) {
                    try { p.Kill(); } catch { }
                }
                Thread.Sleep(300);

                // 2. Write executable
                string dir = GetTargetDir();
                if (!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                }
                string exePath = GetTargetExe();
                byte[] bytes = Convert.FromBase64String(APP_PAYLOAD_B64);
                File.WriteAllBytes(exePath, bytes);

                // 3. Register Win + R (App Paths)
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\App Paths\despierta.exe")) {
                    if (key != null) key.SetValue("", exePath);
                }
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\App Paths\despierta")) {
                    if (key != null) key.SetValue("", exePath);
                }

                // 4. Register in User PATH
                string userPath = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User) ?? "";
                if (!userPath.ToLowerInvariant().Contains(dir.ToLowerInvariant())) {
                    string newPath = string.IsNullOrEmpty(userPath) ? dir : (userPath.TrimEnd(';') + ";" + dir);
                    Environment.SetEnvironmentVariable("Path", newPath, EnvironmentVariableTarget.User);
                }

                // 5. Register in Windows Uninstall (Apps & Features)
                using (RegistryKey uninst = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\Despierta")) {
                    if (uninst != null) {
                        uninst.SetValue("DisplayName", "Modo Despierta (Windows Keep-Awake)");
                        uninst.SetValue("DisplayVersion", "1.0.0");
                        uninst.SetValue("Publisher", "worm91");
                        uninst.SetValue("DisplayIcon", exePath);
                        uninst.SetValue("InstallLocation", dir);
                        uninst.SetValue("UninstallString", "\"" + Application.ExecutablePath + "\" /uninstall");
                        uninst.SetValue("NoModify", 1, RegistryValueKind.DWord);
                        uninst.SetValue("NoRepair", 1, RegistryValueKind.DWord);
                    }
                }

                if (!silent) {
                    DialogResult res = MessageBox.Show(
                        "¡Modo Despierta se instaló con éxito!\n\n" +
                        "Ya puedes presionar Win + R y escribir 'despierta' en cualquier momento.\n\n" +
                        "¿Deseas activar Modo Despierta ahora?",
                        "Instalación Completada",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information
                    );
                    if (res == DialogResult.Yes) {
                        Process.Start(exePath);
                    }
                }
            } catch (Exception ex) {
                if (!silent) {
                    MessageBox.Show("Error durante la instalación:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static void UninstallLogic(bool silent) {
            try {
                // 1. Kill running instances
                Process[] procs = Process.GetProcessesByName("despierta");
                foreach (var p in procs) {
                    try { p.Kill(); } catch { }
                }
                Thread.Sleep(300);

                // 2. Remove App Paths
                Registry.CurrentUser.DeleteSubKeyTree(@"Software\Microsoft\Windows\CurrentVersion\App Paths\despierta.exe", false);
                Registry.CurrentUser.DeleteSubKeyTree(@"Software\Microsoft\Windows\CurrentVersion\App Paths\despierta", false);

                // 3. Clean User PATH
                string dir = GetTargetDir();
                string userPath = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User) ?? "";
                if (userPath.ToLowerInvariant().Contains(dir.ToLowerInvariant())) {
                    string[] parts = userPath.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    System.Collections.Generic.List<string> clean = new System.Collections.Generic.List<string>();
                    foreach (var p in parts) {
                        if (!string.Equals(p.Trim(), dir, StringComparison.OrdinalIgnoreCase)) {
                            clean.Add(p);
                        }
                    }
                    Environment.SetEnvironmentVariable("Path", string.Join(";", clean.ToArray()), EnvironmentVariableTarget.User);
                }

                // 4. Remove Uninstall key
                Registry.CurrentUser.DeleteSubKeyTree(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\Despierta", false);

                // 5. Delete folder
                if (Directory.Exists(dir)) {
                    Directory.Delete(dir, true);
                }

                if (!silent) {
                    MessageBox.Show(
                        "Modo Despierta ha sido desinstalado completamente de tu sistema.",
                        "Desinstalación Completada",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            } catch (Exception ex) {
                if (!silent) {
                    MessageBox.Show("Error durante la desinstalación:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    public class InstallerForm : Form {
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblStatus;
        private Button btnInstall;
        private Button btnUninstall;
        private Button btnClose;

        public InstallerForm() {
            InitializeComponent();
            RefreshStatus();
        }

        private void InitializeComponent() {
            this.Text = "Instalador - Modo Despierta";
            this.Size = new Size(480, 310);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Icon
            this.Icon = CreateCoffeeIcon();

            Panel header = new Panel();
            header.Dock = DockStyle.Top;
            header.Height = 70;
            header.BackColor = Color.FromArgb(33, 37, 41);

            lblTitle = new Label();
            lblTitle.Text = "☕ Modo Despierta (Windows Keep-Awake)";
            lblTitle.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(16, 12);
            lblTitle.AutoSize = true;

            lblSubtitle = new Label();
            lblSubtitle.Text = "Mantiene tu equipo y pantalla despiertos cuando lo necesitas.";
            lblSubtitle.Font = new Font("Segoe UI", 9f);
            lblSubtitle.ForeColor = Color.FromArgb(200, 205, 210);
            lblSubtitle.Location = new Point(18, 38);
            lblSubtitle.AutoSize = true;

            header.Controls.Add(lblTitle);
            header.Controls.Add(lblSubtitle);
            this.Controls.Add(header);

            lblStatus = new Label();
            lblStatus.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            lblStatus.Location = new Point(20, 90);
            lblStatus.Size = new Size(420, 50);
            this.Controls.Add(lblStatus);

            btnInstall = new Button();
            btnInstall.Text = "📥 Instalar / Actualizar";
            btnInstall.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnInstall.Location = new Point(20, 160);
            btnInstall.Size = new Size(180, 42);
            btnInstall.BackColor = Color.FromArgb(13, 110, 253);
            btnInstall.ForeColor = Color.White;
            btnInstall.FlatStyle = FlatStyle.Flat;
            btnInstall.FlatAppearance.BorderSize = 0;
            btnInstall.Cursor = Cursors.Hand;
            btnInstall.Click += (s, e) => {
                Program.InstallLogic(false);
                RefreshStatus();
            };
            this.Controls.Add(btnInstall);

            btnUninstall = new Button();
            btnUninstall.Text = "🗑 Desinstalar";
            btnUninstall.Font = new Font("Segoe UI", 9.5f);
            btnUninstall.Location = new Point(210, 160);
            btnUninstall.Size = new Size(120, 42);
            btnUninstall.BackColor = Color.FromArgb(220, 53, 69);
            btnUninstall.ForeColor = Color.White;
            btnUninstall.FlatStyle = FlatStyle.Flat;
            btnUninstall.FlatAppearance.BorderSize = 0;
            btnUninstall.Cursor = Cursors.Hand;
            btnUninstall.Click += (s, e) => {
                Program.UninstallLogic(false);
                RefreshStatus();
            };
            this.Controls.Add(btnUninstall);

            btnClose = new Button();
            btnClose.Text = "Cerrar";
            btnClose.Font = new Font("Segoe UI", 9.5f);
            btnClose.Location = new Point(340, 160);
            btnClose.Size = new Size(100, 42);
            btnClose.BackColor = Color.FromArgb(108, 117, 125);
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Cursor = Cursors.Hand;
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);

            Label lblHint = new Label();
            lblHint.Text = "Instalación por usuario: No requiere permisos de administrador.\nComando directo: Win + R -> escribe 'despierta'.";
            lblHint.Font = new Font("Segoe UI", 8f);
            lblHint.ForeColor = Color.FromArgb(108, 117, 125);
            lblHint.Location = new Point(20, 220);
            lblHint.Size = new Size(420, 35);
            this.Controls.Add(lblHint);
        }

        private void RefreshStatus() {
            if (Program.IsInstalled()) {
                lblStatus.Text = "✔ Modo Despierta está actualmente INSTALADO en este equipo.\nPuedes presionar [Win + R] y escribir 'despierta'.";
                lblStatus.ForeColor = Color.FromArgb(25, 135, 84);
                btnInstall.Text = "🔄 Reinstalar / Actualizar";
                btnUninstall.Enabled = true;
            } else {
                lblStatus.Text = "ℹ Modo Despierta NO está instalado en este equipo.\nHaz clic en el botón inferior para instalarlo.";
                lblStatus.ForeColor = Color.FromArgb(73, 80, 87);
                btnInstall.Text = "📥 Instalar";
                btnUninstall.Enabled = false;
            }
        }

        private Icon CreateCoffeeIcon() {
            using (Bitmap bmp = new Bitmap(32, 32)) {
                using (Graphics g = Graphics.FromImage(bmp)) {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.Clear(Color.Transparent);
                    using (Brush bg = new SolidBrush(Color.FromArgb(245, 130, 32))) {
                        g.FillEllipse(bg, 1, 1, 30, 30);
                    }
                    using (Pen p = new Pen(Color.White, 2f)) {
                        g.DrawLine(p, 10, 12, 10, 22);
                        g.DrawLine(p, 10, 22, 22, 22);
                        g.DrawLine(p, 22, 22, 22, 12);
                        g.DrawLine(p, 8, 12, 24, 12);
                        g.DrawArc(p, 20, 14, 6, 6, -90, 180);
                    }
                }
                return Icon.FromHandle(bmp.GetHicon());
            }
        }
    }
}
