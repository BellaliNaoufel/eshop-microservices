using Catalog.API.Endpoints;
using Catalog.API.Models;
using Marten;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add Services to container.
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Default")!);
    options.UseSystemTextJsonForSerialization(enumStorage: EnumStorage.AsString, casing: Casing.Default);
    options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate; // TODO: only on dev
})
    .AssertDatabaseMatchesConfigurationOnStartup()
    .UseLightweightSessions();

builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("Cache"));

var app = builder.Build();
// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapProductEndpoints();

app.Run();