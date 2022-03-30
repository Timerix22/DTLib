using System.Runtime.InteropServices;

namespace DTLib.Dtsod.V24.Autoarr;

internal class AutoarrKVPairFunctions : AutoarrFunctions<KVPair>
{
     [DllImport("kerep.dll", CallingConvention = CallingConvention.Cdecl)]
     static extern void kerep_Autoarr_KVPair_create(ushort max_blocks_count, ushort max_block_length, out AutoarrKVPairPtr output);
     internal override AutoarrKVPairPtr Create(ushort maxBlocksCount, ushort maxBlockLength)
     {
          kerep_Autoarr_KVPair_create(maxBlocksCount, maxBlockLength, out var ar);
          return ar;
     }

     [DllImport("kerep.dll", CallingConvention = CallingConvention.Cdecl)]
     internal static extern void kerep_Autoarr_KVPair_free(AutoarrKVPairPtr ar);
     internal override void Free(AutoarrKVPairPtr ar) => kerep_Autoarr_KVPair_free(ar);

     [DllImport("kerep.dll", CallingConvention = CallingConvention.Cdecl)]
     static extern void kerep_Autoarr_KVPair_get(AutoarrKVPairPtr ar, uint index, out KVPair output);
     internal override KVPair Get(AutoarrKVPairPtr ar, uint index)
     {
          kerep_Autoarr_KVPair_get(ar, index, out var output);
          return output;
     }

     [DllImport("kerep.dll",CallingConvention = CallingConvention.Cdecl)]
     internal static extern void kerep_Autoarr_KVPair_add(AutoarrKVPairPtr ar, KVPair element);
     internal override void Add(AutoarrKVPairPtr ar, KVPair element) => kerep_Autoarr_KVPair_add(ar, element);

     [DllImport("kerep.dll", CallingConvention = CallingConvention.Cdecl)]
     internal static extern void kerep_Autoarr_KVPair_set(AutoarrKVPairPtr ar, uint index, KVPair element);
     internal override void Set(AutoarrKVPairPtr ar, uint index, KVPair element) =>
          kerep_Autoarr_KVPair_set(ar, index, element);

     [DllImport("kerep.dll", CallingConvention = CallingConvention.Cdecl)]
     static extern void kerep_Autoarr_KVPair_length(AutoarrKVPairPtr ar, out uint output);
     internal override uint Length(AutoarrKVPairPtr ar)
     {
          kerep_Autoarr_KVPair_length(ar, out var l);
          return l;
     }

     [DllImport("kerep.dll", CallingConvention = CallingConvention.Cdecl)]
     static extern void kerep_Autoarr_KVPair_max_length(AutoarrKVPairPtr ar, out uint output);
     internal override uint MaxLength(AutoarrKVPairPtr ar)
     {
          kerep_Autoarr_KVPair_max_length(ar, out var l);
          return l;
     }
}