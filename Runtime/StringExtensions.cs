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
    }
}