namespace ElsaServer.TEST_ACTIVITIES
{
    using Elsa.Extensions;
    using Elsa.Workflows;
    using Elsa.Workflows.Attributes;
    using Elsa.Workflows.Models;

    public class PurchaseModel
    {
        public int PurchaseId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime Timestamp { get; set; }

        //testing quantity 
        public int Quantity { get; set; } = 1; // Defauly is 1 if not specified
    }

    public class ExtractPurchase : CodeActivity
    {
        [Input]
        public Input<PurchaseModel> Purchase { get; set; } = default!;

        [Output] public Output<int> PurchaseId { get; set; } = default!;
        [Output] public Output<string> UserId { get; set; } = default!;
        //another new output for quantity
        [Output] public Output<int> Quantity { get; set; } = default!;

        protected override void Execute(ActivityExecutionContext context)
        {
            var purchase = Purchase.Get(context);

            PurchaseId.Set(context, purchase.PurchaseId);
            UserId.Set(context, purchase.UserId);
            //added this and its setvariable too
            Quantity.Set(context, purchase.Quantity);


            context.SetVariable("Quantity", purchase.Quantity);
            context.SetVariable("PurchaseId", purchase.PurchaseId);
            context.SetVariable("UserId", purchase.UserId);

            // Set other variables as needed
            context.SetVariable("Name", purchase.Name);
            context.SetVariable("SKU", purchase.SKU);
            context.SetVariable("Description", purchase.Description);
            context.SetVariable("Price", purchase.Price);
            context.SetVariable("IsActive", purchase.IsActive);
            context.SetVariable("Timestamp", purchase.Timestamp);
        }
    }
}