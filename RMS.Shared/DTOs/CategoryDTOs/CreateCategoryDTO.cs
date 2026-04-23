using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.CategoryDTOs
{
    public class CreateCategoryDTO
    {

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = default!;

        [Required]
        [MaxLength(100)]
        public string ArabicName { get; set; } = default!;



    }
}
