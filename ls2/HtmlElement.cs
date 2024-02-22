using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ls2
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement()
        {
            Attributes = new List<string> ();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
        }

        public IEnumerable<HtmlElement> Descendants()
        {
            // יצירת תור
            var queue = new Queue<HtmlElement>();

            // דחיפת האלמנט הנוכחי לתור
            queue.Enqueue(this);

            // לולאה כל עוד התור לא ריק
            while (queue.Count > 0)
            {
                // שליפת האלמנט הראשון מהתור
                var element = queue.Dequeue();

                // הוספת האלמנט לרשימה
                yield return element;

                // הוספת כל הבנים של האלמנט לתור
                foreach (var child in element.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            var current = this.Parent;

            // חזרה על כל ההורים עד להגעה לאלמנט null (שורש העץ)
            while (current != null)
            {
                // הוספת האלמנט הנוכחי לרשימה
                yield return current;

                // מעבר להורה הבא
                current = current.Parent;
            }
        }
        public IEnumerable<HtmlElement> FindBySelector(Selector selector)
        {
            return FindBySelector(selector, new List<HtmlElement>());

            // פונקציה ריקורסיבית
            IEnumerable<HtmlElement> FindBySelector(Selector currentSelector, List<HtmlElement> results)
            {
                // תנאי עצירה - הגענו לסוף הסלקטור
                if (currentSelector == null)
                {
                    // הוספת האלמנט הנוכחי לתוצאות
                    results.Add(this);
                    return results;
                }

                // מציאת כל הצאצאים שעונים לקריטריונים של הסלקטור הנוכחי
                var descendants = this.Descendants().Where(d =>
                    d.Name == currentSelector.TagName &&
                    d.Id == currentSelector.Id &&
                    d.Classes.Any(c => currentSelector.Classes.Contains(c)));

                // חזרה ריקורסיבית על כל הצאצאים המתאימים עם הסלקטור הבא
                foreach (var descendant in descendants)
                {
                    FindBySelector(currentSelector.Child, results);
                }

                return results;
            }
        }


    }
}
