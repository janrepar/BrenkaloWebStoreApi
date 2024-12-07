using BrenkaloWebStoreApi.Data;
using BrenkaloWebStoreApi.Services;
using Microsoft.EntityFrameworkCore;

namespace BrenkaloWebStoreApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<WebStoreContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); // Update the connection string

            builder.Services.AddScoped<IProductService, ProductService>();

            // Add controllers.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Disable HTTPS redirection in development
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection(); // Keep this for production
            }

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
