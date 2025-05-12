using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using Elsa.Workflows;
using Elsa.Extensions;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text.Json;

namespace ElsaServer
{
    public class CheckStockThresholdActivity : CodeActivity
    {
        [Input] public Input<object?> firstStock { get; set; } = default!;
        [Input] public Input<int> Threshold { get; set; } = new(30); // default threshold
        [Output] public Output<bool> IsLow { get; set; } = default!;


        /*
        protected override void Execute(ActivityExecutionContext context)
        {
            var stock = firstStock.Get(context);
            var threshold = Threshold.Get(context);

            if (stock is JsonElement json && json.TryGetProperty("quantity", out var quantityElement) &&
                quantityElement.ValueKind == JsonValueKind.Number)
            {
                var quantity = quantityElement.GetInt32();
                context.Set(IsLow, quantity <= threshold);
            }
            else
            {
                context.Set(IsLow, false);
            }
        }
        */


        //up works down im trying to store in context variable

        protected override void Execute(ActivityExecutionContext context)
        {
            var stock = firstStock.Get(context);
            var threshold = Threshold.Get(context);

            if (stock is JsonElement json && json.TryGetProperty("quantity", out var quantityElement) &&
                quantityElement.ValueKind == JsonValueKind.Number)
            {
                var quantity = quantityElement.GetInt32();
                var isLow = quantity <= threshold;

                // Set the output
                context.Set(IsLow, isLow);

                // Also set it as a workflow variable
                context.SetVariable("IsLow", isLow);
            }
            else
            {
                context.Set(IsLow, false);
                context.SetVariable("IsLow", false);
            }
        }







    }




}
