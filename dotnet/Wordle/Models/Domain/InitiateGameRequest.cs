using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wordle.Models.Domain
{
    /// <summary>
    ///     Request for initilizing a game of Wordle.
    /// </summary>
    [SwaggerSchemaFilter(typeof(InitiateGameSchemaFilter))]
    public class InitiateGameRequest
    {
        /// <summary>
        ///     Length of the word used in the game.
        /// </summary>
        [JsonPropertyName("word_length")]
        [Required(ErrorMessage = "'word_length' required to initilize a game.")]
        [Range(2, 8, ErrorMessage = "The game only supports words between 2 - 8 letters.")]
        public int WordLength { get; set; }
        /// <summary>
        ///     Number of attempts the player has to figure out the word.
        /// </summary>
        [JsonPropertyName("number_of_attempts")]
        [Required(ErrorMessage = "'attempts' required to initilize the game.")]
        [Range(1, 15, ErrorMessage = "Them game only supports up to 15 attempts.")]
        public int NumberOfAttempts { get; set; }
    }

    public class InitiateGameSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Example = new OpenApiObject
            {
                ["word_length"] = new OpenApiInteger(5),
                ["number_of_attempts"] = new OpenApiInteger(5)
            };
        }
    }
}
