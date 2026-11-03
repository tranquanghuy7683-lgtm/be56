
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using _25dh110973_LTW.Models;
using _25dh110973_LTW.Models.ViewModel;

namespace _25dh110973_LTW.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private MyStoreEntities1 db = new MyStoreEntities1();

        // Danh sách sản phẩm
        public ActionResult Index(string searchTerm, decimal? minPrice,
            decimal? maxPrice, string sortOrder, int? page)
        {
            var model = new ProductSearchVM
            {
                SearchTerm = searchTerm,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SortOrder = sortOrder
            };

            var products = db.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p =>
                    p.ProductName.Contains(searchTerm) ||
                    (p.ProductDecription != null &&
                     p.ProductDecription.Contains(searchTerm)) ||
                    p.Category.CategoryName.Contains(searchTerm));
            }

            if (minPrice.HasValue)
            {
                products = products.Where(p =>
                    p.ProductPrice >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p =>
                    p.ProductPrice <= maxPrice.Value);
            }

            switch (sortOrder)
            {
                case "name_asc":
                    products = products.OrderBy(p => p.ProductName);
                    break;

                case "name_desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;

                case "price_asc":
                    products = products.OrderBy(p => p.ProductPrice);
                    break;

                case "price_desc":
                    products = products.OrderByDescending(p => p.ProductPrice);
                    break;

                default:
                    products = products.OrderBy(p => p.ProductName);
                    break;
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;

            model.Products = products.ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        // Xem chi tiết sản phẩm
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = db.Products.Find(id);

            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // GET: Thêm sản phẩm
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(
                db.Categories, "CategoryID", "CategoryName");

            return View();
        }

        // POST: Thêm sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "ProductID,CategoryID,ProductName,ProductDecription,ProductPrice,ProductImage,UploadImg")]
            Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.UploadImg != null &&
                    product.UploadImg.ContentLength > 0)
                {
                    string filename = Path.GetFileName(
                        product.UploadImg.FileName);

                    string savePath = "~/Content/images/";
                    string physicalPath = Server.MapPath(savePath);

                    if (!Directory.Exists(physicalPath))
                        Directory.CreateDirectory(physicalPath);

                    product.ProductImage = savePath + filename;

                    product.UploadImg.SaveAs(
                        Path.Combine(physicalPath, filename));
                }

                try
                {
                    db.Products.Add(product);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    var errors = ex.EntityValidationErrors
                        .SelectMany(e => e.ValidationErrors)
                        .Select(e => e.PropertyName + ": " + e.ErrorMessage);

                    string message = string.Join("; ", errors);

                    ModelState.AddModelError("", message);

                    db.Entry(product).State = EntityState.Detached;
                }
            }

            ViewBag.CategoryID = new SelectList(
                db.Categories, "CategoryID", "CategoryName",
                product.CategoryID);

            return View(product);
        }

        // GET: Sửa sản phẩm
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = db.Products.Find(id);

            if (product == null)
                return HttpNotFound();

            ViewBag.CategoryID = new SelectList(
                db.Categories, "CategoryID", "CategoryName",
                product.CategoryID);

            return View(product);
        }

        // POST: Sửa sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "ProductID,CategoryID,ProductName,ProductDecription,ProductPrice,UploadImg")]
            Product product)
        {
            if (ModelState.IsValid)
            {
                Product existing = db.Products.Find(product.ProductID);

                if (existing == null)
                    return HttpNotFound();

                existing.CategoryID = product.CategoryID;
                existing.ProductName = product.ProductName;
                existing.ProductDecription = product.ProductDecription;
                existing.ProductPrice = product.ProductPrice;

                if (product.UploadImg != null &&
                    product.UploadImg.ContentLength > 0)
                {
                    string filename = Path.GetFileName(
                        product.UploadImg.FileName);

                    string savePath = "~/Content/images/";
                    string physicalPath = Server.MapPath(savePath);

                    if (!Directory.Exists(physicalPath))
                        Directory.CreateDirectory(physicalPath);

                    product.UploadImg.SaveAs(
                        Path.Combine(physicalPath, filename));

                    existing.ProductImage = savePath + filename;
                }

                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    var errors = ex.EntityValidationErrors
                        .SelectMany(e => e.ValidationErrors)
                        .Select(e => e.PropertyName + ": " + e.ErrorMessage);

                    ModelState.AddModelError(
                        "", string.Join("; ", errors));
                }
            }

            ViewBag.CategoryID = new SelectList(
                db.Categories, "CategoryID", "CategoryName",
                product.CategoryID);

            return View(product);
        }

        // GET: Xóa sản phẩm
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = db.Products.Find(id);

            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // POST: Xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);

            if (product == null)
                return HttpNotFound();

            db.Products.Remove(product);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
