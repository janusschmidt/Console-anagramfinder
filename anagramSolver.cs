//using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using anagramfinderConsole.Containers;

namespace anagramfinderConsole
{
    public class AnagramSolver
    {
        //readonly ConcurrentStack<Anagram> anagrammer = new ConcurrentStack<Anagram>();
        int anagramCounter;
        readonly int noOfCharsInAnagram;
        readonly char[] allowedchars;
        readonly AnagramManipulator manipulator = new AnagramManipulator();
        readonly CalcHelper helper = new CalcHelper();
        readonly LapTimer lapTimer = new LapTimer();
        
        public AnagramSolver(string phrase)
        {
            allowedchars = phrase.Replace(" ", string.Empty).ToCharArray();
            noOfCharsInAnagram = allowedchars.Length;
        }

        public long Start()
        {
            lapTimer.Start();

            string[] dict = File.ReadAllLines("../../wordlist.txt");
            var lap0 = lapTimer.Lap("File read in: {0} ms");
            manipulator.LavAlfabetiseretAnagramListe(dict, allowedchars);
            string[] keys = manipulator.GetAlfabetisedWords;
            var lap1 = lapTimer.Lap("Time to compute alfabetized dictionary: {0} ms");

            GetAnagrams3Foreach(keys);

            lapTimer.AppendFormat("Time to compute ALL {0} anagrams: {1} ms", anagramCounter, lapTimer.ElapsedMilliseconds - (lap1 + lap0));
            lapTimer.AppendLine();

            lapTimer.Total("Total time: {0} ms");
            return lapTimer.ElapsedMilliseconds;
        }

        void GetAnagrams3Foreach(string[] dictKeys)
        {
            int dictkeyslength = dictKeys.Length;

            //var ord = keys[0];
            var indexes = Enumerable.Range(0, dictkeyslength);
            indexes.AsParallel().ForAll(level3Index =>
                //for (var level3Index = 0; level3Index < dictkeyslength; level3Index++)
            {
                var level3Ord = dictKeys[level3Index];
                int level3CurrentKeyLength = level3Ord.Length;
                var level3CurrentCharsOk = helper.CharsOk(level3Ord, allowedchars);

                for (var level2Index = level3Index; level2Index < dictkeyslength; level2Index++)
                {
                    var level2Ord = dictKeys[level2Index];
                    var level2CurrentKeyLength = level3CurrentKeyLength + level2Ord.Length;
                    if (level2CurrentKeyLength > noOfCharsInAnagram)
                    {
                        continue;
                    }

                    //hvis level3 + 2*level2 er for kort er vi færdige med at lede.
                    if (level2CurrentKeyLength + level2Ord.Length < noOfCharsInAnagram)
                    {
                        break;
                    }

                    var level2CurrentCharsOk = helper.CharsOk(level2Ord, level3CurrentCharsOk);
                    if (level2CurrentCharsOk == null)
                    {
                        continue;
                    }

                    if (level2CurrentKeyLength == noOfCharsInAnagram)
                    {
                        AnagramFound(new List<string> {level3Ord, level2Ord});
                        continue;
                    }

                    for (var level1Index = level2Index; level1Index < dictkeyslength; level1Index++)
                    {
                        var level1Ord = dictKeys[level1Index];
                        var level1CurrentKeyLength = level2CurrentKeyLength + level1Ord.Length;
                        if (level1CurrentKeyLength > noOfCharsInAnagram)
                        {
                            continue;
                        }

                        if (level1CurrentKeyLength < noOfCharsInAnagram)
                        {
                            break;
                        }

                        if (helper.CharsOk(level1Ord, level2CurrentCharsOk) == null)
                        {
                            continue;
                        }

                        AnagramFound(new List<string> {level3Ord, level2Ord, level1Ord});
                        break;
                    }
                }
            });
        }

        void AnagramFound(List<string> anagram)
        {
            var permutationer = manipulator.Permute(anagram);
            var anagrams = permutationer.SelectMany(manipulator.ConvertAlfabetizedAnagramToRealAnagrams).ToArray();
            CheckAnagramMd5(anagrams);
        }

        void CheckAnagramMd5(IEnumerable<IEnumerable<string>> angramsArr)
        {
            var anagramsAsStrings = angramsArr.Select(a => string.Join(" ", a));
            var angramsAndMd5 = anagramsAsStrings.Select(a => new Anagram { Text = a, Md5 = helper.CalculateMd5Hash(a) }).ToArray();
            anagramCounter += angramsAndMd5.Length;
            //anagrammer.PushRange(angramsAndMd5);

            foreach (var anagram in angramsAndMd5)
            {
                if (anagram.Md5 == "4624d200580677270a54ccff86b9610e")
                {
                    lapTimer.Lap("Time to find anagram \"" + anagram.Text + "\": {0} ms");
                    lapTimer.AppendFormat("Total time to find anagram \"{0}\": {1} ms", anagram.Text, lapTimer.ElapsedMilliseconds);
                    lapTimer.AppendLine();
                }
            }
        }
    }
}