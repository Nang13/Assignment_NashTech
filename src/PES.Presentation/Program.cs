using Microsoft.EntityFrameworkCore;
using PES.Application;
using PES.Application.Helper.ErrorHandler;
using PES.Infrastructure;
using PES.Infrastructure.Data;
using PES.Presentation;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddDbContextPool<PlantManagementContext>(options =>
//    options.UseNpgsql(conn));
builder.Services.AddApplicationService();
builder.Services.AddInfrastructureServices(conn);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureService(builder);
builder.Services.AddHttpClient("JsonPlaceholder", client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseOutputCache();
app.UseHttpsRedirection();

app.UseAuthorization();


app.Use((context, next) =>
{
    context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
    context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, PATCH");
    context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization");
    return next.Invoke();
});

app.UseMiddleware<CustomExceptionMiddleware>();
app.UseCors();
app.MapControllers();

app.Run();

public partial class Program { }