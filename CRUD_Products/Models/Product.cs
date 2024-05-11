﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CRUD_Products.Models
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(100)]
        public string Brand { get; set; } = "";

        [MaxLength(100)]
        public string Category { get; set; } = "";

        [Precision(16,2)]
        public int price { get; set; }
        public string Discription { get; set; } = "";

        [MaxLength(100)]
        public string ImagFileName { get; set; } = "";

        public DateTime CreatedAt { get; set;}


    }
}
