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
using System.Data.SqlClient;

namespace Accounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для AttrOne.xaml
    /// </summary>
    public partial class AttrOne : Page
    {
        public AttrOne()
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

                        if (App.Table.Contains("Brands")) query = "SELECT * FROM Brands WHERE Id = @id";
                        if (App.Table.Contains("Cabinets")) query = "SELECT * FROM Cabinets WHERE Id = @id";
                        if (App.Table.Contains("Software")) query = "SELECT * FROM Software WHERE Id = @id";

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
        
        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Refresh();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void addOrEdit_Click(object sender, RoutedEventArgs e)
        {
            bool ok = App.AddOrEdit(attrOne.Text, NavigationService);

            if (ok == true && App.Table.StartsWith("add"))
            {
                MessageBox.Show($"Запись {attrOne.Text} успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (ok == true && App.Table.StartsWith("edit"))
            {
                MessageBox.Show($"Запись №{App.Id} успешно изменена на {attrOne.Text}!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
