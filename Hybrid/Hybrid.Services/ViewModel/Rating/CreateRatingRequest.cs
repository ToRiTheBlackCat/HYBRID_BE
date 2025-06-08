using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Rating
{
    public class CreateRatingRequest
    {
        [Required]
        public string StudentId { get; set; }
        [Required]

        public string MinigameId { get; set; }
        [Required]

        public double Score { get; set; }

        public string Comment { get; set; }
    }
}
