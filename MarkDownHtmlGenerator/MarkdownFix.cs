using System;
using MarkdownDeep;

namespace MarkDownHtmlGenerator
{

    /// <summary>
    /// Fixes for buggy OnQualifyUrl implementation. Should be removed once
    /// fixed by the referenced library.
    /// (https://github.com/toptensoftware/markdowndeep/pull/43)
    /// </summary>
    public class MarkdownFix : Markdown
    {
        public override string OnQualifyUrl(string url)
        {
            if (QualifyUrl != null)
            {
                var q = QualifyUrl(url);
                if (q != null)
                    return q;
            }

            // Is the url a fragment?
            if (url.StartsWith("#"))
                return url;

            // Is the url already fully qualified?
            if (url.Contains("://") || url.StartsWith("mailto:"))
                return url;

            if (url.StartsWith("/"))
            {
                if (!string.IsNullOrEmpty(UrlRootLocation))
                {
                    return UrlRootLocation + url;
                }

                // Quit if we don't have a base location
                if (string.IsNullOrEmpty(UrlBaseLocation))
                    return url;

                // Need to find domain root
                int pos = UrlBaseLocation.IndexOf("://");
                if (pos == -1)
                    pos = 0;
                else
                    pos += 3;

                // Find the first slash after the protocol separator
                pos = UrlBaseLocation.IndexOf('/', pos);

                // Get the domain name
                string strDomain = pos < 0 ? UrlBaseLocation : UrlBaseLocation.Substring(0, pos);

                // Join em
                return strDomain + url;
            }
            else
            {
                // Quit if we don't have a base location
                if (string.IsNullOrEmpty(UrlBaseLocation))
                    return url;

                if (!UrlBaseLocation.EndsWith("/"))
                    return UrlBaseLocation + "/" + url;
                else
                    return UrlBaseLocation + url;
            }
        }
    }
}
