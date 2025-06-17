using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hybrid.Services.ViewModel.Minigames
{
    public interface IMinigameWithPicture
    {
        public string ImagePath { get; set; }
        public IFormFile? Image { get; set; }
    }

    public class MinigameModels
    {
    }

    [XmlRoot("questions")]
    public class QuestionList<T>
    {
        [XmlElement("question")]
        public List<T> Questions { get; set; }
    }

    [XmlRoot("question")]
    public class ConjunctionQuestion : MinigameModels
    {
        [XmlElement("term")]
        [Required]
        public string Term { get; set; }

        [XmlElement("definition")]
        [Required]
        public string Definition { get; set; }
    }

    [XmlRoot("question")]
    public class QuizQuestion : MinigameModels
    {
        [XmlElement("header")]
        [Required]
        public string Header { get; set; }

        [XmlElement("options")]
        [Required]
        public List<string> Options { get; set; }

        [XmlElement("answers")]
        [Required]
        public List<int> AnswerIndexes { get; set; }
    }

    [XmlRoot("question")]
    public class AnagramQuestion : MinigameModels
    {
        [XmlElement("word")]
        [Required]
        public string Word { get; set; }
    }

    [XmlRoot("question")]
    public class RandomCardQuestion : MinigameModels, IMinigameWithPicture
    {
        [XmlElement("text")]
        [Required]
        public string Text { get; set; }

        private string imagePath = string.Empty;

        [XmlElement("image")]
        public string ImagePath { get => imagePath; set => imagePath = value; }

        [Required]
        [XmlIgnore]
        public IFormFile Image { get; set; }
    }

    [XmlRoot("question")]
    public class SpellingQuestion : MinigameModels, IMinigameWithPicture
    {
        [XmlElement("word")]
        [Required]
        public string Word { get; set; }

        [XmlElement("image")]
        public string ImagePath { get; set; } = string.Empty;

        [XmlIgnore]
        public IFormFile? Image { get; set; }
    }

    [XmlRoot("question")]
    public class FlashCardQuestion : MinigameModels
    {
        [XmlElement("front")]
        [Required]
        public string Front { get; set; }

        [XmlElement("back")]
        [Required]
        public string Back { get; set; }
    }
}
