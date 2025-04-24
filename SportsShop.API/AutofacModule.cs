using Autofac;
using SportsShop.API.ControllerParameter;
using SportsShop.Core.Repositories.Contract;
using SportsShop.Repository;
using SportsShop.Repository.Data;

namespace SportsShop.API
{
    public class AutofacModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ShopContext>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<ControllerParameters>().InstancePerLifetimeScope();
        }
    }
}
