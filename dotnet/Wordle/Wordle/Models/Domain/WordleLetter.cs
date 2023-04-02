using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wordle.Models.Domain
{
    public class WordleLetter
    {
        [JsonPropertyName("letter")]
        public char Letter { get; set; }
        [JsonPropertyName("status")]
        public LetterStatus Status { get; set; }
    }

    /// <summary>
    ///     Status of the letter. Is it in the wrong position, the right position or is it not in the word at all?
    /// </summary>
    public enum LetterStatus
    {
        WrongLetter,
        WrongPosition,
        Correct
    }
}
