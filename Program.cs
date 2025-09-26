using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("TEXT PROCESSOR");
        Console.Write("Nhap doan van ban: ");
        string input = Console.ReadLine() ?? string.Empty;
        string noDiacritics = RemoveDiacritics(input);

        string normalized = NormalizeText(noDiacritics);
        string capitalized = CapitalizeSentences(normalized);

        var words = ExtractWords(capitalized);
        int totalWords = words.Count;
        var freq = CountFrequency(words, StringComparer.CurrentCultureIgnoreCase);
        int distinctWords = freq.Count;

        Console.WriteLine("\nVan ban da chuan hoa");
        Console.WriteLine(capitalized);

        Console.WriteLine("\n--- Thong ke ---");
        Console.WriteLine($"Tong so tu: {totalWords}");
        Console.WriteLine($"So luong tu khac nhau: {distinctWords}");

        Console.WriteLine("\n Bang tan suat");
        foreach (var kv in freq
            .OrderByDescending(kv => kv.Value)
            .ThenBy(kv => kv.Key, StringComparer.CurrentCultureIgnoreCase))
        {
            Console.WriteLine($"{kv.Key} : {kv.Value}");
        }
    }

    static string RemoveDiacritics(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;
        string normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in normalized)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    static string NormalizeText(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;
        string oneSpace = Regex.Replace(text, @"\s+", " ");
        return oneSpace.Trim();
    }

    static string CapitalizeSentences(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;
        text = NormalizeText(text);
        return Regex.Replace(
            text,
            @"(^|[\.!\?]\s*)(\p{Ll})",
            m =>
            {
                string prefix = m.Groups[1].Value;
                string firstChar = m.Groups[2].Value.ToUpper(CultureInfo.CurrentCulture);
                return prefix + firstChar;
            });
    }

    static List<string> ExtractWords(string text)
    {
        var list = new List<string>();
        if (string.IsNullOrEmpty(text)) return list;
        var matches = Regex.Matches(text, @"[\p{L}\p{N}]+", RegexOptions.CultureInvariant);
        foreach (Match m in matches)
        {
            if (!string.IsNullOrWhiteSpace(m.Value))
                list.Add(m.Value);
        }
        return list;
    }

    static Dictionary<string, int> CountFrequency(List<string> words, IEqualityComparer<string> comparer)
    {
        var dict = new Dictionary<string, int>(comparer);
        foreach (var w in words)
        {
            string key = w.ToLower(CultureInfo.CurrentCulture);
            if (dict.ContainsKey(key)) dict[key]++;
            else dict[key] = 1;
        }
        return dict;
    }
}
