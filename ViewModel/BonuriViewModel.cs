using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Supermarket.Model;
using Supermarket.Services;

namespace Supermarket.ViewModel
{
    public class BonuriViewModel : INotifyPropertyChanged
    {
        private readonly BonuriServices _bonuriService;
        private readonly BonuriStocuriServices _bonuriStocuriService;

        public ObservableCollection<Bonuri> Bonuri { get; set; }
        public ObservableCollection<AddedProdusView> AddedProduse { get; set; }

        private Bonuri _selectedBon;

        public Bonuri SelectedBon
        {
            get => _selectedBon;
            set
            {
                _selectedBon = value;
                OnPropertyChanged(nameof(SelectedBon));
                LoadAddedProduse();
            }
        }

        public BonuriViewModel()
        {
            var context = new SupermarketEntities();
            _bonuriService = new BonuriServices(context);
            _bonuriStocuriService = new BonuriStocuriServices(context);

            LoadData();
        }

        private void LoadData()
        {
            Bonuri = new ObservableCollection<Bonuri>(_bonuriService.GetAllBonuri());
            AddedProduse = new ObservableCollection<AddedProdusView>();
        }

        private void LoadAddedProduse()
        {
            if (SelectedBon != null)
            {
                // Get the added products associated with the selected bon
                AddedProduse = _bonuriStocuriService.GetProduse(SelectedBon.Bon_id);
                OnPropertyChanged(nameof(AddedProduse));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
   
}
