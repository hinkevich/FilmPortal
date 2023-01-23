using FilmPortal.Models.Repo;
using FilmPortal.Tests.FakeRepositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmPortal.Tests
{
    public class FilmPortalWebApplicationFactory<TProgram>:WebApplicationFactory<TProgram> where TProgram:class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => 
            {
                var repo = services.SingleOrDefault(s => s.ServiceType == typeof(IActorsRepository));
                services.Remove(repo);
                services.AddScoped<IActorsRepository, FakeActorsRepository>();
                
            });
        }
    }
}
