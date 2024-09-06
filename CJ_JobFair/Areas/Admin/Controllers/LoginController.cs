using CJ_JobFair.Areas.Admin.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace CJ_JobFair.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private MySqlDatabase MySqlDatabase { get; set; }
        public IActionResult Index()
        {
            return View();
        }
        public LoginController(MySqlDatabase mySqlDatabase)
        {

            this.MySqlDatabase = mySqlDatabase;
        }
        public IActionResult LoginView()
        {
            LoginModel obj = new LoginModel();
            return View(obj);
            //return View("Login_View", await this.admin_login(obj));

        }
        const string SessionName = "_Name";
      public IActionResult SearchLogin(LoginModel obj)
        {
            var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"select * from adminlogin where (username='" + obj.username + "' and password='" + obj.password + "')";
            using (var reader = cmd.ExecuteReader())
                if (reader.Read())
                {
                    var n = reader.GetFieldValue<string>(1);
                    HttpContext.Session.SetString(SessionName, n);
                    ViewBag.Name = HttpContext.Session.GetString(SessionName);
                    return View("Index");
                }

            ViewBag.message = "Password or username incorrect!!";
            return View("LoginView");

        }
        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the home page or a specific page after logout
            return View("LoginView");
        }
    }
}
