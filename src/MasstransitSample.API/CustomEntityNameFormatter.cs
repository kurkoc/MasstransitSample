using MassTransit;

namespace MasstransitSample.API
{
    public class CustomEntityNameFormatter : IEntityNameFormatter
    {
        public string FormatEntityName<T>()
        {
            return KebabCaseEndpointNameFormatter.Instance.SanitizeName(typeof(T).Name);
        }
    }
}
