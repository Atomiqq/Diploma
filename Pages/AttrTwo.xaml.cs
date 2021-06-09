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
    public partial class AttrTwo : Page
    {
        public AttrTwo()
        {
            InitializeComponent();


            if (App.Table.EndsWith("Models"))
            {
                attrTwo.Items.Clear();
                lblTwo.Content = "Бренд:";

                using (SqlConnection connection = new SqlConnection(App.Conn))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand("SELECT * FROM Brands", connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows) while (reader.Read()) attrTwo.Items.Add(reader[1]);
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
            if (App.Table.EndsWith("Types"))
            {
                attrTwo.Items.Clear();
                attrTwo.Items.Add("Оргтехника");
                attrTwo.Items.Add("Операция");
            }

            if (App.Table.StartsWith("edit"))
            {
                addOrEdit.Content = "Изменить";

                using (SqlConnection connection = new SqlConnection(App.Conn))
                {
                    try
                    {
                        connection.Open();

                        string query = null;

                        if (App.Table.EndsWith("Models")) query = "SELECT * FROM ModelsView WHERE Id = @id";
                        if (App.Table.EndsWith("Types")) query = "SELECT * FROM Types WHERE Id = @id";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", App.Id);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        if (App.Table.EndsWith("Models"))
                                        {
                                            attrOne.Text = reader[3].ToString();
                                            attrTwo.Text = reader[1].ToString();
                                        }
                                        if (App.Table.EndsWith("Types"))
                                        {
                                            attrOne.Text = reader[1].ToString();
                                            if (reader[2].ToString() == "Оргтехника") attrTwo.SelectedIndex = 0;
                                            else attrTwo.SelectedIndex = 1;
                                        }
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
            bool ok = App.AddOrEdit(attrOne.Text, attrTwo.Text, NavigationService);

            if (ok == true && App.Table.StartsWith("add"))
            {
                MessageBox.Show($"Запись {attrOne.Text} | {attrTwo.Text} успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (ok == true && App.Table.StartsWith("edit"))
            {
                MessageBox.Show($"Запись №{App.Id} успешно изменена на {attrOne.Text} | {attrTwo.Text}!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}
