using ToyStoreMVC.DataAccess.Data;
using ToyStoreMVC.DataAccess.Repository.IRepository;
using ToyStoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToyStoreMVC.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ApplicationUser applicationUser) 
        {
            _db.Update(applicationUser);
        }
    }
}
