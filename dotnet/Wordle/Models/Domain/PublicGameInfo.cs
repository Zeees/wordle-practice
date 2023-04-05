using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wordle.Models.Domain
{
    public class PublicGameInfo
    {
        /// <summary>
        ///     Game session id for the active game.
        /// </summary>
        [JsonPropertyName("game_id")]
        public Guid GameId { get; set; }
        /// <summary>
        ///     Length of the word to guess.
        /// </summary>
        [JsonPropertyName("word_length")]
        public int WordLength { get; set; }
        /// <summary>
        ///     Maximum number of attempts to guess the word.
        /// </summary>
        [JsonPropertyName("max_attempts")]
        public int MaxAttempts { get; set; }
        /// <summary>
        ///     Number of attempts made so far.
        /// </summary>
        [JsonPropertyName("attempts")]
        public int Attempts { get; set; }
        /// <summary>
        ///     All guesses and results so far.
        /// </summary>
        [JsonPropertyName("guesses")]
        public ICollection<WordleGuess>? Guesses { get; set; }
    }
}
