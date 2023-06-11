/*
using KerepWrapper.Dtsod;
using KerepWrapper.Autoarr;
using KerepWrapper.KerepTypes;

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
        Logger.Log("c", "-----[TestDtsodV24/TestBaseTypes]-----");
        DtsodV24 dtsod = new(File.ReadAllText($"Dtsod{Path.Sep}TestResources{Path.Sep}DtsodV24{Path.Sep}base_types.dtsod"));
        foreach (var pair in dtsod)
            Logger.Log("b", pair.ToString());
        Logger.Log("g", "test completed");
    }

    public static void TestComplexes()
    {
        Logger.Log("c", "-----[TestDtsodV24/TestComplexes]-----");
        DtsodV24 dtsod = new(File.ReadAllText($"Dtsod{Path.Sep}TestResources{Path.Sep}DtsodV24{Path.Sep}complexes.dtsod"));
        Logger.Log("h", dtsod.ToString());
        Logger.Log("g", "test completed");
    }
    
    public static void TestLists()
    {
        Logger.Log("c", "-------[TestDtsodV24/TestLists]-------");
        DtsodV24 dtsod = new(File.ReadAllText($"Dtsod{Path.Sep}TestResources{Path.Sep}DtsodV24{Path.Sep}lists.dtsod"));
        foreach (KVPair pair in dtsod)
        {
            var list = new Autoarr<Unitype>(pair.value.VoidPtr, false);
            Logger.Log("b",  pair.key.HGlobalUTF8ToString(), "w", $" length: {list.Length}");
            foreach (var el in list)
            {
                Logger.Log("h", '\t' + el.ToString());
                if (el.TypeCode == KerepTypeCode.AutoarrUnitypePtr)
                {
                    var ar = new Autoarr<Unitype>(el.VoidPtr, false);
                    foreach (var k in ar)
                    {
                        Logger.Log($"\t\t{k.ToString()}");
                    }
                }
            }
        }
        Logger.Log("g", "test completed");
    }

    public static void TestReSerialization()
    {
        Logger.Log("c", "--[TestDtsodV24/TestReSerialization]--");
        var dtsod = new DtsodV24(new DtsodV24(new DtsodV24(
            new DtsodV24(File.ReadAllText($"Dtsod{Path.Sep}TestResources{Path.Sep}DtsodV24{Path.Sep}complexes.dtsod")).ToString()).ToString()).ToString());
        Logger.Log("h", dtsod.ToString());
        Logger.Log("g", "test completed");
    }

    public static void TestSpeed()
    {
        Logger.Log("c", "-------[TestDtsodV24/TestSpeed]-------");
        IDtsod dtsod=null;
        string _text = File.ReadAllText($"Dtsod{Path.Sep}TestResources{Path.Sep}DtsodV23{Path.Sep}messages.dtsod");
        string text = "";
        LogOperationTime( "V23 to V24 conversion", 32, ()=>
            text = DtsodConverter.ConvertVersion(new DtsodV23(_text), DtsodVersion.V24).ToString()
        );
        File.WriteAllText($"Dtsod{Path.Sep}TestResources{Path.Sep}DtsodV24{Path.Sep}messages.dtsod",text);
        LogOperationTime("V24 deserialization", 64, () => dtsod = new DtsodV24(text));
        LogOperationTime("V24 serialization", 64, () => text = dtsod.ToString());
        Logger.Log("g", "test completed");
    }
}
*/