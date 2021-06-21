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
            var product = db.tableProduct.FirstOrDefault(p => p.ProductName == tableProduct.ProductName && p.Price == tableProduct.Price && p.UserID == tableProduct.UserID);
            if (product==null)
            {
                db.tableProduct.Add(tableProduct);
            }
            else
            {
                product.Quantity += tableProduct.Quantity;
                tableProduct = product;
            }
           
            p2.Confirmed = true;
            db.SaveChanges();
            var result = db.tableOrder.Where(o => o.State == false && o.ProductName==p2.ProductName&&o.Quantity<tableProduct.Quantity).ToList();
            foreach (var item in result)
            {
                
                OrderProduct(item);
                //db.tableOrder.Remove(item);
            }
            
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
            bool update = true;
            if (p3.CustomerID == null)
            {
                int userId = Convert.ToInt32(Session["UserID"]);
                p3.CustomerID = userId;
                p3.Time = DateTime.Now;
                update = false;
            }
            else
                p3 = db.tableOrder.Find(p3.ID);
            
            
            var userWallet = db.tableWallet.FirstOrDefault(x => x.UserID == p3.CustomerID);
            var muhWallet = db.tableWallet.FirstOrDefault(x => x.tableUser.UserName == "accounting");
            var productList = db.tableProduct.Where(x => x.ProductName == p3.ProductName && x.Price==p3.Price); // fiyata göre sıralanması 
            int? q = p3.Quantity;
            var money =userWallet.Money;
            decimal availableQuantity = 0;

            string message = "";

            bool completed = false;

            foreach (var item in productList)
            {
                var supplierWallet = db.tableWallet.FirstOrDefault(x=>x.UserID==item.UserID);
                if (item.Quantity > 0)
                {
                    
                    if (priceCentesimal(item.Price * q) <= money)
                    {
                        if (item.Quantity >= q)
                        {

                            item.Quantity -= q;
                            money -= priceCentesimal(item.Price * q);
                            supplierWallet.Money += item.Price * q;
                            muhWallet.Money += (item.Price * q) / 100;
                            message = "Your purchase has been made.";
                            completed = true;
                            break;
                        }
                        else
                        {
                            q -= item.Quantity;
                            money -= priceCentesimal(item.Price * item.Quantity);
                            supplierWallet.Money += (item.Price * item.Quantity);
                            muhWallet.Money += (item.Price * item.Quantity) / 100;
                            message = "You bought " + item.Quantity + " pieces of " + p3.ProductName + "";
                            completed = true;
                            item.Quantity = 0;
                        }
                    }
                    else if (money / item.Price >= 1)
                    {
                        double a = Convert.ToDouble(money / item.Price);
                        availableQuantity = Convert.ToDecimal(Math.Round(a));
                        money -= priceCentesimal(availableQuantity * item.Price);
                        supplierWallet.Money += availableQuantity * item.Price;
                        muhWallet.Money += (availableQuantity * item.Price) / 100;
                        item.Quantity -= Convert.ToInt32(availableQuantity);
                        message = "You bought " + availableQuantity + " pieces of " + p3.ProductName + "";
                        completed = true;
                        break;
                    }
                    else
                    {
                        completed = false;
                        message = "You don't have enough money for this purchase";
                        break;
                    }
                }
            }

            p3.State = completed;

            if (!update)
            {
                db.tableOrder.Add(p3);
            }

            

            userWallet.Money = money;
            db.SaveChanges();

            ViewBag.Message = message;

            return View();
        }
        decimal? priceCentesimal(decimal? price)  //yüzdebir
        {
            decimal? centesimal = price / 100;
            return price + centesimal;
        }





    }



}
