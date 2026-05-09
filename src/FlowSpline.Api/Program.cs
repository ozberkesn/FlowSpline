using FlowSpline.Api.Middleware;
using FlowSpline.Application;
using FlowSpline.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
