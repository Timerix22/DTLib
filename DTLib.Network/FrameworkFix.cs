using System.ComponentModel;

// включает init и record из c# 9.0
#if !NET6_0 && !NET7_0 && !NET8_0
namespace System.Runtime.CompilerServices;

[EditorBrowsable(EditorBrowsableState.Never)]
public class IsExternalInit { }
#endif