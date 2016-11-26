using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CoderBuddy.Actions;
using CoderBuddy.Actions.Available;
using Microsoft.Bot.Connector;

namespace CoderBuddy.Bot.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private readonly HelpAction _helpAction;
        private readonly List<ActionBase> _actions;

        public MessagesController(IEnumerable<ActionBase> actions, HelpAction helpAction)
        {
            _helpAction = helpAction;
            _actions = new List<ActionBase> {helpAction}.Concat(actions).ToList();
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                var payload = activity.ToPayload();

                var matchingActions = _actions.Where(_ => _.Supports(payload)).ToList();

                if (matchingActions.Count == 1)
                {
                    // return our reply to the user
                    var activityResult = matchingActions[0].Execute(payload);
                    var reply = activity.CreateReply(activityResult.Message);

                    reply.Attachments = new List<Attachment>();
                    foreach (var a in activityResult.Attachments)
                    {
                        reply.Attachments.Add(new Attachment(name: a.Name,content: a.Data, contentType: a.ContentType));
                    }
                    
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
                else
                {
                    // return our reply to the user
                    var reply =
                        activity.CreateReply(
                            $"I'm sorry, I didn't get that. Try typing __{_helpAction.Examples[0]}__ to see the kind of things I can do.");
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
            }
            else
            {
                await HandleSystemMessage(activity);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                var connector = new ConnectorClient(new Uri(message.ServiceUrl));
                var reply =
                    message.CreateReply(
                        "Your user information has been deleted.");
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing that the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }
        }
    }
}