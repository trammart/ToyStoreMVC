using System.Collections.Generic;

namespace ToyStoreMVC.Models.ViewModels
{
	public class HomeViewModel
	{
		public IEnumerable<Product> Products { get; set; }
		public ApplicationUser User { get; set; }

		public HomeViewModel()
		{
			Products = new List<Product>(); 
			User = new ApplicationUser();
		}
	}
}
