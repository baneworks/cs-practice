using static System.Console;
using System.Collections.Generic;

namespace list_usage;

class Program
{
    public struct NumberList
    {
        const ushort listMaxLength = 100;
        const ushort listMaxValue = 100;
        List<ushort> list = new(listMaxLength);
        public NumberList()
        {
            Random rnd = new Random();
            for (int i = 0; i < listMaxLength; i++)
                list.Add((ushort)rnd.Next(listMaxValue));
        }
        public void Print() => WriteLine(String.Join(' ', list.Select(x => x.ToString()).ToArray()));
        public void Filter() => list = list.Where(n => n > 25 & n < 50).ToList<ushort>();
    }
    static void Main(string[] args)
    {
        NumberList numList = new();
        WriteLine("------Initial list------");
        numList.Print();
        WriteLine("------Selection for list------");
        numList.Filter();
        numList.Print();
    }
}

