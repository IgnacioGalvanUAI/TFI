using BarberiaTurnos.BE;
using BarberiaTurnos.BLL;
using BarberiaTurnos.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BarberiaTurnos.UI
{
    public class UsersForm : Form, ITranslatableForm
    {
        private readonly UsuarioBLL _usuarioBll = new();
        private readonly DataGridView grid = new();
        private readonly TextBox txtId = new();
        private readonly TextBox txtUser = new();
        private readonly TextBox txtPass = new();
        private readonly TextBox txtName = new();
        private readonly TextBox txtEmail = new();
        private readonly ComboBox cboRole = new();
        private readonly ComboBox cboLanguage = new();
        private readonly CheckBox chkActive = new();
        private readonly Button btnNew = new();
        private readonly Button btnSave = new();
        private readonly Button btnDisable = new();
        private readonly Button btnRefresh = new();
        private readonly Button btnClear = new();
        private readonly Label lblUser = new();
        private readonly Label lblPass = new();
        private readonly Label lblName = new();
        private readonly Label lblEmail = new();
        private readonly Label lblRole = new();
        private readonly Label lblLanguage = new();
        private readonly Label lblActive = new();

        private List<RolBE> _roles = [];

        public UsersForm()
        {
            InitializeComponent();
            ApplyLanguage();
            Load += UsersForm_Load;
        }

        private void InitializeComponent()
        {
            Text = LanguageService.T("users_title");
            Width = 1100;
            Height = 700;
            StartPosition = FormStartPosition.CenterParent;

            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 620
            };

            grid.Dock = DockStyle.Fill;
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.AutoGenerateColumns = false;
            grid.AllowUserToAddRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionChanged += (_, __) => LoadSelected();

            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "IdUsuario", HeaderText = "Id", Visible = false });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Username", HeaderText = "Usuario" });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreCompleto", HeaderText = "Nombre" });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email" });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "RolNombre", HeaderText = "Rol" });
            grid.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = "Activo", HeaderText = "Activo" });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Idioma", HeaderText = "Idioma" });

            split.Panel1.Controls.Add(grid);

            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12) };

            int labelX = 20, inputX = 170, y = 20, rowH = 34, inputW = 280;

            ConfigureLabel(lblUser, labelX, y);
            ConfigureTextBox(txtUser, inputX, y, inputW);
            y += rowH;

            ConfigureLabel(lblPass, labelX, y);
            ConfigureTextBox(txtPass, inputX, y, inputW);
            txtPass.UseSystemPasswordChar = true;
            y += rowH;

            ConfigureLabel(lblName, labelX, y);
            ConfigureTextBox(txtName, inputX, y, inputW);
            y += rowH;

            ConfigureLabel(lblEmail, labelX, y);
            ConfigureTextBox(txtEmail, inputX, y, inputW);
            y += rowH;

            ConfigureLabel(lblRole, labelX, y);
            ConfigureCombo(cboRole, inputX, y, inputW);
            y += rowH;

            ConfigureLabel(lblLanguage, labelX, y);
            ConfigureCombo(cboLanguage, inputX, y, 120);
            cboLanguage.Items.AddRange(["ES", "EN"]);
            y += rowH;

            ConfigureLabel(lblActive, labelX, y);
            chkActive.Location = new Point(inputX, y + 4);
            chkActive.Checked = true;
            y += rowH;

            txtId.Visible = false;

            btnNew.Location = new Point(20, y + 20);
            btnNew.Width = 100;
            btnNew.Click += (_, __) => ClearForm();

            btnSave.Location = new Point(130, y + 20);
            btnSave.Width = 100;
            btnSave.Click += (_, __) => SaveUser();

            btnDisable.Location = new Point(240, y + 20);
            btnDisable.Width = 100;
            btnDisable.Click += (_, __) => DisableUser();

            btnRefresh.Location = new Point(350, y + 20);
            btnRefresh.Width = 100;
            btnRefresh.Click += (_, __) => RefreshData();

            btnClear.Location = new Point(460, y + 20);
            btnClear.Width = 100;
            btnClear.Click += (_, __) => ClearForm();

            panel.Controls.AddRange([
                txtId, lblUser, txtUser, lblPass, txtPass, lblName, txtName, lblEmail, txtEmail,
                lblRole, cboRole, lblLanguage, cboLanguage, lblActive, chkActive,
                btnNew, btnSave, btnDisable, btnRefresh, btnClear
            ]);

            split.Panel2.Controls.Add(panel);

            Controls.Add(split);
        }

        private static void ConfigureLabel(Label lbl, int x, int y)
        {
            lbl.Location = new Point(x, y + 6);
            lbl.AutoSize = true;
        }

        private static void ConfigureTextBox(TextBox txt, int x, int y, int w)
        {
            txt.Location = new Point(x, y);
            txt.Width = w;
        }

        private static void ConfigureCombo(ComboBox cbo, int x, int y, int w)
        {
            cbo.Location = new Point(x, y);
            cbo.Width = w;
            cbo.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void UsersForm_Load(object? sender, EventArgs e)
        {
            _roles = _usuarioBll.ObtenerRoles();
            cboRole.DisplayMember = "Nombre";
            cboRole.ValueMember = "IdRol";
            cboRole.DataSource = _roles.ToList();
            cboLanguage.SelectedIndex = 0;
            RefreshData();
        }

        private void RefreshData()
        {
            grid.DataSource = _usuarioBll.ObtenerUsuarios();
            ClearForm();
        }

        private void LoadSelected()
        {
            if (grid.CurrentRow?.DataBoundItem is not UsuarioBE user)
                return;

            txtId.Text = user.IdUsuario.ToString();
            txtUser.Text = user.Username;
            txtPass.Clear();
            txtName.Text = user.NombreCompleto;
            txtEmail.Text = user.Email;
            cboRole.SelectedValue = user.IdRol;
            cboLanguage.SelectedItem = user.Idioma;
            chkActive.Checked = user.Activo;
        }

        private void ClearForm()
        {
            txtId.Text = "0";
            txtUser.Clear();
            txtPass.Clear();
            txtName.Clear();
            txtEmail.Clear();
            chkActive.Checked = true;
            cboLanguage.SelectedIndex = 0;
            if (cboRole.Items.Count > 0) cboRole.SelectedIndex = 0;
            txtUser.Focus();
        }

        private void SaveUser()
        {
            try
            {
                var user = new UsuarioBE
                {
                    IdUsuario = int.TryParse(txtId.Text, out var id) ? id : 0,
                    Username = txtUser.Text,
                    NombreCompleto = txtName.Text,
                    Email = txtEmail.Text,
                    IdRol = cboRole.SelectedValue is int rolId ? rolId : 0,
                    Activo = chkActive.Checked,
                    Idioma = cboLanguage.SelectedItem?.ToString() ?? "ES"
                };

                _usuarioBll.Guardar(user, txtPass.Text);
                MessageBox.Show(LanguageService.T("users_created"), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DisableUser()
        {
            if (!int.TryParse(txtId.Text, out var id) || id <= 0)
                return;

            var result = MessageBox.Show(LanguageService.T("users_confirm_delete"), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            try
            {
                _usuarioBll.Desactivar(id);
                MessageBox.Show(LanguageService.T("users_deleted"), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void ApplyLanguage()
        {
            Text = LanguageService.T("users_title");
            lblUser.Text = LanguageService.T("users_username");
            lblPass.Text = LanguageService.T("users_password");
            lblName.Text = LanguageService.T("users_name");
            lblEmail.Text = LanguageService.T("users_email");
            lblRole.Text = LanguageService.T("users_role");
            lblLanguage.Text = LanguageService.T("users_language");
            lblActive.Text = LanguageService.T("users_active");
            btnNew.Text = LanguageService.T("users_new");
            btnSave.Text = LanguageService.T("users_save");
            btnDisable.Text = LanguageService.T("users_disable");
            btnRefresh.Text = LanguageService.T("users_refresh");
            btnClear.Text = LanguageService.T("users_clear");
        }
    }
}
