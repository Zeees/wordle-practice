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
    ///     Request model used for performing a guess.
    /// </summary>
    [SwaggerSchemaFilter(typeof(GuessRequestSchemaFilter))]
    public class GuessRequest
    {
        
        /// <summary>
        ///     The user's game session ID.
        /// </summary>
        [JsonPropertyName("game_id")]
        [Required(ErrorMessage = "A game session ID is required.")]
        public Guid GameID { get; set; }
        /// <summary>
        ///     The user's guess as a string.
        /// </summary>
        [JsonPropertyName("guess")]
        [Required]
        [MinLength(2, ErrorMessage = "The guess needs to contain at least two characters.")]
        public string Guess { get; set; } = string.Empty;
    }

    public class GuessRequestSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Example = new OpenApiObject
            {
                ["game_id"] = new OpenApiString("1e80a24f-1399-43ea-ba0f-306f0a0dc0c6"),
                ["guess"] = new OpenApiString("Wordle")
            };
        }
    }
}
