using System.Text.RegularExpressions;

namespace CoderBuddy.Actions.Available
{
    public sealed class HelloAction : ActionBase
    {
        private static readonly Regex Regex = CreateRegex("(hello|hi|yo|wasup|duuude)");

        public override bool Supports(ActivityPayload payload) => Regex.IsMatch(payload.Text);

        public override ActivityResult Execute(ActivityPayload payload)
        {
            return new ActivityResult("Hi there! My name is __CoderBuddy__ and I want to help you with your programming-related tasks!\n\nType ___help___ to discover what I can do.");
        }

        public override string Name => "Hello";
        public override string Description => "Displays a welcome message.";
        public override string Syntax => "Hello";
        public override string[] Examples => new[] { "_hello_", "_hi_" };
    }
}