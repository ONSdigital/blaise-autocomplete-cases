using Blaise.Case.Auto.Populate.Models;

namespace Blaise.Case.Auto.Populate.Interfaces.Services
{
    public interface IPopulateCasesService
    {
        void PopulateCases(AutoPopulateCaseModel model);
    }
}