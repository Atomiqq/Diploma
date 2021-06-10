using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Accounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для AttrSeven.xaml
    /// </summary>
    public partial class AttrSeven : Page
    {
        public AttrSeven()
        {
            InitializeComponent();

            attrSix.Items.Add("Рабочий");
            attrSix.Items.Add("Не рабочий");
            attrTwo.Items.Add("");
            attrTwo.SelectedIndex = 0;
            attrThree.Items.Add("");
            attrThree.SelectedIndex = 0;

            using (SqlConnection connection = new SqlConnection(App.Conn))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM PeripheryView WHERE NOT EXISTS"
                                                            + "(SELECT * FROM Computers WHERE Computers.Monitor_Id = PeripheryView.Id)"
                                                            + "AND Type_Name = 'Монитор'", connection))
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
                    using (SqlCommand command = new SqlCommand("SELECT * FROM ProcessorsView", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    attrThree.Items.Add(reader[6] + " | " + reader[7]);
                                }
                        }
                    }
                    using (SqlCommand command = new SqlCommand("SELECT * FROM Cabinets", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    attrSeven.Items.Add(reader[1]);
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

                        using (SqlCommand command = new SqlCommand("SELECT * FROM ComputersView WHERE Id = @id", connection))
                        {
                            command.Parameters.AddWithValue("@id", App.Id);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        attrOne.Text = reader[0].ToString();
                                        if (!attrTwo.Items.Contains(reader[1].ToString())) attrTwo.Items.Add(reader[1].ToString());
                                        attrTwo.SelectedItem = reader[1].ToString();
                                        attrThree.Text = reader[11].ToString() + " | " + reader[12].ToString();
                                        attrFour.Text = reader[7].ToString();
                                        attrFive.Text = reader[8].ToString();
                                        if (reader[9].ToString() == "Рабочий") attrSix.SelectedIndex = 0;
                                        else attrSix.SelectedIndex = 1;
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
            Regex regex = new Regex("[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]");

            bool ok;

            if (attrFour.Text.Any(char.IsDigit) && Convert.ToInt32(attrFour.Text) >= 0 && Convert.ToInt32(attrFour.Text) <= 100 && attrFive.Text.Any(char.IsDigit) && Convert.ToInt32(attrFive.Text) >= 0 && Convert.ToInt32(attrFive.Text) < 10000)
            {
                if (regex.IsMatch(attrOne.Text) == true) ok = App.AddOrEdit(attrOne.Text, attrTwo.Text, attrThree.Text.Substring(attrThree.Text.IndexOf('|') + 2), attrFour.Text, attrFive.Text, attrSix.Text, attrSeven.Text, NavigationService);
                else
                {
                    MessageBox.Show("Введенный код оргтехники не соответствует шаблону! (Пример: 1234567890 (10 цифр подряд))", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Оперативная и физическая память должны быть числовыми данными больше или равно 0 и меньше 100 для оперативной памяти и 10000 для физической памяти!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            

            if (ok == true && App.Table.StartsWith("add"))
            {
                MessageBox.Show($"Запись {attrOne.Text} | {attrTwo.Text} | {attrThree.Text} | {attrFour.Text} | {attrFive.Text} успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (ok == true && App.Table.StartsWith("edit"))
            {
                MessageBox.Show($"Запись №{App.Id} успешно изменена на {attrOne.Text} | {attrTwo.Text} | {attrThree.Text} | {attrFour.Text} | {attrFive.Text}!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void attrTwo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (attrTwo.SelectedIndex != 0)
            {
                using (SqlConnection connection = new SqlConnection(App.Conn))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand("SELECT Cabinet_Name FROM PeripheryView WHERE Id = @id", connection))
                        {
                            command.Parameters.AddWithValue("@id", attrTwo.SelectedItem.ToString());

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        attrSeven.SelectedItem = reader[0].ToString();
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
                attrSeven.IsEnabled = false;
            }
            else
            {
                attrSeven.SelectedIndex = -1;
                attrSeven.IsEnabled = true;
            }
        }
    }
}
