using System.Text.RegularExpressions;

int? ConsoleReadNumber(string greetings, string? alter = null)
{
    string? str = null;
    string pattern = @"^\s*(\d+)";
    pattern += alter != null ? @"|(" + alter + ")" : "";
    pattern += @"\s*$";
    Regex rx = new Regex(pattern);
    Match? m = null;

    do {
        Console.Write(greetings);
        str = Console.ReadLine();
        m = str != null ? rx.Match(str) : null;
    } while(!m?.Success ?? true);

    return m!.Value == alter ? null : int.Parse(m!.Value);
}

int secret = 0;
do
    secret = ConsoleReadNumber("How much is a fish? ") ?? 0;
while (secret < 1);

Random rnd = new Random();
secret = rnd.Next(secret);

(bool victory, bool giveup, bool greater) result;

do {

    int? guess = ConsoleReadNumber("Guess? ", "stop");

    result =  guess switch
    {
        null => (false, true, false),
        >= 0 when guess == secret => (true, false, false),
        >= 0 when guess > secret => (false, false, true),
        _ => (false, false, false),
    };

    if (result.giveup)
    {
        Console.WriteLine("Are you giving up? I'm disappointed. Bye (");
        break;
    }

    if (result.victory)
    {
        Console.WriteLine("Grand victoria! Bye");
        break;
    }

    if (result.greater)
        Console.WriteLine("Too much!");
    else
        Console.WriteLine("Too little!");

} while (!result.giveup || !result.victory);