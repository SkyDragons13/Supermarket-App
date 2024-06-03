using System.Collections.Generic;
using System.Linq;
using Supermarket.Model;

namespace Supermarket.Services
{
    public class UtilizatorServices
    {
        private readonly SupermarketEntities _context;

        public UtilizatorServices(SupermarketEntities context)
        {
            _context = context;
        }

        public List<Utilizator> GetAllUtilizatori()
        {
            return _context.Utilizator.ToList();
        }
        public List<Utilizator> GetAllCasieri()
        {
            return _context.Utilizator.Where(c => c.Tip == "Casier").ToList();
        }
        public void AddUtilizator(Utilizator utilizator)
        {
            _context.Utilizator.Add(utilizator);
            _context.SaveChanges();
        }

        public void UpdateUtilizator(Utilizator utilizator)
        {
            var existingUtilizator = _context.Utilizator.FirstOrDefault(u => u.Utilizator_id == utilizator.Utilizator_id);
            if (existingUtilizator != null)
            {
                existingUtilizator.Nume = utilizator.Nume;
                existingUtilizator.Parola = utilizator.Parola;
                existingUtilizator.Tip = utilizator.Tip;
                existingUtilizator.Active = utilizator.Active;
                _context.SaveChanges();
            }
        }

        public void DeleteUtilizator(int utilizatorId)
        {
            var existingUtilizator = _context.Utilizator.FirstOrDefault(u => u.Utilizator_id == utilizatorId);
            if (existingUtilizator != null)
            {
                existingUtilizator.Active = false;
                _context.SaveChanges();
            }
        }
    }
}
