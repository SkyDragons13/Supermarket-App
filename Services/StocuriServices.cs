using Supermarket.Model;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System;

namespace Supermarket.Services
{
    public class StocuriServices
    {
        private readonly SupermarketEntities _context;

        public StocuriServices(SupermarketEntities context)
        {
            _context = context;
        }

        public List<Stocuri> GetAllStocuri()
        {
            var stocuri = _context.Stocuri.ToList();
            foreach (var stoc in stocuri)
            {
                if (DateTime.Now > stoc.Data_expirare)
                    stoc.Active = false;
            }
            _context.SaveChanges();
            return stocuri;
        }

        public void UpdateStocuri(Stocuri stoc)
        {
            var existingStoc=_context.Stocuri.FirstOrDefault(s=>s.Stoc_id == stoc.Stoc_id);
            if (existingStoc != null)   
            {
                existingStoc.Cantitate=stoc.Cantitate;
                existingStoc.Unitate_masura=stoc.Unitate_masura;
                existingStoc.Data_aprovizionare=stoc.Data_aprovizionare;
                existingStoc.Data_expirare=stoc.Data_expirare;
                existingStoc.Pret_achizitie=stoc.Pret_achizitie;
                existingStoc.Pret_vanzare = stoc.Pret_vanzare;
                existingStoc.Active=stoc.Active;

                _context.SaveChanges();
            }
        }

        public void DeleteStocuri(int stocId)
        {
            var stocToDelete = _context.Stocuri.Find(stocId);
            if (stocToDelete != null)
            {
                stocToDelete.Active=false;
                _context.SaveChanges();
            }
        }
        public void AddStoc(Stocuri stoc)
        {
            _context.Stocuri.Add(stoc);
            _context.SaveChanges();
        }
    }
}
