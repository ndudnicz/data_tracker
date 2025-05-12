using dotnet_api.Data.Entities;

namespace dotnet_api.Repositories;

public interface IDataRepository
{
    public IEnumerable<DataObj> Get();
    public DataObj Get(int id);
    public DataObj Create(DataObj element);
    
    public int Delete(int id);
}