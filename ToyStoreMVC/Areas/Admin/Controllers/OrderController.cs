using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToyStoreMVC.DataAccess.Repository.IRepository;
using ToyStoreMVC.Models;
using ToyStoreMVC.Models.ViewModels;
using ToyStoreMVC.Utility;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace ToyStoreMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // anh xa du lieu
        [BindProperty]
        public OrderDetailsVM orderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // order process
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult Index()
        {
            return View();
        }

        // Details info
        public IActionResult Details(int id)
        {
            orderVM = new OrderDetailsVM()
            {
                orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id,
                                                includeProperties: "ApplicationUser"),
                orderDetails = _unitOfWork.OrderDetails.GetAll(o => o.OrderId == id, includeProperties: "Product")
            };
            return View(orderVM);
        }

        //details pay
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Details")]
        public IActionResult Details(string stripeToken)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderVM.orderHeader.Id,
                                                includeProperties: "ApplicationUser");
            if (stripeToken != null)
            {
                //process the payment
                var options = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(orderHeader.OrderTotal),
                    Currency = "usd",
                    Description = "Order ID : " + orderHeader.Id,
                    Source = stripeToken
                };

                var service = new ChargeService();
                Charge charge = service.Create(options);

                if (charge.Id == null)
                {
                    orderHeader.PaymentStatus = SD.PaymentStatusRejected;
                }
                else
                {
                    orderHeader.TransactionId = charge.Id;
                }
                if (charge.Status.ToLower() == "succeeded")
                {
                    orderHeader.PaymentStatus = SD.PaymentStatusApproved;

                    orderHeader.PaymentDate = DateTime.Now;
                }

                _unitOfWork.Save();

            }
            return RedirectToAction("Details", "Order", new { id = orderHeader.Id });
        }

        // order process
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            orderHeader.OrderStatus = SD.StatusInProcess;
            
            orderHeader.ShippingDate = DateTime.Now;

            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        //order ship
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult ShipOrder()
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderVM.orderHeader.Id);
            orderHeader.TrackingNumber = orderVM.orderHeader.TrackingNumber;
            orderHeader.Carrier = orderVM.orderHeader.Carrier;
            orderHeader.OrderStatus = SD.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;

            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        // order cancel
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult CancelOrder(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            if (orderHeader.PaymentStatus == SD.StatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Amount = Convert.ToInt32(orderHeader.OrderTotal),
                    Reason = RefundReasons.RequestedByCustomer,
                    Charge = orderHeader.TransactionId

                };
                var service = new RefundService();
                Refund refund = service.Create(options);

                orderHeader.OrderStatus = SD.StatusRefunded;
                orderHeader.PaymentStatus = SD.StatusRefunded;
            }
            else
            {
                orderHeader.OrderStatus = SD.StatusCancelled;
                orderHeader.PaymentStatus = SD.StatusCancelled;
            }

            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        //order update info
       public IActionResult UpdateOrderDetails()
        {
            var orderHEaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderVM.orderHeader.Id);
            orderHEaderFromDb.Name = orderVM.orderHeader.Name;
            orderHEaderFromDb.PhoneNumber = orderVM.orderHeader.PhoneNumber;
            orderHEaderFromDb.StreetAddress = orderVM.orderHeader.StreetAddress;
            orderHEaderFromDb.District = orderVM.orderHeader.District;
            orderHEaderFromDb.City = orderVM.orderHeader.City;
            if (orderVM.orderHeader.Carrier != null)
            {
                orderHEaderFromDb.Carrier = orderVM.orderHeader.Carrier;
            }
            if (orderVM.orderHeader.TrackingNumber != null)
            {
                orderHEaderFromDb.TrackingNumber = orderVM.orderHeader.TrackingNumber;
            }

            _unitOfWork.Save();
            TempData["Error"] = "Cập nhập thành công.";
            return RedirectToAction("Details", "Order", new { id = orderHEaderFromDb.Id });
        }

        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult ExportToExcel()
        {
            var shippedOrders = _unitOfWork.OrderHeader.GetAll(o => o.OrderStatus == SD.StatusShipped, includeProperties: "ApplicationUser");

            // Calculate total revenue
            decimal totalRevenue = shippedOrders.Sum(o => o.OrderTotal);

            byte[] fileContents;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("ShippedOrdersInfo");
            Sheet.Cells["A1"].Value = "Order ID";
            Sheet.Cells["B1"].Value = "Tên khách đặt hàng";
            Sheet.Cells["D1"].Value = "Thành tiền đơn hàng";
            Sheet.Cells["C1"].Value = "Ngày giao hàng";

            int row = 2;
            foreach (var order in shippedOrders)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = order.Id;
                Sheet.Cells[string.Format("B{0}", row)].Value = order.ApplicationUser.Name;
                Sheet.Cells[string.Format("D{0}", row)].Value = order.OrderTotal;
                Sheet.Cells[string.Format("C{0}", row)].Value = order.ShippingDate.ToString("yyyy-MM-dd HH:mm:ss");

                row++;
            }

            // Add a row for total revenue
            Sheet.Cells[string.Format("A{0}", row)].Value = "Total Revenue";
            Sheet.Cells[string.Format("D{0}", row)].Value = totalRevenue;

            Sheet.Cells["A:AZ"].AutoFitColumns();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            fileContents = Ep.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "DonHangThanhCong_TeddyStore.xlsx"
            );
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetOrderList(string status)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<OrderHeader> orderHeaderList;

            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                orderHeaderList = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                orderHeaderList = _unitOfWork.OrderHeader.GetAll(
                                    u => u.ApplicationUserId == claim.Value,
                                    includeProperties: "ApplicationUser"
                                );
            }

            // status order
            switch (status)
            {
                case "pending":
                    orderHeaderList = orderHeaderList.Where(o => o.PaymentStatus == SD.StatusPending);
                    break;
                case "inprocess":
                    orderHeaderList = orderHeaderList.Where(o => o.OrderStatus == SD.StatusApproved ||
                                                            o.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderHeaderList = orderHeaderList.Where(o => o.OrderStatus == SD.StatusShipped);
                    break;
                case "rejected":
                    orderHeaderList = orderHeaderList.Where(o => o.OrderStatus == SD.StatusCancelled ||
                                                            o.OrderStatus == SD.StatusRefunded ||
                                                            o.OrderStatus == SD.PaymentStatusRejected);
                    break;
                default:
                    break;
            }


            return Json(new { data = orderHeaderList });
        }


        #endregion
    }
}
