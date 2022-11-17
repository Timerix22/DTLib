using System.Linq;

namespace DTLib.Tests;

public static class TestDtsodV23
{
    public static void TestAll()
    {
        TestBaseTypes();
        TestLists();
        TestComplexes();
        TestReSerialization();
        TestSpeed();
        TestMemoryConsumption();
    }

    public static void TestBaseTypes()
    {
        OldLogger.Log("c", "-----[TestDtsodV23/TestBaseTypes]-----");
        DtsodV23 dtsod = new(File.ReadAllText($"Dtsod{Путь.Разд}TestResources{Путь.Разд}DtsodV23{Путь.Разд}base_types.dtsod"));
        foreach (var pair in dtsod)
            OldLogger.Log("b", pair.Value.GetType().Name + ' ', "w", pair.Key + ' ', "c", pair.Value.ToString());
        OldLogger.Log("g", "test completed");
    }
    public static void TestLists()
    {
        OldLogger.Log("c", "-------[TestDtsodV23/TestLists]-------");
        DtsodV23 dtsod = new(File.ReadAllText($"Dtsod{Путь.Разд}TestResources{Путь.Разд}DtsodV23{Путь.Разд}lists.dtsod"));
        foreach (var pair in dtsod)
        {
            OldLogger.Log("b",  pair.Value.GetType().Name + ' ', "w", pair.Key, "c",
                $" count: {pair.Value.Count}");
            foreach (var el in pair.Value)
                OldLogger.Log("b", '\t'+el.GetType().Name + ' ', "c", el.ToString());
        }
        OldLogger.Log("g", "test completed");
    }

    public static void TestComplexes()
    {
        OldLogger.Log("c", "-----[TestDtsodV23/TestComplexes]-----");
        DtsodV23 dtsod = new(File.ReadAllText($"Dtsod{Путь.Разд}TestResources{Путь.Разд}DtsodV23{Путь.Разд}complexes.dtsod"));
        foreach (var complex in dtsod)
        {
            OldLogger.Log("b", complex.Value.GetType().Name + ' ', "w", complex.Key,
                "b", " size: ", "c", complex.Value.Keys.Count.ToString());
            foreach (var pair in (DtsodV23) complex.Value)
                OldLogger.Log("b", '\t' + pair.Value.GetType().Name + ' ', "w", pair.Key + ' ',
                    "c", pair.Value.ToString());
        }
        OldLogger.Log("g", "test completed");
    }

    public static void TestReSerialization()
    {
        OldLogger.Log("c", "--[TestDtsodV23/TestReSerialization]--");
        var dtsod = new DtsodV23(new DtsodV23(new DtsodV23(
            new DtsodV23(File.ReadAllText($"Dtsod{Путь.Разд}TestResources{Путь.Разд}DtsodV23{Путь.Разд}complexes.dtsod")).ToString()).ToString()).ToString());
        OldLogger.Log("y", dtsod.ToString());
        OldLogger.Log("g", "test completed");
    }

    public static void TestSpeed()
    {
        OldLogger.Log("c", "-------[TestDtsodV23/TestSpeed]-------");
        IDtsod dtsod=null;
        string text = File.ReadAllText($"Dtsod{Путь.Разд}TestResources{Путь.Разд}DtsodV23{Путь.Разд}messages.dtsod");
        LogOperationTime("V21 deserialization",64,()=>dtsod=new DtsodV21(text));
        LogOperationTime("V21 serialization", 64, () => _=dtsod.ToString());
        LogOperationTime("V23 deserialization", 64, () => dtsod = new DtsodV23(text));
        LogOperationTime("V23 serialization", 64, () => _ = dtsod.ToString());
        OldLogger.Log("g", "test completed");
    }

    public static void TestMemoryConsumption()
    {
        OldLogger.Log("c", "----[TestDtsodV23/TestMemConsumpt]----");
        string text = File.ReadAllText($"Dtsod{Путь.Разд}TestResources{Путь.Разд}DtsodV23{Путь.Разд}messages.dtsod");
        var a = GC.GetTotalMemory(true);
        var dtsods = new DtsodV23[64];
        for (int i = 0; i < dtsods.Length; i++)
            dtsods[i] = new(text);
        var b = GC.GetTotalMemory(true);
        OldLogger.Log("b", "at the start: ","c",$"{a/1024} kb\n", 
            "b", "at the end: ", "c", $"{b / 1024} kb\n{dtsods.Count()}","b"," dtsods initialized");
        OldLogger.Log("g", "test completed");
    }
}
