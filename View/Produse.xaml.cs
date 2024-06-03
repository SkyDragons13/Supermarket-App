using Supermarket.Model;
using Supermarket.ViewModel;
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

namespace Supermarket.View
{
    /// <summary>
    /// Interaction logic for Produse.xaml
    /// </summary>
    public partial class Produse : Page
    {
        public Produse()
        {
            InitializeComponent();
            DataContext = new ProdusViewModel();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string produsNume = txtNume.Text;
            int codBare;
            if (string.IsNullOrWhiteSpace(produsNume) || string.IsNullOrWhiteSpace(txtCod.Text))
            {
                // Handle empty input
                MessageBox.Show("Please fill in all the fields.");
                return;
            }
            if (!int.TryParse(txtCod.Text, out codBare))
            {
                // Handle invalid input for Cod_bare
                MessageBox.Show("Invalid Cod_bare. Please enter a valid integer.");
                return;
            }
            if (comboCategorie.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.");
                return;
            }
            if (comboProducator.SelectedItem == null)
            {
                MessageBox.Show("Please select a producer.");
                return;
            }
            Categorie categorie = (Categorie)comboCategorie.SelectedItem;
            Producator producator = (Producator)comboProducator.SelectedItem;

            bool active = checkActive.IsChecked ?? false;
            int categorie_id = categorie.Categorie_id;
            int producator_id = producator.Producator_id;

            // Call the AddProdus method from the view model
            ((ProdusViewModel)DataContext).AddProdus(produsNume, codBare, categorie, producator, active, categorie_id, producator_id);
        }

    }
}
