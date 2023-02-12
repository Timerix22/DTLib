using System.Runtime.InteropServices;
using KerepWrapper.KerepTypes;

namespace KerepWrapper.Autoarr;

internal class AutoarrUnitypeFunctions : AutoarrFunctions<Unitype>
{
     [DllImport("kerep", CallingConvention = CallingConvention.Cdecl)]
     static extern void kerep_Autoarr_Unitype_create(ushort max_blocks_count, ushort max_block_length, out AutoarrUnitypePtr output);
     internal override AutoarrUnitypePtr Create(ushort maxBlocksCount, ushort maxBlockLength)
     {
          kerep_Autoarr_Unitype_create(maxBlocksCount, maxBlockLength, out var ar);
          return ar;
     }

     [DllImport("kerep", CallingConvention = CallingConvention.Cdecl)]
     internal static extern void kerep_Autoarr_Unitype_free(AutoarrUnitypePtr ar);
     internal override void Free(AutoarrUnitypePtr ar) => kerep_Autoarr_Unitype_free(ar);

     [DllImport("kerep", CallingConvention = CallingConvention.Cdecl)]
     static extern void kerep_Autoarr_Unitype_get(AutoarrUnitypePtr ar, uint index, out Unitype output);
     internal override Unitype Get(AutoarrUnitypePtr ar, uint index)
     {
          kerep_Autoarr_Unitype_get(ar, index, out var output);
          return output;
     }

     [DllImport("kerep",CallingConvention = CallingConvention.Cdecl)]
     internal static extern void kerep_Autoarr_Unitype_add(AutoarrUnitypePtr ar, Unitype element);
     internal override void Add(AutoarrUnitypePtr ar, Unitype element) => kerep_Autoarr_Unitype_add(ar, element);

     [DllImport("kerep", CallingConvention = CallingConvention.Cdecl)]
     internal static extern void kerep_Autoarr_Unitype_set(AutoarrUnitypePtr ar, uint index, Unitype element);
     internal override void Set(AutoarrUnitypePtr ar, uint index, Unitype element) =>
          kerep_Autoarr_Unitype_set(ar, index, element);

     [DllImport("kerep", CallingConvention = CallingConvention.Cdecl)]
     static extern void kerep_Autoarr_Unitype_length(AutoarrUnitypePtr ar, out uint output);
     internal override uint Length(AutoarrUnitypePtr ar)
     {
          kerep_Autoarr_Unitype_length(ar, out var l);
          return l;
     }

     [DllImport("kerep", CallingConvention = CallingConvention.Cdecl)]
     static extern void kerep_Autoarr_Unitype_max_length(AutoarrUnitypePtr ar, out uint output);
     internal override uint MaxLength(AutoarrUnitypePtr ar)
     {
          kerep_Autoarr_Unitype_max_length(ar, out var l);
          return l;
     }
}