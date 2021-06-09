using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Accounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для AttrFour.xaml
    /// </summary>
    public partial class AttrFour : Page
    {
        public AttrFour()
        {
            InitializeComponent();

            if (App.Table.EndsWith("Periphery_Actions")) lbl.Content = "Оргтехника:";

            using (SqlConnection connection = new SqlConnection(App.Conn))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM Types WHERE Kind = 'Операция'", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    attrOne.Items.Add(reader[1]);
                                }
                        }
                    }

                    string query = null;

                    if (App.Table.EndsWith("Periphery_Actions")) query = "SELECT * FROM Periphery";
                    if (App.Table.EndsWith("Computers_Actions")) query = "SELECT * FROM Computers";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    attrTwo.Items.Add(reader[0]);
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

            if (App.Table.StartsWith("edit"))
            {
                addOrEdit.Content = "Изменить";

                using (SqlConnection connection = new SqlConnection(App.Conn))
                {
                    try
                    {
                        connection.Open();

                        string query = null;

                        if (App.Table.EndsWith("Periphery_Actions")) query = "SELECT * FROM Periphery_ActionsView WHERE Id = @id";
                        if (App.Table.EndsWith("Computers_Actions")) query = "SELECT * FROM Computers_ActionsView WHERE Id = @id";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", App.Id);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        attrOne.Text = reader[4].ToString();
                                        attrTwo.Text = reader[5].ToString();
                                        attrThree.Text = Convert.ToDateTime(reader[1]).ToString("dd.MM.yyyy");
                                        attrFour.Text = reader[2].ToString();
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
            bool ok = App.AddOrEdit(attrOne.Text, attrTwo.Text, attrThree.Text, attrFour.Text, null, NavigationService);

            if (ok == true && App.Table.StartsWith("add"))
            {
                MessageBox.Show($"Запись {attrOne.Text} | {attrTwo.Text} | {attrThree.Text} | {attrFour.Text} успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (ok == true && App.Table.StartsWith("edit"))
            {
                MessageBox.Show($"Запись №{App.Id} успешно изменена на {attrOne.Text} | {attrTwo.Text} | {attrThree.Text} | {attrFour.Text}!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
