
using Microsoft.EntityFrameworkCore;
using P2PLearningAPI.Data;

namespace P2PLearningAPI.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using P2PLearningDbContext context = scope.ServiceProvider
                .GetRequiredService<P2PLearningDbContext>();
            context.Database.Migrate();
        }
    }
}
