using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace livemenu.SignalR
{
    public class NotificationHub : Hub
    {
        public void Notificate(string message)
        {
            Clients.All.Notificate(message);
        }

        public override Task OnConnected()
        {
            int kl = 3;

         //   var currentUser = _accountManager.GetCurrentUser();

            //var connectionUserAccount = new ConnectionUserAccount();
            //connectionUserAccount.Account = currentUser;

         //  _connectionMapping.Add(connectionUserAccount, Context.ConnectionId);

            return base.OnConnected();
        }
    }
}