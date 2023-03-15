﻿using System.ComponentModel.DataAnnotations;

namespace Sales.Shared.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no piuede tener mas de {1} caractéres")]

        public string Name { get; set; } = null!;

        public ICollection<SubCategory>? SubCategories { get; set; }

        public int SubCategoryNumber => SubCategories == null ? 0 : SubCategories.Count;
        
    }
}
