namespace CoderBuddy.Actions
{
    public sealed class ActivityResult
    {
        public string Message { get; }

        public ActivityResult(string message)
        {
            Message = message;
        }
    }
}