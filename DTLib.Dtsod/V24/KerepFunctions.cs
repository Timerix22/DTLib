using System.Runtime.InteropServices;
using DtsodPtr=System.IntPtr;

namespace DTLib.Dtsod.V24;

public static unsafe class KerepFunctions
{
    [DllImport("kerep.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern void pinvoke_print(string msg);
    [DllImport("kerep.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern void test_marshalling(string text, out KVPair* k);
    public static void TestMarshalling()
    {
        pinvoke_print("UwU");
        string msg = "hello!";
        test_marshalling(msg, out KVPair* kptr);
        Log("kptr: " + kptr->ToString());
        KVPair k = *kptr;
        Log("y",
            $"{{{Marshal.PtrToStringAnsi(k.key)}, {{{k.value.type.ToString()}, {Marshal.PtrToStringAnsi(k.value.VoidPtr)} }} }}");
    }
    
    
    //parses text to binary values
    [DllImport("kerep.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern void kerep_DtsodV24_deserialize(string text, out DtsodPtr output);
    internal static DtsodPtr Deserialize(string text)
    {
        kerep_DtsodV24_deserialize(text, out var dtsod);
        return dtsod;
    }

    //creates text representation of dtsod
    [DllImport("kerep.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern void kerep_DtsodV24_serialize(DtsodPtr dtsod, out string output);
    internal static string Serialize(DtsodPtr dtsod)
    {
        kerep_DtsodV24_serialize(dtsod, out var text);
        return text;
    }

    //returns value or UniNull if key not found
    [DllImport("kerep.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern void kerep_DtsodV24_get(DtsodPtr dtsod, string key, out Unitype output);

    internal static Unitype Get(DtsodPtr dtsod, string key)
    {
        kerep_DtsodV24_get(dtsod, key, out var output);
        return output;
    }

    //adds or sets value
    [DllImport("kerep.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern void kerep_DtsodV24_addOrSet(DtsodPtr dtsod, string key, Unitype value);
    internal static void AddOrSet(DtsodPtr dtsod, string key, Unitype value)
    {
        kerep_DtsodV24_addOrSet(dtsod, key, value);
    }

    //checks for dtsod contains value or dont
    [DllImport("kerep.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern void kerep_DtsodV24_contains(DtsodPtr dtsod, string key, [MarshalAs(UnmanagedType.I1)] out bool output);
    internal static bool Contains(DtsodPtr dtsod, string key)
    {
        kerep_DtsodV24_contains(dtsod, key, out var output);
        return output;
    }

    //replaces value with UniNull if key exists in dtsod
    [DllImport("kerep.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern void kerep_DtsodV24_remove(DtsodPtr dtsod, string key, [MarshalAs(UnmanagedType.I1)] out bool output);
    internal static bool Remove(DtsodPtr dtsod, string key)
    {
        kerep_DtsodV24_remove(dtsod, key, out var output);
        return output;
    }
}