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

        static InlineKeyboardMarkup inlineKeyboardOK = new InlineKeyboardMarkup(new InlineKeyboardButton() { Text = "Почати тестування", CallbackData = "OK"});
        static InlineKeyboardMarkup urlButton = new InlineKeyboardMarkup(new InlineKeyboardButton() { Text = "Проект на  Github", Url = "https://github.com/innaSlobozhan/Cpp_eater", CallbackData = "URL"});

        static ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(new KeyboardButton[][]
        {
            new KeyboardButton[]
            {
                new KeyboardButton(){Text = "1"},
                new KeyboardButton(){Text = "2"}
            },
            new KeyboardButton[]
            {
                new KeyboardButton(){Text = "3"},
                new KeyboardButton(){Text = "4"}
            }
        });

        static List<User> users = new List<User>();
        
        static TelegramBotClient Client;
        #endregion
        static void Main ()
        {
            Client = new TelegramBotClient("1050115445:AAFMHnHtznwJNdwZtQ_J0kKF6iqZ8K9EnX4");
            XMLmanager.ReadFile(users);

            Client.OnMessage += Command_Handler;
            Client.OnCallbackQuery += Keyboard_Handler;
            Client.StartReceiving();
            Console.ReadKey(true);
            Client.StopReceiving();
        }

        static void Command_Handler(object Sender, MessageEventArgs e)
        {
            User currentUser = users.Find(user => user.ChatID == e.Message.Chat.Id);

            #region Commands
            switch (e.Message.Text)
            {
                case "/start":
                    Client.SendTextMessageAsync(currentUser.ChatID, "Для виклику лекцій використовуйте команду /study. Уважно прочитайте її вміст і приступайте до тестів, в випадку успішного проходження (мінімум 4 тести з 5) Ви отримуєте доступ до наступної лекції.");
                    ExtensionList.Add(users, new User(e.Message.Chat.Id));
                    break;
                case "/study":
                    Client.SendTextMessageAsync(currentUser.ChatID, Lectures[currentUser.Level], replyMarkup: inlineKeyboardOK);
                    currentUser.Manager.SetXFile("Tests\\Test" + currentUser.Level + ".xml");
                    currentUser.Manager.ReadTest();
                    break;
                case "/showlectures":
                    string @string = "";
                    for (int i = 0; i <= currentUser.Level; i++)
                    {
                        @string += $"{i + 1}. " + Lectures[i] + "\n";
                    }
                    Client.SendTextMessageAsync(currentUser.ChatID, @string);
                    break;
                case "/reset":
                    currentUser.Level = 0;
                    Client.SendTextMessageAsync(currentUser.ChatID, "Повернення до початкового рівня.");
                    break;
                case "/donate":
                    Client.SendTextMessageAsync(currentUser.ChatID, "Ви можете оцінити наш проект або почати стежити за ним за наступним посиланням.", replyMarkup: urlButton);
                    break;
                case "1": case "2" :case "3": case "4":
                    currentUser.Manager.Examination.TakeAnswer(Client, currentUser, replyKeyboard, Convert.ToInt32(e.Message.Text));
                    break;
                default:
                    Client.SendTextMessageAsync(currentUser.ChatID, "Невідома команда!");
                    break;
            }
            #endregion
        }

        static void Keyboard_Handler(object sender, CallbackQueryEventArgs e)
        {
            User currentUser = users.Find(user => user.ChatID == e.CallbackQuery.Message.Chat.Id);
            
            if(e.CallbackQuery.Data == "OK")
            {
                Client.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
                currentUser.Manager.Examination.ShowTest(Client, currentUser.ChatID, replyKeyboard);

            }
            else if(e.CallbackQuery.Data == "URL")
            {
                Client.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
            }
        }
    }
}
