﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sandbox.Notes.Api.Infrastructure.Configuration;
using Sandbox.Notes.Api.Infrastructure.DataAccess;
using Sandbox.Notes.Api.Infrastructure.Logging;
using Sandbox.Notes.Api.Notes.Resource;
using Sandbox.Notes.Api.Notes.Services;

namespace Sandbox.Notes.Api
{
    public class Startup
    {
	    // Configuration settings
		public Startup(IHostingEnvironment environment, IConfiguration configuration)
	    {
		    Environment = environment;
		    Configuration = configuration;
	    }

	    public readonly IHostingEnvironment Environment;
	    public readonly IConfiguration Configuration;

	    // Service dependency injection configuration
		public void ConfigureServices(IServiceCollection services)
        {
	        services
				.AddMvc(option =>
		        {
					
			        option.Filters.Add(typeof(HttpGlobalExceptionFilter));
				})
				.AddControllersAsServices();

	        services.AddDbContext<NotesDbContext>(options =>
		        options.UseInMemoryDatabase("NotesDb")
	        );

			services.AddOptions();
	        services.Configure<AppConfiguration>(Configuration.GetSection("AppConfiguration"));

			// Application services
			services.AddTransient<IReadNoteService, ReadNoteService>();
	        services.AddTransient<ICreateNoteService, CreateNoteService>();
	        services.AddTransient<IUpdateNoteService, UpdateNoteService>();
	        services.AddTransient<IDeleteNoteService, DeleteNoteService>();
		}

		// Middleware configuration
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
		{
			app.UseMiddleware<CorrelationMiddleware>();
			app.UseMvcWithDefaultRoute();
			app.UseDeveloperExceptionPage();
		    var context = serviceProvider.GetService<NotesDbContext>();
		    AddTestData(context);
        }

        private static void AddTestData(NotesDbContext context)
        {
            var testNotes = new List<Note>
                {
                    new Note
                    {
                        Id = 2,
                        Text = "Test Note 2",
                        CreatedOn = DateTime.Now.AddDays(-1),
                        Creator = "milos.kerkez"
                    },
                    new Note
                    {
                        Id = 1,
                        Text = "Test Note 1",
                        CreatedOn = DateTime.Now,
                        Creator = "milos.kerkez"
                    },
                    new Note
                    {
                        Id = 3,
                        Text = "Test Note 3",
                        CreatedOn = DateTime.Now.AddHours(-12),
                        Creator = "milos.kerkez"
                    },
                    new Note
                    {
                        Id = 4,
                        Text = "Test Note4",
                        CreatedOn = DateTime.Now,
                        Creator = "milos.kerkez"
                    },
                    new Note
                    {
                        Id = 5,
                        Text = "Test Note 5",
                        CreatedOn = DateTime.Now.AddDays(-2),
                        Creator = "milos.kerkez"
                    },
                    new Note
                    {
                        Id = 6,
                        Text = "Test Note 6",
                        CreatedOn = DateTime.Now.AddDays(-7),
                        Creator = "aleksandar.borkovac"
                    },
                    new Note
                    {
                        Id = 7,
                        Text = "Test Note 7",
                        CreatedOn = DateTime.Now.AddDays(-14),
                        Creator = "milos.kerkez"
                    },
                    new Note
                    {
                        Id = 8,
                        Text = "Test Note 8",
                        CreatedOn = DateTime.Now.AddDays(21),
                        Creator = "aleksandar.borkovac"
                    },
                    new Note
                    {
                        Id = 9,
                        Text = "Test Note 9",
                        CreatedOn = DateTime.Now.AddDays(-5),
                        Creator = "milos.kerkez"
                    }
                };

            context.Notes.AddRange(testNotes);

            context.SaveChanges();
        }
    }
}