using BarberiaTurnos.BLL;
using BarberiaTurnos.DAL;
using BarberiaTurnos.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BarberiaTurnos.UI
{
    public class LoginForm : Form, ITranslatableForm
    {
        private readonly AuthBLL _authBll = new();

        private readonly Label lblTitle = new();
        private readonly Label lblUser = new();
        private readonly Label lblPass = new();
        private readonly Label lblLang = new();
        private readonly Label lblStatus = new();
        private readonly TextBox txtUser = new();
        private readonly TextBox txtPass = new();
        private readonly ComboBox cboLang = new();
        private readonly Button btnLogin = new();

        public LoginForm()
        {
            InitializeComponent();
            ApplyLanguage();
            Load += LoginForm_Load;
        }

        private void InitializeComponent()
        {
            Text = LanguageService.T("login_title");
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Width = 460;
            Height = 320;

            lblTitle.Font = new Font(Font.FontFamily, 16, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, 20);

            lblUser.AutoSize = true;
            lblUser.Location = new Point(30, 80);

            txtUser.Location = new Point(150, 76);
            txtUser.Width = 240;

            lblPass.AutoSize = true;
            lblPass.Location = new Point(30, 120);

            txtPass.Location = new Point(150, 116);
            txtPass.Width = 240;
            txtPass.UseSystemPasswordChar = true;

            lblLang.AutoSize = true;
            lblLang.Location = new Point(30, 160);

            cboLang.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLang.Location = new Point(150, 156);
            cboLang.Width = 120;
            cboLang.Items.AddRange(["ES", "EN"]);
            cboLang.SelectedIndexChanged += (_, __) => LanguageService.SetLanguage(cboLang.SelectedItem?.ToString() ?? "ES");

            btnLogin.Text = LanguageService.T("login_button");
            btnLogin.Location = new Point(150, 205);
            btnLogin.Width = 120;
            btnLogin.Click += BtnLogin_Click;
            AcceptButton = btnLogin;

            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(20, 255);

            Controls.AddRange([lblTitle, lblUser, txtUser, lblPass, txtPass, lblLang, cboLang, btnLogin, lblStatus]);
        }

        private void LoginForm_Load(object? sender, EventArgs e)
        {
            cboLang.SelectedItem = LanguageService.CurrentLanguage;
            if (cboLang.SelectedIndex < 0) cboLang.SelectedIndex = 0;
            lblStatus.Text = Db.TestConnection() ? LanguageService.T("login_db_ok") : LanguageService.T("login_db_fail");
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            var username = txtUser.Text.Trim();
            var password = txtPass.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(LanguageService.T("login_failed"), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LanguageService.SetLanguage(cboLang.SelectedItem?.ToString() ?? "ES");


            var user = _authBll.Login(username, password);
            /*if (user == null)
            {
                MessageBox.Show(LanguageService.T("login_failed"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPass.Clear();
                txtPass.Focus();
                return;
            }*/

            AppSession.Instance.SignIn(user);

            var main = new MainForm();
            main.FormClosed += (_, __) =>
            {
                if (main.WasLogoutRequested)
                {
                    ResetFields();
                    Show();
                }
                else
                {
                    Close();
                }
            };

            Hide();
            main.Show();
        }

        private void ResetFields()
        {
            txtUser.Clear();
            txtPass.Clear();
            cboLang.SelectedItem = LanguageService.CurrentLanguage;
        }

        public void ApplyLanguage()
        {
            Text = LanguageService.T("login_title");
            lblTitle.Text = LanguageService.T("login_title");
            lblUser.Text = LanguageService.T("login_user");
            lblPass.Text = LanguageService.T("login_password");
            lblLang.Text = LanguageService.T("login_language");
            btnLogin.Text = LanguageService.T("login_button");
        }
    }
}
