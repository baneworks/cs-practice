using System.Reactive;
using ReactiveUI;

using Employees.Services;
using Employees.Models;

namespace Employees.ViewModels;

public partial class RecordViewModel : ViewModelBase
{
    Worker _worker = new Worker();
    // (string? first, string? middle, string? last) _name = (null, null, null);
    public RecordViewModel()
    {
        Ok = ReactiveCommand.Create(() => PassWorker());
        Cancel = ReactiveCommand.Create(() => { });
    }
    private Worker? PassWorker()
    {
        if (_worker.IsValid())
            return _worker;
        else
            return null;
    }
    public ReactiveCommand<Unit, Worker?> Ok { get; }
    public ReactiveCommand<Unit, Unit> Cancel { get; }
    public string? FirstName
    {
        get => _worker.FirstName;
        set => _worker.FirstName = value;
    }
    public string? MiddleName
    {
        get => _worker.MiddleName;
        set => _worker.MiddleName = value;
    }
    public string? LastName
    {
        get => _worker.LastName;
        set => _worker.LastName = value;
    }
    public string? Origin
    {
        get => _worker.Origin;
        set => _worker.Origin = value;
    }
    public DateTimeOffset? BirthDate
    {
        get => _worker.BirthDate;
        set => _worker.BirthDate = value?.DateTime;
    }
    public int? Height
    {
        get => _worker.Height;
        set => _worker.Height = value;
    }
}