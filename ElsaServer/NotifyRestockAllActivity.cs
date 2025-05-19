/*
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
        //i added this line below u can remove the new(false thing
        [Input]  public Input<bool> UserConfirmed { get; set; } =  new(false);

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





            //i added this

           
            context.SetVariable("UserConfirmed", userConfirmed);


        }
    }
}

*/


/*


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
        // Inputs
        [Input] public Input<ICollection<int>> StockIds { get; set; } = default!;
        [Input] public Input<string> NotifyUrl { get; set; } = default!;
        [Input] public Input<bool> UserConfirmed { get; set; } = new(false);

        // Outputs
        [Output] public Output<string?> ResponseContent { get; set; } = default!;
        [Output] public Output<int> StatusCode { get; set; } = default!;
        [Output] public Output<bool> UpdatedUserConfirmed { get; set; } = default!;

        protected override void Execute(ActivityExecutionContext context)
        {
            // Get input values
            var stockIds = StockIds.Get(context) ?? Array.Empty<int>();
            var notifyUrl = NotifyUrl.Get(context) ?? "";
            var userConfirmed = UserConfirmed.Get(context);

            // Create notification payload
            var notification = new
            {
                stockIds,
                message = "Stock is low for these items. Restock all?",
                userConfirmed
            };

            // Send HTTP request
            var jsonBody = JsonSerializer.Serialize(notification);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            var response = httpClient.PostAsync(notifyUrl, content).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            // Set outputs
            context.Set(ResponseContent, responseContent);
            context.Set(StatusCode, (int)response.StatusCode);

            // If you need to modify the user confirmation status based on response:
            // var newConfirmationStatus = ParseResponse(responseContent);
            context.Set(UpdatedUserConfirmed, userConfirmed);


            // i added too much those are for the variable table
            context.SetVariable("UserConfirmed", userConfirmed);
            context.SetVariable("UpdatedUserConfirmed", userConfirmed);

        }
    }
}



*/


//check up deepseekk to complete this tuesday


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

        [Output] public Output<string?> ResponseContent { get; set; } = default!;
        [Output] public Output<int> StatusCode { get; set; } = default!;

        protected override void Execute(ActivityExecutionContext context)
        {
            var stockIds = StockIds.Get(context) ?? Array.Empty<int>();
            var notifyUrl = NotifyUrl.Get(context) ?? "";
            var workflowInstanceId = context.WorkflowExecutionContext.Id;

            var notification = new
            {
                stockIds,
                message = "Stock is low for these items. Restock all?",
                workflowInstanceId
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