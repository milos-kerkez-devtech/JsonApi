using System;
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
            services.AddTransient<IReadNoteListService, ReadNoteListService>();
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
                        Id = 1002,
                        Text = "Test Note 2",
                        CreatedOn = DateTime.Now.AddDays(-1),
                        Creator = "milos.kerkez",
                        NoteListId = 1
                    },
                    new Note
                    {
                        Id = 1001,
                        Text = "Test Note 1",
                        CreatedOn = DateTime.Now,
                        Creator = "milos.kerkez",
                        NoteListId = 2
                    },
                    new Note
                    {
                        Id = 1003,
                        Text = "Test Note 3",
                        CreatedOn = DateTime.Now.AddHours(-12),
                        Creator = "milos.kerkez",
                        NoteListId = 0
                    },
                    new Note
                    {
                        Id = 1004,
                        Text = "Test Note4",
                        CreatedOn = DateTime.Now,
                        Creator = "milos.kerkez",
                        NoteListId = 0
                    },
                    new Note
                    {
                        Id = 1005,
                        Text = "Test Note 5",
                        CreatedOn = DateTime.Now.AddDays(-2),
                        Creator = "milos.kerkez",
                        NoteListId = 0
                    },
                    new Note
                    {
                        Id = 1006,
                        Text = "Test Note 6",
                        CreatedOn = DateTime.Now.AddDays(-7),
                        Creator = "aleksandar.borkovac",
                        NoteListId = 0
                    },
                    new Note
                    {
                        Id = 1007,
                        Text = "Test Note 7",
                        CreatedOn = DateTime.Now.AddDays(-14),
                        Creator = "milos.kerkez",
                        NoteListId = 0
                    },
                    new Note
                    {
                        Id = 1008,
                        Text = "Test Note 8",
                        CreatedOn = DateTime.Now.AddDays(21),
                        Creator = "aleksandar.borkovac",
                        NoteListId = 0
                    },
                    new Note
                    {
                        Id = 1009,
                        Text = "Test Note 9",
                        CreatedOn = DateTime.Now.AddDays(-5),
                        Creator = "milos.kerkez",
                        NoteListId = 0
                    }
                };

            var noteLists = new List<NoteList>
            {
                new NoteList
                {
                    Id = 1,
                    Name = "Home"
                },
                new NoteList
                {
                    Id = 2,
                    Name = "Work"
                }
            };

            context.Notes.AddRange(testNotes);
            context.NoteLists.AddRange(noteLists);

            context.SaveChanges();
        }
    }
}
