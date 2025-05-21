namespace ElsaServer
{
    using Elsa.Expressions;
    using Elsa.Extensions;
    using Elsa.Workflows;
    using Elsa.Workflows.Activities;
    using Elsa.Workflows.Attributes;
    using Elsa.Workflows.Models;

    public class ExtractFirstStockActivity : CodeActivity
    {
        [Input] public Input<ICollection<object>> stock { get; set; } = default!;
        [Output] public Output<object?> firstStock { get; set; } = default!;

        protected override void Execute(ActivityExecutionContext context)
        {
            var stockArray = stock.Get(context);

            var first = stockArray?.FirstOrDefault();
            context.Set(firstStock, first);


            //i added this
            context.SetVariable("firstStock", first);




        }
    }

}

