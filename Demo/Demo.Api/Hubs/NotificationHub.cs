using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Helpa.Api.Models;
using Helpa.Entities.Context;

namespace Helpa.Api.Hubs
{
    public class NotificationHub : Hub
    {
        private static readonly ConcurrentDictionary<string, UserHubModels> Users =
            new ConcurrentDictionary<string, UserHubModels>(StringComparer.InvariantCultureIgnoreCase);

        private HelpaContext context = new HelpaContext();

        // Logged User Call
        public void GetNotification()
        {
            try
            {
                string loggedUser = Context.User.Identity.Name;

                // Get Total Notifications
                string totalNotifications = LoadNotificationData(loggedUser);

                // Send To
                if (Users.TryGetValue(loggedUser, out UserHubModels receiver))
                {
                    var cid = receiver.ConnectionIds.FirstOrDefault();
                    var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                    context.Clients.Client(cid).broadcastNotification(totalNotifications);
                }
            }
            catch(Exception ex)
            {
                ex.ToString();
            }
        }

        // Specified User Call
        public void SendNotification(string SentTo)
        {
            try
            {
                // Get Total Notification
                string totalNotifications = LoadNotificationData(SentTo);

                // Send To
                if (Users.TryGetValue(SentTo, out UserHubModels receiver))
                {
                    var cid = receiver.ConnectionIds.FirstOrDefault();
                    var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                    context.Clients.Client(cid).broadcastNotification(totalNotifications);
                }
            }
            catch(Exception ex)
            {
                ex.ToString();
            }
        }

        private string LoadNotificationData(string UserId)
        {
            int total = 0;
            var q = context.Notifications.Where(x => x.ReceiverId == Convert.ToInt32(UserId)).ToList();
            total = q.Count();
            return total.ToString();
        }

        public override Task OnConnected()
        {
            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            var user = Users.GetOrAdd(userName, _ => new UserHubModels
            {
                UserName = userName,
                ConnectionIds = new HashSet<string>()
            });

            lock (user.ConnectionIds)
            {
                user.ConnectionIds.Add(connectionId);
                if (user.ConnectionIds.Count == 1)
                {
                    Clients.Others.userConnected(userName);
                }
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            Users.TryGetValue(userName, out UserHubModels user);

            if (user != null)
            {
                lock (user.ConnectionIds)
                {
                    user.ConnectionIds.RemoveWhere(cid => cid.Equals(connectionId));
                    if (!user.ConnectionIds.Any())
                    {
                        Users.TryRemove(userName, out UserHubModels removedUser);
                        Clients.Others.userDisconnected(userName);
                    }
                }
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}