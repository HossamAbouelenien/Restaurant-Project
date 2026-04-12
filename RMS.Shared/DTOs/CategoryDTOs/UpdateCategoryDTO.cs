using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.CategoryDTOs
{
    public class UpdateCategoryDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

    }
}
