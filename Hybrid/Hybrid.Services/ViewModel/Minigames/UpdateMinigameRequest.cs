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
        public string GameDataJson { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public string TemplateId { get; set; }

        public List<T> GameData => !Validate(null).Any() ?
            JsonSerializer.Deserialize<List<T>>(GameDataJson) : new List<T>();

        private string _teacherId = "";

        public string GetTeacherId()
        {
            return _teacherId;
        }

        public void SetTeacherId(string teacherId)
        {
            _teacherId = teacherId;
        }

        /// <summary>
        /// Validates the GameDataJson.
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(GameDataJson))
            {
                results.Add(new ValidationResult("GameDataJson is required.", new[] { nameof(GameDataJson) }));
                return results;
            }

            try
            {
                var data = JsonSerializer.Deserialize<List<T>>(GameDataJson);
                if (data == null)
                {
                    results.Add(new ValidationResult("GameDataJson could not be deserialized to the expected type.", new[] { nameof(GameDataJson) }));
                }
            }
            catch (JsonException ex)
            {
                results.Add(new ValidationResult($"Invalid JSON format: {ex.Message}", new[] { nameof(GameDataJson) }));
            }

            return results;
        }
    }
}
