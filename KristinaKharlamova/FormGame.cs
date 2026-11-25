using System;
using System.Drawing;
using System.Windows.Forms;

namespace KristinaKharlamova
{
    public partial class FormGame : Form
    {
        private string playerName;
        private string difficulty;
        private int score = 0;      // Счетчик набранных очков
        private int lives = 3;      // Количество жизней
        private int timeLeft = 30;  // Таймер игры (30 секунд)
        private Random rnd = new Random(); // Генератор случайных чисел для мишени
        private int targetX = 0;    // Координата мишени по X
        private int targetY = 0;    // Координата мишени по Y
        private int targetSize = 40; // Размер мишени
        private int speedX = 5;     // Скорость движения по X
        private int speedY = 5;     // Скорость движения по Y
        private Timer secondTimer;  // Второй таймер для секунд
        public FormGame(string name, string diff)
        {
            InitializeComponent();
            playerName = name;
            difficulty = diff;
            this.Text = $"Игра: {playerName} ({difficulty})";
            this.DoubleBuffered = true; // Убираю мерцание
            // Привязываю события вручную
            this.MouseDown += new MouseEventHandler(FormGame_MouseDown);
            this.Paint += new PaintEventHandler(FormGame_Paint);
            // Настраиваю таймер движения мишени
            gameTimer.Tick += new EventHandler(gameTimer_Tick);
            gameTimer.Interval = 140; // Скорость обновления
            gameTimer.Start();
            // Настраиваю таймер для отсчета секунд
            secondTimer = new Timer();
            secondTimer.Interval = 1000;
            secondTimer.Tick += new EventHandler(secondTimer_Tick);
            secondTimer.Start();
        }
        private void FormGame_Load(object sender, EventArgs e)
        {
            SpawnTarget(); // Создаю первую мишень
        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            targetX += speedX; // Двигаю мишень по X
            targetY += speedY; // Двигаю мишень по Y
            // Отскок от левой и правой стены
            if (targetX <= 0 || targetX + targetSize >= this.ClientSize.Width)
                speedX = -speedX;
            // Отскок от верхней и нижней стены
            if (targetY <= 60 || targetY + targetSize >= this.ClientSize.Height)
                speedY = -speedY;
            this.Invalidate(); // Перерисовываю форму
        }
        private void secondTimer_Tick(object sender, EventArgs e)
        {
            timeLeft--;        // Уменьшаю время
            this.Invalidate();
            // Если время вышло или жизни кончились
            if (timeLeft <= 0 || lives <= 0)
            {
                GameOver();    // Завершаю игру
            }
        }
        private void FormGame_Paint(object sender, PaintEventArgs e)
        {
        }
        private void SpawnTarget()
        {
            // Ставлю мишень в случайное место
            targetX = rnd.Next(10, this.ClientSize.Width - targetSize - 10);
            targetY = rnd.Next(60, this.ClientSize.Height - targetSize - 10);
            this.Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            // Рисую красный круг
            g.FillEllipse(Brushes.Red, targetX, targetY, targetSize, targetSize);
            g.DrawEllipse(Pens.DarkRed, targetX, targetY, targetSize, targetSize);
            // Вывожу текст со статистикой
            using (Font myFont = new Font("Arial", 14, FontStyle.Bold))
            {
                g.DrawString($"Очки: {score}", myFont, Brushes.Black, 10, 10);
                g.DrawString($"Жизни: {lives}", myFont, Brushes.Black, 10, 40);
                g.DrawString($"Время: {timeLeft}", myFont, Brushes.Black, 10, 70);
            }
        }
        private void FormGame_MouseDown(object sender, MouseEventArgs e)
        {
            Rectangle targetRect = new Rectangle(targetX, targetY, targetSize, targetSize);

            if (targetRect.Contains(e.Location))
            {
                score += 10;        // Добавляю 10 очков за попадание
                SpawnTarget();      // Перекидываю мишень на новое место
            }
            else
            {
                lives--;            // Отнимаю одну жизнь за промах
                this.Invalidate();  // Перерисовываю форму (обновляю жизни)
                if (lives <= 0) GameOver(); // Если жизней нет - конец игры
            }
        }
        private void GameOver()
        {
            gameTimer.Stop();       // Останавливаю таймер движения
            secondTimer.Stop();     // Останавливаю таймер времени
            FormResult resultForm = new FormResult(playerName, score);
            this.Hide();
            if (resultForm.ShowDialog() == DialogResult.Retry)
            {
                // Если нажали "Играть заново":
                score = 0;          // Обнуляю очки
                lives = 3;          // Возвращаю 3 жизни
                timeLeft = 30;      // Ставлю таймер на 30 секунд
                speedX = 5;         // Возвращаю скорость по X
                speedY = 5;         // Возвращаю скорость по Y

                SpawnTarget();      // Создаю новую мишень
                gameTimer.Start();  // Запускаю движение
                secondTimer.Start();// Запускаю время
                this.Show();        // Показываю игру
                this.Invalidate();  // Перерисовываю экран
            }
            else
            {
                this.Close();       // Если нажали "В меню" - закрываю игру
            }
        }
        private void FormGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            gameTimer.Stop();       // Останавливаю таймер движения
            gameTimer.Dispose();    // Освобождаю ресурсы
            if (secondTimer != null) // Проверяю, что второй таймер существует
            {
                secondTimer.Stop();    // Останавливаю таймер времени
                secondTimer.Dispose(); // Освобождаю его ресурсы
            }
            Form menu = Application.OpenForms["FormMenu"];
            if (menu != null) menu.Show(); // Возвращаю меню
        }
    }
}