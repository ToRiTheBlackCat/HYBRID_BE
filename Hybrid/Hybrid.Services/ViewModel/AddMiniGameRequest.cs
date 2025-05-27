using Hybrid.Repositories.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hybrid.Services.ViewModel
{
    public class AddMiniGameRequest<T>
    {
        //[Required]
        //public string MinigameId { get; set; }

        [Required]
        public string MinigameName { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }

        [Required]
        public string TeacherId { get; set; }

        [Required]
        public string GameDataJson { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public string TemplateId { get; set; }

        [Required]
        public string CourseId { get; set; }

        public List<ConjunctionQuestion> GameData =>
            string.IsNullOrEmpty(GameDataJson)
            ? new List<ConjunctionQuestion>()
            : JsonSerializer.Deserialize<List<ConjunctionQuestion>>(GameDataJson) ?? new List<ConjunctionQuestion>();


        public Minigame ToMiniGame()
        {
            return new Minigame()
            {
                MinigameName = this.MinigameName,
                TeacherId = this.TeacherId,
                Duration = this.Duration,
                TemplateId = this.TemplateId,
                CourseId = this.CourseId,
            };
        }
    }

    [XmlRoot("questions")]
    public class QuestionList<T>
    {
        [XmlElement("question")]
        public List<T> Questions { get; set; }
    }

    [XmlRoot("question")]
    public class ConjunctionQuestion
    {
        [XmlElement("term")]
        public string Term { get; set; }

        [XmlElement("definition")]
        public string Definition { get; set; }
    }
}
