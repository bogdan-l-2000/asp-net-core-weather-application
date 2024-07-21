using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace weatherapplication.Pages
{
    public class CoordinatesModel : PageModel
    {
        public string Location  { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public string NumDays { get; set; }

        public string TemperatureUnit { get; set; }

        public string PrecipitationUnit { get; set; }

        public string WindSpeedUnit { get; set; }

        public List<float> TemperatureList { get; set; }

        [BindProperty]
        public CoordinateDataResponse? ResponseData { get; set; }

        [BindProperty]
        public List<string> parsedDateTimeStrings { get; set; }

        [BindProperty]
        public string parsedCurrentDateTime { get; set; }

        // public List<DateTime> retrievedDateTimes { get; set; }

        public float CurrentTemperature { get; set; }

        public float CurrentWindSpeed { get; set; }

        public float CurrentPrecipitation { get; set; }

        [BindProperty]
        public int currentTimeIndex { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public CoordinatesModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            Location = Request.Form["location"];
            
            Latitude = Request.Form["latitude"];
            Longitude = Request.Form["longitude"];
            NumDays = Request.Form["NumDays"];
            TemperatureUnit = Request.Form["TemperatureUnit"];
            PrecipitationUnit = Request.Form["PrecipitationUnit"];
            WindSpeedUnit = Request.Form["WindSpeedUnit"];
            Console.WriteLine(Latitude);
            Console.WriteLine(Longitude);
            Console.WriteLine(NumDays);
            Console.WriteLine("updated!");
                
            var response = await GetWeatherForecast();
            Console.WriteLine(response);
            Console.WriteLine("after response");


            ResponseData = JsonSerializer.Deserialize<CoordinateDataResponse>(response);

            Console.WriteLine(ResponseData.elevation);
            Console.WriteLine(ResponseData.hourly);
            Console.WriteLine(ResponseData.hourly.temperature_2m);
            Console.WriteLine(ResponseData.hourly.time.Count);

            int utcOffset = ResponseData.utc_offset_seconds;
            DateTime utcTime = DateTime.UtcNow;
            DateTime localTime = utcTime.AddHours(utcOffset / 3600);
            Console.WriteLine(localTime);

            parsedCurrentDateTime = localTime.ToString("dddd MMMM dd, yyyy HH:mm");
            var differenceBetweenLocalTime = 24.0;
            currentTimeIndex = 0;

            for (int i = 0; i < ResponseData.hourly.time.Count; i++) {
                DateTime dateValue = DateTime.Parse(ResponseData.hourly.time[i]);
                parsedDateTimeStrings.Add(dateValue.ToString("dddd MMMM dd, yyyy HH:mm"));
                var hoursDifference = (dateValue - localTime).TotalHours;

                if (Math.Abs(hoursDifference) < differenceBetweenLocalTime) {
                    differenceBetweenLocalTime = Math.Abs(hoursDifference);
                    currentTimeIndex = i;
                }
            }


            return Page();
        }

        public async Task<string> GetWeatherForecast() {
            string apiResponse = "";
            // Console.WriteLine($"https://api.open-meteo.com/v1/forecast?latitude={Latitude}&longitude={Longitude}&hourly=temperature_2m");
            string apiRequest = $"https://api.open-meteo.com/v1/forecast?latitude={Latitude}&longitude={Longitude}&hourly=temperature_2m,precipitation,wind_speed_10m,cloud_cover&timezone=auto&forecast_days={NumDays}&temperature_unit={TemperatureUnit}&precipitation_unit={PrecipitationUnit}&wind_speed_unit={WindSpeedUnit}";
            using ( var httpClient = new HttpClient()) {
                using (HttpResponseMessage response = await httpClient.GetAsync(apiRequest)) {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(apiResponse);
                }
            }

            return apiResponse;
        }
    }

    public class CoordinateDataResponse
    {
        [JsonPropertyName("latitude")]
        public float latitude { get; set; }
        
        [JsonPropertyName("longitude")]
        public float longitude { get; set; }
        
        [JsonPropertyName("generationtime_ms")]
        public float generationtime_ms  { get; set; }

        public int utc_offset_seconds { get; set; }

        public string timezone { get; set; }
        
        public string timezone_abbreviation { get; set; }
        
        [JsonPropertyName("elevation")]
        public float elevation { get; set; }

        [JsonPropertyName("hourly_units")]        
        public CoordinateResponseHourlyUnitsData hourly_units { get; set; }
        
        [JsonPropertyName("hourly")]        
        public CoordinateResponseHourlyData hourly { get; set; }
    }

    public class CoordinateResponseHourlyUnitsData
    {
        [JsonPropertyName("time")]
        public string time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public string temperature_2m { get; set; }

        [JsonPropertyName("precipitation")]
        public string precipitation { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public string wind_speed_10m { get; set; }

        [JsonPropertyName("cloud_cover")]
        public string cloud_cover { get; set; }
    }

    public class CoordinateResponseHourlyData
    {
        [JsonPropertyName("time")]
        public List<string> time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public List<float> temperature_2m { get; set; }
    
        [JsonPropertyName("precipitation")]
        public List<float> precipitation { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public List<float> wind_speed_10m { get; set; }

        [JsonPropertyName("cloud_cover")]
        public List<int> cloud_cover { get; set; }
    }
}

// API used here: https://open-meteo.com

// Weather icons (tentatively): https://erikflowers.github.io/weather-icons/

// Idea: use a separate API call to do "forecast hours = 1" for this 
// Example: https://api.open-meteo.com/v1/forecast?latitude=52.52&longitude=13.41&hourly=temperature_2m&forecast_hours=1