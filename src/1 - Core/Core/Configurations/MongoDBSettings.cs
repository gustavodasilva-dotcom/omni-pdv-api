namespace OmniePDV.Core.Configurations;

public sealed class MongoDBSettings
{
    public const string Position = "MongoDB";

    public string ConnectionString { get; set; }
    public string Database { get; set; }
}