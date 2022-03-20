using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Task_ParallelLibrary
{
     class Program
    {
       public static void Main(string[] args)
        {
            Console.WriteLine("Using Thread");
            string[] words = CreatWordArray(@"https://www.gutenberg.org/files/54700/54700-0.txt");

            #region.ParallelTasks

            Parallel.Invoke(() =>
            {
                Console.WriteLine("Begin First Task..");
                GetLongestWord(words);
            },
            ()=>
                           {
                               Console.WriteLine("Begin Second Task...");
                               GetMostCommonWords(words);
                           },
            ()=>
                            {
                                Console.WriteLine("Begin Third Task...");
                                GetCountForword(words, "sleep");
                            }
            );
            #endregion
        }

        private static void GetCountForword(string[] words, string v)
        {
            var findword = from word in words
                           where word.ToUpper().Contains(v.ToUpper())
                             select word;
            Console.WriteLine($@"Task 3 --- the word  ""{v}"" occurs {findword.Count()} times.");
        }

        private static void GetMostCommonWords(string[] words)
        {
            var frequencyOrder = from word in words
                                 where word.Length > 6
                                 group word by word into g
                                 orderby g.Count() descending
                                 select g.Key;
        }

        private  static string GetLongestWord(string[] words)
        {
            var longestWord = (from w in words
                               orderby w.Length descending
                               select w).First();
                               Console.WriteLine($"task 1 -  longest word is { longestWord}.");
            return longestWord;
        }
        static string[] CreatWordArray(string ur)
        {
            Console.WriteLine($"Retrieving from {ur}");
            string blog = new WebClient().DownloadString(ur);
            return blog .Split(
                new char[] { ' ' ,'\u000A', ',', ',', ';', ';', '-', '/'},
                StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
