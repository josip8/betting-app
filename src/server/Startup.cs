using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using data.Context;
using data.EfCoreModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using server.Services.HostedServices;

namespace server
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
      services.AddDbContext<BettingAppDbContext>(options =>
                options.UseNpgsql(Configuration["ConnectionStrings:Database"]));

      services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<BettingAppDbContext>().AddDefaultTokenProviders();

      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "server", Version = "v1" });
      });
      services.AddHostedService<PairCheckingService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "server v1"));
      }

      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

      Migrate(app);
    }

    public void Migrate(IApplicationBuilder builder)
    {
      using var scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
      using var ctx = scope.ServiceProvider.GetRequiredService<BettingAppDbContext>();

      ctx.Database.Migrate();
    }
  }
}
