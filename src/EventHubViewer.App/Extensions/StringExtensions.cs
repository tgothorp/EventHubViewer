using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EventHubViewer.App.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// https://stackoverflow.com/a/31404282/4991153 Probably a better way but this will do for now
        /// </summary>
        public static bool IsValidJson(this string @string)
        {
            try
            {
                JToken.Parse(@string);
                return true;
            }
            catch (JsonReaderException ex)
            {
                Trace.WriteLine(ex);
                return false;
            }
        }
    }
}