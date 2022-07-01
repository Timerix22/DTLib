using DTLib.Dtsod.V24;
using DTLib.Dtsod.V24.Autoarr;
using DTLib.Dtsod.V24.KerepTypes;

namespace DTLib.Tests;

public static class TestDtsodV24
{
    public static void TestAll()
    {
        TestBaseTypes();
        TestComplexes();
        TestLists();
        TestReSerialization();
        TestSpeed();
    }

    public static void TestBaseTypes()
    {
        Info.Log("c", "-----[TestDtsodV24/TestBaseTypes]-----");
        DtsodV24 dtsod = new(File.ReadAllText($"Dtsod{Путь.Разд}TestResources{Путь.Разд}DtsodV24{Путь.Разд}base_types.dtsod"));
        foreach (var pair in dtsod)
            Info.Log("b", pair.ToString());
        Info.Log("g", "test completed");
    }

    public static void TestComplexes()
    {
        Info.Log("c", "-----[TestDtsodV24/TestComplexes]-----");
        DtsodV24 dtsod = new(File.ReadAllText($"Dtsod{Путь.Разд}TestResources{Путь.Разд}DtsodV24{Путь.Разд}complexes.dtsod"));
        Info.Log("h", dtsod.ToString());
        Info.Log("g", "test completed");
    }
    
    public static void TestLists()
    {
        Info.Log("c", "-------[TestDtsodV24/TestLists]-------");
        DtsodV24 dtsod = new(File.ReadAllText($"Dtsod{Путь.Разд}TestResources{Путь.Разд}DtsodV24{Путь.Разд}lists.dtsod"));
        foreach (KVPair pair in dtsod)
        {
            var list = new Autoarr<Unitype>(pair.value.VoidPtr, false);
            Info.Log("b",  pair.key.ToStringUTF8(), "w", $" length: {list.Length}");
            foreach (var el in list)
            {
                Info.Log("h", '\t' + el.ToString());
                if (el.TypeCode == KerepTypeCode.AutoarrUnitypePtr)
                {
                    var ar = new Autoarr<Unitype>(el.VoidPtr, false);
                    foreach (var k in ar)
                    {
                        Info.Log($"\t\t{k.ToString()}");
                    }
                }
            }
        }
        Info.Log("g", "test completed");
    }

    public static void TestReSerialization()
    {
        Info.Log("c", "--[TestDtsodV24/TestReSerialization]--");
        var dtsod = new DtsodV24(new DtsodV24(new DtsodV24(
            new DtsodV24(File.ReadAllText($"Dtsod{Путь.Разд}TestResources{Путь.Разд}DtsodV24{Путь.Разд}complexes.dtsod")).ToString()).ToString()).ToString());
        Info.Log("h", dtsod.ToString());
        Info.Log("g", "test completed");
    }

    public static void TestSpeed()
    {
        Info.Log("c", "-------[TestDtsodV24/TestSpeed]-------");
        IDtsod dtsod=null;
        string _text = File.ReadAllText($"Dtsod{Путь.Разд}TestResources{Путь.Разд}DtsodV23{Путь.Разд}messages.dtsod");
        string text = "";
        LogOperationTime( "V23 to V24 conversion", 32, ()=>
            text = DtsodConverter.ConvertVersion(new DtsodV23(_text), DtsodVersion.V24).ToString()
        );
        File.WriteAllText($"Dtsod{Путь.Разд}TestResources{Путь.Разд}DtsodV24{Путь.Разд}messages.dtsod",text);
        LogOperationTime("V24 deserialization", 64, () => dtsod = new DtsodV24(text));
        LogOperationTime("V24 serialization", 64, () => text = dtsod.ToString());
        Info.Log("g", "test completed");
    }
}