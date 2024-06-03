using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Data.Entity;
using Supermarket.Model;
using System.Windows.Navigation;
using System.Windows.Controls;
using Supermarket.View;

namespace Supermarket.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoginCommand { get; }
        public ICommand AdminCommand { get; }

        public MainWindowViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }


        private void Login(object parameter)
        {
            string username = (System.Windows.Application.Current.MainWindow.FindName("txtUsername") as System.Windows.Controls.TextBox)?.Text;
            string password = (System.Windows.Application.Current.MainWindow.FindName("txtPassword") as System.Windows.Controls.PasswordBox)?.Password;
            // Query the database to check if username and password match
            using (var context = new SupermarketEntities())
            {
                var user = context.Utilizator.FirstOrDefault(u => u.Nume == username && u.Parola == password && u.Active == true);
                if (user != null)
                {
                    if (user.Tip == "Admin")
                    {
                        switchAdmin();
                    }

                    else
                        switchCasier();
                }
                else
                {
                    MessageBox.Show("Invalid username or password!");
                    // Handle failed login attempt here, such as showing an error message to the user
                }
                context.Dispose();
            }
        }
        private void switchAdmin()
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            //mainWindow.ContentGrid.Visibility = Visibility.Collapsed;
            mainWindow.MainFrame.Navigate(new AdminView());
         }
        private void switchCasier()
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new CasierView());
        }

    }
}
