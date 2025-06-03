using System.Collections.Generic;
using System.Linq;
using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using ElsaServer.TEST_ACTIVITIES;


namespace ElsaServer.TEST_ACTIVITIES
{

    public class Stock
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

     
    }




    public class FindStockByProductId : CodeActivity
    {
        [Input]
        public Input<List<Stock>> Stocks { get; set; } = default!;

        [Input]
        public Input<int> ProductId { get; set; } = default!;

        [Output]
        public Output<Stock?> Stock { get; set; } = default!;

        protected override void Execute(ActivityExecutionContext context)
        {
            var stocks = Stocks.Get(context);
            var productId = ProductId.Get(context);

            var stock = stocks?.FirstOrDefault(s => s.ProductId == productId);

            Stock.Set(context, stock);
            // Optionally, set as workflow variable
            context.SetVariable("Stock", stock);
        }
    }
}