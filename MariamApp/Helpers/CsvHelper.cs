using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace MariamApp.Helpers;

public static class CsvGenerator
{
    public static byte[] GenerateCsv<T>(IEnumerable<T> records)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream);
        using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

        csv.WriteRecords(records);
        writer.Flush();
        return memoryStream.ToArray();
    }
}