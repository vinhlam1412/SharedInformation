namespace HQSOFT.SharedInformation;

public static class SharedInformationDbProperties
{
    public static string DbTablePrefix { get; set; } = "SIF";

    public static string? DbSchema { get; set; } = null;

    public const string ConnectionStringName = "SharedInformation";
}
