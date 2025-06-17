using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Accomplishment
{
    public class AddAccomplishmentResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public AccomplishmentViewModel? Model { get; set; }
    }
}
