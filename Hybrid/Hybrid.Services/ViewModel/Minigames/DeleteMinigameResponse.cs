﻿using Hybrid.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Minigames
{
    public class DeleteMinigameResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Minigame Minigame { get; set; }
    }
}
