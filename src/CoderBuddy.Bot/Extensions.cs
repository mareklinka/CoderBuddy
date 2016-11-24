using CoderBuddy.Actions;
using Microsoft.Bot.Connector;

namespace CoderBuddy.Bot
{
    public static class Extensions
    {
        public static ActivityPayload ToPayload(this Activity activity)
        {
            return new ActivityPayload(activity.Text);
        }
    }
}