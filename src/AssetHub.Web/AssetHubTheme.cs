using MudBlazor;


namespace AssetHub.Web;

/*
 * The application theme. Kept in one place so status colors are consistent between
 * the dashboard titles, the grid chips and the detail page
 */
public class AssetHubTheme
{
    public static readonly MudTheme Default = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#1867c0",
            Secondary = "#37474f",
            Success = "#2e7d32",
            Info = "#0277bd",
            Warning = "#b45309",
            Error = "#c62828",
            Dark = "#424242",
            AppbarBackground = "#1867c0",
            Background = "#f5f7fa",
            DrawerBackground = "#ffffff",
            DrawerText = "#37474f"

        },

        PaletteDark = new PaletteDark
        {
            Primary = "#90caf9",
            Secondary = "#b0bec5",
            Success = "#66bb6a",
            Info = "#4fc3f7",
            Warning = "#ffb74d",
            Error = "#ef5350",
            Dark = "#212121",
            AppbarBackground = "#1e293b",
            Background = "#121820",
            DrawerBackground = "#1b2430",
            DrawerText = "#e2e8f0"
        },

        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "6px",
            DrawerWidthLeft = "260px",
        },

        Typography = new Typography
        {
            Default = new DefaultTypography { FontFamily = ["Roboto", "system-ui", "sans-serif"] }
        }
    };
}
