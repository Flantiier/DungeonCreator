using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public class MultiArraySave
{
    public static void CopyStream(Stream input, Stream output)
    // Helper funtion to copy from one stream to another
    {
        // Magic number is 2^16
        byte[] buffer = new byte[32768];
        int read;
        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
        {
            output.Write(buffer, 0, read);
        }
    }

    public static string Compress(string s)
    {
        var bytes = Encoding.UTF8.GetBytes(s);
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(mso, CompressionMode.Compress))
            {

                CopyStream(msi, gs);
            }
            return Convert.ToBase64String(mso.ToArray());
        }
    }

    public static string Decompress(string s)
    {
        var bytes = Convert.FromBase64String(s);
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(msi, CompressionMode.Decompress))
            {
                CopyStream(gs, mso);
            }
            return Encoding.UTF8.GetString(mso.ToArray());
        }
    }

    public static string GenerateAStringFromAnArray<T>(T[][] collection)
    {
        string s = "";
        // Let's initiate the array!
        collection = new T[200][];
        for (int i = 0; i < 200; i++)
            collection[i] = new T[200];


        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, collection);

        s = Convert.ToBase64String(ms.ToArray());
        s = Compress(s);
        return s;
    }

    public static int[][] LoadAnArrayFromString(string s)
    {
        s = Decompress(s);

        BinaryFormatter bf = new BinaryFormatter();
        Byte[] by = Convert.FromBase64String(s);
        MemoryStream sr = new MemoryStream(by);

        return (int[][])bf.Deserialize(sr);
    }
}
