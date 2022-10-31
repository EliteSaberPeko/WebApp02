using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebApp02.Models;
using WebApp02.ViewModel;

namespace WebApp02.TagHelpers
{
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            _urlHelperFactory = helperFactory;
        }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PageViewModel PageModel { get; set; }
        public string PageAction { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "div";

            TagBuilder tag = new("ul");
            tag.AddCssClass("pagination justify-content-center mb-0");

            TagBuilder currentItem = CreateTag(PageModel.PageNumber, "current-");
            TagBuilder prevItem = PageModel.HasPreviousPage ? CreateTag(PageModel.PageNumber - 1, "previous-") : CreateTag(0, "previous-");
            tag.InnerHtml.AppendHtml(prevItem);
            tag.InnerHtml.AppendHtml(currentItem);
            TagBuilder nextItem = PageModel.HasNextPage ? CreateTag(PageModel.PageNumber + 1, "next-") : CreateTag(0, "next-");
            tag.InnerHtml.AppendHtml(nextItem);
            output.Content.AppendHtml(tag);
        }

        TagBuilder CreateTag(int pageNumber, /*IUrlHelper urlHelper*/ string position)
        {
            TagBuilder item = new TagBuilder("li");
            TagBuilder link = new TagBuilder("button");
            link.Attributes["type"] = "button";
            if (pageNumber == PageModel.PageNumber)
                item.AddCssClass("active");
            else
            //link.Attributes["href"] = urlHelper.Action(PageAction, new { page = pageNumber });
            {
                link.Attributes["id"] = position + "page";
                link.Attributes["value"] = pageNumber.ToString();
            }

            item.AddCssClass("page-item");
            if (pageNumber == 0)
                item.AddCssClass("disabled");
            link.AddCssClass("page-link");
            switch (position)
            {
                case "previous-":
                    link.InnerHtml.Append("< Назад");
                    break;
                case "next-":
                    link.InnerHtml.Append("Вперед >");
                    break;
                default:
                    link.InnerHtml.Append(pageNumber.ToString());
                    break;
            }
            item.InnerHtml.AppendHtml(link);
            return item;
        }
    }
}
