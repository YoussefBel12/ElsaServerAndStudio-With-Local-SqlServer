


/*
namespace ElsaServer
{
    using System.Runtime.CompilerServices;
    using global::Elsa.Expressions.Contracts;
    using global::Elsa.Expressions.Models;
    using global::Elsa.Workflows.Activities.Flowchart.Attributes;
    using global::Elsa.Workflows.Activities.Flowchart.Models;
    using global::Elsa.Workflows.Attributes;
    using global::Elsa.Workflows.Models;
    using global::Elsa.Workflows.UIHints;
    using global::Elsa.Workflows;

   
        [FlowNode("Default")]
        [Activity("Elsa", "Branching", "Evaluate a set of case conditions and schedule the activity for a matching case.", DisplayName = "Switch Tom (flow)")]
      
        public class SwitchFlowTest : Activity
        {
            public SwitchFlowTest([CallerFilePath] string? source = default, [CallerLineNumber] int? line = default) : base(source, line) { }

            [Input(UIHint = "flow-switch-editor")]
            public ICollection<FlowSwitchCase> Cases { get; set; } = new List<FlowSwitchCase>();

            [Input(Description = "The switch mode determines whether the first match should be scheduled, or all matches.", UIHint = InputUIHints.DropDown)]
            public Input<SwitchMode> Mode { get; set; } = new(SwitchMode.MatchFirst);

        
            protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
            {
                var matchingCases = (await FindMatchingCasesAsync(context.ExpressionExecutionContext)).ToList();
                var hasAnyMatches = matchingCases.Any();
                var mode = context.Get(Mode);
                var results = mode == SwitchMode.MatchFirst ? hasAnyMatches ? [matchingCases.First()] : Array.Empty<FlowSwitchCase>() : matchingCases.ToArray();
                var outcomes = hasAnyMatches ? results.Select(r => r.Label).ToArray() : ["Default"];

                await context.CompleteActivityAsync(new Outcomes(outcomes));
            }

            private async Task<IEnumerable<FlowSwitchCase>> FindMatchingCasesAsync(ExpressionExecutionContext context)
            {
                var matchingCases = new List<FlowSwitchCase>();
                var expressionEvaluator = context.GetRequiredService<IExpressionEvaluator>();

                foreach (var switchCase in Cases)
                {
                    var result = await expressionEvaluator.EvaluateAsync<bool?>(switchCase.Condition, context);

                    if (result == true)
                    {
                        matchingCases.Add(switchCase);
                    }
                }

                return matchingCases;
            }


        






    }
}


*/


namespace ElsaServer
{
    using System.Runtime.CompilerServices;
    using global::Elsa.Expressions.Contracts;
    using global::Elsa.Expressions.Models;
    using global::Elsa.Workflows.Activities.Flowchart.Attributes;
    using global::Elsa.Workflows.Activities.Flowchart.Models;
    using global::Elsa.Workflows.Attributes;
    using global::Elsa.Workflows.Models;
    using global::Elsa.Workflows.UIHints;
    using global::Elsa.Workflows;

    [FlowNode("Default")]
    [Activity("Elsa", "Branching", "Evaluate a set of case conditions and schedule the activity for a matching case.", DisplayName = "Switch Custom Made (flow)")]
    public class SwitchFlowTest : Activity
    {
        public SwitchFlowTest([CallerFilePath] string? source = default, [CallerLineNumber] int? line = default) : base(source, line) { }

        [Input(UIHint = "flow-switch-editor")]
        public ICollection<FlowSwitchCase> Cases { get; set; } = new List<FlowSwitchCase>();

        [Input(Description = "The switch mode determines whether the first match should be scheduled, or all matches.", UIHint = InputUIHints.DropDown)]
        public Input<SwitchMode> Mode { get; set; } = new(SwitchMode.MatchFirst);

        protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
        {
            var matchingCases = (await FindMatchingCasesAsync(context)).ToList();
            var hasAnyMatches = matchingCases.Any();
            var mode = context.Get(Mode);

            // Determine the outcomes based on the matching cases
            var results = mode == SwitchMode.MatchFirst
                ? hasAnyMatches ? new[] { matchingCases.First() } : Array.Empty<FlowSwitchCase>()
                : matchingCases.ToArray();

            var outcomes = hasAnyMatches
                ? results.Select(r => r.Label).ToArray()
                : new[] { "Default" };

            // Complete the activity with the determined outcomes
            await context.CompleteActivityAsync(new Outcomes(outcomes));
        }

        private async Task<IEnumerable<FlowSwitchCase>> FindMatchingCasesAsync(ActivityExecutionContext context)
        {
            var matchingCases = new List<FlowSwitchCase>();
            var expressionEvaluator = context.GetRequiredService<IExpressionEvaluator>();

            foreach (var switchCase in Cases)
            {
                // Ensure the condition is treated as a string
                if (switchCase.Condition is not string conditionString)
                {
                    throw new InvalidOperationException("Condition must be a string.");
                }

                // Wrap the condition in an Expression object
                var conditionExpression = new Expression("JavaScript", conditionString);

                // Evaluate the condition using workflow variables
                var result = await expressionEvaluator.EvaluateAsync<bool?>(
                    conditionExpression,
                    context.ExpressionExecutionContext
                );

                if (result == true)
                {
                    matchingCases.Add(switchCase);
                }
            }

            return matchingCases;
        }
    }

    public class FlowSwitchCase
    {
        public string Label { get; set; } = default!;
        public string Condition { get; set; } = default!; // This is the raw condition string
    }

    public enum SwitchMode
    {
        MatchFirst,
        MatchAll
    }
}

