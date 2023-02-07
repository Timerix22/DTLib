#if NETSTANDARD2_1 || NET6_0 || NET7_0 || NET8_0
namespace DTLib.Extensions;

public static class SpanHelper
{
    public static ReadOnlySpan<T> After<T>(this ReadOnlySpan<T> span, T c) where T : IEquatable<T>
    {
        var index = span.IndexOf(c);
        if (index == -1)
            throw new SpanHelperException<T>(c, span);
        return span.Slice(index+1);
    }
    
    public static ReadOnlySpan<T> After<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> s) where T : IEquatable<T>
    {
        var index = span.IndexOf(s);
        if (index == -1)
            throw new SpanHelperException<T>(s, span);
        return span.Slice(index+s.Length);
    }
    
    
    public static ReadOnlySpan<T> AfterLast<T>(this ReadOnlySpan<T> span, T c) where T : IEquatable<T>
    {
        var index = span.LastIndexOf(c);
        if (index == -1)
            throw new SpanHelperException<T>(c, span);
        return span.Slice(index+1);
    }
    
    public static ReadOnlySpan<T> AfterLast<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> s) where T : IEquatable<T>
    {
        var index = span.LastIndexOf(s);
        if (index == -1)
            throw new SpanHelperException<T>(s, span);
        return span.Slice(index+s.Length);
    }
    
    
    public static ReadOnlySpan<T> Before<T>(this ReadOnlySpan<T> span, T c) where T : IEquatable<T>
    {
        var index = span.IndexOf(c);
        if (index == -1)
            throw new SpanHelperException<T>(c, span);
        return span.Slice(0,index);
    }
    
    public static ReadOnlySpan<T> Before<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> s) where T : IEquatable<T>
    {
        var index = span.IndexOf(s);
        if (index == -1)
            throw new SpanHelperException<T>(s, span);
        return span.Slice(0,index);
    }
    
    
    public static ReadOnlySpan<T> BeforeLast<T>(this ReadOnlySpan<T> span, T c) where T : IEquatable<T>
    {
        var index = span.LastIndexOf(c);
        if (index == -1)
            throw new SpanHelperException<T>(c, span);
        return span.Slice(0,index);
    }
    
    public static ReadOnlySpan<T> BeforeLast<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> s) where T : IEquatable<T>
    {
        var index = span.LastIndexOf(s);
        if (index == -1)
            throw new SpanHelperException<T>(s, span);
        return span.Slice(0,index);
    }
    
    
    public class SpanHelperException<T> : Exception
    {
        public SpanHelperException(T el, ReadOnlySpan<T> span)
            : base($"{nameof(T)} {el} not found in span {span.ToString()}")
        {}
        public SpanHelperException(ReadOnlySpan<T> el, ReadOnlySpan<T> span)
            : base($"ReadOnlySpan<{nameof(T)}> {el.ToString()} not found in span {span.ToString()}")
        {}
    }
}
#endif