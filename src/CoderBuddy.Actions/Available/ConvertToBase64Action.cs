using System;
using System.Text.RegularExpressions;

namespace CoderBuddy.Actions.Available
{
    public sealed class ConvertToBase64Action : ActionBase
    {
        private static readonly Regex Regex =
            CreateRegex("convert (?<data>.*) to Base( )?64( in (?<encoding>(ASCII|UTF( )?7|UTF( )?8|UTF( )?32)))?");

        public override bool Supports(ActivityPayload payload) => Regex.IsMatch(payload.Text);

        public override ActivityResult Execute(ActivityPayload payload)
        {
            var match = Regex.Match(payload.Text);

            var stringData = match.Groups["data"].Value;
            var encoding = match.Groups["encoding"].Value?.Replace(" ", string.Empty).ToUpperInvariant();

            if (string.IsNullOrEmpty(stringData))
            {
                return new ActivityResult("No data");
            }

            byte[] data;
            switch (encoding)
            {
                case "ASCII":
                    data = System.Text.Encoding.ASCII.GetBytes(stringData);
                    break;
                case "UTF7":
                    data = System.Text.Encoding.UTF7.GetBytes(stringData);
                    break;
                case "UTF8":
                    data = System.Text.Encoding.UTF8.GetBytes(stringData);
                    break;
                case "UTF32":
                    data = System.Text.Encoding.UTF32.GetBytes(stringData);
                    break;
                default:
                    data = System.Text.Encoding.ASCII.GetBytes(stringData);
                    encoding = "ASCII";
                    break;
            }

            var base64 = Convert.ToBase64String(data);

            return new ActivityResult($"Converting _{stringData}_ to Base64 in _{encoding}_:\n\n{base64}");
        }

        public override string Name => "Convert string to Base64";
        public override string Description => "Converts the specified string into a Base64 string.";
        public override string Syntax => "Convert _string to convert_ to base64[ in (ASCII|UTF7|UTF8|UTF32)]";

        public override string[] Examples
            =>
            new[]
            {
                "Convert Yo, this is a handy feature to base64", "Convert wooot to BASE 64",
                "Convert Cool story to base 64 in UTF8"
            };
    }
}