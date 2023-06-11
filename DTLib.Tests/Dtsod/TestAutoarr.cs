/*
using KerepWrapper.Autoarr;
using KerepWrapper.KerepTypes;

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
        Logger.Log("c", "----------[TestAutoarr/Fill]----------");
        for(uint i=0;i<ar.MaxLength;i++)
            ar.Add(new KVPair($"key_{i}",new Unitype(i)));
        Logger.Log("g", "test completed");
    }

    public static void Print(Autoarr<KVPair> ar)
    {
        Logger.Log("c", "----------[TestAutoarr/Print]---------");
        foreach (KVPair pair in ar) 
            Logger.Log("h", pair.ToString());
        Logger.Log("g", "test completed");
    }

    public static void Free(Autoarr<KVPair> ar)
    {
        Logger.Log("c", "----------[TestAutoarr/Free]----------");
        ar.Dispose();
        Logger.Log("g", "test completed");
    }
}
*/