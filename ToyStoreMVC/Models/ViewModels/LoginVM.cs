using System.Collections.Generic;

namespace ToyStoreMVC.Models.ViewModels
{
    public class LoginVM
    {
        public IEnumerable<Product> Products { get; set; } 
        public ApplicationUser User { get; set; }
    }
}
