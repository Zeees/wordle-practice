using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.Models.Database
{
    public class DbWordleGameInfo
    {
        public Guid GameId { get; set; }
        public string Word { get; set; } = string.Empty;
        public bool IsDone { get; set; }
        public int Attempts { get; set; }
        public int MaxAttempts { get; set; }
        public string GuessesJson { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
