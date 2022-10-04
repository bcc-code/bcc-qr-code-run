namespace LoadTester;

public static class RandomGenerator
{
    private static readonly string[] Suffixes = new []
    {
        "berg",
        "borg",
        "by",
        "bø",
        "dal",
        "eid",
        "fjell",
        "fjord",
        "foss",
        "grunn",
        "hamn",
        "havn",
        "helle",
        "mark",
        "nes",
        "odden",
        "sand",
        "sjøen",
        "stad",
        "strand",
        "strøm",
        "sund",
        "vik",
        "vær",
        "våg",
        "ø",
        "øy",
        "ås",
    };

    private static readonly string[] Prefixes = new[]
    {
        "Fet",
        "Gjes",
        "Høy",
        "Inn",
        "Fager",
        "Lille",
        "Lo",
        "Mal",
        "Nord",
        "Nær",
        "Sand",
        "Sme",
        "Stav",
        "Stor",
        "Tand",
        "Ut",
        "Vest",
        "Ber",
        "Tøn",
        "Har",
        "Tor",
        "Kristian",
        "Aren",
        "Joste",
        "Viks",
        "Nøtter",
        "Hell",
        "Geir",
        "Li",
        "Skarp",
        "Mel",
        "Øst",
        "Vest",
        "Ski",
        "Elve",
        "Eik",
    };

    public static string Church()
    {
        return Prefixes[Random.Shared.Next(Prefixes.Length)] + Suffixes[Random.Shared.Next(Suffixes.Length)];
    }

    public static string TeamName()
    {
        return Guid.NewGuid().ToString();
    }
}