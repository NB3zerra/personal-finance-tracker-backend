using FinanceTracker.Presentation.Exensions;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add MongoDB
builder.Services.AddDatabase(builder.Configuration);

// Add Identity Services
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


