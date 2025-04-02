using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Modules.Common.Application.Extensions;
using Modules.Common.Presentation.Endpoints;
using Modules.Users.Application.UseCases;

namespace Modules.Users.Presentation.Endpoints.Users;

public class UpdateNameEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/users", async (
            [FromBody] UpdateUserNameCommand request,
            [FromServices] ISender sender) =>
        {
            var result = await sender.Send(request);
            if (result.isSuccess)
                return Results.Ok(result.Value);
            return result.ExceptionToResult();
        })
        .RequireAuthorization();
    }

}
