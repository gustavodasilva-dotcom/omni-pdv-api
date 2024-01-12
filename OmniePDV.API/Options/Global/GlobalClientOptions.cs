namespace OmniePDV.API.Options.Global;

public sealed class GlobalDefaultClientOptions
{
    public string Name { get; set; }
    public string SSN { get; set; }
}

public sealed class GlobalClientOptions
{
    public GlobalDefaultClientOptions Default { get; set; }
}
