namespace DTLib.Filesystem;

//
// некоторые старые методы, которые хорошо бы вырезать
//
public static class OldFilework
{
    // чтение параметров из конфига
    public static string ReadFromConfig(string configfile, string key)
    {
        lock (new object())
        {
            key += ": ";
            using var reader = new System.IO.StreamReader(configfile);
            while (!reader.EndOfStream)
            {
                string st = reader.ReadLine();
                if (st.StartsWith(key))
                {
                    string value = "";
                    for (int i = key.Length; i < st.Length; i++)
                    {
                        if (st[i] == '#')
                            return value;
                        if (st[i] == '%')
                        {
                            bool stop = false;
                            string placeholder = "";
                            i++;
                            while (!stop)
                            {
                                if (st[i] == '%')
                                {
                                    stop = true;
                                    value += ReadFromConfig(configfile, placeholder);
                                }
                                else
                                {
                                    placeholder += st[i];
                                    i++;
                                }
                            }
                        }
                        else
                            value += st[i];
                    }
                    reader.Close();
                    //if (value.NullOrEmpty()) throw new System.Exception($"ReadFromConfig({configfile}, {key}) error: key not found");
                    return value;
                }
            }
            reader.Close();
            throw new Exception($"ReadFromConfig({configfile}, {key}) error: key not found");
        }
    }
}
