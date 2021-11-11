using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ServiceApi.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private TelemetryClient _telemetryClient;
        private ILogger<EventsController> _logger;

        public EventsController(TelemetryClient telemetryClient, ILogger<EventsController> logger)
        {
            _telemetryClient = telemetryClient;
            _logger = logger;
        }

        [HttpGet("trace")]
        public void TrackTrace()
        {
            _logger.LogInformation("TrackTrace()");

            var properties = new Dictionary<string, string>
            {
                { "CustomKey", "CustomValue" },
                { "Timestamp", DateTime.UtcNow.ToString() }
            };

            _telemetryClient.TrackTrace("MyTraceMessage", Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Verbose, properties);
        }

        [HttpGet("throw")]
        public void Throw()
        {
            _logger.LogInformation("Throw()");

            throw new Exception("Something went wrong on the Service API!");
        }

        [HttpGet("exception")]
        public void TrackException()
        {
            _logger.LogInformation("TrackException()");

            // Reporting exceptions explicitly
            try
            {
                throw new Exception("Something went wrong on the Service API!");
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

        [HttpGet("event")]
        public void TrackEvent()
        {
            _logger.LogInformation("TrackEvent()");

            var properties = new Dictionary<string, string>
            {
                { "CustomKey", "CustomValue" },
                { "Timestamp", DateTime.UtcNow.ToString() }
            };

            _telemetryClient.TrackEvent("MyCustomEvent", properties);
        }
    }
}
