#define __test__

using System;

namespace Task5;

class Program
{
    #region methods

    static string[] Split(string str) => str.Split(' ');

    static void PrintWords(string[] words)
    {
        foreach (var word in words)
            Console.WriteLine(word);
    }

    static string[] Sort(string[] words)
    {
        string[] sorted = (string[]) words.Clone();
        Array.Sort(sorted);
        return sorted;
    }

    static string[] Sort(string[] words, bool reverseOrder)
    {
        string[] sorted = (string[]) words.Clone();
        if (reverseOrder)
            Array.Reverse(sorted);
        else
            Array.Sort(sorted);
        return sorted;
    }

    #endregion

    static void Main(string[] args)
    {
        // data input
        string? str = null;
        #if __test__
            str = "The quick brown fox jumps over the lazy dog";
        #else
            do
                str = Console.ReadLine();
            while (str == null);
        #endif

        // task 5.1
        string[] res = Split(str.ToLower());
        Console.WriteLine("Sentence's words, order as is:");
        PrintWords(res);

        Console.WriteLine();

        // task 5.2
        res = Sort(res, true);
        Console.WriteLine("Sentence's words in descending order:");
        PrintWords(res);
    }
}