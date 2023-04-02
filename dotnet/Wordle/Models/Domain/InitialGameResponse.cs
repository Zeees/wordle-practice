using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wordle.Models.Domain
{
    /// <summary>
    ///     Response for initilizing a game of Wordle.
    /// </summary>
    public class InitialGameResponse
    {
        /// <summary>
        ///     Id for the initilized game.
        /// </summary>
        [JsonPropertyName("game_id")]
        public Guid GameID { get; set; }
        /// <summary>
        ///     Length of the word to guess.
        /// </summary>
        [JsonPropertyName("word_length")]
        public int WordLength { get; set; }
        /// <summary>
        ///     Number of attempts allowed. 
        /// </summary>
        [JsonPropertyName("number_of_attempts")]
        public int NumberOfAttempts { get; set; }
    }
}
