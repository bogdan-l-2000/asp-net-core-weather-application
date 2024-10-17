using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace weatherapplication.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<string> displayLocations { get; set; }
        
        [BindProperty]
        public List<string> apiCityValues { get; set; }
        public List<string> apiCountryValues { get; set; }

        private string WeatherAPIKey;

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            string jsonData = System.IO.File.ReadAllText("appSecrets.json");
            ApiInfo WeatherAPIInfo = JsonSerializer.Deserialize<ApiInfo>(jsonData);
            WeatherAPIKey = WeatherAPIInfo.ConnectionStrings.OpenWeatherAPI;

            displayLocations = new List<string>{ "Toronto", "Tokyo", "Melbourne", "Cairo", "Amsterdam", "Montevideo", "McMurdo Station" };
            apiCityValues = new List<string>{ "toronto", "tokyo", "melbourne", "cairo", "amsterdam", "montevideo", "mcmurdo"};
            apiCountryValues = new List<string>{ "canada", "japan", "australia", "egypt", "netherlands", "uruguay", "antarctica" };
        }

        public void OnGet()
        {
        }

        public async void OnPost()
        {
            Console.WriteLine("Made Call!");
        }

        public async Task<IActionResult> OnGetDefaults()
        {
            // Source: https://www.thereformedprogrammer.net/asp-net-core-razor-pages-how-to-implement-ajax-requests/
            Console.WriteLine("Made Defaults Call!");

            string UnitType = "metric";
            string TemperatureUnit = "°C";
            string WindSpeedUnit = "m/s";

            string responses = "{";

            for (int i = 0; i < displayLocations.Count; i++) {
                string City = apiCityValues[i];
                string Country = apiCountryValues[i];
                string apiResponse = "";
                string apiRequest = $"https://api.openweathermap.org/data/2.5/weather?q={City},{Country}&units={UnitType}&appid={WeatherAPIKey}";
                using ( var httpClient = new HttpClient()) {
                    using (HttpResponseMessage response = await httpClient.GetAsync(apiRequest)) {
                        apiResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(apiResponse);
                    }
                }
                responses += '"' + apiCityValues[i] + '"' + ":";
                responses += apiResponse;
                if (i < displayLocations.Count - 1) {
                    responses += ",";
                }
            }
            responses += "}";
            return new JsonResult(responses);
        }
    }
}


// Source: https://www.jetbrains.com/guide/dotnet/tutorials/basics/razor-pages/
// Each razor page equates to an endpoint. Razor pages have an associated C# object called the page mode, which holds each page's behaviour.
// Each page works on the limited semantics of HTML, only supporting GET and POST methods.

// https://www.codecademy.com/learn/asp-net-i/modules/asp-net-page-models/cheatsheet
// https://www.learnrazorpages.com/razor-pages/forms

// Idea: have a database of meteorogical facts

// https://stackoverflow.com/questions/72559385/how-to-call-an-asp-net-api-in-an-asp-net-razor-web-application