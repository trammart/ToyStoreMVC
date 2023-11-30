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
    public class SizeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SizeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        // show insert and update
        public IActionResult Upsert(int? id)
        {
            Size size = new Size();
            if (id == null)
            {
                // create
                return View(size);
            }

            // edit
            size = _unitOfWork.Size.Get(id.GetValueOrDefault());
            if (size == null)
            {
                return NotFound();
            }
            return View(size);
        }

        // insert and update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Size size)
        {
            if (ModelState.IsValid)
            {
                if (size.Id == 0)
                {
                    _unitOfWork.Size.Add(size); // insert
                }
                else
                {
                    _unitOfWork.Size.Update(size); // update
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(size);
        }

        #region API CALLS

        // api get size
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Size.GetAll();
            return Json(new { data = allObj });
        }

        //api delete size
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Size.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Xóa thất bại!" });
            }
            _unitOfWork.Size.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Xóa thành công!" });
        }

        #endregion
    }
}
