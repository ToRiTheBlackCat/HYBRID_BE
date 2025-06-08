#nullable disable
using Hybrid;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Profile
{
    public class GetProfileRequest
    {
        [Required]
        public string UserId { get; set; }
        public bool IsTeacher { get; set; }
    }
}
