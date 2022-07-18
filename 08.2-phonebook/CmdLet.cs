namespace CmdLets;

#region const & enums

public struct PatterConsts
{
    public const string Initial = @"^\s*";
    public const string Separator = @"\s+";
    public const string Terminator = @"\s*$";
}

/// <summary>
/// Тип номера:
/// <list>
///     <item>Internal - номера на 8 (не форматируются)</item>
///     <item>International - международные (форматируются)</item>
/// </list>
/// </summary>
public enum PhoneNumType
{
    National,
    International,
    Internal,
    None
}

#endregion

public interface ICmdArg
{
    string Name { get; }
    bool Optional { get; set; }
    bool Success { get; }
    string Pattern();
    bool Parse(GroupCollection groups);
    public static ICmdArg operator !(ICmdArg arg) {
        arg.Optional = true;
        return arg;
    }
    public static ICmdArg[] operator +(ICmdArg arg1, ICmdArg arg2) { return new ICmdArg[] {arg1, arg2}; }
    public static ICmdArg[] operator |(ICmdArg arg1, ICmdArg arg2)
    {
        arg2.Optional = true;
        return arg1 + arg2;
    }
}

/// <summary>
/// Номер телефона с кодом страны и оператора. Позволительное упрощение
/// </summary>
public struct PhoneNum : ICmdArg
{
    static readonly string name = "PhoneNum";
    static readonly string[] components = new string[] {"pref", "op", "phone"};
    static string pattern = @"(?<" + name + @">(?<pref>(?>\+\d|8))[\s\-]\(?(?<op>\d+)\)?[\s\-](?<phone>[0-9\-\s]{7,14}))";
    public string Name => name;
    public PhoneNumType Type { get; private set; } = PhoneNumType.None;
    public ulong Value { get; private set; } = 0;
    void Invalidate()
    {
        Type = PhoneNumType.None;
        Value = 0;
        Success = false;
    }
    public PhoneNum() => Invalidate();
    public PhoneNum(PhoneNumType type, ulong phone)
    {
        Type = type;
        Value = phone;
    }
    // ICmdArg
    public string Pattern()
    {
        if (Optional)
            return pattern + "?";
        return pattern + "??";
    }
    public bool Optional { get; set; } = false;
    public bool Success { get; private set; } = false;
    public bool Parse(GroupCollection groups)
    {
        if (!groups.ContainsKey(PhoneNum.name))
        {
            Invalidate();
            return false;
        }

        foreach (var part in components)
            if (!groups.ContainsKey(part))
            {
                Invalidate();
                return false;
            }

        ulong code = 0;
        if (groups["pref"].Value[0] == '+') {
            Type = PhoneNumType.International;
            code = ulong.Parse("" + groups["pref"].Value[1]);
        }
        else {
            Type = PhoneNumType.National;
            code = 7; // храним одинаково чтобы не было дублей
        }

        ulong op = ulong.Parse(groups["op"].Value);
        ulong num = ulong.Parse(String.Join("", groups["phone"].Value.Split('-', '(', ')')));

        Value = code * 1_000_000_000_0 + op * 1_000_000_0 + num;
        Success = true;
        return true;
    }
    public override string ToString()
    {
        if (Type == PhoneNumType.None)
            return String.Empty;
        if (Type == PhoneNumType.National)
            return (Value + 1_000_000_000_0).ToString();
        else
            return "+" + Value.ToString();
    }
}

/// <summary>
/// Внутренний номер телефона. Без проверки, просто цифры
/// </summary>
public struct InternalPhoneNum : ICmdArg
{
    static readonly string name = "InternalPhoneNum";
    static string pattern = @"(?<" + name + @">(?<phone>[0-9\-\s]{7,14}))";
    public string Name => name;
    public PhoneNumType Type { get; private set; } = PhoneNumType.None;
    public uint Value { get; private set; } = 0;
    void Invalidate()
    {
        Type = PhoneNumType.None;
        Value = 0;
        Success = false;
    }
    public InternalPhoneNum() => Invalidate();
    public InternalPhoneNum(PhoneNumType type, uint phone)
    {
        Type = type;
        Value = phone;
    }
    // ICmdArg
    public string Pattern()
    {
        if (Optional)
            return pattern + "?";
        return pattern + "??";
    }
    public bool Optional { get; set; } = false;
    public bool Success { get; private set; } = false;
    public bool Parse(GroupCollection groups)
    {
        if (!groups.ContainsKey(name))
        {
            Invalidate();
            return false;
        }

        Value = uint.Parse(String.Join("", groups[name].Value.Split('-', '(', ')')));
        Type = PhoneNumType.Internal;
        Success = true;
        return true;
    }
    public override string ToString()
    {
        if (Type == PhoneNumType.None)
            return String.Empty;
        uint[] v = new uint[3];
        v[0] = Value / 10_000;
        v[1] = (Value - v[0] * 10_000) / 100;
        v[2] = (Value - v[0] * 10_000 - v[1] * 100);
        return String.Join('-', v.Select(x => x.ToString()));
    }
}

/// <summary>
/// Фамилия Имя Отчество
/// </summary>
public struct PersonName : ICmdArg
{
    static readonly string name = "PersonName";
    static readonly string[] components = new string[] {"ln", "fn", "mn"};
    static string pattern = @"(?<" + name + @">(?<ln>\w+)\s+(?<fn>\w+)\s+(?<mn>\w+))";
    public string Name => name;
    public string? Value { get; private set; } = null;
    public PersonName() => Invalidate();
    void Invalidate()
    {
        Value = null;
        Success = false;
    }
    // ICmdArg
    public string Pattern()
    {
        if (Optional)
            return pattern + "?";
        return pattern + "??";
    }
    public bool Optional { get; set; } = false;
    public bool Success { get; private set; } = false;
    public bool Parse(GroupCollection groups)
    {
        if (!groups.ContainsKey(name))
        {
            Invalidate();
            return false;
        }

        foreach (var part in components)
            if (!groups.ContainsKey(part))
            {
                Invalidate();
                return false;
            }

        Value = groups["ln"].Value + ' ' +
                groups["fn"].Value + ' ' +
                groups["mn"].Value;
        Success = true;

        return true;
    }
    public override string ToString() => Value ?? String.Empty;
}

/// <summary>
/// Улица, Дом, Квартира
/// </summary>
public struct Address : ICmdArg
{
    static readonly string name = "Address";
    static readonly string[] components = new string[] {"street", "house", "flat"};
    static string pattern = @"(?<" + name + @">(?<street>\w+)\s+(?<house>\w+)\s+(?<flat>\d+))";
    public string Name => name;
    public (string street, string house, uint flat)? Value { get; private set; } = null;
    public Address() => Invalidate();
    void Invalidate()
    {
        Value = null;
        Success = false;
    }
    // ICmdArg
    public string Pattern()
    {
        if (Optional)
            return pattern + "?";
        return pattern + "??";
    }
    public bool Optional { get; set; } = false;
    public bool Success { get; private set; } = false;
    public bool Parse(GroupCollection groups)
    {
        if (!groups.ContainsKey(name))
        {
            Invalidate();
            return false;
        }

        foreach (var part in components)
            if (!groups.ContainsKey(part))
            {
                Invalidate();
                return false;
            }

        Value = (groups["street"].Value,
                 groups["house"].Value,
                 uint.Parse(groups["flat"].Value));
        Success = true;

        return true;
    }
    public override string ToString() => Value != null ? Value.ToString()! : String.Empty;
}

public struct CmdLet : IEnumerable<ICmdArg>
{
    List<ICmdArg> Args = new();
    public string Pattern => $"{PatterConsts.Initial}/(?<cmd>(?>{Cmd}))";
    public string Cmd { get; }
    public bool Success { get; private set; } = false;
    public CmdLet(string cmd) => Cmd = cmd;
    public CmdLet(string cmd, params ICmdArg[] args) : this(cmd) => Args.AddRange(args);
    public bool Parse(string str)
    {
        string ptrn = Pattern;
        if (Args.Count != 0) ptrn += PatterConsts.Separator;
        for (int i = 0; i < Args.Count; i++)
        {
            ptrn += Args[i].Pattern();
            if (i != Args.Count - 1) ptrn += PatterConsts.Separator;
        }
        ptrn += PatterConsts.Terminator;

        Regex rg = new Regex(ptrn);
        Match m = rg.Match(str);
        Success = m.Success;
        if (!Success) return false;

        foreach (var arg in Args)
            if (!arg.Parse(m.Groups) && !arg.Optional)
            {
                Success = false;
                return false;
            }

        Success = true;
        return true;
    }

    public ICmdArg? this[int i] => Args[i];
    public ICmdArg? this[string name]
    {
        get
        {
            foreach (var arg in Args)
                if (arg.Name == name) return arg;
            return null;
        }
    }

    public IEnumerator<ICmdArg> GetEnumerator() => Args.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Args.GetEnumerator();
}

public struct CmdSet : IEnumerable<CmdLet>
{
    List<CmdLet> cmdSet;
    public CmdLet? Match { get; private set; } = null;
    public bool Success { get; private set; } = false;
    public CmdSet() => cmdSet = new();
    public CmdSet(params CmdLet[] cmds) : this() => cmdSet.AddRange(cmds);
    public bool Parse(string str)
    {
        foreach (var cmd in cmdSet)
            if (cmd.Parse(str))
            {
                Success = true;
                Match = cmd;
            }
        return Success;
    }
    public void Add(CmdLet cmd) => cmdSet.Add(cmd);
    public IEnumerator<CmdLet> GetEnumerator() => cmdSet.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => cmdSet.GetEnumerator();
}
