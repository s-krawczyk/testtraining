using ApplicationLayer.CustomerService;
using ApplicationLayer.Validators;
using FluentValidation;
using InfrastructureLayer;
using InfrastructureLayer.ExternalServices;
using InfrastructureLayer.RespositoryPattern;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net.Mime;
using static System.Net.Mime.MediaTypeNames;

namespace WebAPI
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

      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestTrainingWebApi", Version = "v1" });
      });

      #region Connection String
      services.AddDbContext<ApplicationDbContext>(item => item.UseSqlServer(Configuration.GetConnectionString("myconn")));
      #endregion

      #region Services Injected
      services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
      services.AddTransient<ICustomerService, CustomerService>();
      services.AddTransient<IEmailSender, EmailSender>();
      services.AddValidatorsFromAssemblyContaining<CustomerValidator>();
      services.AddScoped<IValidatorFactory, ValidatorFactory>();
      #endregion

      services.AddHttpClient(nameof(EmailSender), httpClient =>
      {
        httpClient.BaseAddress = new System.Uri(Configuration.GetValue<string>("EmailServiceUri"));
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContext)
    {
      dbContext.Database.Migrate();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestTrainingWebApi v1"));
      }

      app.UseExceptionHandler(exceptionHandlerApp =>
      {
        exceptionHandlerApp.Run(async context =>
        {
          var exceptionHandlerPathFeature =
              context.Features.Get<IExceptionHandlerPathFeature>();

          var validationError = exceptionHandlerPathFeature?.Error as ValidationException;

          if (validationError != null)
          {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(validationError.Message);
          }
        });
      });

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
