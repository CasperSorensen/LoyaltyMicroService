using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Config;
using RestMongoDocker.Models;
using RestMongoDocker.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;

namespace RestMongoDocker
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
      var config = new ServerConfig();
      Configuration.Bind(config);

      #region Repositories setup
      var todocontext = new TodoContext(config.MongoDb);
      var todorepo = new TodoRepository(todocontext);
      services.AddSingleton<ITodoRepository>(todorepo);

      var usercontext = new UserContext(config.MongoDb);
      var userrepo = new UserRepository(usercontext);
      services.AddSingleton<IUserRepository>(userrepo);

      #endregion

      #region Kafka setup
      //services.AddSingleton<HostedServices.IHostedService, ConsumerBackgroundService>();
      // Kafka Producer setup
      var producerConfig = new ProducerConfig();

      // producer from appsettings.json
      Configuration.Bind("producer", producerConfig);

      // add the producer as singletons
      services.AddSingleton<ProducerConfig>(producerConfig);
      #endregion

      services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseHsts();
      }

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
