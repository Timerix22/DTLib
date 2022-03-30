global using DtsodPtr=System.IntPtr;
global using AutoarrKVPairPtr=System.IntPtr;
global using AutoarrUnitypePtr=System.IntPtr;
using System.Runtime.InteropServices;
using DTLib.Dtsod.V24.Autoarr;

namespace DTLib.Dtsod.V24;

public static unsafe class DtsodV24Functions
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

    [DllImport("kerep.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern void kerep_DtsodV24_get(DtsodPtr dtsod, string key, out Unitype output);
    //returns value or UniNull if key not found
    internal static Unitype Get(DtsodPtr dtsod, string key)
    {
        kerep_DtsodV24_get(dtsod, key, out var output);
        return output;
    }

    [DllImport("kerep.dll",EntryPoint = "kerep_DtsodV24_addOrSet",CallingConvention = CallingConvention.Cdecl)]
    //adds or sets value
    internal static extern void AddOrSet(DtsodPtr dtsod, string key, Unitype value);

    [DllImport("kerep.dll", CallingConvention = CallingConvention.Cdecl)]
    //checks for dtsod contains value or dont
    static extern void kerep_DtsodV24_contains(DtsodPtr dtsod, string key, [MarshalAs(UnmanagedType.I1)] out bool output);
    internal static bool Contains(DtsodPtr dtsod, string key)
    {
        kerep_DtsodV24_contains(dtsod, key, out var output);
        return output;
    }

    [DllImport("kerep.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern void kerep_DtsodV24_remove(DtsodPtr dtsod, string key, [MarshalAs(UnmanagedType.I1)] out bool output);
    //replaces value with UniNull if key exists in dtsod
    internal static bool Remove(DtsodPtr dtsod, string key)
    {
        kerep_DtsodV24_remove(dtsod, key, out var output);
        return output;
    }
    
    [DllImport("kerep.dll",EntryPoint="kerep_DtsodV24_free", CallingConvention = CallingConvention.Cdecl)]
    //replaces value with UniNull if key exists in dtsod
    internal static extern void Free(DtsodPtr dtsod);

    [DllImport("kerep.dll",EntryPoint="kerep_DtsodV24_free", CallingConvention = CallingConvention.Cdecl)]
    static extern void kerep_DtsodV24_height(DtsodPtr dtsod, out ushort heigth);
    //returns current amounts of rows (Autoarrs of KVPairs) in hashtable
    internal static ushort Height(DtsodPtr ptr)
    {
        kerep_DtsodV24_height(ptr, out var h);
        return h;
    }
    
    [DllImport("kerep.dll",EntryPoint="kerep_DtsodV24_free", CallingConvention = CallingConvention.Cdecl)]
    static extern void kerep_DtsodV24_getrow(DtsodPtr dtsod, ushort h, out AutoarrKVPairPtr row);
    //Returns row from hashtable.
    //check current hashtable height before calling this.
    internal static AutoarrKVPairPtr GetRow(DtsodPtr ptr, ushort height)
    {
        kerep_DtsodV24_getrow(ptr, height, out var rowptr);
        return rowptr;
    }
}