var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddApplicationParts();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true; // Use default if not provided
    options.ReportApiVersions = true;
});

builder.Services.RegisterServices();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
