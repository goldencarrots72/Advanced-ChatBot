using System;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace QnAMakerBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }


        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var message = context.MakeMessage();
            message.Text = activity.Text;

            await context.PostAsync("about to call text analytics");
            TextAnalytics textAnalytics = new TextAnalytics();
            string response = await textAnalytics.Start(activity.Text);
            await context.PostAsync(response);

            await context.PostAsync("about to call qna");
            QnAMaker qna = new QnAMaker();
            response = await qna.TryQuery(activity.Text);
            await context.PostAsync(response);

            if (response.Contains("Fall Back Response"))
            {
                await context.PostAsync("about to call luis");
                await context.Forward(new LUIS(), AfterLuis, activity, System.Threading.CancellationToken.None);
                context.Done(true);
            }

        }

        private async Task AfterLuis(IDialogContext context, IAwaitable<object> result)
        {
            var response = await result;
            //await context.PostAsync(response.ToString());
            context.Wait(MessageReceivedAsync);
        }
                
    }
}