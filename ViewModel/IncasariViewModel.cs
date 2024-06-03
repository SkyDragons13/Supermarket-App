using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Supermarket.Model;
using Supermarket.Services;

namespace Supermarket.ViewModel
{
    public class IncasariViewModel : INotifyPropertyChanged
    {
        private readonly UtilizatorServices _utilizatoriService;
        private readonly BonuriServices _bonuriService;

        public ObservableCollection<Utilizator> Utilizatori { get; set; }
        public ObservableCollection<Incasari> SumaIncasata { get; set; }
        public List<Bonuri> Bonuri { get; set; }

        private Utilizator _selectedUtilizator;
        public Utilizator SelectedUtilizator
        {
            get => _selectedUtilizator;
            set
            {
                _selectedUtilizator = value;
                OnPropertyChanged(nameof(SelectedUtilizator));
                LoadData();
            }
        }
        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                LoadData(); 
            }
        }

        public IncasariViewModel()
        {
            var context = new SupermarketEntities();
            _utilizatoriService = new UtilizatorServices(context);
            _bonuriService = new BonuriServices(context);

            Utilizatori = new ObservableCollection<Utilizator>(_utilizatoriService.GetAllCasieri());
            SumaIncasata = new ObservableCollection<Incasari>();
            Bonuri=new List<Bonuri>(_bonuriService.GetAllBonuri());
        }

        private void LoadData()
        {
            SumaIncasata.Clear();
            if (SelectedUtilizator != null && SelectedDate!=null)
            {
                var bonuriCasier = Bonuri.Where(b => b.Casier == SelectedUtilizator.Nume);
                if (bonuriCasier != null)
                {
                    foreach (var bon in bonuriCasier) 
                    {
                        
                        if (bon.Data_eliberare.Month == SelectedDate.Month && bon.Data_eliberare.Year == SelectedDate.Year)
                        {
                            Incasari incasariItem = new Incasari
                            {
                                Ziua = bon.Data_eliberare, 
                                Suma = bon.Suma_incasata
                            };
                            SumaIncasata.Add(incasariItem);
                        }
                    }
                }
                OnPropertyChanged(nameof(SumaIncasata));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
