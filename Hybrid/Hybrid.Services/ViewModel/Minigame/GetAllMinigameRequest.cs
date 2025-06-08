using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Minigame
{
    public class GetAllMinigameRequest
    {
        public string MinigameName { get; set; } = "";
        public string TemplateId { get; set; } = "";
        public int PageNum { get; set; } = 1;
        public int PageSize { get; set; } = 9;
    }
}
