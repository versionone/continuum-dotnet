using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContinuumDotNet.Utilities.Extensions
{
    public static class CommonExtensions
    {
        public static bool IsValidUrl(this string urlToTest)
        {
            return !string.IsNullOrEmpty(urlToTest) && Uri.IsWellFormedUriString(urlToTest, UriKind.RelativeOrAbsolute);
        }
    }
}
