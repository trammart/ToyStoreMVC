using Microsoft.AspNetCore.Mvc;
using ToyStoreMVC.DataAccess.Repository.IRepository;
using ToyStoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToyStoreMVC.ViewComponents
{
    public class MenuCategory : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public MenuCategory(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        } 


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = _unitOfWork.Category.GetAll();
            return View(categories);
        }
    }
}
