using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Shared.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [Display(Name = "País")]
        [Required(ErrorMessage ="El campo {0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no piuede tener mas de {1} caractéres")]

        public string Name { get; set; } = null!;

    }
}
