/*

namespace ElsaServer
{
    using Elsa.Workflows;
    using Elsa.Workflows.Attributes;
    using Elsa.Workflows.Models;
    using Elsa.Extensions;
    using System.Text.Json;

    public class FilterStocksBelowThresholdActivity : CodeActivity
    {
        [Input] public Input<ICollection<object>> stock { get; set; } = default!;
        [Input] public Input<int> Threshold { get; set; } = new(0);
        [Output] public Output<ICollection<object>> belowThreshold { get; set; } = default!;
         [Output] public Output<ICollection<string>> belowThresholdIds { get; set; } = default!;
        //  [Output] public Output<ICollection<object>> belowThresholdIds { get; set; } = default!;
       // [Output] public Output<ICollection<int>> belowThresholdIds { get; set; } = default!;
        protected override void Execute(ActivityExecutionContext context)
        {
            var stockArray = stock.Get(context) ?? Array.Empty<object>();
            var thresholdValue = Threshold.Get(context);

            var filtered = new List<object>();
            var ids = new List<string>();
         


            foreach (var item in stockArray)
            {
                if (item is JsonElement json)
                {
                    if (json.TryGetProperty("quantity", out var quantityElement) && quantityElement.ValueKind == JsonValueKind.Number)
                    {
                        var quantity = quantityElement.GetInt32();
                        if (quantity < thresholdValue)
                        {
                            filtered.Add(item);
                            // Try to get the id as string
                            if (json.TryGetProperty("id", out var idElement))
                            {
                                if (idElement.ValueKind == JsonValueKind.String)
                                    ids.Add(idElement.GetString()!);
                                else if (idElement.ValueKind == JsonValueKind.Number)
                                    ids.Add(idElement.GetInt32().ToString());
                            }
                        }
                    }
                }
                else if (item is IDictionary<string, object> dict)
                {
                    if (dict.TryGetValue("quantity", out var quantityObj) && quantityObj is int quantityInt)
                    {
                        if (quantityInt < thresholdValue)
                        {
                            filtered.Add(item);
                            if (dict.TryGetValue("id", out var idObj) && idObj != null)
                                ids.Add(idObj.ToString()!);
                        }
                    }
                }
            }

            context.Set(belowThreshold, filtered);
            context.Set(belowThresholdIds, ids);


            //i added this
            context.SetVariable("belowThreshold", filtered);
            context.SetVariable("belowThresholdIds", ids);
        }
    }
}

*/


namespace ElsaServer
{
    using Elsa.Workflows;
    using Elsa.Workflows.Attributes;
    using Elsa.Workflows.Models;
    using Elsa.Extensions;
    using System.Text.Json;

    public class FilterStocksBelowThresholdActivity : CodeActivity
    {
        [Input] public Input<ICollection<object>> stock { get; set; } = default!;
        [Input] public Input<int> Threshold { get; set; } = new(0);
        [Output] public Output<ICollection<object>> belowThreshold { get; set; } = default!;
        [Output] public Output<ICollection<int>> belowThresholdIds { get; set; } = default!; // Changed to int

        protected override void Execute(ActivityExecutionContext context)
        {
            var stockArray = stock.Get(context) ?? Array.Empty<object>();
            var thresholdValue = Threshold.Get(context);

            var filtered = new List<object>();
            var ids = new List<int>(); // Changed to int

            foreach (var item in stockArray)
            {
                if (item is JsonElement json)
                {
                    if (json.TryGetProperty("quantity", out var quantityElement) && quantityElement.ValueKind == JsonValueKind.Number)
                    {
                        var quantity = quantityElement.GetInt32();
                        if (quantity < thresholdValue)
                        {
                            filtered.Add(item);
                            // Try to get the id as int
                            if (json.TryGetProperty("id", out var idElement))
                            {
                                if (idElement.ValueKind == JsonValueKind.String && int.TryParse(idElement.GetString(), out var idAsInt))
                                    ids.Add(idAsInt);
                                else if (idElement.ValueKind == JsonValueKind.Number)
                                    ids.Add(idElement.GetInt32());
                            }
                        }
                    }
                }
                else if (item is IDictionary<string, object> dict)
                {
                    if (dict.TryGetValue("quantity", out var quantityObj) && quantityObj is int quantityInt)
                    {
                        if (quantityInt < thresholdValue)
                        {
                            filtered.Add(item);
                            if (dict.TryGetValue("id", out var idObj) && idObj != null && int.TryParse(idObj.ToString(), out var idAsInt))
                                ids.Add(idAsInt);
                        }
                    }
                }
            }

            context.Set(belowThreshold, filtered);
            context.Set(belowThresholdIds, ids);

            // Optionally set as variables
            context.SetVariable("belowThreshold", filtered);
            context.SetVariable("belowThresholdIds", ids);
        }
    }
}

