using System.Linq;
using System.Web.Mvc;
using LoginnMVC.Models;

namespace LoginnMVC.Controllers
{
    public class AccountController : Controller
    {
        UserDataEntities1 db = new UserDataEntities1(); // Database context

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserRegister ur)
        {
            if (ModelState.IsValid)
            {
                if (db.UserRegisters.Any(x => x.Email == ur.Email))
                {
                    ViewBag.Message = "Email already registered";
                }
                else
                {
                    db.UserRegisters.Add(ur);
                    db.SaveChanges();
                    Response.Write("<script>alert('Registration Successful')</script>");

                }
            }
            return View();

        }

        [HttpPost]
        public ActionResult Login(MyLogin login)
        {
            // Query database for user using email and password
            var user = db.UserRegisters.SingleOrDefault(m => m.Email == login.Email && m.Password == login.Password);

            if (user != null) // Check if user exists
            {
                // Store user details in session
                Session["UserId"] = user.UserID;
                Session["UserName"] = user.Username;
                Session["UserEmail"] = user.Email;

                // Redirect to a dashboard with user-specific data
                return RedirectToAction("Dashboard", new { email = user.Email });
            }
            else
            {
                ViewBag.Message = "Invalid email or password";
            }

            return View();
        }

        public ActionResult Dashboard(string email)
        {
            if (Session["UserId"] == null) // Ensure user is logged in
            {
                return RedirectToAction("Login");
            }

            // Fetch user data using email
            var user = db.UserRegisters.SingleOrDefault(m => m.Email == email);

            if (user != null)
            {
                return View(user); // Pass user data to the dashboard view
            }

            return RedirectToAction("Login"); // Redirect to login if user not found
        }

        public ActionResult Logout()
        {
            Session.Clear(); // Clear session
            return RedirectToAction("Login");
        }
       

    }
}
