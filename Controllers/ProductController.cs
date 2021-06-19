using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yazilimYapimi.Models.Entity;

namespace yazilimYapimi.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product

        MvcDbProjeEntities db = new MvcDbProjeEntities();
        public ActionResult ProductList()
        {
            var model = db.tableProduct.ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult NewProduct()
        {
            if (Session["UserType"] == null)
            {
                return RedirectToAction("ProductList");
            }

            return View();
        }

        [HttpPost]
        public ActionResult NewProduct(tableProduct p1)
        {
            if (Session["UserType"] == null)
            {
                return RedirectToAction("ProductList");
            }

            if (Convert.ToInt16(Session["UserType"]) == 1) //Admin ise
            {
                p1.UserID = Convert.ToInt16(Session["UserID"]);
                db.tableProduct.Add(p1);
                db.SaveChanges();
            }
            else
            {
                int userID = Convert.ToInt32(Session["UserID"]);
                tableConfirmProduct tableConfirmProduct = new tableConfirmProduct();
                tableConfirmProduct.UserID = userID;
                tableConfirmProduct.Price = p1.Price;
                tableConfirmProduct.ProductName = p1.ProductName;
                tableConfirmProduct.Quantity = p1.Quantity;
                tableConfirmProduct.Confirmed = false;
                db.tableConfirmProduct.Add(tableConfirmProduct);
                db.SaveChanges();

            }
            return RedirectToAction("ProductList", "Product");
        }


        public ActionResult ConfirmList()
        {
            if (Convert.ToInt16(Session["UserType"]) == 0)
            {
                return RedirectToAction("ProductList");
            }
            var model = db.tableConfirmProduct.Where(x => x.Confirmed == false); //Onaylanmamış ürünleri çeker false
            return View(model);
        }


        public ActionResult Confirm(int id)
        {
            if (Convert.ToInt16(Session["UserType"]) == 0)
            {
                return RedirectToAction("ProductList");
            }
            var p2 = db.tableConfirmProduct.FirstOrDefault(x => x.ID == id);   //bize gönderilen id confirm producttaki id ye eşitse
            tableProduct tableProduct = new tableProduct();
            tableProduct.ProductName = p2.ProductName;
            tableProduct.Price = p2.Price;
            tableProduct.Quantity = p2.Quantity;
            tableProduct.UserID = p2.UserID;
            db.tableProduct.Add(tableProduct);
            p2.Confirmed = true;
            db.SaveChanges();
            return RedirectToAction("ConfirmList");
        }

        [HttpGet]
        public ActionResult OrderProduct()
        {
            if (Session["UserType"] == null)
            {
                return RedirectToAction("ProductList");
            }
            return View();

        }

        [HttpPost]
        public ActionResult OrderProduct(tableOrder p3)
        {
            if (Session["UserType"] == null)
            {
                return RedirectToAction("ProductList");
            }

            int userId = Convert.ToInt32(Session["UserID"]);
            p3.CustomerID = userId;
            p3.Time = DateTime.Now;
            var userWallet = db.tableWallet.FirstOrDefault(x => x.UserID == userId);
            var productList = db.tableProduct.Where(x => x.ProductName == p3.ProductName && x.Price==p3.Price); // fiyata göre sıralanması 
            int? q = p3.Quantity;
            var money = userWallet.Money;
            decimal availableQuantity = 0;

            string message = "";

            bool islemYapildi = true;

            foreach (var item in productList)
            {
                if (item.Quantity > 0)
                {
                    if (item.Price * q <= money)
                    {
                        if (item.Quantity >= q)
                        {

                            item.Quantity -= q;
                            money -= (item.Price * q);
                            message = "Your purchase has been made.";
                            break;
                        }
                        else
                        {
                            q -= item.Quantity;
                            money -= (item.Price * item.Quantity);

                            message = "You bought " + item.Quantity + " pieces of " + p3.ProductName + "";

                            item.Quantity = 0;
                        }
                    }
                    else if (money / item.Price >= 1)
                    {
                        double a = Convert.ToDouble(money / item.Price);
                        availableQuantity = Convert.ToDecimal(Math.Round(a));
                        money -= availableQuantity * item.Price;
                        item.Quantity -= Convert.ToInt32(availableQuantity);
                        message = "You bought " + availableQuantity + " pieces of " + p3.ProductName + "";
                        break;
                    }
                    else
                    {
                        islemYapildi = false;
                        message = "You don't have enough money for this purchase";
                        break;
                    }
                }
            }

            p3.State = islemYapildi;

            db.tableOrder.Add(p3);

            // p3.bool= islemYapildi;

            userWallet.Money = money;
            db.SaveChanges();

            ViewBag.Message = message;

            return View();
        }






    }



}
