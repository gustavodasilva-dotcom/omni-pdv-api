namespace OmniePDV.API.Options;

public sealed class MongoDBOptions
{
    public const string Position = "MongoDB";

    public string ConnectionString { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
}
