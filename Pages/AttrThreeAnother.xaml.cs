using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Accounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для AttrThreeAnother.xaml
    /// </summary>
    public partial class AttrThreeAnother : Page
    {
        public AttrThreeAnother()
        {
            InitializeComponent();

            using (SqlConnection connection = new SqlConnection(App.Conn))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM ModelsView", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    attrOne.Items.Add(reader[1] + " | " + reader[3]);
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

                        using (SqlCommand command = new SqlCommand("SELECT * FROM ProcessorsView WHERE Id = @id", connection))
                        {
                            command.Parameters.AddWithValue("@id", App.Id);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        attrOne.Text = reader[5].ToString() + " | " + reader[6].ToString();
                                        attrTwo.Text = reader[1].ToString();
                                        attrThree.Text = reader[2].ToString();
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
            bool ok = false;

            if (attrTwo.Text.Any(char.IsDigit) && Convert.ToInt32(attrTwo.Text) >= 1 && Convert.ToInt32(attrTwo.Text) <= 100 && attrThree.Text.Any(char.IsDigit) && Convert.ToInt32(attrThree.Text) >= 1 && Convert.ToInt32(attrThree.Text) < 10000)
            {
                ok = App.AddOrEdit(attrOne.Text.Substring(attrOne.Text.IndexOf('|') + 2), attrTwo.Text, attrThree.Text, NavigationService);
            }
            else
            {
                MessageBox.Show("Количество ядер и частота на ядро должны быть числовыми данными больше 1 и меньше 100 для количества ядер и 10000 для частоты на ядро!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

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
