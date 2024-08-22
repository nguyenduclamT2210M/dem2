using dem2.Models;
using dem2.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;

namespace dem2.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService _authService;
        private readonly HttpClient _httpClient;

        public AuthController()
        {
            _authService = new ApiService();
            _httpClient = new HttpClient();
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy token từ dịch vụ xác thực
                    var token = await _authService.LoginAsync(model);

                    if (token != null)
                    {
                        // Lưu token vào cookie với thời gian hết hạn hợp lý
                        HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTime.UtcNow.AddHours(1)
                        });

                        // Giải mã token để lấy tên người dùng
                        var fullName = _authService.GetFullNameFromToken(token);

                        // Lưu tên người dùng vào TempData để sử dụng trong view
                        TempData["UserName"] = fullName;

                        // Chuyển hướng đến trang chính sau khi đăng nhập thành công
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string result = await _authService.RegisterAsync(model);
                    // Xử lý phản hồi từ API, ví dụ như thông báo đăng ký thành công, v.v.
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }
        // check call api
        public async Task<ActionResult> Get()
        {
            try
            {
                string result = await _authService.GetDataAsync("/api/auth/get");
                ViewBag.ApiResponse = result;
            }
            catch (Exception ex)
            {
                ViewBag.ApiResponse = ex.Message;
            }

            return View();
        }


        
    }
}
