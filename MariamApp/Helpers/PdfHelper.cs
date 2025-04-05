using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace MariamApp.Helpers;

public static class PdfGenerator
{
    public static byte[] GeneratePdf<T>(string title, List<T> records)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);

                page.Content().Column(column =>
                {
                    column.Spacing(10);

                    column.Item().Text(title)
                        .FontSize(24)
                        .Bold()
                        .Underline();

                    column.Item().Text($"Date: {DateTime.Now:yyyy-MM-dd}");

                    // Add a simple table
                    column.Item().Table(table =>
                    {
                        var properties = typeof(T).GetProperties()
                            .OrderBy(p => p.Name) 
                            .ToList();
                        
                        table.ColumnsDefinition(columns =>
                        {
                            foreach (var prop in properties)
                            {
                                columns.RelativeColumn();  
                            }
                        });


                        table.Header(header =>
                        {
                            foreach (var prop in properties)
                            {
                                header.Cell()
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Grey.Medium)
                                    .Padding(5)
                                    .Text(prop.Name)
                                    .SemiBold()
                                    .FontSize(12);
                            }
                        });

                        foreach (var record in records)
                        {
                            foreach (var prop in properties) 
                            {
                                table.Cell()
                                    .Padding(15)
                                    .Text(prop.GetValue(record)?.ToString() ?? "")
                                    .FontSize(10);
                            }
                        }
                    });
                });
                page.Footer()
                    .AlignRight()
                    .Text($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
                    .FontSize(10)
                    .Italic()
                    .FontColor(Colors.Grey.Darken1);
            });
        }).GeneratePdf();
    }

   
}
