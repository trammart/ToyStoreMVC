using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ToyStoreMVC.Models
{
    public class Size
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="Tên loại kích thước")]
        public string Name { get; set; } 
	}
}
