using System.Collections;
using System.Collections.Generic;

namespace CoderBuddy.Actions
{
    public sealed class ActivityResult
    {
        public string Message { get; }
        public ICollection<ActivityResultAttachment> Attachments { get; } = new List<ActivityResultAttachment>();

        public ActivityResult(string message)
        {
            Message = message;
        }

        public ActivityResult(string message, ICollection<ActivityResultAttachment> attachments)
        {
            Message = message;
            Attachments = attachments;
        }
    }
}