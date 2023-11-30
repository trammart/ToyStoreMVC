using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyStoreMVC.DataAccess.Data;
using ToyStoreMVC.DataAccess.Repository.IRepository;
using ToyStoreMVC.Models;
using ToyStoreMVC.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToyStoreMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(string id)
        {
            ApplicationUser user = new ApplicationUser();

            if (!string.IsNullOrEmpty(id))
            {
                user = _db.ApplicationUsers.Include(u => u.Company).FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    return NotFound();
                }
            }

            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            // Lấy vai trò của người dùng
            var userRoleId = userRole.FirstOrDefault(u => u.UserId == user.Id)?.RoleId;
            var userRoleName = roles.FirstOrDefault(r => r.Id == userRoleId)?.Name;

            // Sắp xếp roles sao cho vai trò của người dùng đứng đầu
            var sortedRoles = roles.OrderBy(r => r.Name == userRoleName ? 0 : 1);

            ViewBag.Roles = new SelectList(sortedRoles, "Name", "Name", userRoleName);

            return View(user);
        }




        #region API CALLS

        [HttpPost]
        public IActionResult UpdateUserRole(string userId, string roleName)
        {
            try
            {
                // Lấy người dùng từ cơ sở dữ liệu theo UserId
                var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Người dùng không tồn tại." });
                }

                // Lấy vai trò từ cơ sở dữ liệu theo tên vai trò
                var role = _db.Roles.FirstOrDefault(r => r.Name == roleName);
                if (role == null)
                {
                    return Json(new { success = false, message = "Vai trò không tồn tại." });
                }

				// Kiểm tra xem người dùng đã có vai trò chưa
				var userRole = _db.UserRoles.FirstOrDefault(ur => ur.UserId == userId);
				if (userRole != null)
				{
					// Nếu người dùng đã có vai trò, xóa vai trò hiện tại
					_db.UserRoles.Remove(userRole);
					_db.SaveChanges();
				}

				// Tạo mới UserRole với vai trò mới
				var newUserRole = new IdentityUserRole<string>
				{
					UserId = userId,
					RoleId = role.Id
				};
				_db.UserRoles.Add(newUserRole);
				_db.SaveChanges();

				return Json(new { success = true, message = "Đã cập nhật vai trò." });
			}
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi cập nhật vai trò." });
            }
        }



        // api get user
        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _db.ApplicationUsers.Include(u=>u.Company).ToList();

            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            
            foreach(var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                if(user.Company == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }    
            }    

            return Json(new { data = userList });
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _db.Roles.Select(r => r.Name).ToList();
            return Json(new { data = roles });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if(objFromDb == null)
            {
                return Json(new { success = false, message = "Lỗi không thể khóa!" });
            }
            if(objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                // người dùng hiện đang bị khóa, mở khóa 
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Thành công!" });
        }

        


        #endregion
    }
}
