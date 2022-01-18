namespace DTLib.Dtsod;

public static class DtsodVersionConverter
{
    public static IDtsod Convert(IDtsod src, DtsodVersion targetVersion)
        => targetVersion switch
        {
            DtsodVersion.V21 => new DtsodV21(src.ToDictionary()),
            DtsodVersion.V22 => throw new NotImplementedException("Converting dtsods to V22 isn't implemented"),
            DtsodVersion.V23 => new DtsodV23(src.ToDictionary()),
            DtsodVersion.V30 => new DtsodV30(src.ToDictionary()),
            _ => throw new Exception($"DtsodVersionConverter.Convert() error: unknown target version <{targetVersion}>"),
        };
}
