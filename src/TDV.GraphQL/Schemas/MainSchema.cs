using Abp.Dependency;
using GraphQL.Types;
using GraphQL.Utilities;
using TDV.Queries.Container;
using System;

namespace TDV.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IServiceProvider provider) :
            base(provider)
        {
            Query = provider.GetRequiredService<QueryContainer>();
        }
    }
}