using System.Collections.Generic;
using System.Threading.Tasks;
using microAPI.Models;

namespace microAPI.Interfaces
{
    public interface ICollectionRepository
    {
        Task<List<CollectionModel>> LoadCollectionsAsync();
        Task SaveCollectionsAsync(List<CollectionModel> collection);
    }
}