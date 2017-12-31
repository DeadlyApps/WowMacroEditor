using System.Web.Mvc;
using System.Text;

public static class UrlEncoder
{
    public static string ToFriendlyUrl(this string urlToEncode)
    {
        urlToEncode = (urlToEncode ?? "").Trim().ToLower();

        StringBuilder url = new StringBuilder();

        foreach (char ch in urlToEncode)
        {
            switch (ch)
            {
                case ' ':
                    url.Append('-');
                    break;
                case '&':
                case '+':
                    url.Append("and");
                    break;
                case '\'':
                    break;
                default:
                    if ((ch >= '0' && ch <= '9') ||
                        (ch >= 'a' && ch <= 'z'))
                    {
                        url.Append(ch);
                    }
                    else
                    {
                        url.Append('-');
                    }
                    break;
            }
        }

        return url.ToString();
    }
}