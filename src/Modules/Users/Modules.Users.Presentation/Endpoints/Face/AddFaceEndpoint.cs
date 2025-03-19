using System.Security.Claims;
using System.Text.Json;
using MassTransit.Internals;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Modules.Common.Application.Extensions;
using Modules.Common.Infrastructure.Authentication;
using Modules.Common.Infrastructure.Authentication.Extensions;
using Modules.Common.Presentation.Endpoints;
using Modules.Users.Application.UseCases;

namespace Modules.Users.Presentation.Endpoints.Face;

public class AddFaceEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("face", async (HttpContext httpContext, [FromForm] IFormFile file, [FromServices] ISender sender) =>
        {
            Guid user_id = httpContext.User.GetUserId();
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            var result = await sender.Send(new CreateFaceEmbedingCommand(fileBytes, user_id));
            return result.isSuccess ? Results.NoContent() : result.ExceptionToResult();
        })
        .RequireAuthorization()
        .DisableAntiforgery();
    }

}
