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
    public class AddBonViewModel : INotifyPropertyChanged
    {
        private readonly UtilizatorServices _utilizatoriService;
        private readonly ProdusServices _produsService;
        private readonly BonuriServices _bonuriService;
        private readonly StocuriServices _stocuriService;

        private ObservableCollection<AddedProdus> _addedProduse;

        public ObservableCollection<Utilizator> Utilizatori { get; set; }
        public ObservableCollection<Stocuri> Stocuri { get; set; }
        public ObservableCollection<Produs> Produse { get; set; }
        public ICommand AddButtonCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand AddProdus { get; private set; }
        public ICommand RemoveProdusCommand { get; private set; }

        private Utilizator _selectedUtilizator;
        private Produs _selectedProdus;
        private int _cantitate;
        private AddedProdus _selectedAddedProdus;
        public AddedProdus SelectedAddedProdus
        {
            get=> _selectedAddedProdus;
            set
            {
                _selectedAddedProdus= value;
                OnPropertyChanged(nameof(SelectedAddedProdus));
            }
        }
        public ObservableCollection<AddedProdus> AddedProduse
        {
            get => _addedProduse;
            set
            {
                _addedProduse = value;
                OnPropertyChanged(nameof(AddedProduse));
            }
        }
        public int Cantitate
        {
            get => _cantitate;
            set
            {
                _cantitate = value;
                OnPropertyChanged(nameof(Cantitate));
            }
        }
        public Utilizator SelectedUtilizator
        {
            get => _selectedUtilizator;
            set
            {
                _selectedUtilizator = value;
                OnPropertyChanged(nameof(SelectedUtilizator));
            }

        }
        public Produs SelectedProdus
        {
            get => _selectedProdus;
            set
            {
                _selectedProdus = value;
                OnPropertyChanged(nameof(SelectedProdus));
            }
        }
        public AddBonViewModel()
        {
            var context = new SupermarketEntities();
            _utilizatoriService = new UtilizatorServices(context);
            _produsService = new ProdusServices(context);
            _bonuriService = new BonuriServices(context);
            _stocuriService = new StocuriServices(context);

            AddButtonCommand = new RelayCommand(AddButtonClicked);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            AddProdus = new RelayCommand(AddNewProdus);
            RemoveProdusCommand = new RelayCommand(RemoveAddedProdus);
            LoadData();
        }
        private void LoadData()
        {
            Utilizatori = new ObservableCollection<Utilizator>(_utilizatoriService.GetAllCasieri());
            Produse = new ObservableCollection<Produs>(_produsService.GetAllProduse().Where(p=>p.Active));
            Stocuri = new ObservableCollection<Stocuri>(_stocuriService.GetAllStocuri());
            AddedProduse = new ObservableCollection<AddedProdus>();
        }

        private void RemoveAddedProdus(object parameter)
        {
            if (SelectedAddedProdus != null )
            {
                var activeStoc=Stocuri.FirstOrDefault(s=>s.Produs_id==SelectedAddedProdus.id && s.Active);
                activeStoc.Cantitate += SelectedAddedProdus.Cantitate;
                AddedProduse.Remove(SelectedAddedProdus);
                OnPropertyChanged(nameof(AddedProduse));
            }
        }
        private void AddButtonClicked(object parameter)
        {
            List<int> stocID = new List<int>();
            foreach (var newAddedProdus in AddedProduse)
            {
                // Filter active stock items based on the product ID
                var activeStockItems = Stocuri.FirstOrDefault(s => s.Produs_id == newAddedProdus.id && s.Active);
                if (activeStockItems.Cantitate == 0)
                    activeStockItems.Active = false;
                stocID.Add(activeStockItems.Stoc_id);
            }
            _bonuriService.AddBonuriWithStocuri(stocID, AddedProduse.ToList(), SelectedUtilizator.Nume);
            AddedProduse.Clear();
        }
        private void AddNewProdus(object parameter)
        {
            if (SelectedProdus == null)
            {
                MessageBox.Show("Please select a product.");
                return;
            }

            // Check if Cantitate is not a valid positive integer
            if (Cantitate <= 0 || !int.TryParse(Cantitate.ToString(), out _))
            {
                MessageBox.Show("Please enter a valid positive integer value for quantity.");
                return;
            }

            // Find the active stock for the selected product
            Stocuri activeStock = SelectedProdus.Stocuri.FirstOrDefault(s => s.Active);
            if (activeStock == null)
            {
                MessageBox.Show("No active stock found for the selected product.");
                return;
            }

            // Check if the entered quantity exceeds the available stock
            if (Cantitate > activeStock.Cantitate)
            {
                MessageBox.Show("Insufficient stock for the selected product. Available :"+ activeStock.Cantitate);
                return;
            }

            // Create a new AddedProdus object
            AddedProdus newAddedProdus = new AddedProdus
            {
                Produs_nume = SelectedProdus.Produs_nume,
                id = SelectedProdus.Produs_id,
                Cod_bare = SelectedProdus.Cod_bare,
                Cantitate = Cantitate,
                Pret = activeStock.Pret_vanzare,
                PretTotal = activeStock.Pret_vanzare * Cantitate
            };

            // Add the new AddedProdus to the collection
            activeStock.Cantitate -= newAddedProdus.Cantitate;
            AddedProduse.Add(newAddedProdus);
            OnPropertyChanged(nameof(AddedProduse));
        }


        private void Update(object parameter)
        {
            // Update command logic
        }

        private void Delete(object parameter)
        {
            // Delete command logic
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}