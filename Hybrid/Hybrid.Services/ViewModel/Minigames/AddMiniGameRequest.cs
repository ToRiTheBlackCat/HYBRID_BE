#nullable disable
using Hybrid;
using Hybrid.Repositories.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hybrid.Services.ViewModel.Minigames
{
    public class AddMiniGameRequest<T> where T : MinigameModels
    {
        [Required]
        public string MinigameName { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }

        public string TeacherId;

        [Required]
        public int Duration { get; set; }

        public string TemplateId;

        [Required]
        public string CourseId { get; set; }

        [Required]
        public List<T> GameData { get; set; }

        public Minigame ToMiniGame()
        {
            return new Minigame()
            {
                MinigameName = MinigameName,
                TeacherId = TeacherId,
                Duration = Duration,
                TemplateId = TemplateId,
                CourseId = CourseId,
            };
        }

        /// <summary>
        /// Validates the GameDataJson.
        /// </summary>
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    var results = new List<ValidationResult>();

        //    if (string.IsNullOrWhiteSpace(GameDataJson))
        //    {
        //        results.Add(new ValidationResult("GameDataJson is required.", new[] { nameof(GameDataJson) }));
        //        return results;
        //    }

        //    try
        //    {
        //        var data = JsonSerializer.Deserialize<List<T>>(GameDataJson);
        //        if (data == null)
        //        {
        //            results.Add(new ValidationResult("GameDataJson could not be deserialized to the expected type.", new[] { nameof(GameDataJson) }));
        //        }
        //    }
        //    catch (JsonException ex)
        //    {
        //        results.Add(new ValidationResult($"Invalid JSON format: {ex.Message}", new[] { nameof(GameDataJson) }));
        //    }

        //    return results;
        //}
    }
}

