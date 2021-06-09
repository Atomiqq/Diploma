using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Accounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для AttrFive.xaml
    /// </summary>
    public partial class AttrFive : Page
    {
        public AttrFive()
        {
            InitializeComponent();

            attrFour.Items.Add("Рабочий");
            attrFour.Items.Add("Не рабочий");

            using (SqlConnection connection = new SqlConnection(App.Conn))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT * FROM Types WHERE Kind = 'Оргтехника'", connection))
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
                    using (SqlCommand command = new SqlCommand("SELECT * FROM ModelsView", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    attrThree.Items.Add(reader[1] + " | " + reader[3]);
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
                                    attrFive.Items.Add(reader[1]);
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

                        using (SqlCommand command = new SqlCommand("SELECT * FROM PeripheryView WHERE Id = @id", connection))
                        {
                            command.Parameters.AddWithValue("@id", App.Id);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        attrOne.Text = reader[0].ToString();
                                        attrTwo.Text = reader[3].ToString();
                                        attrThree.Text = reader[9].ToString() + " | " + reader[10].ToString();
                                        attrFive.Text = reader[8].ToString();
                                        if (reader[1].ToString() == "Рабочий") attrFour.SelectedIndex = 0;
                                        else attrFour.SelectedIndex = 1;
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

        /// <summary>
        /// Метод при нажатии на кнопку для добавления или изменения записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addOrEdit_Click(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex("[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]");

            bool ok;

            if (regex.IsMatch(attrOne.Text) == true) ok = App.AddOrEdit(attrOne.Text, attrTwo.Text, attrThree.Text.Substring(attrThree.Text.IndexOf('|') + 2), attrFour.Text, attrFive.Text, NavigationService);
            else
            {
                MessageBox.Show("Введенный код оргтехники не соответствует шаблону! (Пример: 1234567890 (10 цифр подряд))", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

        /// <summary>
        /// Метод на обновление страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Refresh();
        }

        /// <summary>
        /// Метод на возврат к таблице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
