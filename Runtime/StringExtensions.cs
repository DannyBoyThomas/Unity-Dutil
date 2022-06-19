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

        /// <summary>
        /// Converts to Rich Text with italics
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string I(this string text)
        {
            return $"<i>{text}</i>";
        }
        /// <summary>
        /// Converts to rich text with bold
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string B(this string text)
        {
            return $"<b>{text}</b>";
        }
        /// <summary>
        /// Converts to rich text with a custom size
        /// </summary>
        /// <param name="text"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string Size(this string text, int size)
        {
            return $"<size={size}>{text}</size>";
        }
        /// <summary>
        /// Converts to rich text with a custom colour
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string Color(this string text, Color color)
        {
            string colorString = color.ToHex();
            string res = $"<color={colorString}>{text}</color>";
            return res;
        }

    }
}