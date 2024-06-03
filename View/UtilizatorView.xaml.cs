using System;
using System.Collections.Generic;
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
using Supermarket.Model;
using Supermarket.ViewModel;

namespace Supermarket.View
{
    /// <summary>
    /// Interaction logic for UtilizatorView.xaml
    /// </summary>
    public partial class UtilizatorView : Page
    {
        public UtilizatorView()
        {
            InitializeComponent();
            DataContext=new UtilizatorViewModel();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string nume = txtNume.Text;
            string parola = txtParola.Text;
            string tip = txtTip.Text;
            bool active = checkActive.IsChecked ?? false;
            if (string.IsNullOrWhiteSpace(nume) || string.IsNullOrWhiteSpace(parola) || string.IsNullOrWhiteSpace(tip))
            {
                MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Exit the method
            }
            ((UtilizatorViewModel)DataContext).AddUtilizator(nume, parola, tip, active);
        }
    }
}
