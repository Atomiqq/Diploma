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

        private void updatePw_Click(object sender, RoutedEventArgs e)
        {
            Windows.PwChange pwChange = new Windows.PwChange();
            pwChange.ShowDialog();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void computers_Click(object sender, RoutedEventArgs e)
        {
            dataFrame.Navigate(new Uri(@"Pages\Computers.xaml", UriKind.RelativeOrAbsolute));
        }

        private void periphery_Click(object sender, RoutedEventArgs e)
        {
            dataFrame.Navigate(new Uri(@"Pages\Periphery.xaml", UriKind.RelativeOrAbsolute));
        }

        private void processors_Click(object sender, RoutedEventArgs e)
        {
            dataFrame.Navigate(new Uri(@"Pages\Processors.xaml", UriKind.RelativeOrAbsolute));
        }

        private void cabinets_Click(object sender, RoutedEventArgs e)
        {
            dataFrame.Navigate(new Uri(@"Pages\Cabinets.xaml", UriKind.RelativeOrAbsolute));
        }

        private void brands_Click(object sender, RoutedEventArgs e)
        {
            dataFrame.Navigate(new Uri(@"Pages\Brands.xaml", UriKind.RelativeOrAbsolute));
        }

        private void models_Click(object sender, RoutedEventArgs e)
        {
            dataFrame.Navigate(new Uri(@"Pages\Models.xaml", UriKind.RelativeOrAbsolute));
        }

        private void computersActions_Click(object sender, RoutedEventArgs e)
        {
            dataFrame.Navigate(new Uri(@"Pages\Computers_Actions.xaml", UriKind.RelativeOrAbsolute));
        }

        private void peripheryActions_Click(object sender, RoutedEventArgs e)
        {
            dataFrame.Navigate(new Uri(@"Pages\Periphery_Actions.xaml", UriKind.RelativeOrAbsolute));
        }

        private void types_Click(object sender, RoutedEventArgs e)
        {
            dataFrame.Navigate(new Uri(@"Pages\Types.xaml", UriKind.RelativeOrAbsolute));
        }

        private void software_Click(object sender, RoutedEventArgs e)
        {
            dataFrame.Navigate(new Uri(@"Pages\Software.xaml", UriKind.RelativeOrAbsolute));
        }

        private void versions_Click(object sender, RoutedEventArgs e)
        {
            dataFrame.Navigate(new Uri(@"Pages\Versions.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
