#nullable disable
using Hybrid.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel
{
    public class UpdateProfileResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UpdateProfileView ProfileView { get; set; }
    }

    public class UpdateProfileView
    {
        public string UserId { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public int YearOfBirth { get; set; }

        public string TierName { get; set; }

    }
}
