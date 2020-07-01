using BlaiseCaseAutoComplete.Models;
using StatNeth.Blaise.API.DataRecord;

namespace BlaiseCaseAutoComplete.Interfaces.Services
{
    public interface ICompleteCaseService
    {
        void CompleteCase(AutoCompleteCaseModel model);
        void CompleteCase(IDataRecord dataRecord, AutoCompleteCaseModel model);
    }
}