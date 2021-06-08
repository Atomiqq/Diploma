using System;
using System.Data.SqlClient;
using System.Windows;

namespace Accounting.Windows
{
    /// <summary>
    /// Логика взаимодействия для PwChange.xaml
    /// </summary>
    public partial class PwChange : Window
    {
        public PwChange()
        {
            InitializeComponent();
        }

        private void changePw_Click(object sender, RoutedEventArgs e)
        {
            if (App.CheckPasswordComplexity(pwNew.Password) == false) return;
            if (pwOld.Password == pwNew.Password)
            {
                MessageBox.Show("Старый и новый пароли совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (pwNew.Password == pwRepeat.Password)
            {
                using (SqlConnection connection = new SqlConnection(App.Conn))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "EXEC PwChange @login, @pwOld, @pwNew";
                            command.Parameters.AddWithValue("@login", App.Login);
                            command.Parameters.AddWithValue("@pwOld", pwOld.Password);
                            command.Parameters.AddWithValue("@pwNew", pwNew.Password);
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Пароль успешно изменён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        Close();

                        ((MainWindow)Application.Current.MainWindow).mainFrame.Navigate(new Uri(@"Pages\Auth.xaml", UriKind.RelativeOrAbsolute));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Введенные пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            pwNew.Clear();
            pwOld.Clear();
            pwRepeat.Clear();
        }
    }
}
