using Supermarket.Model;
using Supermarket.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Services
{
    public class BonuriServices
    {
        private readonly SupermarketEntities _context;

        public BonuriServices(SupermarketEntities context)
        {
            _context = context;
        }
        public List<Bonuri> GetAllBonuri()
        {
            return _context.Bonuri.ToList();
        }
        //PS SA IL FACI SA MEARGA PE LISTA DUPA CE FACI SA MEARGA ACEL ADD BUTTON DIN VIEW PENTRU DATA GRID
        public void AddBonuriWithStocuri(List<int> stocIds, List<AddedProdus> addedProduse, string nume)
        {
            // Create a new bonuri instance
            Bonuri bon = new Bonuri
            {
                Data_eliberare = DateTime.Now, // Set the current date
                Suma_incasata = 0, // Initialize the total sum to 0
                Casier=nume
            };

            // Add the bonuri instance to the context
            _context.Bonuri.Add(bon);

            // Save changes to the database to generate the Bon_id
            _context.SaveChanges();

            // Retrieve the generated Bon_id
            int bonId = bon.Bon_id;

            // Find the stocuri item by its ID
            for (int i = 0; i < stocIds.Count; i++)
            {
                int stocId = stocIds[i];
                int quantity = addedProduse[i].Cantitate;

                // Find the stock item by its ID
                Stocuri stoc = _context.Stocuri.Find(stocId);
                if (stoc != null)
                {
                    // Create a new Bonuri_Stocuri instance
                    Bonuri_Stocuri bonuriStocuri = new Bonuri_Stocuri
                    {
                        Bon_id = bonId,
                        Stoc_id = stocId,
                        Cantitate = quantity,
                        Produs_nume = stoc.Produs.Produs_nume, // Get the name of the product
                        Pret = quantity * stoc.Pret_vanzare // Calculate the price (cantitate * pret_vanzare)
                    };

                    // Add the Bonuri_Stocuri instance to the context
                    _context.Bonuri_Stocuri.Add(bonuriStocuri);

                    // Update the total sum on the bon
                    bon.Suma_incasata += bonuriStocuri.Pret;
                }
            }


            // Save changes to the database
            _context.SaveChanges();
        }

    }
}
