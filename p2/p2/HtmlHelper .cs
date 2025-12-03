using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace p2
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;

        public string[] AllTag { get; set; }
        public string[] VoidTag { get; set; }

        private HtmlHelper()
        {
            var at = File.ReadAllText("JSON Files\\HtmlTags.json");
            AllTag= JsonSerializer.Deserialize<string[]>(at);

            var vt= File.ReadAllText("JSON Files\\HtmlVoidTags.json");
            VoidTag = JsonSerializer.Deserialize<string[]>(vt);

           
        }
    }
}
