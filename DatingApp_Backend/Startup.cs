
using System.Text;
using AutoMapper;
using DatingApp_Backend.Data;
using DatingApp_Backend.Models;
using DatingApp_Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp_Backend
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
            services.AddDbContext<UserDbContext>(option => option.UseSqlServer("Data Source=db-mssql;Initial Catalog=s17514;Integrated Security=True"));
            services.AddControllers();
            services.AddAutoMapper(typeof(DatingRepository).Assembly);
            services.AddScoped<IDBService, SqlServiceDBService>();
            services.AddScoped<IDatingRepository, DatingRepository>();
            services.AddControllers().AddNewtonsoftJson(opt => 
            {
               opt.SerializerSettings.ReferenceLoopHandling =
               Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
               {
                   option.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"])),
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidIssuer = "Rizo",
                       ValidAudience = "Users"
                   };


               });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
