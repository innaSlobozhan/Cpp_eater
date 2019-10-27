using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ConsoleBot
{
    static class XMLmanager
    {
        static XmlDocument xDoc = new XmlDocument();
        static XmlElement xRoot;
        static XMLmanager()
        {
            xDoc.Load("Users\\Users.xml");
            xRoot = xDoc.DocumentElement;
        }

        static public void ReadFile(List<User> list)
        {
            foreach (XmlNode node in xRoot)
            {
                User user = new User(Convert.ToInt64(node.Attributes.GetNamedItem("ID").Value));
                foreach  (XmlNode childNode in node.ChildNodes)
                {
                    if(childNode.Name == "level")
                    {
                        user.Level = Convert.ToInt32(childNode.InnerText);
                    }
                }
                list.Add(user);
            }
        }
        static public void AddToFile(User user)
        {
            XmlElement userElement = xDoc.CreateElement("user");
            XmlAttribute xID = xDoc.CreateAttribute("ID");
            XmlElement xLevel = xDoc.CreateElement("level");
            XmlText userID = xDoc.CreateTextNode(Convert.ToString(user.ChatID));
            XmlText userLevel = xDoc.CreateTextNode(Convert.ToString(user.Level));

            xID.AppendChild(userID);
            xLevel.AppendChild(userLevel);
            userElement.Attributes.Append(xID);
            xLevel.AppendChild(userLevel);
            userElement.AppendChild(xLevel);
            xRoot.AppendChild(userElement);
            xDoc.Save("Users\\Users.xml");
        }
        static public void UpdateLevel(User user)
        {
            XmlNodeList xUser = xDoc.GetElementsByTagName("user");

            foreach (XmlNode item in xUser)
            {
                if (Convert.ToInt64(item.Attributes.GetNamedItem("ID").Value) == user.ChatID)
                    item.ChildNodes[0].InnerText = Convert.ToString(user.Level);
            }
            xDoc.Save("Users\\Users.xml");
        }
    }
}
