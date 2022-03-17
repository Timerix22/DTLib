namespace DTLib.Dtsod;

public static class DtsodFunctions
{
    public static IDtsod ConvertVersion(IDtsod src, DtsodVersion targetVersion)
        => targetVersion switch
        {
            DtsodVersion.V21 => new DtsodV21(src.ToDictionary()),
            DtsodVersion.V22 => throw new NotImplementedException("DtsodV22 is deprecated"),
            DtsodVersion.V23 => new DtsodV23(src.ToDictionary()),
#if DEBUG
            DtsodVersion.V30 => new DtsodV30(src.ToDictionary()),
#endif
            _ => throw new Exception($"DtsodFunctions.Convert() error: unknown target version <{targetVersion}>"),
        };

    // заменяет дефолтные значения на пользовательские
    public static DtsodV23 UpdateByDefault(DtsodV23 old, DtsodV23 updatedDefault)
    {
        DtsodV23 updated = new();
        foreach (KeyValuePair<string,dynamic> p in updatedDefault)
        {
            if (old.TryGetValue(p.Key, out var oldValue))
            {
                if (oldValue.GetType() != p.Value.GetType())
                    throw new Exception(
                        "uncompatible config value type\n  " +
                        $"launcher.dtsod: {p.Key}:{oldValue} is {oldValue.GetType()}, " +
                        $"must be {p.Value.GetType()}");
                else updated.Add(p.Key,oldValue);
            }
            else updated.Add(p.Key,p.Value);
        }

        return updated;
    }
}
