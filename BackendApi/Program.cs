using BackendApi.Models;
using BackendApi.Services;
using BackendApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// CORS for local development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Register Services as Singletons for In-Memory storage (Mock)
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<IUserService, MockUserService>();
builder.Services.AddSingleton<IStudyService, MockStudyService>();
builder.Services.AddSingleton<IAddressService, MockAddressService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

// No Auth middleware for now
app.MapControllers();

app.Run();
