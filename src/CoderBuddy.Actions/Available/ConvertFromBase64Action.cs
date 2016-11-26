using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace CoderBuddy.Actions.Available
{
    [UsedImplicitly]
    public sealed class ConvertFromBase64Action : ActionBase
    {
        private static readonly Regex Regex =
            CreateRegex("convert (?<data>.*) from Base( )?64( in (?<encoding>(ASCII|UTF( )?7|UTF( )?8|UTF( )?32)))?");

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

            try
            {
                var data = Convert.FromBase64String(stringData);
                string resultString;

                switch (encoding)
                {
                    case "ASCII":
                        resultString = System.Text.Encoding.ASCII.GetString(data);
                        break;
                    case "UTF7":
                        resultString = System.Text.Encoding.UTF7.GetString(data);
                        break;
                    case "UTF8":
                        resultString = System.Text.Encoding.UTF8.GetString(data);
                        break;
                    case "UTF32":
                        resultString = System.Text.Encoding.UTF32.GetString(data);
                        break;
                    default:
                        resultString = System.Text.Encoding.ASCII.GetString(data);
                        encoding = "ASCII";
                        break;
                }

                return new ActivityResult($"Converting _{stringData}_ to Base64 in _{encoding}_:\n\n{resultString}");
            }
            catch (FormatException)
            {
                return new ActivityResult($"Conversion failed. _{stringData}_ is not a valid Base64 string.");
            }
        }

        public override string Name => "Convert string to Base64";
        public override string Description => "Converts the specified string into a Base64 string.";
        public override string Syntax => "Convert _string to convert_ to base64[ in (ASCII|UTF7|UTF8|UTF32)]";
        public override string[] Examples => new[] { "Convert Yo, this is a handy feature to base64", "Convert wooot to BASE 64", "Convert Cool story to base 64 in UTF8" };
    }
}