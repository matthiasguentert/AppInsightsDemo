using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace AppInsightsDemo
{
    public class CloudRoleNameTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleName = "Frontend-Server-Side";
        }
    }
}
