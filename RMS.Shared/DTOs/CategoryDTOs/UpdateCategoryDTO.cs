using System.ComponentModel.DataAnnotations;

namespace RMS.Shared.DTOs.CategoryDTOs
{
    public class UpdateCategoryDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ArabicName { get; set; } = string.Empty;
    }
}
