using System;
using System.Linq;
using Helpa.Entities;
using Helpa.Entities.Context;
using Helpa.Services.Interface;
using Helpa.Services.Repository;
using System.Threading.Tasks;
using System.Data.Entity;
using Helpa.Entities.CustomEntities;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.Device.Location;

namespace Helpa.Services
{
    public class HelperServices : IHelperServices
    {
        private HelpaContext helpaContext = new HelpaContext();
        private IRepository<HelperService> context;
        private IRepository<Helper> helperContext;
        private IRepository<HelperServiceScope> scopeContext;
        private IRepository<AspNetUser> userContext;
        private IRepository<ReviewAndRating> reviewContext;

        public HelperServices()
        {
            context = new Repository<HelperService>();
            helperContext = new Repository<Helper>();
            scopeContext = new Repository<HelperServiceScope>();
            userContext = new Repository<AspNetUser>();
            reviewContext = new Repository<ReviewAndRating>();
        }

        #region User
        public async Task<bool> FindByUserIdAsync(int Id)
        {
            var result = await userContext.FindByIdAsync(Id);
            if (result != null)
                return true;

            return false;
        }

        public IQueryable<AspNetUser> GetAllUsers()
        {
            var result = userContext.GetAll(r => r.RowStatus == true);

            return result;
        }

        public IQueryable<AspNetUser> GetUserByEmail(string Email)
        {
            var result = userContext.GetAll(e => e.Email == Email).IncludeEntities(x => x.Location);
            return result;
        }

        public IQueryable<AspNetUser> GetUserByPhone(string MobileNumber)
        {
            var result = userContext.GetAll(e => e.PhoneNumber == MobileNumber).IncludeEntities(x => x.Location);
            return result;
        }
        #endregion

        #region Helper Service
        public IQueryable<HomeHelpers> GetHelpers(int radius = 0, double Latitude = 0, double Longitude = 0)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/ProfileImages/");

            var r = helpaContext.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("HELPER") && x.RowStatus == true).IncludeEntities(x => x.Location);

            var d = r.ToList();

            if (radius >= 0 && Latitude > 0 && Longitude > 0)
            {
                DbGeography center = DbGeography.FromText("Point(" + Longitude + " " + Latitude + ")");
                r = r.Where(x => x.Location.LocationGeography.Distance(center) * 0.00062 <= radius);
                d = r.ToList();
            }

            var h = helpaContext.HelperServices.IncludeEntities(x => x.Helper, x => x.Service);

            var result1 = r.GroupJoin(h, x => x.Id, y => y.Helper.HelperId, (x, y) => new { r = x, h = y })
                .SelectMany(x => x.h.DefaultIfEmpty(), (x, y) => new { x.r, h = y });

            var result = new List<HomeHelpers>();
            HomeHelpers helpers = null;

            foreach (var item in result1)
            {
                helpers = new HomeHelpers
                {
                    AverageRating = item.r.AverageRating.ToString(),
                    Description = item.r.Description,
                    Name = item.r.UserName,
                    Status = item.r.RowStatus.ToString()
                };

                if (item.r.LocationId != null)
                {
                    helpers.Latitude = item.r.Location.LocationGeography.Latitude.ToString();
                    helpers.Location = item.r.Location.LocationName;
                    helpers.Longitude = item.r.Location.LocationGeography.Longitude.ToString();
                }

                if (item.r.ProfileImage != null)
                {
                    helpers.ProfilePicture = path + item.r.ProfileImage;
                }

                if (item.h != null)
                {
                    helpers.Service = item.h.Service.ServiceName;
                }

                result.Add(helpers);
            }

            return result.AsQueryable();
        }

        public IQueryable<HelperService> GetAllHelpers()
        {
            var result = context.GetAll(r => r.RowStatus != "D")
                .IncludeEntities(x => x.Helper, x => x.Service);
            var k = result.ToList();
            return result;
        }

        public IQueryable<HomeHelpers> GetHelperByEmail(string Email)
        {
            var result = GetHelpers();
            return result;
        }

        public IQueryable<HomeHelpers> GetHelperByPhone(string MobileNo)
        {
            var result = GetHelpers();
            return result;
        }

        public IQueryable<HelperService> GetHelperById(int Id)
        {
            var result = context.GetById(x => x.HelperServiceId == Id && x.RowStatus != "D")
                                .IncludeEntities(x => x.Helper, x => x.Service);

            return result;
        }

        public async Task<HelperService> GetHelperAsync(int Id)
        {
            HelperService result = null;

            using (var context = new HelpaContext())
            {
                result = await (context.HelperServices.Where(x => x.HelperId == Id)
                                .IncludeEntities(x => x.Helper, x => x.Service).FirstOrDefaultAsync());
            }
            return result;
        }

        public async Task<HelperService> AddHelperServiceAsync(HelperService helperService)
        {
            var result = await context.InsertAsync(helperService);
            return result;
        }
        #endregion

        #region Helper
        public Helper GetHelper(int Id)
        {
            var result = helperContext.GetEntity(x => x.HelperId == Id && x.RowStatus != "D");
            return result;
        }

        public async Task<Helper> FindByHelperIdAsync(int Id)
        {
            var result = await helperContext.FindByIdAsync(Id);
            return result;
        }

        public async Task<Helper> AddHelperAsync(Helper helper)
        {
            var result = await helperContext.InsertAsync(helper);
            return result;
        }

        public int UpdateHelper(Helper helper)
        {
            helper.RowStatus = "U";
            helper.Status = true;
            helper.UpdatedDate = DateTime.UtcNow;
            var result = helperContext.Update(helper);
            return result;
        }

        public async Task<int> UpdateHelperAsync(Helper helper)
        {
            helper.RowStatus = "U";
            helper.Status = true;
            helper.UpdatedDate = DateTime.UtcNow;
            var result = await helperContext.UpdateAsync(helper);
            return result;
        }
        #endregion

        #region Helper Scope
        public HelperServiceScope AddHelperServiceScope(HelperServiceScope helperServiceScope)
        {
            var result = scopeContext.Insert(helperServiceScope);
            return result;
        }

        public async Task<HelperServiceScope> AddHelperServiceScopeAsync(HelperServiceScope helperServiceScope)
        {
            var result = await scopeContext.InsertAsync(helperServiceScope);
            return result;
        }
        #endregion

        #region Custom Results

        public List<object> GetClusteredHelpers(SearchParams searchParams)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/ProfileImages/");
            IQueryable<HelperLocation> h = null;
            DbGeography targetCircle = null;
            List<HelperLocation> LoactionList = new List<HelperLocation>();
            if (searchParams != null)
            {
                if (searchParams.Latitude > 0 && searchParams.Longitude > 0 && searchParams.Radius >= 0)
                {
                    DbGeography center = DbGeography.FromText("Point(" + searchParams.Latitude + " " + searchParams.Longitude + ")");
                    targetCircle = center.Buffer(searchParams.Radius);

                    h = helpaContext.HelperLocations
                        .IncludeEntities(x => x.Helper, x => x.Location, x => x.Helper.HelperServices).Include(x => x.Helper.HelperServices.Select(q => q.Service)).Include(q => q.Helper.HelperServices.Select(x => x.HelperServiceScopes.Select(l => l.Scope)));
                    var io = helpaContext.HelperLocations.Where(x => x.Location.LocationGeography.Latitude.ToString().Contains(targetCircle.Latitude.ToString()) && x.Location.LocationGeography.Longitude.ToString().Contains(targetCircle.Longitude.ToString())).ToList();
                    if (searchParams.ServiceId > 0)
                    {
                        h = h.Where(x => x.Helper.HelperServices.Any(m => m.ServiceId == searchParams.ServiceId));
                    }
                    if (searchParams.LocationTypeId > 0)
                    {
                        h = h.Where(x => x.Helper.LocationType == searchParams.LocationTypeId);
                    }
                    if (searchParams.ScopeId > 0)
                    {
                        h = h.Where(x => x.Helper.HelperServices.Any(m => m.HelperServiceScopes.Any(n => n.HelperScopeId == searchParams.ScopeId)));
                    }
                }
            }

            else
            {
                h = helpaContext.HelperLocations
                    .IncludeEntities(x => x.Helper, x => x.Location, x => x.Helper.HelperServices.Select(y => y.Service));
                var t = h.ToList();

                if (searchParams != null)
                {
                    if (searchParams.ServiceId > 0)
                    {
                        h = h.Where(x => x.Helper.HelperServices.Select(q => q.ServiceId).FirstOrDefault() == searchParams.ServiceId);
                    }

                    if (searchParams.LocationTypeId > 0)
                    {
                        h = h.Where(x => x.Helper.LocationType == searchParams.LocationTypeId);
                    }

                    if (searchParams.ScopeId > 0)
                    {
                        h = h.Where(x => x.Helper.HelperServices.Any(m => m.HelperServiceScopes.Any(n => n.HelperScopeId == searchParams.ScopeId)));
                    }
                }
            }

            var pl = h.ToList();

            foreach (var item in pl)
            {
                var lat = Convert.ToDouble(item.Location.LocationGeography.Latitude);
                var Longi = Convert.ToDouble(item.Location.LocationGeography.Longitude);
                var SearchLat= Convert.ToDouble(searchParams.Latitude);
                var SearchLongi = Convert.ToDouble(searchParams.Longitude);
                var sCoord = new GeoCoordinate(SearchLat, SearchLongi);
                var eCoord = new GeoCoordinate(lat, Longi);

              var DistanceRadius= sCoord.GetDistanceTo(eCoord);
              
                if (DistanceRadius<=searchParams.Radius)
                {
                    LoactionList.Add(item);
                }
            }
            var r = helpaContext.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("HELPER") && x.RowStatus == true).ToList();
            var final = LoactionList.Join(r, x => x.Helper.UserId, y => y.Id, (x, y) => new { q = x, w = y }).ToList();
            List<object> data = new List<object>();
            HelpersHomeDTO helpers = new HelpersHomeDTO();
            foreach (var item in final)
            {
                var w = item.w;
                var q = item.q;

                helpers = new HelpersHomeDTO();
                helpers.LocalityName = item.q.Location.Locality;
                string location = item.q.Location.LocationGeography.StartPoint.ToString();
                int startIndex = location.IndexOf("(");
                int endIndex = location.IndexOf(")");
                var LatLong = location.Substring(startIndex + 1, endIndex - startIndex - 1).Split(null);
                helpers.Latitude = Math.Round(Convert.ToDouble(LatLong[1]), 8).ToString();
                helpers.Longitude = Math.Round(Convert.ToDouble(LatLong[0]), 8).ToString();
                var EMPLOYEE = helpaContext.AspNetUsers.Where(x => x.LocationId == item.q.LocationId).ToList();
                helpers.NumberOfHelpersInLocality = final.Count();
                List<HelpersInLocalty> Obj = new List<HelpersInLocalty>();
                foreach (var v in final)
                {
                    HelpersInLocalty ob = new HelpersInLocalty();
                    ob.UserId = v.w.Id;
                    ob.Name = v.w.UserName;
                    ob.ProfilePicture = path + v.w.ProfileImage;
                    ob.Description = v.w.Description;
                    ob.AverageRating = v.w.AverageRating.ToString();
                    ob.RatingCount = v.w.AverageRating.ToString();
                    ob.Status = v.w.RowStatus.ToString();
                    ob.Latitude = helpaContext.Locations.Where(x => x.LocationId == v.w.LocationId).Select(x => x.LocationGeography.Latitude).FirstOrDefault();
                    ob.Longitude = helpaContext.Locations.Where(x => x.LocationId == v.w.LocationId).Select(x => x.LocationGeography.Longitude).FirstOrDefault();
                  
                    Obj.Add(ob);
                }
                helpers.HelpersInLocalties = Obj;
                data.Add(helpers);
                break;
            }

            if (data.Count > 0)
            {
                return data;
            }
            return null;
        }

        public IQueryable<AspNetUserLogin> AspNetUserLogins()
        {
            throw new NotImplementedException();
        }

       

        public int AddLogin(AspNetUserLogin AspNetUserLogin)
        {
            //helpaContext .Add<Author>(author);
            //context.SaveChanges();
            try
            {
                helpaContext.AspNetUserLogins.Add(AspNetUserLogin);
                helpaContext.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {

                throw;
            }
            

        }
        //public int AddRole(AspNetRole AspNetRole)
        //{
        //    //helpaContext .Add<Author>(author);
        //    //context.SaveChanges();
        //    helpaContext.AspNetRoles.Add(AspNetRole);
        //    helpaContext.SaveChanges();
        //    return 1;
        //}

        #endregion
    }
}
