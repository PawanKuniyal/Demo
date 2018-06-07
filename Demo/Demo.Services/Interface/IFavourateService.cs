using System.Linq;
using System.Threading.Tasks;
using Helpa.Entities;

namespace Helpa.Services.Interface
{
    public interface IFavourateService
    {
        IQueryable<Favourate> GetFavourates();
        IQueryable<Favourate> GetFavourate(int Id);
        Favourate GetFavourateById(int Id);
        Task<Favourate> GetFavourateAsync(int Id);
        Favourate AddFavourate(Favourate favourate);
        Task<Favourate> AddFavourateAsync(Favourate favourate);
        int RemoveFavourate(Favourate favourate);
        Task<int> RemoveFavourateAsync(Favourate favourate);
        Favourate FindFavourate(int Id);
        Task<Favourate> FindFavourateAsync(int Id);
    }
}
