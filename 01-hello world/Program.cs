global using System;

string[] says = new string[] {"Hello", " ", "World", "!!!"};

foreach (string word in says)
{
  Console.Write(word);
}

Console.WriteLine();
Console.ReadKey();