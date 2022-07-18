// namespace task_08_222;

// #region CmdType

// // fixme: свести CmdType, CmdTypeMethods, CmdParse в один класс

// /// <summary>
// /// Командлеты. Из перечисления собирается шаблон распознования
// /// <list>
// ///     <item><term>add</term>  <description>Add a record</description></item>
// ///     <item><term>ls</term>   <description>List all records</description></item>
// ///     <item><term>rm</term>   <description>Remove the record</description></item>
// ///     <item><term>find</term> <description>Find record by phone</description></item>
// /// </list>
// /// </summary>
// internal enum CmdType
// {
//         add,
//         ls,
//         rm,
//         find,
//         exit
// }

// internal static class CmdTypeMethods
// {
//     public static string ToPattern(this CmdType cmd) =>
//         @"^\s*/(?<cmd>(?>" + String.Join('|', Enum.GetNames(typeof(CmdType))) + @"))";
//     public static CmdType? FromString(this string cmd)
//     {
//         int idx = 0;
//         foreach (var name in Enum.GetNames(typeof(CmdType)))
//             if (name == cmd)
//                 return (CmdType)idx;
//             else
//                 idx++;
//         return null;
//     }
// }

// #endregion

// #region CmdParser

// /// <summary>
// /// Разбор командной строки
// /// </summary>
// struct CmdParser
// {
//     //* static
//     // разделил на отдельные шаблоны дабы не выносить мозг читающему
//     static string ptrnCmd = CmdTypeMethods.ToPattern(new CmdType()) + @"\s+";
//     static string ptrnPhone = PhoneNum.ToPattern() + @"\s+";
//     // static string ptrnName = @"(?<ln>\w+)\s+(?<fn>\w+)\s+(?<mn>\w+)\s*$";

//     //* fields & props
//     public CmdType? Cmd { get; private set; } = null;
//     public (string phone, string name)? Args { get; private set; } = null;

//     //* constructors
//     public CmdParser(string strCmd) => Parse(strCmd);

//     //* methods
//     /// <summary>
//     /// Заполняет поля:
//     /// <list>
//     ///     <item>CmdType Cmd</item>
//     ///     <item>(PhoneNum phone, string Name) Args</item>
//     /// </list>
//     /// </summary>
//     /// <param name="strCmd"></param>
//     /// <returns>True on success</returns>
//     public bool Parse(string strCmd)
//     {
//         Regex rg = new Regex(ptrnCmd);
//         Match m = rg.Match(strCmd);
//         if (!m.Success) return false;

//         Cmd = CmdTypeMethods.FromString(m.Groups["cmd"].Value);

//         switch (Cmd)
//         {
//             case CmdType.ls:
//             case CmdType.exit:
//                 return true;
//             case CmdType.rm:
//                 rg = new Regex(ptrnPhone);
//                 break;
//             case CmdType.find:
//                 rg = new Regex(ptrnPhone);
//                 break;
//             case CmdType.add:
//                 break;
//             default:
//                 return false;
//         }

//         //? Why don't work GroupsCollection?
//         // m.Groups.Select(Name => new string[]{"fn", "mn", "ls"});
//         string name = String.Join(' ', m.Groups["fn"].Value, m.Groups["mn"].Value, m.Groups["ln"].Value);

//         // PhoneNum phone = new PhoneNum();
//         // phone.Parse(m.Groups);
//         // string phone = String.Join('-', m.Groups["pref"], m.Groups["phone"]);

//         // Args = (phone, name);

//         return true;
//     }
// }

// #endregion
// */