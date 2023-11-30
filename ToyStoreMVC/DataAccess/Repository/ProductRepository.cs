using ToyStoreMVC.DataAccess.Data;
using ToyStoreMVC.DataAccess.Repository.IRepository;
using ToyStoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToyStoreMVC.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            var objFromDb = _db.Products.FirstOrDefault(s => s.Id == product.Id);
            if(objFromDb != null)
            {
                if(product.ImageUrl != null)
                {
                    objFromDb.ImageUrl = product.ImageUrl;
                }

                objFromDb.Name = product.Name;
                objFromDb.Discription = product.Discription;
                objFromDb.Quantity = product.Quantity;
                objFromDb.Sold = product.Sold;
                objFromDb.Price = product.Price;
                objFromDb.Discount = product.Discount;
                objFromDb.CategoryId = product.CategoryId;
                objFromDb.BrandId = product.BrandId;
                objFromDb.ProductSizes = product.ProductSizes; 
            }
        }
    }
}
