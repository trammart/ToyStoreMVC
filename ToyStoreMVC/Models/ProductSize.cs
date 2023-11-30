using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ToyStoreMVC.Models
{
	public class ProductSize
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int ProductId { get; set; }
		[ForeignKey("ProductId")]
		public Product Product { get; set; }

		[Required]
		public int SizeId { get; set; }
		[ForeignKey("SizeId")]
		public Size Size { get; set; }

		public long PriceIncrement { get; set; }
	}
}
