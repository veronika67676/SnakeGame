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
        Point apple;
        int len;
        int direction;//1-лево,2-право,3-вверх,4-вниз
        bool gameOver = false;
        int score = 0;
        public Form1()
        {
            InitializeComponent();
            len = 5;
            p = new Point[200];
            direction = 3;
            for (int i=0;i<5;i++)
            {
                p[i].X = 100;
                p[i].Y = 100+i*10;
            }
            apple.X = 10;
            apple.Y = 10;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (gameOver)
            {
                Font font = new Font("Arial", 20, FontStyle.Bold);
                SolidBrush gameOverBrush = new SolidBrush(Color.Red);
                e.Graphics.DrawString($"Игра окончена! Счет: {score}", font, gameOverBrush, panel1.Width / 2 - 150, panel1.Height / 2 - 20);
                return; 
            }
            for (int i=198;i>=0;i--)
            {
                p[i+1].X = p[i].X;
                p[i+1].Y = p[i].Y;
            }
            if (direction == 1)
            {
                p[0].X = p[1].X - 10;
                p[0].Y = p[1].Y;
            }
            if (direction == 2)
            {
                p[0].X = p[1].X + 10;
                p[0].Y = p[1].Y;
            }
            if (direction == 3)
            {
                p[0].X = p[1].X;
                p[0].Y = p[1].Y-10;
            }
            if (direction == 4)
            {
                p[0].X = p[1].X;
                p[0].Y = p[1].Y + 10;
            }
            SolidBrush b=new SolidBrush(Color.Purple);
            for(int i=0;i<len;i++)
            {
                e.Graphics.FillEllipse(b, p[i].X, p[i].Y,10,10);
            }
            SolidBrush b1 = new SolidBrush(Color.Turquoise);
                e.Graphics.FillEllipse(b1, apple.X, apple.Y, 10, 10);
            if (p[0].X==apple.X && p[0].Y == apple.Y)
            {
                len++;
                Random R;
                R=new Random();
                apple.X=R.Next(0,50)*10;
                apple.Y=R.Next(0,50)*10;
                score += 1;
            }
            for (int i = 1; i < len; i++)
            {
                if (p[0].X == p[i].X && p[0].Y == p[i].Y)
                {
                    GameOver();
                    return;
                }
            }
            if (p[0].X < 0 || p[0].X > panel1.Width - 10 || p[0].Y < 0 || p[0].Y > panel1.Height - 10)
            {
                GameOver();
                return;
            }
            Font scoreFont = new Font("Arial", 12, FontStyle.Regular);
            SolidBrush scoreBrush = new SolidBrush(Color.White);
            e.Graphics.DrawString($"Счет: {score}", scoreFont, scoreBrush, 10, 10);

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
                for (int i = 0; i < len; i++)
                {
                    if (apple.X == p[i].X && apple.Y == p[i].Y)
                    {
                        validPosition = false;
                        break;
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
        }
        private void GameOver()
        {
            gameOver = true;
            timer1.Stop();
            MessageBox.Show($"Игра окончена! Счет: {score}");
        }
    }
}
