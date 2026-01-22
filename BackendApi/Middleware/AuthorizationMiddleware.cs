using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using BackendApi.Models;
using BackendApi.Services;

namespace BackendApi.Middleware;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the endpoint has an Authorize attribute with Roles
        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            var authorizeData = endpoint.Metadata.GetOrderedMetadata<Microsoft.AspNetCore.Authorization.IAuthorizeData>();
            var requiredRoles = authorizeData
                .Where(ad => !string.IsNullOrEmpty(ad.Roles))
                .SelectMany(ad => ad.Roles.Split(','))
                .Select(role => role.Trim())
                .ToList();

            if (requiredRoles.Any())
            {
                var userService = context.RequestServices.GetRequiredService<IUserService>();
                var userIdClaim = context.User.FindFirst("sub")?.Value;

                if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
                {
                    var user = await userService.GetUserByIdAsync(userId);
                    if (user != null && !requiredRoles.Contains(user.Role.ToString()))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Forbidden: Insufficient permissions.");
                        return;
                    }
                }
            }
        }

        await _next(context);
    }
}