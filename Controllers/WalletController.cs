using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using yazilimYapimi.Models.Entity;


namespace yazilimYapimi.Controllers
{
    public class WalletController : Controller
    {
        // GET: Wallet
        MvcDbProjeEntities db = new MvcDbProjeEntities();
        [HttpGet]
        public ActionResult Wallet()
        {
            if (Session["UserType"] == null)
            {
                return RedirectToAction("ProductList", "Product");
            }

            int userId = Convert.ToInt32(Session["UserID"]);
            var wallet = db.tableWallet.FirstOrDefault(x=>x.UserID == userId);
            if (wallet != null)
            {
                ViewBag.Money = wallet.Money;
            }


            return View();
        }
        [HttpPost]
        public ActionResult Wallet(tableWallet w)
        {
            if (Session["UserType"] == null)
            {
                return RedirectToAction("ProductList", "Product");
            }

            int userId = Convert.ToInt32(Session["UserID"]);
            var user = db.tableUser.Find(userId);
            var wallet = db.tableWallet.FirstOrDefault(x => x.UserID == userId);


            if (user.UserType == false)
            {
                tableConfirmMoney confirmMoney = new tableConfirmMoney()
                {
                    Confirmed = false,
                    Money = w.Money,
                    UserID = userId
                };
                db.tableConfirmMoney.Add(confirmMoney);

                ViewBag.Message = "Your request has been created.";
            }
            else
            {
                
                wallet.Money += w.Money;
                ViewBag.Message = "Your money has been added to your wallet.";
                
            }

            db.SaveChanges();
            ViewBag.Money = wallet.Money;

            return View();
        }

        public ActionResult ConfirmList()
        {
            if (Convert.ToInt16(Session["UserType"]) == 0)
            {
                return RedirectToAction("ProductList", "Product");
            }

            var list = db.tableConfirmMoney.Where(x=>x.Confirmed == false).ToList();

            return View(list);
        }

        public ActionResult Confirm(int id)
        {
            if (Convert.ToInt16(Session["UserType"]) == 0)
            {
                return RedirectToAction("ProductList", "Product");
            }
            
            var result = db.tableConfirmMoney.Find(id);
            int? userId = result.UserID;
            var tableWallet = db.tableWallet.FirstOrDefault(x => x.UserID == userId);

            tableWallet.Money += currencyToTRY(result.Money,"USD");
            result.Confirmed = true;
            db.SaveChanges();
            

            return RedirectToAction("ConfirmList");
        }
        private decimal? currencyToTRY(decimal? money,string currencyType)
        {
            XDocument xDocument = XDocument.Load("http://www.tcmb.gov.tr/kurlar/today.xml");

            var s = xDocument.Element("Tarih_Date").Elements("Currency").FirstOrDefault(a => a.Attribute("Kod").Value == currencyType);
            var bElement = s.Element("ForexBuying");

            decimal fiyat = Convert.ToDecimal(bElement.Value.Replace('.', ','));
            return money * fiyat;
        }

    }
}