using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для Models.xaml
    /// </summary>
    public partial class Models : Page
    {
        public Models()
        {
            InitializeComponent();

            using (SqlConnection connection = new SqlConnection(App.Conn))
            {
                try
                {
                    connection.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Brands", connection);
                    SqlDataAdapter adapter1 = new SqlDataAdapter("SELECT * FROM Models", connection);

                    DataSet ds = new DataSet();

                    adapter.Fill(ds, "Brands");
                    adapter1.Fill(ds, "Models");

                    DataRelation relation = ds.Relations.Add("BrandsModels", ds.Tables["Brands"].Columns["Brand_Id"], ds.Tables["Models"].Columns["Brand_Id"]);

                    dg.ItemsSource = relation.ParentTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
