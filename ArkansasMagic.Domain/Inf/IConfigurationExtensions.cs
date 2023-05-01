using Microsoft.Extensions.Configuration;

namespace ArkansasMagic.Domain.Inf
{
    public static class IConfigurationExtensions
    {
        public static TConfiguration BindSection<TConfiguration>(this IConfiguration configuration, string sectionName)
            where TConfiguration : class, new()
        {
            IConfigurationSection section = configuration.GetSection(sectionName);
            var target = new TConfiguration();
            section.Bind(target);
            return target;
        }
    }
}
