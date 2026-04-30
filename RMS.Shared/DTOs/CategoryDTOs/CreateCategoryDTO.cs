using System.ComponentModel.DataAnnotations;

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
