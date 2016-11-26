using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace CoderBuddy.Actions
{
    public abstract class ActionBase
    {
        public abstract bool Supports(ActivityPayload payload);

        public abstract ActivityResult Execute(ActivityPayload payload);

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract string Syntax { get; }

        public abstract string[] Examples { get; }

        protected static Regex CreateRegex([RegexPattern]string pattern)
        {
            return new Regex($"^(\\s*){pattern}(\\s*)$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline);
        }

        public virtual bool IsSecret => false;
    }
}
