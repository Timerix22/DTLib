global using DtsodPtr=System.IntPtr;
global using AutoarrKVPairPtr=System.IntPtr;
global using AutoarrUnitypePtr=System.IntPtr;
global using CharPtr=System.IntPtr;
using System.Runtime.InteropServices;
using DTLib.Dtsod.V24.KerepTypes;

namespace DTLib.Dtsod.V24;

internal static class DtsodV24Functions
{
    private const string kereplib = "kerep";
    
    static DtsodV24Functions()
    {
        DependencyResolver.CopyLibs();
    }
    
    static void TryThrowErrmsg(CharPtr err)
    {
        if (err == IntPtr.Zero) return;
        string errmsg = Unmanaged.HGlobalUTF8ToString(err);
        Marshal.FreeHGlobal(err);
        throw new Exception(errmsg);
    }
    
    
    //parses text to binary values
    [DllImport(kereplib, CallingConvention = CallingConvention.Cdecl)]
    static extern void kerep_DtsodV24_deserialize(string text, out DtsodPtr output, out CharPtr errmsg);
    internal static DtsodPtr Deserialize(string text)
    {
        kerep_DtsodV24_deserialize(text, out var dtsod,out var err);
        TryThrowErrmsg(err);
        return dtsod;
    }

    //creates text representation of dtsod
    [DllImport(kereplib, CallingConvention = CallingConvention.Cdecl)]
    static extern void kerep_DtsodV24_serialize(DtsodPtr dtsod, out CharPtr output, out CharPtr errmsg);
    internal static string Serialize(DtsodPtr dtsod)
    {
        kerep_DtsodV24_serialize(dtsod, out var text, out var err);
        TryThrowErrmsg(err);
        return Unmanaged.HGlobalUTF8ToString(text);
    }

    [DllImport(kereplib, CallingConvention = CallingConvention.Cdecl)]
    static extern void kerep_DtsodV24_get(DtsodPtr dtsod, string key, out Unitype output);
    //returns value or UniNull if key not found
    internal static Unitype Get(DtsodPtr dtsod, string key)
    {
        kerep_DtsodV24_get(dtsod, key, out var output);
        return output;
    }

    [DllImport(kereplib,EntryPoint = "kerep_DtsodV24_addOrSet",CallingConvention = CallingConvention.Cdecl)]
    //adds or sets value
    static extern void kerep_DtsodV24_addOrSet(DtsodPtr dtsod, IntPtr key, Unitype value);

    internal static void AddOrSet(DtsodPtr dtsod, string key, Unitype value)
    {
        IntPtr keyptr = key.StringToHGlobalUTF8();
        kerep_DtsodV24_addOrSet(dtsod, keyptr, value);
    }

    [DllImport(kereplib, CallingConvention = CallingConvention.Cdecl)]
    //checks for dtsod contains value or dont
    static extern void kerep_DtsodV24_contains(DtsodPtr dtsod, string key, [MarshalAs(UnmanagedType.I1)] out bool output);
    internal static bool Contains(DtsodPtr dtsod, string key)
    {
        kerep_DtsodV24_contains(dtsod, key, out var output);
        return output;
    }

    [DllImport(kereplib, CallingConvention = CallingConvention.Cdecl)]
    static extern void kerep_DtsodV24_remove(DtsodPtr dtsod, string key, [MarshalAs(UnmanagedType.I1)] out bool output);
    //replaces value with UniNull if key exists in dtsod
    internal static bool Remove(DtsodPtr dtsod, string key)
    {
        kerep_DtsodV24_remove(dtsod, key, out var output);
        return output;
    }
    
    [DllImport(kereplib,EntryPoint="kerep_DtsodV24_free", CallingConvention = CallingConvention.Cdecl)]
    //replaces value with UniNull if key exists in dtsod
    internal static extern void Free(DtsodPtr dtsod);

    [DllImport(kereplib, CallingConvention = CallingConvention.Cdecl)]
    static extern void kerep_DtsodV24_height(DtsodPtr dtsod, out ushort heigth);
    //returns current amounts of rows (Autoarrs of KVPairs) in hashtable
    internal static ushort Height(DtsodPtr ptr)
    {
        kerep_DtsodV24_height(ptr, out var h);
        return h;
    }
    
    [DllImport(kereplib, CallingConvention = CallingConvention.Cdecl)]
    static extern void kerep_DtsodV24_getrow(DtsodPtr dtsod, ushort h, out AutoarrKVPairPtr row);
    //Returns row from hashtable.
    //check current hashtable height before calling this.
    internal static AutoarrKVPairPtr GetRow(DtsodPtr ptr, ushort height)
    {
        kerep_DtsodV24_getrow(ptr, height, out var rowptr);
        return rowptr;
    }
}