using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wordle.Models.Domain
{
    /// <summary>
    ///     Response received after calling the guess endpoint.
    /// </summary>
    public class GuessResponse
    {
        /// <summary>
        ///     ID of the active game.
        /// </summary>
        [JsonPropertyName("game_id")]
        public Guid GameID { get; set; }
        /// <summary>
        ///     The guessed word as a string.
        /// </summary>
        [JsonPropertyName("guess")]
        public string Guess { get; set; } = string.Empty;
        /// <summary>
        ///     Each letter in the guessed word and what status they have (wrong letter, wrong position, correct)
        /// </summary>
        [JsonPropertyName("letters")]
        public ICollection<WordleLetter> Letters { get; set; } = new List<WordleLetter>();
        /// <summary>
        ///     Was the guess correct?
        /// </summary>
        [JsonPropertyName("is_correct")]
        public bool IsCorrect { get; set; }
        /// <summary>
        ///     Was the guess a valid word?
        /// </summary>
        [JsonPropertyName("is_valid")]
        public bool IsValid { get; set; }
    }
}
