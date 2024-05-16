using System.Globalization;

namespace PersonDirectory.Api.Middlewares;

/// <inheritdoc/>
public class LocalizationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        var cultureQuery = context.Request.Headers.AcceptLanguage;
        var culture = cultureQuery.ToString();

        var supportedCultures = new[] { "en-US", "ka-GE" };

        if (supportedCultures.Contains(culture))
        {
            CultureInfo.CurrentCulture = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
        }

        await _next(context);
    }
}
