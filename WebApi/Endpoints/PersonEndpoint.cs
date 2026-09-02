using Application.DTOs.Persons;
using Application.UseCases.Persons;

namespace WebApi.Endpoints
{
    public static class PersonEndpoint
    {
        public static void MapPersonEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/persons").WithTags("Persons");

            group.MapGet("/{id:guid}", async (Guid id, GetPersonByIdUseCase useCase) =>
            {
                try
                {
                    var person = await useCase.EntityAsync(id);
                    return Results.Ok(person);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            })
            .WithName("GetPersonById")
            .WithSummary("Obtener una persona por su id")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

           group.MapPost("/", async (CreatePersonDTO request, CreatePersonsUseCase useCase) =>
            {
                try
                {
                    var person = await useCase.ExecuteAsync(request);
                    return Results.Created($"/api/persons/{person.Id}", person);

                }
                catch (InvalidOperationException ex)
                {

                    return Results.BadRequest(new {error = ex.Message});
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
            .WithName("CreatePerson")
            .WithSummary("Crear una nueva persona")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/", async (GetAllPersonsUseCases useCase) =>
            {
                try
                {
                    var persons = await useCase.ExecuteAsync();
                    return Results.Ok(persons);
                }
                catch (Exception ex)
                {

                    return Results.BadRequest(new {error = ex.Message});
                }  
            })
            .WithName("GetAllPersons")
            .WithSummary("Obtener Todas la personas")
            .Produces(StatusCodes.Status200OK);

            group.MapPut("/{id:guid}", async (Guid id,UpdatePersonDTO dto, UpdatePersonsUseCase useCase ) =>
            {

                if (id != dto.Id)
                {
                    return Results.BadRequest("Los id no corresponden");
                }
                try
                {
                   
                    var person = await useCase.ExecuteAsync(dto);
                    return Results.Ok(person);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
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
                .WithName("UpdatePerson")
                .WithSummary("Actualizar una persona existente")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            group.MapDelete("/{id:guid}", async (Guid id, DeletePersonUseCase useCase) =>
            {
                try
                {
                    await useCase.ExecuteAsync(id);
                    return Results.NoContent();
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
             .WithName("DeletePerson")
            .WithSummary("Eliminar una persona existente")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

            group.MapGet("/code/{code}", async (string code, GetPersonByCodeUseCase useCase) =>
            {
                try
                {
                    var persons = await useCase.ExecuteAsync(code);
                    return Results.Ok(persons);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
             .WithName("GetPersonByCode")
            .WithSummary("Obtener una persona por codigo")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
