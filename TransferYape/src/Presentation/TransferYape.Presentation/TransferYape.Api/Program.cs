using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using TransferYape.Api.Configuration;
using TransferYape.Application.Transactions.Commands.CreateTransaction;
using TransferYape.Application.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureServices(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<CreateTranstactionCommandHandlerValidator>();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";

        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception is ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var errors = validationException.Errors.Select(e => new
            {
                field = e.PropertyName,
                error = e.ErrorMessage
            });

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                message = "Validation failed.",
                errors
            }));

            return;
        }

        // Default: Internal Server Error
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            message = "An unexpected error occurred.",
            details = exception?.Message
        }));
    });
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
