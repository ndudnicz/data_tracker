using dotnet_api.Data.Entities;
using dotnet_api.Repositories;

namespace dotnet_api.Services;

public class DataService(IDataRepository dataRepository): IDataService
{
    private readonly IDataRepository _dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));
    
    public IEnumerable<DataObj> Get()
    {
        return _dataRepository.Get();
    }

    public DataObj Get(int id)
    {
        return _dataRepository.Get(id);
    }

    public DataObj Create(DataObj element)
    {
        return _dataRepository.Create(element);
    }

    public int Delete(int id)
    {
        return _dataRepository.Delete(id);
    }
}