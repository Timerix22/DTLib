using System.Runtime.InteropServices;
using DTLib.Dtsod.V24;
using DTLib.Dtsod.V24.KerepTypes;

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

    static public void TestUTF8()
    {
        Info.Log("c", "--------[TestPInvoke/TestUTF8]--------", "b", "");
        IntPtr ptr;
        string str="_$\"\\\\'''\ta ыыы000;2;=:%d;```";
        for(int i=0; i<1000; i++)
        {
            ptr = Unmanaged.StringToHGlobalUTF8(str);
            str = Unmanaged.HGlobalUTF8ToString(ptr);
        }
        Info.Log("y", str);
    }
    
    [DllImport("kerep", CallingConvention = CallingConvention.Cdecl)]
    static extern void pinvoke_print([MarshalAs(UnmanagedType.LPUTF8Str)] string msg);

    public static void TestPrintf()
    {
        Info.Log("c", "---------[TestPInvoke/Printf]---------", "b", "");
        pinvoke_print("ъъ~ 中文");
        Info.Log("g", "test completed");
    }
    
    [DllImport("kerep", CallingConvention = CallingConvention.Cdecl)]
    static extern unsafe void test_marshalling([MarshalAs(UnmanagedType.LPUTF8Str)] string text, out IntPtr kptr);
    
    public static unsafe void TestMarshalling()
    {
        Info.Log("c", "---------[TestAutoarr/TestMarshalling]----------");
        string msg = "ъъ~ 中文";
        test_marshalling(msg, out var kptr);
        KVPair k = *(KVPair*)kptr;
        Info.Log("b", k.ToString());
        Info.Log("g", "test completed");
    }
}