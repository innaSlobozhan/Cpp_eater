using System;
using System.Xml;

namespace ConsoleBot.TestApi
{
    class TestManager
    {
        XmlDocument xDoc = new XmlDocument();
        XmlElement xRoot;
        public Exam Examination { get; } = new Exam();
        public void SetXFile(string xFile)
        {
            xDoc.Load(xFile);
            xRoot = xDoc.DocumentElement;
        }
        public void ReadTest()
        {
            Examination.tests.Clear();
            foreach (XmlNode node in xRoot)
            {
                Test currentTest = new Test();
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (childNode.Name == "Question")
                        currentTest.TestName = childNode.InnerText;
                    else if (childNode.Name == "Answer")
                        currentTest.AddQuestion(childNode.InnerText, Convert.ToBoolean(childNode.Attributes.GetNamedItem("answer").Value));
                }
                Examination.AddTest(currentTest);
            }
        }
    }
}
