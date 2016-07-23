using System.Linq;
using System.Threading.Tasks;
using livemenu.Kernel.Authentication;
using livemenu.Models.Auth;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.ServiceLocation;

namespace livemenu.SignalR
{
    [CustomAuthorize]
    public class HeartBeatHub : Hub
    {
        private ConnectionUserAccount GetConnectionUserAccount(CustomPrincipal principal, ref IConnectionMapping<ConnectionUserAccount> mapping)
        {
            var _connectionMapping = ServiceLocator.Current.GetInstance<IConnectionMapping<ConnectionUserAccount>>();
            mapping = _connectionMapping;

            var currentUser = principal;

            var account = new Account()
                {
                    ID = currentUser.UserId,
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    Login = currentUser.Login,
                    RightSubjectID = currentUser.RightSubjectID,
                };


            var connectionUserAccount = new ConnectionUserAccount { Account = account };
            return connectionUserAccount;
        }

        public void Connect()
        {
        }

    
        public override Task OnConnected()
        {
            var user = this.Context.User as CustomPrincipal;

            IConnectionMapping<ConnectionUserAccount> mapping = null;
            var connectionUserAccount = GetConnectionUserAccount(user, ref mapping);
            mapping.Add(connectionUserAccount, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var user = this.Context.User as CustomPrincipal;
            IConnectionMapping<ConnectionUserAccount> mapping = null;
            var _connectionMapping = ServiceLocator.Current.GetInstance<IConnectionMapping<ConnectionUserAccount>>();

            _connectionMapping.RemoveByConnectionId(Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }


        public override Task OnReconnected()
        {
            var user = this.Context.User as CustomPrincipal;
            IConnectionMapping<ConnectionUserAccount> mapping = null;
            var connectionUserAccount = GetConnectionUserAccount(user, ref mapping);

            if (!mapping.GetConnections(connectionUserAccount).Contains(Context.ConnectionId))
            {
                mapping.Add(connectionUserAccount, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }

    public class ConnectionUserAccount
    {
        public Account Account { get; set; }
    }
}