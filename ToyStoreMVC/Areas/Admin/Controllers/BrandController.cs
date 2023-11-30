using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToyStoreMVC.DataAccess.Repository.IRepository;
using ToyStoreMVC.Models;
using ToyStoreMVC.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToyStoreMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BrandController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        // insert and update
        public IActionResult Upsert(int? id)
        {
            Brand brand = new Brand();
            if(id == null)
            {
                // create
                return View(brand);
            }

            // edit
            brand = _unitOfWork.Brand.Get(id.GetValueOrDefault());
            if(brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // insert and update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Brand brand)
        {
            if (ModelState.IsValid)
            {
                if(brand.Id == 0)
                {
                    _unitOfWork.Brand.Add(brand); // insert
                }
                else
                {
                    _unitOfWork.Brand.Update(brand); // update
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        #region API CALLS

        // api get
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Brand.GetAll();
            return Json(new { data = allObj });
        }

        // api delete
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Brand.Get(id);
            if(objFromDb == null)
            {
                return Json(new { success = false, message = "Xóa thất bại!" });
            }
            _unitOfWork.Brand.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Xóa thành công!" });
        }

        #endregion
    }
}
