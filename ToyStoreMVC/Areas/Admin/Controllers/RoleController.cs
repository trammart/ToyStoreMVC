using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ToyStoreMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Chỉ cho phép người dùng có vai trò Admin truy cập
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new InputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(InputModel model)
        {
            if (ModelState.IsValid)
            {
                var newRole = new IdentityRole(model.Name);
                var result = await _roleManager.CreateAsync(newRole);

                if (result.Succeeded)
                {
                    TempData["StatusMessage"] = $"Role '{model.Name}' đã được thêm thành công.";
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            var model = new InputModel { ID = id, Name = role.Name };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(InputModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.ID);

                if (role != null)
                {
                    role.Name = model.Name;
                    var result = await _roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        TempData["StatusMessage"] = $"Role '{model.Name}' đã được cập nhật thành công.";
                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Role không tồn tại.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    TempData["StatusMessage"] = $"Role '{role.Name}' đã được xóa thành công.";
                    return RedirectToAction("Index");
                }

                TempData["StatusMessage"] = "Lỗi khi xóa role.";
            }

            return RedirectToAction("Index");
        }

        public class InputModel
        {
            public string ID { set; get; }

            [Required(ErrorMessage = "Phải nhập tên vai trò")]
            [Display(Name = "Tên của vai trò")]
            [StringLength(100, ErrorMessage = "{0} phải từ {2} đến {1} ký tự.", MinimumLength = 3)]
            public string Name { set; get; }
        }
    }
}
