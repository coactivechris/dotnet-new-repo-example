using System.Threading.Tasks;

namespace DefaultNamespace.Repositories.ModelName
{
    /// <summary>
    /// A simple CRUD repository for our ModelName to DynamoDb
    /// </summary>
    public interface IModelNameRepository
    {
        Task<Models.ModelName> SelectAsync(dynamic id);
        Task UpdateAsync(Models.ModelName model);
        Task<Models.ModelName> InsertAsync(Models.ModelName model);
        Task DeleteAsync(dynamic id);
    }
}