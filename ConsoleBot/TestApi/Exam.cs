﻿using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleBot.TestApi
{
    class Exam
    {
        public List<Test> tests = new List<Test>();
        private int testPos = 0;
        private int answerCount = 0;

        public void AddTest(Test test)
        {
            tests.Add(test);
        }


        public void ShowTest(TelegramBotClient bot, long ChatId, InlineKeyboardMarkup keyboard)
        {
            try
            {
                bot.SendTextMessageAsync(ChatId, tests[testPos].ShowTest(), replyMarkup: keyboard);
            }
            catch (Exception)
            { 

            }
        }
        public void TakeAnswer(TelegramBotClient bot, User currentUser, InlineKeyboardMarkup keyboard, int answer)
        {
            if(testPos <= tests.Count - 1)
            {
                if (tests[testPos].GetAnswer()[answer - 1])
                    answerCount++;
                testPos++;
                ShowTest(bot, currentUser.ChatID, keyboard);
            }
            else
            {
                if(answerCount >= tests.Count - 1)
                {

                    bot.SendTextMessageAsync(currentUser.ChatID, $"Ви успішно пройшли тест!\nВи відповіли на {answerCount} питань з {tests.Count}.");
                    answerCount = 0;
                    testPos = 0;

                    if (currentUser.Level < 10)
                        currentUser.LevelUp();

                    XMLmanager.UpdateLevel(currentUser);
                }
                else if(answerCount < tests.Count - 1)
                {
                    bot.SendTextMessageAsync(currentUser.ChatID, $"Ви не пройшли тест! Визвіть команду /study\nВи відповіли на {answerCount} питань з {tests.Count}.");
                    answerCount = 0;
                    testPos = 0;
                }
                    
            }
        }
    }
}