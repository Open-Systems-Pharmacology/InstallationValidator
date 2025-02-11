﻿using System.Threading;
using FakeItEasy;
using InstallationValidator.Core;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Container;

namespace InstallationValidator.IntegrationTests
{
   [IntegrationTests]
   public abstract class ContextForIntegration<T> : ContextSpecification<T>
   {
      public override void GlobalContext()
      {
         if (IoC.Container != null) return;

         var container = ValidatorRegister.Initialize();

         //use only in tests
         using (container.OptimizeDependencyResolution())
         {
            container.AddRegister(x => x.FromType<ValidatorRegister>());

            container.RegisterImplementationOf(A.Fake<IDialogCreator>());
            container.RegisterImplementationOf(A.Fake<IExceptionView>());
            container.RegisterImplementationOf(new SynchronizationContext());
         }

         sut = IoC.Resolve<T>();
      }
   }
}