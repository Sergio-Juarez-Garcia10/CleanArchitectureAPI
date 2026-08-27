using Application.DTOs.Visits;
using Application.UseCases.Visits;

namespace WebApi.Endpoints
{
    public static class VisitsEndpoints
    {
        public static void MapVisitsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/visits").WithTags("Visits");

            group.MapGet("/", async (GetAllVisitsUseCase useCase) =>
            {
                try
                {
                    var visits = await useCase.ExecuteAsync();
                    return Results.Ok(visits);
                }
                catch (Exception ex)
                {

                    return Results.BadRequest(new { error = ex.Message });
                }
            })
                .WithName("GetAllVisitsUseCase")
                .WithSummary("Obtener todas las visitas")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError);



            group.MapPost("/entry", async (RegisterEntryDto dto, RegisterEntryUseCase useCase) =>
            {
                try
                {
                    var visit = await useCase.ExecuteAsync(dto);
                    return Results.Created($"/api/visits/{visit.Id}", visit);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("RegisterEntry")
            .WithSummary("Registrar la entrada de una visita")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/active", async (GetActiveVisitsUseCase useCase) =>
            {
                try
                {
                    var activeVisits = await useCase.ExecuteAsync();
                    return Results.Ok(activeVisits);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
                .WithName("GetActiveVisitsUseCase")
                .WithSummary("Obtener todas las visitas activas ")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError);


            group.MapGet("/person/{personId:guid}", async (Guid personId, GetVisitsByPersonUseCase useCase) =>
            {
                try
                {
                    var activeVisits = await useCase.ExecuteAsync(personId);
                    return Results.Ok(activeVisits);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
                .WithName("GetVisitsByPerson")
                .WithSummary("Obtener todas las visitas de una persona por su ID")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError);

            group.MapPost("/exit", async ( RegisterExitDTO dto,RegisterExitUseCase useCase) =>
            {
                try
                {
                    var visit = await useCase.ExecuteAsync(dto);
                    return Results.Ok(visit);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("RegisterExit")
            .WithSummary("Registrar la salida de una visita")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        }
    }
}
