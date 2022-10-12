using Domain.Interfaces.Services.User;
using Microsoft.Extensions.DependencyInjection;
using Service.Services;

namespace CrossCutting.DependencyInjection
{
    public class ConfigureService
    {
        public static void ConfigureDependenciesService (IServiceCollection serviceColletion)
        {
            serviceColletion.AddTransient<IUserService, UserService>();
            serviceColletion.AddTransient<ILoginService, LoginService>();
        }
    }
}
