using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Accounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для Versions.xaml
    /// </summary>
    public partial class Versions : Page
    {
        DataTable dt;

        public Versions()
        {
            InitializeComponent();

            App.Table = "Versions";

            dt = App.Fill();
            dg.ItemsSource = dt.DefaultView;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            App.Table = "addVersions";

            NavigationService.Navigate(new Uri(@"Pages\AttrThree.xaml", UriKind.RelativeOrAbsolute));
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItem != null)
            {
                var cellInfo = dg.SelectedCells[0];
                App.Id = (cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text;
                App.Table = "editVersions";

                NavigationService.Navigate(new Uri(@"Pages\AttrThree.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            App.Delete(dg, dt);
        }
    }
}
