using System.IO;
using System.Reflection;
using System.Collections.Generic;

using Employees.Models;

namespace Employees.Services;

public class Database
{
    static string dbFileName;
    FileStream dbFile; //* to lock our `DB` we will keep FileStream opened
    Dictionary<RecID, Worker> db = new();
    static Database()
    {
        dbFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/../../../employees.db";
    }
    public Database()
    {
        if (!File.Exists(dbFileName))
            throw new System.IO.FileLoadException("employees.db not found!");

        dbFile = new FileStream(dbFileName, FileMode.Open, FileAccess.Read, FileShare.None);
        LoadDb();
    }
    public IEnumerable<Worker> GetItems() => db.Values;

    //? more elegant but not working
    // (RecID id, Worker worker) rec = fields switch
    // {
    //     var [id, dt, name, _, height, birthdate, origin, _] => (new RecID(id, dt), new Worker { /* ... */ }),
    //     _ => throw new System.IO.FileLoadException("employees.db is corrupt!"),
    // };
    private (RecID, Worker) ParseRec(string[] fields) =>
        fields switch
                {
                    var f when f.Length == 7 => (new RecID(f[0], f[1]),
                                                 new Worker
                                                 {
                                                     LastName = f[2].Split(' ')[0],
                                                     FirstName = f[2].Split(' ')[1],
                                                     MiddleName = f[2].Split(' ')[2],
                                                     // f[3] is age, skipped
                                                     Height = int.Parse(f[4]),
                                                     BirthDate = DateTime.Parse(f[5]),
                                                     Origin = f[6],
                                                 }),
                    _ => throw new System.IO.FileLoadException("employees.db is corrupt!"),
                };
    private void LoadDb()
    {
        // better way is foreach (line in File.ReadLines(dbFileName)), but task is task
        using (StreamReader reader = new StreamReader(dbFile))
        {
            string? line;
            do
            {
                line = reader.ReadLine();
                if (line == null)
                    break;

                //? its possible to decompose tuple and calling ParseRec in one line? lambda?
                (RecID id, Worker worker) rec = ParseRec(line!.Split('#'));
                db.Add(rec.id, rec.worker);

            } while(line != null);

            if (db.Count == 0)
                throw new System.IO.FileLoadException("employees.db is corrupt!");
        }
    }
    public void Add(Worker worker)
    {
        RecID id = new RecID(db.Keys.Count + 1, DateTime.Now.ToString("dd.MM.yyyy hh:mm"));
        db.Add(id, worker);
        UpdateDb();
    }
    public void UpdateDb()
    {
        dbFile.Dispose();
        dbFile = new FileStream(dbFileName, FileMode.Open, FileAccess.Write, FileShare.None);
        using (StreamWriter writer = new StreamWriter(dbFile))
        {
            foreach (var rec in db)
            {
                string res = rec.Key.ToString() + '#' + rec.Value.ToString();
                writer.WriteLine(res);
            }
        }
        dbFile.Dispose();
        dbFile = new FileStream(dbFileName, FileMode.Open, FileAccess.Read, FileShare.None);
    }
    // fixme: never called
    ~Database()
    {
    }
}

public struct RecID
{
    public int Index { get; }
    public DateTime TimeStamp { get; }
    public RecID(int idx, string dtString) : this()
    {
        Index = idx;
        TimeStamp = DateTime.ParseExact(dtString, "dd.MM.yyyy hh:mm", null);
    }
    public RecID(string idx, string dtString) : this(int.Parse(idx), dtString) {}
    public RecID(string str) : this()
    {
        var (id, dt) = str.Split('#') switch
        {
            var a when a.Length == 2 => (a[0], a[1]),
            _ => throw new System.ArgumentNullException("id or datetime parse error"),
        };
        Index = int.Parse(id!);
        TimeStamp = DateTime.Parse(dt!);
    }
    public override string ToString()
    {
        return String.Join('#', Index, TimeStamp.ToString("dd.MM.yyyy hh:mm"));
    }
}