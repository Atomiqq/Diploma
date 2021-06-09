using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Accounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для Reg.xaml
    /// </summary>
    public partial class Reg : Page
    {
        public Reg()
        {
            InitializeComponent();

            serv.Text = Saved.Default.Server;
        }

        /// <summary>
        /// Метод возврата на страницу авторизации по нажатию кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        /// <summary>
        /// Метод обновления страницы по нажатию кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Refresh();
        }

        /// <summary>
        /// Метод регистрации пользователя по нажатию кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reg_Click(object sender, RoutedEventArgs e)
        {
            App.Conn = $@"Data Source={serv.Text};Initial Catalog=Accounting;Integrated Security=True";

            if (App.IsUserExist(login.Text, pw.Password, pwRepeat.Password, serv.Text, false)) NavigationService.GoBack();
        }
    }
}
