﻿using ToyStoreMVC.DataAccess.Data;
using ToyStoreMVC.DataAccess.Repository.IRepository;
using ToyStoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToyStoreMVC.DataAccess.Repository
{
    public class BrandRepository : Repository<Brand>, IBrandRepositoty 
    {
        private readonly ApplicationDbContext _db;

        public BrandRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Brand brand)
        {
            var objFromDb = _db.Brands.FirstOrDefault(s => s.Id == brand.Id);
            if(objFromDb != null)
            {
                objFromDb.Name = brand.Name;
                objFromDb.Discription = brand.Discription;
                objFromDb.Logo = brand.Logo;
            }
        }
    }
}
