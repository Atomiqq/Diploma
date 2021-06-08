using System;
using System.Collections.Generic;
using System.Data;
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
