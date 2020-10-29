using Blaise.Case.Auto.Populate.Models;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Case.Auto.Populate.Interfaces.Services
{
    public interface IPopulateCaseService
    {
        void PopulateCase(AutoPopulateCaseModel model);
        void PopulateCase(IDataRecord dataRecord, AutoPopulateCaseModel model);
    }
}