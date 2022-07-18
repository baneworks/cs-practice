#define __test__

global using System.Collections;
global using System.Collections.Generic;
global using System.Text.RegularExpressions;
global using static System.Console;

using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

using CmdLets;

#if __test__
using task_08_4_tests;
#endif

namespace task_08_4;

using NbDictionary = Dictionary<string, (PhoneNum, InternalPhoneNum, Address)>;

class Program
{
    static XDocument? GenXml(NbDictionary data)
    {
        // fixme: internal xsd ref not working!
        XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";

        XDocument xd = new XDocument(
            new XDeclaration("1.0", "utf-8", "true"),
            new XElement("notebook",
                         new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                         new XAttribute(xsi + "noNamespaceSchemaLocation", "notebook.db.xsd")));

        XElement? note = xd.Element("notebook");
        if (note == null) return null;

        note.Add(
            from kv in data
            select new XElement(
                "Person",
                new XAttribute("Name", kv.Key),
                new XElement(
                    "Address",
                    new XElement(
                        "Street",
                        (((string, string, uint))kv.Value.Item3.Value!).Item1
                    ),
                    new XElement(
                        "HouseNumber",
                        (((string, string, uint))kv.Value.Item3.Value!).Item2
                    ),
                    new XElement(
                        "FlatNumber",
                        (((string, string, uint))kv.Value.Item3.Value!).Item3
                    )
                ),
                new XElement(
                    "Phones",
                    new XElement(
                        "MobilePhone",
                        kv.Value.Item1.ToString()
                    ),
                    new XElement(
                        "FlatPhone",
                        kv.Value.Item2.ToString()
                    )
                )
            )
        );

        return xd;
    }
    static bool ValidateXml(XDocument xd, string xsFile)
    {
        XmlSchemaSet xs = new XmlSchemaSet();
        xs.Add("", XmlReader.Create(new StreamReader(xsFile)));
        bool errors = false;
        xd.Validate(xs, (sender, e) =>
        {
            WriteLine(e.Message);
            errors = true;
        }, true);
        return !errors;
    }
    static bool SaveToXml(string xdFileName, string xsFileName, NbDictionary data)
    {
        XDocument? xd = GenXml(data);
        if (xd == null) return false;

        if (!ValidateXml(xd, xsFileName)) return false;

        FileStream file = File.Open(xdFileName, FileMode.Truncate, FileAccess.Write, FileShare.Read);
        using (StreamWriter sw = new StreamWriter(file)) xd.Save(sw);
        return true;
    }
    static void Main(string[] args)
    {
        const string xdFileName = "notebook.db.xml";
        const string xsFileName = "notebook.db.xsd";

        // StreamWriter sw = File.OpenWrite(xmlFileName, FileAccess.Write, FileShare.Read);

        CmdSet cmdSet = new CmdSet {
            new CmdLet ("add", new PersonName(), new PhoneNum(), new InternalPhoneNum(), new Address()),
            new CmdLet ("ls"),
            new CmdLet ("save"),
            new CmdLet ("exit"),
        };

        NbDictionary db = new();

        Write("Command [/add /ls /save /exit <params>]? ");

        string? strCmd = null;

        #if __test__
        WriteLine();
        int idx = 0;
        #endif

        bool exit = false;

        do {
            do {
                #if __test__
                    strCmd = Tests.CmdSeq[idx];
                    idx += 1;
                #else
                    do
                        strCmd = ReadLine();
                    while (strCmd == null);
                #endif
            } while(!cmdSet.Parse(strCmd));

            CmdLet cmd = cmdSet.Match!.Value;

            switch(cmd.Cmd)
            {
                case "add":
                    WriteLine(String.Join(' ', '/' + cmd.Cmd, cmd[0], cmd[1], cmd[2]));
                    db.Add(cmd["PersonName"]?.ToString()!,
                           ((PhoneNum)cmd["PhoneNum"]!,
                            (InternalPhoneNum)cmd["InternalPhoneNum"]!,
                            (Address)cmd["Address"]!));
                    break;
                case "ls":
                    WriteLine(String.Join(' ', '/' + cmd.Cmd));
                    foreach (var k in db.Keys)
                        WriteLine(k.ToString() + ": " + db[k]);
                    break;
                case "save":
                    if(!SaveToXml(xdFileName, xsFileName, db)) {
                        WriteLine("Failed to save!");
                    } else {
                        WriteLine("Save success!");
                    }
                    break;
                case "exit":
                    exit = true;
                    break;
            }

        } while(!exit);

        WriteLine($"Records in dictionary: {db.Count}");
    }
}
