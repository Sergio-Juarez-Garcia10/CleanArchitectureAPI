using Application.UseCases.Persons;
using Application.UseCases.Visits;
using Data;
using Data.Repositories;
using Domain;
using Domain.Abstractions;
using WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Falta conexión a la BD");

//builder.Services.AddScoped<IRepository<PersonEntity, Guid>, PersonRepository>();
//builder.Services.AddScoped<ICodeRepository<PersonEntity>, PersonRepository>();

builder.Services.AddDataServices(connectionString);

builder.Services.AddScoped<CreatePersonsUseCase>();
builder.Services.AddScoped<DeletePersonUseCase>();
builder.Services.AddScoped<GetAllPersonsUseCases>();
builder.Services.AddScoped<GetPersonByCodeUseCase>();
builder.Services.AddScoped<GetPersonByIdUseCase>();
builder.Services.AddScoped<UpdatePersonsUseCase>();

builder.Services.AddScoped<GetActiveVisitsUseCase>();
builder.Services.AddScoped<GetAllVisitsUseCase>();
builder.Services.AddScoped<GetVisitsByPersonUseCase>();
builder.Services.AddScoped<RegisterEntryUseCase>();
builder.Services.AddScoped<RegisterExitUseCase>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPersonEndpoints();
app.MapVisitsEndpoints();

app.Run();