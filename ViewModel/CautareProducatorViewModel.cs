using Supermarket.Model;
using Supermarket.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Supermarket.ViewModel
{
    public class CautareProducatorViewModel : INotifyPropertyChanged
    {
        private readonly ProdusServices _produsService;
        private readonly CategorieServices _categorieService;
        private readonly ProducatorServices _producatorService;

        public ObservableCollection<Produs> Produse { get; set; }
        public ObservableCollection<Categorie> Categorii { get; set; }
        public ObservableCollection<Producator> Producatori { get; set; }
        public ObservableCollection<Produs> AllProduse { get; set; }
        public ObservableCollection<Categorie> AllCategorii { get; set; }

        private Producator _selectedProducator;
        public Producator SelectedProducator
        {
            get => _selectedProducator;
            set
            {
                _selectedProducator = value;
                OnPropertyChanged(nameof(SelectedProducator));
                LoadCategorizedProducts();
            }
        }
        private Categorie _selectedCategorie;
        public Categorie SelectedCategorie
        {
            get => _selectedCategorie;
            set
            {
                _selectedCategorie = value;
                OnPropertyChanged(nameof(SelectedCategorie));
                LoadProduseByCategorie();
            }
        }

        public CautareProducatorViewModel()
        {
            var context = new SupermarketEntities();
            _producatorService = new ProducatorServices(context);
            _produsService = new ProdusServices(context);
            _categorieService = new CategorieServices(context);
            LoadData();
        }
        private void LoadCategorizedProducts()
        {
            if (SelectedProducator != null)
            {
                ObservableCollection<Produs> tempProduse = new ObservableCollection<Produs>( AllProduse.Where(p => p.Producator_id == SelectedProducator.Producator_id).ToList());
                Categorii = new ObservableCollection<Categorie>();
                var uniqueCategoryIds = tempProduse.Select(p => p.Categorie_id).Distinct();

                foreach (var categoryId in uniqueCategoryIds)
                {
                    var category = AllCategorii.FirstOrDefault(c => c.Categorie_id == categoryId);
                    if (category != null)
                    {
                        Categorii.Add(category);
                    }
                }
                OnPropertyChanged(nameof(Categorii));
            }
        }
        private void LoadProduseByCategorie()
        {
            if (SelectedCategorie != null)
            {
                Produse = new ObservableCollection<Produs>(
                    AllProduse.Where(p => p.Categorie_id == SelectedCategorie.Categorie_id)
                              .ToList());
                OnPropertyChanged(nameof(Produse));
            }
        }

        private void LoadData()
        {
            Producatori = new ObservableCollection<Producator>(_producatorService.GetAllProducatori());
            AllCategorii= new ObservableCollection<Categorie>(_categorieService.GetAllCategories());
            AllProduse = new ObservableCollection<Produs>(_produsService.GetAllProduse());
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
