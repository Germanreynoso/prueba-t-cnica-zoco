using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using BackendApi.Models;
using BackendApi.Services;

namespace BackendApi.Middleware;

public class OwnershipMiddleware
{
    private readonly RequestDelegate _next;

    public OwnershipMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the endpoint requires ownership verification
        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            var ownershipAttribute = endpoint.Metadata.GetMetadata<OwnershipAttribute>();
            if (ownershipAttribute != null)
            {
                var userIdClaim = context.User.FindFirst("sub")?.Value;
                if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
                {
                    // Admins bypass ownership checks
                    if (context.User.IsInRole("Admin"))
                    {
                        await _next(context);
                        return;
                    }

                    var resourceId = context.Request.RouteValues[ownershipAttribute.ResourceIdParameter]?.ToString();
                    if (resourceId != null && int.TryParse(resourceId, out int id))
                    {
                        var userService = context.RequestServices.GetRequiredService<IUserService>();
                        var isOwner = await userService.IsOwnerAsync(userId, ownershipAttribute.ResourceType, id);

                        if (!isOwner)
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            await context.Response.WriteAsync("Forbidden: You do not own this resource.");
                            return;
                        }
                    }
                }
            }
        }

        await _next(context);
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class OwnershipAttribute : Attribute
{
    public string ResourceType { get; }
    public string ResourceIdParameter { get; }

    public OwnershipAttribute(string resourceType, string resourceIdParameter)
    {
        ResourceType = resourceType;
        ResourceIdParameter = resourceIdParameter;
    }
}