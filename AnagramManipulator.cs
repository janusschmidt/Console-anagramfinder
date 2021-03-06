﻿using System.Collections.Generic;
using System.Linq;

namespace anagramfinderConsole
{
    
    public class AnagramManipulator 
    {
        readonly Dictionary<string, List<string>> alfabetiseretAnagramListe = new Dictionary<string, List<string>>();

        public string[] GetAlfabetisedWords
        {
            get { return alfabetiseretAnagramListe.Keys.ToArray(); }
        }

        public void LavAlfabetiseretAnagramListe(IEnumerable<string> wordlist, char[] allowedchars)
        {
            var calc = new CalcHelper();
            foreach (var ord in wordlist)
            {
                if (calc.CharsOk(ord, allowedchars) == null) continue;
                var alfabetiseretOrd =
                    new string(ord.ToCharArray().Where(c => c != '\'' && c != ' ').OrderBy(c => c).ToArray());
                if (alfabetiseretAnagramListe.ContainsKey(alfabetiseretOrd))
                {
                    if (!alfabetiseretAnagramListe[alfabetiseretOrd].Contains(ord))
                        alfabetiseretAnagramListe[alfabetiseretOrd].Add(ord);
                }
                else
                {
                    alfabetiseretAnagramListe.Add(alfabetiseretOrd, new List<string> { ord });
                }
            }
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
