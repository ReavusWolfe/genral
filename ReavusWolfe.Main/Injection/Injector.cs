using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using ReavusWolfe.Services;

namespace ReavusWolfe.Main.Injection
{
    public interface IInjector
    {
        T GetUniqueInstance<T>();
    }
    public class Injector : IInjector
    {
        private readonly IKernel _kernel;

        public Injector(IKernel kernel)
        {
            _kernel = kernel;
        }

        public T GetUniqueInstance<T>()
        {
            return _kernel.Get<T>();
        }
    }

    public class LiveModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(x => x.FromAssembliesMatching("ReavusWolfe.*").SelectAllClasses().BindDefaultInterface());

            Kernel.Rebind<IMessageBoxService>().To<MessageBoxService>().InSingletonScope();
            Kernel.Rebind<IMessengerService>().To<MessengerService>().InSingletonScope();
        }
    }

    public class UnitTestingModule : NinjectModule
    {
        public override void Load()
        {

        }
    }

}
