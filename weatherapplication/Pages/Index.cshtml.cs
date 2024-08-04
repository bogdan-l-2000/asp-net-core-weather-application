using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace weatherapplication.Pages
{
    public class IndexModel : PageModel
    {
        public List<string> displayLocations { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            displayLocations = new List<string>{ "Toronto", "Tokyo", "Melbourne", "Cairo", "Amsterdam", "Montevideo", "McMurdo Station" };
        }

        public void OnGet()
        {
        }

        public async void OnPost()
        {
            Console.WriteLine("Made Call!");
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