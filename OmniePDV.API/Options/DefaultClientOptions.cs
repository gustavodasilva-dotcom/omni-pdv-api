namespace OmniePDV.API.Options;

public sealed class DefaultClientOptions
{
    public const string Position = "DefaultClient";

    public string Name { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
}
