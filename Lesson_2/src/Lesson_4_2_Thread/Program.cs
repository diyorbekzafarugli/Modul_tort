using System.Text.Json;

namespace Lesson_4_2_Thread;

internal class Program
{
    static string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
    static JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

    static object _lock = new object();

    static void Main(string[] args)
    {
        for (int i = 0; i < 1000; i++)
        {
            Thread thread = new Thread(CreateFile);
            thread.Start(i);
        }

    }


    static void CreateFile(object number)
    {
        int num = (int)number;
        List<string> strings = [];
        for (int i = 0; i < num; i++)
        {
            lock (_lock)
            {
                strings.Add($"Salom {i} - marta");
                string path = Path.Combine(_filePath, $"{i}.json");
;
                SaveToFile(strings, path);
            }
        }
    }
    static void SaveToFile(List<string> strings, string path)
    {
        var json = JsonSerializer.Serialize(strings, options);
        Directory.CreateDirectory(_filePath);
        File.WriteAllText(path, json);
    }
}
