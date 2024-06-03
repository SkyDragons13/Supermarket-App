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
    /// Interaction logic for StocuriView.xaml
    /// </summary>
    public partial class StocuriView : Page
    {
        public StocuriView()
        {
            InitializeComponent();
            DataContext = new StocuriViewModel();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
           stackAchizite.Visibility = Visibility.Visible;
           stackVanzare.Visibility = Visibility.Collapsed;
           btnAdd.Visibility = Visibility.Visible;
           
        }
        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            // Check if all necessary fields are filled
            if (!string.IsNullOrWhiteSpace(txtCantitate.Text) &&
                !string.IsNullOrWhiteSpace(txtUnitate.Text) &&
                !string.IsNullOrWhiteSpace(txtData.Text) &&
                !string.IsNullOrWhiteSpace(txtDataF.Text))
            {
                // Parse necessary fields
                if (int.TryParse(txtCantitate.Text, out int cantitate) &&
                    DateTime.TryParse(txtData.Text, out DateTime dataAprovizionare) &&
                    DateTime.TryParse(txtDataF.Text, out DateTime dataExpirare))
                {
                    int pretAchizitie = 0;
                    int pretVanzare = 0;
                    if (stackAchizite.Visibility == Visibility.Visible && int.TryParse(txtAchizitie.Text, out int achizitie))
                    {
                        pretAchizitie = achizitie;
                        pretVanzare = 2 * pretAchizitie;
                    }
                    else if (stackVanzare.Visibility == Visibility.Visible && int.TryParse(txtVanzare.Text, out int vanzare))
                    {
                        pretVanzare = vanzare;
                    }
                    Produs produs = (Produs)comboProdus.SelectedItem;

                    // Add the new stock item to the ViewModel by passing the necessary information
                    ((StocuriViewModel)DataContext).AddStoc(cantitate, txtUnitate.Text, dataAprovizionare, dataExpirare, pretAchizitie, pretVanzare, checkActive.IsChecked ?? false, produs);
                }
                else
                {
                    MessageBox.Show("Invalid input for numeric fields or date fields.");
                }
            }
            else
            {
                MessageBox.Show("Please fill in all required fields.");
            }
            stackAchizite.Visibility = Visibility.Collapsed;
            stackVanzare.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Collapsed;
            comboProdus.SelectedItem = null;
            txtAchizitie.Text = string.Empty;
            txtCantitate.Text = string.Empty;
            txtData.Text = string.Empty;
            txtDataF.Text = string.Empty;
            txtUnitate.Text = string.Empty;
        }


    }
}
