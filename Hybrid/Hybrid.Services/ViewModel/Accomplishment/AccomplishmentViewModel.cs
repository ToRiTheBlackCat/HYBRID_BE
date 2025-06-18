using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Accomplishment
{
    public class AccomplishmentViewModel
    {
        public string StudentId { get; set; }

        public string StudentName { get; set; }

        public string MinigameId { get; set; }

        public double Score { get; set; }

        public int Duration { get; set; }

        public DateTime TakenDate { get; set; }
    }
}
