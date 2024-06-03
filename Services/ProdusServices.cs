using Supermarket.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Services
{

    public class ProdusServices
    {
        private readonly SupermarketEntities _dbContext;
        public ProdusServices(SupermarketEntities context)
        {
            _dbContext = context;
        }
        public List<Produs> GetAllProduse()
        {
            return _dbContext.Produs.ToList();
        }
        public void UpdateProdus(Produs updatedProdus)
        {
            var existingProdus = _dbContext.Produs.FirstOrDefault(p => p.Produs_id == updatedProdus.Produs_id);

            if (existingProdus != null)
            {

                existingProdus.Produs_nume = updatedProdus.Produs_nume;
                existingProdus.Cod_bare = updatedProdus.Cod_bare;
                existingProdus.Categorie_id = updatedProdus.Categorie_id;
                existingProdus.Producator_id = updatedProdus.Producator_id;
                existingProdus.Active = updatedProdus.Active;

                _dbContext.SaveChanges();
            }
        }
        public void DeleteProdus(int produsId)
        {
            string deleteProcCall = "EXEC DeleteProdus @ProdusId";
            _dbContext.Database.ExecuteSqlCommand(deleteProcCall, new SqlParameter("@ProdusId", produsId));

            // Optionally, update the Active status using EF
            var existingProdus = _dbContext.Produs.FirstOrDefault(p => p.Produs_id == produsId);
            if (existingProdus != null)
            {
                existingProdus.Active = false;
                _dbContext.SaveChanges();
            }
        }

        public void AddProdus(Produs produs)
        {
            // Call the stored procedure using Entity Framework
            _dbContext.Database.ExecuteSqlCommand(
                "EXEC AddProdus @ProdusNume, @CodBare, @CategorieId, @ProducatorId, @Active",
                new SqlParameter("@ProdusNume", produs.Produs_nume),
                new SqlParameter("@CodBare", produs.Cod_bare),
                new SqlParameter("@CategorieId", produs.Categorie_id),
                new SqlParameter("@ProducatorId", produs.Producator_id),
                new SqlParameter("@Active", produs.Active)
            );
        }

    }
}
