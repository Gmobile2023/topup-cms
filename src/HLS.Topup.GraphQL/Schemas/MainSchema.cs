using Abp.Dependency;
using GraphQL;
using GraphQL.Types;
using HLS.Topup.Queries.Container;

namespace HLS.Topup.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IDependencyResolver resolver) :
            base(resolver)
        {
            Query = resolver.Resolve<QueryContainer>();
        }
    }
}