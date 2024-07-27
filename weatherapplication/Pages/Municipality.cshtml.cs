using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

// Uses OpenWeatherMap API: https://openweathermap.org/current

namespace weatherapplication.Pages
{
    public class MunicipalityModel : PageModel
    {
        public string City { get; set; }

        public string Country { get; set; }

        public float CurrentTemperature { get; set; }

        public string TemperatureUnit { get; set; }


        public float CurrentWindSpeed { get; set; }

        public float CurrentPrecipitation { get; set; }

        [BindProperty]
        public MunicipalityCurrentDataResponse? CurrentWeatherResponseData { get; set; }

        private string WeatherAPIKey;

        private readonly ILogger<IndexModel> _logger;

        public MunicipalityModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            //using StreamReader jsonReader = new("../appSecrets.json");
            //var jsonData = jsonReader.ReadToEnd();
            string jsonData = System.IO.File.ReadAllText("appSecrets.json");
            ApiInfo WeatherAPIInfo = JsonSerializer.Deserialize<ApiInfo>(jsonData);
            Console.WriteLine(WeatherAPIInfo.ConnectionStrings.OpenWeatherAPI);
            WeatherAPIKey = WeatherAPIInfo.ConnectionStrings.OpenWeatherAPI;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            City = Request.Form["City"];
            Country = Request.Form["Country"];
            Console.WriteLine(City);
            Console.WriteLine(Country);

            var response = await GetWeatherInfo();
            CurrentWeatherResponseData = JsonSerializer.Deserialize<MunicipalityCurrentDataResponse>(response);


            return Page();
        }

        public async Task<string> GetWeatherInfo() {
            string apiResponse = "";
            string apiRequest = $"https://api.openweathermap.org/data/2.5/weather?q={City},{Country}&appid={WeatherAPIKey}";
            using ( var httpClient = new HttpClient()) {
                using (HttpResponseMessage response = await httpClient.GetAsync(apiRequest)) {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(apiResponse);
                }
            }

            return apiResponse;
        }
    }

    public class ApiInfo
    {
        [JsonPropertyName("ConnectionStrings")]
        public KeyInfo ConnectionStrings { get; set; }
    }

    public class KeyInfo
    {
        [JsonPropertyName("OpenWeatherAPI")]
        public string OpenWeatherAPI { get; set; }
    }

    public class MunicipalityCurrentDataResponse
    {
        [JsonPropertyName("coord")]
        public MunicipalityCoordinateData coord { get; set; }
        
        [JsonPropertyName("weather")]
        public List<WeatherDescription> weather { get; set; }

        [JsonPropertyName("base")]
        public string base_str { get; set; }

        [JsonPropertyName("main")]
        public MainWeatherInfo main { get; set; }

        [JsonPropertyName("visibility")]
        public int visibility { get; set; }

        [JsonPropertyName("wind")]
        public MunicipalityWindData wind { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }
    }

    public class MunicipalityCoordinateData
    {
        [JsonPropertyName("lon")]
        public float lon { get; set; }

        [JsonPropertyName("lat")]
        public float lat { get; set; }
    }

    public class WeatherDescription
    {
        [JsonPropertyName("id")]
        public int id { get; set; }
        
        [JsonPropertyName("main")]
        public string main { get; set; }
        
        [JsonPropertyName("description")]
        public string description { get; set; }
        
        [JsonPropertyName("icon")]
        public string icon { get; set; }

    }

    public class MainWeatherInfo
    {
        public float temp { get; set; }
        public float feels_like { get; set; }
        public float temp_min { get; set; }
        public float temp_max { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public int sea_level { get; set; }
        public int grnd_level { get; set; }

    }

    public class MunicipalityWindData
    {
        public float speed { get; set; }
        public int deg { get; set; }
        public float gust { get; set; }

    }

}