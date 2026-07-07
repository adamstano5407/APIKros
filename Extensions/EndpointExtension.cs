using APIKros.Data;
using APIKros.Endpoints;
using APIKros.Seeders;

namespace APIKros.Extensions;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapRoutes(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api");

        if (!app.ServiceProvider.GetRequiredService<IHostEnvironment>().IsDevelopment())
        {
            api.RequireRateLimiting("default");
        }
        app.MapCompanyEndpoints();
        app.MapDivisionEndpoints();
        app.MapProjectEndpoints();
        app.MapDepartmentEndpoints();
        app.MapEmployeeEndpoints();
        if (app.ServiceProvider.GetRequiredService<IHostEnvironment>().IsDevelopment())
        {
            app.MapPost("/dev/seed", async (AppDbContext context) =>
            {
                await DatabaseSeeder.SeedAsync(context);
                return Results.Ok("Database seeded.");
            });
        }
        return app;
    }
}