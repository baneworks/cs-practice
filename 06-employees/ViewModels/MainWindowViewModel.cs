using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

using Employees.Services;
using Employees.Models;

namespace Employees.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private Database _db;
    private DbViewModel _dbView;
    private ViewModelBase _view;
    public MainWindowViewModel(Database db)
    {
        _db = db;
        _view = _dbView = new DbViewModel(_db);
    }
    public ViewModelBase View
    {
        get => _view;
        private set => this.RaiseAndSetIfChanged(ref _view, value);
    }
    public void AddItem()
    {
        var vmRecord = new RecordViewModel(_dbView.SelectedWorker);

        Observable.Merge(
            vmRecord.Ok,
            _dbView.Edit,
            vmRecord.Cancel.Select(_ => (Worker?)null))
            .Take(1)
            .Subscribe(model =>
            {
                if (model != null && model.IsValid())
                    _dbView.Add(model);

                View = _dbView;
            });

        View = vmRecord;

    }
}