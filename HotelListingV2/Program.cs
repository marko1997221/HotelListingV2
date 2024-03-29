using HotelListingV2.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using AutoMapper;
using HotelListingV2.Implementacija;
using HotelListingV2.Interfejsi;
using HotelListingV2.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HotelListingV2.Middleware;
using Microsoft.AspNetCore.Builder;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var cnnString = builder.Configuration.GetConnectionString("cnnString");
        builder.Services.AddDbContext<HotelListingDbContext>(options =>
        {
            options.UseSqlServer(cnnString);
        });
        builder.Services.AddControllers();
        builder.Services.AddIdentityCore<ApiUser>()
            .AddRoles<IdentityRole>()
            .AddTokenProvider<DataProtectorTokenProvider<ApiUser>>("HotelListingApi")
            .AddEntityFrameworkStores<HotelListingDbContext>()
            .AddDefaultTokenProviders();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAutoMapper(typeof(MapConfing));
        builder.Services.AddScoped(typeof(IGenericInterface<>),typeof(GenericImplementation<>));
        builder.Services.AddScoped<ICountyInterface, CountryImplementation>();
        builder.Services.AddScoped<IHotelInterface, HotelImplementacija>();
        builder.Services.AddScoped<IAuthMenager, AuthMenager>();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = builder.Configuration["JWTSettings:Issuer"],
                ValidAudience = builder.Configuration["JWTSettings:Audiance"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:Key"]))
            };
        });
        builder.Services.AddResponseCaching(options =>
        {
            options.MaximumBodySize= 1024;
            options.UseCaseSensitivePaths= true;
        });

        builder.Host.UseSerilog((ctx, lg) =>
        {
            lg.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration);
        });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", options =>
            {
                options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            });
        });
        var app = builder.Build();

        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
       
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseSerilogRequestLogging();
        app.UseCors("AllowAll");
        //deo za kesiranje obavezno je stavitiga ispod CORS
        app.UseResponseCaching();
        app.Use(async (context, next) =>
        {
            context.Response.GetTypedHeaders().CacheControl =
            new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = TimeSpan.FromSeconds(5),
            };
            context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
            new string[] { "Accept-Encoding" };
            await next();
        });
            // ovaj blok pre je za kesiranje
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}