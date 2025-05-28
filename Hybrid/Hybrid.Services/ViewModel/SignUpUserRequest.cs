using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel
{
    public class SignUpUserRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string RoleId { get; set; }
        [Required]
        public string TierId { get; set; }

    }
    public class SignUpTeacher_StudentRequest
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public int YearOfBirth { get; set; }

    }
}
