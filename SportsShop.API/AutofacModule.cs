using Autofac;
using AutoMapper;
using FluentValidation;
using MediatR;
using SportsShop.API.ControllerParameter;
using SportsShop.API.Helpers;
using SportsShop.API.Validators;
using SportsShop.Core.Repositories.Contract;
using SportsShop.Core.Services.Contract;
using SportsShop.Repository;
using SportsShop.Repository.Data;
using SportsShop.Service.CQRS.Payment.Commands;
using SportsShop.Service.CQRS.Products.Queries;
using SportsShop.Service.CQRS.ShoppingCarts.Queries;
using SportsShop.Service.Helpers;

namespace SportsShop.API
{
    public class AutofacModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ShopContext>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<AuthService>().As<IAuthService>().InstancePerLifetimeScope();
            builder.RegisterType<ShoppingCartRepository>().As<IShoppingCartRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ControllerParameters>().InstancePerLifetimeScope();
            builder.RegisterType<PaymentHandle>().As<IPaymentHandle>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();

            // Register your handlers (assuming they are in the same assembly)
            builder.RegisterAssemblyTypes(typeof(GtAllProductsQueryHandler).Assembly)
                   .AsClosedTypesOf(typeof(IRequestHandler<,>))
                   .AsImplementedInterfaces();

            // Or register all types from the assembly where your CQRS handlers are located
            builder.RegisterAssemblyTypes(typeof(GetAllProductsQuery).Assembly)
                   .AsClosedTypesOf(typeof(IRequestHandler<,>))
                   .AsImplementedInterfaces();

            // Register your handlers (assuming they are in the same assembly)
            builder.RegisterAssemblyTypes(typeof(GetProductByIdQueryHandler).Assembly)
                   .AsClosedTypesOf(typeof(IRequestHandler<,>))
                   .AsImplementedInterfaces();

            // Or register all types from the assembly where your CQRS handlers are located
            builder.RegisterAssemblyTypes(typeof(GetProductByIdQuery).Assembly)
                   .AsClosedTypesOf(typeof(IRequestHandler<,>))
                   .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(GetShoppingCartQuery).Assembly)
                   .AsClosedTypesOf(typeof(IRequestHandler<,>))
                   .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(CreateOrUpdatePaymentIntentCommand).Assembly)
                .AsClosedTypesOf(typeof (IRequestHandler<,>))
                .AsImplementedInterfaces();



            builder.Register(context => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            }).CreateMapper()).As<IMapper>().InstancePerLifetimeScope();


            builder.RegisterAssemblyTypes(typeof(ProductCreateDtoValidators).Assembly)
               .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
               .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(CreateOrderDtoValidators).Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
               .AsImplementedInterfaces();
        }
    }
}
