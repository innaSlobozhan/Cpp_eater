﻿using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleBot
{
    class Program
    {
        #region Vars
        static string[] Lectures = { "https://telegra.ph/Osnovi-OOP-Lekc%D1%96ya-1-Vstup-10-27", "https://telegra.ph/Osnovi-OOP-Lekc%D1%96ya-2-10-28", "https://telegra.ph/Osnovi-OOP-Lekc%D1%96ya-3-10-28", "https://telegra.ph/Osnovi-OOP-Lekc%D1%96ya-4-10-28", "https://telegra.ph/Osnovi-OOP-Lekc%D1%96ya-5-10-28", "https://telegra.ph/Osnovi-OOP-Lekc%D1%96ya-6-10-29", "https://telegra.ph/Osnovi-OOP-Lekc%D1%96ya-7-10-29", "https://telegra.ph/Osnovi-OOP-Lekcіya-8-10-29", "https://telegra.ph/Osnovi-OOP-Lekcіya-9-10-29", "https://telegra.ph/Osnovi-OOP-Lekc%D1%96ya-10-10-29" };
        static InlineKeyboardMarkup inlineKeyboardOK = new InlineKeyboardMarkup(new InlineKeyboardButton() { Text = "Почати тестування", CallbackData = "OK"});
        static InlineKeyboardMarkup urlButton = new InlineKeyboardMarkup(new InlineKeyboardButton() { Text = "Проект на  Github", Url = "https://github.com/innaSlobozhan/Cpp_eater", CallbackData = "URL"});

        static ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(new KeyboardButton[]
        {
            new KeyboardButton(){Text = "1"},
            new KeyboardButton(){Text = "2"},
            new KeyboardButton(){Text = "3"},
            new KeyboardButton(){Text = "4"}
        }, resizeKeyboard: true);

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
            bool truth = true;
            while (truth)
            {
                string str = Console.ReadLine();
                string[] com = str.Split(" ");
                switch (com[0])
                {
                    case "send":
                        Client.SendTextMessageAsync(com[1], com[2]);
                        break;
                    case "exit":
                        truth = false;
                        break;
                }
            }
            Client.StopReceiving();
        }

        static void Command_Handler(object Sender, MessageEventArgs e)
        {
            User currentUser = users.Find(user => user.ChatID == e.Message.Chat.Id);
            try
            {
                #region Commands
                switch (e.Message.Text)
                {
                    case "/start":
                        ExtensionList.Add(users, new User(e.Message.Chat.Id));
                        Client.SendTextMessageAsync(e.Message.Chat.Id, "Для виклику лекцій використовуйте команду /study. Уважно прочитайте її вміст і приступайте до тестів, в випадку успішного проходження (мінімум 60% вірних відповідей) Ви отримуєте доступ до наступної лекції. Вдалого навчання 😉");
                        break;
                    case "/study":
                        if (currentUser.Level < 10)
                        {

                            Client.SendTextMessageAsync(currentUser.ChatID, Lectures[currentUser.Level], replyMarkup: inlineKeyboardOK);
                            currentUser.Manager.SetXFile("Tests\\Test" + currentUser.Level + ".xml");
                            currentUser.Manager.ReadTest();
                        }
                        else
                        {
                            Client.SendTextMessageAsync(e.Message.Chat.Id, "Ви вже закінчили навчання ✅ \nЩоб пройти курс заново, оберіть команду /reset.");
                        }
                        break;
                    case "/showlectures":
                        string @string = "";
                        for (int i = 0; i <= currentUser.Level; i++)
                        {
                            if(i < 10)
                                @string += $"{i + 1}. " + Lectures[i] + "\n";
                        }
                        Client.SendTextMessageAsync(currentUser.ChatID, @string);
                        break;
                    case "/reset":
                        currentUser.Level = 0;
                        Client.SendTextMessageAsync(currentUser.ChatID, "Повернення до початкового рівня ⏪");
                        XMLmanager.UpdateLevel(currentUser);
                        break;
                    case "/donate":
                        Client.SendTextMessageAsync(currentUser.ChatID, "Ви можете оцінити наш проект або почати стежити за ним за наступним посиланням 😉", replyMarkup: urlButton);
                        break;
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                        currentUser.Manager.Examination.TakeAnswer(Client, currentUser, replyKeyboard, Convert.ToInt32(e.Message.Text));
                        break;
                    default:
                        Client.SendTextMessageAsync(currentUser.ChatID, "Невідома команда!");
                        break;
                }
                #endregion
            }
            catch(NullReferenceException nullEx)
            {
                ExtensionList.Add(users, new User(e.Message.Chat.Id));
                Client.SendTextMessageAsync(e.Message.Chat.Id, "Для виклику лекцій використовуйте команду /study. Уважно прочитайте її вміст і приступайте до тестів, в випадку успішного проходження (мінімум 60% вірних відповідей) Ви отримуєте доступ до наступної лекції. Вдалого навчання 😉");
                Console.WriteLine(nullEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void Keyboard_Handler(object sender, CallbackQueryEventArgs e)
        {
            User currentUser = users.Find(user => user.ChatID == e.CallbackQuery.Message.Chat.Id);
            try
            {
                if (e.CallbackQuery.Data == "OK")
                {
                    Client.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
                    currentUser.Manager.Examination.ShowTest(Client, currentUser.ChatID, replyKeyboard);
                }
                else if (e.CallbackQuery.Data == "URL")
                {
                    Client.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
                }
            }
            catch (NullReferenceException nullEx)
            {
                ExtensionList.Add(users, new User(e.CallbackQuery.Message.Chat.Id));
                Client.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Для виклику лекцій використовуйте команду /study. Уважно прочитайте її вміст і приступайте до тестів, в випадку успішного проходження (мінімум 60% вірних відповідей) Ви отримуєте доступ до наступної лекції. Вдалого навчання 😉");
                Console.WriteLine(nullEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
