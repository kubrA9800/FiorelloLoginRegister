﻿using System.ComponentModel.DataAnnotations;

namespace FiorelloBackend.Models
{
    public class Category:BaseEntity
    {
        [Required(ErrorMessage ="Can't be empty")]
        public string? Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
