using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextNormalizer
{
    public static class Normalizer
    {
        private static readonly Dictionary<char, string> Translations = new Dictionary<char, string>()
        {
            { 'ك', "ک" },
            { 'ﻻ', "لا" },
            { 'ي', "ی" },
            { 'َ', ""},//shift + A
            { 'ُ', ""},//shift + S
            { 'ِ', ""},//shift + D
            { 'ّ', ""},//shift + F
            { 'ً', ""},//shift + Q
            { 'ٌ', ""},//shift + W
            { 'ٍ', ""},//shift + E
            { 'ـ', ""},//shift + J
            { '٠', "۰" },
            { '١', "۱" },
            { '٢', "۲" },
            { '٣', "۳" },
            { '٤', "۴" },
            { '٥', "۵" },
            { '٦', "۶" },
            { '٧', "۷" },
            { '٨', "۸" },
            { '٩', "۹" },


        };

        public static string Normalize(char input)
        {
            return Translations.ContainsKey(input) ? Translations[input] : input.ToString();
        }

        public static string Normalize(string input)
        {
            var sb = new StringBuilder(input.Length);
            foreach (var ch in input)
            {
                if (Translations.ContainsKey(ch)) sb.Append(Translations[ch]);
                else sb.Append(ch);
            }
            return sb.ToString();
        }

        public static string[] Normalize(IEnumerable<string> input)
        {
            return input.Select(Normalize).ToArray();
        }
    }
}
