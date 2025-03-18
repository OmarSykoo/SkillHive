using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Common.Application.Extensions;
using Modules.Common.Presentation.Endpoints;
using Modules.Users.Application.UseCases.CreateUser;
using Modules.Users.Domain;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Presentation.Endpoints.Users;

public class CreateUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users", async (CreateUserCommand request, ISender sender) =>
        {
            var userId = await sender.Send(request);
            if (userId.isSuccess)
                return Results.Ok(userId.Value);
            return userId.ExceptionToResult();
        }).WithName("Users");
    }

}
