using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Rating
{
    public class GetAllRatingResponse
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }

        public string MinigameId { get; set; }

        public double Score { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
