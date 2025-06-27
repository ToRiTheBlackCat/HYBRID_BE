using Hybrid.Repositories.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        public virtual string IsValidInput()
        {
            return "";
        }
        public virtual void GenerateGame() { }
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

    [XmlRoot("question")]
    public class CompletionQuestion : MinigameModels
    {
        [XmlElement("sentence")]
        [Required]
        public string Sentence { get; set; }

        [XmlElement("options")]
        [Required]
        public List<string> Options { get; set; }

        [XmlElement("answers")]
        [Required]
        public List<int> AnswerIndexes { get; set; }
    }

    [XmlRoot("question")]
    public class PairingQuestion : MinigameModels
    {
        [XmlElement("words")]
        [Required]
        public List<string> Words { get; set; }
    }

    [XmlRoot("question")]
    public class RestorationQuestion : MinigameModels
    {
        [XmlElement("words")]
        [Required]
        public List<string> Words { get; set; }
    }

    [XmlRoot("question")]
    public class WordFindQuestion : MinigameModels
    {
        [XmlElement("hint")]
        [Required]
        public string Hint { get; set; }

        [XmlElement("words")]
        [Required]
        [MinLength(1)]
        public List<string> Words { get; set; }

        [XmlElement("dimension")]
        [Range(minimum: 8, maximum: 20)]
        public int DimensionSize { get; set; } = 8;

        #region Generation Logic
        [XmlElement("array")]
        public string CharArray = "";

        [XmlIgnore]
        private int accessTime = 0;

        [XmlIgnore]
        private static readonly Random _rand = new();

        public override void GenerateGame()
        {
            var ordered = Words.OrderBy(x => !string.IsNullOrEmpty(x) ? x.Length : 0);
            var longestWord = ordered.Last();
            var shortestWord = ordered.First();
            if (shortestWord.Length <= 2)
            {
                throw new Exception($"Words must be longer than 2 characters.");
            }
            if (longestWord.Length > DimensionSize)
            {
                throw new Exception($"'{longestWord}' Character length can't be longer than {nameof(DimensionSize)}({DimensionSize})");
            }

            var grid = new char[this.DimensionSize, this.DimensionSize];

            // Initialize with placeholders
            for (int i = 0; i < this.DimensionSize; i++)
                for (int j = 0; j < this.DimensionSize; j++)
                    grid[i, j] = '.';

            foreach (var word in this.Words)
            {
                if (!PlaceWord(grid, word.ToUpper(), this.DimensionSize))
                {
                    //Console.WriteLine($"Failed to place word: {word}");
                }
            }

            // Fill empty cells with random uppercase letters
            for (int i = 0; i < DimensionSize; i++)
            {
                for (int j = 0; j < DimensionSize; j++)
                {
                    Console.Write(grid[i, j] + " ");
                    if (grid[i, j] == '.')
                        grid[i, j] = (char)('A' + _rand.Next(26));
                }
                Console.WriteLine();
            }

            // Convert 2D grid to single string (row-major)
            accessTime++;
            //return FlattenGrid(grid, n);
            CharArray = FlattenGrid(grid, this.DimensionSize);
        }

        private static bool PlaceWord(char[,] grid, string word, int n)
        {
            var directions = new (int dx, int dy)[]
            {
            (1, 0),   // → right
            (0, 1),   // ↓ down
            (1, 1),   // ↘ diagonal
            (-1, 1),  // ↙ diagonal
            };

            for (int attempt = 0; attempt < 100; attempt++)
            {
                var (dx, dy) = directions[_rand.Next(directions.Length)];
                int row = _rand.Next(n);
                int col = _rand.Next(n);

                if (CanPlace(grid, word, row, col, dx, dy, n))
                {
                    for (int i = 0; i < word.Length; i++)
                    {
                        grid[row + i * dy, col + i * dx] = word[i];
                    }
                    return true;
                }
            }

            return false; // placement failed
        }

        private static bool CanPlace(char[,] grid, string word, int row, int col, int dx, int dy, int n)
        {
            for (int i = 0; i < word.Length; i++)
            {
                int r = row + i * dy;
                int c = col + i * dx;

                if (r < 0 || r >= n || c < 0 || c >= n)
                    return false;

                if (grid[r, c] != '.' && grid[r, c] != word[i])
                    return false;
            }

            return true;
        }

        private static string FlattenGrid(char[,] grid, int n)
        {
            var chars = new char[n * n];
            int index = 0;
            for (int i = 0; i < n; i++)         // row-major
            {
                for (int j = 0; j < n; j++)
                {
                    chars[index++] = grid[i, j];
                }
            }

            return new string(chars);
        }
        #endregion
    }

    [XmlRoot("question")]
    public class TrueFalseQuestion : MinigameModels
    {
        [XmlElement("statement")]
        [Required]
        public List<string> Statement { get; set; }

        [XmlElement("answer")]
        [Required]
        public bool Answer { get; set; }
    }

    [XmlRoot("question")]
    public class CrossWordsQuestion : MinigameModels
    {
        [XmlElement("words")]
        [Required]
        public List<string> Words { get; set; }

        [XmlElement("clues")]
        [Required]
        public List<string> Clues { get; set; }

        [XmlElement("dimension")]
        [Required]
        [Range(8, int.MaxValue)]
        public int DimensionSize { get; set; } = 8;

        [XmlElement("array")]
        public string Array = "";

        [XmlElement("positions")]
        public List<string> StartPosition = new List<string>();

        #region Generation logic
        private char[,] grid;
        private List<string> placedWords = new();
        private Random _random = new();
        private const int minWordLength = 2;

        public override string IsValidInput()
        {
            if (Words == null || Words.Count == 0)
            {
                return $"{nameof(this.Words)} can't be empty.";
            }

            if (Words.Count != Clues.Count)
            {
                return $"Number of Words({Words.Count}) doesn't match number of Clues({Clues.Count})";
            }

            var orderedLength = Words.OrderBy(x => x.Length);
            if (orderedLength.First().Length <= minWordLength)
            {
                return $"{nameof(this.Words)} can't have words with length shorter than {minWordLength}.";
            }
            else if (orderedLength.Last().Length > DimensionSize)
            {
                return $"{nameof(this.Words)} can't have words with length longer than {DimensionSize}.";
            }

            return string.Empty;
        }

        /// <summary>
        /// Generate the CrossWords game
        /// </summary>
        public override void GenerateGame()
        {
            if (Words == null || Words.Count == 0) return;

            grid = new char[DimensionSize, DimensionSize];
            // Initialize grid with empty space
            for (int i = 0; i < DimensionSize; i++)
                for (int j = 0; j < DimensionSize; j++)
                    grid[i, j] = '.';

            Words = Words.OrderByDescending(w => w.Length).ToList(); // Place longest words first

            // Place the first word horizontally in the middle
            if (CanPlaceWord(Words[0].ToUpper(), DimensionSize / 2, (DimensionSize - Words[0].Length) / 2, true))
            {
                PlaceWord(Words[0].ToUpper(), DimensionSize / 2, (DimensionSize - Words[0].Length) / 2, true);
            }
            else
            {
                PlaceWord(Words[0].ToUpper(), DimensionSize / 2, 0, true);
            }

            placedWords.Add(Words[0].ToUpper());

            for (int w = 1; w < Words.Count; w++)
            {
                var word = Words[w].ToUpper();
                if (!TryPlaceWord(word))
                    Console.WriteLine($"Could not place word: {word}");
                else
                {
                    //Console.WriteLine($"Place word: {word}");
                    //PrintGrid();
                }
            }

            this.Array = PrintGrid();
        }

        /// <summary>
        /// Method for placing the word on the grid
        /// </summary>
        /// <param name="word">The word to be placed</param>
        /// <returns></returns>
        private bool TryPlaceWord(string word)
        {
            var shuffledPlacedWords = placedWords.OrderBy(_ => _random.Next()).ToList();
            foreach (var placed in shuffledPlacedWords)
            {
                // Get pairs of mathing character indexes for placedWord and newWord (placedWordIndex, newWordIndex)
                var matches = Enumerable.Range(0, placed.Length)
                    .SelectMany(i => Enumerable.Range(0, word.Length)
                    .Where(j => placed[i] == word[j])
                    .Select(j => (i, j)))
                    .OrderBy(_ => _random.Next());

                foreach (var (placedIndex, wordIndex) in matches)
                {
                    for (int r = 0; r < DimensionSize; r++)
                    {
                        for (int c = 0; c < DimensionSize; c++)
                        {
                            if (grid[r, c] != placed[placedIndex])
                                continue;

                            // Randomize orientation choice
                            foreach (var isHorizontal in new[] { true, false }.OrderBy(_ => _random.Next()))
                            {
                                int startRow = isHorizontal ? r : r - wordIndex;
                                int startCol = isHorizontal ? c - wordIndex : c;

                                if (CanPlaceWord(word, startRow, startCol, isHorizontal))
                                {
                                    //Console.WriteLine($"StartRow {startRow}");
                                    //Console.WriteLine($"StartColumn {startCol}");
                                    //Console.WriteLine($"Matched {placed[placedIndex]}: ({placedIndex}, {wordIndex})");
                                    //Console.WriteLine($"Placed {placed}");
                                    //Console.WriteLine($"Word {word}");
                                    //Console.WriteLine($"Matching index: ({r}, {c})");


                                    PlaceWord(word, startRow, startCol, isHorizontal);
                                    placedWords.Add(word);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;

        }

        /// <summary>
        /// Checks if the word can actually be placed on the grid
        /// </summary>
        /// <param name="word">The word to place</param>
        /// <param name="row">Vertical index of the grid</param>
        /// <param name="col">Horizontal index of the grid</param>
        /// <param name="isHorizontal">Whether to place the word horizontally or vertically</param>
        private bool CanPlaceWord(string word, int row, int col, bool isHorizontal)
        {
            for (int i = 0; i < word.Length; i++)
            {
                int r = isHorizontal ? row : row + i;
                int c = isHorizontal ? col + i : col;

                if (r < 0 || r >= DimensionSize || c < 0 || c >= DimensionSize)
                    return false;

                if (grid[r, c] != '.' && grid[r, c] != word[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Place word into the grid as characters
        /// </summary>
        /// <param name="word">The word to place</param>
        /// <param name="row">Vertical index of the grid</param>
        /// <param name="col">Horizontal index of the grid</param>
        /// <param name="isHorizontal">Whether to place the word horizontally or vertically</param>
        private void PlaceWord(string word, int row, int col, bool isHorizontal)
        {

            StartPosition.Add($"{row},{col}");
            for (int i = 0; i < word.Length; i++)
            {
                int r = isHorizontal ? row : row + i;
                int c = isHorizontal ? col + i : col;
                grid[r, c] = word[i];
            }
        }

        /// <summary>
        /// Print array to viewable Grid in Console
        /// </summary>
        public string PrintGrid()
        {
            var chars = new char[DimensionSize * DimensionSize];
            var index = 0;
            for (int i = 0; i < DimensionSize; i++)
            {
                for (int j = 0; j < DimensionSize; j++)
                {
                    Console.Write(grid[i, j] + " ");
                    chars[index++] = grid[i, j];
                }
                Console.WriteLine();
            }

            Console.WriteLine($"Words: {this.Words.ToString()}");
            Console.WriteLine($"Start Positions: {this.StartPosition.ToString()}");

            return new string(chars);
        }
        #endregion
    }
}
