using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.User
{
    public class FilterUsersResponse
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public DateTime CreatedDate { get; set; }

        public string RoleId { get; set; }

        public bool IsActive { get; set; }

    }
}
