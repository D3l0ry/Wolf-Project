using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Wolf.Helper.User;

namespace Wolf
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationMenu.xaml
    /// </summary>
    public partial class AuthorizationMenu : Window
    {
        public bool CheckRegistration;

        public AuthorizationMenu()
        {
            InitializeComponent();

            LoginBox.Text = Properties.Settings.Default.Login;
            PasswordBox.Password = Properties.Settings.Default.Password;

            if (Properties.Settings.Default.IsAuthorized)
            {
                if (WolfClient.ClientAuthorize("http://wolf.wolfproject.ru/UIUser/LogIn/", Properties.Settings.Default.Login, Properties.Settings.Default.Password))
                {
                    Hide();
                    new MainMenu().Show();
                }
                else
                {
                    Properties.Settings.Default.IsAuthorized = false;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void RegistrationDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CheckRegistration = !CheckRegistration;

            RegistrationLabel.Content = !CheckRegistration ? "Зарегистрироваться" : "Авторизироваться";
            AuthorizationButton.Content = CheckRegistration ? "Зарегистрироваться" : "Авторизироваться";
        }

        private void AuthorizationClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((LoginBox.Text.Length > 3 && LoginBox.Text.Length <= 25) && (PasswordBox.Password.Length > 5))
                {
                    if ((string)AuthorizationButton.Content == "Авторизироваться")
                    {
                        if (WolfClient.ClientAuthorize("http://wolf.wolfproject.ru/UIUser/LogIn/", LoginBox.Text.ToLower(), PasswordBox.Password))
                        {
                            Properties.Settings.Default.Login = LoginBox.Text;
                            Properties.Settings.Default.Password = PasswordBox.Password;
                            Properties.Settings.Default.IsAuthorized = true;
                            Properties.Settings.Default.Save();

                            Hide();
                            new MainMenu().Show();
                        }
                        else
                        {
                            MessageBox.Show("Вы ввели неправильный логин или пароль! \nВозможно ваш HWID больше не действителен!", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        if (WolfClient.ClientAuthorize("http://wolf.wolfproject.ru/UIUser/SignUp/", LoginBox.Text.ToLower(), PasswordBox.Password))
                        {
                            Properties.Settings.Default.Login = LoginBox.Text;
                            Properties.Settings.Default.Password = PasswordBox.Password;
                            Properties.Settings.Default.IsAuthorized = true;
                            Properties.Settings.Default.Save();

                            Hide();
                            new MainMenu().Show();
                        }
                        else
                        {
                            MessageBox.Show("Такой пользователь уже создан!", "Ошибка регистрации!", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Логин должен быть больше 3 символов! \nПароль должен быть больше 7 символов!", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message, "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }
    }
}