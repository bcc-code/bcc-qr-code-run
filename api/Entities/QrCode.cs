using System.Text.Json.Serialization;

namespace api.Repositories;

/// 
public class QrCode
{
    /// <summary>Uniquely identifies this specific QR Code</summary>
    public int QrCodeId { get; init; }

    /// <summary>Group - for example post - only one QR code from each group can be scanned</summary>
    public int GroupId { get; init; }

    public FunFact FunFact { get; init; }
    public int Points { get; init; }
    public bool IsSecret { get; init; }
}

public record FunFact(string Title, string Content)
{
    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public int FunFactId { get; set; }
}