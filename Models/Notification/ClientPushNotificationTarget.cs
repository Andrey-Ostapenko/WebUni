using System.Linq;
using BWP.Kernel.Notification;
using livemenu.Kernel.Authentication;
using livemenu.SignalR;
using Microsoft.AspNet.SignalR;
using livemenu.Kernel.Catalog;
using System;

namespace livemenu.Models.Notification
{
    public class ClientPushNotificationTarget : INotificationTarget
    {
        private readonly IConnectionMapping<ConnectionUserAccount> _connectionMapping;
        private readonly IAccountManager _accountManager;

        public ClientPushNotificationTarget(IConnectionMapping<ConnectionUserAccount> connectionMapping, IAccountManager accountManager)
        {
            _connectionMapping = connectionMapping;
            _accountManager = accountManager;
        }

        public void Notificate(NotificationMessage message, NotificationType type = NotificationType.Info, NotificationTargetType targeType = NotificationTargetType.Current, bool isPermanent = false)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            if (targeType == NotificationTargetType.Broadcast)
            {
                context.Clients.All.sendMessage(message.Text, (int)type, isPermanent);
            }
            else if (targeType == NotificationTargetType.Current)
            {
                var currentUser = _accountManager.GetCurrentUser();
                var connectionIDs = _connectionMapping.GetConnections(new ConnectionUserAccount() {Account = currentUser}).ToList();
                context.Clients.Clients(connectionIDs).sendMessage(message.Text, (int)type, isPermanent);
            }
        }

        public void notificateAboutSavedCatalogItem(NewCatalogTreeItemData newCatalogTreeItemData, NotificationTargetType targetType = NotificationTargetType.Current, bool isPermanent = false)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            if (targetType == NotificationTargetType.Broadcast)
            {
                context.Clients.All.updateCatalogTreeItem(newCatalogTreeItemData);
            }
            else if (targetType == NotificationTargetType.Current)
            {
                var currentUser = _accountManager.GetCurrentUser();
                var connectionIDs = _connectionMapping.GetConnections(new ConnectionUserAccount() { Account = currentUser }).ToList();
                context.Clients.Clients(connectionIDs).updateCatalogTreeItem(newCatalogTreeItemData);
            }
        }

        public void NotificateAboutProgress(int progressInPercent, NotificationTargetType targetType = NotificationTargetType.Current, bool useQueue = false)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            if (targetType == NotificationTargetType.Broadcast)
            {
                context.Clients.All.progress(progressInPercent);
            }
            else if (targetType == NotificationTargetType.Current)
            {
                var currentUser = _accountManager.GetCurrentUser();
                var connectionIDs = _connectionMapping.GetConnections(new ConnectionUserAccount() { Account = currentUser }).ToList();
                context.Clients.Clients(connectionIDs).progress(progressInPercent);
            }
        }
    }
}