using System.ComponentModel.DataAnnotations;

namespace RMS.Shared.DTOs.IngredientDTOs
{
    public class CreateIngredientDTO
    {
        [Required]
        [MaxLength(20)]
        [MinLength(2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [MinLength(2)]
        public string ArabicName { get; set; } = string.Empty;

        public string Unit { get; set; }
    }
}
