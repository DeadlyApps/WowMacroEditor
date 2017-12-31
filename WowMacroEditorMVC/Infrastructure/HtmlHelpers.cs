using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace WowMacroEditorMVC.Infrastructure
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            string currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            string currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            if (actionName == currentAction && controllerName == currentController)
            {
                return htmlHelper.ActionLink(linkText, actionName, controllerName, null, new { @class = "selected bigButton" });
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName);
        }
        
        public static MvcHtmlString UserTag(this HtmlHelper htmlHelper, Guid userID)
        {
            return htmlHelper.Action("Tag", "Profile", new { userID = userID });
        }

        public static MvcHtmlString ProfileLink(this HtmlHelper htmlHelper, int userIntID, string displayName, string linkInnerHtml = null)
        {
            string htmlContent = string.Format("<a href='/User/{0}/{1}'>{2}</a>",
                userIntID,
                displayName.ToFriendlyUrl(),
                linkInnerHtml ?? displayName);
            MvcHtmlString html = new MvcHtmlString(htmlContent);

            return html;
        }


        public static MvcHtmlString PagerLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, Boolean selected)
        {
            if (selected)
            {
                return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, new { @class = "selected" });
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues);
        }

        public static MvcHtmlString ProfileImage(this HtmlHelper htmlHelper, string gravatarID, int size = 80)
        {
            string url = GravatarUtility.GetGravatarUrl(gravatarID, size);
            string imageHtml = string.Format("<img src=\"{0}\" width=\"{1}\" height=\"{1}\" />", url, size);
            
            return new MvcHtmlString(imageHtml);
        }



        
    }

}
