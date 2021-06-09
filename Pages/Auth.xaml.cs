using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Accounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Page
    {
        public Auth()
        {
            InitializeComponent();

            login.Text = null;
            serv.Text = Saved.Default.Server;
        }

        /// <summary>
        /// Метод перехода на страницу регистрации по нажатию кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(@"Pages\Reg.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метод обновления страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Refresh();
        }

        /// <summary>
        /// Метод перехода на главную страницу по нажатию кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void auth_Click(object sender, RoutedEventArgs e)
        {
            App.Conn = $@"Data Source={serv.Text};Initial Catalog=Accounting;Integrated Security=False;User Id={login.Text};Password={pw.Password}";

            if (App.IsUserExist(login.Text, pw.Password, null, serv.Text, true))
            {
                NavigationService.Navigate(new Uri(@"Pages\Main.xaml", UriKind.RelativeOrAbsolute));

                App.Login = login.Text;
            }
        }
    }
}
