using APICatalogo.Context;
using APICatalogo.Services;
using APICatalogo.Extensions;
using APICatalogo.Filters;
using APICatalogo.Repositories;
using APICatalogo.Logging;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options=> options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => 
  options.UseMySql(mySqlConnection,
    ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddScoped<APILoggingFilter>();
builder.Services.AddTransient<IMeuServico,MeuServico>();

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
  LogLevel = LogLevel.Information
}));

builder.Services.AddControllers(options => {
  options.Filters.Add(typeof(APIExceptionFilter));
})
.AddJsonOptions(options => {
  options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
