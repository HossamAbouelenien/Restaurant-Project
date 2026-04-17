using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.BasketDTOs
{
    public record BasketItemDto(
         int Id,
         string Name,
         string PictureUrl,
         [Range(1,int.MaxValue)]
        decimal Price,
         [Range(1,100)]
        int Quantity
         );
}
