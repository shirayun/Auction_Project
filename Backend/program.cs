using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// הוספת שירותים ל-Dependency Injection
//הוספת DbContext עם SQLite
builder.Services.AddDbContext<AuctionDbContext>(options =>
    options.UseSqlite("Data Source=auction.db"));

builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddHostedService<AuctionBackgroundService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("https://localhost:4300")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();
app.UseCors("AllowAngular"); 
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.MapHub<AuctionHub>("/auctionHub");
app.Run();
