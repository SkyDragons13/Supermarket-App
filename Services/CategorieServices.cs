using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using Supermarket.Model;
using Supermarket.View;

namespace Supermarket.Services
{
    public class CategorieServices
    {
        private readonly SupermarketEntities _dbContext;

        public CategorieServices(SupermarketEntities context)
        {
            _dbContext = context;
        }

        public List<Categorie> GetAllCategories()
        {
            return _dbContext.Categorie.ToList();
        }
        public List<Categorie> GetAllCategoriesSQL()
        {
            var categories = _dbContext.Database.SqlQuery<Categorie>("exec dbo.GetAllCategories").ToList();
            return categories;
        }

        public Categorie GetCategorieById(int id)
        {
            return _dbContext.Categorie.FirstOrDefault(c => c.Categorie_id == id);
        }
        public void UpdateCategorie(Categorie categorie) 
        {
            var categorieIdParam = new SqlParameter("@CategorieId", categorie.Categorie_id);
            var categorieNameParam = new SqlParameter("@CategorieName", categorie.Categorie_nume);
            var activeParam = new SqlParameter("@Active", categorie.Active);

            // Execute the stored procedure
            _dbContext.Database.ExecuteSqlCommand("UpdateCategorie @CategorieId, @CategorieName, @Active",
                                                 categorieIdParam, categorieNameParam, activeParam);
        }
        public void DeleteCategorie(int categorieId)
        {
            string deleteProcedure = "DeleteCategorie @CategorieId";
            SqlParameter parameter = new SqlParameter("@CategorieId", categorieId);
            _dbContext.Database.ExecuteSqlCommand(deleteProcedure, parameter);
            var existingCategorie = _dbContext.Categorie.FirstOrDefault(p => p.Categorie_id == categorieId);
            if (existingCategorie != null)
            {
                existingCategorie.Active = false;
                _dbContext.SaveChanges();
            }
        }
        public void AddCategorie(Categorie categorie)
        {
            string addProcedure = "AddCategorie @CategorieNume, @Active";
            SqlParameter[] parameters =
            {
                new SqlParameter("@CategorieNume", categorie.Categorie_nume),
                new SqlParameter("@Active", categorie.Active)
            };

            _dbContext.Database.ExecuteSqlCommand(addProcedure, parameters);
        }
    }
}
