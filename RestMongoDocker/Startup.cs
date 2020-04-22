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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

      #endregion

      #region Kafka setup

      // Kafka Producer and Consumer setup
      var producerConfig = new ProducerConfig();
      var consumerConfig = new ConsumerConfig();

      // producer and consumer from appsettings.json
      Configuration.Bind("producer", producerConfig);
      // Configuration.Bind("consumer", consumerConfig);

      // add the consumer and producer as singletons
      services.AddSingleton<ProducerConfig>(producerConfig);
      services.AddSingleton<ConsumerConfig>(consumerConfig);
      // services.AddHostedService<ConsumerConfig>()

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
