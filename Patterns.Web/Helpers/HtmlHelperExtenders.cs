using System;
using System.Web.Mvc;
using Patterns.Resources;

namespace Patterns.Web.Helpers
{
    public static class HtmlHelperExtenders
    {
        public static String TimeAgo(this HtmlHelper helper, DateTime dateTime)
        {
            var result = helper.PastTime(dateTime);
            if (result != Strings.JustNow)
            {
                result += " " + Strings.Ago;
            }
            return result;
        }

        public static string PastTime(this HtmlHelper helper, DateTime dateTime)
        {
            String result;
            var timeSpan = DateTime.UtcNow - dateTime;
            if (timeSpan.TotalMinutes < 1)
            {
                result = Strings.JustNow;
            }
            else if (timeSpan.TotalMinutes < 60)
            {
                result = GetAgoString(timeSpan.TotalMinutes, Strings.Minutes1, Strings.Minutes2, Strings.Minutes3, Strings.Minutes4);
            }
            else if (timeSpan.TotalHours < 24)
            {
                result = GetAgoString(timeSpan.TotalHours, Strings.Hours1, Strings.Hours2, Strings.Hours3, Strings.Hours4);
            }
            else if (timeSpan.TotalDays < 7)
            {
                result = GetAgoString(timeSpan.TotalDays, Strings.Days1, Strings.Days2, Strings.Days3, Strings.Days4);
            }
            else
            {
                var totalWeeks = timeSpan.TotalDays/7;
                result = GetAgoString(totalWeeks, Strings.Weeks1, Strings.Weeks2, Strings.Weeks3, Strings.Weeks4);
            }
            return result;
        }

        private static String GetAgoString(double value, String unit1, String unit2, String unit3, String unit4)
        {
            var usedValue = (int) value;
            var usedUnit = GetUnit(usedValue, unit1, unit2, unit3, unit4);
            var template = "{0} {1}";
            var result = String.Format(template, (int) value, usedUnit);
            return result;
        }

        private static object GetUnit(int value, string unit1, string unit2, string unit3, string unit4)
        {
            if (value == 1)
            {
                return unit1;
            }
            if (value > 1 && value < 5)
            {
                return unit2;
            }
            if (value > 20 && value % 10 == 1)
            {
                return unit4;
            }
            if (value > 20 && value%10 > 1 && value%10 < 5)
            {
                return unit2;
            }
            return unit3;
        }
    }
}