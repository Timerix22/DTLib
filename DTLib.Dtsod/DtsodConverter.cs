namespace DTLib.Dtsod;

public static class DtsodConverter
{
    public static IDtsod ConvertVersion(IDtsod src, DtsodVersion targetVersion)
        => targetVersion switch
        {
            DtsodVersion.V21 => new DtsodV21(src.ToDictionary()),
            DtsodVersion.V22 => throw new Exception("DtsodV22 is deprecated"),
            DtsodVersion.V23 => new DtsodV23(src.ToDictionary()),
            // DtsodVersion.V24 => new DtsodV24(src.ToDictionary()),
#if DEBUG
            //DtsodVersion.V30 => new DtsodV30(src.ToDictionary()),
#endif
            _ => throw new Exception($"DtsodConverter.Convert() error: unknown target version <{targetVersion}>"),
        };

    // заменяет дефолтные значения на пользовательские
    public static DtsodV23 UpdateByDefault(DtsodV23 old, DtsodV23 updatedDefault, string contextName = "")
    {
        DtsodV23 updated = new();
        foreach (KeyValuePair<string, dynamic> p in updatedDefault)
        {
            string keyWithContext = contextName + "." + p.Key;
            if (old.TryGetValue(p.Key, out var oldValue))
            {
                if (oldValue.GetType() != p.Value.GetType())
                    throw new Exception(
                        "uncompatible config value type\n  " +
                        $"<{keyWithContext}>: {oldValue} is {oldValue.GetType()}, " +
                        $"must be {p.Value.GetType()}");
                if (oldValue is DtsodV23)
                {
                    var subdtsod = UpdateByDefault(oldValue, p.Value, keyWithContext);
                    updated.Add(p.Key, subdtsod);
                }
                else if (oldValue is IList)
                    updated.Add(p.Key, oldValue);
                else updated.Add(p.Key, oldValue);
            }
            else updated.Add(p.Key, p.Value);
        }

        return updated;
    }
}