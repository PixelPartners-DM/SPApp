using Microsoft.Extensions.Logging;
using PixelPalApp.Services;

namespace SPApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                // Tilføj OpenSans-fonten til appen, så den kan bruges i XAML og C#-kode
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            // Konfigurer HttpClient med baseadresse til API'et
            builder.Services.AddScoped(sp =>
                new HttpClient
                {
                    BaseAddress = new Uri("http://pixelwebsiteapi.duckdns.org:5000/")
                });

            // Registrer ApiService og CartService som scoped services
            // En scoped service oprettes en gang per side eller komponent, hvilket er passende for disse services
            builder.Services.AddScoped<ApiService>();
            builder.Services.AddScoped<CartService>();

            return builder.Build();
        }
    }
}