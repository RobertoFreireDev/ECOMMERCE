var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .ConfigureApplicationPartManager(apm =>
    {
        apm.FeatureProviders.Add(new InternalControllerFeatureProvider());
    })
    .AddApplicationParts();

builder.Services.ConfigureApi();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrationsAsync();
}
else
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
