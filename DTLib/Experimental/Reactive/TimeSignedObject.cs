namespace DTLib.Experimental.Reactive
{
    public class TimeSignedObject<T>
    {
        public T Value { get; init; }
        public long Time { get; init; }

        public TimeSignedObject(T value)
        {
            Value = value;
            Time = DateTime.Now.Ticks;
        }
    }
}
