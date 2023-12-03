using Microsoft.Extensions.DependencyInjection;

namespace Spriggan.Foundation.Mail;

public static class DependencyInjectionConfiguration
{
    #region For: IServiceCollection

    public static IServiceCollection AddFoundationMail(this IServiceCollection services)
    {
        //
        // Services

        services
            .AddScoped<IMailer, Mailer>();

        return services;
    }

    #endregion
}
