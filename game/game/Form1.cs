using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace game
{
    public partial class Form1 : Form
    {
        Point[] p;
        Point[] p2;
        Point apple;
        int len1;
        int len2;
        int direction;//1-лево,2-право,3-вверх,4-вниз
        int direction2;
        bool gameOver = false;
        int score1 = 0,score2=0;

        public Form1()
        {
            InitializeComponent();
            len1 = 5;
            len2 = 5;
            p = new Point[200];
            p2 = new Point[200];
            direction = 3;
            direction2 = 3;
            for (int i=0;i<5;i++)
            {
                p[i].X = 100;
                p[i].Y = 100+i*10;
                p2[i].X = 150; 
                p2[i].Y = 100 + i * 10;
            }
            apple.X = 10;
            apple.Y = 10;
        }
        private void DrawSnake(Graphics g, Point[] snake, int length, Color color)
        {
            SolidBrush brush = new SolidBrush(color);
            for (int i = 0; i < length; i++)
            {
                g.FillRectangle(brush, snake[i].X, snake[i].Y, 10, 10);
            }
            brush.Dispose();
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (gameOver)
            {
                Font font = new Font("Arial", 20, FontStyle.Bold);
                SolidBrush gameOverBrush = new SolidBrush(Color.Red);
                e.Graphics.DrawString($"Игра окончена! Счет: {score1} Счет 2:{score2}", font, gameOverBrush, panel1.Width / 2 - 150, panel1.Height / 2 - 20);
                return; 
            }
            MoveSnake(p, ref len1, direction);
            MoveSnake(p2, ref len2, direction2);

            DrawSnake(e.Graphics, p, len1, Color.Fuchsia);
            DrawSnake(e.Graphics, p2, len2, Color.Chartreuse);

            SolidBrush b1 = new SolidBrush(Color.Teal);
            e.Graphics.FillEllipse(b1, apple.X, apple.Y, 10, 10);

            CheckAppleCollision(p, ref len1, ref score1);
            CheckAppleCollision(p2, ref len2, ref score2);
            CheckCollision(p, len1);
            CheckCollision(p2, len2);
            CheckBorders(p);
            CheckBorders(p2);

            Font scoreFont = new Font("Arial", 12, FontStyle.Regular);
            SolidBrush scoreBrush = new SolidBrush(Color.Black);
            e.Graphics.DrawString($"Счет 1: {score1}, Счет 2:{score2}", scoreFont, scoreBrush, 10, 10);
        }
        private void MoveSnake(Point[] snake, ref int length, int dir)
        {
            for (int i = length - 1; i >= 0; i--)
            {
                if (i > 0)
                {
                    snake[i] = snake[i - 1];
                }
            }
            switch (dir)
            {
                case 1:
                    snake[0].X -= 10;
                    break;
                case 2:
                    snake[0].X += 10;
                    break;
                case 3:
                    snake[0].Y -= 10;
                    break;
                case 4:
                    snake[0].Y += 10;
                    break;
            }
        }

        private void CheckAppleCollision(Point[] snake, ref int length, ref int score)
        {
            if (snake[0].X == apple.X && snake[0].Y == apple.Y)
            {
                length++;
                score++;
                GenerateNewApplePosition();
            }
        }

        private void CheckCollision(Point[] snake, int length)
        {
            for (int i = 1; i < length; i++)
            {
               
                if (snake[0].X == snake[i].X && snake[0].Y == snake[i].Y)
                {
                    GameOver();
                    return; 
                }
            }
        }
        private void CheckBorders(Point[] snake)
        {
            if (snake[0].X < 0 || snake[0].X > panel1.Width - 10 || snake[0].Y < 0 || snake[0].Y > panel1.Height - 10)
            {
                GameOver();
            }
        }

        private void GenerateNewApplePosition()
        {
            Random R = new Random();
            bool validPosition = false;
            while (!validPosition)
            {
                apple.X = R.Next(0, panel1.Width / 10) * 10;
                apple.Y = R.Next(0, panel1.Height / 10) * 10;

                validPosition = true;
                for (int i = 0; i < len1; i++)
                {
                    if (apple.X == p[i].X && apple.Y == p[i].Y)
                    {
                        validPosition = false;
                        break;
                    }
                }
                for (int i = 1; i < len1; i++)
                {
                    if (p[0].Y == p[i].Y && p[0].X == p[i].X)
                    {
                        len1 = i;
                    }
                }
                for (int i = 0; i < len2; i++)
                {
                    if (apple.X == p2[i].X && apple.Y == p2[i].Y)
                    {
                        validPosition = false;
                        break;
                    }
                }
                for (int i = 1; i < len2; i++)
                {
                    if (p[0].Y == p[i].Y && p[0].X == p[i].X)
                    {
                        len2 = i;
                    }
                }
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!gameOver)
            {
                panel1.Invalidate();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                direction = 1;
            if (e.KeyCode == Keys.Right)
                direction = 2;
            if (e.KeyCode == Keys.Up)
                direction = 3;
            if (e.KeyCode == Keys.Down)
                direction = 4;

            if (e.KeyCode == Keys.A)
                direction2 = 1;
            if (e.KeyCode == Keys.D)
                direction2 = 2;
            if (e.KeyCode == Keys.W)
                direction2 = 3;
            if (e.KeyCode == Keys.S)
                direction2 = 4;
        }
        private void GameOver()
        {
            gameOver = true;
            timer1.Stop();
            MessageBox.Show($"Игра окончена! Счет: {score1}, Счет 2:{score2}");
        }
    }
}
