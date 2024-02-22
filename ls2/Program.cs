using ls2;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
// טעינת קוד HTML
var html = await Load("https://learn.malkabruk.co.il/practicode/");

// ניקוי רווחים מיותרים
var cleanHtml = new Regex("\\s+").Replace(html, " "); 
// פיצול קוד לפי תגיות
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);

var htmlElement = "span .md-ellipsis";

Selector selector1 = Selector.Parse(htmlElement);
//$$("span.md-ellipsis")
//$$("li.md-tabs__item")
//פונקציה הטוענת תוכן מכתובת 
static async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
static HtmlElement Serialize(List<string> htmlLines)
{
    //יצירת שורש העץ
    HtmlElement root = new HtmlElement();
    HtmlElement currentElement = root;//מצביע על שורש העץ

    foreach (var line in htmlLines)
    {
        //קבלת המילה הראשונה בשורה
        var firstWord = line.Split(' ')[0];

        if (firstWord.StartsWith("/html"))
        {
            break;
        }

        else if (firstWord.StartsWith("/"))
        {
            if (currentElement.Parent != null)
                currentElement = currentElement.Parent;
        }

        else if (HtmlHelper.Helper.AllTags.Contains(firstWord))
        {
            var newElement = new HtmlElement();
            newElement.Name = firstWord;
            var remainingString = Regex.Replace(line, @"^\w+\s*", "");
            ParseAttributes(remainingString, newElement);
            // Set the name and id attributes if present

            newElement.Parent = currentElement;
            currentElement.Children.Add(newElement);
            if (firstWord.EndsWith("/") || HtmlHelper.Helper.SelfClosing.Contains(firstWord))
            {
                currentElement = newElement.Parent;
            }
            else
                currentElement = newElement;
        }
        else
        {
            currentElement.InnerHtml = line;
        }
    }
    return root;
}
HtmlElement root = Serialize(htmlLines.ToList());
PrintTree(root);
//  שנבחר selctor  מציאת כל האלמנטים המתאימים  
var all = root.FindBySelector(selector1.Child);
foreach (var element in all)
{
    Console.WriteLine(element);
}
//הדפסת העץ
static void PrintTree(HtmlElement element)
{
    Console.WriteLine($"<{element.Name}>");

    // Print attributes
    if (element.Attributes.Count > 0)
    {
        Console.WriteLine("Attributes:");
        foreach (var attribute in element.Attributes)
        {
            Console.WriteLine($"\t{attribute}");
        }
    }

    // Print children
    if (element.Children.Count > 0)
    {
        Console.WriteLine("Children:");
        foreach (var child in element.Children)
        {
            PrintTree(child);
        }
    }

    // Print inner HTML
    if (!string.IsNullOrEmpty(element.InnerHtml))
    {
        Console.WriteLine($"InnerHtml: {element.InnerHtml}");
    }

    Console.WriteLine($"</{element.Name}>");
}

static void ParseAttributes(string attributeLine, HtmlElement element)
{
    var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(attributeLine);
    foreach (Match attribute in attributes)
    {
        var attributeName = attribute.Groups[1].Value;
        var attributeValue = attribute.Groups[2].Value;

        if (attributeName == "class")
        {
            // Handle the 'class' attribute by splitting it into parts and updating the 'Classes' property
            element.Classes.AddRange(attributeValue.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }
        else if (attributeName == "id")
        {
            // Change the 'Id' property to string and update it
            element.Id = attributeValue;
        }
        else
        {
            // Update the 'Attributes' property for other attributes
            element.Attributes.Add($"{attributeName}=\"{attributeValue}\"");
        }
    }
}
foreach (var item in all)
{
    var ance = item.Ancestors();
    foreach (var a in ance)
    {
        Console.WriteLine(a);
    }
}

