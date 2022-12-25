using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSharpEssentials
{
    internal class RegularExp
    {
        public void RegularExpfn()
        {
            string testStr = "Hello, How Are You?";
            string testStr1 = "hello, how are you?";

            Regex CapWord = new(@"[A-Z]\w+");

            //@symbol outside the string means the string literial content. 
            //Hey .NET if you ever see something like a back slash inside the string, I'm not escaping something that's a real backslash in there, and that's how you should treat it.

            //So I'm going to look for a regular expression where any characters in the range of A to Z are followed by a non-white space word character, one or more of those

            //The isMatch functio is used to determine if the content of a string 

            //var result  =  CapWord.IsMatch(testStr);
            //var result1 =  CapWord.IsMatch(testStr1);
            //Console.WriteLine($"{testStr} - Statment having caps character? {result}");
            //Console.WriteLine($"{testStr1} - Statment having caps character? {result1}");

            //var result2 = CapWord.Match(testStr); //Match method returns a single match at a time


            //while (result2.Success)
            //{
            //    Console.WriteLine($"{result2.Value} found at position: {result2.Index}");
            //    result2 = result2.NextMatch();
            //}

            //var mc = CapWord.Matches(testStr);
            //Console.WriteLine($" No of mactched words:  {mc.Count}");
            //foreach (Match match in mc)
            //{
            //    Console.WriteLine($"{match.Value} found at position: {match.Index}");
            //}

            ////REPLACE

            //var replacedStr = CapWord.Replace(testStr, "***");
            //Console.WriteLine($"Replaced string {replacedStr}");

            string MakeUpper(Match m)
            {
                string s = m.ToString();

                return s.ToUpper();
            }

            string upperStr = CapWord.Replace(testStr, new MatchEvaluator(MakeUpper));
            Console.WriteLine($"Replaced content on the fly: {upperStr}");

        }
    }
}
