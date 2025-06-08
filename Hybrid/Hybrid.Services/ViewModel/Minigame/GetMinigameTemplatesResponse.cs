using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Minigame
{
    public class GetMinigameTemplatesResponse
    {
        public int ItemCount { get; set; }
        public List<GetMinigameTemplatesModel> MinigameTemplates { get; set; }
    }

    public class GetMinigameTemplatesModel
    {
        public string TemplateId { get; set; }

        public string TemplateName { get; set; }

        public string Image { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }
    }
}
