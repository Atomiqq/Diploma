﻿using System;
using System.Collections.Generic;
using System.Data;
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

namespace Accounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для Periphery.xaml
    /// </summary>
    public partial class Periphery : Page
    {
        DataTable dt;

        public Periphery()
        {
            InitializeComponent();

            App.Table = "Periphery";

            dt = App.Fill();
            dg.ItemsSource = dt.DefaultView;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            App.Table = "addPeriphery";

            NavigationService.Navigate(new Uri(@"Pages\AttrFive.xaml", UriKind.RelativeOrAbsolute));
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItem != null)
            {
                var cellInfo = dg.SelectedCells[0];
                App.Id = (cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text;
                App.Table = "editPeriphery";

                NavigationService.Navigate(new Uri(@"Pages\AttrFive.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            App.Delete(dg, dt);
        }
    }
}
