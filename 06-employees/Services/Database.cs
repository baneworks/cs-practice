using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using Employees.Models;

namespace Employees.Services;

public class WorkerWithId : Worker
{
    RecID? id = null;
    public void Id(RecID id) => this.id = id;
    public RecID? Id() => this.id;
    public WorkerWithId() : base() {}
    public WorkerWithId(RecID id) : base() => this.id = id;
    public WorkerWithId(Worker worker, RecID id) : base()
    {
        this.id = id;
        _age = worker.Age;
        BirthDate = worker.BirthDate;
        FirstName = worker.FirstName;
        MiddleName = worker.MiddleName;
        LastName = worker.LastName;
        Height = worker.Height;
        Origin = worker.Origin;
    }
}

/// <summary>
/// Provides DB for worker's records
/// </summary>
public sealed class Database : IDisposable, IEnumerable<WorkerWithId>, INotifyCollectionChanged
{
    static readonly string dbFileName = @"employees.db";
    Dictionary<RecID, WorkerWithId> db = new();
    // fixme: this not at all! dbFileStream disposed anyway (
    // readonly FileStream dbFileStream; //* to lock our `DB` we will keep FileStream opened
    //! hack
    FileStream dbFileStream;

    //? Strange GC things
    //? 1. create dbFileStream in shared memory for Database
    // readonly static FileStream dbFileStream;

    #region Singleton
    readonly static Database? instance = new Database();
    public static Database GetInstance() => instance!;
    #endregion

    #region Constructors
    // fixme: remove - just for example
    static Database()
    {
        //? 2. Initialize with reference to FileStream instance
        // dbFileStream = new FileStream(dbFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);

        // instance = new Database();
    }
    private Database()
    {
        if (!File.Exists(dbFileName))
            throw new System.IO.FileLoadException("employees.db not found!");
        dbFileStream = File.Open(dbFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
        Load();
    }
    #endregion

    #region file io
    /// <summary>
    /// Load DB from file
    /// </summary>
    private void Load()
    {
        //* 3. If dbFileStream is static - object exists, all ok
        using (StreamReader sr = new StreamReader(dbFileStream))
        {
            string? line;
            while ((line = sr!.ReadLine()) != null)
            {
                (RecID id, WorkerWithId worker) rec = ParseRec(line!.Split('#'));
                rec.worker.Id(rec.id);
                db.Add(rec.id, rec.worker);
            }
        }

        if (db.Count == 0)
                throw new System.IO.FileLoadException("employees.db is corrupt!");

        NotifyCollectionChanged();
    }
    /// <summary>
    /// Save DB to file
    /// </summary>
    public void Update()
    {
        //! 4. If dbFileStream is static - object disposed!
        //! correction - object disposed anyway
        //! hack:
        dbFileStream = dbFileStream.CanWrite ?
                           dbFileStream :
                           File.Open(dbFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
        //? But why: Databased instances is in game, shared memory for Database exist,
        //? reference count > 0?
        using (StreamWriter sw = new StreamWriter(dbFileStream))
            foreach (var rec in db)
                sw.WriteLine(rec.Key.ToString() + '#' + rec.Value.ToString());
    }
    #endregion

    #region collection manage
    // public IEnumerable<Worker> GetItems() => db.Values;
    private (RecID, WorkerWithId) ParseRec(string[] fields) =>
        fields switch
                {
                    var f when f.Length == 7 => (new RecID(f[0], f[1]),
                                                 new WorkerWithId(new RecID(f[0], f[1]))
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
    public void Add(Worker worker)
    {
        RecID id = new RecID(db.Keys.Count + 1, DateTime.Now.ToString("dd.MM.yyyy hh:mm"));
        db.Add(id, new WorkerWithId(worker, id));
        Update();
        NotifyCollectionChanged();
    }
     public void Add(WorkerWithId worker)
    {
        RecID id = new RecID(db.Keys.Count + 1, DateTime.Now.ToString("dd.MM.yyyy hh:mm"));
        worker.Id(id);
        db.Add(id, worker);
        Update();
        NotifyCollectionChanged();
    }
    public bool Remove(WorkerWithId worker)
    {
        RecID? id = worker.Id();
        if (id != null && db.ContainsKey(id.Value))
        {
            bool res = db.Remove(id.Value);
            Update();
            NotifyCollectionChanged();
            return res;
        }
        else
            return false;
    }
    public Worker this[RecID id] => db[id];

    #endregion

    #region Interfaces
    public IEnumerator<WorkerWithId> GetEnumerator() => db.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => db.Values.GetEnumerator();

    //* just for simplicity in all cases we reset collection
    static readonly NotifyCollectionChangedEventArgs collectionReset =
        new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    private void NotifyCollectionChanged()
    {
        NotifyCollectionChangedEventHandler? tmpColChg = CollectionChanged;
        if (tmpColChg != null) tmpColChg(this, collectionReset);
    }

    public void Dispose()
    {
        Update();
        dbFileStream.Dispose();
    }
    #endregion
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
    public RecID(string str)
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