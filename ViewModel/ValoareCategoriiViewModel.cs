using Supermarket.Model;
using Supermarket.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Supermarket.ViewModel
{
    public class ValoareCategoriiViewModel : INotifyPropertyChanged
    {
        private readonly StocuriServices _stocuriService;
        private readonly CategorieServices _categoriiService;

        public ObservableCollection<CategorieValoarePair> CategoriiValoarePairs { get; set; }

        public ValoareCategoriiViewModel()
        {
            var context = new SupermarketEntities();
            _stocuriService = new StocuriServices(context);
            _categoriiService = new CategorieServices(context);
            LoadData();
        }

        private void LoadData()
        {
            // Load all categories
            var categorii = new ObservableCollection<Categorie>(_categoriiService.GetAllCategories());

            // Calculate the value for each category
            CategoriiValoarePairs = new ObservableCollection<CategorieValoarePair>();
            foreach (var categorie in categorii)
            {
                int valoareCategorie = CalculateValoareCategorie(categorie.Categorie_id);
                CategoriiValoarePairs.Add(new CategorieValoarePair { Categorie = categorie, Valoare = valoareCategorie });
            }
        }

        private int CalculateValoareCategorie(int categorieId)
        {
            // Get all stocks
            var allStocuri = _stocuriService.GetAllStocuri();

            // Filter stocks by the specified category ID
            var stocuriInCategorie = allStocuri.Where(stoc => stoc.Produs.Categorie_id == categorieId);

            // Calculate the total value for the category
            int totalValoare = 0;
            foreach (var stoc in stocuriInCategorie)
            {
                // Consider the current selling price (either regular or reduced)
                int pretVanzare = stoc.Pret_vanzare;
                totalValoare += stoc.Cantitate * pretVanzare;
            }

            return totalValoare;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class CategorieValoarePair
    {
        public Categorie Categorie { get; set; }
        public int Valoare { get; set; }
    }
}
