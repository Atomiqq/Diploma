using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Accounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для Brands.xaml
    /// </summary>
    public partial class Brands : Page
    {
        DataTable dt;
        public Brands()
        {
            InitializeComponent();

            App.Table = "Brands";

            dt = App.Fill();
            dg.ItemsSource = dt.DefaultView;
        }

        /// <summary>
        /// Метод перехода на страницу добавления записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_Click(object sender, RoutedEventArgs e)
        {
            App.Table = "addBrands";

            NavigationService.Navigate(new Uri(@"Pages\AttrOne.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Метод перехода на страницу изменения записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItem != null)
            {
                var cellInfo = dg.SelectedCells[0];
                App.Id = (cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text;
                App.Table = "editBrands";

                NavigationService.Navigate(new Uri(@"Pages\AttrOne.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        /// <summary>
        /// Метод удаления выделенной записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            App.Delete(dg, dt);
        }
    }
}
