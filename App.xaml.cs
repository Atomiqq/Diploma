using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Navigation;

namespace Accounting
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool CheckPasswordComplexity(string pw)
        {
            if (pw.Length < 8)
            {
                MessageBox.Show("Введенный пароль меньше 8 символов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (pw.Any(char.IsWhiteSpace))
            {
                MessageBox.Show("Введенный пароль содержит пробел(ы)!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!pw.Any(char.IsLower))
            {
                MessageBox.Show("Введенный пароль не содержит строчных букв!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!pw.Any(char.IsUpper))
            {
                MessageBox.Show("Введенный пароль не содержит прописных букв!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!pw.Any(char.IsDigit))
            {
                MessageBox.Show("Введенный пароль не содержит цифр!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!pw.Any(char.IsPunctuation))
            {
                MessageBox.Show("Введенный пароль не содержит специальных символов!\nПример: !\"#%&'()*,-./:;?@[\\]_{}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public static bool CheckLoginComplexity(string login)
        {
            if (login.Length < 4)
            {
                MessageBox.Show("Введенный логин меньше 4 символов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            if (login.StartsWith(" "))
            {
                MessageBox.Show("Введенный логин начинается с пробела!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            if (login.EndsWith(" "))
            {
                MessageBox.Show("Введенный логин заканчивается на пробел!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            return true;
        }

        public static string Conn { get; set; }

        public static string Login { get; set; }

        public static bool IsUserExist(string login, string pw, string pwRepeat, string serv, bool exist)
        {
            using (SqlConnection connection = new SqlConnection(Conn))
            {
                try
                {
                    connection.Open();

                    if (exist)
                    {
                        MessageBox.Show($"Добро пожаловать, {login}!", "Приветствие", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (CheckLoginComplexity(login) == false) return false;
                        if (CheckPasswordComplexity(pw) == false) return false;
                        if (pw != pwRepeat)
                        {
                            MessageBox.Show("Введенные пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }

                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "EXEC AddUser @id, @pw";
                            command.Parameters.AddWithValue("@id", login);
                            command.Parameters.AddWithValue("@pw", pw);
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show($"Пользователь {login} успешно зарегистрирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    Saved.Default.Server = serv;
                    Saved.Default.Save();

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        public static DataTable Fill(string query, out SqlDataAdapter adapter)
        {
            using (SqlConnection connection = new SqlConnection(Conn))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);

                    using (adapter = new SqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    adapter = null;
                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
