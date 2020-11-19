using Microsoft.Extensions.DependencyInjection;

namespace Coco.Comsumer
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddCocoComsumer<T>(this IServiceCollection service) where T : ComsumerService
        {
            service.AddHostedService<T>();
            return service;
        }
    }
}
