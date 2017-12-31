using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Web.Routing;
using WowMacroEditorMVC.Models;

namespace WowMacroEditorMVC.Controllers
{
    [RequireHttp]
    public class HomeController : Controller
    {

        public ActionResult SiteMap()
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XElement root = new XElement(xmlns + "urlset");

            //some kind of foreach here to get the loc variable for all URLs in the site
            //for each URL in the collection, add it to the root element as here
            WowMacroEditorEntities db = new WowMacroEditorEntities();

            List<string> urlList = new List<string>();

            urlList.Add(GetUrl(new { controller = "Macros", action = "Browse"}));
            urlList.Add(GetUrl(new { controller = "Macros", action = "Edit" }));
            
            var macros = from x in db.Macroes
                        select new
                            {
                                x.Title,
                                x.MacroID
                            };
            foreach(var value in macros)
                urlList.Add(GetUrl(new { controller = "Macros", action = "Details", id = value.MacroID, MacroString = value.Title.ToFriendlyUrl() }));

            foreach (var profile in db.UserProfiles)
                urlList.Add(GetUrl(new { controller="Profile", action = "Index", userIntID = profile.UserIntID, profileName = profile.DisplayName.ToFriendlyUrl()}));


            foreach(var url in urlList)
            root.Add(
                new XElement(xmlns + "url",
                    new XElement(xmlns + "loc", url)));

            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(ms, Encoding.UTF8))
                {
                    root.Save(writer);
                }

                return Content(Encoding.UTF8.GetString(ms.ToArray()), "text/xml", Encoding.UTF8);
            }
        }

        protected string GetUrl(object routeValues)
        {
            RouteValueDictionary values = new RouteValueDictionary(routeValues);
            RequestContext context = new RequestContext(HttpContext, RouteData);

            string url = RouteTable.Routes.GetVirtualPath(context, values).VirtualPath;

            return new Uri(Request.Url, url).AbsoluteUri;
        }
    }
}
