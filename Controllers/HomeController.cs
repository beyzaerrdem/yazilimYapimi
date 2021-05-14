using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yazilimYapimi.Models.Entity;




namespace yazilimYapimi.Controllers
{
    public class HomeController : Controller
    {
        MvcDbProjeEntities db = new MvcDbProjeEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(tableUser login)
        {
            if (db.tableUser.Any(x => x.UserName == login.UserName && x.Password == login.Password))  //kullanıcının girdiği veri, veritabanından kontrol ediliyor
            {
                var usr = db.tableUser.FirstOrDefault(x => x.UserName == login.UserName && x.Password == login.Password);
                Session["UserID"] = usr.ID;
                Session["UserName"] = usr.UserName;
                Session["UserType"] = usr.UserType;
                return RedirectToAction("ProductList", "Product");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(tableUser register)
        {
            register.UserType = false;
            db.tableUser.Add(register);
            db.SaveChanges();

            int userId = db.tableUser.ToList().Last().ID;

            tableWallet tableWallet = new tableWallet()
            {
                Money = 0,
                UserID = userId
            };
            db.tableWallet.Add(tableWallet);

            db.SaveChanges();

            return RedirectToAction("Login");
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Home");
        }
    }
}