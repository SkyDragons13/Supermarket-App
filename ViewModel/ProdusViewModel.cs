using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Supermarket.Model;
using Supermarket.Services;

namespace Supermarket.ViewModel
{
    internal class ProdusViewModel : INotifyPropertyChanged
    {
        private readonly ProdusServices _produsService;
        private readonly CategorieServices _categorieService;
        private readonly ProducatorServices _producatorService;
        private Produs _selectedProdus;

        public ObservableCollection<Produs> Produse { get; set; }
        public ObservableCollection<Categorie> Categorie { get; set; }
        public ObservableCollection<Producator> Producator { get; set; }

        public Produs SelectedProdus
        {
            get => _selectedProdus;
            set
            {
                _selectedProdus = value;
                OnPropertyChanged(nameof(SelectedProdus));
                OnPropertyChanged(nameof(SelectedCategory));
                OnPropertyChanged(nameof(SelectedProducer));
            }
        }

        public Categorie SelectedCategory
        {
            get => SelectedProdus?.Categorie;
            set
            {
                if (SelectedProdus != null)
                {
                    SelectedProdus.Categorie = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                }
            }
        }

        public Producator SelectedProducer
        {
            get => SelectedProdus?.Producator;
            set
            {
                if (SelectedProdus != null)
                {
                    SelectedProdus.Producator = value;
                    OnPropertyChanged(nameof(SelectedProducer));
                }
            }
        }

        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        SupermarketEntities context;
        public ProdusViewModel()
        {
            context = new SupermarketEntities();
            _produsService = new ProdusServices(context);
            _categorieService = new CategorieServices(context); // Initialize service for categories
            _producatorService = new ProducatorServices(context); // Initialize service for producers
            UpdateCommand = new RelayCommand(UpdateProdus);
            DeleteCommand = new RelayCommand(DeleteProdus);
            LoadData();
        }

        private void LoadData()
        {
            List<Produs> produseFromService = _produsService.GetAllProduse();
            Produse = new ObservableCollection<Produs>(produseFromService);
            Categorie = new ObservableCollection<Categorie>(_categorieService.GetAllCategories());
            Producator = new ObservableCollection<Producator>(_producatorService.GetAllProducatori());
        }

        public void UpdateProdus(object parameter)
        {
            if (SelectedProdus != null)
            {
                if (Produse.Any(p => p.Cod_bare == SelectedProdus.Cod_bare))
                {
                    MessageBox.Show("Cod_bare must be unique. Please enter a different value.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(SelectedProdus.Produs_nume) ||
            SelectedProdus.Cod_bare <= 0 ||
            SelectedProdus.Categorie_id <= 0 ||
            SelectedProdus.Producator_id <= 0)
                {
                    // Display an error message if any required field is null or empty
                    MessageBox.Show("Please fill in all the required fields and ensure numeric values are greater than 0(cod_bare is a number)");
                    return;
                }
                SelectedProdus.Categorie_id = SelectedProdus.Categorie.Categorie_id;
                SelectedProdus.Producator_id = SelectedProdus.Producator.Producator_id;
                _produsService.UpdateProdus(SelectedProdus);
            }
        }

        public void DeleteProdus(object parameter)
        {
            if (SelectedProdus != null)
            {
                _produsService.DeleteProdus(SelectedProdus.Produs_id);
                Produse = new ObservableCollection<Produs>(_produsService.GetAllProduse());
                OnPropertyChanged(nameof(Produse));
                OnPropertyChanged(nameof(SelectedProdus));
            }
        }
        public void AddProdus(string produsNume, int codBare, Categorie categorie, Producator producator, bool active, int categorie_id, int producator_id)
        {
            Produs newProdus = new Produs
            {
                Produs_nume = produsNume,
                Cod_bare = codBare,
                Categorie = categorie,
                Producator = producator,
                Active = active,
                Categorie_id=categorie_id,
                Producator_id = producator_id,
            };
            if (Produse.Any(p => p.Cod_bare == codBare))
            {
                MessageBox.Show("Cod_bare must be unique. Please enter a different value.");
                return;
            }
            // Add the new produs using the service
            _produsService.AddProdus(newProdus);
            Produse = new ObservableCollection<Produs>(_produsService.GetAllProduse());
            OnPropertyChanged(nameof(Produse));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
