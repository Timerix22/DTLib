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
        DtsodV24 dtsod = new(File.ReadAllText($"DtsodV24{Путь.Разд}base_types.dtsod"));
        foreach (var autoarr in dtsod)
        {
            foreach (KVPair pair in autoarr)
                Info.LogNoTime("b", pair.ToString());
        }
        Info.Log("g", "test completed");
    }

    public static void TestComplexes()
    {
        Info.Log("c", "-----[TestDtsodV24/TestComplexes]-----");
        DtsodV24 dtsod = new(File.ReadAllText($"DtsodV24{Путь.Разд}complexes.dtsod"));
        Info.Log("h", dtsod.ToString());
        Info.Log("g", "test completed");
    }
    
    public static void TestLists()
    {
        Info.Log("c", "-------[TestDtsodV24/TestLists]-------");
        DtsodV24 dtsod = new(File.ReadAllText($"DtsodV24{Путь.Разд}lists.dtsod"));
        foreach (var autoarr in dtsod)
            foreach (KVPair pair in autoarr)
            {
                var list = new Autoarr<Unitype>(pair.value.VoidPtr, false);
                Info.LogNoTime("b",  pair.key.ToStringUTF8(), "w", $" length: {list.Length}");
                foreach (var el in list)
                    Info.LogNoTime("h", '\t' + el.ToString());
            }
        Info.Log("g", "test completed");
    }

    public static void TestReSerialization()
    {
        Info.Log("c", "--[TestDtsodV24/TestReSerialization]--");
        var dtsod = new DtsodV24(new DtsodV24(new DtsodV24(
            new DtsodV24(File.ReadAllText($"DtsodV24{Путь.Разд}complexes.dtsod")).ToString()).ToString()).ToString());
        Info.Log("h", dtsod.ToString());
        Info.Log("g", "test completed");
    }

    public static void TestSpeed()
    {
        Info.Log("c", "-------[TestDtsodV24/TestSpeed]-------");
        IDtsod dtsod=null;
        string _text = File.ReadAllText($"DtsodV23{Путь.Разд}messages.dtsod");
        string text = "";
        LogOperationTime( "V23 to V24 conversion", 32, ()=>
            text = DtsodConverter.ConvertVersion(new DtsodV23(_text), DtsodVersion.V24).ToString()
        );
        LogOperationTime("V24 deserialization", 64, () => dtsod = new DtsodV24(text));
        LogOperationTime("V24 serialization", 64, () => text = dtsod.ToString());
        Info.Log("g", "test completed");
    }
}