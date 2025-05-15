/*
namespace ElsaServer
{
    using Elsa.Workflows;
    using Elsa.Workflows.Attributes;
    using Elsa.Workflows.Models;
    using Elsa.Extensions;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class ForEachBelowThresholdIdsActivity : CodeActivity
    {
        [Input] public Input<ICollection<string>> Ids { get; set; } = default!;
        [Input] public Input<string> ApiUrlTemplate { get; set; } = default!; // e.g. "https://localhost:7094/api/Stock/stock/{id}"

        protected override void Execute(ActivityExecutionContext context)
        {
            var ids = Ids.Get(context) ?? Array.Empty<string>();
            var urlTemplate = ApiUrlTemplate.Get(context) ?? "";

            foreach (var id in ids)
            {
                var url = urlTemplate.Replace("{id}", id);

                // Example: Do a PUT request for each ID (synchronously for demo; use async for real use)
                using var httpClient = new HttpClient();
                var content = new StringContent($"{{ \"id\": \"{id}\", \"quantity\": 0 }}", System.Text.Encoding.UTF8, "application/json");
                var response = httpClient.PutAsync(url, content).Result;

                // Optionally, handle response or log
                // var result = response.Content.ReadAsStringAsync().Result;
            }
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
    using System.Net.Http;
    using System.Text;
    using System.Collections;

    public class ForEachBelowThresholdIdsActivity : CodeActivity
    {
        [Input] public Input<IEnumerable> Ids { get; set; } = default!; // Accepts any enumerable (array, list, etc.)
        [Input] public Input<string> ApiUrlTemplate { get; set; } = default!; // e.g. "https://localhost:7094/api/Stock/stock/{id}"

        protected override void Execute(ActivityExecutionContext context)
        {
            var idsEnumerable = Ids.Get(context) ?? Array.Empty<object>();
            var urlTemplate = ApiUrlTemplate.Get(context) ?? "";

            using var httpClient = new HttpClient();

            foreach (var idObj in idsEnumerable)
            {
                // Convert each id to int (handles string or int input)
                if (idObj == null)
                    continue;

                int id;
                if (idObj is int intId)
                    id = intId;
                else if (int.TryParse(idObj.ToString(), out var parsedId))
                    id = parsedId;
                else
                    continue; // Skip if cannot parse

                var url = urlTemplate.Replace("{id}", id.ToString());

                // Add the required "command" field
                var jsonBody = $"{{ \"id\": {id}, \"quantity\": 0, \"command\": \"update\" }}";
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = httpClient.PutAsync(url, content).Result;

                // Optionally, log or handle response if needed
            }
        }
    }
}
