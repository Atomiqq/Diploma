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
                        if (Table.EndsWith("Cabinets")) command.CommandText = "SELECT * FROM Cabinets";
                        if (Table.EndsWith("Computers")) command.CommandText = "SELECT * FROM ComputersView";
                        if (Table.EndsWith("Computers_Actions")) command.CommandText = "SELECT * FROM Computers_ActionsView";
                        if (Table.EndsWith("Models")) command.CommandText = "SELECT * FROM ModelsView";
                        if (Table.EndsWith("Periphery")) command.CommandText = "SELECT * FROM PeripheryView";
                        if (Table.EndsWith("Periphery_Actions")) command.CommandText = "SELECT * FROM Periphery_ActionsView";
                        if (Table.EndsWith("Processors")) command.CommandText = "SELECT * FROM ProcessorsView";
                        if (Table.EndsWith("Software")) command.CommandText = "SELECT * FROM Software";
                        if (Table.EndsWith("Types")) command.CommandText = "SELECT * FROM Types";
                        if (Table.EndsWith("Versions")) command.CommandText = "SELECT * FROM VersionsView";

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                            if (Table.EndsWith("Brands"))
                                adapter.DeleteCommand = new SqlCommand("DeleteBrand", connection);
                            if (Table.EndsWith("Cabinets"))
                                adapter.DeleteCommand = new SqlCommand("DeleteCabinet", connection);
                            if (Table.EndsWith("Computers"))
                                adapter.DeleteCommand = new SqlCommand("DeleteComputer", connection);
                            if (Table.EndsWith("Computers_Actions"))
                                adapter.DeleteCommand = new SqlCommand("DeleteComputer_Action", connection);
                            if (Table.EndsWith("Models"))
                                adapter.DeleteCommand = new SqlCommand("DeleteModel", connection);
                            if (Table.EndsWith("Periphery"))
                                adapter.DeleteCommand = new SqlCommand("DeletePeriphery", connection);
                            if (Table.EndsWith("Periphery_Actions"))
                                adapter.DeleteCommand = new SqlCommand("DeletePeriphery_Action", connection);
                            if (Table.EndsWith("Processors"))
                                adapter.DeleteCommand = new SqlCommand("DeleteProcessor", connection);
                            if (Table.EndsWith("Software"))
                                adapter.DeleteCommand = new SqlCommand("DeleteSoftware", connection);
                            if (Table.EndsWith("Types"))
                                adapter.DeleteCommand = new SqlCommand("DeleteType", connection);
                            if (Table.EndsWith("Versions"))
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
        public static bool AddOrEdit(string attr, NavigationService nav)
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
                            if (Table.StartsWith("edit"))
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
                                return false;
                            }
                        }

                        if (Table.EndsWith("Brands")) nav.Navigate(new Uri($@"Pages\Brands.xaml", UriKind.RelativeOrAbsolute));
                        if (Table.EndsWith("Cabinets")) nav.Navigate(new Uri($@"Pages\Cabinets.xaml", UriKind.RelativeOrAbsolute));
                        if (Table.EndsWith("Software")) nav.Navigate(new Uri($@"Pages\Software.xaml", UriKind.RelativeOrAbsolute));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Строки не могут быть пустыми и содержать только пробелы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Метод добавления или изменения записи в таблицах с двумя изменяемыми атрибутами
        /// </summary>
        /// <param name="attrOne"></param>
        /// <param name="attrTwo"></param>
        /// <param name="nav"></param>
        public static bool AddOrEdit(string attrOne, string attrTwo, NavigationService nav)
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
                                if (Table.EndsWith("Types"))
                                {
                                    command.CommandText = "AddType";
                                    command.Parameters.AddWithValue("@kind", attrTwo);
                                }
                                if (Table.EndsWith("Models"))
                                {
                                    command.CommandText = "AddModel";
                                    command.Parameters.AddWithValue("@brand", attrTwo);
                                }
                            }
                            if (Table.StartsWith("edit"))
                            {
                                if (Table.EndsWith("Types"))
                                {
                                    command.CommandText = "EditType";
                                    command.Parameters.AddWithValue("@kind", attrTwo);
                                }
                                if (Table.EndsWith("Models"))
                                {
                                    command.CommandText = "EditModel";
                                    command.Parameters.AddWithValue("@brand", attrTwo);
                                }
                                command.Parameters.AddWithValue("@id", Id);
                            }

                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@name", attrOne);

                            if (command.ExecuteNonQuery() == -1)
                            {
                                MessageBox.Show("Введенная запись уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return false;
                            }
                        }

                        if (Table.EndsWith("Types")) nav.Navigate(new Uri($@"Pages\Types.xaml", UriKind.RelativeOrAbsolute));
                        if (Table.EndsWith("Models")) nav.Navigate(new Uri($@"Pages\Models.xaml", UriKind.RelativeOrAbsolute));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Строки не могут быть пустыми и содержать только пробелы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Метод добавления или изменения записи в таблицах с тремя изменяемыми атрибутами
        /// </summary>
        /// <param name="attrOne"></param>
        /// <param name="attrTwo"></param>
        /// <param name="attrThree"></param>
        /// <param name=""></param>
        public static bool AddOrEdit(string attrOne, string attrTwo, string attrThree, NavigationService nav)
        {
            if (!string.IsNullOrWhiteSpace(attrOne) && !string.IsNullOrWhiteSpace(attrTwo) && !string.IsNullOrWhiteSpace(attrThree))
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
                                if (Table.EndsWith("Versions"))
                                {
                                    command.CommandText = "AddVersion";
                                    command.Parameters.AddWithValue("@computer_id", attrOne);
                                    command.Parameters.AddWithValue("@software", attrTwo);
                                    command.Parameters.AddWithValue("@value", attrThree);
                                }
                                if (Table.EndsWith("Processors"))
                                {
                                    command.CommandText = "AddProcessor";
                                    command.Parameters.AddWithValue("@model", attrOne);
                                    command.Parameters.AddWithValue("@cores", attrTwo);
                                    command.Parameters.AddWithValue("@frequency", attrThree);
                                }
                            }
                            if (Table.StartsWith("edit"))
                            {
                                if (Table.EndsWith("Versions"))
                                {
                                    command.CommandText = "EditVersion";
                                    command.Parameters.AddWithValue("@computer_id", attrOne);
                                    command.Parameters.AddWithValue("@software", attrTwo);
                                    command.Parameters.AddWithValue("@value", attrThree);
                                }
                                if (Table.EndsWith("Processors"))
                                {
                                    command.CommandText = "EditProcessor";
                                    command.Parameters.AddWithValue("@model", attrOne);
                                    command.Parameters.AddWithValue("@cores", attrTwo);
                                    command.Parameters.AddWithValue("@frequency", attrThree);
                                }
                                command.Parameters.AddWithValue("@id", Id);
                            }

                            command.CommandType = CommandType.StoredProcedure;

                            if (command.ExecuteNonQuery() == -1)
                            {
                                MessageBox.Show("Введенная запись уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return false;
                            }
                        }

                        if (Table.EndsWith("Versions")) nav.Navigate(new Uri($@"Pages\Versions.xaml", UriKind.RelativeOrAbsolute));
                        if (Table.EndsWith("Processors")) nav.Navigate(new Uri($@"Pages\Processors.xaml", UriKind.RelativeOrAbsolute));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Строки не могут быть пустыми или содержать только пробелы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Метод добавления или изменения записи в таблицах с четырьмя или пятью изменяемыми атрибутами 
        /// </summary>
        /// <param name="attrOne"></param>
        /// <param name="attrTwo"></param>
        /// <param name="attrThree"></param>
        /// <param name="attrFour"></param>
        /// <param name="nav"></param>
        /// <returns></returns>
        public static bool AddOrEdit(string attrOne, string attrTwo, string attrThree, string attrFour, string attrFive, NavigationService nav)
        {
            if (!string.IsNullOrWhiteSpace(attrOne) && !string.IsNullOrWhiteSpace(attrTwo) && !string.IsNullOrWhiteSpace(attrThree))
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
                                if (Table.EndsWith("Periphery"))
                                {
                                    command.CommandText = "AddPeriphery";
                                    command.Parameters.AddWithValue("@id", attrOne);
                                    command.Parameters.AddWithValue("@type", attrTwo);
                                    command.Parameters.AddWithValue("@model", attrThree);
                                    command.Parameters.AddWithValue("@is_working", attrFour);
                                    command.Parameters.AddWithValue("@cabinet", attrFive);
                                }
                                if (Table.EndsWith("Computers_Actions"))
                                {
                                    command.CommandText = "AddComputer_Action";
                                    command.Parameters.AddWithValue("@type", attrOne);
                                    command.Parameters.AddWithValue("@computer_id", attrTwo);
                                    command.Parameters.AddWithValue("@date", attrThree);
                                    command.Parameters.AddWithValue("@note", attrFour);
                                }
                                if (Table.EndsWith("Periphery_Actions"))
                                {
                                    command.CommandText = "AddPeriphery_Action";
                                    command.Parameters.AddWithValue("@type", attrOne);
                                    command.Parameters.AddWithValue("@periphery_id", attrTwo);
                                    command.Parameters.AddWithValue("@date", attrThree);
                                    command.Parameters.AddWithValue("@note", attrFour);
                                }
                            }
                            if (Table.StartsWith("edit"))
                            {
                                if (Table.EndsWith("Periphery"))
                                {
                                    command.CommandText = "EditPeriphery";
                                    command.Parameters.AddWithValue("@newId", attrOne);
                                    command.Parameters.AddWithValue("@type", attrTwo);
                                    command.Parameters.AddWithValue("@model", attrThree);
                                    command.Parameters.AddWithValue("@is_working", attrFour);
                                    command.Parameters.AddWithValue("@cabinet", attrFive);
                                }
                                if (Table.EndsWith("Computers_Actions"))
                                {
                                    command.CommandText = "EditComputer_Action";
                                    command.Parameters.AddWithValue("@type", attrOne);
                                    command.Parameters.AddWithValue("@computer_id", attrTwo);
                                    command.Parameters.AddWithValue("@date", attrThree);
                                    command.Parameters.AddWithValue("@note", attrFour);
                                }
                                if (Table.EndsWith("Periphery_Actions"))
                                {
                                    command.CommandText = "AddPeriphery_Action";
                                    command.Parameters.AddWithValue("@type", attrOne);
                                    command.Parameters.AddWithValue("@periphery_id", attrTwo);
                                    command.Parameters.AddWithValue("@date", attrThree);
                                    command.Parameters.AddWithValue("@note", attrFour);
                                }
                                command.Parameters.AddWithValue("@id", Id);
                            }

                            command.CommandType = CommandType.StoredProcedure;

                            if (command.ExecuteNonQuery() == -1)
                            {
                                MessageBox.Show("Введенная запись уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return false;
                            }
                        }

                        if (Table.EndsWith("Periphery")) nav.Navigate(new Uri($@"Pages\Periphery.xaml", UriKind.RelativeOrAbsolute));
                        if (Table.EndsWith("Computers_Actions")) nav.Navigate(new Uri($@"Pages\Computers_Actions.xaml", UriKind.RelativeOrAbsolute));
                        if (Table.EndsWith("Periphery_Actions")) nav.Navigate(new Uri($@"Pages\Periphery_Actions.xaml", UriKind.RelativeOrAbsolute));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Строки не могут быть пустыми или содержать только пробелы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Метод добавления или изменения записи в таблицах с семью изменяемыми атрибутами
        /// </summary>
        /// <param name="attrOne"></param>
        /// <param name="attrTwo"></param>
        /// <param name="attrThree"></param>
        /// <param name="attrFour"></param>
        /// <param name="attrFive"></param>
        /// <param name="attrSix"></param>
        /// <param name="attrSeven"></param>
        /// <param name="nav"></param>
        /// <returns></returns>
        public static bool AddOrEdit(string attrOne, string attrTwo, string attrThree, string attrFour, string attrFive, string attrSix, string attrSeven, NavigationService nav)
        {
            if (!string.IsNullOrWhiteSpace(attrOne) && !string.IsNullOrWhiteSpace(attrSix) && !string.IsNullOrWhiteSpace(attrSeven))
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
                                if (Table.EndsWith("Computers"))
                                {
                                    command.CommandText = "AddComputer";
                                    command.Parameters.AddWithValue("@id", attrOne);
                                    command.Parameters.AddWithValue("@monitor_id", attrTwo);
                                    command.Parameters.AddWithValue("@processor", attrThree);
                                    command.Parameters.AddWithValue("@ram", float.Parse(attrFour));
                                    command.Parameters.AddWithValue("@rom", float.Parse(attrFive));
                                    command.Parameters.AddWithValue("@is_working", attrSix);
                                    command.Parameters.AddWithValue("@cabinet", attrSeven);
                                }
                            }
                            if (Table.StartsWith("edit"))
                            {
                                if (Table.EndsWith("Computers"))
                                {
                                    command.CommandText = "EditComputer";
                                    command.Parameters.AddWithValue("@newId", attrOne);
                                    command.Parameters.AddWithValue("@monitor_id", attrTwo);
                                    command.Parameters.AddWithValue("@processor", attrThree);
                                    command.Parameters.AddWithValue("@ram", float.Parse(attrFour));
                                    command.Parameters.AddWithValue("@rom", float.Parse(attrFive));
                                    command.Parameters.AddWithValue("@is_working", attrSix);
                                    command.Parameters.AddWithValue("@cabinet", attrSeven);
                                }
                                command.Parameters.AddWithValue("@id", Id);
                            }

                            command.CommandType = CommandType.StoredProcedure;

                            if (command.ExecuteNonQuery() == -1)
                            {
                                MessageBox.Show("Введенная запись уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return false;
                            }
                        }

                        if (Table.EndsWith("Computers")) nav.Navigate(new Uri($@"Pages\Computers.xaml", UriKind.RelativeOrAbsolute));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Строки не могут быть пустыми или содержать только пробелы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
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
                        if (Table.EndsWith("Cabinets")) command.CommandText = "SELECT * FROM Cabinets";
                        if (Table.EndsWith("Computers")) command.CommandText = "SELECT * FROM ComputersView";
                        if (Table.EndsWith("Computers_Actions")) command.CommandText = "SELECT * FROM Computers_ActionsView";
                        if (Table.EndsWith("Models")) command.CommandText = "SELECT * FROM ModelsView";
                        if (Table.EndsWith("Periphery")) command.CommandText = "SELECT * FROM PeripheryView";
                        if (Table.EndsWith("Periphery_Actions")) command.CommandText = "SELECT * FROM Periphery_ActionsView";
                        if (Table.EndsWith("Processors")) command.CommandText = "SELECT * FROM ProcessorsView";
                        if (Table.EndsWith("Software")) command.CommandText = "SELECT * FROM Software";
                        if (Table.EndsWith("Types")) command.CommandText = "SELECT * FROM Types";
                        if (Table.EndsWith("Versions")) command.CommandText = "SELECT * FROM VersionsView";

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
