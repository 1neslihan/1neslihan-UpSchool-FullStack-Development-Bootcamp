using Infrastructure;
using Application;
using Microsoft.EntityFrameworkCore;
using WebApi.Hubs;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;
using System.Text;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Format exception messages
builder.Services.AddControllers(/*opt =>*/
//{
    //opt.Filters.Add<ValidationFilter>();
/*}*/);

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); //base64 encoding


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHttpClient();

builder.Services.AddHostedService<BackgroundBot>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.MapHub<UserLogHub>("/Hubs/UserLogHub");
app.MapHub<DataTransferHub>("/Hubs/DataTransferHub");

app.Run();
