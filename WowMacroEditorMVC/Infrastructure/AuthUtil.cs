using System;
using System.Linq;
using System.Web.Security;
using System.Web;
using WowMacroEditorMVC.Models;
using System.Security.Cryptography;
using System.Text;

public static class AuthUtil
{
    public static CompleteUser GetUser()
    {
        WowMacroEditorEntities db = new WowMacroEditorEntities();

        return db.GetCompleteUser(GetUserID().Value);
    }

    public static Guid? GetUserID()
    {
        MembershipUser User = Membership.GetUser();
        if (User == null)
        {
            WowMacroEditorEntities db = new WowMacroEditorEntities();
            String claimedIdentifier = HttpContext.Current.User.Identity.Name;
            if (db.OpenIDs.Count(x => String.Compare(x.OpenIdClaimedIdentifier, claimedIdentifier, false) == 0) > 0)
                return db.OpenIDs.Single(x => String.Compare(x.OpenIdClaimedIdentifier, claimedIdentifier, false) == 0).UserID;
        }
        else
        {
            if (User.ProviderUserKey != null)
                return (Guid)User.ProviderUserKey;
        }
        return null;
    }

    public static string GetMd5Hash(string input)
    {
        input = input.ToLower();
        MD5 md5Hash = MD5.Create();
        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }
}