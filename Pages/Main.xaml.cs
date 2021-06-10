using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Accounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        public Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Метод на открытие окна по изменению пароля по нажатию кнопки меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updatePw_Click(object sender, RoutedEventArgs e)
        {
            Windows.PwChange pwChange = new Windows.PwChange();
            pwChange.ShowDialog();
        }

        /// <summary>
        /// Метод возврата на страницу авторизации по нажатию кнопки меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(@"Pages\Auth.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метод выхода из приложения по нажатию кнопки меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Метод перехода на страницу Компьютеры по нажатию кнопки меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void computers_Click(object sender, RoutedEventArgs e)
        {
            lbl.Visibility = Visibility.Hidden;
            dataFrame.Navigate(new Uri(@"Pages\Computers.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метод перехода на страницу Оргтехника по нажатию кнопки меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void periphery_Click(object sender, RoutedEventArgs e)
        {
            lbl.Visibility = Visibility.Hidden;
            dataFrame.Navigate(new Uri(@"Pages\Periphery.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метод перехода на страницу Процессоры по нажатию кнопки меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void processors_Click(object sender, RoutedEventArgs e)
        {
            lbl.Visibility = Visibility.Hidden;
            dataFrame.Navigate(new Uri(@"Pages\Processors.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метод перехода на страницу Кабинеты по нажатию кнопки меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cabinets_Click(object sender, RoutedEventArgs e)
        {
            lbl.Visibility = Visibility.Hidden;
            dataFrame.Navigate(new Uri(@"Pages\Cabinets.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метод перехода на страницу Марки по нажатию кнопки меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void brands_Click(object sender, RoutedEventArgs e)
        {
            lbl.Visibility = Visibility.Hidden;
            dataFrame.Navigate(new Uri(@"Pages\Brands.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метод перехода на страницу Модели по нажатию кнопки меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void models_Click(object sender, RoutedEventArgs e)
        {
            lbl.Visibility = Visibility.Hidden;
            dataFrame.Navigate(new Uri(@"Pages\Models.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метод перехода на страницу Операции с компьютерами по нажатию кнопки меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void computersActions_Click(object sender, RoutedEventArgs e)
        {
            lbl.Visibility = Visibility.Hidden;
            dataFrame.Navigate(new Uri(@"Pages\Computers_Actions.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метод перехода на страницу Операции с оргтехникой по нажатию кнопки меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void peripheryActions_Click(object sender, RoutedEventArgs e)
        {
            lbl.Visibility = Visibility.Hidden;
            dataFrame.Navigate(new Uri(@"Pages\Periphery_Actions.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метода перехода на страницу Типы по нажатию кнопки меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void types_Click(object sender, RoutedEventArgs e)
        {
            lbl.Visibility = Visibility.Hidden;
            dataFrame.Navigate(new Uri(@"Pages\Types.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метода перехода на страницу ПО по нажатию кнопки меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void software_Click(object sender, RoutedEventArgs e)
        {
            lbl.Visibility = Visibility.Hidden;
            dataFrame.Navigate(new Uri(@"Pages\Software.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метод перехода на страницу Установленное ПО по нажатию кнопки меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void versions_Click(object sender, RoutedEventArgs e)
        {
            lbl.Visibility = Visibility.Hidden;
            dataFrame.Navigate(new Uri(@"Pages\Versions.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
