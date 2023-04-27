using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.Models.Domain
{
    public class WordleGameInfo
    {
        public Guid GameId { get; set; }
        public string Word { get; set; } = string.Empty;
        public bool IsDone { get; set; }
        public int Attempts { get; set; }
        public int MaxAttempts { get; set; }
        public ICollection<WordleGuess> Guesses { get; set; } = new List<WordleGuess>();
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }

    public class WordleGuess
    {
        public string Guess = string.Empty;
        public ICollection<WordleLetter> Letters { get; set; } = new List<WordleLetter>();
    }
}
