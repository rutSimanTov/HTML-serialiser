
using p2;
using System;
using System.ComponentModel.Design;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

var html = await Load("http://ariella.learn.fun.ruti.tamar.s3-website-us-east-1.amazonaws.com/");

var cleanHtml = new Regex("\\s").Replace(html, " ");
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0).ToList();//המרנו לרשימה .ToList()

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}



//delete empty lines

for (int i = 0; i < htmlLines.Count; i++)
{
    if (string.IsNullOrWhiteSpace(htmlLines[i]))
        htmlLines.Remove(htmlLines[i]);
}


//-----------build tree------

HtmlElement current = new();

for (int i = 0; i < htmlLines.Count; i++)
{
    string word = htmlLines[i].Split(' ')[0];

    if (word.Equals("/html"))
        break;


    else if (word[0].Equals('/'))
        current = current.Parent;

    else if (HtmlHelper.Instance.AllTag.Contains(word) || HtmlHelper.Instance.VoidTag.Contains(word))
    {
        HtmlElement e = new(word);

        if (word.Equals("html"))
            current = e;

        if (e!=current)
        {
            
            if (current.Children == null)
                current.Children = new List<HtmlElement>(); 
            
            current.Children.Add(e);
            e.Parent = current;     
        }


        var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlLines[i]);
        foreach (Match attr in attributes)
        { 
            //get key
            string attr2 = attr.Value;
            string key = attr2.Split("=")[0];

            //get value
            int startIndex = attr2.IndexOf("\"") + 1;
            int length = attr2.LastIndexOf("\"") - startIndex;
            string value = attr2.Substring(startIndex, length);
            value = value.ToString();


            if (key.Equals("class")) {
                if (e.Classes == null)
                    e.Classes = new List<string>();
                e.Classes.AddRange(value.Split(' ').ToList());
            }

            else if (key.Equals("id"))
                e.Id = value;

            else { 
                if (e.Attributes == null)
                    e.Attributes = new List<string>(); 

                e.Attributes.Add(attr2 + " "); 
                 }

        }

        if (!HtmlHelper.Instance.VoidTag.Contains(word))
            current = e; 
    }
    else
        current.InnerHtml = htmlLines[i];
    
}
HtmlElement.Root = current;























//running
//IEnumerable<HtmlElement> h = HtmlElement.Root.Descendants();
//foreach (HtmlElement item in h)
//{
//    item.Print();

//    Console.WriteLine("\n  ----  ");
//}


//selector
Selector s = new();
Selector s1 = s.SelectorTree("body p");
List<HtmlElement> htmlElements = current.FindElement(s1);

foreach (HtmlElement e in htmlElements)
{
    Console.Write("the element is:  "); e.Print();
    Console.WriteLine("   ----   ");
}


