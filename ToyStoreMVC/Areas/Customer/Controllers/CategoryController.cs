using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToyStoreMVC.DataAccess.Repository.IRepository;
using ToyStoreMVC.Models;
using ToyStoreMVC.Models.ViewModels;
using ToyStoreMVC.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ToyStoreMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // Tìm kiếm sản phẩm theo danh mục và tên sản phẩm
        public IActionResult Index(int categoryId, string productName, int pageNumber = 1, int pageSize = 6)
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category,Brand");
            string romoveHintProductName;
            if (productName != "" && productName != null)
            {
                romoveHintProductName = SD.RemoveVietnameseTone(productName);
            }
            else
            {
                romoveHintProductName = productName;
            }
           
            if((productName == "" || productName == null) && categoryId == 0)
            { }    
            else if (productName == "" || productName == null || categoryId == 0)
            {
                if(productName != "" && productName != null)
                {
                    products = products.Where(p => SD.RemoveVietnameseTone(p.Name).Contains(romoveHintProductName));
                }
                else
                {
                    products = products.Where(p => p.CategoryId == categoryId);
                }
                
            }
            else if((productName != "" && productName != null) && categoryId > 0)
            {
                products = products.Where(p => p.CategoryId == categoryId && SD.RemoveVietnameseTone(p.Name).Contains(romoveHintProductName));
            }

            return View(Pagination.PagedResult(products, pageNumber, pageSize));
        }
    }
}
