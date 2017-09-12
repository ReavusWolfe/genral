using System;

namespace ReavusWolfe.Domain.DAL
{
    public interface IDbFactory : IDisposable
    {
        IEntityContext Create();
    }

    public class DbFactory : IDbFactory
    {
        public static FakeEntityContext FakeEntityContext;

        public IEntityContext Create()
        {
            if (FakeEntityContext != null)
                return FakeEntityContext;

            return new EntityContext(new EntityModel());
        }

        public void Dispose()
        {

        }
    }
}