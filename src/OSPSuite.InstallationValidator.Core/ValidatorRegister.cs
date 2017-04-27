using OSPSuite.Core;
using OSPSuite.InstallationValidator.Core.Domain;
using OSPSuite.Utility.Container;

namespace OSPSuite.InstallationValidator.Core
{
   public class ValidatorRegister : IRegister
   {
      public void RegisterInContainer(IContainer container)
      {
         container.AddScanner(x =>
         {
            x.AssemblyContainingType<ValidatorRegister>();
            x.ExcludeType<InstallationValidatorConfiguration>();
            x.WithConvention(new OSPSuiteRegistrationConvention(registerConcreteType: true));
            x.RegisterAs(LifeStyle.Transient);
         });

         container.Register<FolderInfo, FolderInfo>();
         container.Register<IApplicationConfiguration, IInstallationValidatorConfiguration, InstallationValidatorConfiguration>(LifeStyle.Singleton);
      }
   }
}