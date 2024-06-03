using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Supermarket.ViewModels
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        private string _selectedType;

        public ObservableCollection<string> SearchTypes { get; } = new ObservableCollection<string>
        {
            "Producator",
            "Categorie",
            "Cod de bare",
            "Data expirare",
            "Nume"
        };

        public string SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertyChanged(nameof(SelectedType));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
