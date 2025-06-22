using Hybrid.Repositories.Models;
using Hybrid.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Minigames
{
    public class GetAllMinigameResponse
    {
        public int PageNum { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public List<GetAllMinigameModel> Minigames { get; set; } = new List<GetAllMinigameModel>();

        public static GetAllMinigameResponse ToResponse(IEnumerable<Minigame> minigames, int pageSize, int pageNum = 1)
        {
            var count = minigames.Count();
            var totalPages = (int)Math.Ceiling((double)count / pageSize);
            var data = minigames.Skip((pageNum - 1) * pageSize).Take(pageSize)
                .Select(x => x.ToGetAllMinigameModel())
                .ToList() ?? new List<GetAllMinigameModel>();

            return new GetAllMinigameResponse()
            {
                PageNum = pageNum,
                TotalPages = totalPages,
                TotalCount = count,
                PageSize = pageSize,
                Minigames = data
            };
        }
    }

    public class GetAllMinigameModel
    {
        public string MinigameId { get; set; }

        public string MinigameName { get; set; }

        public string TeacherId { get; set; }

        public string TeacherName { get; set; }

        public string ThumbnailImage { get; set; }

        public int Duration { get; set; }

        public int ParticipantsCount { get; set; }

        public double? RatingScore { get; set; }

        public string TemplateId { get; set; }

        public string TemplateName { get; set; }

        public string CourseId { get; set; }
    }
}
