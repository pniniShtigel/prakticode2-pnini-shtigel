using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace ls2
{
    internal class HtmlHelper
    {
        private static readonly HtmlHelper _htmlHelper = new HtmlHelper();
        public static HtmlHelper Helper => _htmlHelper;
        public string[] AllTags { get; set; }
        public string[] SelfClosing { get; set; }
        private HtmlHelper()
        {         
            var context = File.ReadAllText("AllTag.json");
            AllTags = JsonSerializer.Deserialize<string[]>(context);
            var v22 = File.ReadAllText("SelfClosingTags.json");
            SelfClosing = JsonSerializer.Deserialize<string[]>(context);
        }
    }
}
