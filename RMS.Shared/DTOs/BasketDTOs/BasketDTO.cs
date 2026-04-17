using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.BasketDTOs
{
    public record BasketDTO(string Id, ICollection<BasketItemDto> Items);
}
