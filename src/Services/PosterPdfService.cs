using projekt_io.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QRCoder;
using System.Globalization;

namespace projekt_io.Services;

public class PosterPdfService : IPosterPdfService
{
    private readonly IWebHostEnvironment _env;

    public PosterPdfService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public byte[] GenerateLostReportPoster(LostReportDto report, string? ownerEmail, string baseUrl)
    {
        // Zdjęcie
        var webRoot = _env.WebRootPath;
        var photoPath = report?.Animal?.PhotoPath ?? "/uploads/default-animal.png";
        var absolutePhotoPath = Path.Combine(
            webRoot,
            photoPath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString())
        );

        byte[]? imageBytes = null;
        if (File.Exists(absolutePhotoPath))
            imageBytes = File.ReadAllBytes(absolutePhotoPath);

        // Dane
        var lostAt = report.LostAt?.ToString("dd.MM.yyyy") ?? "-";
        var city = string.IsNullOrWhiteSpace(report.Location?.City) ? "-" : report.Location.City;

        var latText = report.Location?.Latitude.ToString("0.000000", CultureInfo.InvariantCulture) ?? "-";
        var lonText = report.Location?.Longitude.ToString("0.000000", CultureInfo.InvariantCulture) ?? "-";

        // URL + QR (DWIE sztuki: strona + mapa)
        var reportUrl = $"{baseUrl}/report/{report.Id}";
        var mapUrl = (report.Location != null)
            ? $"https://www.google.com/maps?q={report.Location.Latitude.ToString(CultureInfo.InvariantCulture)},{report.Location.Longitude.ToString(CultureInfo.InvariantCulture)}"
            : null;

        var reportQr = MakeQrPng(reportUrl);
        var mapQr = MakeQrPng(mapUrl);

        var title = string.IsNullOrWhiteSpace(report.Title) ? "Zaginione zwierzę" : report.Title;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(26); // trochę mniejsze, żeby wszystko weszło
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .PaddingBottom(8)
                    .Column(col =>
                    {
                        col.Item().Text("ZAGINIONE ZWIERZĘ").FontSize(26).SemiBold();
                        col.Item().Text(title).FontSize(16);
                        col.Item().LineHorizontal(1);
                    });

                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    // GÓRA: zdjęcie + dane (BEZ QR mapy tutaj)
                    col.Item().Row(row =>
                    {
                        row.Spacing(14);

                        // LEWA: zdjęcie (mniejsze, by zmieścić QR-y na dole)
                        row.RelativeItem(1)
                           .Height(240)
                           .Border(1)
                           .Padding(6)
                           .AlignMiddle()
                           .AlignCenter()
                           .Element(e =>
                           {
                               if (imageBytes != null)
                                   e.Image(imageBytes).FitArea();
                               else
                                   e.Text("Brak zdjęcia").FontColor(Colors.Grey.Darken2);
                           });

                        // PRAWA: dane
                        row.RelativeItem(1).Column(info =>
                        {
                            info.Spacing(5);

                            info.Item().Text($"Imię: {Cap(report.Animal?.Name)}").FontSize(14);
                            info.Item().Text($"Gatunek: {Cap(report.Animal?.Species)}");
                            info.Item().Text($"Rasa: {Cap(report.Animal?.Breed) ?? "Nie podano"}");
                            info.Item().Text($"Data zaginięcia: {lostAt}");

                            info.Item().PaddingTop(8).Text("Ostatnia znana lokalizacja").SemiBold();
                            info.Item().Text($"Miasto: {city}");
                            info.Item().Text($"Współrzędne: {latText}, {lonText}");

                            info.Item().PaddingTop(8).Text("Opis").SemiBold();
                            info.Item()
                                .Border(1)
                                .Padding(8)
                                .Text(string.IsNullOrWhiteSpace(report.Animal?.Description) ? "-" : report.Animal.Description);

                            info.Item().PaddingTop(8).Text("Kontakt").SemiBold();
                            info.Item().Text($"E-mail: {ownerEmail ?? "-"}");
                        });
                    });

                    // KODY QR (TYLKO TU, DWIE SZTUKI, PODPISANE)
                    col.Item().PaddingTop(6).Text("Kody QR").SemiBold();

                    col.Item().Row(r =>
                    {
                        r.Spacing(18);

                        // QR: strona zgłoszenia
                        r.RelativeItem().Column(c =>
                        {
                            c.Spacing(4);
                            c.Item().Text("Strona zgłoszenia").SemiBold().FontSize(11);

                            if (reportQr != null)
                            {
                                c.Item().Width(130).Height(130).Border(1).Padding(6)
                                    .Image(reportQr).FitArea();
                            }
                            else
                            {
                                c.Item().Text("Brak QR").FontSize(9).FontColor(Colors.Grey.Darken2);
                            }

                            c.Item().Text("Zeskanuj, aby zobaczyć szczegóły zgłoszenia")
                                .FontSize(9)
                                .FontColor(Colors.Grey.Darken2);
                        });

                        // QR: mapa
                        r.RelativeItem().Column(c =>
                        {
                            c.Spacing(4);
                            c.Item().Text("Mapa").SemiBold().FontSize(11);

                            if (mapQr != null)
                            {
                                c.Item().Width(130).Height(130).Border(1).Padding(6)
                                    .Image(mapQr).FitArea();

                                c.Item().Text("Zeskanuj, aby otworzyć lokalizację w mapach")
                                    .FontSize(9)
                                    .FontColor(Colors.Grey.Darken2);
                            }
                            else
                            {
                                c.Item().Text("Brak danych lokalizacji")
                                    .FontSize(9)
                                    .FontColor(Colors.Grey.Darken2);
                            }
                        });
                    });

                    col.Item().PaddingTop(8).LineHorizontal(1);

                    col.Item().Text("Jeśli widziałeś/aś to zwierzę, skontaktuj się z właścicielem i podaj możliwie dokładną lokalizację.")
                        .FontSize(11);
                });

                page.Footer().AlignCenter()
                    .Text($"Wygenerowano: {DateTime.Now:yyyy-MM-dd HH:mm}")
                    .FontSize(9)
                    .FontColor(Colors.Grey.Darken2);
            });
        });

        return document.GeneratePdf();
    }

    private static string? Cap(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        s = s.Trim();
        return char.ToUpper(s[0]) + s.Substring(1);
    }

    private static byte[]? MakeQrPng(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        using var generator = new QRCodeGenerator();
        using var data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        var qr = new PngByteQRCode(data);

        // 8 jest OK do druku; jeśli chcesz mniejszy QR bez utraty jakości:
        // zostaw 8, a zmniejsz rozmiar w PDF (Width/Height)
        return qr.GetGraphic(pixelsPerModule: 8);
    }
}
