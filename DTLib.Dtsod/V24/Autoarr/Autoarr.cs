using System.Runtime.InteropServices;

namespace DTLib.Dtsod.V24.Autoarr;

public class Autoarr<T> : IEnumerable<T>, IDisposable where T : struct
{
    public readonly IntPtr UnmanagedPtr;
    //if true, destructor frees allocated unmanaged memory
    public bool AutoDispose;
    private readonly AutoarrFunctions<T> Funcs;
    public readonly uint MaxLength;
    public uint Length { get; private set; }

    public Autoarr(IntPtr ptr, bool autoDispose)
    {
        AutoDispose = autoDispose;
        Funcs = AutoarrFunctions<T>.GetFunctions();
        UnmanagedPtr = ptr;
        MaxLength = Funcs.MaxLength(UnmanagedPtr);
        Length = Funcs.Length(UnmanagedPtr);
    }

    public Autoarr(ushort blockCount, ushort blockLength, bool autoDispose=true)
    {
        AutoDispose = autoDispose;
        Funcs = AutoarrFunctions<T>.GetFunctions();
        UnmanagedPtr = Funcs.Create(blockCount, blockLength);
        MaxLength = Funcs.MaxLength(UnmanagedPtr);
        Length = Funcs.Length(UnmanagedPtr);
    }


    public T this[uint i]
    {
        get
        {
            if (i < Length) return Funcs.Get(UnmanagedPtr, i);
            throw new IndexOutOfRangeException($"index {i} >= Autoarr.Length {Length}");
        }
        set
        {
            if (i < Length) Funcs.Set(UnmanagedPtr, i, value);
            else throw new IndexOutOfRangeException($"index {i} >= Autoarr.Length {Length}");
        }
    }

    public void Dispose()
    {
        Funcs.Free(UnmanagedPtr);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new AutoarrEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T value)
    {
        if (Length < MaxLength)
        {
            Funcs.Add(UnmanagedPtr, value);
            Length++;
        }
        else throw new IndexOutOfRangeException($"Autoarr.Length == MaxLength ({MaxLength})");
    }

    ~Autoarr()
    {
        if (AutoDispose) Dispose();
    }

    private class AutoarrEnumerator : IEnumerator<T>
    {
        private readonly Autoarr<T> arr;
        private uint index;

        public AutoarrEnumerator(Autoarr<T> ar)
        {
            arr = ar;
        }

        public T Current { get; private set; }
        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (index >= arr.Length) return false;
            Current = arr[index];
            index++;
            return true;
        }

        public void Reset()
        {
            index = 0;
        }
    }
}