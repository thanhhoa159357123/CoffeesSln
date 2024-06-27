using CoffeeStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CoffeeStore.Controllers
{
    public class LoginController : KeepLoginController
    {
        private readonly ICustomerRepository _repository;

        public LoginController(ICustomerRepository repository)
        {
            _repository = repository;
        }

        private void KeepSession()
        {
            var phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            if (phoneNumber != null)
            {
                HttpContext.Session.SetString("PhoneNumber", phoneNumber);
            }

            var otpCode = HttpContext.Session.GetInt32("OTPCode");
            if (otpCode.HasValue)
            {
                HttpContext.Session.SetInt32("OTPCode", otpCode.Value);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string phoneNumber)
        {
            if (_repository.CheckPhoneNumber(phoneNumber))
            {
                Random rand = new Random();
                int otpCode = rand.Next(100000, 999999);

                HttpContext.Session.SetInt32("OTPCode", otpCode);
                HttpContext.Session.SetString("PhoneNumber", phoneNumber);

                return RedirectToAction("OTP");
            }
            else
            {
                ViewBag.ErrorMessage = "Số điện thoại chưa đăng ký";
                return View();
            }
        }

        [HttpGet]
        public IActionResult OTP()
        {
            var phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            var otpCode = HttpContext.Session.GetInt32("OTPCode");

            if (phoneNumber != null && otpCode != null)
            {
                ViewBag.OTPCode = otpCode;
                KeepSession();
                return View();
            }
            else
            {
                TempData["OTPErrorMessage"] = "Không có mã OTP nào được tạo. Vui lòng thử lại.";
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public IActionResult ConfirmOTP(int otpCode)
        {
            KeepSession();

            if (HttpContext.Session.GetInt32("OTPCode") == otpCode)
            {
                HttpContext.Session.Remove("OTPCode");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Mã OTP không hợp lệ";
                return View("OTP");
            }
        }

        public IActionResult InformationCustomers()
        {
            var phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var user = _repository.Customers.FirstOrDefault(u => u.Phone == phoneNumber);
                if (user != null)
                {
                    ViewBag.Customer = user;
                    ViewBag.NgaySinhFormatted = user.NgaySinh.ToString("dd/MM/yyyy");
                }
                else
                {
                    ViewBag.ErrorMessage = "Không tìm thấy người dùng.";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Số điện thoại không tồn tại.";
            }

            return View();
        }

        [HttpPost]
        public IActionResult UpdateCustomer(Customer updatedCustomer)
        {
            var phoneNumber = HttpContext.Session.GetString("PhoneNumber");

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var user = _repository.Customers.FirstOrDefault(u => u.Phone == phoneNumber);

                if (user != null)
                {
                    user.LastName = updatedCustomer.LastName;
                    user.FirstName = updatedCustomer.FirstName;

                    _repository.UpdateCustomer(user);

                    ViewBag.Customer = user;
                    return RedirectToAction("InformationCustomers");
                }
                else
                {
                    ViewBag.ErrorMessage = "Không tìm thấy người dùng.";
                    return View("InformationCustomers");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Số điện thoại không tồn tại.";
                return View("InformationCustomers");
            }
        }

        public IActionResult HistoryOrder()
        {
            return View();
        }
    }
}
