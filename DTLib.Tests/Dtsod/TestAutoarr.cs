using DTLib.Dtsod.V24.Autoarr;
using DTLib.Dtsod.V24.KerepTypes;

namespace DTLib.Tests;

public static class TestAutoarr
{
    public static void TestAll()
    {
        var ar = new Autoarr<KVPair>(4, 4, false);
        Fill(ar);
        Print(ar);
        Free(ar);
    }

    public static void Fill(Autoarr<KVPair> ar)
    {
        OldLogger.Log("c", "----------[TestAutoarr/Fill]----------");
        for(uint i=0;i<ar.MaxLength;i++)
            ar.Add(new KVPair($"key_{i}",new Unitype(i)));
        OldLogger.Log("g", "test completed");
    }

    public static void Print(Autoarr<KVPair> ar)
    {
        OldLogger.Log("c", "----------[TestAutoarr/Print]---------");
        foreach (KVPair pair in ar) 
            OldLogger.Log("h", pair.ToString());
        OldLogger.Log("g", "test completed");
    }

    public static void Free(Autoarr<KVPair> ar)
    {
        OldLogger.Log("c", "----------[TestAutoarr/Free]----------");
        ar.Dispose();
        OldLogger.Log("g", "test completed");
    }
}