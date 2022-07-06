using System.Text.RegularExpressions;

string? str = null;

Regex rx = new Regex(@"^[\s\-\d]+$"); // fix for negative number
MatchCollection mc;

do {
    do {
        Console.Write("Sequence? ");
        str = "0 -1 3 2";
        str = Console.ReadLine();
    } while(str == null ? false : !rx.Match(str).Success);

    str = " " + str + " ";
    rx = new Regex(@"\s+([\-\d]+)"); // fix for negative number
    mc = rx.Matches(str); // fix a tupo
} while(mc.Count == 0);

int min = (int) (new int[mc.Count])
                .Select((x, i) =>
                            int.Parse(mc[i].Value))
                .Min();

Console.WriteLine($"Minimal element is {min}, have a nice day!", min);