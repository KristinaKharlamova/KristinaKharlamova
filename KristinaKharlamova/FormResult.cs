using System;
using System.Windows.Forms;

namespace KristinaKharlamova
{
    public partial class FormResult : Form
    {
        public FormResult(string name, int score)
        {
            InitializeComponent();
            lblResult.Text = $"Игрок: {name}\nНабрано очков: {score}";

            // ЖЕСТКАЯ ПРИВЯЗКА КНОПОК В КОДЕ тк баг шел
            btnMenu.Click += new EventHandler(btnMenu_Click);
            btnRestart.Click += new EventHandler(btnRestart_Click);
        }
        // Кнопка "В меню"
        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK; // Говорим игре: "В меню"
            this.Close();
        }
        // Кнопка "Играть заново"
        private void btnRestart_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry; // Говорим игре: "Заново"
            this.Close();
        }
    }
}