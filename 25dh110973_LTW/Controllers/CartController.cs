using System;using System.Collections.Generic;using System.Linq;using System.Web.Mvc;using _25dh110973_LTW.Models;using _25dh110973_LTW.Models.ViewModel;
namespace _25dh110973_LTW.Controllers { public class CartController:Controller {MyStoreEntities1 db=new MyStoreEntities1();
 List<CartItem> Cart {get{var c=Session["Cart"] as List<CartItem>;if(c==null){c=new List<CartItem>();Session["Cart"]=c;}return c;}}
 public ActionResult Index(){return View(Cart);}
 [HttpPost,ValidateAntiForgeryToken] public ActionResult Add(int id,int quantity=1){var p=db.Products.Find(id);if(p==null)return HttpNotFound();quantity=Math.Max(1,Math.Min(99,quantity));var item=Cart.FirstOrDefault(x=>x.ProductID==id);if(item==null)Cart.Add(new CartItem{ProductID=id,ProductName=p.ProductName,ProductImage=p.ProductImage,Price=p.ProductPrice,Quantity=quantity});else item.Quantity=Math.Min(99,item.Quantity+quantity);return RedirectToAction("Index");}
 [HttpPost,ValidateAntiForgeryToken] public ActionResult Update(int id,int quantity){var item=Cart.FirstOrDefault(x=>x.ProductID==id);if(item!=null){if(quantity<=0)Cart.Remove(item);else item.Quantity=Math.Min(99,quantity);}return RedirectToAction("Index");}
 [HttpPost,ValidateAntiForgeryToken] public ActionResult Remove(int id){Cart.RemoveAll(x=>x.ProductID==id);return RedirectToAction("Index");}
 } }