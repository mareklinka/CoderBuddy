using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace CoderBuddy.Actions.Available
{
    [UsedImplicitly]
    public sealed class SearchStackOverflowAction : ActionBase
    {
        private static readonly Regex Regex = CreateRegex("(stackoverflow|so) (?<search>.*)");

        public override bool Supports(ActivityPayload payload) => Regex.IsMatch(payload.Text);

        public override ActivityResult Execute(ActivityPayload payload)
        {
            var match = Regex.Match(payload.Text);

            try
            {
                var searchString = match.Groups["search"].Value;
                var searchStringEncoded = HttpUtility.UrlEncode(searchString);

                var url = $"https://www.bing.com/search?q=site%3Astackoverflow.com+{searchStringEncoded}";
                var web = new HtmlWeb();
                web.PreRequest = request => request.AllowAutoRedirect = true;
                var doc = web.Load(url);
                var searchResults =
                    doc.DocumentNode.Descendants("li")
                        .Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("b_algo"));

                var links = searchResults.Select(_ => _.Descendants("a").First()).ToList();
                var resultLinks = string.Join("\n\n",
                    links.Select(_ => $"* [{_.InnerText}]({_.Attributes["href"].Value})"));

                var result = $"Search StackOverflow for _{searchString}_:\n\n{resultLinks}";
                return new ActivityResult(result);
            }
            catch (Exception)
            {
                return new ActivityResult("Searching StackOverflow failed.");
            }
        }

        public override string Name => "Search StackOverflow";
        public override string Description => "Searches StackOverflow for the given term";
        public override string Syntax => "so _search terms_";

        public override string[] Examples => new[] {"so MS Bot Framework", "stackoverflow winforms"};
    }
}