using BlaiseCaseAutoComplete.Models;
using StatNeth.Blaise.API.DataRecord;

namespace BlaiseCaseAutoComplete.Interfaces.Services
{
    public interface IPopulateCaseService
    {
        void CompleteCase(AutoCompleteCaseModel model);
        void CompleteCase(IDataRecord dataRecord, AutoCompleteCaseModel model);
    }
}