using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppInsightsDemo.Pages
{
    public class ExceptionsModel : PageModel
    {
        private readonly ILogger<ExceptionsModel> _logger;
        private static readonly HttpClient _client = new HttpClient();
        private readonly IConfiguration _config;
        private readonly TelemetryClient _telemetryClient;

        public ExceptionsModel(ILogger<ExceptionsModel> logger, IConfiguration config, TelemetryClient telemetryClient)
        {
            _logger = logger;
            _config = config;
            _telemetryClient = telemetryClient;
        }

        public async Task OnGetThrowOnServiceApi()
        {
            var url = $"{_config["ServiceApi"]}/api/events/throw";
            await _client.GetAsync(url);
        }

        public void OnGetThrowOnBackend()
        {
            _logger.LogInformation("OnGetThrowOnBackend()");

            // Reporting exceptions explicitly
            try
            {
                throw new Exception("Something went wrong on the Backend");
            }
            catch (Exception e)
            {
                var properties = new Dictionary<string, string>
                {
                    { "CustomKey", "CustomValue" },
                    { "Message", "This exception was explicitly reported!" }
                };

                _telemetryClient.TrackException(e, properties);
            }
        }
    }
}
