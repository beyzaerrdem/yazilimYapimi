using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using yazilimYapimi.Models.Entity;


namespace yazilimYapimi.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        MvcDbProjeEntities db = new MvcDbProjeEntities();

        public ActionResult Report()
        {
            var model = db.tableOrder.ToList();
            return View (model);
        }

        public ActionResult GeneratePdf()
        {
            return new Rotativa.ActionAsPdf("Report");
        }
        

    }
}