using HotelListingV2.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using AutoMapper;
using HotelListingV2.Implementacija;
using HotelListingV2.Interfejsi;
using HotelListingV2.Configuration;
using Microsoft.AspNetCore.Identity;

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
            .AddRoles<IdentityRole>().
            AddEntityFrameworkStores<HotelListingDbContext>();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAutoMapper(typeof(MapConfing));
        builder.Services.AddScoped(typeof(IGenericInterface<>),typeof(GenericImplementation<>));
        builder.Services.AddScoped<ICountyInterface, CountryImplementation>();
        builder.Services.AddScoped<IHotelInterface, HotelImplementacija>();
        builder.Services.AddScoped<IAuthMenager, AuthMenager>();
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
        
        app.UseSerilogRequestLogging();
        app.UseCors("AllowAll");
        app.UseHttpsRedirection();
        
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}