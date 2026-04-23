using BarberiaTurnos.BLL;
using BarberiaTurnos.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BarberiaTurnos.UI
{
    public class MainForm : Form, ITranslatableForm
    {
        private readonly MenuStrip menu = new();
        private readonly ToolStripMenuItem mnuSystem = new();
        private readonly ToolStripMenuItem mnuUsers = new();
        private readonly ToolStripMenuItem mnuLogout = new();
        private readonly ToolStripMenuItem mnuExit = new();
        private readonly ToolStripMenuItem mnuLanguage = new();
        private readonly ToolStripMenuItem mnuSpanish = new();
        private readonly ToolStripMenuItem mnuEnglish = new();
        private readonly StatusStrip status = new();
        private readonly ToolStripStatusLabel lblUser = new();
        private readonly ToolStripStatusLabel lblRole = new();
        private readonly ToolStripStatusLabel lblTime = new();
        private readonly Timer timer = new();

        public bool WasLogoutRequested { get; private set; }

        public MainForm()
        {
            InitializeComponent();
            ApplyLanguage();
            Load += MainForm_Load;
        }

        private void InitializeComponent()
        {
            IsMdiContainer = true;
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;
            KeyPreview = true;

            mnuUsers.Click += (_, __) => OpenChild<UsersForm>();
            mnuLogout.Click += (_, __) => Logout();
            mnuExit.Click += (_, __) => { WasLogoutRequested = false; Close(); };
            mnuSpanish.Click += (_, __) => { LanguageService.SetLanguage("ES"); RefreshLanguage(); };
            mnuEnglish.Click += (_, __) => { LanguageService.SetLanguage("EN"); RefreshLanguage(); };

            mnuLanguage.DropDownItems.AddRange([mnuSpanish, mnuEnglish]);
            mnuSystem.DropDownItems.AddRange([mnuUsers, mnuLogout, mnuExit]);
            menu.Items.AddRange([mnuSystem, mnuLanguage]);
            MainMenuStrip = menu;
            Controls.Add(menu);

            status.Items.AddRange([lblUser, lblRole, lblTime]);
            Controls.Add(status);

            timer.Interval = 1000;
            timer.Tick += (_, __) => lblTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            timer.Start();
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            UpdateStatusTexts();

            if (!string.Equals(AppSession.Instance.CurrentUser?.RolNombre, "Administrador", StringComparison.OrdinalIgnoreCase))
            {
                mnuUsers.Enabled = false;
            }

            OpenChild<DashboardForm>();
        }

        private void UpdateStatusTexts()
        {
            lblUser.Text = $"{LanguageService.T("login_user")}: {AppSession.Instance.CurrentUser?.Username}";
            lblRole.Text = $"{LanguageService.T("users_role")}: {AppSession.Instance.CurrentUser?.RolNombre}";
            lblTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void Logout()
        {
            WasLogoutRequested = true;
            new AuthBLL().Logout();
            Close();
        }

        private void RefreshLanguage()
        {
            ApplyLanguage();
            UpdateStatusTexts();
            foreach (Form child in MdiChildren)
            {
                if (child is ITranslatableForm translatable)
                    translatable.ApplyLanguage();
            }
        }

        public void ApplyLanguage()
        {
            Text = LanguageService.T("main_title");
            mnuSystem.Text = LanguageService.T("menu_system");
            mnuUsers.Text = LanguageService.T("menu_users");
            mnuLogout.Text = LanguageService.T("menu_logout");
            mnuExit.Text = LanguageService.T("menu_exit");
            mnuLanguage.Text = LanguageService.T("menu_language");
            mnuSpanish.Text = LanguageService.T("menu_spanish");
            mnuEnglish.Text = LanguageService.T("menu_english");
        }

        private void OpenChild<T>() where T : Form, new()
        {
            var existing = MdiChildren.OfType<T>().FirstOrDefault();
            if (existing != null)
            {
                existing.Activate();
                return;
            }

            var form = new T
            {
                MdiParent = this
            };
            form.Show();
        }
    }
}
