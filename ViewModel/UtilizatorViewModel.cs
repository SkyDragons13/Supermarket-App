using Supermarket.Model;
using Supermarket.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Supermarket.ViewModel
{
    public class UtilizatorViewModel : INotifyPropertyChanged
    {
        private readonly UtilizatorServices _utilizatorService;
        public ObservableCollection<Utilizator> Utilizatori { get; set; }
        private Utilizator _selectedUtilizator;

        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public UtilizatorViewModel()
        {
            var context = new SupermarketEntities();
            _utilizatorService = new UtilizatorServices(context);
            UpdateCommand = new RelayCommand(UpdateUtilizator);
            DeleteCommand=new RelayCommand(DeleteUtilizator);
            LoadData();
        }


        public Utilizator SelectedUtilizator {
            get => _selectedUtilizator;
            set
            {
                _selectedUtilizator = value;
                OnPropertyChanged(nameof(SelectedUtilizator));
            }
        }


        public void LoadData()
        {
            List<Utilizator> utilizatoriFromService = _utilizatorService.GetAllUtilizatori();
            Utilizatori = new ObservableCollection<Utilizator>(utilizatoriFromService);
        }

        public void UpdateUtilizator(object parameter)
        {
            if (SelectedUtilizator != null)
            {
                if (string.IsNullOrWhiteSpace(SelectedUtilizator.Nume) ||
                    string.IsNullOrWhiteSpace(SelectedUtilizator.Parola) ||
                    string.IsNullOrWhiteSpace(SelectedUtilizator.Tip))
                {
                    // Display an error message if any field is empty
                    MessageBox.Show("Please fill in all the fields.");
                    return;
                }
                _utilizatorService.UpdateUtilizator(SelectedUtilizator);
                LoadData();
            }
        }

        public void DeleteUtilizator(object parameter)
        {
            if (SelectedUtilizator != null)
            {
                _utilizatorService.DeleteUtilizator(SelectedUtilizator.Utilizator_id);
                LoadData();
                OnPropertyChanged(nameof(SelectedUtilizator));
                OnPropertyChanged(nameof(Utilizatori));
            }
        }
        public void AddUtilizator(string nume, string parola, string tip, bool active)
        {
            Utilizator newUtilizator = new Utilizator
            {
                Nume = nume,
                Parola = parola,
                Tip = tip,
                Active = active
            };

            // Call the AddUtilizator method from the service
            _utilizatorService.AddUtilizator(newUtilizator);
            LoadData();
            OnPropertyChanged(nameof(Utilizatori));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

