using System.Runtime.InteropServices;
using DTLib.Dtsod.V24;
using DTLib.Dtsod.V24.KerepTypes;

namespace DTLib.Tests;

public static class TestPInvoke
{
    public static void TestAll()
    {
        DependencyResolver.CopyLibs();
        TestPrintf();
        TestMarshalling();
    }
    
    
    [DllImport("kerep", CallingConvention = CallingConvention.Cdecl)]
    static extern void pinvoke_print([MarshalAs(UnmanagedType.LPUTF8Str)] string msg);

    public static void TestPrintf()
    {
        Info.Log("c", "---------[TestPInvoke/Printf]---------");
        pinvoke_print("ъъ~ 中文");
        Info.Log("g", "test completed");
    }
    
    [DllImport("kerep", CallingConvention = CallingConvention.Cdecl)]
    static extern unsafe void test_marshalling([MarshalAs(UnmanagedType.LPUTF8Str)] string text, out KVPair* k);
    
    public static unsafe void TestMarshalling()
    {
        Info.Log("c", "---------[TestAutoarr/Print]----------");
        string msg = "ъъ~ 中文";
        test_marshalling(msg, out KVPair* kptr);
        KVPair k = *kptr;
        Info.Log("b", k.ToString());
        Info.Log("g", "test completed");
    }
}