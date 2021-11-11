using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppInsightsDemo.Pages
{
    public class TrackModel : PageModel
    {
        private readonly IConfiguration _config;
        private ILogger<TrackModel> _logger;
        private static readonly HttpClient _client = new HttpClient();

        public TrackModel(IConfiguration config, ILogger<TrackModel> logger)
        {
            _config = config;
            _logger = logger;
        } 

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetTraceAsync()
        {
            var url = $"{_config["ServiceApi"]}/api/events/trace";
            _logger.LogInformation($"OnPostTraceAsync() => {url}");

            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return Content($"Timestamp: {DateTime.UtcNow} StatusCode: {(int)response.StatusCode}");
        }

        public async Task<IActionResult> OnGetExceptionAsync()
        {
            var url = $"{_config["ServiceApi"]}/api/events/exception";
            _logger.LogInformation($"OnPostExceptionAsync() => {url}");

            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return Content($"Timestamp: {DateTime.UtcNow} StatusCode: {(int)response.StatusCode}");
        }

        public async Task<IActionResult> OnGetEventAsync()
        {
            var url = $"{_config["ServiceApi"]}/api/events/event";
            _logger.LogInformation($"OnPostEventAsync() => {url}");

            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return Content($"Timestamp: {DateTime.UtcNow} StatusCode: {(int)response.StatusCode}");
        }
    }
}
