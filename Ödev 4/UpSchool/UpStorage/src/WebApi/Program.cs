using Application;
using Domain.Settings;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text;
using WebApi.Controllers.Filters;
using WebApi.Filters;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Application.Common.Interfaces;
using WebApi.Services;
using Serilog;

Log.Logger=new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();


    // Add services to the container.

    builder.Services.AddControllers(opt =>
    {
        //opt.Filters.Add<ValidationFilter>();
        opt.Filters.Add<GlobalExceptionFilter>();
    });

    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(setupAction =>
    {
        setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = $"Input your Bearer token in this format - Bearer token to access this API",
        });
        setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        }, new List<string>()
                    },
                });
    });
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructure(builder.Configuration, builder.Environment.WebRootPath);


    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
                    .AddJwtBearer(o =>
                    {
                        o.RequireHttpsMetadata = false;
                        o.SaveToken = false;
                        o.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero,
                            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                            ValidAudience = builder.Configuration["JwtSettings:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
                        };
                    });

    builder.Services.AddLocalization(options =>
    {
        options.ResourcesPath="Resources";
    });

    builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        var defaultCulture = new CultureInfo("en-GB");
        List<CultureInfo> cultureInfos = new List<CultureInfo>()
    {
        defaultCulture,
        new ("tr-TR")

    };

        options.SupportedCultures=cultureInfos;
        options.SupportedUICultures=cultureInfos;
        options.DefaultRequestCulture= new RequestCulture(defaultCulture);
        options.ApplyCurrentCultureToResponseHeaders=true;
    });

    builder.Services.AddSignalR();
    builder.Services.AddScoped<IAccountHubService, AccountHubManager>();
    builder.Services.AddMemoryCache();
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    var requestLocalizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
    if (requestLocalizationOptions is not null) app.UseRequestLocalization(requestLocalizationOptions.Value);


    app.UseStaticFiles();
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
    //throw new AbandonedMutexException("Sana Söz");
    //throw new NullReferenceException("Sana söz");
    app.Run();
    
}
catch (Exception ex)
{
    Log.Fatal(ex, "Applicationterminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}


