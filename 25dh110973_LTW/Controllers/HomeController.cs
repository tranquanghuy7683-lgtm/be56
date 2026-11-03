using System;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using PagedList;
using _25dh110973_LTW.Models;
using _25dh110973_LTW.Models.ViewModel;
namespace _25dh110973_LTW.Controllers { public class HomeController:Controller {
 MyStoreEntities1 db=new MyStoreEntities1();
 public ActionResult Index(string search,string sort,int? page) { var q=db.Products.Include(x=>x.Category).AsQueryable(); if(!String.IsNullOrWhiteSpace(search)) q=q.Where(x=>x.ProductName.Contains(search)||x.ProductDecription.Contains(search)||x.Category.CategoryName.Contains(search));
 switch(sort) {case "price_asc":q=q.OrderBy(x=>x.ProductPrice);break;case "price_desc":q=q.OrderByDescending(x=>x.ProductPrice);break;case "name":q=q.OrderBy(x=>x.ProductName);break;default:q=q.OrderByDescending(x=>x.ProductID);break;}
 var featured=db.OrderDetails.GroupBy(x=>x.ProductID).Select(g=>new{Id=g.Key,Qty=g.Sum(x=>x.Quantity)}).OrderByDescending(x=>x.Qty).Take(10).ToList(); var ids=featured.Select(x=>x.Id).ToList();var products=db.Products.Where(x=>ids.Contains(x.ProductID)).ToList();var ranked=ids.Select(id=>products.FirstOrDefault(p=>p.ProductID==id)).Where(p=>p!=null).ToList();
 return View(new ShopHomeVM{FeaturedProducts=ranked,Products=q.ToPagedList(Math.Max(1,page??1),6),Search=search,Sort=sort}); }
 public ActionResult ProductList(int? id,int? page,string sort) {var q=db.Products.Include(x=>x.Category).AsQueryable();if(id.HasValue)q=q.Where(x=>x.CategoryID==id.Value);ViewBag.CategoryID=id;ViewBag.Sort=sort;switch(sort){case "price_asc":q=q.OrderBy(x=>x.ProductPrice);break;case "price_desc":q=q.OrderByDescending(x=>x.ProductPrice);break;default:q=q.OrderBy(x=>x.ProductID);break;}return View(q.ToPagedList(Math.Max(1,page??1),9));}
 public ActionResult ProductDetail(int id){var p=db.Products.Include(x=>x.Category).FirstOrDefault(x=>x.ProductID==id);if(p==null)return HttpNotFound();return View(p);}
 [ChildActionOnly] public PartialViewResult Categories(){return PartialView("_CategoriesPV",db.Categories.OrderBy(x=>x.CategoryName).ToList());}
 public ActionResult About(){return View();} public ActionResult Contact(){return View();}
 protected override void Dispose(bool disposing){if(disposing)db.Dispose();base.Dispose(disposing);}
 } }