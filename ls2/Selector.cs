using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ls2
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }
        public Selector()
        {
            Classes = new List<string>();
        }
        public static Selector Parse(string query)
        {
            // פירוק המחרוזת לחלקים לפי רווחים
            var parts = query.Split(' ');

            // יצירת אוביקט שורש
            var root = new Selector();

            // משתנה זמני לעבודה עם הסלקטור הנוכחי
            var current = root;

            // לולאה על רשימת החלקים
            foreach (var part in parts)
            {
                // פירוק החלק לפי # ו-.
                var subparts = part.Split('#', '.');

                // עדכון מאפייני הסלקטור הנוכחי
                foreach (var sub in subparts)
                {
                    if (sub.StartsWith("#"))
                    {
                        current.Id = sub.Substring(1);
                    }
                    else if (sub.StartsWith("."))
                    {
                        current.Classes.Add(sub.Substring(1));
                    }
                    else if (IsValidHtmlTag(sub))
                    {
                        current.TagName = sub;
                    }
                }

                // יצירת אוביקט סלקטור חדש והוספתו כבן
                var child = new Selector();
                current.Child = child;

                // עדכון הסלקטור הנוכחי
                current = child;
            }

            return root;
        }

        public void ToString()
        {
            StringBuilder sb = new StringBuilder();
            while (Child != null)
            {
                if (!string.IsNullOrEmpty(Child.TagName))
                {
                    Console.Write("TagName: " + Child.TagName);
                }
                // Print ID
                if (!string.IsNullOrEmpty(Child.Id))
                {
                    Console.Write("Id: " + Child.Id);
                }
                if (Child.Classes.Count > 0)
                {
                    Console.Write(" class=\"");
                    Console.Write(string.Join(" ", Child.Classes));
                    Console.Write("\"");
                }
                Child = Child.Child;
                Console.WriteLine();
            }

        }
        private static bool IsValidHtmlTag(string tagName)
        {
            // תווים חוקיים בשם תגית HTML
            var validChars = new HashSet<char>(
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_");

            // בדיקת תקינות כל תו
            return tagName.All(c => validChars.Contains(c));
        }

    }
}
