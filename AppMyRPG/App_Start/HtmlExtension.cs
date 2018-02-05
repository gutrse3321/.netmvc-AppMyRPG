using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppMyRPG
{
    public static class HtmlExtension
    {
        public static MvcHtmlString Submit(this HtmlHelper helper, string value)
        {
            TagBuilder tag = new TagBuilder("input");
            tag.MergeAttribute("type", "submit");
            tag.MergeAttribute("value", value);

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString Submit(this HtmlHelper helper, string value,string mclass)
        {
            TagBuilder tag = new TagBuilder("input");
            tag.MergeAttribute("type", "submit");
            tag.MergeAttribute("value", value);
            tag.MergeAttribute("class", mclass);

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.SelfClosing));
        }
    }
}