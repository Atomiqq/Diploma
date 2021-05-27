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
                            command.CommandText = "EXEC PwChange @login, @pwNew, @pwOld";
                            command.Parameters.AddWithValue("@login", App.Login);
                            command.Parameters.AddWithValue("@pwNew", pwNew.Password);
                            command.Parameters.AddWithValue("@pwOld", pwOld.Password);
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Пароль успешно изменён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        Close();

                        ((MainWindow)Application.Current.MainWindow).mainFrame.GoBack();
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
            else
            {
                MessageBox.Show("Введенные пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            foreach (FrameworkElement el in pwChGrid.Children)
            {
                if (el is PasswordBox)
                {
                    PasswordBox elem = (PasswordBox) el;
                    elem.Password = null;
                }
            }
        }
    }
}
