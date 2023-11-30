using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToyStoreMVC.Areas.Admin.Views
{
    public static class ManageNavPages
    {
        public static string Brand => "Brand";

        public static string Category => "Category";

        public static string Company => "Company";

        public static string Order => "Order";

        public static string Product => "Product";

        public static string Size => "Size";

        public static string User => "User";

        public static string Dashboard => "Dashboard";

        public static string Role => "Role";

        public static string BrandNavClass(ViewContext viewContext) => PageNavClass(viewContext, Brand);

        public static string CategoryNavClass(ViewContext viewContext) => PageNavClass(viewContext, Category);

        public static string CompanyNavClass(ViewContext viewContext) => PageNavClass(viewContext, Company);

        public static string OrderNavClass(ViewContext viewContext) => PageNavClass(viewContext, Order);

        public static string ProductNavClass(ViewContext viewContext) => PageNavClass(viewContext, Product);

        public static string SizeNavClass(ViewContext viewContext) => PageNavClass(viewContext, Size);

        public static string UserNavClass(ViewContext viewContext) => PageNavClass(viewContext, User);

        public static string DashboardNavClass(ViewContext viewContext) => PageNavClass(viewContext, Dashboard);

        public static string RoleNavClass(ViewContext viewContext) => PageNavClass(viewContext, Role);
        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
