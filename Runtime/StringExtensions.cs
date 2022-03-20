using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Dutil
{
    public static class StringExtensions
    {
        public static bool MatchAny(this string value, params string[] strings)
        {
            return strings.ToList().Select(x => x.ToUpper()).Contains(value.ToUpper());
        }
        public static string Capitalise(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1).ToLower();
        }
        public static string CapitaliseAll(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return s.Split(' ').Select(x => x.Capitalise()).Aggregate((x, y) => x + " " + y);
        }
        public static string Remove(this string s, params string[] strings)
        {
            return strings.Aggregate(s, (x, y) => x.Replace(y, ""));
        }


    }
}