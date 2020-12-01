using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;

namespace Coco.Comsumer
{
    public static class ServiceExtension
    {
        //public static IServiceCollection AddCocoComsumer<T>(this IServiceCollection service) where T : ComsumerService
        //{
        //    service.AddHostedService<T>();
        //    return service;
        //}

        public static IServiceCollection AddCocoComsumer<T>(this IServiceCollection service) where T : Comsumer, IHostedService
        {
            service.AddHostedService<T>();
            return service;
        }

        public static IServiceCollection AddCocoComsumer<T>(this IServiceCollection service, Func<T, T> action) where T : Comsumer, IHostedService, new()
        {
            //service.AddHostedService<T>();
            //service.AddHostedService<T>(options => action(T));
            //service.AddSingleton<IHostedService, T>(options => action(new T()));
            //service.AddSingleton<IHostedService, T>();
            service.TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService, T>());
            return service;
        }
    }
}
