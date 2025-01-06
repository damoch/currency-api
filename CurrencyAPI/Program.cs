
using CurrencyAPI.Data;
using CurrencyAPI.Services.Abstracts;
using CurrencyAPI.Services.Implementations;
using CurrencyAPI.Shared.Abstracts;
using CurrencyAPI.Shared.Implementations;
using Microsoft.EntityFrameworkCore;
using Nager.Holiday;

namespace CurrencyAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IRemoteApiService, NPBApiService>();
            builder.Services.AddScoped<ICurrencyDataService, CurrencyDataService>();
            builder.Services.AddScoped<HolidayClient, HolidayClient>();
            builder.Services.AddScoped<IValidationSevice, NBPApiValidationService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
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
        }
    }
}
