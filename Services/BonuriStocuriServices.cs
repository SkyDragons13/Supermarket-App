using Supermarket.Model;
using Supermarket.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Services
{
    public class BonuriStocuriServices
    {
        private readonly SupermarketEntities _context;
        public BonuriStocuriServices(SupermarketEntities context)
            {
                _context = context;
            }
        public ObservableCollection<AddedProdusView> GetProduse(int bonID)
        {
            var addedProduse = _context.Bonuri_Stocuri
                                    .Where(bs => bs.Bon_id == bonID)
                                    .Select(bs => new AddedProdusView
                                    {
                                        Produs_nume = bs.Produs_nume,
                                        Cantitate = bs.Cantitate,
                                        Pret = bs.Pret,
                                        PretTotal = bs.Cantitate*bs.Pret
                                    })
                                    .ToList();

            return new ObservableCollection<AddedProdusView>(addedProduse);
        }
    }
}
