using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using yazilimYapimi.Models.Entity;
using ChoETL;

namespace yazilimYapimi.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        MvcDbProjeEntities db = new MvcDbProjeEntities();

        public ActionResult Report()
        {
            int userId=(int)Session["UserID"];
            var model = db.tableOrder.Where(o => o.CustomerID == userId).ToList();
            string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)+"\\" + userId}.csv";
            try
            {
         
                using (var w = new ChoCSVWriter<tableOrder>(path).WithFirstLineHeader())
                {
                    w.Write(model);
                    w.Dispose();
                }

                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            try
            {
                byte[] byteArray = FileToByteArray(path);
                return new FileContentResult(byteArray, "application/octet-stream");
            }
            catch (Exception e)
            {
                return View(model);
            }
            //return View(model);
        }
        public byte[] FileToByteArray(string fileName)
        {
            byte[] buff = null;
            FileStream fs = new FileStream(fileName,
                FileMode.Open,
                FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            buff = br.ReadBytes((int)numBytes);
            br.Close();
            fs.Close();
            return buff;
        }

    }
}