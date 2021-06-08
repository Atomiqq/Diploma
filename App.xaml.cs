using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Accounting
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Метод проверки сложности пароля
        /// </summary>
        /// <param name="pw"></param>
        /// <returns></returns>
        public static bool CheckPasswordComplexity(string pw)
        {
            if (pw.Length < 6)
            {
                MessageBox.Show("Введенный пароль меньше 6 символов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

        /// <summary>
        /// Метод проверки логина
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
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
            if (login.Contains("  "))
            {
                MessageBox.Show("Введенный логин содержит двойной пробел!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Строка подключения к базе данных
        /// </summary>
        public static string Conn { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public static string Login { get; set; }

        /// <summary>
        /// Название таблицы или операция с ней
        /// </summary>
        public static string Table { get; set; }

        /// <summary>
        /// Идентификатор записи в таблице
        /// </summary>
        public static string Id { get; set; }

        /// <summary>
        /// Метод на проверку существования пользователя и его регистрацию
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pw"></param>
        /// <param name="pwRepeat"></param>
        /// <param name="serv"></param>
        /// <param name="exist"></param>
        /// <returns></returns>
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
            }
        }

        /// <summary>
        /// Метод удаления выделенной записи из таблицы
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="dt"></param>
        public static void Delete(DataGrid dg, DataTable dt)
        {
            if (dg.SelectedItem != null)
            {
                DataRowView datarowView = dg.SelectedItem as DataRowView;
                if (datarowView != null)
                {
                    DataRow dataRow = datarowView.Row;
                    dataRow.Delete();
                }
            }

            using (SqlConnection connection = new SqlConnection(Conn))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        if (Table.EndsWith("Brands")) command.CommandText = "SELECT * FROM Brands";
                        else if (Table.EndsWith("Cabinets")) command.CommandText = "SELECT * FROM Cabinets";
                        else if (Table.EndsWith("Computers")) command.CommandText = "SELECT * FROM ComputersView";
                        else if (Table.EndsWith("Computers_Actions")) command.CommandText = "SELECT * FROM Computers_ActionsView";
                        else if (Table.EndsWith("Models")) command.CommandText = "SELECT * FROM ModelsView";
                        else if (Table.EndsWith("Periphery")) command.CommandText = "SELECT * FROM PeripheryView";
                        else if (Table.EndsWith("Periphery_Actions")) command.CommandText = "SELECT * FROM Periphery_ActionsView";
                        else if (Table.EndsWith("Processors")) command.CommandText = "SELECT * FROM ProcessorsView";
                        else if (Table.EndsWith("Software")) command.CommandText = "SELECT * FROM Software";
                        else if (Table.EndsWith("Types")) command.CommandText = "SELECT * FROM Types";
                        else if (Table.EndsWith("Versions")) command.CommandText = "SELECT * FROM VersionsView";

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                            if (Table.EndsWith("Brands"))
                                adapter.DeleteCommand = new SqlCommand("DeleteBrand", connection);
                            else if (Table.EndsWith("Cabinets"))
                                adapter.DeleteCommand = new SqlCommand("DeleteCabinet", connection);
                            else if (Table.EndsWith("Computers"))
                                adapter.DeleteCommand = new SqlCommand("DeleteComputer", connection);
                            else if (Table.EndsWith("Computers_Actions"))
                                adapter.DeleteCommand = new SqlCommand("DeleteComputer_Action", connection);
                            else if (Table.EndsWith("Models"))
                                adapter.DeleteCommand = new SqlCommand("DeleteModel", connection);
                            else if (Table.EndsWith("Periphery"))
                                adapter.DeleteCommand = new SqlCommand("DeletePeriphery", connection);
                            else if (Table.EndsWith("Periphery_Actions"))
                                adapter.DeleteCommand = new SqlCommand("DeletePeriphery_Action", connection);
                            else if (Table.EndsWith("Processors"))
                                adapter.DeleteCommand = new SqlCommand("DeleteProcessor", connection);
                            else if (Table.EndsWith("Software"))
                                adapter.DeleteCommand = new SqlCommand("DeleteSoftware", connection);
                            else if (Table.EndsWith("Types"))
                                adapter.DeleteCommand = new SqlCommand("DeleteType", connection);
                            else if (Table.EndsWith("Versions"))
                                adapter.DeleteCommand = new SqlCommand("DeleteVersion", connection);

                            adapter.DeleteCommand.CommandType = CommandType.StoredProcedure;
                            adapter.DeleteCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");

                            adapter.Update(dt);
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

        /// <summary>
        /// Метод добавления или изменения записи в таблицах с одним изменяемым атрибутом
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="nav"></param>
        public static void AddOrEdit(string attr, NavigationService nav)
        {
            if (!string.IsNullOrWhiteSpace(attr))
            {
                using (SqlConnection connection = new SqlConnection(Conn))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = connection.CreateCommand())
                        {
                            if (Table.StartsWith("add"))
                            {
                                if (Table.EndsWith("Brands")) command.CommandText = "AddBrand";
                                if (Table.EndsWith("Cabinets")) command.CommandText = "AddCabinet";
                                if (Table.EndsWith("Software")) command.CommandText = "AddSoftware";
                            }
                            else if (Table.StartsWith("edit"))
                            {
                                if (Table.EndsWith("Brands")) command.CommandText = "EditBrand";
                                if (Table.EndsWith("Cabinets")) command.CommandText = "EditCabinet";
                                if (Table.EndsWith("Software")) command.CommandText = "EditSoftware";
                                command.Parameters.AddWithValue("@id", Id);
                            }

                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@name", attr);

                            if (command.ExecuteNonQuery() == -1)
                            {
                                MessageBox.Show("Введенная запись уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            if (Table.StartsWith("add") && command.ExecuteNonQuery() != -1)
                            {
                                MessageBox.Show($"Запись {attr} успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            if (Table.StartsWith("edit") && command.ExecuteNonQuery() != -1)
                            {
                                MessageBox.Show($"Запись №{Id} успешно изменена на {attr}!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }

                        Table = Table.Replace("add", "");
                        Table = Table.Replace("edit", "");

                        nav.Navigate(new Uri($@"Pages\{Table}.xaml", UriKind.RelativeOrAbsolute));
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
                MessageBox.Show("Название не может быть пустым или содержать только пробелы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// Метод добавления и изменения записи в таблицах с двумя изменяемыми атрибутами
        /// </summary>
        /// <param name="attrOne"></param>
        /// <param name="attrTwo"></param>
        /// <param name="nav"></param>
        public static void AddOrEdit(string attrOne, string attrTwo, NavigationService nav)
        {
            if (!string.IsNullOrWhiteSpace(attrOne) && !string.IsNullOrWhiteSpace(attrTwo))
            {
                using (SqlConnection connection = new SqlConnection(Conn))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = connection.CreateCommand())
                        {
                            if (Table.StartsWith("add"))
                            {
                                if (Table.EndsWith("Types")) command.CommandText = "AddType";
                            }
                            else if (Table.StartsWith("edit"))
                            {
                                if (Table.EndsWith("Types")) command.CommandText = "EditType";
                                command.Parameters.AddWithValue("@id", Id);
                            }

                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@name", attrOne);
                            command.Parameters.AddWithValue("@kind", attrTwo);

                            if (command.ExecuteNonQuery() == -1)
                            {
                                MessageBox.Show("Введенная запись уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            if (Table.StartsWith("add") && command.ExecuteNonQuery() != -1)
                            {
                                MessageBox.Show($"Запись {attrOne} | {attrTwo} успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            if (Table.StartsWith("edit") && command.ExecuteNonQuery() != -1)
                            {
                                MessageBox.Show($"Запись №{Id} успешно изменена на {attrOne} | {attrTwo}!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }

                        Table = Table.Replace("add", "");
                        Table = Table.Replace("edit", "");

                        nav.Navigate(new Uri($@"Pages\{Table}.xaml", UriKind.RelativeOrAbsolute));
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
        /// Метод добаления и изменения записи в таблицах с тремя изменемыми атрибутами
        /// </summary>
        /// <param name="attrOne"></param>
        /// <param name="attrTwo"></param>
        /// <param name="attrThree"></param>
        /// <param name=""></param>
        public static void AddOrEdit(string attrOne, string attrTwo, string attrThree, NavigationService nav)
        {

        }

       /// <summary>
       /// Метод, возвращающий заполненный DataTable с таблицей
       /// </summary>
       /// <returns></returns>
        public static DataTable Fill()
        {
            using (SqlConnection connection = new SqlConnection(Conn))
            {
                try
                {
                    connection.Open();

                   using (SqlCommand command = connection.CreateCommand())
                   {
                        if (Table.EndsWith("Brands")) command.CommandText = "SELECT * FROM Brands";
                        else if (Table.EndsWith("Cabinets")) command.CommandText = "SELECT * FROM Cabinets";
                        else if (Table.EndsWith("Computers")) command.CommandText = "SELECT * FROM ComputersView";
                        else if (Table.EndsWith("Computers_Actions")) command.CommandText = "SELECT * FROM Computers_ActionsView";
                        else if (Table.EndsWith("Models")) command.CommandText = "SELECT * FROM ModelsView";
                        else if (Table.EndsWith("Periphery")) command.CommandText = "SELECT * FROM PeripheryView";
                        else if (Table.EndsWith("Periphery_Actions")) command.CommandText = "SELECT * FROM Periphery_ActionsView";
                        else if (Table.EndsWith("Processors")) command.CommandText = "SELECT * FROM ProcessorsView";
                        else if (Table.EndsWith("Software")) command.CommandText = "SELECT * FROM Software";
                        else if (Table.EndsWith("Types")) command.CommandText = "SELECT * FROM Types";
                        else if (Table.EndsWith("Versions")) command.CommandText = "SELECT * FROM VersionsView";

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                   }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }
    }
}
