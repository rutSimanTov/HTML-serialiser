using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace p2
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Child { get; set; }
        public Selector Parent { get; set; }

        public Selector SelectorTree(string str)
        {
            //מוחק רווחים מיותרים
            str = Regex.Replace(str.Trim(), @"\s{2,}", " ");
            if (str.Equals(""))
                return null;
            var mySelect = str.Split(" ");
            Selector root = new();
            Selector current = new();

            for (int i = 0; i < mySelect.Length; i++)
            {

                string tname = Regex.Match(mySelect[i], @"^(\w+)").Groups[1].Value;


                if (tname.Length > 0)
                {
                    if (HtmlHelper.Instance.AllTag.Contains(tname) || HtmlHelper.Instance.VoidTag.Contains(tname))
                        current.TagName = tname;
                }

                current.Id = Regex.Match(mySelect[i], @"#(\w+)").Groups[1].Value;


                foreach (Match match in Regex.Matches(mySelect[i], @"\.(\w+[-\w]*)"))
                {
                    if (current.Classes == null)
                        current.Classes = new List<string>();
                    current.Classes.Add(match.Groups[1].Value);
                }
                if (i == 0)
                    root = current;

                if (i < mySelect.Length - 1)
                {
                    Selector selector = new();
                    current.Child = selector;
                    selector.Parent = current;
                    current = selector;
                }

            }



            return root;
        }
    }
}