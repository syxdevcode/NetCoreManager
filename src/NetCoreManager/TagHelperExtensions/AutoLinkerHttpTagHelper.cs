﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NetCoreManager.Mvc.TagHelperExtensions
{
    [HtmlTargetElement("p")]
    public class AutoLinkerHttpTagHelper:TagHelper
    {
        // This filter must run before the AutoLinkerWwwTagHelper as it searches and replaces http and 
        // the AutoLinkerWwwTagHelper adds http to the markup.
        //public override int Order
        //{
        //    get { return int.MinValue; }
        //}

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = output.Content.IsModified ? output.Content.GetContent() :
                (await output.GetChildContentAsync()).GetContent();

            // Find Urls in the content and replace them with their anchor tag equivalent.
            output.Content.SetHtmlContent(Regex.Replace(
                 childContent,
                 @"\b(?:https?://)(\S+)\b",
                  "<a target=\"_blank\" href=\"$0\">$0</a>"));  // http link version}
        }
    }
}
