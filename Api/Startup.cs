using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Api.Data.Entities.Authentication;
using Api.DataAccess;
using Api.Features.Authentication.Entities;
using Api.Features.BaseRepository;
using Api.Features.BaseRepository.Interfaces;
using Api.Features.SubTasks.Entities;
using Api.Features.SubTasks.Models;
using Api.Features.SubTasks.Repositories;
using Api.Features.SubTasks.Services;
using Api.Features.Tasks.Entities;
using Api.Features.Tasks.Models;
using Api.Features.Tasks.Repositories;
using Api.Features.Tasks.Services;
using Api.Features.Users.Models;
using Api.Features.Users.Repositories;
using Api.Features.Users.Services;
using Api.Helpers;
using Api.Helpers.Converters;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sieve.Models;
using Sieve.Services;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void ConfigureServicesDependency(IServiceCollection services)
        {
            services.AddScoped<IEmailSender, EmailSender>();

            //// Register Tasks services
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IBaseRepository<Task>, BaseRepository<Task>>();

            //// Register SubTasks services
            services.AddScoped<ISubTaskRepository, SubTaskRepository>();
            services.AddScoped<ISubTaskService, SubTaskService>();
            services.AddScoped<IBaseRepository<SubTask>, BaseRepository<SubTask>>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBaseRepository<ApplicationUser>, BaseRepository<ApplicationUser>>();

            services.AddScoped<ISieveProcessor, TaskSieveProcessor>();
            services.AddScoped<ISieveProcessor, UserSieveProcessor>();
            services.AddScoped<ISieveProcessor, SubTaskSieveProcessor>();

            services.AddScoped<ISieveProcessor, SieveProcessor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<SieveProcessor>();

            // register options
            services.AddOptions<SieveOptions>().Bind(Configuration.GetSection("Sieve"));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TaskProfile());
                mc.AddProfile(new SubTaskProfile());
                mc.AddProfile(new UserProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureServicesDependency(services);

            ConfigureAuthentication(services);

            //services.RegisterServices();

            //get local database ConnectionString from AppSettings
            services.AddMvc(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.EnableEndpointRouting = false;
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            });

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Develop")));

            services.AddCors(options => options.AddPolicy("AllowAllOrigins", builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddAuthorization();

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddSignInManager()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.Configure<DataProtectionTokenProviderOptions>(o =>
                             o.TokenLifespan = TimeSpan.FromHours(3));

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.ClaimsIssuer = "http://localhost:63778/";
                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = signingKey,
                    ValidateAudience = true,
                    ValidAudience = this.Configuration["Tokens:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = this.Configuration["Tokens:Issuer"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            }).AddGoogle(options =>
            {
                var googleAuth = Configuration.GetSection("Authentication:Google");

                options.ClientId = googleAuth["ClientId"];
                options.ClientSecret = googleAuth["ClientSecret"];
                options.SignInScheme = IdentityConstants.ExternalScheme;
            }).AddCookie(options => {
                options.Cookie.IsEssential = true;
            });
            //    .AddFacebook(options =>
            //    {
            //        options.AppId = Configuration["FacebookOptions:AppId"];
            //        options.AppSecret = Configuration["FacebookOptions:AppSecret"];
            //    });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/api/auth/login";
                options.SlidingExpiration = true;
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // TIME MEASUREMENT
            //var times = new List<long>();
            //app.Use(async (context, next) =>
            //{
            //    var sw = new Stopwatch();
            //    sw.Start();
            //    await next.Invoke();
            //    sw.Stop();
            //    times.Add(sw.ElapsedMilliseconds);
            //    var text = $"AVG: {(int)times.Average()}ms; AT {sw.ElapsedMilliseconds}; COUNT: {times.Count()}";
            //    Console.WriteLine(text);
            //    await context.Response.WriteAsync($"<!-- {text} -->");
            //});

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowAllOrigins");
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
