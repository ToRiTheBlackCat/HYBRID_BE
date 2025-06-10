#nullable disable
using Hybrid;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Minigames
{
    public class UpdateMinigameRequest<T> where T : MinigameModels
    {
        [Required]
        public string MinigameId { get; set; }

        [Required]
        public string MinigameName { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public string TemplateId { get; set; }

        [Required]
        public List<T> GameData { get; set; }

        private string _teacherId = "";

        public string GetTeacherId()
        {
            return _teacherId;
        }

        public void SetTeacherId(string teacherId)
        {
            _teacherId = teacherId;
        }
    }
}
