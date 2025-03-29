using lake_data_api.Services;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// data connections
builder.Services.AddTransient(x => new MySqlConnection(builder.Configuration.GetConnectionString("Default")));

// Services
builder.Services.AddScoped<ILakeWaterLevelService, USGSLakeWaterLevelService>();

// Repositories
builder.Services.AddScoped<ILakeRepository, LakeRepository>();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
