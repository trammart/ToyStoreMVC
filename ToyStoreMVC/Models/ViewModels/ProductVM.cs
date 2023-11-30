using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToyStoreMVC.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> BrandList { get; set; }
        public IEnumerable<SelectListItem> SizeList { get; set; } 
		public List<int> SelectedSizes { get; set; } // Danh sách các kích thước đã chọn
        public Dictionary<int, long> SizePrices { get; set; } // Lưu giá sản phẩm cho mỗi kích thước
    }
}
