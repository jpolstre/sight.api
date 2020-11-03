
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using sight.api.Models;
using sight.api.seeders;

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
      // live reload para el desarrollo.
      // services.AddControllersWithViews().AddRazorRuntimeCompilation();

      //In-process hosting model
      services.Configure<IISServerOptions>(options =>
      {
        options.AutomaticAuthentication = false;
      });

      //Out-of-process hosting model
      /* services.Configure<IISOptions>(options =>
       {
           options.ForwardClientCertificate = false;
       });*/

      /*cors only open in dev*/
      services.AddCors(opt =>
      {
        opt.AddPolicy("CorsPolicy", c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
      });
      /**/

      //dotnet user-secrets set secretConnectionString "User ID=pdev;Password=root;Server=localhost;Port=5432;Database=sight.api;Security=true;Pooling=true;"
      _connectionString = Configuration["secretConnectionString"];

      services.AddControllers();
      //   services.AddEntityFrameworkNpgsql() ya no va en esta version .netCore 2.1>= (ya no)
      services.AddDbContext<ApiContext>(
          // no esta funcionanado el paquete Microsoft.Extensions.SecretManager.Tools  esta devolviendo null al hacer el deply en IIS -> _connectionString
          opt => opt.UseNpgsql("User ID=pdev;Password=root;Server=localhost;Port=5432;Database=sight.api;Integrated Security=true;Pooling=true;"));

      /*seeders*/
      services.AddTransient<DataSeed>();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataSeed seed)
    {
      if (env.IsDevelopment() || env.IsProduction())
      {
        app.UseDeveloperExceptionPage();
        /*cors only open in dev*/
        app.UseCors("CorsPolicy");
        /**/
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
