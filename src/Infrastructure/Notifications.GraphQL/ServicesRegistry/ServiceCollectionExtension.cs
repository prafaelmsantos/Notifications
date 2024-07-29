namespace Notifications.GraphQL.ServicesRegistry
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddGraphQLServices(this IServiceCollection services)
        {
            services
                .AddGraphQLServer()
                .AddApolloTracing(HotChocolate.Execution.Options.TracingPreference.Always)
                .AddType<ClientMessageType>()
                .AddQueryType<Query>()
                .AddFiltering()
                .AddSorting()
                .SetPagingOptions(new PagingOptions
                {
                    MaxPageSize = int.MaxValue,
                    IncludeTotalCount = true,
                    DefaultPageSize = 10
                })
                .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

            return services;
        }
    }
}
