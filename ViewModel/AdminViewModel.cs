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
    public class AdminViewModel
    {
        private readonly Frame _frame;
        public ICommand ShowProductsCommand { get; }
        public ICommand ShowProducersCommand { get; }
        public ICommand ShowCategoriesCommand { get; }
        public ICommand ShowStocksCommand { get; }
        public ICommand ShowUsersCommand { get; }
        public ICommand ShowReceiptsCommand { get; }
        public ICommand ProdSearchCommand {  get; }
        public ICommand CategoryValCommand { get; }
        public ICommand ShowIncasariCommand { get; }
        public ICommand ShowBonMareCommand { get; }
        public ICommand ShowAddReceiptsCommand { get; }
        public AdminViewModel(Frame frame)
        {
            _frame = frame;
            ShowProductsCommand = new RelayCommand(ShowProducts);
            ShowProducersCommand = new RelayCommand(ShowProducers);
            ShowCategoriesCommand = new RelayCommand(ShowCategories);
            ShowStocksCommand = new RelayCommand(ShowStocks);
            ShowUsersCommand = new RelayCommand(ShowUsers);
            ShowReceiptsCommand = new RelayCommand(ShowReceipts);
            ProdSearchCommand = new RelayCommand(ShowProdSearch);
            CategoryValCommand = new RelayCommand(ShowCategoryVal);
            ShowIncasariCommand=new RelayCommand(ShowIncasari);
            ShowBonMareCommand = new RelayCommand(BonMare);
            ShowAddReceiptsCommand = new RelayCommand(AddBon);
        }

        // Command implementations
        private void ShowProducts(object parameter)
        {
            _frame.Navigate(new Produse());
        }

        private void ShowProducers(object parameter)
        {
            _frame.Navigate(new ProducatorView());
        }

        private void ShowCategories(object parameter)
        {
            _frame.Navigate(new CategorieView());
        }

        private void ShowStocks(object parameter)
        {
            _frame.Navigate(new StocuriView());
        }
        private void ShowUsers(object parameter)
        {
            _frame.Navigate(new UtilizatorView());
        }

        private void ShowReceipts(object parameter)
        {
            _frame.Navigate(new BonuriView());
        }
        private void ShowProdSearch(object parameter)
        {
            _frame.Navigate(new CautareProducatorView());
        }
        private void ShowCategoryVal(object parameter)
        {
            _frame.Navigate(new ValoareCategoriiView());
        }
        private void ShowIncasari(object parameter)
        {
            _frame.Navigate(new IncasariView());
        }
        private void BonMare(object parameter)
        {
            _frame.Navigate(new BonMareView());
        }
        private void AddBon(object parameter)
        {
            _frame.Navigate(new AddBonView());
        }
    }
}