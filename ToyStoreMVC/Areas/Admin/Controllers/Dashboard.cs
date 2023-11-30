using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToyStoreMVC.DataAccess.Repository.IRepository;
using ToyStoreMVC.Models;
using ToyStoreMVC.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ToyStoreMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // Lấy tổng số lượng sản phẩm từ database
            int totalProducts = _unitOfWork.Product.GetAll().Count();
            ViewBag.TotalProducts = totalProducts;

            var totalShippedOrdersValue = GetTotalShippedOrdersValue();
            ViewBag.TotalShippedOrdersValue = totalShippedOrdersValue;

            // Lấy ngày đầu tiên của tháng hiện tại
            DateTime firstDayOfMonth = new(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Lấy danh sách đơn hàng đã giao trong tháng hiện tại
            var shippedOrders = _unitOfWork.OrderHeader.GetAll(
                o => o.OrderStatus == SD.StatusShipped && o.ShippingDate >= firstDayOfMonth
            );
            // Tính tổng doanh thu
            decimal totalRevenue = shippedOrders.Sum(o => o.OrderTotal);
            ViewBag.TotalRevenue = totalRevenue;

            // Lấy số lượng đơn hàng đang ở trạng thái "Đang chờ xử lý"
            int pendingOrdersCount = GetPendingOrdersCount();

            // Truyền giá trị số lượng đơn hàng đang chờ xử lý đến view
            ViewBag.PendingOrdersCount = pendingOrdersCount;

            ViewBag.CurrentMonth = DateTime.Now.ToString("MM/yyyy");
            ViewBag.Pre1 = DateTime.Now.AddMonths(-1).ToString("MM/yyyy");
            ViewBag.Pre2 = DateTime.Now.AddMonths(-2).ToString("MM/yyyy");
            ViewBag.Pre3 = DateTime.Now.AddMonths(-3).ToString("MM/yyyy");
            ViewBag.Pre4 = DateTime.Now.AddMonths(-4).ToString("MM/yyyy");

            ViewBag.MonthlyRevenueData1 = GetMonthlyRevenueData(-1);
            ViewBag.MonthlyRevenueData2 = GetMonthlyRevenueData(-2);
            ViewBag.MonthlyRevenueData3 = GetMonthlyRevenueData(-3);
            ViewBag.MonthlyRevenueData4 = GetMonthlyRevenueData(-4);

            return View();
        }

        private decimal GetTotalShippedOrdersValue()
        {
            var shippedOrders = _unitOfWork.OrderHeader.GetAll(o => o.OrderStatus == SD.StatusShipped);
            decimal totalShippedOrdersValue = shippedOrders.Sum(o => o.OrderTotal);
            return totalShippedOrdersValue;
        }
        private int GetPendingOrdersCount()
        {
            // Lấy danh sách đơn hàng đang ở trạng thái "Đang chờ xử lý"
            var pendingOrders = _unitOfWork.OrderHeader.GetAll(o => o.OrderStatus == SD.StatusApproved);

            // Đếm số lượng đơn hàng
            int count = pendingOrders.Count();

            return count;
        }
        private decimal GetMonthlyRevenueData(int monthsAgo)
        {
            // Lấy ngày đầu tiên của tháng hiện tại
            DateTime currentDate = DateTime.Now;
            DateTime targetMonth = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(monthsAgo);

            // Lấy danh sách đơn hàng đã giao trong tháng
            var shippedOrders = _unitOfWork.OrderHeader.GetAll(
                o => o.OrderStatus == SD.StatusShipped && o.ShippingDate.Year == targetMonth.Year && o.ShippingDate.Month == targetMonth.Month
            );

            // Tính tổng doanh thu
            decimal totalRevenue = shippedOrders.Sum(o => o.OrderTotal);

            // Trả về giá trị decimal
            return totalRevenue;
        }

        #region API CALL
        [HttpGet]
        public IActionResult GetMonthlyRevenueData()
        {
            // Lấy ngày đầu tiên của tháng hiện tại
            DateTime currentDate = DateTime.Now;
            DateTime firstDayOfCurrentMonth = new(currentDate.Year, currentDate.Month, 1);

            // Lấy dữ liệu doanh thu cho 5 tháng gần nhất
            var monthlyRevenueData = new List<MonthlyRevenueData>();

            for (int i = 4; i >= 0; i--)
            {
                DateTime targetMonth = firstDayOfCurrentMonth.AddMonths(-i);

                // Lấy danh sách đơn hàng đã giao trong tháng
                var shippedOrders = _unitOfWork.OrderHeader.GetAll(
                    o => o.OrderStatus == SD.StatusShipped && o.ShippingDate.Year == targetMonth.Year && o.ShippingDate.Month == targetMonth.Month
                );

                // Tính tổng doanh thu
                decimal totalRevenue = shippedOrders.Sum(o => o.OrderTotal);

                // Thêm dữ liệu vào danh sách
                monthlyRevenueData.Add(new MonthlyRevenueData
                {
                    Month = targetMonth.ToString("MM/yyyy"),
                    Revenue = totalRevenue
                });
            }

            // Chuyển đổi danh sách thành định dạng JSON và trả về
            return Json(monthlyRevenueData);
        }

        public class MonthlyRevenueData
        {
            public string Month { get; set; }
            public decimal Revenue { get; set; }
        }
        #endregion
    }
}
