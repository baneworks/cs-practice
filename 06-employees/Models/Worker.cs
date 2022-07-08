using System;

namespace Employees.Models;

public class Worker
{
    protected int? _age = null;
    private DateTime? _birthdate = null;
    public string? FirstName { get; set; } = null;
    public string? MiddleName { get; set; } = null;
    public string? LastName { get; set; } = null;
    public string? Origin { get; set; } = null;
    public int? Age => _age;
    public int? Height { get; set; } = null;
    public DateTime? BirthDate
    {
        get => _birthdate;
        set
        {
            _birthdate = value;
            CalcAge(value);
        }
    }

    // fixme: wrong if birth date is 29.02.xxxx
    private void CalcAge(DateTime? bd)
    {
        if (bd == null)
        {
            _age = null;
            return;
        }
        int age;
        // age = (int) ((DateTime.Today - bd.Value.ToDateTime(TimeOnly.MinValue)).TotalDays/365.2425);
        age = (int) ((DateTime.Today - bd.Value).TotalDays/365.2425);
        _age = age > 0 ? age : null;
    }

    public bool IsValid()
    {
        bool res;
        res = FirstName != null && MiddleName != null && LastName != null ? true : false;
        res = res && Height != null && BirthDate != null && Origin != null ? true : false;
        return res;
    }
    public override string ToString()
    {
        return String.Join('#', String.Join(' ', LastName, FirstName, MiddleName), Age, Height, BirthDate?.ToString("dd.MM.yyyy"), Origin);
    }
}
