using System.Threading;
using OSPSuite.BDDHelper;
using OSPSuite.InstallationValidator.Core;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.InstallationValidator.IntegrationTests
{
   [IntegrationTests]
   public abstract class ContextForIntegration<T> : ContextSpecification<T>
   {
      public override void GlobalContext()
      {
         if (IoC.Container != null) return;

         ValidatorRegister.Initialize();
         //use only in tests
         var container = IoC.Container;
         using (container.OptimizeDependencyResolution())
         {
            container.AddRegister(x =>
            {
               x.FromType<ValidatorRegister>();
            });

            container.Register<IEventPublisher, EventPublisher>();
            container.Register<IExceptionManager, ExceptionManagerForSpecs>();
            container.RegisterImplementationOf(new SynchronizationContext());
            
         }
         sut = IoC.Resolve<T>();
      }
   }
}
