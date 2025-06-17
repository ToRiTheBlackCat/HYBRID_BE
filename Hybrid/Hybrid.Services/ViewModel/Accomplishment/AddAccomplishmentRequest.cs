using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Accomplishment
{
    public class AddAccomplishmentRequest
    {
        [Required]
        public string MinigameId { get; set; }

        [Required]
        [Range(0d, 100d)]
        public double Percent { get; set; }

        [Required]
        [Range(0d, int.MaxValue)]
        public int DurationInSeconds { get; set; }

        [Required]
        public DateTime TakenDate { get; set; } = DateTime.UtcNow;

        public string StudentId = "";
    }
}
