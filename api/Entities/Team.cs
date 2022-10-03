using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using api.Data;
using api.Controllers;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class Team
{
    private readonly DataContext _dataContext;

    public Team(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public int Id { get; set; }
    
    public string TeamName { get; init; }
    public string ChurchName { get; init; }
    public int Members { get; init; }

    [JsonIgnore]
    public List<Score> QrCodesScanned { get; set; } = new();

    [NotMapped]
    public IEnumerable<Score> Posts => QrCodesScanned.Where(x=>x.IsSecret == false);
    
    [NotMapped]
    public IEnumerable<Score> SecretsFound => QrCodesScanned.Where(x => x.IsSecret);
    

    public int Score => QrCodesScanned.Sum(x => x.Points);

    public DateTime? FirstScannedQrCode { get; set; }
    public DateTime? LastScannedQrCode { get; set; }
    [JsonConverter(typeof(SimpleTimespanConverter))]
    public TimeSpan? TimeSpent => LastScannedQrCode - FirstScannedQrCode;


    public async Task<bool> AddQrCodeAsync(QrCode qrCode)
    {
        if (QrCodesScanned.Any(x => x.Id == qrCode.GroupId))
            return false;
        
        _dataContext.Add(new Score(qrCode.GroupId, qrCode.Points, qrCode.IsSecret)
        {
            TeamId = Id,
        });
        
        FirstScannedQrCode ??= DateTime.UtcNow;
        LastScannedQrCode = DateTime.UtcNow;

        await _dataContext.SaveChangesAsync();

        return true;
    }
}

public record Score(int Id, int Points, bool IsSecret)
{
    public int TeamId { get; set; }
    [JsonIgnore]
    public Team Team { get; set; }
};


public class SimpleTimespanConverter : JsonConverter<TimeSpan>
{
    /// <inheritdoc />
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return TimeSpan.ParseExact(reader.GetString() ?? "", @"hh\:mm", CultureInfo.InvariantCulture);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(@"hh\:mm"));
    }
}
