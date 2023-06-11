/*
using System.Runtime.InteropServices;
using KerepWrapper.Dtsod;
using KerepWrapper.KerepTypes;

namespace DTLib.Tests;

public static class TestPInvoke
{
    public static void TestAll()
    {
        DependencyResolver.CopyLibs();
        TestUTF8();
        TestPrintf();
        TestMarshalling();
    }

    public static void TestUTF8()
    {
        Logger.Log("c", "--------[TestPInvoke/TestUTF8]--------", "b", "");
        IntPtr ptr;
        string str="_$\"\\\\'''\ta ыыы000;2;=:%d;```";
        for(int i=0; i<1000; i++)
        {
            ptr = Unmanaged.StringToHGlobalUTF8(str);
            str = Unmanaged.HGlobalUTF8ToString(ptr);
        }
        Logger.Log("y", str);
    }
    
    [DllImport("kerep", CallingConvention = CallingConvention.Cdecl)]
    static extern void pinvoke_print([MarshalAs(UnmanagedType.LPStr)] string msg);

    public static void TestPrintf()
    {
        Logger.Log("c", "---------[TestPInvoke/Printf]---------", "b", "");
        pinvoke_print("ъъ~ 中文");
        Logger.Log("g", "test completed");
    }
    
    [DllImport("kerep", CallingConvention = CallingConvention.Cdecl)]
    static extern unsafe void test_marshalling([MarshalAs(UnmanagedType.LPStr)] string text, out IntPtr kptr);
    
    public static unsafe void TestMarshalling()
    {
        Logger.Log("c", "---------[TestAutoarr/TestMarshalling]----------");
        string msg = "ъъ~ 中文";
        test_marshalling(msg, out var kptr);
        KVPair k = *(KVPair*)kptr;
        Logger.Log("b", k.ToString());
        Logger.Log("g", "test completed");
    }
}
*/