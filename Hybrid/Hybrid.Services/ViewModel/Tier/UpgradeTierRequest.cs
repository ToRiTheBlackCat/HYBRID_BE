﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Tier
{
    public class UpgradeTierRequest
    {
        [Required]
        public string UserId { get; set; }
    
        public long OrderCode {  get; set; }

        public bool IsTeacher { get; set; }

        [Required]
        public string TierId { get; set; }

    }
}
