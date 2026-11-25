using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KristinaKharlamova
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
        }

        private void FormMenu_Load(object sender, EventArgs e)
        {

        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
                // 1. Проверяю, ввел ли пользователь имя
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Пожалуйста, введите имя игрока!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Останавливаем выполнение, если имя пустое
                }

                // 2. Берем сложность из выпадающего списка
                // Если ничего не выбрано, ставим "Легко" по умолчанию
                string selectedDifficulty = "Легко";
                if (cmbDifficulty.SelectedItem != null)
                {
                    selectedDifficulty = cmbDifficulty.SelectedItem.ToString();
                }

                // 3. Создаем игровую форму и передаем ей имя и сложность
                FormGame gameForm = new FormGame(txtName.Text.Trim(), selectedDifficulty);

                // 4. Скрываю меню и показываю игру
                this.Hide();
                gameForm.Show();
            }
        }
    }
