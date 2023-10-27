using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GlassEcommerce.Models;

namespace GlassEcommerce.Controllers
{
    [RoutePrefix("Products")]
    public class ProductsController : Controller
    {
        private glassesdbEntities db = new glassesdbEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }


        public ActionResult PDetails(int id)
        {
            Product product = db.Products.FirstOrDefault(p => p.Product_id== id);
            if (product == null)
            {
                return HttpNotFound(); // Handle the case when the product is not found
            }

            return View(product);
        }
        [Route("Search")]
        public ActionResult Search(string query)
        {
            // Query the database or any data source to get filtered products based on the search query
            var filteredProducts = db.Products.Where(p => p.Product_name.Contains(query)).ToList();

            return View("Products", filteredProducts);
        }

        public ActionResult Cart()
        {


            return View();
        }
        public ActionResult AddToCart(int id)
        {

            List<Product> list;
            if (Session["myCart"] == null)
            {
                list = new List<Product>();
            }
            else
            {
                list = (List<Product>)Session["myCart"];
            }
            list.Add(db.Products.Where(p => p.Product_id == id).FirstOrDefault());
            list[list.Count - 1].PRO_QUANTITY = 1;

            Session["myCart"] = list;
            return RedirectToAction("cart");
        }
        //public ActionResult Cart()
        //{
        //    // Retrieve carted items from the session
        //    List<Product> cart = Session["myCart"] as List<Product>;

        //    return View(cart);
        //}

        //public ActionResult AddToCart(int id)
        //{
        //    // Add products to the cart (your existing logic)
        //    // ...

        //    return RedirectToAction("Cart");
        //}

        //public ActionResult CustomerDetails()
        //{
        //    // Display a view for collecting customer details
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult ProcessCustomerDetails(User user)
        //{
        //    // Store customer details in the database or wherever you prefer
        //    // ...

        //    // Retrieve carted items from the session
        //    List<Product> cart = Session["myCart"] as List<Product>;

        //    // Pass cart and customer details to the admin view
        //    ViewBag.Cart = cart;
        //    ViewBag.User = user;

        //    return RedirectToAction("AdminPage");
        //}

        //public ActionResult AdminPage()
        //{
        //    return View();
        //}

        public ActionResult MinusFromCart(int RowNo)
        {
            List<Product> list = (List<Product>)Session["myCart"];
            list[RowNo].PRO_QUANTITY--;
            Session["myCart"] = list;
            return RedirectToAction("cart");
        }
        public ActionResult PlusToCart(int RowNo)
        {
            List<Product> list = (List<Product>)Session["myCart"];
            list[RowNo].PRO_QUANTITY++;
            Session["myCart"] = list;
            return RedirectToAction("cart");
        }
        public ActionResult RemoveFromCart(int RowNo)
        {
            List<Product> list = (List<Product>)Session["myCart"];
            list.RemoveAt(RowNo);
            Session["myCart"] = list;
            return RedirectToAction("cart");
        }
















        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Cat_Fid = new SelectList(db.Categories, "Cat_id", "Cat_Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Products.Add(product);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            if (ModelState.IsValid)
            {

                product.Pro_pic.SaveAs(Server.MapPath("~/ProPic/" + product.Pro_pic.FileName));

                if (product.Pro_pic.FileName != "")
                {
                    product.Product_pic = "~/ProPic/" + product.Pro_pic.FileName;
                    db.Products.Add(product);
                    db.SaveChanges();
                }
                else
                {
                    product.Product_pic = null;
                }

                return RedirectToAction("Index");
            }
            ViewBag.Cat_Fid = new SelectList(db.Categories, "Cat_id", "Cat_Name", product.Cat_Fid);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cat_Fid = new SelectList(db.Categories, "Cat_id", "Cat_Name", product.Cat_Fid);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Product_id,Product_name,Product_desc,Product_pic,Product_price,Cat_Fid")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Cat_Fid = new SelectList(db.Categories, "Cat_id", "Cat_Name", product.Cat_Fid);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
