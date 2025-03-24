using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Modules.Common.Application.Extensions;
using Modules.Common.Presentation.Endpoints;
using Modules.Users.Application.UseCases;

namespace Modules.Users.Presentation.Endpoints.Auth;

public class LoginWithFaceEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/face", async ([FromForm] IFormFile file, [FromServices] ISender sender) =>
        {
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }
            var result = await sender.Send(new LoginWithFaceCommand(fileBytes));
            return result.isSuccess ? Results.Ok(result.Value) : result.ExceptionToResult();
        })
        .AllowAnonymous()
        .DisableAntiforgery();
    }

}
