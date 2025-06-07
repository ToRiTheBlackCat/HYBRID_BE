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


}
