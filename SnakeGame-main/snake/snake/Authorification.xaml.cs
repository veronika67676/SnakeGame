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
    /// Логика взаимодействия для Authorification.xaml
    /// </summary>
    public partial class Authorification : Page
    {
        private const string predefinedLogin = "snake";
        private const string predefinedPassword = "1234";
        public Authorification()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginTextBox.Text;
            var password = PasswordBox.Password;

            if (login == predefinedLogin && password == predefinedPassword)
            {
                StatusTextBlock.Text = "Успешная авторизация!";
                StatusTextBlock.Foreground = System.Windows.Media.Brushes.Green;

                MainWindow nigger = new MainWindow();
                nigger.Show();

                
            }
            else
            {
                StatusTextBlock.Text = "Логин или пароль неверны.";
                StatusTextBlock.Foreground = System.Windows.Media.Brushes.Red;
            }
        }
    }
}
