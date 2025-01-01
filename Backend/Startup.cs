using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Backend.Data;
using Backend.Extensions;
using Backend.Services;

namespace Backend;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddScoped<TodoTasksService, TodoTasksService>();
        // services.AddCors(opt =>
        // {
        //     opt.AddPolicy("ProxyApi", policyBuilder =>
        //     {
        //         policyBuilder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
        //     });
        // });
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web API", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app)
    {
        // using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        // {
        //     var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //     context.Database.Migrate();
        // }

        app.UseRouting();
        //app.UseCors("ProxyApi");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.UseGlobalExceptionHandler();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "WEB API V1");
        });
    }
}