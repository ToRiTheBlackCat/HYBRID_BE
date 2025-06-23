using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.SignUp
{
    public class SignupUserGoogleRequest
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RoleId { get; set; }
    }
}
