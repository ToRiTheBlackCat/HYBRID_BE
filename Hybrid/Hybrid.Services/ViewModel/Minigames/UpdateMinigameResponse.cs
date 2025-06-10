using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Minigames
{
    public class UpdateMinigameResponse
    {
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = "";
        public AddMiniGameResponseModel? Model { get; set; }
    }

    public class UpdateMiniGameModel
    {
        public string MinigameId { get; set; }

        public string MinigameName { get; set; }

        public string TeacherId { get; set; }

        public string ThumbnailImage { get; set; }

        public int Duration { get; set; }

        public string TemplateId { get; set; }

        public string TemplateName { get; set; }

        public string CourseId { get; set; }
    }
}
