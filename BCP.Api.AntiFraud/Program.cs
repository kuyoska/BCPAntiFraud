using BCP.Api.AntiFraud;
using BCP.Api.AntiFraud.HostedService;
using BCP.Api.AntiFraud.Kafka;
using BCP.Helpers;
using BCP.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAntiFraud, AntiFraud>();
builder.Services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();
builder.Services.AddTransient<IDbService, DbService>();
builder.Services.AddTransient<IKafkaProducer, KafkaProducer>();
//builder.Services.AddSingleton<IHostedService, ValidateTransactionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
