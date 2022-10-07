using Microsoft.AspNetCore.Razor.TagHelpers;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApp02.TagHelpers
{
    /*public class MultiSelectTagHelper : TagHelper
    {
        public Dictionary<int, string> Items { get; set; } = new();
        public IEnumerable<int> SelectedItems { get; set; } = new List<int>();
        public int SelectedItem { get; set; } = 0;
        public int Size { get; set; } = 1;
        public bool Multiple { get; set; } = false;
        public object For { get; set; } = "Id";
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "select";
            output.TagMode = TagMode.StartTagAndEndTag;
            if (Multiple)
            {
                output.Attributes.SetAttribute("multiple", "");
                output.Attributes.SetAttribute("size", Size);
            }
            else
            {
                SelectedItems = new List<int>() { SelectedItem };
            }
            output.Attributes.SetAttribute("id", For);
            output.Attributes.SetAttribute("name", For);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in Items)
            {
                string selected = SelectedItems.Contains(item.Key) ? " selected" : "";
                stringBuilder.Append($"<option value={item.Key}{selected}>{item.Value}</option>");
            }
            output.Content.SetHtmlContent(stringBuilder.ToString());
            //base.Process(context, output);
        }
    }*/
}
