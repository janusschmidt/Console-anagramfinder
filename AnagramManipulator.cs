using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace anagramfinderConsole
{
    public class AnagramManipulator 
    {
        readonly ConcurrentDictionary<string, List<string>> alfabetiseretAnagramListe = new ConcurrentDictionary<string, List<string>>();

        public string[] GetAlfabetisedWords
        {
            get { return alfabetiseretAnagramListe.Keys.OrderByDescending(k=>k.Length).ToArray(); }
        }

        public void LavAlfabetiseretAnagramListe(string[] wordlist, char[] allowedchars)
        {
            var calc = new CalcHelper();
            alfabetiseretAnagramListe.TryAdd("a", new List<string> { "a" });
            alfabetiseretAnagramListe.TryAdd("i", new List<string> { "i" });
            var indexes = Enumerable.Range(0, wordlist.Length);
            indexes.AsParallel().ForAll(i =>
            {
                var ord = wordlist[i];
                if (calc.CharsOk(ord, allowedchars) != null && ord.Length > 1)
                {
                    var alfabetiseretOrd = new string(ord.ToLower().ToCharArray().OrderBy(c => c).ToArray());

                    if (!alfabetiseretAnagramListe.TryAdd(alfabetiseretOrd, new List<string> {ord}))
                    {
                        if (!alfabetiseretAnagramListe[alfabetiseretOrd].Contains(ord))
                            alfabetiseretAnagramListe[alfabetiseretOrd].Add(ord);
                    }
                }
            });
        }

        public IEnumerable<IEnumerable<string>> ConvertAlfabetizedAnagramToRealAnagrams(IEnumerable<string> anagramIn)
        {
            var anagram = new Stack<string>(anagramIn);

            if (anagram.Count > 0)
            {
                var word = anagram.Pop();
                var alphabetizisedEquivalents = alfabetiseretAnagramListe[word];
                if (anagram.Count == 0)
                {
                    foreach (var w in alphabetizisedEquivalents)
                    {
                        yield return new[] { w };
                    }
                }
                else
                {
                    foreach (var subAnagrams in ConvertAlfabetizedAnagramToRealAnagrams(anagram))
                    {
                        var subAnagramsArr = subAnagrams.ToArray();
                        foreach (var w in alphabetizisedEquivalents)
                        {
                            yield return subAnagramsArr.Concat(new[] { w });
                        }
                    }
                }
            }
        }

        public IEnumerable<List<string>> Permute(List<string> anagram)
        {
            if (anagram.Count == 1)
            {
                yield return new List<string>(anagram);
            }
            else
            {
                foreach (var word in anagram)
                {
                    var subanagram = new List<string>(anagram);
                    subanagram.Remove(word);
                    foreach (var subperm in Permute(subanagram))
                    {
                        subperm.Add(word);
                        yield return subperm;
                    }
                }
            }
        }
    }
}
