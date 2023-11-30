using ToyStoreMVC.DataAccess.Data;
using ToyStoreMVC.DataAccess.Repository.IRepository;
using ToyStoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToyStoreMVC.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Brand = new BrandRepository(_db);
            Product = new ProductRepository(_db);
            Size = new SizeRepository(_db);
            Company = new CompanyRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            SP_Call = new SP_Call(_db);


        }

        public ICategoryRepository Category { get; private set; }
        public IBrandRepositoty Brand { get; private set; }
        public IProductRepository Product { get; private set; }
        public ISizeRepository Size { get; private set; } 
        public ICompanyRepository Company { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailsRepository OrderDetails { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ISP_Call SP_Call { get; private set; }

        // giai phong tai nguyen khi doi tuong bi huy
        public void Dispose()
        {
            _db.Dispose();
        }

        // luu thay doi database
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
