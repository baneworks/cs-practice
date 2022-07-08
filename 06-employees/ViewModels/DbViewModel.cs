using ReactiveUI;
using System.Reactive;

using Employees.Services;
using Employees.Models;

namespace Employees.ViewModels;

public partial class DbViewModel : ViewModelBase
{
    public Database Employees { get; }
    public WorkerWithId? SelectedWorker { get; set; } = null;
    public ReactiveCommand<Unit, WorkerWithId?> Edit { get; }
    public void Add(Worker worker) => Employees.Add(worker);
    public void Remove()
    {
        if (SelectedWorker != null)
        {
            Employees.Remove(SelectedWorker);
            SelectedWorker = null;
        }
    }
    public DbViewModel(Database db)
    {
        Employees = db;
        Edit = ReactiveCommand.Create(() => SelectedWorker);
    }
}