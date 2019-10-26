using System;
using System.Collections.Generic;

namespace ConsoleBot
{
    static class ExtensionList
    {
        public static void Add(this List<User> list, User user)
        {
            foreach (User item in list)
                if (item.ChatID == user.ChatID)
                    return;
            
            list.Add(user);
            XMLmanager.AddToFile(user);
            Console.WriteLine($"User add {user.ChatID}");

        } 
    }
}
