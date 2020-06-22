// [2016/5/31 14:33]

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Mohammad.Net;
using Mohammad.Threading.Tasks;

namespace Mohammad.Helpers
{
    public static class IpRangeHelper
    {
        public static IEnumerable<ResolveAndPingResult> ResolveAndPing(this IEnumerable<IpAddress> range) => range.Select(ip => ip.ResolveAndPing());

        public static ResolveAndPingResult[] FastResolveAndPing(this IEnumerable<IpAddress> range) => range.FastForEachFunc(ip => ip.ResolveAndPing());

        public static void FastResolveAndPing(this IEnumerable<IpAddress> range, Action<ResolveAndPingResult> onResolved)
        {
            range.FastForEachFunc(ip => ip.ResolveAndPing(), onResolved);
        }

        public static void FastResolveAndPing(this IEnumerable<IpAddress> range, Action<IpAddress, string> onSucceed)
        {
            range.FastForEachFunc(ip => ip.ResolveAndPing(),
                r =>
                {
                    if (r.PingStatus == IPStatus.Success)
                        onSucceed(r.Ip, r.MachineName);
                });
        }

        public static void FastResolveAndPing(this IEnumerable<IpAddress> range, Func<ResolveAndPingResult, bool> onResolved)
        {
            range.FastForEachBreak(ip => onResolved(ip.ResolveAndPing()));
        }
    }
}