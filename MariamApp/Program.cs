using MariamApp.Helpers.ServiceExtensions;
using MariamApp.Hub;
using MariamApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using db = MariamApp.Data;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

ConfigurationManager configuration = builder.Configuration;
builder.Services
    .AddDatabase(configuration)
    .AddJwtAuthentication(configuration)
    .AddAppServices()
    .AddAutoMapperProfiles()
    .AddSwaggerDocs()
    .AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<NotificationHub>("/notificationHub");

#region InitializeTables
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<db.AppUsersDbContext>();
        db.Database.Migrate();

        var umService = scope.ServiceProvider.GetRequiredService<IAuthService>();
        await umService.InitializeDb();
    }
#endregion


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseStaticFiles();
app.Run();
