using System;
using System.Reflection;

namespace ClassifiedAds.Application
{
    public static class Utils
    {
        public static bool IsHandlerInterface(Type type)
        {
            if (!type.IsGenericType)
            {
                return false;
            }

            var typeDefinition = type.GetGenericTypeDefinition();

            return typeDefinition == typeof(ICommandHandler<>)
                || typeDefinition == typeof(IQueryHandler<,>);
        }

        public static string getCurrentClassMethod(Type type)
        {
            return type.Name.Replace("Controller", "");
        }
    }
}
