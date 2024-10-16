using Microsoft.Extensions.DependencyInjection;
using QuestSystem.Domain.Interfaces;
using QuestSystem.Infrastructure.Repositories;
using QuestSystem.Application.Interfaces;
using QuestSystem.Application.Services;

namespace QuestSystem.Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IQuestRepository, QuestRepository>();
            services.AddScoped<IPlayerQuestRepository, PlayerQuestRepository>();
            services.AddScoped<IPlayerItemRepository, PlayerItemRepository>();

            // Services
            services.AddScoped<IQuestService, QuestService>();
            services.AddScoped<IPlayerService, PlayerService>();

            return services;
        }
    }
}
