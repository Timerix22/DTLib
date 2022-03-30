namespace DTLib.Dtsod.V24.Autoarr;

public class Autoarr<T> : IEnumerable<T>, IDisposable where T : struct
{
    private readonly IntPtr AutoarrHandler;
    private readonly AutoarrFunctions<T> Funcs;
    public readonly uint MaxLength;
    //if true, destructor frees allocated unmanaged memory
    public bool AutoDispose;

    public Autoarr(IntPtr ptr, bool autoDispose=true)
    {
        AutoDispose = autoDispose;
        Funcs = AutoarrFunctions<T>.GetFunctions();
        MaxLength = Funcs.MaxLength(AutoarrHandler);
        Length = Funcs.Length(AutoarrHandler);
        AutoarrHandler = ptr;
    }

    public Autoarr(ushort blockCount, ushort blockLength, bool autoDispose=true) : this(IntPtr.Zero,autoDispose)
    {
        AutoarrHandler = Funcs.Create(blockCount, blockLength);
    }

    public uint Length { get; private set; }

    public T this[uint i]
    {
        get
        {
            if (i < Length) return Funcs.Get(AutoarrHandler, i);
            throw new IndexOutOfRangeException($"index {i} >= Autoarr.Length {Length}");
        }
        set
        {
            if (i < Length) Funcs.Set(AutoarrHandler, i, value);
            else throw new IndexOutOfRangeException($"index {i} >= Autoarr.Length {Length}");
        }
    }

    public void Dispose()
    {
        Funcs.Free(AutoarrHandler);
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
        if (Length++ < MaxLength - 1) Funcs.Add(AutoarrHandler, value);
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
            var r = ++index < arr.Length;
            if (r) Current = arr[index];
            return r;
        }

        public void Reset()
        {
            index = 0;
        }
    }
}