using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace QnAMakerBot
{

    [LuisModel("key", "key")]
    [Serializable]
    public class LUIS : LuisDialog<object>
    {

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {

            var message = context.MakeMessage(); message.TextFormat = TextFormatTypes.Markdown;
            await context.PostAsync("You've triggered the None intent with " + result.Query);

            QnAMaker qna = new QnAMaker();
            message.Text = await qna.TryQuery(result.Query);
            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }
        [LuisIntent("Greetings")]
        public async Task Greetings(IDialogContext context, LuisResult result)
        {

            var message = context.MakeMessage(); message.TextFormat = TextFormatTypes.Markdown;
            message.Text = ("Hello there!");

            await context.PostAsync(message);
        }

        /// <summary>
        /// Tells the user where UPS's headquarters are located
        /// </summary>
        /// <param name="context">Context object provided by Microsoft Bot Framework</param>
        /// <param name="result">Object Provided by LUIS containing utterance information</param>
        [LuisIntent("HQ")]
        public async Task HQ(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage(); message.TextFormat = TextFormatTypes.Markdown;
            message.Text = ("The headquartered in my hometown of Atlanta, GA.");

            await context.PostAsync(message);

        }

        /// <summary>
        /// Tells user what the bot can help with
        /// </summary>
        /// <param name="context">Context object provided by Microsoft Bot Framework</param>
        /// <param name="result">Object Provided by LUIS containing utterance information</param>
        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage(); message.TextFormat = TextFormatTypes.Markdown;
            message.Text = ("I also need help though");

            await context.PostAsync(message);
        }

        [LuisIntent("SignUp")]
        public async Task MCSignUp(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage(); message.TextFormat = TextFormatTypes.Markdown;
            message.Text = ("Go to the website to sign up");

            await context.PostAsync(message);
        }

        [LuisIntent("Features")]
        public async Task FeatureList(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage(); message.TextFormat = TextFormatTypes.Markdown;
            message.Text = ("You've asked for the features list.");

            await context.PostAsync(message);

        }

    }
}
