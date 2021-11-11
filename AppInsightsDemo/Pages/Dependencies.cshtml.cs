using AppInsightsDemo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppInsightsDemo.Pages
{
    public class DependenciesModel : PageModel
    {
        private readonly ILogger<DependenciesModel> _logger;
        private readonly IConfiguration _config;
        private static readonly HttpClient _client = new HttpClient();

        public DependenciesModel(ILogger<DependenciesModel> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<IActionResult> OnGetRandomDrinkAsync()
        {
            var response = await _client.GetAsync("https://www.thecocktaildb.com/api/json/v1/1/random.php");
            var content = await response.Content.ReadAsStringAsync();
            var drink = JsonSerializer.Deserialize<Drinks>(content).Root[0];

            return Content($"Drink Name: {drink.StrDrink}");
        }

        public IActionResult OnGetCreateBlob()
        {
            var url = $"{_config["ServiceApi"]}/api/store/{Guid.NewGuid()}.txt";

            _logger.LogInformation($"OnPostCreateBlob() calling {url}");

            var response = _client.PutAsync(url, new StringContent(DateTime.UtcNow.ToString())).Result;
            var reply = $"StatusCode: {(int)response.StatusCode}";

            return Content(reply);
        }

        public async Task<IActionResult> OnGetForecastAsync()
        {
            var url = $"{_config["ServiceApi"]}/api/forecast";

            _logger.LogInformation($"OnPostGetWeatherForecast() calling {url}");

            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            
            var forecast = JsonSerializer.Deserialize<List<Forecast>>(content)[0];

            return Content($"Forecast Summary: {forecast.Summary}");
        }
    }
}
