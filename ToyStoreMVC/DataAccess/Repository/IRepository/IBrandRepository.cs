using ToyStoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToyStoreMVC.DataAccess.Repository.IRepository
{
    public interface IBrandRepositoty : IRepository<Brand>
    {
        void Update(Brand brand);
    }
}
