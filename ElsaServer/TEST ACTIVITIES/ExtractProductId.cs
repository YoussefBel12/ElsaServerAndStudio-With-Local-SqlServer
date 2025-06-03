using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;

namespace ElsaServer.TEST_ACTIVITIES
{

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty; // Stock Keeping Unit
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;

       
    }



    public class ExtractProductId : CodeActivity
    {
        [Input]
        public Input<Product> Product { get; set; } = default!;

        [Output]
        public Output<int> ProductId { get; set; } = default!;

        protected override void Execute(ActivityExecutionContext context)
        {
            var product = Product.Get(context);
            var productId = product?.Id ?? 0;

            ProductId.Set(context, productId);
            context.SetVariable("ProductId", productId);
        }
    }
}
