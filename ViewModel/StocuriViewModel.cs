using Supermarket.Model;
using Supermarket.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Supermarket.ViewModel
{
    internal class StocuriViewModel : INotifyPropertyChanged
    {
        private readonly StocuriServices _stocuriService;
        private readonly ProdusServices _produsService;
        private Stocuri _selectedStoc;

        public ObservableCollection<Stocuri> Stocuri { get; set; }
        public ObservableCollection<Produs> Produse { get; set; }
        public Stocuri SelectedStoc
        {
            get => _selectedStoc;
            set
            {
                _selectedStoc = value;
                OnPropertyChanged(nameof(SelectedStoc));
                OnPropertyChanged(nameof(SelectedProdus));
            }
        }
        public Produs SelectedProdus
        {
            get => SelectedStoc?.Produs;
            set
            {
                if (SelectedProdus != null)
                {
                    SelectedStoc.Produs = value;
                    OnPropertyChanged(nameof(SelectedProdus));
                }
            }
        }

        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public StocuriViewModel()
        {
            var context = new SupermarketEntities();
            _stocuriService = new StocuriServices(context);
            _produsService = new ProdusServices(context);
            UpdateCommand = new RelayCommand(UpdateStoc);
            DeleteCommand = new RelayCommand(DeleteStoc);
            LoadData();
        }

        private void LoadData()
        {
            List<Stocuri> stocuriFromService = _stocuriService.GetAllStocuri();
            Stocuri = new ObservableCollection<Stocuri>(stocuriFromService);
            List<Produs> produseFromService = _produsService.GetAllProduse();
            Produse = new ObservableCollection<Produs>(produseFromService);
        }

        public void UpdateStoc(object parameter)
        {
            if (SelectedStoc != null)
            {
                if (SelectedStoc.Cantitate <= 0 ||
                    string.IsNullOrWhiteSpace(SelectedStoc.Unitate_masura) ||
                    SelectedStoc.Pret_achizitie <= 0 ||
                    SelectedStoc.Pret_vanzare <= 0 ||
                    SelectedStoc.Produs_id <= 0)
                {
                    // Display an error message if any required field is null or empty
                    MessageBox.Show("Please fill in all the required fields and ensure numeric values are greater than 0 and selling price is bigger than aquisition");
                    return;
                }
                if (SelectedStoc.Data_aprovizionare == null || !IsValidDateFormat(SelectedStoc.Data_aprovizionare.ToString("yyyy-MM-dd")) ||
                SelectedStoc.Data_expirare == null || !IsValidDateFormat(SelectedStoc.Data_expirare.ToString("yyyy-MM-dd")))
                {
                    // Display an error message if the date format is incorrect
                    MessageBox.Show("Please enter valid dates in YYYY-MM-DD format.");
                    return;
                }
                SelectedStoc.Produs_id = SelectedStoc.Produs.Produs_id;
                _stocuriService.UpdateStocuri(SelectedStoc);
                OnPropertyChanged(nameof(SelectedStoc));
            }
        }
        private bool IsValidDateFormat(string date)
        {
            // Regular expression for YYYY-MM-DD format
            string pattern = @"^\d{4}-\d{2}-\d{2}$";
            return Regex.IsMatch(date, pattern);
        }
        public void DeleteStoc(object parameter)
        {
            if (SelectedStoc != null)
            {
                _stocuriService.DeleteStocuri(SelectedStoc.Stoc_id);
                Stocuri = new ObservableCollection<Stocuri>(_stocuriService.GetAllStocuri());
                OnPropertyChanged(nameof(SelectedStoc));
                OnPropertyChanged(nameof(Stocuri));
            }
        }
        public void AddStoc(int cantitate, string unitateMasura, DateTime dataAprovizionare, DateTime dataExpirare, int pretAchizitie, int pretVanzare, bool active, Produs produs)
        {
            Stocuri newStoc = new Stocuri
            {
                Cantitate = cantitate,
                Unitate_masura = unitateMasura,
                Data_aprovizionare = dataAprovizionare,
                Data_expirare = dataExpirare,
                Pret_achizitie = pretAchizitie,
                Pret_vanzare = pretVanzare,
                Active = active,
                Produs_id = produs.Produs_id
            };
            _stocuriService.AddStoc(newStoc);
            Stocuri = new ObservableCollection<Stocuri>(_stocuriService.GetAllStocuri());
            OnPropertyChanged(nameof(Stocuri));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
