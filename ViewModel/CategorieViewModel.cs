using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Supermarket.Model;
using Supermarket.Services;

namespace Supermarket.ViewModel
{
    internal class CategorieViewModel : INotifyPropertyChanged
    {
        private readonly CategorieServices _categorieService;
        private Categorie _selectedCategorie;

        public ObservableCollection<Categorie> Categorii { get; set; }
        public Categorie SelectedCategorie
        {
            get => _selectedCategorie;
            set
            {
                _selectedCategorie = value;
                OnPropertyChanged(nameof(SelectedCategorie));
            }
        }

        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public CategorieViewModel()
        {
            var context = new SupermarketEntities();
            _categorieService = new CategorieServices(context);
            UpdateCommand = new RelayCommand(UpdateCategorie);
            DeleteCommand = new RelayCommand(DeleteCategorie);
            LoadData();
        }

        private void LoadData()
        {
            Categorii = new ObservableCollection<Categorie>(_categorieService.GetAllCategoriesSQL());
        }

        public void UpdateCategorie(object parameter)
        {
            if (SelectedCategorie != null)
            {
                if (string.IsNullOrWhiteSpace(SelectedCategorie.Categorie_nume))
                {
                    // Display an error message if the category name is empty
                    MessageBox.Show("Please enter a category name.");
                    return;
                }
                _categorieService.UpdateCategorie(SelectedCategorie);
                LoadData();
            }
        }

        public void DeleteCategorie(object parameter)
        {
            if (SelectedCategorie != null)
            {
                _categorieService.DeleteCategorie(SelectedCategorie.Categorie_id);
                LoadData();
                OnPropertyChanged(nameof(SelectedCategorie));
                OnPropertyChanged(nameof(Categorii));
            }
        }
        public void AddCategorie(string nume, bool active)
        {
            Categorie categorie = new Categorie { 
                Categorie_nume = nume,
                Active = active
            };
            _categorieService.AddCategorie(categorie);
            LoadData();
            OnPropertyChanged(nameof(Categorii));
             
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
