using System.Collections.Generic;

namespace ConsoleBot.TestApi
{
    class Test
    {
        struct Question
        {
            public string question;
            public bool answer;

            public Question(string question, bool answer)
            {
                this.question = question;
                this.answer = answer;
            }
        }
        public string TestName { get; set; }
        List<Question> questions = new List<Question>();

        public void AddQuestion(string question, bool answer)
        {
            questions.Add(new Question(question, answer));
        }

        public string ShowTest()
        {
            string[] emojies = { "1⃣", "2⃣", "3⃣", "4⃣" };
        
            string strQuest = "";
            int i = 0;

            strQuest += $"❓ {TestName}\n\n";

            foreach (Question item in questions)
            {
                strQuest += $"{emojies[i]} {item.question}\n\n";
                i++;
            }

            return strQuest;
        }

        public List<bool> GetAnswer()
        {
            List<bool> answers = new List<bool>();

            foreach (Question item in questions)
            {
                answers.Add(item.answer);
            }

            return answers;
        }
    }
}
