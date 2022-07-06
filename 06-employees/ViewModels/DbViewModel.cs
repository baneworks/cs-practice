using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Employees.Services;
using Employees.Models;

namespace Employees.ViewModels;

public partial class DbViewModel : ViewModelBase
{
    Database _db;
    public ObservableCollection<Worker> Employees { get; }
    public void Add(Worker worker)
    {
        if (worker.IsValid())
        {
            Employees.Add(worker);
            _db.Add(worker);
        }
    }
    public DbViewModel(Database db)
    {
        _db = db;
        Employees = new ObservableCollection<Worker>(db.GetItems());
    }
}