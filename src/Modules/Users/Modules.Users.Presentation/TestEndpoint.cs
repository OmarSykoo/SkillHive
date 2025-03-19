using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Common.Presentation.Endpoints;

namespace Modules.Users.Presentation;

public class TestEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("Test", (string text) => { return Results.Ok(text); })
        .RequireAuthorization(policy => policy.RequireAuthenticatedUser());
    }

}
