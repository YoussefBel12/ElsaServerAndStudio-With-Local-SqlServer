using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace ElsaServer
{
    public class RestockNotificationDto  // Renamed to avoid confusion with Order Project's class
    {
        public int Id { get; set; }
        public List<int> StockIds { get; set; } = new();
        public string? Message { get; set; }
        public bool UserConfirmed { get; set; }
        public string WorkflowInstanceId { get; set; } = string.Empty;
    }

    public class CheckConfirmationActivity : CodeActivity<bool>
    {
        [Input] public Input<string> OrderProjectBaseUrl { get; set; } = default!;
        [Input] public Input<TimeSpan> Timeout { get; set; } = new(TimeSpan.FromSeconds(30));

        protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
        {
            var baseUrl = OrderProjectBaseUrl.Get(context).TrimEnd('/');
            var notificationId = context.GetVariable<int>("NotificationId");
            var timeout = Timeout.Get(context);

            using var httpClient = new HttpClient { Timeout = timeout };

            try
            {
                var response = await httpClient.GetAsync($"{baseUrl}/api/notifications/{notificationId}");

                if (!response.IsSuccessStatusCode)
                {
                    context.SetResult(false);
                    return;
                }

                // Option 1: Strongly-typed deserialization
                var notification = await response.Content.ReadFromJsonAsync<RestockNotificationDto>();
                context.SetResult(notification?.UserConfirmed ?? false);

                /* Option 2: Manual JSON handling (if property names might differ)
                var json = await response.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(json);
                context.SetResult(jsonDoc.RootElement.GetProperty("userConfirmed").GetBoolean());
                */
            }
            catch (Exception ex)
            {
      //          context.Logger.LogError(ex, "Failed to check confirmation status");
                context.SetResult(false);
            }
        }
    }
}