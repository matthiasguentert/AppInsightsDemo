using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AppInsightsDemo.Pages
{
    public class FunnelModel : PageModel
    {
        private readonly ILogger<FunnelModel> _logger;
        private readonly TelemetryClient _telemetryClient;

        public FunnelModel(ILogger<FunnelModel> logger, TelemetryClient telemetryClient)
        {
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        public void OnPostFunnelTest(string id)
        {
            _logger.LogInformation(id);
            _telemetryClient.TrackEvent(id);
        }
    }
}
