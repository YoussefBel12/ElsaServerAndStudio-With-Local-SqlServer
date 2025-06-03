using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;

namespace ElsaServer.TEST_ACTIVITIES
{
    public class ExtractStockId : CodeActivity
    {
        [Input]
        public Input<Stock> Stock { get; set; } = default!;

        [Output]
        public Output<int> StockId { get; set; } = default!;

        //added this
        [Output]
        public Output<int> StockQuantity { get; set; } = default!;

        protected override void Execute(ActivityExecutionContext context)
        {
            var stock = Stock.Get(context);
            var stockId = stock?.Id ?? 0;
            //and added this
            var stockQuantity  = stock?.Quantity ?? 0;

            StockId.Set(context, stockId);
            context.SetVariable("StockId", stockId);
            //and i added this too

            StockQuantity.Set(context, stockQuantity);
            context.SetVariable("StockQuantity", stockQuantity);
        }
    }
}
