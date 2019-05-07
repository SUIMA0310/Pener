using System;
using System.Collections.Generic;
using System.Text;

namespace Pener.Utils
{
    public static class UnixTimeExpansion
    {
        private static DateTime BASE_TIME = new DateTime(1970, 1, 1);

        public static DateTime FromUnixTime(this int unixTime)
        {
            return BASE_TIME.AddSeconds(unixTime).ToLocalTime();
        }

        public static int ToUnixTime(this DateTime time)
        {
            return (int)(time.ToUniversalTime() - BASE_TIME).TotalSeconds;
        }

    }
}
