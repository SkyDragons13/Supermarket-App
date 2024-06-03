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
using Supermarket.ViewModel;

namespace Supermarket.View
{
    /// <summary>
    /// Interaction logic for ProducatorView.xaml
    /// </summary>
    public partial class ProducatorView : Page
    {
        public ProducatorView()
        {
            InitializeComponent();
            DataContext = new ProducatorViewModel();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string producatorNume = txtNume.Text;
            string taraOrigine = txtTara.Text;
            if (string.IsNullOrWhiteSpace(producatorNume) || string.IsNullOrWhiteSpace(taraOrigine))
            {
                // Handle empty input
                MessageBox.Show("Please fill in all the fields.");
                return;
            }
            bool active = checkActive.IsChecked ?? false; 

            ((ProducatorViewModel)DataContext).AddProducator(producatorNume, taraOrigine, active);
        }

    }
}
