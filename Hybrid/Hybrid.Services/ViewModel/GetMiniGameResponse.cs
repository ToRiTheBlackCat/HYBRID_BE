using Hybrid.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel
{
    public class GetMiniGameResponse
    {
        public string MinigameId { get; set; }

        public string MinigameName { get; set; }

        public string TeacherId { get; set; }

        public string TeacherName { get; set; }

        public string ThumbnailImage { get; set; }

        public string DataText { get; set; }

        public int Duration { get; set; }

        public int? ParticipantsCount { get; set; }

        public double? RatingScore { get; set; }

        public string TemplateId { get; set; }

        public string TemplateName { get; set; }

        public string CourseId { get; set; }
    }
}
