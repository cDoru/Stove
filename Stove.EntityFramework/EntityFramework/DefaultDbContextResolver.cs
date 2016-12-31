using System;

using Autofac.Extras.IocManager;

namespace Stove.EntityFramework.EntityFramework
{
    public class DefaultDbContextResolver : IDbContextResolver, ITransientDependency
    {
        private readonly IDbContextTypeMatcher _dbContextTypeMatcher;
        private readonly IIocResolver _iocResolver;

        public DefaultDbContextResolver(IIocResolver iocResolver, IDbContextTypeMatcher dbContextTypeMatcher)
        {
            _iocResolver = iocResolver;
            _dbContextTypeMatcher = dbContextTypeMatcher;
        }

        public TDbContext Resolve<TDbContext>(string connectionString)
        {
            Type dbContextType = typeof(TDbContext);

            if (!dbContextType.IsAbstract)
            {
                return _iocResolver.Resolve<TDbContext>(new
                {
                    nameOrConnectionString = connectionString
                });
            }

            Type concreteType = _dbContextTypeMatcher.GetConcreteType(dbContextType);
            return (TDbContext)_iocResolver.Resolve(concreteType, new
            {
                nameOrConnectionString = connectionString
            });
        }
    }
}