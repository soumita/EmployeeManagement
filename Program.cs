using EmployeeManagement.Data;
using EmployeeManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace EmployeeManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // for database connection
            builder.Services.AddDbContext<AppDbContext>
            ( 
            options=>options.UseInMemoryDatabase("EmployeeDb")
            );

            // so that angular can access the api

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPlicy", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            // add the employee repository to the dependency injection container

            builder.Services.AddScoped<IEmployeeRepositories , EmployeeRepositories>();

            // to add controllers
            builder.Services.AddControllers();

            // test api endpoint
            builder.Services.AddEndpointsApiExplorer();
            // add swagger for api documentation
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // ad swagger middleware if the environment is development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseCors("MyPlicy");

            //app.MapGet("/", () => "Hello World!");
            app.MapControllers();

            app.Run();
        }
    }
}
