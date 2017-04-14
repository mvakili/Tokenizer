using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tokenizer
{
    public class TextAnalyzer
    {
        public Dictionary<string,Word> Words { get; private set; }
        public Dictionary<char, Word> Letters { get; private set; }

        private Encoding utf16Encoder = Encoding.GetEncoding("UTF-16", new EncoderReplacementFallback(string.Empty), new DecoderExceptionFallback());
        private List<string> Texts { get; set; }
        private List<string> StandardWords { get; set; }
        private static string Pattern => @"([0-9]+\.[0-9]+)([0-9]+)|([٪])|([۰-۹٠-٩]+\.[۰-۹٠-٩]+)|([۰-۹٠-٩]+)|([ًٌٍَُِّا-یكي]+)|([a-zA-z]+)|([[:punct:]])|(.)";

        public void Initialize(IEnumerable<string> texts, IEnumerable<string> standardWords)
        {

            Words = new Dictionary<string, Word>();
            Letters = new Dictionary<char, Word>();
            Texts = new List<string>();
            StandardWords = new List<string>();

            foreach (var word in standardWords)
            {
                StandardWords.Add(UnicodeToUTF16(word));
            }
            foreach (var text in texts)
            {
                Texts.Add(UnicodeToUTF16(text));
            }
        }

        private string UnicodeToUTF16(string input)
        {
            return utf16Encoder.GetString(utf16Encoder.GetBytes(input));
        }
        private bool IsStandardWord(string word)
        {
            return StandardWords.Any(u => u == word);
        }

        private List<string> GetSuggestions(string word)
        {
            return StandardWords.Where(u => CalculateEditDistance(word, u) < 3).ToList();
        }

        private static int CalculateEditDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        public void Run()
        {
            if (Words == null || Letters == null || Texts == null)
            {
                throw new NotInitializedException();
            }

            for (var i = 0; i < Texts.Count; i++)
            {
                foreach (Match m in Regex.Matches(Texts[i], Pattern))
                {
                    AddWord(m.Value, new Location { CharNumber = m.Index, Row = i, NormalizedValue = TextNormalizer.Normalizer.Normalize(m.Value)});
                    for (int j = 0; j < m.Value.Length; j++)
                    {
                        AddLetter(m.Value[j], new Location { CharNumber = m.Index + j, Row = i, NormalizedValue = TextNormalizer.Normalizer.Normalize(m.Value[j]) });
                    }
                }
            }
        }

        private void AddWord(string word, Location location)
        {
            if (!Words.ContainsKey(word))
            {
                Words[word] = new Word { Normalized = TextNormalizer.Normalizer.Normalize(word), Locations = new List<Location>() };
                Words[word].IsStandard = IsStandardWord(word);
                if (Words[word].IsStandard)
                {
                    Words[word].Suggestions = new List<string>();
                } else
                {
                    Words[word].Suggestions = GetSuggestions(word);
                }
            }
            Words[word].Locations.Add(location);
        }

        private void AddLetter(char letter, Location location)
        {
            if (!Letters.ContainsKey(letter))
            {
                Letters[letter] = new Word {Normalized = TextNormalizer.Normalizer.Normalize(letter), Locations = new List<Location>()};
            }
            Letters[letter].Locations.Add(location);
        }


    }
}
