using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Others
{
    public class ConfirmResestRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(maximumLength: 6, MinimumLength = 6)]
        public string ResetCode { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
