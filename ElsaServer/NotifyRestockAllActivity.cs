using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ElsaServer
{
    public class NotifyRestockAllActivity : CodeActivity
    {
        [Input] public Input<ICollection<int>> StockIds { get; set; } = default!;
        [Input] public Input<string> NotifyUrl { get; set; } = default!;
        //i added this line below
        [Input] public Input<bool> UserConfirmed { get; set; }  

        [Output] public Output<string?> ResponseContent { get; set; } = default!;
        [Output] public Output<int> StatusCode { get; set; } = default!;
       

        protected override void Execute(ActivityExecutionContext context)
        {
            var stockIds = StockIds.Get(context) ?? Array.Empty<int>();
            var notifyUrl = NotifyUrl.Get(context) ?? "";
            //added this 
            var userConfirmed = UserConfirmed.Get(context);

            var notification = new
            {
                stockIds = stockIds,
                message = "Stock is low for these items. Restock all?"
                //this is important i added false to check decistion
               , userConfirmed = userConfirmed


            };
            var jsonBody = JsonSerializer.Serialize(notification);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            var response = httpClient.PostAsync(notifyUrl, content).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            context.Set(ResponseContent, responseContent);
            context.Set(StatusCode, (int)response.StatusCode);
        }
    }
}
