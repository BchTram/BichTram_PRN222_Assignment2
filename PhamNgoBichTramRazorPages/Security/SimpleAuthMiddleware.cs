using FUNewsManagement.Repository.Models;

namespace PhamNgoBichTramRazorPages.Security;

public sealed class SimpleAuthMiddleware
{
    private readonly RequestDelegate _next;

    public SimpleAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;

        // Let static files + SignalR through
        if (path.StartsWith("/hubs/", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        if (path.StartsWith("/Admin", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/Staff", StringComparison.OrdinalIgnoreCase))
        {
            var user = context.Session.GetJson<UserSession>(SessionKeys.UserSession);
            if (user is null)
            {
                context.Response.Redirect("/Login");
                return;
            }

            if (path.StartsWith("/Admin", StringComparison.OrdinalIgnoreCase) && user.Role != UserRole.Admin)
            {
                context.Response.Redirect("/AccessDenied");
                return;
            }

            if (path.StartsWith("/Staff", StringComparison.OrdinalIgnoreCase) && user.Role != UserRole.Staff)
            {
                context.Response.Redirect("/AccessDenied");
                return;
            }
        }

        await _next(context);
    }
}

