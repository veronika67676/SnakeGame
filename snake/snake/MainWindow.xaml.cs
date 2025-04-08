using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace snake
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point[] p;
        Point[] p2;
        Point apple;
        int len1;
        int len2;
        int direction; // 1-лево, 2-право, 3-вверх, 4-вниз
        int direction2;
        bool gameOver = false;
        int score1 = 0, score2 = 0;
        System.Windows.Threading.DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            len1 = 5;
            len2 = 5;
            p = new Point[200];
            p2 = new Point[200];
            direction = 3;
            direction2 = 3;

            for (int i = 0; i < 5; i++)
            {
                p[i] = new Point(100, 100 + i * 10);
                p2[i] = new Point(150, 100 + i * 10);
            }

            apple = new Point(10, 10);
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!gameOver)
            {
                MoveSnake(p, ref len1, direction);
                MoveSnake(p2, ref len2, direction2);
                CheckAppleCollision(p, ref len1, ref score1);
                CheckAppleCollision(p2, ref len2, ref score2);
                CheckCollision(p, len1);
                CheckCollision(p2, len2);
                CheckBorders(p);
                CheckBorders(p2);
                DrawGame();
            }
        }

        private void DrawGame()
        {
            Canvas.Children.Clear(); 

            if (gameOver)
            {
                var text = new TextBlock
                {
                    Text = $"Игра окончена! Счет: {score1} Счет 2: {score2}",
                    FontSize = 20,
                    Foreground = Brushes.Red,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Canvas.Children.Add(text);
                return;
            }

            DrawSnake(p, len1, Brushes.Fuchsia);
            DrawSnake(p2, len2, Brushes.Chartreuse);

            Ellipse appleEllipse = new Ellipse
            {
                Fill = Brushes.Teal,
                Width = 10,
                Height = 10
            };
            Canvas.SetLeft(appleEllipse, apple.X);
            Canvas.SetTop(appleEllipse, apple.Y);
            Canvas.Children.Add(appleEllipse);

            TextBlock scoreText = new TextBlock
            {
                Text = $"Счет 1: {score1}, Счет 2: {score2}",
                FontSize = 12,
                Foreground = Brushes.Black
            };
            Canvas.Children.Add(scoreText);
        }

        private void DrawSnake(Point[] snake, int length, Brush color)
        {
            for (int i = 0; i < length; i++)
            {
                Rectangle rect = new Rectangle
                {
                    Fill = color,
                    Width = 10,
                    Height = 10
                };
                Canvas.SetLeft(rect, snake[i].X);
                Canvas.SetTop(rect, snake[i].Y);
                Canvas.Children.Add(rect);
            }
        }

        private void MoveSnake(Point[] snake, ref int length, int dir)
        {
            for (int i = length - 1; i > 0; i--)
            {
                snake[i] = snake[i - 1];
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
            if (snake[0].X < 0 || snake[0].X > (int)Canvas.ActualWidth - 10 || snake[0].Y < 0 || snake[0].Y > (int)Canvas.ActualHeight - 10)
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
                apple.X = R.Next(0, (int)(Canvas.ActualWidth / 10)) * 10;
                apple.Y = R.Next(0, (int)(Canvas.ActualHeight / 10)) * 10;

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

        private void GameOver()
        {
            gameOver = true;
            timer.Stop();
            MessageBox.Show($"Игра окончена! Счет: {score1}, Счет 2: {score2}");
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Left)
                direction = 1;
            if (e.Key == Key.Right)
                direction = 2;
            if (e.Key == Key.Up)
                direction = 3;
            if (e.Key == Key.Down)
                direction = 4;

            if (e.Key == Key.A)
                direction2 = 1;
            if (e.Key == Key.D)
                direction2 = 2;
            if (e.Key == Key.W)
                direction2 = 3;
            if (e.Key == Key.S)
                direction2 = 4;
        }
    }
}
    

