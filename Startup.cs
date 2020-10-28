using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using sight.api.Models;

namespace sight.api
{
  public class Startup
  {
    private string _connectionString = null;
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      //dotnet user-secrets set secretConnectionString "User ID=pdev;Password=root;Server=localhost;Port=5432;Database=sight.api;Security=true;Pooling=true;"
      _connectionString = Configuration["secretConnectionString"];

      services.AddControllers();
      //   services.AddEntityFrameworkNpgsql() ya no va en esta version .netCore 2.1>= (ya no)
      services.AddDbContext<ApiContext>(
          opt => opt.UseNpgsql(_connectionString));

      /*seeders*/
      services.AddTransient<DataSeed>();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataSeed seed)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      /*init data seeders from constructor*/
      seed.SeedData(200, 1000);
      /**/

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

    }
  }
}
