using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Accomplishment
{
    public class StudentAccomplishmentModel
    {
        public string StudentId { get; set; }
        public string MinigameId { get; set; }
        public string MinigameName { get; set; }
        public string ThumbnailImage { get; set; }
        public double Score { get; set; }
        public string TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime TakenDate { get; set; }
    }
}
