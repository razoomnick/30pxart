using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualBasic.FileIO;

namespace Scorm.Core.IpLookup
{
    public class GeoIpLookup
    {
        private static IpRange[] ranges;
        private RangeStartComparer rangeStartComparer = new RangeStartComparer();

        static GeoIpLookup()
        {
            var rangesList = new List<IpRange>();
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("Scorm.Core.IpLookup.GeoIPCountryWhois.csv"))
            {
                using (var parser = new TextFieldParser(stream))
                {
                    parser.HasFieldsEnclosedInQuotes = true;
                    parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        var fields = parser.ReadFields();
                        var rangeStart = long.Parse(fields[0]);
                        var rangeEnd = long.Parse(fields[1]);
                        var country = fields[2];
                        var range = new IpRange(rangeStart, rangeEnd, country);
                        rangesList.Add(range);
                    }
                }
            }
            ranges = rangesList.ToArray();
            Array.Sort(ranges, (x, y) => x.RangeStart.CompareTo(y.RangeStart));
        }

        public String GetCountryByIp(String ip)
        {
            String result = "";
            var longIp = ConvertIpStringToLong(ip);
            if (longIp > 0)
            {
                var rangeToSearch = new IpRange(longIp, longIp, "");
                var index = Array.BinarySearch(ranges, rangeToSearch, rangeStartComparer);
                if (index < 0)
                {
                    index = ~index - 1;
                }
                if (longIp >= ranges[index].RangeStart && longIp <= ranges[index].RangeEnd)
                {
                    result = ranges[index].Country;
                }
            }
            return result;
        }

        private static long ConvertIpStringToLong(string ip)
        {
            var chunks = ip.Split(new[] {'.'});
            var longIp = -1L;
            if (chunks.Length == 4)
            {
                longIp = long.Parse(chunks[0])*256*256*256 +
                         long.Parse(chunks[1])*256*256 +
                         long.Parse(chunks[2])*256 +
                         long.Parse(chunks[3]);
            }
            return longIp;
        }

        private class RangeStartComparer : IComparer<IpRange>
        {
            public int Compare(IpRange x, IpRange y)
            {
                return x.RangeStart.CompareTo(y.RangeStart);
            }
        }

        private class IpRange
        {
            public IpRange(long rangeStart, long rangeEnd, String country)
            {
                RangeStart = rangeStart;
                RangeEnd = rangeEnd;
                Country = country;
            }

            public long RangeStart { get; private set; }
            public long RangeEnd { get; private set; }
            public String Country { get; private set; }
        }
    }
}
