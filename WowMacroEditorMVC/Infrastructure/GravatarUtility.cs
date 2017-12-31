using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WowMacroEditorMVC.Infrastructure
{
    public static class GravatarUtility
    {
        public static string GetGravatarUrl(string gravatarID, int size)
        {
            string hashOfGravatarID = AuthUtil.GetMd5Hash(gravatarID);
            string url = string.Format("http://www.gravatar.com/avatar/{0}?d=retro&r=r&s={1}", hashOfGravatarID, size);
            return url;
        }

    }
}