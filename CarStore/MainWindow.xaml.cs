using CarStore.Viewes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CarStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Controller controller;
        internal SQLController sqlDB;

        public MainWindow()
        {
            InitializeComponent();
            controller = new Controller();
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            if (clickedButton.Name == "button_Search")
            {
                if (Row_Search.ActualHeight > 0)
                {
                    Row_Search.Height = new GridLength(0);
                }
                else
                {
                    Row_Search.Height = new GridLength(40);
                }
            }
            if(clickedButton.Name == "button_Home")
            {
                list_Cars.Visibility = Visibility.Visible;
                list_Orders.Visibility = Visibility.Hidden;
                grid_Settings.Visibility = Visibility.Hidden;
            }
            if (clickedButton.Name == "button_Docs")
            {
                list_Cars.Visibility = Visibility.Hidden;
                list_Orders.Visibility = Visibility.Visible;
                grid_Settings.Visibility = Visibility.Hidden;
            }
            if (clickedButton.Name == "button_Settings")
            {
                list_Cars.Visibility = Visibility.Hidden;
                list_Orders.Visibility = Visibility.Hidden;
                grid_Settings.Visibility = Visibility.Visible;
            }
            if (clickedButton.Name == "bt_Connect")
            {
                sqlDB = new SQLController();
                Bt_Connect();
            }
        }

        private void Bt_Connect()
        {
            if (sqlDB.connection == null || sqlDB.connection.State == ConnectionState.Closed)
            {
                if (sqlDB.Connect())
                {
                    
                    bt_Connect.Content = "Close DB";
                    bt_Connect.Background = Brushes.Green;
                    sqlDB.Update(ref controller);
                    UpdateStackView();
                }
                return;
            }
            else if (sqlDB.connection.State == ConnectionState.Open)
            {
                if (sqlDB.Disconnect())
                {
                    bt_Connect.Content = "Connect to DB";
                    bt_Connect.Background = Brushes.Red;
                    controller.Clear();
                    UpdateStackView();
                }
                return;
            }

        }

        private void Bt_add_Click(object sender, RoutedEventArgs e)
        {
            if (sqlDB.connection == null || sqlDB.connection.State == ConnectionState.Closed)
            {
                MessageBox.Show("DataBase is disconnected");
                return;
            }
            AddToDb Window = new AddToDb(sqlDB.connection);
            Window.ShowDialog();
            sqlDB.Update(ref controller);
        }

        private void UpdateStackView()
        {
            if (controller.ReturnAll().Count == 0)
            {
                list_Cars.Children.Clear();
            }
            foreach (var element in controller.ReturnAll())
            {
                list_Cars.Children.Add(new StackElement(element.CarName, element.CarInfo, element.Price, element.Image).MainGrid);
            }
        }

        private void Tbx_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sqlDB != null)
            {
                if (!tbx_Search.Text.Equals(""))
                {
                    foreach (var element in list_Cars.Children)
                    {
                        (element as Grid).Children.Clear();
                    }
                    list_Cars.Children.Clear();
                    foreach (var element in controller.FindItem(tbx_Search.Text))
                    {
                        list_Cars.Children.Add(new StackElement(element.CarName, element.CarInfo, element.Price, element.Image).MainGrid);
                    }
                }
                else
                {
                    foreach (var element in list_Cars.Children)
                    {
                        (element as Grid).Children.Clear();
                    }
                    list_Cars.Children.Clear();
                    UpdateStackView();
                }
            }
        }

    }

}
