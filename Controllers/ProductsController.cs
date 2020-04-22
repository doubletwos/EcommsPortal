using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EcommsPortal.Models;

namespace EcommsPortal.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.ProductType);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = db.Products
                .Include(s => s.Files)
                .SingleOrDefault(c => c.ProductId == id);


            if (product == null)
            {
                return HttpNotFound();
            }

          

            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {

             

            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "ProductTypeId", "Name");
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategorys, "ProductCategoryId", "Name");
            ViewBag.ProductSizeId = new SelectList(db.ProductSize, "ProductSizeId", "Name");


            return View();
        }

        // POST: Products/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase upload1, HttpPostedFileBase upload2)
        {
            if (ModelState.IsValid)
            {
                product.Files = new List<File>(); // created a new list called File

                if (upload1 != null)
                {
                    var avatar = new File
                    {
                        FileName = System.IO.Path.GetFileName(upload1.FileName),
                        FileType = FileType.Avatar,
                        ContentType = upload1.ContentType
                    };
                    using (var reader = new System.IO.BinaryReader(upload1.InputStream))
                    {
                        avatar.Content = reader.ReadBytes(upload1.ContentLength);
                    }
                    product.Files.Add(avatar);
                }


                if (upload2 != null)
                {

                    var avatar1 = new File
                    {
                        FileName = System.IO.Path.GetFileName(upload2.FileName),
                        FileType = FileType.Avatar1,
                        ContentType = upload2.ContentType
                    };
                    using (var reader = new System.IO.BinaryReader(upload2.InputStream))
                    {
                        avatar1.Content = reader.ReadBytes(upload2.ContentLength);
                    }
                    product.Files.Add(avatar1);

                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "ProductTypeId", "Name", product.ProductTypeId);
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
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "ProductTypeId", "Name", product.ProductTypeId);
            return View(product);
        }

        // POST: Products/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase upload1, HttpPostedFileBase upload2)
        {
            var productinDb = db.Products.Include(f => f.Files).Single(c => c.ProductId == product.ProductId);
            productinDb.Name = product.Name;
            productinDb.Description = product.Description;
            productinDb.Price = product.Price;
            productinDb.ProductType = product.ProductType;
            productinDb.ProductCategory = product.ProductCategory;
            productinDb.ProductSizes = product.ProductSizes;


            if (upload1 != null)
            {
                if (productinDb.Files.Any(f => f.FileType == FileType.Avatar))
                {
                    db.Files.Remove(productinDb.Files.First(f => f.FileType == FileType.Avatar));
                }
                var avatar = new File
                {
                    FileName = System.IO.Path.GetFileName(upload1.FileName),
                    FileType = FileType.Avatar,
                    ContentType = upload1.ContentType
                };
                using (var reader = new System.IO.BinaryReader(upload1.InputStream))
                {
                    avatar.Content = reader.ReadBytes(upload1.ContentLength);
                }
                productinDb.Files.Add(avatar);
            }


            if (upload2 != null )
            {
                if (productinDb.Files.Any(f => f.FileType == FileType.Avatar1))
                {
                    db.Files.Remove(productinDb.Files.First(f => f.FileType == FileType.Avatar1));
                }
                var avatar1 = new File
                {
                    FileName = System.IO.Path.GetFileName(upload2.FileName),
                    FileType = FileType.Avatar1,
                    ContentType = upload2.ContentType
                };
                using (var reader = new System.IO.BinaryReader(upload2.InputStream))
                {
                    avatar1.Content = reader.ReadBytes(upload2.ContentLength);
                }
                productinDb.Files.Add(avatar1);
            }
            db.Entry(productinDb).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

           
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
