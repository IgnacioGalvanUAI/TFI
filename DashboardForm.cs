using BarberiaTurnos.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BarberiaTurnos.UI
{
    public class DashboardForm : Form, ITranslatableForm
    {
        private readonly Label lblUser = new();
        private readonly Label lblHint = new();

        public DashboardForm()
        {
            InitializeComponent();
            ApplyLanguage();
        }

        private void InitializeComponent()
        {
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Width = 640;
            Height = 280;
            StartPosition = FormStartPosition.CenterParent;

            Controls.AddRange([lblUser, lblHint]);
        }

        public void ApplyLanguage()
        {
            Text = LanguageService.T("dashboard_title");
            lblHint.Text = LanguageService.T("dashboard_hint");
        }
    }
}