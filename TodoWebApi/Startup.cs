using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using TodoDB.Data.Auth;
using TodoDB.Data.Todo;

namespace TodoWebApi
{

    public class Startup
    {

        public static string CurrentDB = "";
        public static string ConnectionString = "";        
        public static string SecretKey = "";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //******** added *******************

            SecretKey = Configuration.GetSection("AppSettings:Token").Value;
            CurrentDB = Configuration.GetSection("AppSettings:CurrentDB").Value;    
            
            if (CurrentDB == "MSSQL")
            {
                ConnectionString = Configuration.GetConnectionString("MSSQLDBConnectionString");                
                services.AddTransient<IAuthRepo>(s => new AuthRepoMSSQL(ConnectionString, SecretKey));
                services.AddTransient<ITodoRepo>(s => new TodoRepoMSSQL(ConnectionString, SecretKey));
            }
            else if (CurrentDB == "MYSQL")
            {
                ConnectionString = Configuration.GetConnectionString("MYSQLDBConnectionString");
                services.AddTransient<IAuthRepo>(s => new AuthRepoMYSQL(ConnectionString, SecretKey));
                services.AddTransient<ITodoRepo>(s => new TodoRepoMYSQL(ConnectionString, SecretKey));
            }


               
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
             .AddJsonOptions(opt =>
             {
                 opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
             });

            services.AddCors();

            //services.AddAutoMapper();

            services.AddAutoMapper(
                cfg => {
                                
                });

            services.AddSwaggerGen(
                   opt => {
                              opt.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info()
                                              {
                                                  Title ="Todo Core API Docs",
                                                  Description = "Produced by Swagger"
                                              } 
                                          );

                              //in project properties-> build ---> check output -> xml doc file  -->  copy file name..  TodoWebApi.xml
                              var xmlPath = System.AppDomain.CurrentDomain.BaseDirectory + @"TodoWebApi.xml";
                              opt.IncludeXmlComments(xmlPath);
                        });

            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });

            //******** /added *******************

        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //--- for global handler  ----

                app.UseExceptionHandler(builder => {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if (error != null)
                        {
                            //context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);

                        }
                    });
                });

                //--- /for global handler  ----
            }


            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvc();
            

            app.UseSwagger();
            app.UseSwaggerUI(
                    c=> {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Core API Docs");                       
                    }
                );


        }// Configure


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }

        //    app.UseMvc();
        //}


    }
}
