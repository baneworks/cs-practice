#define __test__

global using System.Collections;
global using System.Collections.Generic;
global using System.Text.RegularExpressions;
global using static System.Console;

namespace task_08_3;

#if __test__
struct Tests
{
    static public readonly int[] testArr = new[] {10, 12 ,13, 14, 15, 12, 155, 11, 23, 45, 10};
}
#endif

class Program
{
    static void Main(string[] args)
    {
        Write("Number? ");

        #if __test__
        WriteLine();
        int idx = 0;
        #endif

        string? strCmd = null;

        Regex rg = new Regex(@"^\s*(([\-\d]+)|(?>exit))s*$");
        Match m;

        bool exit = false;

        HashSet<int> hash = new();

        do {
            do {
                #if __test__
                    if (idx >= Tests.testArr.Count())
                        strCmd = "exit";
                    else
                        strCmd = Tests.testArr[idx].ToString();
                    idx += 1;
                #else
                    do
                        strCmd = ReadLine();
                    while (strCmd == null);
                #endif
                m = rg.Match(strCmd);
            } while(!m.Success);

            if (m.Groups[1].Value != "exit") {
                int num = int.Parse(m.Groups[1].Value);
                if (hash.Add(num))
                    WriteLine($"added new: {num}");
                else
                    WriteLine($"not added (already in set): {num}");
            }
            else
                exit = true;

        } while(!exit);
    }
}
