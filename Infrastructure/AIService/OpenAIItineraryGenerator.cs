using Application.Common.Interfaces;
using Application.Itinerary;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Infrastructure.AIService;

public class OpenAIItineraryGenerator : IAIItineraryGeneratorService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public OpenAIItineraryGenerator(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<ItineraryResponse> GenerateItineraryAsync(ItineraryRequest request, CancellationToken cancellationToken)
    {
        //This should be in a file but i will leave it here for simplicity
        var jsonOutputSchema = @"
        {
          ""$schema"": ""http://json-schema.org/draft-07/schema#"",
          ""title"": ""Itinerary"",
          ""type"": ""object"",
          ""properties"": {
            ""destination"": {
              ""type"": ""string""
            },
            ""startDate"": {
              ""type"": ""string"",
              ""format"": ""date""
            },
            ""endDate"": {
              ""type"": ""string"",
              ""format"": ""date""
            },
            ""totalEstimatedCost"": {
              ""type"": ""number""
            },
            ""dayPlans"": {
              ""type"": ""array"",
              ""items"": {
                ""type"": ""object"",
                ""properties"": {
                  ""date"": {
                    ""type"": ""string"",
                    ""format"": ""date""
                  },
                  ""activities"": {
                    ""type"": ""array"",
                    ""items"": {
                      ""type"": ""object"",
                      ""properties"": {
                        ""time"": {
                          ""type"": ""string""
                        },
                        ""description"": {
                          ""type"": ""string""
                        },
                        ""category"": {
                          ""type"": ""string""
                        },
                        ""estimatedCost"": {
                          ""type"": ""number""
                        }
                      },
                      ""required"": [""time"", ""description"", ""category"", ""estimatedCost""]
                    }
                  },
                  ""hotel"": {
                    ""type"": ""object"",
                    ""properties"": {
                      ""name"": {
                        ""type"": ""string""
                      },
                      ""costPerNight"": {
                        ""type"": ""number""
                      }
                    },
                    ""required"": [""name"", ""costPerNight""]
                  },
                  ""restaurants"": {
                    ""type"": ""array"",
                    ""items"": {
                      ""type"": ""string""
                    }
                  }
                },
                ""required"": [""date"", ""activities"", ""hotel"", ""restaurants""]
              }
            }
          },
          ""required"": [""destination"", ""startDate"", ""endDate"", ""totalEstimatedCost"", ""days""]
        }";


        string systemPrompt = $"You are a travel planner AI. Provide a JSON itinerary with days, activities, hotels, restaurants, and costs. Use this example json template {jsonOutputSchema}";
        string userPrompt = $"Plan a trip to {request.Destination} from {request.TravelDates[0]:yyyy-MM-dd} " +
                            $"to {request.TravelDates[^1]:yyyy-MM-dd} for a budget of ${request.Budget}. ";

        if (request.Interests != null && request.Interests.Any())
        {
            userPrompt += $"Interests: {string.Join(", ", request.Interests)}.";
        }

        var requestBody = new
        {
            model = "gpt-4o-mini",
            messages = new object[]
            {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
            },
            temperature = 0.7
        };

        //Configure the HTTP client for OpenAI API
        string apiKey = _configuration["OpenAI:ApiKey"];
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new Exception($"OpenAI API error: {error}");
        }

        string jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);

        JObject jsonRes = JObject.Parse(jsonResponse);
        string strContent = (string)jsonRes["choices"]?[0]?["message"]?["content"];
        string cleanedJson = Regex.Replace(strContent, @"^```(?:json)?\s*|```$", "").Trim();
        return JsonConvert.DeserializeObject<ItineraryResponse>(cleanedJson);
    }
}
