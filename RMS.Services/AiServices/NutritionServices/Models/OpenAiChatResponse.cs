using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.AiServices.NutritionServices.Models
{
    internal class OpenAiChatResponse
    {
        public List<OpenAiChoice>? Choices { get; set; }
    }
}
