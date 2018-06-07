using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Helpa.Entities;
using Helpa.Entities.Context;
using Helpa.Entities.CustomEntities;
using Helpa.Api.Hubs;

namespace Helpa.Api.Controllers
{
    public class NotificationsController : ApiController
    {
        private HelpaContext db = new HelpaContext();
        private NotificationHub hub = new NotificationHub();

        // GET: api/Notifications
        public IQueryable<Notification> GetNotifications()
        {
            return db.Notifications;
        }

        // GET: api/Notifications/5
        [ResponseType(typeof(Notification))]
        public async Task<IHttpActionResult> GetNotification(int id)
        {
            Notification notification = await db.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            return Ok(notification);
        }

        // PUT: api/Notifications/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutNotification(int id, Notification notification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notification.NotificationId)
            {
                return BadRequest();
            }

            db.Entry(notification).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Notifications
        [ResponseType(typeof(Notification))]
        public async Task<IHttpActionResult> PostNotification(NotificationDTO notificationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Notification notification = new Notification
            {
                ReceiverId = notificationDTO.ReceiverId,
                SenderId = notificationDTO.SenderId,
                Message = notificationDTO.Message,
                CreatedDate = DateTime.UtcNow,
                NotificationState = "U",
                RowStatus = "I",
                Title = notificationDTO.Title
            };

            db.Configuration.ProxyCreationEnabled = false;
            db.Notifications.Add(notification);
            await db.SaveChangesAsync();
            
            hub.SendNotification(notificationDTO.ReceiverId.ToString());

            return Ok(notification.NotificationId);
        }

        // DELETE: api/Notifications/5
        [ResponseType(typeof(Notification))]
        public async Task<IHttpActionResult> DeleteNotification(int id)
        {
            Notification notification = await db.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            db.Notifications.Remove(notification);
            await db.SaveChangesAsync();

            return Ok(notification);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NotificationExists(int id)
        {
            return db.Notifications.Count(e => e.NotificationId == id) > 0;
        }
    }
}