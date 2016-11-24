namespace CoderBuddy.Actions
{
    public sealed class ActivityPayload
    {
        public ActivityPayload(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
