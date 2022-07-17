#define __test__

global using System.Collections;
global using System.Collections.Generic;
global using System.Text.RegularExpressions;
global using static System.Console;

using CmdLets;

#if __test__
using task_08_2_tests;
#endif

namespace task_08_2;

class Program
{
    static void Main(string[] args)
    {
        CmdSet cmdSet = new CmdSet
        {
            new CmdLet ("add", new PhoneNum(), new PersonName()),
            new CmdLet ("ls"),
            new CmdLet ("rm", new PhoneNum()),
            new CmdLet ("find", new PhoneNum()),
            new CmdLet ("exit"),
        };

        Dictionary<PhoneNum, string> db = new();

        Write("Command [/ls /add /rm /find <phonenum> <full name>]? ");

        string? strCmd = null;

        #if __test__
        WriteLine();
        int idx = 0;
        #endif

        do {
            do {
                #if __test__
                    strCmd = Tests.CmdSeq[idx];
                    idx += 1;
                #else
                    do
                        strCmd = ReadLine();
                    while (strCmd == null);
                #endif
            } while(!cmdSet.Parse(strCmd));

            CmdLet cmd = cmdSet.Match!.Value;

            switch(cmd.Cmd)
            {
                case "add":
                    WriteLine(String.Join(' ', '/' + cmd.Cmd, cmd[0], cmd[1]));
                    db.Add((PhoneNum)cmd["PhoneNum"]!, cmd["PersonName"]?.ToString()!);
                    break;
                case "rm":
                    WriteLine(String.Join(' ', '/' + cmd.Cmd, cmd[0]));
                    if (db.ContainsKey((PhoneNum)cmd["PhoneNum"]!))
                    {
                        if (db.Remove((PhoneNum)cmd["PhoneNum"]!))
                            WriteLine($"Removed: {(PhoneNum)cmd["PhoneNum"]!}");
                        else
                            WriteLine($"Failed to remove: {(PhoneNum)cmd["PhoneNum"]!}");
                    }
                    else
                        WriteLine($"Not found: {(PhoneNum)cmd["PhoneNum"]!}");
                    break;
                case "ls":
                    WriteLine(String.Join(' ', '/' + cmd.Cmd));
                    foreach (var k in db.Keys)
                        WriteLine(k.ToString() + ": " + db[k]);
                    break;
                case "find":
                    WriteLine(String.Join(' ', '/' + cmd.Cmd, cmd[0]));
                    if (db.ContainsKey((PhoneNum)cmd["PhoneNum"]!))
                        WriteLine($"Found: {(PhoneNum)cmd["PhoneNum"]!} => {db[(PhoneNum)cmd["PhoneNum"]!]?.ToString()}");
                    else
                        WriteLine($"Not found: {(PhoneNum)cmd["PhoneNum"]!}");
                    break;
                // case _:
                    // break;
            }

        } while(cmdSet.Match?.Cmd != "exit");

        WriteLine($"Records in dictionary: {db.Count}");
    }
}