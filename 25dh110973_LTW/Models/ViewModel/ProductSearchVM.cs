using System.Collections.Generic;
using _25dh110973_LTW.Models;
using PagedList;

namespace _25dh110973_LTW.Models.ViewModel
{
    public class ProductSearchVM
    {
        // Tieu chi tim kiem theo ten, mo ta hoac danh muc
        public string SearchTerm { get; set; }

        // Tieu chi tim kiem theo khoang gia
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // Thu tu sap xep
        public string SortOrder { get; set; }

        // Danh sach san pham phan trang
        public IPagedList<Product> Products { get; set; }
    }
}
