using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.IngredientDTOs
{
    public class CreateIngredientDTO
    {
        [Required]
        [MaxLength(20)]
        [MinLength(2)]
        public string Name { get; set; }

        public string Unit { get; set; }
    }
}
