using Elsa.Extensions;
using Elsa.Workflows;


namespace ElsaServer
{

    public class PrintMessage : CodeActivity
    {
        public string stock { get; set; }

        //this should access vars

    //  public string firstStock { get; set; }

        protected override void Execute(ActivityExecutionContext context)
        {
            Console.WriteLine(stock);
        
        }
    }
}

