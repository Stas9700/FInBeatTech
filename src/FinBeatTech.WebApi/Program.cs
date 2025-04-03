
using FinBeatTech.Database;
using FinBeatTech.Services;
using FinBeatTech.Services.Implementations;
using FinBeatTech.Services.Interfaces;
using FinBeatTech.WebApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext
builder.Services.AddDbContextFactory<FinBeatDbContext>(options =>
{
    options.UseSqlite($"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "finBeatTechDb")}");
});

// Register services
builder.Services.AddScoped<IDataService, DataService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add logging middleware
app.UseMiddleware<ApiLoggingMiddleware>();

app.UseAuthorization();

app.MapControllers();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<FinBeatDbContext>>();
    using FinBeatDbContext dbContext = factory.CreateDbContext();
    dbContext.Database.Migrate();
}

app.Run();