using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AppInsightsDemo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            var message = "Message from App Insights Logger Extension";

            _logger.LogInformation(message);
            _logger.LogDebug(message);
            _logger.LogTrace(message);

            _logger.LogError(message);
            _logger.LogWarning(message);
            _logger.LogCritical(message);
        }
    }
}
