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
    /// Interaction logic for CategorieView.xaml
    /// </summary>
    public partial class CategorieView : Page
    {
        public CategorieView()
        {
            InitializeComponent();
            DataContext=new CategorieViewModel();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Collect data from the text and combo boxes
            string categorieNume = txtNume.Text;
            if(string.IsNullOrWhiteSpace(categorieNume) ) 
            {
                MessageBox.Show("Please fill in all the fields.");
                return;
            }
            bool active = checkActive.IsChecked ?? false;
            // Call the AddProdus method from the view model
            ((CategorieViewModel)DataContext).AddCategorie(categorieNume,active);
        }
    }
}
