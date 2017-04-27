using OSPSuite.Core.Domain;

namespace OSPSuite.InstallationValidator.Core.Domain
{
   public interface IWithValidationState
   {
      ValidationState State { get; }
   }

 
}