using System.Security.Cryptography;
using System.Text;

namespace anagramfinderConsole
{
    public class CalcHelper
    {
        public string CalculateMd5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            foreach (byte t in hash)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString().ToLower();
        }

        public char[] CharsOk(string str, char[] okchars)
        {
            return CharsOk(str.ToCharArray(), okchars);
        }

        public char[] CharsOk(char[] charsToCheck, char[] okchars)
        {
            var okcharsToRemove = new bool[okchars.Length];

            if (charsToCheck.Length == 0)
            {
                return null;
            }

            int noOfRemovedOkChars = 0;
            foreach (var c in charsToCheck)
            {
                //if (c == '\'')
                //{
                //    continue;
                //}

                var charMatchInOkChars = false;
                for (var j = 0; j < okchars.Length; j++)
                {
                    if (!okcharsToRemove[j] && c == okchars[j])
                    {
                        okcharsToRemove[j] = true;
                        charMatchInOkChars = true;
                        noOfRemovedOkChars++;
                        break;
                    }
                }

                if (!charMatchInOkChars)
                {
                    return null;
                }
            }

            var okcharsOut = new char[okchars.Length - noOfRemovedOkChars];
            var index = 0;
            for (var i = 0; i < okchars.Length; i++)
            {
                if (!okcharsToRemove[i])
                {
                    okcharsOut[index++] = okchars[i];
                }
            }
            return okcharsOut;
        }

        //public List<char> CharsOk(char[] charsToCheck, List<char> okchars)
        //{
        //    var okcharstmp = okchars.ToList();

        //    if (charsToCheck.Length == 0)
        //    {
        //        return null;
        //    }

        //    foreach (var c in charsToCheck)
        //    {
        //        if (c == '\'')
        //        {
        //            continue;
        //        }

        //        if (okcharstmp.Contains(c))
        //        {
        //            okcharstmp.Remove(c);
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }

        //    return okcharstmp;
        //}
    }
}
