using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Findkaninen;
using Findkaninen.Containers;

namespace anagramfinderConsole
{
    public class AnagramSolver
    {
        public readonly ConcurrentStack<Anagram> Anagrammer = new ConcurrentStack<Anagram>();
        readonly public int NoOfCharsInAnagram;
        readonly public char[] Allowedchars;
        readonly public AnagramManipulator Manipulator = new AnagramManipulator();
        readonly CalcHelper helper = new CalcHelper();
        readonly LapTimer lapTimer = new LapTimer();

        public AnagramSolver(string phrase)
        {
            Allowedchars = phrase.Replace(" ", string.Empty).ToCharArray();
            NoOfCharsInAnagram = Allowedchars.Length;
        }

        public void Start()
        {
            lapTimer.Start();

            string[] dict = File.ReadAllLines("../../wordlist.txt");
            lapTimer.Lap("File read in: {0} ms");
            Manipulator.LavAlfabetiseretAnagramListe(dict, Allowedchars);
            string[] keys = Manipulator.GetAlfabetisedWords;
            var lap1 = lapTimer.Lap("Time to compute alfabetized dictionary: {0} ms");

            var indexes = Enumerable.Range(0, keys.Length);
          
            indexes.AsParallel().ForAll(i => GetAnagramsIteration(i, keys, 3, new List<string>(), Allowedchars, 0));
            //GetAnagramsLoop(0, keys, 3, new List<string>(), Allowedchars, 0);

            lapTimer.AppendFormat("Time to compute ALL {0} anagrams: {1} ms", Anagrammer.Count(), lapTimer.ElapsedMilliseconds - lap1);
            lapTimer.AppendLine();

            lapTimer.Total("Total time: {0} ms");
        }

        public void GetAnagramsLoop(int dictStartPos, string[] dictKeys, int worddepth, List<string> ordDerSkalVæreMedIAnagram, char[] tmpOkChars, int ordDerSkalVæreMedLength)
        {
            for (var i = dictStartPos; i < dictKeys.Length; i++)
            {
                GetAnagramsIteration(i, dictKeys, worddepth, ordDerSkalVæreMedIAnagram, tmpOkChars, ordDerSkalVæreMedLength);
            }
        }

        public void GetAnagramsIteration(int i, string[] dictKeys, int worddepth, List<string> ordDerSkalVæreMedIAnagram, char[] tmpOkChars, int ordDerSkalVæreMedLength)
        {
            var ord = dictKeys[i];
            var anagramLength = ordDerSkalVæreMedLength + ord.Length;

            //tjek om det er et anagram ellers
            //forsøg at kombinere med flere ord via rekursion, hvis de ord der forsøges med nu ikke er for lange
            if ((worddepth == 1 && anagramLength == NoOfCharsInAnagram) ||
                (worddepth > 1 && anagramLength <= NoOfCharsInAnagram))
            {
                var anagram = new List<string>(ordDerSkalVæreMedIAnagram) { ord };

                var newtmpOkChars = helper.CharsOk(ord, tmpOkChars);
                if (newtmpOkChars != null)
                {
                    if (anagramLength == NoOfCharsInAnagram)
                    {
                        IEnumerable<IEnumerable<string>> permutationer = Manipulator.Permute(anagram);
                        var anagrams = permutationer.SelectMany(Manipulator.ConvertAlfabetizedAnagramToRealAnagrams).ToArray();
                        CheckAnagramMd5(anagrams);
                    }
                    else if (worddepth > 1)
                    {
                        GetAnagramsLoop(i, dictKeys, worddepth - 1, anagram, newtmpOkChars, anagramLength);
                    }
                }
            }
        }

        private void CheckAnagramMd5(IEnumerable<IEnumerable<string>> angramsArr)
        {
            var anagramsAsStrings = angramsArr.Select(a => string.Join(" ", a));
            var angramsAndMd5 = anagramsAsStrings.Select(a => new Anagram { Text = a, Md5 = helper.CalculateMd5Hash(a) }).ToArray();
            Anagrammer.PushRange(angramsAndMd5);

            foreach (var anagram in angramsAndMd5)
            {
                if (anagram.Md5 == "4624d200580677270a54ccff86b9610e")
                {
                    lapTimer.AppendFormat("Total time to find anagram \"{0}\": {1} ms", anagram.Text, lapTimer.ElapsedMilliseconds);
                    lapTimer.AppendLine();
                }
            }
        }
    }
}