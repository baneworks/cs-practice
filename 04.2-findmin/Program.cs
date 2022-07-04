using System.Text.RegularExpressions;

string? str = null;
Regex rx = new Regex(@"^[\s\d]+$");
MatchCollection mc;

do {
    do {
        Console.Write("Sequence? ");
        str = Console.ReadLine();
    } while(str == null ? false : !rx.Match(str).Success);

    str = " " + str + " ";
    rx = new Regex(@"\s+(\d+)");
    mc = rx.Matches(str, 2);
} while(mc.Count == 0);

int min = (int) (new int[mc.Count])
                .Select((x, i) =>
                            int.Parse(mc[i].Value))
                .Min();

Console.WriteLine($"Minimal element is {min}, have a nice day!", min);