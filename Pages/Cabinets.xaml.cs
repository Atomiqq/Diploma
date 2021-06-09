using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Accounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для Cabinets.xaml
    /// </summary>
    public partial class Cabinets : Page
    {
        DataTable dt;

        public Cabinets()
        {
            InitializeComponent();

            App.Table = "Cabinets";

            dt = App.Fill();
            dg.ItemsSource = dt.DefaultView;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            App.Table = "addCabinets";

            NavigationService.Navigate(new Uri(@"Pages\AttrOne.xaml", UriKind.RelativeOrAbsolute));
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItem != null)
            {
                var cellInfo = dg.SelectedCells[0];
                App.Id = (cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text;
                App.Table = "editCabinets";

                NavigationService.Navigate(new Uri(@"Pages\AttrOne.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            App.Delete(dg, dt);
        }
    }
}
