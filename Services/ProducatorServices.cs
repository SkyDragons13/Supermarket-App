using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using Supermarket.Model;

namespace Supermarket.Services
{
    public class ProducatorServices
    {
        private readonly SupermarketEntities _context;

        public ProducatorServices(SupermarketEntities context)
        {
            _context = context;
        }

        public List<Producator> GetAllProducatori()
        {
            return _context.Producator.ToList();
        }
        public List<Producator> GetAllProducatoriSQL()
        {
            return _context.Database.SqlQuery<Producator>("EXEC dbo.GetAllProducatori").ToList();
        }

        public Producator GetProducatorById(int id)
        {
            return _context.Producator.FirstOrDefault(p => p.Producator_id == id);
        }
        public void UpdateProducator(Producator producator)
        {
            var producatorIdParam = new SqlParameter("@ProducatorId", producator.Producator_id);
            var producatorNumeParam = new SqlParameter("@ProducatorNume", producator.Producator_nume);
            var taraOrigineParam = new SqlParameter("@TaraOrigine", producator.Tara_origine);
            var activeParam = new SqlParameter("@Active", producator.Active);

            _context.Database.ExecuteSqlCommand("exec dbo.UpdateProducator @ProducatorId, @ProducatorNume, @TaraOrigine, @Active",
                producatorIdParam, producatorNumeParam, taraOrigineParam, activeParam);
        }
        public void DeleteProducator(int producatorId)
        {
            var producatorIdParam = new SqlParameter("@ProducatorId", producatorId);
            _context.Database.ExecuteSqlCommand("exec dbo.DeleteProducator @ProducatorId", producatorIdParam);
            var existingProducator = _context.Producator.FirstOrDefault(p => p.Producator_id == producatorId);
            if (existingProducator != null)
            {
                existingProducator.Active = false;
                _context.SaveChanges();
            }
        }
        public void AddProducator(Producator producator)
        {
            var producatorNumeParam = new SqlParameter("@ProducatorNume", producator.Producator_nume);
            var taraOrigineParam = new SqlParameter("@TaraOrigine", producator.Tara_origine);
            var activeParam = new SqlParameter("@Active", producator.Active);

            _context.Database.ExecuteSqlCommand(
                "EXEC dbo.AddProducator @ProducatorNume, @TaraOrigine, @Active",
                producatorNumeParam, taraOrigineParam, activeParam);
        }

    }
}
