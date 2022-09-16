using System.Text.Json;

namespace api.Features;

public static class QrCodeParser
{
    public static QrCodeData? Parse(QrCodeScanned request)
    {
        var jsonData = Convert.FromBase64String(request.Data);
        return JsonSerializer.Deserialize<QrCodeData>(jsonData);
    }
}

public record QrCodeData(int QrCodeId);