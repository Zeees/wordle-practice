using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedResources.Helpers
{
    public static class HttpClientHelper
    {

        /// <summary>
        ///     Send a http request.
        /// </summary>
        /// <typeparam name="In">Type of data to send with the request.</typeparam>
        /// <typeparam name="Out">Response type expected from the request.</typeparam>
        /// <param name="client">Http client created by using IHttpClientFactory</param>
        /// <param name="path">Relative path to the desired endpoint.</param>
        /// <param name="method">Http method.</param>
        /// <param name="payload">Payload to send with the http request.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        ///     Exception with the error code in case something unexpected happens.
        /// </exception>
        public async static Task<Out?> SendAsync<In, Out>(
            HttpClient client,
            string path,
            HttpMethod method,
            In payload)
        {

            //Create and send http request.
            var request = CreateRequest<In>(method, path, payload);
            var response = await client.SendAsync(request);   

            //If the call was successfull then try to parse the body of the response.
            if(response.IsSuccessStatusCode)
            {
                return await ParseHttpResponse<Out>(response);
            }

            //Get error response as a string.
            var message = await response.Content.ReadAsStringAsync();

            switch(response.StatusCode)
            {
                //If the response was a 404 not found, return default(null)
                case System.Net.HttpStatusCode.NotFound:
                    return default;
                default:
                    throw new Exception(message); 
            }
        }

        /// <summary>
        ///     Send a http request.
        /// </summary>
        /// <typeparam name="Out">Response type expected from the request.</typeparam>
        /// <param name="client">Http client created by using IHttpClientFactory</param>
        /// <param name="path">Relative path to the desired endpoint.</param>
        /// <param name="method">Http method.</param>
        public async static Task<Out?> SendAsync<Out>(
            HttpClient client,
            string path,
            HttpMethod method)
        {
            return await SendAsync<Object, Out>(client, path, method, new Object());
        }

        public async static Task<Out?> GetAsync<Out>(
            HttpClient client,
            string path)
        {
            return await SendAsync<Out>(client, path, HttpMethod.Get);
        }

        public static HttpRequestMessage CreateRequest<T>(HttpMethod method, string path, T payload)
        {
            var options = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };

            var json = JsonSerializer.Serialize(payload, options);

            return new HttpRequestMessage(method, path)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }

        private static async Task<T> ParseHttpResponse<T>(HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions();
            options.Converters.Add(new CultureSpecificQuotedDecimalConverter());

            var deserilizedResponse = JsonSerializer.Deserialize<T>(content, options);
            if (deserilizedResponse != null) { return deserilizedResponse; }
            throw new Exception("Failed to parse response");
        }

    }

    public class CultureSpecificQuotedDecimalConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();

                if (value == null)
                {
                    return 0;
                }
                else
                {
                    if (!double.TryParse(value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedValue))
                    {
                        Console.WriteLine($"Failed to parse value: {value}");
                    }

                    return double.Parse(value.Replace(",", "."), CultureInfo.InvariantCulture);
                }
            }
            else
            {
                return reader.GetDouble();
            }
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }

    }
}
