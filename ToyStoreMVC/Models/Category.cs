using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ToyStoreMVC.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="Tên Loại")]
        [Required(ErrorMessage = "Không được để tên trống")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name="Mô tả")]
        [MaxLength(5000)]
        public string Discription { get; set; }
         
    }
}
