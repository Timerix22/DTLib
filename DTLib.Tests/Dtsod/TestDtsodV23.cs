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
        ColoredConsole.WriteLine("c", "-----[TestDtsodV23/TestBaseTypes]-----");
        DtsodV23 dtsod = new(File.ReadAllText(Path.Concat("Dtsod","TestResources","DtsodV23", "base_types.dtsod")));
        foreach (var pair in dtsod)
            ColoredConsole.WriteLine("b", pair.Value.GetType().Name + ' ', "w", pair.Key + ' ', "c", pair.Value.ToString());
        ColoredConsole.WriteLine("g", "test completed");
    }
    public static void TestLists()
    {
        ColoredConsole.WriteLine("c", "-------[TestDtsodV23/TestLists]-------");
        DtsodV23 dtsod = new(File.ReadAllText(Path.Concat("Dtsod","TestResources","DtsodV23", "lists.dtsod")));
        foreach (var pair in dtsod)
        {
            ColoredConsole.WriteLine("b",  pair.Value.GetType().Name + ' ', "w", pair.Key, "c",
                $" count: {pair.Value.Count}");
            foreach (var el in pair.Value)
                ColoredConsole.WriteLine("b", '\t'+el.GetType().Name + ' ', "c", el.ToString());
        }
        ColoredConsole.WriteLine("g", "test completed");
    }

    public static void TestComplexes()
    {
        ColoredConsole.WriteLine("c", "-----[TestDtsodV23/TestComplexes]-----");
        DtsodV23 dtsod = new(File.ReadAllText(Path.Concat("Dtsod","TestResources","DtsodV23", "complexes.dtsod")));
        foreach (var complex in dtsod)
        {
            ColoredConsole.WriteLine("b", complex.Value.GetType().Name + ' ', "w", complex.Key,
                "b", " size: ", "c", complex.Value.Keys.Count.ToString());
            foreach (var pair in (DtsodV23) complex.Value)
                ColoredConsole.WriteLine("b", '\t' + pair.Value.GetType().Name + ' ', "w", pair.Key + ' ',
                    "c", pair.Value.ToString());
        }
        ColoredConsole.WriteLine("g", "test completed");
    }

    public static void TestReSerialization()
    {
        ColoredConsole.WriteLine("c", "--[TestDtsodV23/TestReSerialization]--");
        var dtsod = new DtsodV23(new DtsodV23(new DtsodV23(
            new DtsodV23(File.ReadAllText(Path.Concat("Dtsod","TestResources","DtsodV23", "complexes.dtsod")))).ToString()).ToString());
        ColoredConsole.WriteLine("y", dtsod.ToString());
        ColoredConsole.WriteLine("g", "test completed");
    }

    public static void TestSpeed()
    {
        ColoredConsole.WriteLine("c", "-------[TestDtsodV23/TestSpeed]-------");
        IDtsod dtsod=null;
        string text = File.ReadAllText(Path.Concat("Dtsod","TestResources","DtsodV23", "messages.dtsod"));
        LogOperationTime("V21 deserialization",64,()=>dtsod=new DtsodV21(text));
        LogOperationTime("V21 serialization", 64, () => _=dtsod.ToString());
        LogOperationTime("V23 deserialization", 64, () => dtsod = new DtsodV23(text));
        LogOperationTime("V23 serialization", 64, () => _ = dtsod.ToString());
        ColoredConsole.WriteLine("g", "test completed");
    }

    public static void TestMemoryConsumption()
    {
        ColoredConsole.WriteLine("c", "----[TestDtsodV23/TestMemConsumpt]----");
        string text = File.ReadAllText(Path.Concat("Dtsod","TestResources","DtsodV23", "messages.dtsod"));
        var a = GC.GetTotalMemory(true);
        var dtsods = new DtsodV23[64];
        for (int i = 0; i < dtsods.Length; i++)
            dtsods[i] = new(text);
        var b = GC.GetTotalMemory(true);
        ColoredConsole.WriteLine("b", "at the start: ","c",$"{a/1024} kb\n", 
            "b", "at the end: ", "c", $"{b / 1024} kb\n{dtsods.Count()}","b"," dtsods initialized");
        ColoredConsole.WriteLine("g", "test completed");
    }
}
