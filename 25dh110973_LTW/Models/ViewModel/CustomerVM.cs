using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PagedList;
namespace _25dh110973_LTW.Models.ViewModel {
 public class ShopHomeVM { public List<_25dh110973_LTW.Models.Product> FeaturedProducts {get;set;} public IPagedList<_25dh110973_LTW.Models.Product> Products {get;set;} public string Search {get;set;} public string Sort {get;set;} }
 public class RegisterVM { [Required] public string Username {get;set;} [Required,MinLength(6),DataType(DataType.Password)] public string Password {get;set;} [Compare("Password"),DataType(DataType.Password)] public string ConfirmPassword {get;set;} [Required] public string CustomerName {get;set;} [Required,Phone] public string CustomerPhone {get;set;} [Required,EmailAddress] public string CustomerEmail {get;set;} [Required] public string CustomerAddress {get;set;} }
 public class LoginVM { [Required] public string Username {get;set;} [Required,DataType(DataType.Password)] public string Password {get;set;} }
 public class ProfileVM { public string Username {get;set;} [Required] public string CustomerName {get;set;} [Phone] public string CustomerPhone {get;set;} [EmailAddress] public string CustomerEmail {get;set;} public string CustomerAddress {get;set;} }
 public class ChangePasswordVM { [Required,DataType(DataType.Password)] public string OldPassword {get;set;} [Required,MinLength(6),DataType(DataType.Password)] public string NewPassword {get;set;} [Compare("NewPassword"),DataType(DataType.Password)] public string ConfirmPassword {get;set;} }
 public class CartItem { public int ProductID {get;set;} public string ProductName {get;set;} public string ProductImage {get;set;} public decimal Price {get;set;} public int Quantity {get;set;} public decimal Total {get{return Price*Quantity;}} }
}