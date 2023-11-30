using ToyStoreMVC.DataAccess.Data;
using ToyStoreMVC.DataAccess.Repository.IRepository;
using ToyStoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToyStoreMVC.DataAccess.Repository
{
    public class SizeRepository : Repository<Size>, ISizeRepository
    {
        private readonly ApplicationDbContext _db;

        public SizeRepository(ApplicationDbContext db) : base(db)
        { 
            _db = db;
        }

        public void Update(Size size)
        {
            var objFromDb = _db.Sizes.FirstOrDefault(s => s.Id == size.Id);
            if(objFromDb != null)
            {
                objFromDb.Name = size.Name;
            }
        }
    }
}
