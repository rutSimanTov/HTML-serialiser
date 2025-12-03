using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace p2
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

        public static HtmlElement Root { get; set; }

        public HtmlElement() { }
        public HtmlElement(string name) { this.Name = name; }

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> q = new Queue<HtmlElement>();
            q.Enqueue(this);
            while (q.Count > 0)
            {
                HtmlElement el = q.Dequeue();
                yield return el;
                if (el.Children != null)
                {
                    for (int i = 0; i < el.Children.Count; i++)
                        q.Enqueue(el.Children[i]);
                }
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement element = this;
            while (element.Parent != null)
            {
                yield return element.Parent;
                element = element.Parent;

            }
        }


        public List<HtmlElement> FindElement(Selector selector)
        {
            HashSet<HtmlElement> list = new();
            if (selector == null)
                return list.ToList();
            Find2(this, selector, list);

            return list.ToList();

        }

        private void Find2(HtmlElement element, Selector selector, HashSet<HtmlElement> list)
        {
            if (selector == null)
            {
                list.Add(element);
                return;
            }
            var childern = element.Descendants();

            foreach (var child in childern)
            {
                if (selector.TagName == null)
                    selector.TagName = "";
                if (selector.Id == null)
                    selector.Id = "";
                if (selector.Classes == null)
                    selector.Classes = new List<string>();
                if (child.Classes == null)
                    child.Classes = new List<string>();
                if ((selector.TagName.Length == 0 || selector.TagName.Equals(child.Name)))
                {
                    if (selector.Id.Length == 0 || selector.Id.Equals(child.Id))
                    {
                        var flag = true;
                        if (selector.Classes.Count > 0 && child.Classes == null && child.Classes.Count == 0)
                            flag = false;
                        if (child.Classes != null )
                            foreach (var cl in selector.Classes)
                                if (!child.Classes.Contains(cl))
                                    flag = false;

                        if (flag)
                            Find2(child, selector.Child, list);

                    }
                }
            }
        }


        public void Print()
        {
            Console.WriteLine( $"name:{this.Name} id:{Id} innerHtml:{this.InnerHtml}");
            if(this.Parent != null)
                Console.Write($"parent:{this.Parent.Name} ");

            if (this.Classes != null) {
                Console.Write("classes:");
            foreach (var item1 in this.Classes)
                Console.Write(item1+ ",");}
            
            
            if (this.Attributes != null) { Console.Write("atrribute:"); 
            foreach (var item2 in this.Attributes)
                Console.Write(item2 +",");}
           

            if (this.Children != null){
                Console.Write(" children:");
            foreach (var item3 in this.Children)
                Console.Write( item3.Name+",");
                
            }
        }











    }
}
