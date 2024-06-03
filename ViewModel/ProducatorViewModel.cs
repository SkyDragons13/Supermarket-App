using Supermarket.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Supermarket.Services;
using System.Windows;

namespace Supermarket.ViewModel
{
    internal class ProducatorViewModel : INotifyPropertyChanged
    {
        private readonly ProducatorServices _producatorService;
        private Producator _selectedProducator;

        public ObservableCollection<Producator> Producatori { get; set; }

        public Producator SelectedProducator
        {
            get => _selectedProducator;
            set
            {
                _selectedProducator = value;
                OnPropertyChanged(nameof(SelectedProducator));
            }
        }

        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public ProducatorViewModel()
        {
            var context = new SupermarketEntities();
            _producatorService = new ProducatorServices(context);
            UpdateCommand = new RelayCommand(UpdateProducator);
            DeleteCommand = new RelayCommand(DeleteProducator);
            LoadData();
        }

        private void LoadData()
        {
            Producatori = new ObservableCollection<Producator>(_producatorService.GetAllProducatoriSQL());
        }

        public void UpdateProducator(object parameter)
        {
            if (SelectedProducator != null)
            {
                if (string.IsNullOrWhiteSpace(SelectedProducator.Producator_nume) ||
                    string.IsNullOrWhiteSpace(SelectedProducator.Tara_origine))
                {
                    // Display an error message if any required field is empty
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }
                _producatorService.UpdateProducator(SelectedProducator);
                LoadData();
                OnPropertyChanged(nameof(SelectedProducator));
            }
        }

        public void DeleteProducator(object parameter)
        {
            if (SelectedProducator != null)
            {
                _producatorService.DeleteProducator(SelectedProducator.Producator_id);
                LoadData();
                OnPropertyChanged(nameof(Producatori));
                OnPropertyChanged(nameof(SelectedProducator));
            }
        }

        public void AddProducator(string producatorNume, string taraOrigine, bool active)
        {
            Producator newProducator = new Producator
            {
                Producator_nume = producatorNume,
                Tara_origine = taraOrigine,
                Active = active
            };

            // Call the service method to add the new producer
            _producatorService.AddProducator(newProducator);

            // Refresh the list of producers
            LoadData();
            OnPropertyChanged(nameof(Producatori));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
