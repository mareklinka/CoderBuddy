using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace CoderBuddy.Actions.Available
{
    [UsedImplicitly]
    public sealed class PrettifyXmlAction : ActionBase
    {
        private static readonly Regex Regex = CreateRegex("prett(y|ify) xml (?<xml>.*)");

        public override bool Supports(ActivityPayload payload) => Regex.IsMatch(payload.Text);

        public override ActivityResult Execute(ActivityPayload payload)
        {
            try
            {
                var match = Regex.Match(payload.Text);

                var doc = XDocument.Parse(match.Groups["xml"].Value);
                var result = System.Web.HttpUtility.HtmlEncode(doc.ToString().Replace(Environment.NewLine, "\n\n"));
                return new ActivityResult($"Prettified XML:\n\n{result}");
            }
            catch (Exception)
            {
                return new ActivityResult($"Could not prettify the provided data.");
            }
        }

        public override string Name => "Prettify XML";
        public override string Description => "Reformats XML data into a more readable format.";
        public override string Syntax => "prettify _xml_";
        public override string[] Examples => new[] { "pretty xml <response><error code=\"1\"> Success</error></response>", "prettify xml <response><error code=\"1\"> Success</error></response>" };
    }
}