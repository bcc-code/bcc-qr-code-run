using api.Features;

namespace api.Repositories;

public class QrCodeRepository
{
    public async Task<QrCode?> GetQrCode(QrCodeData qrCodeData)
    {
        return new QrCode(qrCodeData.QrCodeId, 1, new FunFact("Test", "Lorem Ipsum"), 3, false);
    }
}

/// <param name="QrCodeId">Uniquely identifies this specific QR Code</param>
/// <param name="Id">Group - for example post - only one QR code from each group can be scanned</param>
public record QrCode(int QrCodeId, int Id, FunFact FunFact, int Points, bool IsSecret);

public record FunFact(string Title, string Content);