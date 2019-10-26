using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleBot
{
    class Program
    {
        #region Vars
        static string[] Lectures = { "http://telegra.ph/October-2-2014-01-22", "http://www.telegraph.co.uk/news/worldnews/europe/italy/7223365/Silvio-Berlusconi-says-immigrants-not-welcome-but-beautiful-girls-can-stay.html" };
        static InlineKeyboardMarkup inlineKeyboardAnswers = new InlineKeyboardMarkup( new InlineKeyboardButton[][]
        {
            new InlineKeyboardButton[]
            {
                new InlineKeyboardButton(){Text = "1", CallbackData = "1"},
                new InlineKeyboardButton(){Text = "2", CallbackData = "2"}
            },
            new InlineKeyboardButton[]
            {
                new InlineKeyboardButton(){Text = "3", CallbackData = "3"},
                new InlineKeyboardButton(){Text = "4", CallbackData = "4"}
            }
        });
        static InlineKeyboardMarkup inlineKeyboardOK = new InlineKeyboardMarkup(new InlineKeyboardButton() { Text = "Почати тестування", CallbackData = "OK"});
        static InlineKeyboardMarkup urlButton = new InlineKeyboardMarkup(new InlineKeyboardButton() { Text = "Проект на  Github", Url = "https://www.youtube.com/watch?v=x_UfBJF47nk", CallbackData = "URL"}); 

        static List<User> users = new List<User>();
        
        static TelegramBotClient Client;
        #endregion
        static void Main ()
        {
            Client = new TelegramBotClient("1050115445:AAFMHnHtznwJNdwZtQ_J0kKF6iqZ8K9EnX4");
            XMLmanager.ReadFile(users);

            Client.OnMessage += FirstCommand;
            Client.OnCallbackQuery += Keyboard_Handler;
            Client.StartReceiving();
            Console.ReadKey(true);
            Client.StopReceiving();
        }

        static void FirstCommand(object Sender, MessageEventArgs e)
        {
            User currentUser = users.Find(user => user.ChatID == e.Message.Chat.Id);
            if (e.Message.Text == "/start")
            {
                Client.SendTextMessageAsync(currentUser.ChatID, "Для виклику лекцій використовуйте команду /study. Уважно прочитайте її вміст і приступайте до тестів, в випадку успішного проходження (мінімум 4 тести з 5) Ви отримуєте доступ до наступної лекції.");
                ExtensionList.Add(users, new User(e.Message.Chat.Id));
            }
            else if (e.Message.Text == "/study")
            {
                Client.SendTextMessageAsync(currentUser.ChatID, Lectures[currentUser.Level], replyMarkup: inlineKeyboardOK);
                currentUser.Manager.SetXFile("Tests\\Test" + currentUser.Level + ".xml");
                currentUser.Manager.ReadTest();
            }
            else if (e.Message.Text == "/showlectures")
            {
                string @string = "";
                for (int i = 0; i <= currentUser.Level; i++)
                {
                   @string += $"{i + 1}. " + Lectures[i] + "\n";
                }
                Client.SendTextMessageAsync(currentUser.ChatID, @string);
            }
            else if (e.Message.Text == "/debuglevel")
            {
                currentUser.Level = 0;
                Client.SendTextMessageAsync(currentUser.ChatID, "Повернення до початкового рівня.");
            }
            else if(e.Message.Text == "/donate")
            {
                Client.SendTextMessageAsync(currentUser.ChatID, "Ви можете оцінити наш проект або почати стежити за ним за наступним посиланням.", replyMarkup: urlButton);
            }
        }

        static void Keyboard_Handler(object sender, CallbackQueryEventArgs e)
        {
            User currentUser = users.Find(user => user.ChatID == e.CallbackQuery.Message.Chat.Id);
            
            if(e.CallbackQuery.Data == "OK")
            {
                currentUser.Manager.Examination.ShowTest(Client, currentUser.ChatID, inlineKeyboardAnswers);
            }
            else if(e.CallbackQuery.Data == "URL")
            {

            }
            else
            {
                currentUser.Manager.Examination.TakeAnswer(Client, currentUser, inlineKeyboardAnswers, Convert.ToInt32(e.CallbackQuery.Data));
            }
        }
    }
}
