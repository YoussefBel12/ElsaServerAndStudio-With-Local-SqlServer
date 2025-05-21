/*

namespace ElsaServer
{
    using Elsa.Extensions;
    using Elsa.Workflows;
    using Elsa.Workflows.Activities;
    using Elsa.Workflows.Attributes;
    using Elsa.Workflows.Models;
    using System.Text.Json;

    public class ExtractNotification : CodeActivity
    {
        [Input] public Input<object> Notification { get; set; } = default!;
        [Output] public Output<bool?> UserConfirmed { get; set; } = default!;

        protected override void Execute(ActivityExecutionContext context)
        {
            var notification = Notification.Get(context);

            // Attempt to parse userConfirmed from JSON object
            if (notification is JsonElement element && element.TryGetProperty("UserConfirmed", out var confirmedElement))
            {
                var value = confirmedElement.GetBoolean();
                context.Set(UserConfirmed, value);
                context.SetVariable("UserConfirmed", value);
            }
            else
            {
                context.Set(UserConfirmed, null);
                context.SetVariable("UserConfirmed", null);
            }
        }
    }
}

*/

/*
namespace ElsaServer
{
    using System.Text.Json;
    using Elsa.Expressions;
    using Elsa.Extensions;
    using Elsa.Workflows;
    using Elsa.Workflows.Activities;
    using Elsa.Workflows.Attributes;
    using Elsa.Workflows.Models;

    public class ExtractNotification : CodeActivity
    {
        [Input] public Input<object> Notification { get; set; } = default!;
        [Output] public Output<bool?> UserConfirmed { get; set; } = default!;

        protected override void Execute(ActivityExecutionContext context)
        {
            var notification = Notification.Get(context);

            var userConfirmed = 
           

            //i added this
            context.SetVariable("UserConfirmed", userConfirmed);




        }
    }

}
*/




namespace ElsaServer
{
    using System.Text.Json;
    using Elsa.Extensions;
    using Elsa.Workflows;
    using Elsa.Workflows.Attributes;
    using Elsa.Workflows.Models;

    public class ExtractNotification : CodeActivity
    {
        [Input] public Input<object> Notification { get; set; } = default!;
        [Output] public Output<bool?> UserConfirmed { get; set; } = default!;

        protected override void Execute(ActivityExecutionContext context)
        {
            var notification = Notification.Get(context);
            bool? userConfirmed = null;

            // Safely handle JSON input (since it's application/json)
            if (notification is JsonElement jsonElement)
            {
                // Check if "userConfirmed" exists (case-sensitive)
                if (jsonElement.TryGetProperty("userConfirmed", out var userConfirmedProp) &&
                    (userConfirmedProp.ValueKind == JsonValueKind.True ||
                     userConfirmedProp.ValueKind == JsonValueKind.False))
                {
                    userConfirmed = userConfirmedProp.GetBoolean();
                }
            }

            // Set output & workflow variable
            context.Set(UserConfirmed, userConfirmed);
            context.SetVariable("UserConfirmed", userConfirmed);
        }
    }
}




