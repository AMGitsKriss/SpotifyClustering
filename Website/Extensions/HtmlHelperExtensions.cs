using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Westwind.AspNetCore.Markdown;

namespace Website.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static string ParseMarkdown(this IHtmlHelper html, string markdown)
        {
            string rawHtml = Markdown.Parse(markdown);
            return rawHtml;
        }

        public static IHtmlContent Panel<TModel>(this IHtmlHelper<TModel> html, string title)
        {
            TagBuilder panel = new TagBuilder("div");
            panel.AddCssClass("panel panel-inverse");

            TagBuilder panelHeading = new TagBuilder("div");
            panelHeading.AddCssClass("panel-heading");

            TagBuilder panelTitle = new TagBuilder("h4");
            panelTitle.AddCssClass("panel-title");
            panelTitle.InnerHtml.Append(title);

            TagBuilder panelButtons = new TagBuilder("div");
            panelButtons.AddCssClass("panel-heading-btn");
            panelButtons.InnerHtml.AppendHtml("<a href=\"javascript:;\" class=\"btn btn-xs btn-icon btn-circle btn-default\" data-toggle=\"tooltip\" title=\"Maximise\" data-click=\"panel-expand\"><i class=\"fa fa-expand\"></i></a>");
            panelButtons.InnerHtml.AppendHtml("<a href=\"javascript:;\" class=\"btn btn-xs btn-icon btn-circle btn-success\" data-toggle=\"tooltip\" title=\"Refresh\" data-click=\"panel-reload\"><i class=\"fa fa-redo\"></i></a>");
            panelButtons.InnerHtml.AppendHtml("<a href=\"javascript:;\" class=\"btn btn-xs btn-icon btn-circle btn-warning\" data-toggle=\"tooltip\" title=\"Minimise\" data-click=\"panel-collapse\"><i class=\"fa fa-minus\"></i></a>");
            panelButtons.InnerHtml.AppendHtml("<a href=\"javascript:;\" class=\"btn btn-xs btn-icon btn-circle btn-danger\" data-toggle=\"tooltip\" title=\"Close\" data-click=\"panel-remove\"><i class=\"fa fa-times\"></i></a>");

            panelHeading.InnerHtml.AppendHtml(panelTitle);
            panelHeading.InnerHtml.AppendHtml(panelButtons);
            panel.InnerHtml.AppendHtml(panelHeading);

            TagBuilder panelBody = new TagBuilder("div");
            panelBody.AddCssClass("panel-body");
            //panelBody.InnerHtml.AppendHtml;
            panel.InnerHtml.AppendHtml(panelBody);

            //return panel;
            return panel;
        }

        public static IDisposable BeginPanel(this IHtmlHelper htmlHelper, string title, bool expandable = false, bool reloadable = false, bool minimisable = true, bool deletable = true)
        {
            htmlHelper.ViewContext.Writer.Write(@"<div class=""panel panel-inverse"">");

            TagBuilder panelHeading = new TagBuilder("div");
            panelHeading.AddCssClass("panel-heading");

            TagBuilder panelTitle = new TagBuilder("h4");
            panelTitle.AddCssClass("panel-title");
            panelTitle.InnerHtml.Append(title);

            TagBuilder panelButtons = new TagBuilder("div");
            panelButtons.AddCssClass("panel-heading-btn");
            if(expandable)
                panelButtons.InnerHtml.AppendHtml("<a href=\"javascript:;\" class=\"btn btn-xs btn-icon btn-circle btn-default\" data-toggle=\"tooltip\" title=\"Maximise\" data-click=\"panel-expand\"><i class=\"fa fa-expand\"></i></a>");
            if (reloadable)
                panelButtons.InnerHtml.AppendHtml("<a href=\"javascript:;\" class=\"btn btn-xs btn-icon btn-circle btn-success\" data-toggle=\"tooltip\" title=\"Refresh\" data-click=\"panel-reload\"><i class=\"fa fa-redo\"></i></a>");
            if (minimisable)
                panelButtons.InnerHtml.AppendHtml("<a href=\"javascript:;\" class=\"btn btn-xs btn-icon btn-circle btn-warning\" data-toggle=\"tooltip\" title=\"Minimise\" data-click=\"panel-collapse\"><i class=\"fa fa-minus\"></i></a>");
            if (deletable)
                panelButtons.InnerHtml.AppendHtml("<a href=\"javascript:;\" class=\"btn btn-xs btn-icon btn-circle btn-danger\" data-toggle=\"tooltip\" title=\"Close\" data-click=\"panel-remove\"><i class=\"fa fa-times\"></i></a>");

            panelHeading.InnerHtml.AppendHtml(panelTitle);
            panelHeading.InnerHtml.AppendHtml(panelButtons);

            htmlHelper.ViewContext.Writer.Write(panelHeading);
            htmlHelper.ViewContext.Writer.Write(@"<div class=""panel-body"">");

            return new DisposableHtmlHelper(htmlHelper.EndPanel);
        }

        public static void EndDiv(this IHtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("</div></div>");
        }

        public static void EndPanel(this IHtmlHelper htmlHelper)
        {
            htmlHelper.EndDiv();
        }
    }

    internal class DisposableHtmlHelper : IDisposable
    {
        private readonly Action _end;

        public DisposableHtmlHelper(Action end)
        {
            _end = end;
        }

        public void Dispose()
        {
            _end();
        }
    }
}
