// <copyright file="EndpointGroupExtension.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.API.Extensions
{
    using System.Reflection;

    public static class EndpointGroupExtension
    {
        public static void RegisterAllEndpointGroups(this IEndpointRouteBuilder app)
        {
            var endpointGroupTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IEndpointGroup).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in endpointGroupTypes)
            {
                if (Activator.CreateInstance(type) is IEndpointGroup group)
                {
                    group.MapEndpoints(app);
                }
            }
        }
    }
}
