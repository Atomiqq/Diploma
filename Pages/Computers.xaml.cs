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
    /// Логика взаимодействия для Computers.xaml
    /// </summary>
    public partial class Computers : Page
    {
        DataTable dt;
        string query = "SELECT * FROM ComputersView";

        public Computers()
        {
            InitializeComponent();

            dt = App.Fill(query);
            dg.ItemsSource = dt.DefaultView;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            App.Delete(dg, query, dt);
        }
    }
}
