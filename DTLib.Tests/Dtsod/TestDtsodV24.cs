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
        OldLogger.Log("c", "-----[TestDtsodV24/TestBaseTypes]-----");
        DtsodV24 dtsod = new(File.ReadAllText($"Dtsod{Путь.Разд}TestResources{Путь.Разд}DtsodV24{Путь.Разд}base_types.dtsod"));
        foreach (var pair in dtsod)
            OldLogger.Log("b", pair.ToString());
        OldLogger.Log("g", "test completed");
    }

    public static void TestComplexes()
    {
        OldLogger.Log("c", "-----[TestDtsodV24/TestComplexes]-----");
        DtsodV24 dtsod = new(File.ReadAllText($"Dtsod{Путь.Разд}TestResources{Путь.Разд}DtsodV24{Путь.Разд}complexes.dtsod"));
        OldLogger.Log("h", dtsod.ToString());
        OldLogger.Log("g", "test completed");
    }
    
    public static void TestLists()
    {
        OldLogger.Log("c", "-------[TestDtsodV24/TestLists]-------");
        DtsodV24 dtsod = new(File.ReadAllText($"Dtsod{Путь.Разд}TestResources{Путь.Разд}DtsodV24{Путь.Разд}lists.dtsod"));
        foreach (KVPair pair in dtsod)
        {
            var list = new Autoarr<Unitype>(pair.value.VoidPtr, false);
            OldLogger.Log("b",  pair.key.HGlobalUTF8ToString(), "w", $" length: {list.Length}");
            foreach (var el in list)
            {
                OldLogger.Log("h", '\t' + el.ToString());
                if (el.TypeCode == KerepTypeCode.AutoarrUnitypePtr)
                {
                    var ar = new Autoarr<Unitype>(el.VoidPtr, false);
                    foreach (var k in ar)
                    {
                        OldLogger.Log($"\t\t{k.ToString()}");
                    }
                }
            }
        }
        OldLogger.Log("g", "test completed");
    }

    public static void TestReSerialization()
    {
        OldLogger.Log("c", "--[TestDtsodV24/TestReSerialization]--");
        var dtsod = new DtsodV24(new DtsodV24(new DtsodV24(
            new DtsodV24(File.ReadAllText($"Dtsod{Путь.Разд}TestResources{Путь.Разд}DtsodV24{Путь.Разд}complexes.dtsod")).ToString()).ToString()).ToString());
        OldLogger.Log("h", dtsod.ToString());
        OldLogger.Log("g", "test completed");
    }

    public static void TestSpeed()
    {
        OldLogger.Log("c", "-------[TestDtsodV24/TestSpeed]-------");
        IDtsod dtsod=null;
        string _text = File.ReadAllText($"Dtsod{Путь.Разд}TestResources{Путь.Разд}DtsodV23{Путь.Разд}messages.dtsod");
        string text = "";
        LogOperationTime( "V23 to V24 conversion", 32, ()=>
            text = DtsodConverter.ConvertVersion(new DtsodV23(_text), DtsodVersion.V24).ToString()
        );
        File.WriteAllText($"Dtsod{Путь.Разд}TestResources{Путь.Разд}DtsodV24{Путь.Разд}messages.dtsod",text);
        LogOperationTime("V24 deserialization", 64, () => dtsod = new DtsodV24(text));
        LogOperationTime("V24 serialization", 64, () => text = dtsod.ToString());
        OldLogger.Log("g", "test completed");
    }
}