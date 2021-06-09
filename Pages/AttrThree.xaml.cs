using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Accounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для AttrThree.xaml
    /// </summary>
    public partial class AttrThree : Page
    {
        public AttrThree()
        {
            InitializeComponent();

            using (SqlConnection connection = new SqlConnection(App.Conn))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM Computers", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    attrOne.Items.Add(reader[0]);
                                }
                        }
                    }
                    using (SqlCommand command = new SqlCommand("SELECT * FROM Software", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    attrTwo.Items.Add(reader[1]);
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

                        using (SqlCommand command = new SqlCommand("SELECT * FROM VersionsView WHERE Id = @id", connection))
                        {
                            command.Parameters.AddWithValue("@id", App.Id);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        attrOne.Text = reader[2].ToString();
                                        attrTwo.Text = reader[4].ToString();
                                        attrThree.Text = reader[1].ToString();
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
            bool ok = App.AddOrEdit(attrOne.Text, attrTwo.Text, attrThree.Text, NavigationService);

            if (ok == true && App.Table.StartsWith("add"))
            {
                MessageBox.Show($"Запись {attrOne.Text} | {attrTwo.Text} | {attrThree.Text} успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (ok == true && App.Table.StartsWith("edit"))
            {
                MessageBox.Show($"Запись №{App.Id} успешно изменена на {attrOne.Text} | {attrTwo.Text} | {attrThree.Text}!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
