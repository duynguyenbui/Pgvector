namespace Database.PgVector.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddNpgsql<CatalogContext>(builder.Configuration.GetConnectionString("CatalogDB"),
            optionsBuilder => { optionsBuilder.UseVector(); });

        builder.Services.AddMigration<CatalogContext, CatalogContextSeed>();

        builder.Services.AddOptions<CatalogOptions>()
            .BindConfiguration(nameof(CatalogOptions));

        builder.Services.AddOptions<AIOptions>()
            .BindConfiguration("AI");

        builder.Services.AddHttpClient();

        builder.Services.AddSingleton<ICatalogAI, CatalogAI>();
    }
}