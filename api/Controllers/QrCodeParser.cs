using System.Text.Json;

namespace api.Controllers;

public static class QrCodeParser
{
    public static QrCodeData? Parse(QrCodeScanned request)
    {
        try
        {
            var jsonData = Convert.FromBase64String(request.Data);
            return JsonSerializer.Deserialize<QrCodeData>(jsonData);
        }
        catch (Exception)
        {
            return null;
        }
    }
}

public record QrCodeData(int QrCodeId);