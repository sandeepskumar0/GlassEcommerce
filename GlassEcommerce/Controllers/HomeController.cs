using GlassEcommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlassEcommerce.Controllers
{
    public class HomeController : Controller
    {
        private glassesdbEntities db = new glassesdbEntities();
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

       

        public ActionResult products(int? id)
        {

            ProductModel p = new ProductModel();

            p.cat = db.Categories.ToList();


            if (id == null)

            {
                p.pro = db.Products.ToList();
            }
            else
            {
                p.pro = db.Products.Where(z => z.Cat_Fid == id).ToList();
            }

            return View(p);
        }










       



    }
}