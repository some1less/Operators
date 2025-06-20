using Operators.Application;
using Operators.DTO;
using Operators.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("OperatorDatabase")
                       ?? throw new Exception("DeviceDatabase connection string is not found");

builder.Services.AddScoped<IOperatorRepository>(_ => new OperatorRepository(connectionString));
builder.Services.AddScoped<IOperatorService, OperatorService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("api/mobiles", (IOperatorService service) =>
{
    try
    {
        var res = service.GetPhoneNumbers();
        return Results.Ok(res);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("api/mobiles", (IOperatorService service, CreatePhoneNumberDTO dto) =>
{
    try
    {
        var adv = service.CreatePhoneNumber(dto);
        return Results.Created($"api/mobiles/{adv.Id}", adv);

    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Run();
