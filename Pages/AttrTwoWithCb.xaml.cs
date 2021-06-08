using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AttrTwo.xaml
    /// </summary>
    public partial class AttrTwoWithCb : Page
    {
        public AttrTwoWithCb()
        {
            InitializeComponent();

            if (App.Table.StartsWith("edit"))
            {
                addOrEdit.Content = "Изменить";

                using (SqlConnection connection = new SqlConnection(App.Conn))
                {
                    try
                    {
                        connection.Open();

                        string query = null;

                        if (App.Table.Contains("Types")) query = "SELECT * FROM Types WHERE Id = @id";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", App.Id);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        attrOne.Text = reader[1].ToString();
                                        if (reader[2].ToString() == "Оргтехника") attrTwo.SelectedIndex = 0;
                                        else attrTwo.SelectedIndex = 1;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
        }

        private void addOrEdit_Click(object sender, RoutedEventArgs e)
        {
            App.AddOrEdit(attrOne.Text, attrTwo.Text, NavigationService);
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Refresh();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
