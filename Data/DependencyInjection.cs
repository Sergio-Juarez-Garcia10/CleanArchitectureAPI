using Data.Persistence;
using Data.Repositories;
using Domain;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, string connectionStrings)
        {
            // Register your data services here
            // Example: services.AddScoped<IYourRepository, YourRepository>();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionStrings);
            });

            services.AddScoped<IRepository<PersonEntity, Guid>, PersonRepository>();
            services.AddScoped<ICodeRepository<PersonEntity>, PersonRepository>();

            services.AddScoped<IRepository<VisitEntity, Guid>, VisitRepository>();
            services.AddScoped<IVisitRepository<VisitEntity>, VisitRepository>();


            return services;
        }
    }
}
