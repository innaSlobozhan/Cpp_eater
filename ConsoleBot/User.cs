using ConsoleBot.TestApi;

namespace ConsoleBot
{
    class User
    {
        long chatID;
        int level;
        public TestManager Manager { get; } = new TestManager();
        public long ChatID { get { return chatID; } }
        public int Level { get { return level; } set { level = value; } }
        public User(long chatID)
        {
            this.chatID = chatID;
            this.level = 0;
        }
        public void LevelUp()
        {
            level++;
        }
    }
}
