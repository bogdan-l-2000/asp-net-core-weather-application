using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace weatherapplication.Pages
{
    public class MunicipalityModel : PageModel
    {
        public string Location  { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public MunicipalityModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async void OnPost()
        {

        }

        public async Task<JsonResult> GetWeather() {
            using ( var httpClient = new HttpClient()) {
                using (HttpResponseMessage response = await httpClient.GetAsync($"https://api.open-meteo.com/v1/forecast?latitude={Latitude}&longitude={Longitude}")) {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(apiResponse);
                }
            }

            return new JsonResult("");
        }
    }
}