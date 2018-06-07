using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpa.Entities.CustomEntities;

namespace Helpa.StoredProcedures.Services
{
    public class HelperService
    {
        public IQueryable<HomeHelpers> GetHomeHelpers(int radius, string latitude, string longitude)
        {
            using (var context = new StoredProcedure.HelpaEntities())
            {
                var result = context.GetHelpers(radius, latitude, longitude, null);
            }
            return null;
        }
    }
}
