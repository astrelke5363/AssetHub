namespace AssetHub.Domain;

public static class Roles
{
    public const string Administrator = "Administrator";
    public const string AssetManager = "AssetManager";
    public const string Technician = "Technician";
    public const string Viewer = "Viewer";

    public static readonly IReadOnlyList<string> All =
    [
        Administrator,
        AssetManager,
        Technician,
        Viewer
    ];
}
