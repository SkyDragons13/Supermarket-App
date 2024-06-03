using Supermarket.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Supermarket.ViewModel
{
    public class CasierViewModel
    {
        private readonly Frame _frame;
        public ICommand ShowSearchCommand { get; }
        public ICommand ShowBonCommand { get; }
       
        public CasierViewModel(Frame frame)
        {
            _frame = frame;
            ShowSearchCommand = new RelayCommand(ShowProducts);
            ShowBonCommand = new RelayCommand(ShowBon);
        }

        // Command implementations
        private void ShowProducts(object parameter)
        {
            _frame.Navigate(new Produse());
        }

        private void ShowBon(object parameter)
        {
            _frame.Navigate(new AddBonView());
        }

    }
}
