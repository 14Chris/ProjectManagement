using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ProjectManagement.Api.MailUtilities;
using ProjectManagement.Api.Repositories;
using ProjectManagement.Api.Services;
using ProjectManagement.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ProjectManagement.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder
                       .AllowAnyOrigin()
                       .WithOrigins("http://localhost:8080")
                       .WithOrigins("http://localhost:8085")
                       .WithOrigins("https://projectmanagement-portal.azurewebsites.net/")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .SetPreflightMaxAge(TimeSpan.FromDays(5));

                });
            });

            services.AddControllers()
                    .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);



            ///Gestion de l'authentification d'un utilisateur avec un jeton JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            string? dbConnectString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            string connectionString = (dbConnectString != null) ? dbConnectString : @"Server=.\SQLEXPRESS;Database=ProjectManagement;Trusted_Connection=True;";


            if(dbConnectString != null)
            {
                services.AddDbContext<ProjectManagementContext>(options =>
                   options.UseNpgsql(connectionString));
            }
            else
            {
                services.AddDbContext<ProjectManagementContext>(options =>
                  options.UseSqlServer(connectionString));
            }
       

            //Configure services dependencies
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITaskService, TaskService>();

            // Configure IFluentEmail
            services.AddFluentEmail("lenfant.chris@hotmail.fr")
            .AddRazorRenderer()
            .AddSmtpSender(new SmtpClient("smtp-mail.outlook.com", 587)
            {
                Credentials = new NetworkCredential("lenfant.chris@hotmail.fr", "Mmajjbmt15!14"),
                EnableSsl = true
            });

            // Add our service
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddHttpContextAccessor();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ProjectManagementContext>();
                dbContext.Database.Migrate();
            }

            

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
