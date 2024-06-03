﻿using Supermarket.Model;
using Supermarket.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Supermarket.ViewModel
{
    internal class BonMareViewModel:INotifyPropertyChanged
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
        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                UpdateSelectedBon();
                OnPropertyChanged(nameof(SelectedBon));
                OnPropertyChanged(nameof(AddedProduse));
            }
        }

        public BonMareViewModel()
        {
            var context = new SupermarketEntities();
            _bonuriService = new BonuriServices(context);
            _bonuriStocuriService = new BonuriStocuriServices(context);
            LoadData();
        }

        private void UpdateSelectedBon()
        {
            var bonuriWithSameDate = new List<Bonuri>();

            foreach (var bon in Bonuri)
            {
                if (bon.Data_eliberare.Month == SelectedDate.Month &&
                    bon.Data_eliberare.Day == SelectedDate.Day &&
                    bon.Data_eliberare.Year == SelectedDate.Year)
                {
                    bonuriWithSameDate.Add(bon);
                }
            }
            SelectedBon = bonuriWithSameDate.OrderByDescending(b => b.Suma_incasata).FirstOrDefault();
            LoadAddedProduse();
        }

        private void LoadData()
        {
            Bonuri = new ObservableCollection<Bonuri>(_bonuriService.GetAllBonuri());
            AddedProduse = new ObservableCollection<AddedProdusView>();
        }

        private void LoadAddedProduse()
        {
            AddedProduse.Clear();
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

