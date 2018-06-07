using System;
using System.Linq;
using System.Threading.Tasks;
using Helpa.Entities;
using Helpa.Services.Interface;
using Helpa.Entities.Context;
using Helpa.Services.Repository;

namespace Helpa.Services
{
    public class FavourateService : IFavourateService
    {
        private HelpaContext context = null;
        private IRepository<Favourate> favourateContext = null;

        public FavourateService()
        {
            context = new HelpaContext();
            favourateContext = new Repository<Favourate>();
        }

        #region Favourates
        public IQueryable<Favourate> GetFavourates()
        {
            var result = favourateContext.GetAll(x => x.RowStatus != "D").IncludeEntities(x => x.Helper);
            return result;
        }

        public IQueryable<Favourate> GetFavourate(int Id)
        {
            var result = favourateContext.GetById(x => x.RowStatus != "D" && x.UserId == Id).IncludeEntities(x => x.Helper);
            return result;
        }

        public Favourate GetFavourateById(int Id)
        {
            var result = favourateContext.GetEntity(x => x.RowStatus != "D" && x.FavourateId == Id);
            return result;
        }

        public async Task<Favourate> GetFavourateAsync(int Id)
        {
            var result = await favourateContext.GetEntityAsync(x => x.RowStatus != "D" && x.FavourateId == Id);
            return result;
        }

        public Favourate AddFavourate(Favourate favourate)
        {
            favourate.CreatedDate = DateTime.Now;
            favourate.RowStatus = "I";
            favourate.Status = true;
            var result = favourateContext.Insert(favourate);
            return result;
        }

        public async Task<Favourate> AddFavourateAsync(Favourate favourate)
        {
            favourate.CreatedDate = DateTime.Now;
            favourate.RowStatus = "I";
            favourate.Status = true;
            var result = await favourateContext.InsertAsync(favourate);
            return result;
        }

        public int RemoveFavourate(Favourate favourate)
        {
            favourate.RowStatus = "D";
            favourate.Status = false;
            favourate.UpdatedDate = DateTime.UtcNow;
            var result = favourateContext.Update(favourate);
            return result;
        }

        public async Task<int> RemoveFavourateAsync(Favourate favourate)
        {
            favourate.RowStatus = "D";
            favourate.Status = false;
            favourate.UpdatedDate = DateTime.UtcNow;
            var result = await favourateContext.UpdateAsync(favourate);
            return result;
        }

        public Favourate FindFavourate(int Id)
        {
            var result = context.Favourates.Find(Id);
            return result;
        }

        public async Task<Favourate> FindFavourateAsync(int Id)
        {
            var result = await context.Favourates.FindAsync(Id);
            return result;
        }

        #endregion
    }
}
