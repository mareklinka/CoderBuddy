using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CoderBuddy.Actions.Available
{
    public sealed class HelpAction : ActionBase
    {
        private static readonly Regex Regex = CreateRegex("(help( me)?|wtf|dafuq)");

        private readonly IEnumerable<ActionBase> _actions;

        public override bool Supports(ActivityPayload payload) => Regex.IsMatch(payload.Text);

        public HelpAction(IEnumerable<ActionBase> actions)
        {
            _actions = actions;
        }

        public override ActivityResult Execute(ActivityPayload payload)
        {
            var builder = new StringBuilder();

            var nonSecretMessages = new List<ActionBase> { this }.Concat(_actions).Where(_ => !_.IsSecret).ToList();
            var i = 0;
            foreach (var action in nonSecretMessages)
            {
                builder.Append($"__{action.Name}__\n\n");
                builder.Append($"{action.Description}\n\n");
                builder.Append("__Syntax__\n\n");
                builder.Append($"  {action.Syntax}\n\n");
                builder.Append("__Example__\n\n");

                foreach (var example in action.Examples)
                {
                    builder.Append($"  {example}\n\n");
                }

                if (i < nonSecretMessages.Count - 1)
                {
                    builder.Append("-----\n\n");
                }

                ++i;
            }

            return new ActivityResult(builder.ToString());
        }

        public override string Name => "Help";
        public override string Description => "Displays this help information";
        public override string Syntax => "Help";
        public override string[] Examples => new[] {"_help_", "_help me_"};
    }
}