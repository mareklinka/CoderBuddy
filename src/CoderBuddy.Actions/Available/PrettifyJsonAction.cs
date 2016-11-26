using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace CoderBuddy.Actions.Available
{
    [UsedImplicitly]
    public sealed class PrettifyJsonAction : ActionBase
    {
        private static readonly Regex Regex = CreateRegex("prett(y|ify) json (?<json>.*)");

        public override bool Supports(ActivityPayload payload) => Regex.IsMatch(payload.Text);

        public override ActivityResult Execute(ActivityPayload payload)
        {
            try
            {
                var match = Regex.Match(payload.Text);

                var result = JsonConvert.DeserializeObject(match.Groups["json"].Value);

                var formatted = JsonConvert.SerializeObject(result, Formatting.Indented).Replace(Environment.NewLine, "\n\n");
                return new ActivityResult($"Prettified JSON:\n\n{formatted}");
            }
            catch (Exception)
            {
                return new ActivityResult($"Could not prettify the provided data.");
            }
        }

        public override string Name => "Prettify JSON";
        public override string Description => "Reformats JSON data into a more readable format.";
        public override string Syntax => "prettify _json_";
        public override string[] Examples => new[] { "pretty json [\"Starcraft\",\"Halo\",\"Legend of Zelda\"]", "prettify json [\"Starcraft\",\"Halo\",\"Legend of Zelda\"]" };
    }
}