using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StackOverflowTags.Data;
using StackOverflowTags.Interfaces;
using StackOverflowTags.Repositories;

namespace StackOverflowTags
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Services.AddScoped<ITagsRepository, TagsRepository>();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "StackOverflow_Tags_API",
                    Description = "Api includes tags from SO (When you first run it, you get 1000.)",
                });
            });

            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
