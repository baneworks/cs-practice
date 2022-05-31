global using System;
global using System.Text;

namespace varbasic;

record struct Student(
  string firstName,
  string middleName,
  string lastName,
  uint age,
  string email,
  double scoreProg,
  double scoreMath,
  double scorePhys)
  {
    public double totalScore = scoreProg + scoreMath + scorePhys;
    public double avgScore = (scoreProg + scoreMath + scorePhys) / 3.0;
    public override string ToString()
    {
      return ($"| {firstName,19} " + 
              $"| {middleName,19} " + 
              $"| {lastName,19} " + 
              $"| {age,5:d} " + 
              $"| {email,29} " + 
              $"| {scoreProg,15} " + 
              $"| {scoreMath,15} " + 
              $"| {scorePhys,15} " + 
              $"| {totalScore,15:f2} " + 
              $"| {avgScore,15:f2} |");
    }
  };

class Program
{
  static void Main(string[] args)
  {
    Student me = new("Rinat", "Vladimirovich", "Levchuk", 43, "rinat.levchuk@gmail.com", 4.5, 4.5, 4.7);
    Student linus = new("Linus", "Benedict", "Torvalds", 52, "torvalds@linux-foundation.org", 5.0, 5.0, 5.0);

    Console.Write("Press enter for output...");
    Console.ReadKey();

    // header for table
    Console.Write(
      "\n" +
      "+-----First Name------|" +
       "-----Middle Name-----|" +
       "------Last Name------|" +
       "--Age--|" +
       "-------e-mail------------------|" +
       "---Prog. score---|" +
       "---Math. score---|" +
       "---Phys. score---|" + 
       "---Total score---|" +
       "----Avg. score---+\n");

    // print with string interpolation
    // TODO: truncate long string
    Console.Write(
      $"| {me.firstName,19} " + 
      $"| {me.middleName,19} " + 
      $"| {me.lastName,19} " + 
      $"| {me.age,5:d} " + 
      $"| {me.email,29} " + 
      $"| {me.scoreProg,15} " + 
      $"| {me.scoreMath,15} " + 
      $"| {me.scorePhys,15} " +
      $"| {me.totalScore,15:f2} " +
      $"| {me.avgScore,15:f2} |\n");

    // print with Studend.ToString
    Console.WriteLine(linus);

    // end of table
    Console.WriteLine((new StringBuilder()).Append("+").Append('-', 195).Append("+"));
  }
}